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
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml;
using System.Runtime.Serialization;
using System.Reflection;
using Gurux.Device.Editor;
using System.IO;
using Quartz;
using Quartz.Impl;
using Gurux.Common;
using Gurux.Device.PresetDevices;
using Gurux.Device.Properties;

namespace Gurux.Device
{    
	/// <summary>
	/// The top component in GXDevice hierarcy. Contains methods, properties and events for controlling the GXDevice hierarchy.
	/// </summary>
    [DataContract()]
    [GXDataIOSourceAttribute(true, GXDataIOSourceType.DeviceList, GXDeviceList.AvailableTargets.All)]
    public class GXDeviceList : GXSite, IDisposable, INotifyPropertyChanged
    {
        [DataMember(Name = "Name", IsRequired = false, EmitDefaultValue = false)]
        string m_Name;
        [DataMember(Name = "CustomUI", IsRequired = false, EmitDefaultValue = false)]
        byte[] m_CustomUI;
        internal static bool IsLoading = false;
        internal static bool IsSaving = false;
        internal ISchedulerFactory m_SchedulerFactory;
        internal IScheduler m_sched;

        int FreezeEvents = 0;
        internal static GXIDGenerator IDGenerator = new GXIDGenerator();

        Dictionary<Type, object> m_Services = new Dictionary<Type, object>();
        GXDeviceGroupCollection m_DeviceGroups;
        GXScheduleCollection m_Schedules;
		Gurux.Device.Editor.DisplayTypes m_DisplayType = DisplayTypes.None;

        static Dictionary<string, GXProtocolAddIn> m_Protocols;
        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDeviceList()
        {            
            DeviceGroups = new GXDeviceGroupCollection();
            this.Schedules = new GXScheduleCollection();
        }

        /// <summary>
        /// Override this to made changes before device list load.
        /// </summary>
        protected override void OnDeserializing(bool designMode)
        {
			DeviceGroups = new GXDeviceGroupCollection();
			this.Schedules = new GXScheduleCollection();
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~GXDeviceList()
        {
            Dispose();
        }

        /// <summary>
        /// Enumerates what information the class offers for the DataIOSource to use.
        /// </summary>
        /// <seealso cref="GXTable.AvailableTargets">GXTable.AvailableTargets</seealso>
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
			/// Only the name is targetable.
			/// </summary>
			Name = 0x1,
		}

        /// <summary>
        /// Dispose must call after device list use. 
        /// Otherwice schedules will keel devicelist up and it will newer go down.
        /// </summary>
        public override void Dispose()
        {
            try
            {                
                ++FreezeEvents;
                Clear();
                this.CloseSchedules();                
            }
            finally
            {
                FreezeEvents = 0;
            }
        }

		internal void StartSchedules()
		{
			if (m_SchedulerFactory == null)
			{
				m_SchedulerFactory = new StdSchedulerFactory();
				m_sched = m_SchedulerFactory.GetScheduler();
                m_sched.ListenerManager.RemoveJobListener(typeof(GXScheduleListener).Name);                
				// Set up the listener
                m_sched.ListenerManager.AddJobListener(new GXScheduleListener());
				m_sched.Start();				
			}
		}

        /// <summary>
        /// Closes schedules.
        /// </summary>
        public void CloseSchedules()
        {
            //m_sched is null when we load devicelist.
            if (m_sched != null)
            {
                m_sched.Shutdown(true);
            }
        }

		/// <summary>
		/// The root node for GXDeviceGroup collection hierarchy. Contains GXDeviceGroups that can contain GXDevices and child GXDeviceGroups.
		/// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public GXDeviceGroupCollection DeviceGroups
        {
            get
            {
                return m_DeviceGroups;
            }
            internal set
            {
                m_DeviceGroups = value;
                if (m_DeviceGroups != null)
                {
                    m_DeviceGroups.Parent = this;
                }
            }
        }

		/// <summary>
		/// Update protocols and preset devices.
		/// </summary>
        static public void Update()
        {
            Update(GXCommon.ProtocolAddInsPath);
        }

		/// <summary>
		/// Update protocols and preset devices.
		/// </summary>
        static public void Update(string path)
        {
            m_Protocols = FindProtocols(path);
            if (m_PresetDevices == null)
            {
                m_PresetDevices = new GXDeviceManufacturerCollection();
            }
            GXDeviceManufacturerCollection.Load(m_PresetDevices);
        }

        static GXDeviceManufacturerCollection m_PresetDevices;
        /// <summary>
        /// Contains list of available GXProtocolAddIns.
        /// </summary>
        [Browsable(false)]
        public static GXDeviceManufacturerCollection PresetDevices
        {
            get
            {
                if (m_PresetDevices == null)
                {
                    m_PresetDevices = new GXDeviceManufacturerCollection();
                    GXDeviceManufacturerCollection.Load(m_PresetDevices);
                }
                return m_PresetDevices;
            }
        }


		/// <summary>
		/// Contains list of available GXProtocolAddIns.
		/// </summary>
        [Browsable(false)]
        static public Dictionary<string, GXProtocolAddIn> Protocols
        {
            get
            {
                if (m_Protocols == null)
                {
                    m_Protocols = FindProtocols();
                }
                return m_Protocols;
            }
        }

		/// <summary>
		/// The name of the GXDeviceList.
		/// </summary>
        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
                NotifyUpdated(this, new GXItemEventArgs(this));
            }
        }

        /// <summary>
        /// UI Components
        /// </summary>
        public byte[] CustomUI
        {
            get
            {
                return m_CustomUI;
            }
            set
            {
                if (m_CustomUI != value)
                {
                    m_CustomUI = value;
                    NotifyUpdated(this, new GXItemEventArgs(this));
                }
            }
        }
      
        /// <summary>
        /// Device list file name.
        /// </summary>
        public string FileName
        {
            get;
            set;
        }

		/// <summary>
		/// Returns all user changed GXTables and GXProperties.
		/// </summary>
		public List<object> GetUserChangedItems()
		{
			//TODO: Implement to support writing.
			return new List<object>();
		}

		/// <summary>
		/// Clears everything in the GXDeviceList including schedules and device groups.
		/// </summary>
        public void Clear()
        {
            try
            {
                ++FreezeEvents;
                Disconnect();
				this.CustomUI = null;
                this.Name = this.FileName = "";
                if (Schedules != null)
                {
                    this.Schedules.Clear();
                }
                if (this.DeviceGroups != null)
                {
                    this.DeviceGroups.Clear();
                }
            }
            finally
            {
                --FreezeEvents;
                NotifyClear(this, new GXItemEventArgs(this));
            }
        }
		
		/// <summary>
		/// The DisplayType of the GXDeviceList.
		/// </summary>
        public Gurux.Device.Editor.DisplayTypes DisplayType
        {
			get
			{
				return m_DisplayType;
			}
			set
			{
				if (m_DisplayType != value)
				{
					m_DisplayType = value;
					OnDisplayTypeChanged(value);
					foreach(GXDevice dev in this.DeviceGroups.GetDevicesRecursive())
					{
						dev.NotifyDisplayTypeChanged(value);						
						foreach (var cat in dev.Categories)
						{
							cat.Properties.NotifyDisplayTypeChanged(value);
							foreach (var prop in cat.Properties)
							{
								prop.DisplayType = value;
							}
						}
						foreach (var table in dev.Tables)
						{
							table.Columns.NotifyDisplayTypeChanged(value);
							foreach (var prop in table.Columns)
							{
								prop.DisplayType = value;
							}
						}
						foreach (var cat in dev.Events.Categories)
						{
							cat.Properties.NotifyDisplayTypeChanged(value);
							foreach (var prop in cat.Properties)
							{
								prop.DisplayType = value;
							}
						}
						foreach (var table in dev.Events.Tables)
						{
							table.Columns.NotifyDisplayTypeChanged(value);
							foreach (var prop in table.Columns)
							{
								prop.DisplayType = value;
							}
						}
					}
				}
			}
        }

		System.Globalization.CultureInfo m_LocaleIdentifier;
		/// <summary>
		/// CultureInfo of the GXDeviceList.
		/// </summary>
        public System.Globalization.CultureInfo LocaleIdentifier
        {
			get
			{
				return m_LocaleIdentifier;
			}
			set
			{
				if (m_LocaleIdentifier != value)
				{
					m_LocaleIdentifier = value;
					if (OnLocaleIdentifierChanged != null)
					{
						OnLocaleIdentifierChanged(value);
					}
				}
			}
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
		/// List of GXSchedules in the GXDeviceList.
		/// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public GXScheduleCollection Schedules
        {
            get
            {
                return m_Schedules;
            }
            internal set
            {
                m_Schedules = value;
                if (m_Schedules != null)
                {
                    m_Schedules.Parent = this;
                }
            }
        }

        object m_SelectedItem;
		/// <summary>
		/// Selected object in the GXDeviceList.
		/// </summary>
        public object SelectedItem
        {
            get
            {
                return m_SelectedItem;
            }
            set
            {
                GXSelectedItemEventArgs e = new GXSelectedItemEventArgs(value, m_SelectedItem);
                NotifySelectedItemChanging(e);
                if (e.CanChange)
                {
                    m_SelectedItem = value;
                    NotifySelectedItemChanged(e);
                }
            }
        }

		/// <summary>
		/// Add a service to the GXDeviceList.
		/// </summary>
        public void AddService(Type type, object service)
        {
            m_Services[type] = service;
        }

		/// <summary>
		/// Get a service from the GXDeviceList.
		/// </summary>
        public object GetService(Type type)
        {
            return m_Services[type];
        }

		/// <summary>
		/// Connect all the child GXDevices.
		/// </summary>
        public void Connect()
        {
            this.DeviceGroups.Connect();

		}

		/// <summary>
		/// Disconnect all the child GXDevices.
		/// </summary>
        public void Disconnect()
        {
            if (this.DeviceGroups != null)
            {
                this.DeviceGroups.Disconnect();
            }
        }

		/// <summary>
		/// Start monitoring all the child GXDevices.
		/// </summary>
        public void StartMonitoring()
        {
            this.DeviceGroups.StartMonitoring();
        }

        static internal bool CanExecute(DisabledActions disabledActions, DeviceStates status, bool read)
        {
            //If monitoring.
            if ((status & DeviceStates.Monitoring) != 0)
            {
                return (disabledActions & DisabledActions.Monitor) == 0;
            }
            //If scheduling.
            if ((status & DeviceStates.Scheduling) != 0)
            {
                return (disabledActions & DisabledActions.Schedule) == 0;
            }
            //Can read.
            if (read)
            {
                return (disabledActions & DisabledActions.Read) == 0;
            }
            //Can write.
            return (disabledActions & DisabledActions.Write) == 0;
        }

		/// <summary>
		/// Stop monitoring all the child GXDevices.
		/// </summary>
        public void StopMonitoring()
        {
            this.DeviceGroups.StopMonitoring();
        }

		/// <summary>
		/// Read all the child GXDevices.
		/// </summary>
        public void Read()
        {
            this.DeviceGroups.Read();
        }

		/// <summary>
		/// Write all the child GXDevices.
		/// </summary>
        public void Write()
        {
            this.DeviceGroups.Write();
        }

		/// <summary>
		/// Reset all the child GXDevices.
		/// </summary>
        public void Reset(ResetTypes type)
        {
            this.DeviceGroups.Reset(type);
        }

        static GXMediaTypeCollection m_MediaTypes;

		/// <summary>
		/// Get available media types.
		/// </summary>
        public static GXMediaTypeCollection GetMediaTypes()
        {
            if (m_MediaTypes == null)
            {
                m_MediaTypes = new GXMediaTypeCollection();
				string[] medias = Gurux.Communication.GXClient.GetAvailableMedias();
                foreach (string it in medias)
                {
                    m_MediaTypes.Add(new GXMediaType(it, ""));
                }
            }
            return m_MediaTypes;
        }

        static string GetProfilePath()
        {
            string path = GXCommon.ApplicationDataPath;
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                path = System.IO.Path.Combine(path, ".Gurux");
            }
            else
            {
                path = System.IO.Path.Combine(path, "Gurux");
            }
            path = System.IO.Path.Combine(path, "Devices");
            path = System.IO.Path.Combine(path, "profiles.xml");
            return path;
        }

        internal static void SaveDeviceProfiles(GXDeviceProfileCollection list)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.Encoding = System.Text.Encoding.UTF8;
            settings.CloseOutput = true;
            settings.CheckCharacters = false;
            DataContractSerializer x = new DataContractSerializer(typeof(GXDeviceProfileCollection));
            using (XmlWriter writer = XmlWriter.Create(GetProfilePath(), settings))
            {
                x.WriteObject(writer, list);
            }
        }

        internal static GXDeviceProfileCollection LoadDeviceProfiles()
        {
            GXDeviceProfileCollection items = new GXDeviceProfileCollection(null);
            string TemplatePath = GetProfilePath();
            bool profilesFileExists = System.IO.File.Exists(TemplatePath);
            bool oldWay = !profilesFileExists && System.IO.File.Exists(Path.GetDirectoryName(TemplatePath) + "Templates.xml");
            //if old way.
            if (oldWay)
            {
                Gurux.Device.Editor.GXTemplateManager m = new Gurux.Device.Editor.GXTemplateManager();
                foreach (Gurux.Device.Editor.GXTemplateManager.GXTemplateManagerItem it in m.AvailableTemplates(null).List)
                {
                    GXDeviceProfile item = new GXDeviceProfile();
                    item.Protocol = it.Parent.Protocol;
                    item.Name = it.Name;
                    GXDevice device;
                    //Try to load file. Remove it if not exists.
                    try
                    {
                        device = GXDevice.Load(it.Path);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    device.Save(device.ProfilePath);
                    item.DeviceGuid = device.Guid;
                    GXDevice newDevice = GXDevice.Load(device.ProfilePath);
                    PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(device);
                    foreach (PropertyDescriptor p in properties)
                    {
                        if (!p.IsReadOnly)
                        {
                            object value = p.GetValue(device);
                            object value2 = p.GetValue(newDevice);
                            if (value == null && value2 == null)
                            {
                            }
                            else if (!value.Equals(value2))
                            {
                                System.Diagnostics.Debug.WriteLine("Wrong value " + p.Name + " " + value.ToString() + " " + value2.ToString());
                            }
                        }
                    }
                    items.Add(item);
                };
            }
            else if (profilesFileExists)
            {
                DataContractSerializer x = new DataContractSerializer(typeof(GXDeviceProfileCollection));
                using (FileStream reader = new FileStream(TemplatePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    try
                    {
                        items = x.ReadObject(reader) as GXDeviceProfileCollection;
                    }
                    catch (Exception Ex)
                    {
                        System.Diagnostics.Debug.WriteLine(Ex.Message);
                    }
                }
            }
            return items;
        }

        /// <summary>
        /// DeviceTypes is the collection of device types that are registered to the computer.
        /// </summary>
        /// <remarks>
        /// With the DeviceTypes collection you can easily figure out, what device type templates are registered to the computer.
        /// With GXPublisher you can easily see registered device types. If the name of the protocol is empty,
        /// all the protocols are returned, otherwise only the ones under it.
        /// </remarks>
        /// <param name="preset">Are preset device get.</param>
        /// <param name="protocol">The name of the protocol.</param>
        /// <returns>Collection of device types.</returns> 
        static public GXDeviceProfileCollection GetDeviceTypes(bool preset, string protocol)
        {
            GXDeviceProfileCollection items = null;
            if (preset)
            {
                items = new GXDeviceProfileCollection(null);
                foreach (GXDeviceManufacturer man in GXDeviceList.PresetDevices)
                {
                    foreach (GXDeviceModel model in man.Models)
                    {
                        foreach (GXDeviceVersion version in model.Versions)
                        {
                            foreach (GXDeviceProfile type in version.Templates)
                            {
                                items.Add(type);
                            }
                        }
                    }
                }
            }
            else
            {
                items = LoadDeviceProfiles();               
            }
            return items;
        }       

        /// <summary>
        /// CanChangeSelectedItem asks device list clients, if it is OK to change to a new item.
        /// </summary>
        /// <param name="item">Selected item</param>
        /// <returns>True, if selected item can be changed.</returns>
        public bool CanChangeSelectedItem(object item)
        {
            GXSelectedItemEventArgs e = new GXSelectedItemEventArgs(item, m_SelectedItem);
            NotifySelectedItemChanging(e);
            return e.CanChange;
        }

		/// <summary>
		/// Clear this instance of GXDeviceList and set a name for it.
		/// </summary>
        public void CreateEmpty(string name)
        {
            Clear();
            this.Name = name;
            this.Dirty = false;
        }

        void NotifySerialized(bool serialized, GXDeviceGroupCollection groups)
        {
            GXSite site;
            foreach (GXDeviceGroup group in groups)
            {
                foreach (GXDevice device in group.Devices)
                {
                    foreach (GXCategory cat in device.Categories)
                    {
                        foreach (GXProperty prop in cat.Properties)
                        {
                            site = prop as GXSite;
                            site.NotifySerialized(serialized);
                        }
                        site = cat as GXSite;
                        site.NotifySerialized(serialized);
                    }
                    foreach (GXTable table in device.Tables)
                    {
                        foreach (GXProperty prop in table.Columns)
                        {
                            site = prop as GXSite;
                            site.NotifySerialized(serialized);
                        }
                        site = table as GXSite;
                        site.NotifySerialized(serialized);
                    }
                }
                NotifySerialized(serialized, group.DeviceGroups);
                site = group as GXSite;
                site.NotifySerialized(serialized);
            }
        }

        /// <summary>
        /// Load device list from xml file.
        /// </summary>
        /// <param name="path"></param>
        public void Load(string path)
        {
            try
            {
                NotifyLoadBegin();
                IsLoading = true;
                ++FreezeEvents;
                Clear();
                Type deviceType = null;
                bool isJson = true;
                using (TextReader reader = File.OpenText(path))
                {
                    //Valid XML starts always with '<'
                    isJson = reader.Peek() != '<';
                }
                GXSite site;
                System.Collections.Generic.List<Type> types = new System.Collections.Generic.List<Type>();
                if (System.Environment.OSVersion.Platform != PlatformID.Unix)
                {
                    foreach (GXProtocolAddIn it in GXDeviceList.Protocols.Values)
                    {
                        GetAddInInfo(it, out deviceType, types);
                    }
                }
                using (FileStream reader = new FileStream(path, FileMode.Open))
                {
                    DataContractSerializer x = new DataContractSerializer(this.GetType(), types.ToArray());
                    GXDeviceList list = (GXDeviceList)x.ReadObject(reader);
                    list.m_sched = null;
                    this.Name = list.Name;
                    this.CustomUI = list.CustomUI;
                    //Move items to other collection.
                    this.DeviceGroups.AddRange(list.DeviceGroups);
                    foreach (GXDeviceGroup it in this.DeviceGroups)
                    {
                        it.Parent = this.DeviceGroups;
                    }
                    list.DeviceGroups.Clear();
                    //Move items to other collection.
                    this.Schedules.AddRange(list.Schedules);
                    foreach (GXSchedule it in this.Schedules)
                    {
                        it.Parent = this.Schedules;
                        site = it as GXSite;
                        site.NotifySerialized(false);
                    }
                    list.Schedules.Clear();
                    this.DisabledActions = list.DisabledActions;
                    reader.Close();
                }
                NotifySerialized(false, this.DeviceGroups);
                site = this as GXSite;
                site.NotifySerialized(false);
                foreach (GXSchedule schedule in this.Schedules)
                {
                    string[] items = null;
                    if (schedule.SerializedItems != null)
                    {
                        items = schedule.SerializedItems.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string it in items)
                        {
                            ulong id = ulong.Parse(it);
                            object target = FindItemByID(id);
                            schedule.Items.Add(target);
                        }
                    }
                    if (schedule.SerializedExcludedItems != null)
                    {
                        items = schedule.SerializedExcludedItems.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string it in items)
                        {
                            ulong id = ulong.Parse(it);
                            object target = FindItemByID(id);
                            schedule.ExcludedItems.Add(target);
                        }
                    }
                }
                this.FileName = path;                
                this.Dirty = false;
            }
            finally
            {
                IsLoading = false;
                --FreezeEvents;
                NotifyLoadEnd();
            }
        }

        internal static void GetAddInInfo(GXProtocolAddIn addIn, out Type deviceType, System.Collections.Generic.List<Type> types)
        {
            deviceType = addIn.GetDeviceType();
			types.Add(typeof(Gurux.Communication.ChecksumType));
            types.Add(typeof(PartialReadType));
			types.Add(typeof(PartialReadType));
            types.Add(deviceType);
            types.AddRange(addIn.GetCategoryTypes(null));
			types.AddRange(addIn.GetTableTypes(null));
			types.AddRange(addIn.GetPropertyTypes(null));
			Type[] tmp = addIn.GetExtraTypes(null);
            if (tmp != null && tmp.Length != 0)
            {
                types.AddRange(tmp);
            }
        }

		/// <summary>
		/// Serialize the GXDevicelist to a XML file.
		/// </summary>
        public void Save(string filePath)
        {
            try
            {          
                Type deviceType = null;
                System.Collections.Generic.List<Type> types = new System.Collections.Generic.List<Type>();
				if (System.Environment.OSVersion.Platform != PlatformID.Unix)
				{
	                foreach (GXProtocolAddIn it in GXDeviceList.Protocols.Values)
	                {
	                    GetAddInInfo(it, out deviceType, types);
	                }
				}
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.Encoding = System.Text.Encoding.UTF8;
                settings.CloseOutput = true;
                settings.CheckCharacters = false;
                IsSaving = true;
                using (XmlWriter writer = XmlWriter.Create(filePath, settings))
                {
                    DataContractSerializer x = new DataContractSerializer(this.GetType(), types.ToArray());
                    x.WriteObject(writer, this);
                    writer.Close();
                }
                GXSite site;
                foreach (GXSchedule schedule in this.Schedules)
                {
                    site = schedule as GXSite;
                    site.NotifySerialized(true);
                }
                NotifySerialized(true, this.DeviceGroups);
                this.Dirty = false;
                this.FileName = filePath;
            }
            finally
            {
                IsSaving = false;
            }
        }

        /// <summary>
        /// Load Device Template to own namespace.
        /// </summary>
        class GXProxyClass : MarshalByRefObject
        {
            public List<string> Assemblies = new List<string>();
            public List<string> Protocols = new List<string>();            
            
            string TargetDirectory;
            public void ImportProtocolAddIns(string path, string target, List<string> foundProtocols)
            {
                TargetDirectory = path;
                try
                {
                    AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
                    Assembly asm = null;
                    try
                    {
                        asm = Assembly.LoadFile(target);
                    }
                    catch
                    {
                        return;
                    }
                    Dictionary<string, GXProtocolAddIn> protocols = new Dictionary<string, GXProtocolAddIn>();
                    FindProtocolAddIns(protocols, asm);
                    foreach (var it in protocols)
                    {
                        if (!foundProtocols.Contains(it.Key))
                        {
                            Protocols.Add(it.Value.GetType().Assembly.Location);
                        }
                        else
                        {
                            Assemblies.Clear();
                        }
                    }
                }
                finally
                {
                    AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve);
                }
            }

            System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
            {
                foreach (string it in Directory.GetFiles(TargetDirectory, "*.dll"))
                {
                    Assembly asm;
                    try
                    {
                        asm = Assembly.LoadFile(it);
                    }
                    catch
                    {
                        continue;
                    }
                    if (asm.GetName().ToString() == args.Name)
                    {
                        Assemblies.Add(it);
                        return asm;
                    }
                }
                //Search local dlls.
                //This must be done because Unit tests.
                foreach (string it in Directory.GetFiles(Path.GetDirectoryName(this.GetType().Assembly.Location), "*.dll"))
                {
                    Assembly asm = Assembly.LoadFile(it);
                    if (asm.GetName().ToString() == args.Name)
                    {
                        //Do not add item to the Assemblies list.
                        return asm;
                    }
                }
                return null;
            }
        }

        static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            //Find loaded AddIns.
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (asm.GetName().ToString() == args.Name)
                {                    
                    return asm;
                }
            }
            return null;
        }

        static void FindAddins(string strPath, Dictionary<string, GXProtocolAddIn> protocols)
        {
            // Iterate through all dlls in this path
            DirectoryInfo di = new DirectoryInfo(strPath);
            //If directory not exists.
            if (!di.Exists)
            {
                return;
            }
            List<string> foundProtocols = new List<string>(protocols.Keys);
            //If application domain is already cfeated.
            if (!string.IsNullOrEmpty(AppDomain.CurrentDomain.RelativeSearchPath))
            {
                GXProxyClass pc = new GXProxyClass();                
                foreach (FileInfo file in di.GetFiles("*.dll"))
                {
                    // Load the assembly so we can query for info about it.					
                    if (file.Name.StartsWith("Interop.") ||
                        file.Name.StartsWith("AxInterop."))
                    {
                        continue;
                    }
                    pc.ImportProtocolAddIns(strPath, file.FullName, foundProtocols);
                }
                foreach (string it in pc.Assemblies)
                {
                    Assembly asm = Assembly.LoadFile(it);
                }
                foreach (string it in pc.Protocols)
                {
                    Assembly asm = Assembly.LoadFile(it);
                    FindProtocolAddIns(protocols, asm);
                }
            }
            else
            {
                // Create an Application Domain:
                string pathToDll = typeof(GXDeviceList).Assembly.CodeBase;
                AppDomainSetup domainSetup = new AppDomainSetup { PrivateBinPath = pathToDll };
                System.AppDomain td = AppDomain.CreateDomain("FindAddinsDomain", null, domainSetup);
                GXProxyClass pc = (GXProxyClass)(td.CreateInstanceFromAndUnwrap(pathToDll, typeof(GXProxyClass).FullName));                
                foreach (FileInfo file in di.GetFiles("*.dll"))
                {
                    // Load the assembly so we can query for info about it.					
                    if (file.Name.StartsWith("Interop.") ||
                        file.Name.StartsWith("AxInterop."))
                    {
                        continue;
                    }
                    pc.ImportProtocolAddIns(strPath, file.FullName, foundProtocols);
                }
                List<string> assemblies = pc.Assemblies;
                List<string> protocols2 = pc.Protocols;
                // Unload the application domain:
                System.AppDomain.Unload(td);
                AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
                foreach (string it in assemblies)
                {
                    Assembly asm = Assembly.LoadFile(it);
                }
                foreach (string it in protocols2)
                {
                    Assembly asm = Assembly.LoadFile(it);
                    FindProtocolAddIns(protocols, asm);
                }
                AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
        }

        private static void FindProtocolAddIns(Dictionary<string, GXProtocolAddIn> protocols, Assembly asm)
        {
            // Iterate through each module in this assembly
            foreach (Module mod in asm.GetModules())
            {
                try
                {
                    // Iterate through the types in this module
                    foreach (Type type in mod.GetTypes())
                    {
                        if (type.IsAbstract)
                        {
                            continue;
                        }
                        GXProtocolAddIn GXAddIn = null;
                        if (type.IsSubclassOf(typeof(GXProtocolAddIn)))
                        {
                            GXAddIn = (GXProtocolAddIn)Activator.CreateInstance(type);
                            //AddIn resources must load separetly, or resources don't update.
                            string path = Path.Combine(System.IO.Path.GetDirectoryName(asm.Location), System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);
                            path = Path.Combine(path, System.IO.Path.GetFileNameWithoutExtension(asm.Location) + ".resources.dll");
                            if (System.IO.File.Exists(path))
                            {
                                Assembly.LoadFile(path);
                            }
                            string tmp = GXAddIn.Name;
                            //If name already exists.
                            if (protocols.ContainsKey(tmp))
                            {
                                continue;
                            }
                            protocols.Add(tmp, GXAddIn);
                        }
                    }
                }
                catch (Exception Ex)
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// FindItemByID finds an item using item identification.
        /// </summary>
        /// <param name="id">Item ID</param>
        /// <returns>
        /// Returns any item whose ID matches the given ID.
        /// </returns>
        public virtual object FindItemByID(UInt64 id)
        {
            if (id == 0)
            {
                return this;
            }
            return this.DeviceGroups.FindItemByID(id);
        }

        /// <summary>
		/// Returns available GXProtocolAddins.
		/// </summary>
        static Dictionary<string, GXProtocolAddIn> FindProtocols()
        {
            return FindProtocols(GXCommon.ProtocolAddInsPath);
        }

		/// <summary>
		/// Returns available GXProtocolAddins.
		/// </summary>
        static Dictionary<string, GXProtocolAddIn> FindProtocols(string path)
        {			
            // Create hashtable to fill in
            Dictionary<string, GXProtocolAddIn> protocols = new Dictionary<string, GXProtocolAddIn>();
            string name = null;
            try
            {
                //Find loaded AddIns.
                foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    FindProtocolAddIns(protocols, asm);                    
				}
                FindAddins(path, protocols);				
            }
            catch (Exception Ex)
            {
                if (name != null)
                {
                    Ex = new Exception(string.Format(Resources.FailedToLoadAddIn01, name, Ex.Message));
                }
                GXCommon.ShowError(Ex);
            }
            return protocols;
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

		/// <summary>
		/// Notifies, when the collection is cleared.
		/// </summary>
		public event ItemClearEventHandler OnClear;
		/// <summary>
		/// Notifies, when an item is removed from the device.
		/// </summary>
		public event ItemRemovedEventHandler OnRemoved;
		/// <summary>
		/// Notifies, when an item has changed.
		/// </summary>
        public event ItemUpdatedEventHandler OnUpdated;
		/// <summary>
		/// Notifies, when an item is added to the target.
		/// </summary>
		public event ItemAddedEventHandler OnAdded;

        internal virtual void NotifyClear(object sender, GXItemEventArgs e)
        {
            if (!IsLoading)
            {
                this.NotifyDirty(sender);
                if (this.FreezeEvents == 0 && OnClear != null)
                {
                    OnClear(sender, e);
                }
            }
        }

        internal virtual void NotifyRemoved(object sender, GXItemChangedEventArgs e)
        {
            this.NotifyDirty(sender);
            if (this.FreezeEvents == 0 && OnRemoved != null)
            {
                OnRemoved(sender, e);
            }
        }

        internal virtual void NotifyUpdated(object sender, GXItemEventArgs e)
        {
            if (this.FreezeEvents == 0)
            {
                bool dirty = false;
                GXPropertyEventArgs p = e as GXPropertyEventArgs;
                GXCategoryEventArgs c = e as GXCategoryEventArgs;
                GXTableEventArgs t = e as GXTableEventArgs;
                GXDeviceEventArgs d = e as GXDeviceEventArgs;
                if (e.Item is GXDeviceGroup || e.Item is GXDeviceList)
                {
                    dirty = true;
                }
                else if (p != null)
                {
                    dirty = (p.Status & PropertyStates.Updated) != 0;
                }
                else if (c != null)
                {
                    dirty = (c.Status & CategoryStates.Updated) != 0;
                }
                else if (t != null)
                {
                    dirty = (t.Status & TableStates.Updated) != 0;
                }
                else if (d != null)
                {
                    dirty = (d.Status & DeviceStates.Updated) != 0;
                }
                if (dirty)
                {
                    this.NotifyDirty(sender);
                }
                if (this.FreezeEvents == 0 && OnUpdated != null)
                {
                    OnUpdated(sender, e);
                }
            }
        }

        internal virtual void NotifyAdded(object sender, GXItemChangedEventArgs e)
        {
            this.NotifyDirty(sender);
            if (this.FreezeEvents == 0 && OnAdded != null)
            {
                OnAdded(sender, e);
            }
        }
        bool m_Dirty;
		/// <summary>
		/// True if some change is unsaved.
		/// </summary>
        [Browsable(false)]
        public bool Dirty
        {
            get
            {
                return m_Dirty;
            }
            set
            {
                if (m_Dirty != value)
                {
                    m_Dirty = value;
                    if (OnDirty != null)
                    {
                        OnDirty(this, new GXDirtyEventArgs(this, m_Dirty));
                    }
                }
            }
        }

		/// <summary>
		/// Notifie
		/// </summary>
        protected virtual void NotifyPropertyChanged(GXPropertyEventArgs e)
        {
            if (OnUpdated != null)
            {
                OnUpdated(this, e);
            }
        }

        void NotifySelectedItemChanging(GXSelectedItemEventArgs e)
        {
            if (OnSelectedItemChanging != null)
            {
                OnSelectedItemChanging(this, e);
            }
        }

        void NotifySelectedItemChanged(GXSelectedItemEventArgs e)
        {
            if (OnSelectedItemChanged != null)
            {
                OnSelectedItemChanged(this, e);
            }
        }

        void NotifyLoadProgress(GXSelectedItemEventArgs e)
        {
            if (OnLoadProgress != null)
            {
                //OnLoadProgress(this, e);
            }
        }

        void NotifyLoadBegin()
        {
            if (this.FreezeEvents == 0 && OnLoadBegin != null)
            {
                OnLoadBegin(this);
            }
        }

        void NotifyLoadEnd()
        {
            if (this.FreezeEvents == 0 && OnLoadEnd != null)
            {
                OnLoadEnd(this);
            }
        }

        internal void NotifyTransactionProgress(object sender, GXTransactionProgressEventArgs e)
        {
            if (this.FreezeEvents == 0 && OnTransactionProgress != null)
            {
                OnTransactionProgress(sender, e);
            }
        }

        internal void NotifyError(object sender, Exception e)
        {
            if (OnError != null)
            {
                OnError(sender, e);
            }
        }

        void NotifyChange(string propertyName)
        {
            if (m_OnPropertyChanged != null)
            {
                m_OnPropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        internal void NotifyDirty(object sender)
        {
            if (this.FreezeEvents == 0 && !IsLoading && !this.Dirty)
            {
                this.Dirty = true;                
            }
        }

		/// <summary>
		/// Represents the method that will handle the error event of a Gurux component.
		/// </summary>
        public event Gurux.Common.ErrorEventHandler OnError;
		/// <summary>
		/// Notifies, if the selected device, device group, category, table or item has changed.
		/// </summary>
		public event SelectedItemChangedEventHandler OnSelectedItemChanged;
		/// <summary>
		/// Notifies, when the display type has changed.
		/// </summary>
        public event DisplayTypeChangedEventHandler OnDisplayTypeChanged;

		/// <summary>
		/// Notifies, when the device settings are changed.
		/// </summary>
        public event DirtyEventHandler OnDirty;
		/// <summary>
		/// Notifies that selected device, device group, category, table or item is about to change.
		/// </summary>
        public event SelectedItemChangingEventHandler OnSelectedItemChanging;
		/// <summary>
		/// Notifies the progress of the loading.
		/// </summary>
        public event LoadProgressEventHandler OnLoadProgress;
		/// <summary>
		/// Notifies read and write transactions in the DeviceGroup, device, category, table and item.
		/// </summary>
        public event TransactionProgressEventHandler OnTransactionProgress;
		/// <summary>
		/// Notifies that the input language of the application has been changed.
		/// </summary>
        public event LocaleIdentifierChangedEventHandler OnLocaleIdentifierChanged;
		/// <summary>
		/// Notifies that loading has begun.
		/// </summary>
        public event LoadBeginEventHandler OnLoadBegin;
		/// <summary>
		/// Notifies that loading has ended.
		/// </summary>
        public event LoadEndEventHandler OnLoadEnd;
    }
}
