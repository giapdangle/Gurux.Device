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
using System.Xml;
using System.Windows.Forms;
using System.Globalization;
using System.ComponentModel;
using Gurux.Device.Editor;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Gurux.Device.Properties;

namespace Gurux.Device
{
    /// <summary>
    /// Category is the category, where GXProperty instance is located. Categories are used to group properties.
    /// </summary>
    [TypeConverter(typeof(GXObjectTypeConverter))]
    [GXDataIOSourceAttribute(true, GXDataIOSourceType.Category, GXCategory.AvailableTargets.All)]
    [DataContract()]
    [Serializable]
    public class GXCategory : GXSite, INotifyPropertyChanged
    {
        /// <summary>
        /// Enumerates what information the class offers for the DataIOSource to use.
        /// </summary>
        /// <seealso cref="GXTable.AvailableTargets">GXTable.AvailableTargets</seealso>
        /// <seealso cref="GXProperty.AvailableTargets">GXProperty.AvailableTargets</seealso>
        [Flags]
        public enum AvailableTargets : long
        {
            /// <summary>
            /// Everything is targetable.
            /// </summary>
            All = -1,
            /// <summary>
            /// Nothing is targetable.
            /// </summary>
            None = 0x0,
            /// <summary>
            /// Target is a name.
            /// </summary>
            Name = 0x1 //Name must always be number one.
        }

        GXPropertyCollection m_Properties = null;
        string m_CategoryName;

        /// <summary>
        /// Object Identifier.
        /// </summary>
        [ValueAccess(ValueAccessType.None, ValueAccessType.None)]
        [ReadOnly(true), DefaultValue(0)]
        [DataMember(Name = "ID", IsRequired = true)]
        public ulong ID
        {
            get;
            set;
        }

        /// <summary>
        /// Read and write statistics for the category.
        /// </summary>
        [ValueAccess(ValueAccessType.None, ValueAccessType.None)]
        [System.Xml.Serialization.XmlIgnore()]
        public GXCategoryStatistics Statistics
        {
            get;
            internal set;
        }

        /// <summary>
        /// DisplayName is a localized name for the categoy.
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
        /// Adds the category to the items list if it matches the search and passes it to GXProperties.
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
            foreach (GXProperty it in this.Properties)
            {
                it.FindByPropertyValue(name, value, items);
            }
        }

        /// <summary>
        /// Read this category.
        /// </summary>
        public void Read()
        {
            if (GXDeviceList.CanExecute(DisabledActions, Device.Status, true))
            {
                this.Device.Read(this);
            }
        }

        /// <summary>
        /// Write this category.
        /// </summary>
        public void Write()
        {
            if (GXDeviceList.CanExecute(DisabledActions, Device.Status, false))
            {
                this.Device.Write(this);
            }
        }

        /// <summary>
        /// Reset child GXProperties.
        /// </summary>
        public void Reset(ResetTypes type)
        {
            switch (type)
            {
                case ResetTypes.Values:
                    foreach (GXProperty prop in this.Properties)
                    {
                        prop.Reset(ResetTypes.Values);
                    }
                    break;
                case ResetTypes.Errors:
                    foreach (GXProperty prop in this.Properties)
                    {
                        prop.Reset(ResetTypes.Errors);
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Reset statistic values.
        /// </summary>
        public void ResetStatistic()
        {
            this.Statistics.Reset();
            foreach (GXProperty it in this.Properties)
            {
                it.Statistics.Reset();
            }
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
        /// Determines if the category is stored to the database.
        /// </summary>
        [Category("Appearance"), Description("Determines if the category is stored to the database."), System.ComponentModel.DefaultValue(false)]
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        [ValueAccess(ValueAccessType.Edit, ValueAccessType.Edit)]
        public virtual bool Nonstorable
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the GXCategory class.
        /// </summary>
        public GXCategory() :
            this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the GXCategory class.
        /// </summary>
        public GXCategory(string name)
        {
            if (name != null)
            {
                Name = name;
            }
            Properties = new GXPropertyCollection();
            Statistics = new GXCategoryStatistics();
        }

        /// <summary>
        /// Override this to made changes before category load.
        /// </summary>
        protected override void OnDeserializing(bool designMode)
        {
            Properties = new GXPropertyCollection();
            Statistics = new GXCategoryStatistics();
        }

        /// <summary>
        /// Retrieves or sets the name of the category.
        /// </summary>
        [Category("Design"), Description("Indicates the name used in code to identify the object.")]
        [DataMember()]
        [ValueAccess(ValueAccessType.Edit, ValueAccessType.None)]
        public string Name
        {
            get
            {
                if (Site != null)
                {
                    return Site.Name;
                }
                return m_CategoryName;
            }
            set
            {
                if (value.Length == 0)
                {
                    throw new Exception(Resources.TheNameOfTheCategoryCanTBeEmpty);
                }
                if (Site != null)
                {
                    Site.Name = value;
                }
                else
                {
                    if (m_CategoryName != value)
                    {
                        m_CategoryName = value;
                        NotifyUpdated(this, new GXCategoryEventArgs(this, CategoryStates.Updated));
                    }
                }

            }
        }

        /// <summary>
        /// Retrieves or sets a string that describes the category.
        /// </summary>
        [DefaultValue(null), Category("Design"), Description("Retrieves or sets a string that describes the category.")]
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        [ValueAccess(ValueAccessType.Edit, ValueAccessType.None)]
        public virtual string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Retrieves a string representation of the value of the instance in the GXCategory class.
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
        /// Retrieves the properties collection.
        /// </summary>
        [ValueAccess(ValueAccessType.None, ValueAccessType.None)]
        [DataMember(Name = "Properties", IsRequired = true, EmitDefaultValue = false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [ReadOnly(true)]
        public GXPropertyCollection Properties
        {
            get
            {
                return m_Properties;
            }
            internal set
            {
                m_Properties = value;
                if (m_Properties != null)
                {
                    m_Properties.Parent = this;
                }
            }
        }

        /// <summary>
        /// The parent GXCategory collection.
        /// </summary>
        [ValueAccess(ValueAccessType.None, ValueAccessType.None)]
        [System.Xml.Serialization.XmlIgnore()]
        public GXCategoryCollection Parent
        {
            get;
            internal set;
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
                return Parent.Parent as GXDevice;
            }
        }

        /// <summary>
        /// Checks if the properties in the category are valid. 
        /// </summary>
        /// <param name="designMode"></param>
        /// <param name="tasks">Collection of tasks.</param>
        public virtual void Validate(bool designMode, GXTaskCollection tasks)
        {
            if (string.IsNullOrEmpty(Name))
            {
                tasks.Add(new GXTask(this, Resources.Name, Resources.CategoryNameIsUnknown));
            }
            foreach (GXProperty it in Properties)
            {
                it.Validate(designMode, tasks);
            }
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

        internal virtual void NotifyUpdated(object sender, GXItemEventArgs e)
        {
            if (OnUpdated != null)
            {
                OnUpdated(sender, e);
            }
            if (this.Device != null)
            {
                this.Device.NotifyUpdated(sender, e);
            }
        }

        #region INotifyPropertyChanged Members

        void NotifyChange(string propertyName)
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