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
using System.Linq;
using System.Xml;
using System.Windows.Forms;
using System.ComponentModel;
using Gurux.Device.Editor;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Gurux.Device
{
	/// <summary>
    /// A table that contains a collection of GXPropertyCollection. Each GXProperty acts as a column, so each row has the same 
    /// properties with possibly different values. Row indexing is zero-based. 
    /// </summary>
    [TypeConverter(typeof(GXObjectTypeConverter))]
    [GXDataIOSourceAttribute(true, GXDataIOSourceType.Table, GXTable.AvailableTargets.All)]
    [DataContract()]
    [Serializable]
    public class GXTable : GXSite, INotifyPropertyChanged, IGXPartialRead
	{
		private List<object[]> m_DeviceValues = null;
		private List<object[]> m_UIValues = null;

		/// <summary>
        /// Enumerates what information the class offers for the DataIOSource to use.
		/// </summary>
        /// <seealso cref="GXCategory.AvailableTargets">GXCategory.AvailableTargets</seealso>
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
        
        string m_Name;

        [DataMember(Name = "SortedProperty", IsRequired = false, EmitDefaultValue = false)]
        ulong m_SortedPropertyID = 0;

		/// <summary>
		/// Gets or sets the plugin form that is displayed in the GXDirector.
		/// </summary>
        [System.Xml.Serialization.XmlIgnore()]
		public object PluginForm = null;

		/// <summary>
		/// Initializes a new instance of the GXTable class.
		/// </summary>
		public GXTable()
		{            
            AccessMode = Gurux.Device.AccessMode.Read;
            Columns = new GXPropertyCollection();
            Statistics = new GXTableStatistics();
			m_DeviceValues = new List<object[]>();
			m_UIValues = new List<object[]>();
		}

        /// <summary>
        /// Override this to made changes before property load.
        /// </summary>
        protected override void OnDeserializing(bool designMode)
		{			
			AccessMode = Gurux.Device.AccessMode.ReadWrite;
			Columns = new GXPropertyCollection();
			Statistics = new GXTableStatistics();
			m_DeviceValues = new List<object[]>();
			m_UIValues = new List<object[]>();
        }

		/// <summary>
		/// Sets the sorting column.
		/// </summary>
        protected override void OnDeserialized(bool designMode)
        {
            if (m_SortedPropertyID != 0)
            {
                SortedProperty = this.Columns.FindItemByID(m_SortedPropertyID) as GXProperty;
            }
        }

		/// <summary>
		/// Ensures that the sorting column is selected.
		/// </summary>
        protected override void OnSerializing(bool designMode)
        {
            m_SortedPropertyID = 0;
            if (SortedProperty != null)
            {
                m_SortedPropertyID = SortedProperty.ID;
            }
        }

		/// <summary>
		/// Adds the category to the items list and passes it to GXProperties.
		/// </summary>
        public void FindByPropertyValue(string name, object value, List<object> items)
        {
            PropertyDescriptorCollection Props = TypeDescriptor.GetProperties(this, true);
            foreach (PropertyDescriptor it in Props)
            {
                if (it.Name == name)
                {
                    items.Add(this);
                    break;
                }
            }
        }

		/// <summary>
		/// Statistics for GXTable.
		/// </summary>
        [ValueAccess(ValueAccessType.None, ValueAccessType.None)]
        [System.Xml.Serialization.XmlIgnore()]
        public GXTableStatistics Statistics
        {
            get;
            internal set;
        }

		/// <summary>
		/// Resets Statistics.
		/// </summary>
        public void ResetStatistic()
        {
            this.Statistics.Reset();
        }

        /// <summary>
        /// Get value of a single cell from the table.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <param name="useUIValue"></param>
        /// <returns></returns>
		public object GetCell(int column, int row, bool useUIValue)
        {
			if (useUIValue)
			{
				return m_UIValues[row][column];
			}
			else
			{
				return m_DeviceValues[row][column];
			}
        }

        /// <summary>
		/// Set value of a single cell in the table.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <param name="value"></param>
        /// <param name="useUIValue"></param>
		public void SetCell(int column, int row, object value, bool useUIValue)
        {
			if (useUIValue)
			{
				m_UIValues[row][column] = value;
                m_DeviceValues[row][column] = this.Device.UIValueToDeviceValue(this.Columns[column], value);
			}
			else
			{
				m_DeviceValues[row][column] = value;
                m_UIValues[row][column] = this.Device.DeviceValueToUIValue(this.Columns[column], value);
			}
        }

		/// <summary>
		/// Current row count in the GXTable.
		/// </summary>
        [ValueAccess(ValueAccessType.None, ValueAccessType.None)]
        [Browsable(false)]
		public int RowCount
		{
            get
            {
                return m_DeviceValues.Count;
            }
		}       

        /// <summary>
        /// Property according to which rows are sorted.
        /// </summary>
        /// <remarks>
        /// Usually this is a date time.
        /// </remarks>        
        [ValueAccess(ValueAccessType.Edit, ValueAccessType.None)]
        [TypeConverter(typeof(GXTypeConverterNoExpand))]
        [Editor(typeof(GXTableColumnEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Category("Data")]
        virtual public GXProperty SortedProperty
        {
            get;
            set;
        }

        /// <summary>
        /// Is Sorted Property Unique.
        /// </summary>
        /// <remarks>
        /// Default is false.
        /// </remarks>
        [DefaultValue(false)]
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        [ValueAccess(ValueAccessType.Edit, ValueAccessType.None)]
        [Category("Data")]
        virtual public bool IsSortedPropertyUnique
        {
            get;
            set;
        }

        /// <summary>
        /// Are old values cleared before new read starts.
        /// </summary>
        /// <remarks>
        /// Default is false.
        /// </remarks>
        [DefaultValue(false)]
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        [ValueAccess(ValueAccessType.Edit, ValueAccessType.Edit)]
        [Category("Data")]
        virtual public bool ClearPreviousValues
        {
            get;
            set;
        }


        /// <summary>
        /// Get a collection of rows from the table.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <param name="useUIValue"></param>
        /// <returns></returns>
        public List<object[]> GetRows(int start, int count, bool useUIValue)
        {
            if (count == -1)
            {
                count = m_UIValues.Count;
            }
			List<object[]> retVal = new List<object[]>();
			for (int i = start; i < start + count; ++i)
			{
				if (useUIValue)
				{
					retVal.Add(m_UIValues[i].ToArray());
				}
				else
				{
					retVal.Add(m_DeviceValues[i].ToArray());
				}
			}

			return retVal;
        }

        /// <summary>
        /// Set a collection of rows in to the table.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="rows"></param>
        /// <param name="useUIValue"></param>
		public void SetRows(int start, List<object[]> rows, bool useUIValue)
        {
			for (int rowIndex = start; rowIndex < start + rows.Count; ++rowIndex)
			{
				List<object> tmpValues = new List<object>();
				if (useUIValue)
				{
					m_UIValues[rowIndex] = rows[rowIndex-start];
					for (int colIndex = 0; colIndex < Columns.Count; ++colIndex)
					{
						object value = rows[rowIndex - start][colIndex];
						tmpValues.Add(this.Device.UIValueToDeviceValue(this.Columns[colIndex], value));
					}
					m_DeviceValues[rowIndex] = tmpValues.ToArray();
				}
				else
				{
					m_DeviceValues[rowIndex] = rows[rowIndex - start];
					for (int colIndex = 0; colIndex < Columns.Count; ++colIndex)
					{
						object value = rows[rowIndex - start][colIndex];
						tmpValues.Add(this.Device.DeviceValueToUIValue(this.Columns[colIndex], value));
					}
					m_DeviceValues[rowIndex] = tmpValues.ToArray();
				}
			}
        }

        /// <summary>
        /// Add new rows.
        /// </summary>
		/// <param name="index"></param>
        /// <param name="rows"></param>
        /// <param name="isUIValue"></param>
		public void AddRows(int index, List<object[]> rows, bool isUIValue)
        {
			List<object[]> inputList = new List<object[]>();
			List<object[]> convertedList = new List<object[]>();
			foreach (object[] arr in rows)
			{
				inputList.Add(arr);

				List<object> tmpValues = new List<object>();
				for (int colIndex = 0; colIndex < Columns.Count; ++colIndex)
				{
					object value = arr[colIndex];
                    if (isUIValue)
					{
                        tmpValues.Add(this.Device.UIValueToDeviceValue(this.Columns[colIndex], value));
					}
					else
					{
                        tmpValues.Add(this.Device.DeviceValueToUIValue(this.Columns[colIndex], value));
					}
				}
				convertedList.Add(tmpValues.ToArray());

			}
            if (isUIValue)
			{
				m_UIValues.InsertRange(index, inputList);
				m_DeviceValues.InsertRange(index, convertedList);
			}
			else
			{
				m_DeviceValues.InsertRange(index, inputList);
				m_UIValues.InsertRange(index, convertedList);
			}
			this.NotifyTableChange(Gurux.Device.TableStates.RowsAdded, index, convertedList);

        }

        /// <summary>
        /// Clear rows.
        /// </summary>
        public void ClearRows()
        {
			this.NotifyTableChange(Gurux.Device.TableStates.RowsClear, 0, null);
			m_DeviceValues.Clear();
			m_UIValues.Clear();
        }


        /// <summary>
        /// Remove rows.
        /// </summary>
		public void RemoveRows(int index, int count)
        {
			this.NotifyTableChange(Gurux.Device.TableStates.RowsRemoved, index, m_DeviceValues.GetRange(index, count));
			m_DeviceValues.RemoveRange(index, count);
			m_UIValues.RemoveRange(index, count);
		}

        /// <summary>
        /// Object Identifier.
        /// </summary>
        [ValueAccess(ValueAccessType.None, ValueAccessType.None)]
        [DefaultValue(0)]
        [DataMember(Name = "ID", IsRequired = true)]
        [ReadOnly(true)]
        public ulong ID
        {
            get;
            set;
        }
        
		/// <summary>
		/// Parent GXTable collection.
		/// </summary>
        [ValueAccess(ValueAccessType.None, ValueAccessType.None)]
        [ReadOnly(true)]
        [System.Xml.Serialization.XmlIgnore()]
        public GXTableCollection Parent
        {
            get;
            internal set;
        }
        
        /// <summary>
        /// Determines which actions are blocked from use.
        /// </summary>
        [ValueAccess(ValueAccessType.None, ValueAccessType.Edit)]
        [Browsable(false)]
        [DefaultValue(DisabledActions.None)]
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public DisabledActions DisabledActions
        {
            get;
            set;
        }

		/// <summary>
		/// Parent GXDevice.
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
        /// The access mode.
        /// </summary>        
        [Browsable(false), System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The access mode."),
        DefaultValue(Gurux.Device.AccessMode.Read)]
        [ValueAccess(ValueAccessType.Edit, ValueAccessType.None)]
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public virtual Gurux.Device.AccessMode AccessMode
        {
            get;
            set;
        }

		/// <summary>
		/// Read the GXTable.
		/// </summary>
        public void Read()
        {
            if (GXDeviceList.CanExecute(DisabledActions, Device.Status, true))
            {
                this.Device.Read(this);
            }
        }

		/// <summary>
		/// Write the GXTable.
		/// </summary>
        public void Write()
        {
            if (GXDeviceList.CanExecute(DisabledActions, Device.Status, false))
            {
                this.Device.Write(this);
            }
        }

		/// <summary>
		/// Reset the GXTable.
		/// </summary>
        public void Reset(ResetTypes type)
        {

        }

        GXPropertyCollection m_Columns;

		/// <summary>
		/// Collection of GXProperties representing the columns of the GXTable.
		/// </summary>
        [ValueAccess(ValueAccessType.None, ValueAccessType.None)]
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor(typeof(System.ComponentModel.Design.CollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [ReadOnly(true)]
        public GXPropertyCollection Columns
        {
            get
            {
                return m_Columns;
            }
            set
            {
                m_Columns = value;
                if (m_Columns != null)
                {
                    m_Columns.Parent = this;
                }
            }
        }       

        /// <summary>
        /// DisplayName is a localized name for the table.
        /// </summary>
        [ValueAccess(ValueAccessType.None, ValueAccessType.None)]
        public string DisplayName
        {
            get
            {
                return Name;
            }
        }

		/// <summary>
		/// Determines if the table is stored to a database.
		/// </summary>
        [Category("Appearance"), Description("Determines if the table is stored to a database."), System.ComponentModel.DefaultValue(false)]
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        [ValueAccess(ValueAccessType.Edit, ValueAccessType.Edit)]
        public virtual bool Nonstorable
		{
			get;
			set;
		}		
			
        /// <summary>
        /// Retrieves a string representation of the value of the instance in the GXTable class.
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
		/// Retrieves or sets the name of the table.
		/// </summary>	
        [ValueAccess(ValueAccessType.Edit, ValueAccessType.None)]
        [Category("Design"), Description("Indicates the name used in code to identify the object.")]
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
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
					throw new Exception("Table name can't be empty.");
				}
				if (Site != null)
				{
					Site.Name = value;
				}
				else
				{
                    if (m_Name != value)
                    {
					    m_Name = value;
                        NotifyUpdated(this, new GXTableEventArgs(this, TableStates.Updated, -1, null));
                    }
				}
			}
		}

		/// <summary>
		/// Retrieves or sets a string that describes the table.
		/// </summary>        
        [DefaultValue(null)]
        [Category("Design"), Description("Retrieves or sets a string that describes the table.")]
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        [ValueAccess(ValueAccessType.Edit, ValueAccessType.None)]
        public virtual string Description
		{
			get;
			set;
		}

		/// <summary>
        /// Checks if the properties in the table are valid.
		/// </summary>
		/// <param name="designMode"></param>
		/// <param name="tasks">Collection of tasks.</param>
        public virtual void Validate(bool designMode, GXTaskCollection tasks)
		{
            if (string.IsNullOrEmpty(Name))
            {
                tasks.Add(new GXTask(this, "Name", "Table name is unknown."));
            }
            foreach (GXProperty prop in Columns)
            {
                prop.Validate(designMode, tasks);
            }
		}

        /// <summary>
        /// Initialize default settings.
        /// </summary>
        public virtual void Initialize()
        {

        }

        /// <summary>
        /// NotifyPropertyChange informs software of changes in the GXProperty.
        /// </summary>
        /// <param name="status">The type of the event to be raised.</param>        
		/// <param name="index">Index of the changed item.</param>
		/// <param name="rows">Rows of the current GXTable</param>
        public void NotifyTableChange(TableStates status, int index, List<object[]> rows)
        {
            NotifyUpdated(this, new GXTableEventArgs(this, status, index, rows));
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

		/// <summary>
		/// Notifies a changed property.
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

        #region IGXPartialRead Members

        PartialReadType IGXPartialRead.Type
        {
            get;
            set;
        }

        object IGXPartialRead.Start
        {
            get;
            set;
        }

        object IGXPartialRead.End
        {
            get;
            set;
        }

		/// <summary>
		/// Returns set start and end time for the GXTable.
		/// </summary>
        void IGXPartialRead.GetStartEndTime(out DateTime starttm, out DateTime endtm)
        {
            IGXPartialRead tmp = this as IGXPartialRead;
            switch (tmp.Type)
            {
                case Gurux.Device.Editor.PartialReadType.New:
                    if (tmp.Start == null)
                    {
                        starttm = DateTime.MinValue;
                    }
                    else
                    {
                        starttm = Convert.ToDateTime(tmp.Start);
                    }
                    endtm = DateTime.MaxValue;
                    break;
                case Gurux.Device.Editor.PartialReadType.All:
                    starttm = DateTime.MinValue;
                    endtm = DateTime.MaxValue;
                    break;
                case Gurux.Device.Editor.PartialReadType.Last:
                    starttm = Convert.ToDateTime(tmp.Start);
                    endtm = Convert.ToDateTime(tmp.End);
                    break;
                case Gurux.Device.Editor.PartialReadType.Entry:
                    throw new ArgumentOutOfRangeException("Entry read type is invalid.");                    
                case Gurux.Device.Editor.PartialReadType.Range:
                    if (tmp.Start == null)
                    {
                        starttm = DateTime.MinValue;
                    }
                    else
                    {
                        starttm = Convert.ToDateTime(tmp.Start);
                    }
                    if (tmp.End == null)
                    {
                        endtm = DateTime.MaxValue;
                    }
                    else
                    {
                        endtm = Convert.ToDateTime(tmp.End);
                    }
                    break;
                default:
                    throw new Exception("Invalid start type.");
            }
        }

        #endregion
    }
}
