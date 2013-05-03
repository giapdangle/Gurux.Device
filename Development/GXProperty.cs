//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL$
//
// Version:         $Revision$,
//                  $Date$
//                  $Author$
//
// Copyright (c) Gurux Ltd
//
//---------------------------------------------------------------------------
//
//  DESCRIPTION
//
// This file is a part of Gurux Device Framework.
//
// Gurux Device Framework is Open Source software; you can redistribute it
// and/or modify it under the terms of the GNU General Public License 
// as published by the Free Software Foundation; version 2 of the License.
// Gurux Device Framework is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of 
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details.
//
// This code is licensed under the GNU General Public License v2. 
// Full text may be retrieved at http://www.gnu.org/licenses/gpl-2.0.txt
//---------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Xml;
using System.Windows.Forms;
using System.ComponentModel;
using Gurux.Device.Editor;
using System.Runtime.Serialization;
using System.IO;
using System.Collections.Generic;
using Gurux.Communication;

namespace Gurux.Device
{	
	/// <summary>
    /// The GXProperty component implements the GXProperty interface, which represents 
    /// a property of a physical device. GXProperty components can be queried from a 
    /// GXDevice object. 
	/// </summary>        
    [TypeConverter(typeof(GXObjectTypeConverter))]
	[GXDataIOSourceAttribute(true, GXDataIOSourceType.Property, GXProperty.AvailableTargets.All)]
    [DataContract()]
    [Serializable]
    public abstract class GXProperty : GXSite, INotifyPropertyChanged
	{
        Type m_ValueType;
        object m_Value;
        /// <summary>
        /// Template ID defines whitch property template is used. Use 0 if no template is used.
        /// </summary>				
        string m_Name;
		DisplayTypes m_DisplayType = DisplayTypes.None;

		/// <summary>
		/// Enumerates what information the class offers for the DataIOSource to use.
		/// </summary>
        /// <seealso cref="GXCategory.AvailableTargets">GXCategory.AvailableTargets</seealso>
        /// <seealso cref="GXTable.AvailableTargets">GXTable.AvailableTargets</seealso>
        public enum AvailableTargets : long
		{
			/// <summary>
			/// Everything is targetable.
			/// </summary>
            All = -1, 
			/// <summary>
			/// Nothing is targetable.
			/// </summary>
            None = 0,
            /// <summary>
            /// Target is a name.
            /// </summary>
			Name = 1,
            /// <summary>
            /// Target is a value.
            /// </summary>
			Value = 2,
            /// <summary>
            /// Target is last read property.
            /// </summary>
			LastRead = 4,
            /// <summary>
            /// Target is last written property.
            /// </summary>
			LastWrite = 0x8,
            /// <summary>
            /// Target is minimum value.
            /// </summary>
			MinimumValue = 0x10,
            /// <summary>
            /// Target is maximum value.
            /// </summary>
			MaximumValue = 0x20,
            /// <summary>
            /// Target is average value.
            /// </summary>
			AvarageValue = 0x40,
            /// <summary>
            /// Target is unit.
            /// </summary>
			Unit = 0x80,
            /// <summary>
            /// Target is parameter.
            /// </summary>
			Parameter = 0x100
		}

		/// <summary>
		/// Initializes a new instance of the GXProperty class.
		/// </summary>
		public GXProperty() :
            this(null)
		{
		}

        /// <summary>
        /// Initializes a new instance of the GXProperty class.
        /// </summary>
        public GXProperty(string name)
        {
            this.Values = new GXValueItemCollection();
            this.AccessMode = Gurux.Device.AccessMode.ReadWrite;
            Statistics = new GXPropertyStatistics();
            BitMask = false;            
            //Use device value for transaction delay.
            TransactionDelay = -1;
            if (name != null)
            {
                Name = name;
            }
        }
				
        /// <summary>
        /// Override this to made changes before property load.
        /// </summary>
        protected override void OnDeserializing(bool designMode)
        {			
			this.Values = new GXValueItemCollection();
			this.AccessMode = Gurux.Device.AccessMode.ReadWrite;
			Statistics = new GXPropertyStatistics();
			BitMask = false;			
			//Use device value for transaction delay.
			TransactionDelay = -1;			
        }
				
		/// <summary>
		/// Destructor. 
		/// </summary>
		~GXProperty()
		{			
		}       

        /// <summary>
        /// Determines which actions are blocked from use.
        /// </summary>
        [ValueAccess(ValueAccessType.None, ValueAccessType.Edit)]
        [DefaultValue(DisabledActions.None)]
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        [Browsable(false)]
        public DisabledActions DisabledActions
        {
            get;
            set;
        }

        /// <summary>
        /// Retrieves a string representation of the value of the instance in the GXProperty class.
        /// </summary>
        /// <returns>A string representation of the value of the instance.</returns>
		public override string ToString()
		{
            if (Name == null)
            {
                return "";
            }
            return Name;
		}

        /// <summary>
        /// Object Identifier.
        /// </summary>
        [ValueAccess(ValueAccessType.None, ValueAccessType.None)]
        [DataMember(Name = "ID", IsRequired = true)]
        [ReadOnly(true)]
        public ulong ID
        {
            get;
            set;
        }

        /// <summary>
        /// Retrieves or sets the name of the property.
        /// </summary>
        [Category("Design"), Description("Indicates the name used in code to identify the object.")]
        [Browsable(true), DataMember(IsRequired = false, EmitDefaultValue = false)]
        [GXTableColumn()]
        [ValueAccess(ValueAccessType.Edit, ValueAccessType.None)]
        public string Name
        {
            get
            {
                if (Site != null)
                {
                    return Site.Name;
                }
                return m_Name;
            }
            set
            {
                if (value == null || value.Length == 0)
                {
                    throw new Exception("Property name can't be empty.");
                }
                if (Site != null)
                {
                    Site.Name = value;
                }
                else
                {
                    m_Name = value;
                }
            }
        }

		/// <summary>
		/// Adds the GXProperty to the items list if it matches the search.
		/// </summary>
		public void FindByPropertyValue(string name, object value, List<object> items)
        {
            PropertyDescriptorCollection Props = TypeDescriptor.GetProperties(this, true);
            foreach (PropertyDescriptor it in Props)
            {
                if (it.Name == name)
                {
					if (object.Equals(it.GetValue(this), value))
					{
						items.Add(this);
					}
                    break;
                }
            }
        }

		/// <summary>
		/// Determines if the property is stored to the database.
		/// </summary>
        [Category("Appearance"), Description("Determines if the property is stored to the database."), System.ComponentModel.DefaultValue(false)]
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        [ValueAccess(ValueAccessType.Edit, ValueAccessType.Edit)]
        public virtual bool Nonstorable
		{
			get;
			set;
		}

        /// <summary>
		/// DisplayType property informs how to present numeric values. If DisplayType is not found
		/// in AvailableDisplayTypes, property does not support the type and it is not set. The attempt 
		/// to set an unavailable display type as DisplayType is ignored, and the type stays as is.
		/// </summary>
		/// <remarks>Display type of all items in GXDeviceList must be changed at the same time.</remarks>
		/// <seealso href="E_Gurux_Device_GXPropertyCollection_OnUpdated.htm">GXPropertyCollection.PropertyChanged</seealso>
		/// <seealso href="E_Gurux_Device_GXDevice_OnDisplayTypeChanged.htm">GXDevice.OnDisplayTypeChanged</seealso>
		/// <seealso href="E_Gurux_Device_GXDeviceList_OnDisplayTypeChanged.htm">GXDeviceList.OnDisplayTypeChanged</seealso>
        [System.Xml.Serialization.XmlIgnore()]
        [ValueAccess(ValueAccessType.None, ValueAccessType.None)]
        public DisplayTypes DisplayType
        {
			get
			{
				return m_DisplayType;
			}
			internal set
			{
				if (m_DisplayType != value)
				{
					m_DisplayType = value;
					NotifyDisplayTypeChanged(value);
				}
			}
        }

		/// <summary>
		/// Parent GXTable. Null if the GXProperty is not part of a GXTable.
		/// </summary>
        [ValueAccess(ValueAccessType.None, ValueAccessType.None)]
        [System.Xml.Serialization.XmlIgnore()]
        public GXTable Table
        {
            get
            {
                if (this.Parent == null)
                {
                    return null;
                }
                return this.Parent.Parent as GXTable;
            }
        }

		/// <summary>
		/// Parent GXCategory. Null if the GXProperty is not part of a GXCategory.
		/// </summary>
        [ValueAccess(ValueAccessType.None, ValueAccessType.None)]
        [System.Xml.Serialization.XmlIgnore()]
        public GXCategory Category
        {
            get
            {
                if (this.Parent == null)
                {
                    return null;
                }
                return this.Parent.Parent as GXCategory;
            }
        }

		/// <summary>
		/// Checks if the parameter values of the property are valid.
		/// </summary>
        public virtual void Validate(bool designMode, GXTaskCollection tasks)
		{
            if (string.IsNullOrEmpty(Name))
            {
                tasks.Add(new GXTask(this, "Name", "Property Name is unknown."));
            }
        }
	
		/// <summary>
		/// The default value of the property. Value is given as UI value.
		/// </summary>
		[System.ComponentModel.Category("Behavior"), System.ComponentModel.DefaultValue(null),
		TypeConverter(typeof(StringConverter)),
		System.ComponentModel.Description("The default value of the property. Value is given as UI value.")]
        [GXUserLevelAttribute(UserLevelType.Experienced)]
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public virtual object DefaultValue
		{
			get;
			set;
		}

        /// <summary>
        /// The maximum value property. The value is given as an UI value.
        /// </summary>
        [System.ComponentModel.Category("Behavior"), System.ComponentModel.DefaultValue(null),
        TypeConverter(typeof(StringConverter)),
        System.ComponentModel.Description("The maximum value of the property . The value is given as a UI value.")]
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        [GXUserLevelAttribute(UserLevelType.Experienced)]
        public virtual object Maximum
        {
            get;
            set;
        }

        /// <summary>
        /// The minimum value of the property. Value is given as UI value.
        /// </summary>
        [System.ComponentModel.Category("Behavior"), System.ComponentModel.DefaultValue(null),
        TypeConverter(typeof(StringConverter)),
        System.ComponentModel.Description("The minimum value of the property. Value is given as UI value.")]
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        [GXUserLevelAttribute(UserLevelType.Experienced)]
        public virtual object Minimum
        {
            get;
            set;
        }
	
		/// <summary>
		/// The access mode.
		/// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The access mode."),
		DefaultValue(Gurux.Device.AccessMode.ReadWrite)]
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        [ValueAccess(ValueAccessType.Edit, ValueAccessType.None)]
        public virtual Gurux.Device.AccessMode AccessMode
		{
			get;
			set;
		}		

		/// <summary>
		/// Retrieves or sets a string that describes the property.
		/// </summary>
		[Editor(typeof(UITextEditor), typeof(System.Drawing.Design.UITypeEditor)),
        DefaultValue(null), System.ComponentModel.Category("Design"),
        System.ComponentModel.Description("Retrieves or sets a string that describes the property.")]
        [ValueAccess(ValueAccessType.Edit, ValueAccessType.None)]
        [DataMember(Name = "Description", IsRequired = false, EmitDefaultValue = true)]        
        public virtual string Description
		{
            get;
            set;
		}

		/// <summary>
		/// The status of the property.
		/// </summary>
        [ValueAccess(ValueAccessType.None, ValueAccessType.None)]
        [System.Xml.Serialization.XmlIgnore()]
        public Gurux.Device.PropertyStates Status
		{
			get;
			set;
		}

		enum TransactionDelayEnum
		{
			UseDeviceValue = -1
		}

		/// <summary>
		/// TransactionDelay is the minimum transaction delay time, in milliseconds, between transactions.
		/// </summary>
		[System.ComponentModel.Category("Behavior"), System.ComponentModel.Description("TransactionDelay is the minimum transaction delay time, in milliseconds, between transactions."),
		TypeConverter(typeof(GXNumberEnumeratorConverter)), GXNumberEnumeratorConverterAttribute(typeof(TransactionDelayEnum)),
		DefaultValue(-1)]
		[GXUserLevelAttribute(UserLevelType.Experienced)]
        [ValueAccess(ValueAccessType.None, ValueAccessType.None)]
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public int TransactionDelay
		{
			get;
			set;
		}        

		/// <summary>
        /// The UI data type of the property, described in the template file.
		/// </summary>
        /// <remarks>
        /// UIDataType handles serialization.
        /// </remarks>
        [System.ComponentModel.Category("Design"), DefaultValue(null),
        System.ComponentModel.Description("The UI data type of the property, described in the template file.")]
        [ValueAccess(ValueAccessType.None, ValueAccessType.None)]
        [System.Xml.Serialization.XmlIgnore()]
        virtual public Type ValueType
		{
            get
            {
                return m_ValueType;
            }
            set
            {
                if (m_ValueType != value)
                {
                    m_ValueType = value;
                    NotifyChange("ValueType");
                }
            }
		}

        /// <summary>
        /// Save Value type as string.
        /// </summary>
        /// <remarks>
        /// Rerserved for inner use.
        /// </remarks>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElement("ValueType")]
		[DataMember(Name = "ValueType", IsRequired = false, EmitDefaultValue = false)]
		public string ValueTypeName
        {
            get
            {
                return ValueType == null ? null : ValueType.AssemblyQualifiedName;
            }
            set
            {
                ValueType = string.IsNullOrEmpty(value) ? null : Type.GetType(value);
            }
        }


		/// <summary>
		/// The unit type of the property, described in the template file.
		/// </summary>
		[DefaultValue(null), System.ComponentModel.Category("Appearance"),
        System.ComponentModel.Description("The unit type of the property, described in the template file.")]
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public virtual string Unit
		{
			get;
			set;
		}	
	
		/// <summary>
		/// The properties collection that owns the property.
		/// </summary>
        [ValueAccess(ValueAccessType.None, ValueAccessType.None)]
        [System.Xml.Serialization.XmlIgnore()]
		public GXPropertyCollection Parent
		{
			get;
            internal set;
		}

		/// <summary>
		/// Force usage of preset GXValueItems.
		/// </summary>
        [Category("Values")]
        [DataMember(Name = "Force", IsRequired = false, EmitDefaultValue = false)]
        [ValueAccess(ValueAccessType.Edit, ValueAccessType.None)]
        [Browsable(false), DefaultValue(false), GXUserLevelAttribute(UserLevelType.Experienced)]
        public virtual bool ForcePresetValues
        {
            get;
            set;
        }

        /// <summary>
        /// Can user modify value collection.
        /// </summary>
        /// <remarks>
        /// Default value is true.
        /// </remarks>
        [Category("Values")]
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(true), ValueAccess(ValueAccessType.None, ValueAccessType.None)]        
        public bool Modify
        {
            get;
            set;
        }

        /// <summary>
        /// Determines if the value of the property is shown as Bit Mask.
        /// </summary>		
        [ValueAccess(ValueAccessType.Edit, ValueAccessType.None)]
        [DefaultValue(false), Category("Values"), Description("Determines if the property is shown as Bit Mask.")]
        [GXUserLevelAttribute(UserLevelType.Experienced)]
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        [Browsable(false)]
        public virtual bool BitMask
        {
            get;
            set;
        }

		/// <summary>
		/// Collection of GXValueItems for this GXProperty.
		/// </summary>
        [Category("Values")]
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        [ValueAccess(ValueAccessType.Edit, ValueAccessType.None)]
        [Editor(typeof(GXValueCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Browsable(false), ReadOnly(true)]
        public virtual GXValueItemCollection Values
        {
            get;
            set;
        }

        /// <summary>
        /// DisplayName is a localized name for the GXProperty.
        /// </summary>
        [ValueAccess(ValueAccessType.None, ValueAccessType.None)]
        public string DisplayName
        {
            get
            {
                return this.Name;
            }
        }

		/// <summary>
		/// The parent GXDevice.
		/// </summary>
        [ValueAccess(ValueAccessType.None, ValueAccessType.None)]
        [System.Xml.Serialization.XmlIgnore()]
        public GXDevice Device
        {
            get
            {
                if (Parent == null)
                {
                    return null;
                }
                GXCategory cat = Parent.Parent as GXCategory;
                if (cat != null)
                {
                    return cat.Device;
                }
                GXTable table = Parent.Parent as GXTable;
                if (table != null)
                {
                    return table.Device;
                }
                throw new Exception("Invalid parent.");
            }
        }

		/// <summary>
		/// Creates a clone of this GXProperty.
		/// </summary>
        public GXProperty Clone()
        {
            List<Type> knownTypes = new List<Type>();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.Encoding = System.Text.Encoding.UTF8;
            settings.CloseOutput = true;
            settings.CheckCharacters = false;
            GXProperty prop = null;
            knownTypes.Add(this.GetType());
            knownTypes.Add(typeof(DisabledActions));
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (StreamReader reader = new StreamReader(memoryStream))
                {
                    DataContractSerializer x = new DataContractSerializer(this.GetType(), knownTypes);
                    x.WriteObject(memoryStream, this);
                    memoryStream.Position = 0;
                    prop = x.ReadObject(memoryStream) as GXProperty;
                }
            }
            prop.ID = GXDeviceList.IDGenerator.GenerateTemplateID();
            return prop;
        }


        /// <summary>
        /// GetValue returns the value of a GXProperty instance.
        /// </summary>
        /// <remarks>
        /// If Type is none, property value is returned as is. Note: UIValue formula is however executed, 
        /// if UseUIValue parameter is set to True. If Type is String (GX_VT_STR), property value is returned 
        /// in type determined by GXProperty.DisplayType Property. For example, if display type is Hex, value 
        /// is returned in hexadecimal, and if it is bit, returned as bit string. Else property value is returned 
        /// in type set with Type parameter.
        /// </remarks>  
        /// <example>
        /// <code>
        /// Dim value
        /// value = GXProperty1.GetValue(GX_VT_INT32, False)
        /// </code>
        /// </example>
        /// <param name="useUIValue">If True, UIValue formulas are used.</param>
        public object GetValue(bool useUIValue)
        {
            object value = m_Value;
            if (value == null)
            {
                value = DefaultValue;
            }
            if (useUIValue)
            {
                if (value != null && Values.Count != 0)
                {
                    GXValueItem item = Values.FindByUIValue(value, this.ForcePresetValues);
                    if (item != null)
                    {
                        value = item.UIValue;
                    }
                }
                return value;
            }
            if (value != null && Values.Count != 0)
            {
                return Values.UIValueToDeviceValue(value, this.ForcePresetValues);                
            }
            return this.Device.UIValueToDeviceValue(this, value);
        }

		/// <summary>
		/// Returns the value of the GXProperty as hex.
		/// </summary>
        public virtual string GetValueAsHex(bool useUIValue)
        {
			object value = GetValue(useUIValue);
			try
			{
				double tryValue = 0;
				if (value == null)
				{
					return string.Empty;
				}
				else if (value is string)
				{
					return BitConverter.ToString(System.Text.ASCIIEncoding.ASCII.GetBytes(value as string));
				}
				else if (value is byte[])
				{
					return BitConverter.ToString(value as byte[]);
				}
				else if (double.TryParse(value.ToString(), out tryValue))
				{
					return BitConverter.ToString(BitConverter.GetBytes(tryValue));
				}
				else
				{
					//This case should be handled in the protocol addin overload.
					return string.Empty;
				}
			}
			catch
			{
				return Convert.ToString(value);
			}
        }
        
		/// <summary>
		/// SetValue sets the value of a GXProperty. If ValueItems are used, the value must be in the ValueItems collection
		/// and the Type must be String.
		/// </summary>
		/// <example>
		/// <code>
		/// Dim value = "SomeValue"
		/// GXProperty1.SetValue value, False
		/// </code>
		/// </example>
		/// <param name="value">Property value</param>
		/// <param name="uiValue">Is given vaue UI value or device value.</param>
		/// <param name="status">A status to be set for the GXProperty after the value has been set.</param>
		/// <seealso href="T_Gurux_Device_GXValue.htm">GXValue</seealso>
        public void SetValue(object value, bool uiValue, PropertyStates status)
        {
            bool change = false;
            if (!uiValue)
            {
                bool bFound = false;
                foreach (GXValueItem it in this.Values)
                {
                    if (string.Compare(Convert.ToString(it.DeviceValue), Convert.ToString(value), true) == 0)
                    {
                        bFound = true;
                        value = it.UIValue;
                        break;
                    }
                }
                if (!bFound)
                {
                    value = this.Device.DeviceValueToUIValue(this, value);
                }
            }
            //If value collection is used.
            else if (this.Values.Count != 0)
            {
                if (ForcePresetValues)
                {
                    bool bFound = false;
                    foreach (GXValueItem it in this.Values)
                    {
                        if (string.Compare(Convert.ToString(it.UIValue), Convert.ToString(value), true) == 0)
                        {
                            bFound = true;
                            break;
                        }
                    }
                    if (!bFound)
                    {
                        throw new Exception("Invalid value. Forceed preset value not found.");
                    }
                }
                
            }
            else if (ValueType != null)
            {
                value = Convert.ChangeType(value, ValueType);
            }
            if (m_Value != value)
            {
                change = true;
            }
            m_Value = value;
            if (change)
            {
                status |= Gurux.Device.PropertyStates.ValueChanged;
            }
            NotifyPropertyChange(status);
        }

		/// <summary>
		/// Set value for the GXProperty as hex.
		/// </summary>
        public void SetValueAsHex(string value, bool useUIValue, PropertyStates status)
        {
            //TODO:
            NotifyPropertyChange(status);
        }               		

		/// <summary>
		/// Read the GXProperty.
		/// </summary>
        public void Read()
        {            
            if (GXDeviceList.CanExecute(DisabledActions, Device.Status, true))
            {
                this.Device.Read(this);
            }
        }

		/// <summary>
		/// Write the GXProperty.
		/// </summary>
		public void Write()
        {
            if (GXDeviceList.CanExecute(DisabledActions, Device.Status, false))
            {
                this.Device.Write(this);
            }
        }

		/// <summary>
		/// Reset the GXProperty.
		/// </summary>
        public void Reset(ResetTypes type)
        {
			switch (type)
			{
				case ResetTypes.Values:
					this.SetValue(this.DefaultValue, false, PropertyStates.ValueChangedByUser);
					break;
				case ResetTypes.Errors:
					this.Status &= ~PropertyStates.Error;
					break;
				default:
					break;
			}
        }

		/// <summary>
		/// Statistics for the GXProperty.
		/// </summary>
        [ValueAccess(ValueAccessType.None, ValueAccessType.None)]
        [System.Xml.Serialization.XmlIgnore()]
        public GXPropertyStatistics Statistics
        {
            get;
            internal set;
        }

        /// <summary>
		/// ReadTime is the time, when GXProperty was last read.
		/// </summary>
		/// <remarks>
		/// GXDevice updates ReadTime automatically.
		/// </remarks>
		/// <seealso href="E_Gurux_Device_GXPropertyCollection_OnUpdated.htm">GXPropertyCollection.PropertyChanged</seealso>
        [ValueAccess(ValueAccessType.None, ValueAccessType.None)]
        [System.Xml.Serialization.XmlIgnore()]
        public DateTime ReadTime
        {
            get;
            set;
        }       
        
        /// <summary>
		/// WriteTime is the time, when GXProperty was last written.
		/// </summary>
		/// <remarks>
		/// GXDevice updates WriteTime automatically.
		/// </remarks>
		/// <seealso href="E_Gurux_Device_GXPropertyCollection_OnUpdated.htm">GXPropertyCollection.PropertyChanged</seealso>
        [ValueAccess(ValueAccessType.None, ValueAccessType.None)]
        [System.Xml.Serialization.XmlIgnore()]
        public DateTime WriteTime
        {
            get;
            set;
        }

        /// <summary>
        /// Initialize default settings.
        /// </summary>
        public virtual void Initialize(object info)
        {

        }

		/// <summary>
		/// Notifies, when an item has changed.
		/// </summary>
        public event ItemUpdatedEventHandler OnUpdated;

        /// <summary>
        /// NotifyPropertyChange informs software of changes in the GXProperty.
        /// </summary>
        /// <param name="status">The type of the event to be raised.</param>
        public void NotifyPropertyChange(PropertyStates status)
        {
            NotifyUpdated(new GXPropertyEventArgs(this, status));
        }

        internal void NotifyUpdated(GXPropertyEventArgs e)
        {
            if (OnUpdated != null)
            {
                OnUpdated(this, e);
            }
            if (Parent != null)
            {
                Parent.NotifyUpdated(this, e);
            }
        }

		internal virtual void NotifyDisplayTypeChanged(DisplayTypes type)
		{
			if (OnDisplayTypeChanged != null)
			{
				OnDisplayTypeChanged(type);
			}
		}

		/// <summary>
		/// Notifies, when the display type has changed.
		/// </summary>
		public event DisplayTypeChangedEventHandler OnDisplayTypeChanged;

        #region INotifyPropertyChanged Members

		/// <summary>
		/// Notifies a change in the GXProperty.
		/// </summary>
        protected void NotifyChange(string propertyName)
        {
            if (m_OnPropertyChanged != null)
            {
                m_OnPropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        PropertyChangedEventHandler m_OnPropertyChanged;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                m_OnPropertyChanged += value;
            }
            remove
            {
                m_OnPropertyChanged -= value;
            }
        }

        #endregion
    }
}