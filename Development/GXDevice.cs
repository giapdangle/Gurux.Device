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
using Microsoft.Win32;
using System.Xml;
using System.Xml.Schema;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections;
using Gurux.Device.Editor;
using System.IO;
using System.Xml.Serialization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Gurux.Communication;
using Quartz;
using Gurux.Common;
using Gurux.Device.PresetDevices;

namespace Gurux.Device
{
	/// <summary>
	/// Device template in XML form.
	/// </summary>    
	[TypeConverter(typeof(GXObjectTypeConverter))]
	[GXDataIOSourceAttribute(true, GXDataIOSourceType.Device, GXDevice.AvailableTargets.All)]
	[DataContract()]
	[Serializable]
    public abstract class GXDevice : GXSite, INotifyPropertyChanged, IDisposable
	{
        System.Diagnostics.TraceLevel m_Trace;
        bool Tracing = false;
		IGXPacketHandler m_PacketHandler;
		GXKeepalive m_Keepalive;
		string m_Name;
		int m_UpdateInterval = 0;
		DeviceStates m_Status = DeviceStates.None;
		GXMediaTypeCollection m_AllowedMediaTypes = null;
		string m_Description = null;
		GXDeviceStatistics m_Statistics = null;
		bool m_Dirty = false;
		int FreezeEvents = 0;
		/// <summary>
		/// Protocol Addin that device uses.
		/// </summary>
		internal GXProtocolAddIn m_AddIn;
		internal GXEvents m_Events;
		Gurux.Communication.GXClient m_GXClient = null;
		private GXCategoryCollection m_Categories = null;
		private GXTableCollection m_Tables = null;
		private object m_sync;
		private object m_transactionsync;

		[DataMember(Name = "ID", IsRequired = false, EmitDefaultValue = false)]
		ulong m_ID;

        /// <summary>
        /// Is device preset.
        /// </summary>
        [Browsable(false)]
        public bool IsPreset
        {
            get
            {
                return !string.IsNullOrEmpty(PresetName);
            }
        }
        

        /// <summary>
        /// Preset Name of the device instance.
        /// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        [Browsable(false)]
        public string PresetName
        {
            get;
            set;
        }
        

		/// <summary>
		/// Initializes a new instance of the GXDevice class.
		/// </summary>        
		public GXDevice()
		{
			Keepalive = new GXKeepalive();
			m_Events = new GXEvents();
			this.GXClient = new GXClient();
			Categories = new GXCategoryCollection();
			Tables = new GXTableCollection();
			m_AllowedMediaTypes = new GXMediaTypeCollection(this);
			m_Statistics = new GXDeviceStatistics();
			m_sync = new object();
			m_transactionsync = new object();
		}

		/// <summary>
		/// Destructor closes the connection.
		/// </summary>
		~GXDevice()
		{
			if (this.GXClient != null)
			{
				this.GXClient.CloseServer();
			}
		}

		/// <summary>
		/// Dispose closes the connection and removes the GXDevice from parent collection.
		/// </summary>
		public override void Dispose()
		{
			if (this.GXClient != null)
			{
				this.GXClient.CloseServer();
			}
			if (this.Parent != null)
			{
				this.Parent.Remove(this);
			}
		}

        /// <summary>
        /// AddIn that device uses.
        /// </summary>
        [Browsable(false)]
        public GXProtocolAddIn AddIn
        {
            get
            {
                return m_AddIn;
            }
        }

		/// <summary>
		/// Notifies a change in the device trought events.
		/// </summary>
		protected void NotifyChange(string propertyName)
		{
			if (m_OnPropertyChanged != null)
			{
				m_OnPropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		/// <summary>
		/// Gets an object that can be used to synchronize the connection.
		/// </summary>        
		[Browsable(false)]
		public object SyncRoot
		{
			get
			{
				return m_sync;
			}
		}

		/// <summary>
		/// Override this to made changes before device load.
		/// </summary>
		/// <remarks>
		/// Rember to call base.
		/// </remarks>
		protected override void OnDeserializing(bool designMode)
		{
			Keepalive = new GXKeepalive();
			m_Events = new GXEvents();
			this.GXClient = new GXClient();
			Categories = new GXCategoryCollection();
			Tables = new GXTableCollection();
			m_AllowedMediaTypes = new GXMediaTypeCollection(this);
			m_Statistics = new GXDeviceStatistics();
			m_sync = new object();
			m_transactionsync = new object();

		}

		/// <summary>
		/// Retrieves or sets the name of the protocol of the device.
		/// </summary>
		[ReadOnly(true), Category("Design"), Description("Retrieves or sets the name of the protocol of the device.")]
		[DataMember(IsRequired = true)]
		[ValueAccess(ValueAccessType.Show, ValueAccessType.None)]
		public string ProtocolName
		{
			get;
			set;
		}
        /// <summary>
        /// Save protocol file name for Unzipping.
        /// </summary>        
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        string ProtocolFile
        {
            get;
            set;
        }

		/// <summary>
		/// Name of the GXDevice.
		/// </summary>
		[DefaultValue(null), DataMember(IsRequired = false, EmitDefaultValue = false)]
		[ValueAccess(ValueAccessType.None, ValueAccessType.None)]
		public string Name
		{
			get
			{
				return m_Name;
			}
			set
			{
				bool bChange = m_Name != value;
				m_Name = value;
				if (bChange && this.DeviceList != null)
				{
					this.DeviceList.NotifyDirty(this);
					this.NotifyUpdated(this, new GXDeviceEventArgs(this, DeviceStates.None));
				}
			}
		}

		/// <summary>
		/// Retrieves or sets the DeviceType of the device. DeviceType is defined in device template.
		/// </summary>
		[Category("Design"), Description("Retrieves or sets the DeviceType of the device. DeviceType is defined in device template.")]
		[DataMember(IsRequired = true)]
		[ValueAccess(ValueAccessType.None, ValueAccessType.None)]
		public string DeviceType
		{
			get;
			set;
		}

        /// <summary>
        /// Retrieves or sets the manufacturer.
        /// </summary>
        [Category("Design"), Description("Retrieves or sets the manufacturer.")]
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        [Browsable(false)]
        public string Manufacturer
        {
            get;
            set;
        }

        /// <summary>
        /// Retrieves or sets the model.
        /// </summary>
        [Category("Design"), Description("Retrieves or sets the model.")]
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        [Browsable(false)]
        public string Model
        {
            get;
            set;
        }

        /// <summary>
        /// Retrieves or sets the version info.
        /// </summary>
        [Category("Design"), Description("Retrieves or sets the version info.")]
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        [Browsable(false)]
        public string Version
        {
            get;
            set;
        }

		/// <summary>
		/// Locks communication channel during communication.
		/// </summary>
		/// <remarks>
		/// Set True, if several devices are using same Media and simuntaneous communication is not allowed.
		/// </remarks>
		[Category("Design"), Description("Set True, if several devices are using same Media and simuntaneous communication is not allowed.")]
		[DataMember(IsRequired = false, EmitDefaultValue = false)]
		[Browsable(false)]
		[DefaultValue(false)]
		public bool ReserveMedia
		{
			get;
			set;
		}

		/// <summary>
		/// Each device has creation Guid that is set when new device template is created.
		/// </summary>
		[DataMember(IsRequired = true)]
		[ValueAccess(ValueAccessType.None, ValueAccessType.None)]
		[ReadOnly(false)]
		public Guid Guid
		{
			get;
			set;
		}

		/// <summary>
		/// Retrieves a string representation of the value of the instance in the GXDevice class.
		/// </summary>
		/// <returns>A string representation of the value of the instance.</returns>
		public override string ToString()
		{
			return DeviceType;
		}

		/// <summary>
		/// Keepalive settings.
		/// </summary>
		[ValueAccess(ValueAccessType.None, ValueAccessType.None)]
		[DataMember(IsRequired = true)]
		[ReadOnly(true)]
		public GXKeepalive Keepalive
		{
			get
			{
				return m_Keepalive;
			}
			set
			{
				m_Keepalive = value;
				if (m_Keepalive != null)
				{
					m_Keepalive.Parent = this;
				}
			}
		}

		/// <summary>
		/// Object Identifier.
		/// </summary>
		[ValueAccess(ValueAccessType.None, ValueAccessType.None)]
		[ReadOnly(true), DefaultValue(0)]
		public ulong ID
		{
			get
			{
				return m_ID;
			}
			set
			{
				m_ID = value;
				if (m_ID != 0)
				{
					Categories.UpdateID(m_ID);
					Tables.UpdateID(m_ID);
				}
			}
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
			/// Target is a name.
			/// </summary>
			Name = 0x1,
			/// <summary>
			/// Target is a DeviceType.
			/// </summary>
			DeviceType = 0x2,
			/// <summary>
			/// Target is a ProtocolName.
			/// </summary>
			ProtocolName = 0x4,
			/// <summary>
			/// Target is an UpdateInterval.
			/// </summary>
			UpdateInterval = 0x8,
			/// <summary>
			/// Target is a ResendWaitTime.
			/// </summary>
			ResendWaitTime = 0x10,
			/// <summary>
			/// Target is a ResendCount.
			/// </summary>
			ResendCount = 0x20,
			/// <summary>
			/// Target is a Description.
			/// </summary>
			Description = 0x40
		}

		[DataMember(Name = "PacketHandler")]
		private string PacketHandlerName
		{
			get
			{
				if (PacketHandler == null)
				{
					return null;
				}
				return PacketHandler.GetType().FullName;
			}
			set
			{
				if (PacketHandler == null && !string.IsNullOrEmpty(value))
				{
                    Type type = Type.GetType(value);
                    if (type == null)
                    {
                        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                        {
                            if ((type = assembly.GetType(value)) != null)
                            {
                                break;
                            }
                        }
                    }					
					PacketHandler = Activator.CreateInstance(type) as IGXPacketHandler;
				}
			}
		}

		/// <summary>
		/// The current IGXPacketHandler provided by the GXProtocolAddin.
		/// </summary>
		[Browsable(false)]
        [ReadOnly(true)]
		public IGXPacketHandler PacketHandler
		{
			get
			{
				return m_PacketHandler;
			}
			set
			{
				m_PacketHandler = value;
				if (m_PacketHandler != null)
				{
					m_PacketHandler.Parent = this;
				}
			}
		}

		internal static void UpdateAttributes(GXDevice device)
		{
			GXCommunicationAttribute att = TypeDescriptor.GetAttributes(device.m_AddIn)[typeof(GXCommunicationAttribute)] as GXCommunicationAttribute;
			if (att == null || att.PacketHandlerType == null)
			{
				throw new Exception("Invalid GXCommunicationAttribute.");
			}
			if (att != null && att.PacketParserType != null)
			{
				device.m_GXClient.PacketParser = Activator.CreateInstance(att.PacketParserType) as IGXPacketParser;
			}
			if (att.PacketHandlerType == null)
			{
				throw new Exception("Invalid PacketHandlerType.");
			}
			if (device.PacketHandler == null)
			{
				device.PacketHandler = Activator.CreateInstance(att.PacketHandlerType) as IGXPacketHandler;
			}
		}

		/// <summary>
		/// Creates a new instance of GXDevice using the specfied GXProtocolAddIn.
		/// </summary>
		public static GXDevice CreateDeviceTemplate(GXProtocolAddIn addIn, string deviceType)
		{
			if (addIn == null)
			{
				throw new Exception("No Protocols Found.");
			}
			GXDevice device = Activator.CreateInstance(addIn.GetDeviceType()) as GXDevice;
			device.Guid = Guid.NewGuid();
            device.m_AddIn = addIn;
			device.ProtocolName = addIn.Name;
            device.ProtocolFile = Path.GetFileName(addIn.GetType().Assembly.Location);
			device.DeviceType = deviceType;
			return device;
		}

        /// <summary>
        /// Creates a new instance of GXProtocolAddIn.
        /// </summary>
        /// <param name="manufacturer">Name of the device Manufacturer.</param>
        /// <param name="model">Device model</param>
        /// <param name="deviceVersion">Device model version.</param>
        /// <param name="presetName">Preset name.</param>
        /// <param name="name">Name of the device.</param>
        /// <returns></returns>
        public static GXDevice Create(string manufacturer, string model, string deviceVersion, string presetName, string name)
        {
            GXDeviceType type = GXDeviceList.PresetDevices.Find(manufacturer, model, deviceVersion, presetName);
            if (type == null)
            {
                throw new Exception("Device type is not preset.");
            }
            GXDevice device = Load(type.Path);
            device.Manufacturer = manufacturer;
            device.Model = model;
            device.Version = deviceVersion;
            device.PresetName = presetName;
            device.Name = name;
            device.Status = DeviceStates.Loaded;
            if (device.AllowedMediaTypes.Count == 0)
            {
                device.AllowedMediaTypes.AddRange(GXDeviceList.GetMediaTypes());
            }
            return device;
        }

        /// <summary>
        /// Creates a new instance of GXProtocolAddIn.
        /// </summary>
        /// <param name="protocolName">Custom protocol name.</param>
        /// <param name="deviceType">Device template name.</param>
        /// <param name="name">Name of the device.</param>
        /// <returns></returns>
		public static GXDevice Create(string protocolName, string deviceType, string name)
		{
			if (!GXDeviceList.Protocols.ContainsKey(protocolName))
			{
				throw new Exception("Invalid protocol AddIn " + protocolName);

			}			
            GXDevice device = Load(GetDeviceTemplatePath(protocolName, deviceType));
            device.Name = name;
			device.Status = DeviceStates.Loaded;
			if (device.AllowedMediaTypes.Count == 0)
			{
				device.AllowedMediaTypes.AddRange(GXDeviceList.GetMediaTypes());
			}
			return device;
		}

		/// <summary>
		/// Reset statistics for everything in the GXDevice.
		/// </summary>
		public void ResetStatistic()
		{
			this.Statistics.Reset();
			foreach (GXCategory it in this.Categories)
			{
				it.ResetStatistic();
			}
			foreach (GXTable it in this.Tables)
			{
				it.ResetStatistic();
			}
		}

		/// <summary>
		/// The curretly used GXClient for the GXDevice.
		/// </summary>
		[ValueAccess(ValueAccessType.None, ValueAccessType.None)]
		[DataMember(IsRequired = false, EmitDefaultValue = false)]
		[ReadOnly(true)]
		public Gurux.Communication.GXClient GXClient
		{
			get
			{
				return m_GXClient;
			}
			set
			{
				if (m_GXClient != null)
				{
					m_GXClient.OnMediaStateChange -= new MediaStateChangeEventHandler(OnMediaStateChange);
					m_GXClient.OnBeforeSend -= new Gurux.Communication.BeforeSendEventHandler(this.OnBeforeSend);
					m_GXClient.OnCountChecksum -= new Gurux.Communication.CountChecksumEventHandler(this.OnCountChecksum);
					m_GXClient.OnIsReplyPacket -= new Gurux.Communication.IsReplyPacketEventHandler(this.OnIsReplyPacket);
					m_GXClient.OnAcceptNotify -= new Gurux.Communication.AcceptNotifyEventHandler(this.OnAcceptNotify);
					m_GXClient.OnLoad -= new Gurux.Communication.LoadEventHandler(this.OnLoad);
					m_GXClient.OnParsePacketFromData -= new Gurux.Communication.ParsePacketFromDataEventHandler(this.OnParsePacketFromData);
					m_GXClient.OnReceiveData -= new Gurux.Communication.ReceiveDataEventHandler(ReceiveData);
					m_GXClient.OnReceived -= new Gurux.Communication.ReceivedEventHandler(OnReceived);
					m_GXClient.OnError -= new Gurux.Common.ErrorEventHandler(NotifyError);					
					m_GXClient.OnUnload -= new Gurux.Communication.UnloadEventHandler(OnUnload);
					m_GXClient.Owner = null;
				}
				m_GXClient = value;
				if (m_GXClient != null)
				{
					m_GXClient.Owner = this;
					m_GXClient.OnMediaStateChange += new MediaStateChangeEventHandler(OnMediaStateChange);
					m_GXClient.OnBeforeSend += new Gurux.Communication.BeforeSendEventHandler(this.OnBeforeSend);
					m_GXClient.OnCountChecksum += new Gurux.Communication.CountChecksumEventHandler(this.OnCountChecksum);
					m_GXClient.OnIsReplyPacket += new Gurux.Communication.IsReplyPacketEventHandler(this.OnIsReplyPacket);
					m_GXClient.OnAcceptNotify += new Gurux.Communication.AcceptNotifyEventHandler(this.OnAcceptNotify);
					m_GXClient.OnLoad += new Gurux.Communication.LoadEventHandler(this.OnLoad);
					m_GXClient.OnParsePacketFromData += new Gurux.Communication.ParsePacketFromDataEventHandler(this.OnParsePacketFromData);
					m_GXClient.OnReceiveData += new Gurux.Communication.ReceiveDataEventHandler(ReceiveData);
					m_GXClient.OnReceived += new Gurux.Communication.ReceivedEventHandler(OnReceived);
					m_GXClient.OnError += new Gurux.Common.ErrorEventHandler(NotifyError);					
					m_GXClient.OnUnload += new Gurux.Communication.UnloadEventHandler(OnUnload);
				}
			}
		}


		void OnDirty(object sender, object component, bool isDirty)
		{
			if (this.DeviceList != null)
			{
				this.DeviceList.NotifyDirty(this);
			}
		}

		/// <summary>
		/// OnReceived event. Called when a GXPacket is received.
		/// </summary>
		public virtual void OnReceived(object sender, GXReceivedPacketEventArgs e)
		{
		}

		/// <summary>
		/// ReceiveData event. Called when raw data is received.
		/// </summary>
		public virtual void ReceiveData(object sender, Gurux.Communication.GXReceiveDataEventArgs e)
		{
		}

		/// <summary>
		/// OnParsePacketFromData event. Called to parse raw data to a GXPacket.
		/// </summary>
		public virtual void OnParsePacketFromData(object sender, Gurux.Communication.GXParsePacketEventArgs e)
		{
		}

		/// <summary>
		/// OnLoad event. Called when the GXDevice is loaded.
		/// </summary>
		public virtual void OnLoad(object sender)
		{

		}

		/// <summary>
		/// OnUnload event. Called when the GXDevice is unloaded.
		/// </summary>
		void OnUnload(object sender)
		{

		}

		/// <summary>
		/// OnAcceptNotify event. Called when a notify is received to determine if it is accepted.
		/// </summary>
		public virtual void OnAcceptNotify(object sender, Gurux.Communication.GXReplyPacketEventArgs e)
		{

		}

		/// <summary>
		/// OnIsReplyPacket event. Called when a packet is received to determine if it is a reply packet.
		/// </summary>
		public virtual void OnIsReplyPacket(object sender, Gurux.Communication.GXReplyPacketEventArgs e)
		{

		}

		/// <summary>
		/// OnCountChecksum event. Called when checksum is counted.
		/// </summary>
		public virtual void OnCountChecksum(object sender, Gurux.Communication.GXChecksumEventArgs e)
		{

		}

		/// <summary>
		/// OnCountChecksum event. Called before sending a packet.
		/// </summary>
		public virtual void OnBeforeSend(object sender, Gurux.Communication.GXPacket packet)
		{

		}

		/// <summary>
		/// Creates a clone GXDevice with same settings, attributes and properties.
		/// </summary>
		public virtual GXDevice Clone()
		{
			Type deviceType = null;
			System.Collections.Generic.List<Type> types = new System.Collections.Generic.List<Type>();
			GXDeviceList.GetAddInInfo(this.m_AddIn, out deviceType, types);
			GXDevice device = null;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (StreamReader reader = new StreamReader(memoryStream))
				{
					DataContractSerializer x = new DataContractSerializer(deviceType, deviceType.Name, "", types.ToArray());
					x.WriteObject(memoryStream, this);
					memoryStream.Position = 0;
					device = x.ReadObject(memoryStream) as GXDevice;
				}
			}
			device.m_AddIn = this.m_AddIn;
			UpdateAttributes(device);
			device.ID = 0;
			device.Status = DeviceStates.Loaded;
			return device;
		}

		/// <summary>
		/// True if something is changed and unsaved.
		/// </summary>
		[ValueAccess(ValueAccessType.None, ValueAccessType.None)]
		[System.Xml.Serialization.XmlIgnore()]
		public bool Dirty
		{
			get
			{
				return m_Dirty;
			}
			set
			{
				m_Dirty = value;
			}
		}

		/// <summary>
		/// The parent GXDeviceCollection.
		/// </summary>
		[ValueAccess(ValueAccessType.None, ValueAccessType.None)]
		[System.Xml.Serialization.XmlIgnore()]
		public GXDeviceCollection Parent
		{
			get;
			internal set;
		}

		/// <summary>
		/// Reset all child components.
		/// </summary>
		public void Reset(ResetTypes type)
		{
			switch (type)
			{
				case ResetTypes.Values:
					foreach (GXCategory cat in this.Categories)
					{
						cat.Reset(ResetTypes.Values);
					}
					break;
				case ResetTypes.Transaction:
					//TODO:
					break;
				case ResetTypes.Errors:
					this.Status &= ~DeviceStates.Error;
					foreach (GXCategory cat in this.Categories)
					{
						cat.Reset(ResetTypes.Values);
					}
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// Returns USER allowed media types.
		/// </summary>
		[CategoryAttribute("Design"), Description("Allowed media types."),
		TypeConverter(typeof(AllowedMediaTypesConverter))]
		[DataMember(Name = "AllowedMediaTypes", IsRequired = false, EmitDefaultValue = false)]
		[ValueAccess(ValueAccessType.Edit, ValueAccessType.None)]
		[ReadOnly(true)]
		[System.ComponentModel.Editor(typeof(GXNoUITypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
		public GXMediaTypeCollection AllowedMediaTypes
		{
			get
			{               
				return m_AllowedMediaTypes;
			}
			set
			{
				m_AllowedMediaTypes = value;
				if (m_AllowedMediaTypes != null)
				{
					m_AllowedMediaTypes.Parent = this;
				}
			}
		}

		/// <summary>
		/// Returns collection of media types that are allowed and installed.
		/// </summary>
		/// <returns></returns>
		public GXMediaTypeCollection GetAllowedMediaTypes()
		{
			if (m_AllowedMediaTypes == null || m_AllowedMediaTypes.Count == 0)
			{
				return GXDeviceList.GetMediaTypes();
			}
			GXMediaTypeCollection medias = new GXMediaTypeCollection();
			GXMediaTypeCollection available = GXDeviceList.GetMediaTypes();
			foreach (GXMediaType it in m_AllowedMediaTypes)
			{
				if (available[it.Name] != null)
				{
					medias.Add(it);
				}
			}
			return medias;
		}

		enum TransactionWaitTimeEnum
		{
			WaitTime = -1
		}

		/// <summary>
		/// Is device stored to the database.
		/// </summary>
		[Category("Appearance"), Description("Is device stored to the database."),
		DataMember(IsRequired = false, EmitDefaultValue = false),
		ValueAccess(ValueAccessType.Edit, ValueAccessType.None),
		Browsable(true), DefaultValue(false)]
		public virtual bool Nonstorable
		{
			get;
			set;
		}

		/// <summary>
		/// Contains collection of GXTables ans GXCategories that function as device event listeners.
		/// </summary>
		[ValueAccess(ValueAccessType.None, ValueAccessType.None)]
		public GXEvents Events
		{
			get
			{
				return m_Events;
			}
		}

		/// <summary>
		/// The parent GXDeviceList.
		/// </summary>
		[ValueAccess(ValueAccessType.None, ValueAccessType.None)]
		public GXDeviceList DeviceList
		{
			get
			{
				if (Parent == null)
				{
					return null;
				}
				return Parent.GetDeviceList();
			}
		}

		/// <summary>
		/// Read device.
		/// </summary>
		public void Read()
		{
            if (GXDeviceList.CanExecute(DisabledActions, Status, true))
            {
                this.Read(this);
            }
		}

		/// <summary>
		/// Write device.
		/// </summary>
		public void Write()
		{
            if (GXDeviceList.CanExecute(DisabledActions, Status, false))
            {
                return;
            }
			bool writing = (this.Status & DeviceStates.Writing) != 0;
			try
			{
				if (!writing)
				{
					NotifyUpdated(this, new GXDeviceEventArgs(this, DeviceStates.WriteStart));
				}
				this.Write(this);
			}
			catch (Exception Ex)
			{
				this.Status |= DeviceStates.Error;
				throw Ex;
			}
			finally
			{
				//Reset Writing flag.
				if (!writing)
				{
					this.Status &= ~DeviceStates.Writing;
					NotifyUpdated(this, new GXDeviceEventArgs(this, DeviceStates.WriteEnd));
				}
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
			foreach (GXCategory cat in this.Categories)
			{
				cat.FindByPropertyValue(name, value, items);
			}
			foreach (GXTable table in this.Tables)
			{
				table.FindByPropertyValue(name, value, items);
			}
		}

		/// <summary>
		/// Finds the first property.
		/// </summary>
		/// <returns>The first property.</returns>
		public GXProperty GetFirstProperty()
		{
			foreach (GXCategory it in Categories)
			{
				foreach (GXProperty prop in it.Properties)
				{
					return prop;
				}
			}
			return null;
		}

		/// <summary>
		/// Retrieves or sets a string that describes the device object.
		/// </summary>
		[DefaultValue(null), Category("Design"), Description("Retrieves or sets a string that describes the device object.")]
		[DataMember(IsRequired = false, EmitDefaultValue = false)]
		[ValueAccess(ValueAccessType.Edit, ValueAccessType.None)]
		public string Description
		{
			get
			{
				return m_Description;
			}
			set
			{
				m_Description = value;
			}
		}

		/// <summary>
		/// Gets progress of the current transaction.
		/// </summary>
		public GXTransactionProgressEventArgs GetTransactionProgress()
		{
			GXTransactionProgressEventArgs status = new GXTransactionProgressEventArgs(TransactionObject, TransactionPos, TransactionCount, this.Status);
			return status;
		}

		/// <summary>
		/// Get information about the protocol.
		/// </summary>
		static public void GetProtocolInfo(string filePath, out bool preset, out string protocol, out string type, out Guid guid)
		{
			XmlDataDocument myXmlDocument = new XmlDataDocument();
			using (Stream stream = File.OpenRead(filePath))
			{
				myXmlDocument.Load(stream);
				protocol = myXmlDocument.DocumentElement["ProtocolName"].InnerText;
				type = myXmlDocument.DocumentElement["DeviceType"].InnerText;
                XmlElement tmp = myXmlDocument.DocumentElement["PresetName"];
                preset = tmp != null && !string.IsNullOrEmpty(tmp.InnerText);
                guid = new Guid(myXmlDocument.DocumentElement["Guid"].InnerText);
			}
		}

		/// <summary>
		/// Load device settings from the XML File.
		/// </summary>
		/// <param name="filePath">Path to the XML file.</param>
		static public GXDevice Load(string filePath)
		{
			if (!File.Exists(filePath))
			{
				System.Diagnostics.Debug.WriteLine("Failed to Load file: " + filePath);
				throw new FileNotFoundException("Failed to Load file: " + filePath);
			}
			string protocolName = null, type = null;
            Guid deviceGuid;
            bool preset;
            GetProtocolInfo(filePath, out preset, out protocolName, out type, out deviceGuid);
			if (!GXDeviceList.Protocols.ContainsKey(protocolName))
			{
				throw new Exception("Invalid protocol AddIn " + protocolName);
			}
            GXProtocolAddIn addIn = GXDeviceList.Protocols[protocolName];
			GXDevice device = null;
			System.Collections.Generic.List<Type> types = new System.Collections.Generic.List<Type>();
			Type deviceType = null;
			GXDeviceList.GetAddInInfo(addIn, out deviceType, types);			
			using (FileStream reader = new FileStream(filePath, FileMode.Open))
			{
                DataContractSerializer x = new DataContractSerializer(deviceType, deviceType.Name, "", types.ToArray());
				device = (GXDevice)x.ReadObject(reader);
				reader.Close();
			}
			device.m_AddIn = addIn;
			UpdateAttributes(device);
			GXSite site = device as GXSite;
			site.NotifySerialized(false);
			foreach (GXCategory cat in device.Categories)
			{
				foreach (GXProperty prop in cat.Properties)
				{
					site = prop as GXSite;
					site.NotifySerialized(false);
				}
				site = cat as GXSite;
				site.NotifySerialized(false);
			}
			foreach (GXTable table in device.Tables)
			{
				foreach (GXProperty prop in table.Columns)
				{
					site = prop as GXSite;
					site.NotifySerialized(false);
				}
				site = table as GXSite;
				site.NotifySerialized(false);
			}
			return device;
		}

		/// <summary>
		/// Saves device to given XML file. 
		/// </summary>
		/// <param name="filePath">Path to file, where XML is saved to.</param>
		public virtual void Save(string filePath)
		{
            string dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
			System.Collections.Generic.List<Type> types = new System.Collections.Generic.List<Type>();
			Type deviceType = null;
			if (System.Environment.OSVersion.Platform != PlatformID.Unix)
			{
				GXDeviceList.GetAddInInfo(this.AddIn, out deviceType, types);
			}
			else
			{
                deviceType = this.AddIn.GetDeviceType();
			}

			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.Encoding = System.Text.Encoding.UTF8;
			settings.CloseOutput = true;
			settings.CheckCharacters = false;
			using (XmlWriter writer = XmlWriter.Create(filePath, settings))
			{
                DataContractSerializer x = new DataContractSerializer(deviceType, deviceType.Name, "", types.ToArray());
				x.WriteObject(writer, this);
				writer.Close();
			}
			Gurux.Common.GXFileSystemSecurity.UpdateFileSecurity(filePath);
			GXSite site = this as GXSite;
			site.NotifySerialized(true);
			foreach (GXCategory cat in this.Categories)
			{
				foreach (GXProperty prop in cat.Properties)
				{
					site = prop as GXSite;
					site.NotifySerialized(true);
				}
				site = cat as GXSite;
				site.NotifySerialized(true);
			}
			foreach (GXTable table in this.Tables)
			{
				foreach (GXProperty prop in table.Columns)
				{
					site = prop as GXSite;
					site.NotifySerialized(true);
				}
				site = table as GXSite;
				site.NotifySerialized(true);
			}
			this.Dirty = false;
		}

		/// <summary>
		/// Checks if the properties of the device are valid. 
		/// </summary>
		/// <param name="designMode"></param>
		/// <param name="tasks">Collection of tasks.</param>
		public virtual void Validate(bool designMode, GXTaskCollection tasks)
		{
		}

        /// <summary>
        /// Returns custom device template path.
        /// </summary>
        /// <param name="protocolName"></param>
        /// <param name="deviceType"></param>
        /// <returns></returns>
		public static string GetDeviceTemplatePath(string protocolName, string deviceType)
		{
			string path = GXCommon.ApplicationDataPath;
			if (Environment.OSVersion.Platform == PlatformID.Unix)
			{
				path = Path.Combine(path, ".Gurux");
			}
			else
			{
				path = Path.Combine(path, "Gurux");
			}
            path = Path.Combine(path, "Devices");
            path = Path.Combine(path, protocolName);
            path = Path.Combine(path, deviceType + ".gxt");
			return path;
		}

        /// <summary>
        /// Returns preset or published device template path.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static string GetDeviceTemplatePath(Guid guid)
        {
            string path = GXCommon.ApplicationDataPath;
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                path = Path.Combine(path, ".Gurux");
            }
            else
            {
                path = Path.Combine(path, "Gurux");
            }
            path = Path.Combine(path, "PresetDevices");
            path = Path.Combine(path, guid.ToString() + ".gxt");
            return path;
        }

		/// <summary>
		/// Determines the path to the device template.
		/// </summary>
		[ReadOnly(true), Category("Files"), Description("Determines the Device template path.")]
		[ValueAccess(ValueAccessType.Show, ValueAccessType.None)]
        [GXUserLevelAttribute(UserLevelType.Experienced)]
        public string DeviceTemplatePath
		{
			get
			{
                if (IsPreset)
                {
                    return GetDeviceTemplatePath(Guid);
                }
				return GetDeviceTemplatePath(ProtocolName, DeviceType);
			}
		}

        /// <summary>
        /// Determines the path to the device UI.
        /// </summary>
        [Browsable(false), Category("Files"), Description("Determines the path to the device UI.")]
        [ValueAccess(ValueAccessType.Show, ValueAccessType.None)]
        public string DeviceUIPath
        {
            get
            {
                string path = GXCommon.ApplicationDataPath;
                if (Environment.OSVersion.Platform == PlatformID.Unix)
                {
                    path = Path.Combine(path, ".Gurux");
                }
                else
                {
                    path = Path.Combine(path, "Gurux");
                }
                path = Path.Combine(path, "Devices");
                path = Path.Combine(path, ProtocolName);
                path = Path.Combine(path, DeviceType + ".gxi");
                return path;
            }
        }

		/// <summary>
		/// Determines the interval between updates made to the device. 
		/// The interval is given in seconds.
		/// </summary>
		[Category("Behavior"), System.ComponentModel.DefaultValue(0),
		TypeConverter(typeof(GXNumberEnumeratorConverter)), GXNumberEnumeratorConverterAttribute(typeof(GXNumberEnum)),
		Description("Determines the interval between updates made to the device. The frequency is given in seconds.")]
		[ValueAccess(ValueAccessType.None, ValueAccessType.None)]
		[DataMember(IsRequired = false, EmitDefaultValue = false)]
		public int UpdateInterval
		{
			get
			{
				return m_UpdateInterval;
			}
			set
			{
				bool bChange = m_UpdateInterval != value;
				m_UpdateInterval = value;
				if (bChange && this.DeviceList != null)
				{
					this.DeviceList.NotifyDirty(this);
					this.NotifyUpdated(this, new GXDeviceEventArgs(this, DeviceStates.None));
				}
			}
		}


		/// <summary>
		/// Returns categories collection.
		/// </summary>
		[ValueAccess(ValueAccessType.None, ValueAccessType.None)]
		[DataMember(Name = "Categories", IsRequired = false, EmitDefaultValue = false)]
		[ReadOnly(true)]
		public GXCategoryCollection Categories
		{
			get
			{
				return m_Categories;
			}
			set
			{
				m_Categories = value;
				if (m_Categories != null)
				{
					m_Categories.Parent = this;
				}
			}
		}

		/// <summary>
		/// Returns the Tables collection.
		/// </summary>
		[ValueAccess(ValueAccessType.None, ValueAccessType.None)]
		[DataMember(Name = "Tables", IsRequired = false, EmitDefaultValue = false)]
		[ReadOnly(true)]
		public GXTableCollection Tables
		{
			get
			{
				return m_Tables;
			}
			set
			{
				m_Tables = value;
				if (m_Tables != null)
				{
					m_Tables.Parent = this;
				}
			}
		}

		/// <summary>
		/// Determines how long the response packet will be waited for, until it is regarded 
		/// expired, and the original message is resent. Give the value in milliseconds.
		/// </summary>
		[CategoryAttribute("Behavior"),
		Description("Determines how long the response packet will be waited for, until it is regarded expired, and the original message is resent. Give the value in milliseconds."),
		System.ComponentModel.DefaultValue(1000)]
		[DataMember(IsRequired = false, EmitDefaultValue = false)]
		[ValueAccess(ValueAccessType.Edit, ValueAccessType.None)]
		public virtual int WaitTime
		{
			get
			{
				return m_GXClient.m_WaitTime;
			}
			set
			{
				bool bChange = m_GXClient.m_WaitTime != value;
				m_GXClient.m_WaitTime = value;
				if (bChange && this.DeviceList != null)
				{
					this.DeviceList.NotifyDirty(this);
					this.NotifyUpdated(this, new GXDeviceEventArgs(this, DeviceStates.None));
				}
			}
		}

		/// <summary>
		/// Determines how many times the message is tried to send again, in case of failure.
		/// </summary>
		[CategoryAttribute("Behavior"),
		Description("Determines how many times the message is tried to send again, in case of failure."),
		System.ComponentModel.DefaultValue(0),
		TypeConverter(typeof(GXNumberEnumeratorConverter)), GXNumberEnumeratorConverterAttribute(typeof(GXNumberEnum))]
		[DataMember(IsRequired = false, EmitDefaultValue = false)]
		[ValueAccess(ValueAccessType.Edit, ValueAccessType.None)]
		public virtual int ResendCount
		{
			get
			{
				return m_GXClient.m_ResendCount;
			}
			set
			{
				bool bChange = m_GXClient.m_ResendCount != value;
				m_GXClient.m_ResendCount = value;
				if (bChange && this.DeviceList != null)
				{
					this.DeviceList.NotifyDirty(this);
					this.NotifyUpdated(this, new GXDeviceEventArgs(this, DeviceStates.None));
				}

			}
		}

		/// <summary>
		/// Checks, if the Device is registered.
		/// </summary>
		/// <returns>True, if the Device is registered.</returns>
		public bool IsRegistered()
		{
            if (this.IsPreset)
            {
                GXDeviceType type = GXDeviceList.PresetDevices.Find(Manufacturer, Model, Version, PresetName);
                return type != null;
            }
            else
            {
                GXTemplateManager m = new GXTemplateManager();
                return m.IsExists(this.ProtocolName, DeviceType);
            }
		}

        /// <summary>
        /// Unregisters the preset Device.
        /// </summary>
        public static bool Unregister(string manufacturer, string model, string deviceVersion, string presetName)
        {
            foreach (GXDeviceManufacturer man in GXDeviceList.PresetDevices)
            {
                if (string.Compare(manufacturer, man.Name, true) == 0)
                {
                    foreach (GXDeviceModel mod in man.Models)
                    {
                        if (string.Compare(model, mod.Name, true) == 0)
                        {
                            foreach (GXDeviceVersion ver in mod.Versions)
                            {
                                if (string.Compare(deviceVersion, ver.Name, true) == 0)
                                {
                                    foreach (GXPublishedDeviceType type in ver.Templates)
                                    {
                                        if (string.Compare(type.PresetName, presetName, true) == 0)
                                        {
                                            ver.Templates.Remove(type);
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }        

		/// <summary>
		/// Unregisters the custom Device.
		/// </summary>
		public static void Unregister(string protocolName, string deviceType)
		{
            GXTemplateManager m = new GXTemplateManager();
            m.RemoveTemplate(protocolName, deviceType);
            m.Save();
		}

        /// <summary>
		/// Registers the custom Device.
		/// </summary>
        public void Register()
		{
            GXTemplateManager m = new GXTemplateManager();
            m.AddTemplate(ProtocolName, DeviceType, DeviceTemplatePath);
            m.Save();
		}

		/// <summary>
		/// Status is an enumerated state of the device.
		/// </summary>
		/// <seealso cref="GXDevice.OnDeviceStateChanged">OnDeviceStateChanged</seealso>
		/// <seealso cref="GXDeviceList.OnDeviceStateChanged">GXDeviceList.OnDeviceStateChanged</seealso>
		[ValueAccess(ValueAccessType.None, ValueAccessType.None)]
		[System.Xml.Serialization.XmlIgnore()]
		public DeviceStates Status
		{
			get
			{
				return m_Status;
			}
			internal set
			{
				m_Status = value;
			}
		}

		/// <summary>
		/// Statistic values.
		/// </summary>
		[ValueAccess(ValueAccessType.None, ValueAccessType.None)]
		[System.Xml.Serialization.XmlIgnore()]
		public GXDeviceStatistics Statistics
		{
			get
			{
				return m_Statistics;
			}
			internal set
			{
				m_Statistics = value;
			}
		}

		/// <summary>
		/// FindItemByID finds an item using item identification.
		/// </summary>
		/// <param name="id">Item ID</param>
		/// <returns>
		/// Returns any item (category, table, row or property), of the device, 
		/// whose ID matches the given ID.
		/// </returns>
		public virtual object FindItemByID(UInt64 id)
		{
			if (id == this.ID)
			{
				return this;
			}
			object item = null;
			if ((item = this.Categories.FindItemByID(id)) != null)
			{
				return item;
			}
			if ((item = this.Tables.FindItemByID(id)) != null)
			{
				return item;
			}
			return null;
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
		/// NotifyError notifies a custom error message using this method.
		/// </summary>
		/// <remarks>
		/// Use PropertyChanged to notify your own application from the device script. For example, if temperature goes under determined limit, you can send an alarm to your application.
		/// </remarks>
		public void NotifyError(object sender, Exception ex)
		{
			if (OnError != null)
			{
				OnError(sender, ex);
			}
			if (this.DeviceList != null)
			{
				this.DeviceList.NotifyError(sender, ex);
			}
		}

		/// <summary>
		/// True if an error has occured.
		/// </summary>
		[TypeConverter(typeof(GXTypeConverterNoExpand))]
		[ValueAccess(ValueAccessType.None, ValueAccessType.None)]
		[System.Xml.Serialization.XmlIgnore()]
		public bool IsErrorOccurred
		{
			get;
			set;
		}

        /// <summary>
        /// Trace level of the GXDevice.
        /// </summary>
        /// <remarks>
        /// Used in System.Diagnostic.Trace.Writes.
        /// </remarks>
        [Browsable(false)]
        public System.Diagnostics.TraceLevel Trace
        {
            get
            {
                return m_Trace;
            }
            set
            {
                if (m_Trace != value)
                {
                    m_Trace = value;
                    if (GXClient != null)
                    {
                        GXClient.Trace = this.Trace;
                        if (this.Trace != System.Diagnostics.TraceLevel.Off)
                        {
                            //Listen events only once.
                            if (!Tracing)
                            {
                                Tracing = true;
                                GXClient.OnTrace += new Gurux.Common.TraceEventHandler(GXClient_OnTrace);
                            }
                        }
                        else if (Tracing)
                        {         
                            GXClient.OnTrace -= new Gurux.Common.TraceEventHandler(GXClient_OnTrace);
                            Tracing = false;                            
                        }
                    }
                }
            }
        }

		/// <summary>
		/// Opens connection.
		/// </summary>
		public void Connect()
		{
            GXClient.Trace = this.Trace;
            if (!Tracing && this.Trace != System.Diagnostics.TraceLevel.Off)
            {
                Tracing = true;
                GXClient.OnTrace += new Gurux.Common.TraceEventHandler(GXClient_OnTrace);                
            }
			GXClient.Open();
			Keepalive.Start();
			Statistics.ConnectTime = DateTime.Now;
		}

        void GXClient_OnTrace(object sender, TraceEventArgs e)
        {
            try
            {
                if (m_OnTrace != null)
                {
                    m_OnTrace(sender, e);
                }
            }
            catch (Exception ex)
            {
                NotifyError(this, ex);
            }
        }

		/// <summary>
		/// Reconnect.
		/// </summary>
		/// <param name="suppressEvents">Are connect and disconnect events notified.</param>
		public void Reconnect(bool suppressEvents)
		{
			try
			{
				if (suppressEvents)
				{
					++FreezeEvents;
				}
				Disconnect();
				Connect();
			}
			finally
			{
				if (suppressEvents)
				{
					--FreezeEvents;
				}
			}
		}

		/// <summary>
		/// Stops keepalive and closes the connection.
		/// </summary>
		public void Disconnect()
		{
			try
			{
                StopMonitoring();
				Keepalive.Stop();
				GXClient.CloseMedia();
                if (Tracing)
                {
                    GXClient.OnTrace -= new Gurux.Common.TraceEventHandler(GXClient_OnTrace);
                    Tracing = false;
                }
			}
			finally
			{
				Statistics.DisconnectTime = DateTime.Now;
			}
		}

		/// <summary>
		/// Update new communication settings when media changed.
		/// </summary>
		/// <param name="newMedia"></param>
		public virtual void UpdateCommunicationSettings(object newMedia)
		{

		}

		void OnMediaStateChange(object sender, MediaStateEventArgs e)
		{
			//If user has selected new media.
			if (e.State == MediaState.Changed)
			{
				//There is no need to update those values on load.
				if (!GXDeviceList.IsLoading)
				{
					//Update wait time and resend count here to the GXClient
					this.GXClient.WaitTime = this.WaitTime;
					this.GXClient.ResendCount = this.ResendCount;
					OnDirty(this, this, true);
				}
				UpdateCommunicationSettings(this.GXClient.Media);
				return;
			}           

			//Do not notify if media is opening.
			if (e.State == MediaState.Opening)
			{
				return;
			}

			//Stop monitoring and execute initial actions.
			if (e.State == MediaState.Closing)
			{
                try
                {
                    StopMonitoring();
                    ExecuteInitialAction(InitialActionType.Disconnecting);
                }
                finally
                {
                    if (m_PacketHandler != null)
                    {
                        m_PacketHandler.Disconnect(this);
                    }
                }
				return;
			}
			if (e.State == MediaState.Open)
			{
				this.Status |= DeviceStates.Connected | DeviceStates.MediaConnected;
				//Reset error flag.
				this.Status &= ~DeviceStates.Error;
                if (m_PacketHandler != null)
                {
                    m_PacketHandler.Connect(this);
                }
				ExecuteInitialAction(InitialActionType.Connected);
				NotifyUpdated(this, new GXDeviceEventArgs(this, DeviceStates.Connected));
			}
			else if (e.State == MediaState.Closed)
			{
				StopMonitoring();
				this.Status &= ~(DeviceStates.Reading | DeviceStates.Writing | DeviceStates.Connected | DeviceStates.MediaConnected);
				NotifyUpdated(this, new GXDeviceEventArgs(this, DeviceStates.Disconnected));
			}
			else
			{
				throw new Exception("Invalid media state.");
			}
			return;
		}

		/// <summary>
		/// Starts monitoring the GXDevice.
		/// </summary>
		public void StartMonitoring()
		{
            if ((DisabledActions & DisabledActions.Monitor) != 0)
            {
                return;
            }
			if (this.UpdateInterval == 0)
			{
				throw new Exception("Update interval is not set.");
			}
			if (this.DeviceList == null)
			{
				throw new Exception("Parentless device monitoring is not allowed.");
			}
			if ((this.Status & DeviceStates.Connected) == 0)
			{
				Connect();
			}
			this.Status |= DeviceStates.Monitoring;
			this.NotifyUpdated(this, new GXDeviceEventArgs(this, this.Status | DeviceStates.MonitorStart));
			this.DeviceList.StartSchedules();
			JobDetail job = new JobDetail(this.Name + this.ID.ToString(), this.Name + this.ID.ToString(), typeof(Gurux.Device.GXMonitorJob));
			job.AddJobListener("GXMonitorListener");
			job.JobDataMap.Add("Target", this);
			// Trigger the job to run on the next round minute
			DateTime next = DateTime.Now.AddSeconds(1).ToUniversalTime();
			TimeSpan offset = new TimeSpan(this.UpdateInterval * 10000000);
			int repeatCnt = -1;
			SimpleTrigger trigger = new SimpleTrigger(this.Name + this.ID.ToString(), next, DateTime.MaxValue, repeatCnt, offset);
			this.DeviceList.m_sched.ScheduleJob(job, trigger);
			this.Statistics.MonitorStartTime = DateTime.Now;
		}

		/// <summary>
		/// Stops monitoring the GXDevice.
		/// </summary>
		public void StopMonitoring()
		{
			if ((this.Status & DeviceStates.Monitoring) != 0)
			{
				try
				{
					this.DeviceList.m_sched.DeleteJob(this.Name + this.ID.ToString(), this.Name + this.ID.ToString());
				}
				finally
				{
					//Clear monitoring flag.
					this.Status &= ~DeviceStates.Monitoring;
					this.Statistics.MonitorEndTime = DateTime.Now;
					this.NotifyUpdated(this, new GXDeviceEventArgs(this, this.Status | DeviceStates.MonitorEnd));
				}
			}
		}

		object TransactionObject;
		int TransactionCount, TransactionPos;

		/// <summary>
		/// Execute transaction.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="client"></param>
		/// <param name="att"></param>
		/// <param name="initialTransaction"></param>
		/// <param name="read"></param>
		/// <returns>Returs False if if there are more items to read.</returns>
		internal bool Execute(object sender, GXClient client, GXCommunicationMessageAttribute att, bool initialTransaction, bool read)
		{
			if (Keepalive.TransactionResets)
			{
				if (client.Trace >= System.Diagnostics.TraceLevel.Info)
				{
					Gurux.Common.GXCommon.TraceWriteLine(DateTime.Now.ToShortTimeString() + " Resets keepalive.");
				}
				Keepalive.Reset();
			}
			if (att != null)
			{
                if (!initialTransaction && att.EnableParentRead)
				{
					return true;
				}
				GXProperty prop = null;
				GXCategory cat = null;
				GXTable table = null;
				GXDevice device = null;
				if (sender is GXProperty)
				{
					prop = sender as GXProperty;
					prop.NotifyPropertyChange(read ? PropertyStates.ReadStart : PropertyStates.WriteStart);
				}
				else if (sender is GXCategory)
				{
					cat = sender as GXCategory;
				}
				else if (sender is GXTable)
				{
					table = sender as GXTable;
				}
				else if (sender is GXDevice)
				{
					device = sender as GXDevice;
				}
				DateTime start = DateTime.Now;
                List<GXPacket> packets = new List<GXPacket>();
                try
                {
                    GXPacket packet = client.CreatePacket();
                    packets.Add(packet);
                    if (!string.IsNullOrEmpty(att.RequestMessageHandler))
                    {
                        this.PacketHandler.ExecuteSendCommand(sender, att.RequestMessageHandler, packet);
                        //If there is no data to send.
                        if (packet.GetSize(PacketParts.Data) == 0)
                        {
                            return true;
                        }
                        //If we are not expected reply.
                        if (string.IsNullOrEmpty(att.ReplyMessageHandler))
                        {
                            packet.ResendCount = -1;
                        }
                        if (ReserveMedia)
                        {
                            lock (client.SyncCommunication)
                            {
                                client.Send(packet, true);
                            }
                        }
                        else
                        {
                            client.Send(packet, true);
                        }

                        Statistics.PacketSendTime = DateTime.Now;
                        if ((packet.Status & (PacketStates.Timeout | PacketStates.SendFailed)) != 0)
                        {
                            //If connection is closed.
                            if (!client.MediaIsOpen)
                            {
                                return false;
                            }
                            if ((packet.Status & PacketStates.Timeout) != 0)
                            {
                                throw new Exception("Timeout occurred.");
                            }
                            else
                            {
                                throw new Exception("Send failed.");
                            }
                        }
                        if (string.IsNullOrEmpty(att.IsAllSentMessageHandler))
                        {
                            if (!string.IsNullOrEmpty(att.ReplyMessageHandler))
                            {
                                this.PacketHandler.ExecuteParseCommand(sender, att.ReplyMessageHandler, packets.ToArray());

                            }
                        }
                        else
                        {
                            while (!this.PacketHandler.IsTransactionComplete(sender, att.IsAllSentMessageHandler, packet))
                            {
                                packet = client.CreatePacket();
                                packets.Add(packet);
                                this.PacketHandler.ExecuteSendCommand(sender, att.AcknowledgeMessageHandler, packet);
                                //If there is no data to send.
                                if (packet.GetSize(PacketParts.Data) == 0)
                                {
                                    throw new Exception("Invalid Acknowledge message: " + att.AcknowledgeMessageHandler + ".");
                                }
                                if (ReserveMedia)
                                {
                                    lock (client.SyncCommunication)
                                    {
                                        client.Send(packet, true);
                                    }
                                }
                                else
                                {
                                    client.Send(packet, true);
                                }
                                Statistics.PacketSendTime = DateTime.Now;
                                if ((packet.Status & (PacketStates.Timeout | PacketStates.SendFailed)) != 0)
                                {
                                    //If connection is closed.
                                    if (!client.MediaIsOpen)
                                    {
                                        return false;
                                    }
                                    if ((packet.Status & PacketStates.Timeout) != 0)
                                    {
                                        throw new Exception("Timeout occurred.");
                                    }
                                    else
                                    {
                                        throw new Exception("Send failed.");
                                    }
                                }
                            }
                            this.PacketHandler.ExecuteParseCommand(sender, att.ReplyMessageHandler, packets.ToArray());
                        }
                        if (prop != null)
                        {
                            if (read)
                            {
                                ++prop.Statistics.ReadCount;
                            }
                            else
                            {
                                ++prop.Statistics.WriteCount;
                            }
                            prop.Statistics.UpdateExecutionTime(DateTime.Now - start);
                            prop.NotifyPropertyChange(read ? PropertyStates.ReadEnd : PropertyStates.WriteEnd);
                        }
                        else if (cat != null)
                        {
                            if (read)
                            {
                                ++cat.Statistics.ReadCount;
                            }
                            else
                            {
                                ++cat.Statistics.WriteCount;
                            }
                            cat.Statistics.UpdateExecutionTime(DateTime.Now - start);
                        }
                        else if (table != null)
                        {
                            if (read)
                            {
                                ++table.Statistics.ReadCount;
                            }
                            else
                            {
                                ++table.Statistics.WriteCount;
                            }
                            table.Statistics.UpdateExecutionTime(DateTime.Now - start);
                        }
                        else if (device != null && !(att is GXInitialActionMessage))
                        {
                            if (read)
                            {
                                ++device.Statistics.ReadCount;
                            }
                            else
                            {
                                ++device.Statistics.WriteCount;
                            }
                            device.Statistics.UpdateExecutionTime(DateTime.Now - start);
                        }
                    }
                }
                catch (Exception Ex)
                {
                    if (prop != null)
                    {
                        if (read)
                        {
                            ++prop.Statistics.ReadFailCount;
                        }
                        else
                        {
                            ++prop.Statistics.WriteFailCount;
                        }

                    }
                    else if (cat != null)
                    {
                        if (read)
                        {
                            ++cat.Statistics.ReadFailCount;
                        }
                        else
                        {
                            ++cat.Statistics.WriteFailCount;
                        }
                    }
                    else if (table != null)
                    {
                        if (read)
                        {
                            ++table.Statistics.ReadFailCount;
                        }
                        else
                        {
                            ++table.Statistics.WriteFailCount;
                        }
                    }
                    else if (device != null && !(att is GXInitialActionMessage))
                    {
                        if (read)
                        {
                            ++device.Statistics.ReadFailCount;
                        }
                        else
                        {
                            ++device.Statistics.WriteFailCount;
                        }
                    }
                    if (prop != null)
                    {
                        throw new Exception(string.Format("{0} Read failed.\r\n{1}", prop.Name, Ex.Message), Ex);
                    }
                    else if (cat != null)
                    {
                        throw new Exception(string.Format("{0} Read failed.\r\n{1}", cat.Name, Ex.Message), Ex);
                    }
                    else if (table != null)
                    {
                        throw new Exception(string.Format("{0} Read failed.\r\n{1}", table.Name, Ex.Message), Ex);
                    }
                    NotifyTransactionProgress(this, new GXTransactionProgressEventArgs(sender, 1, 1, DeviceStates.ReadEnd));
                    throw Ex;
                }
                finally
                {
                    client.ReleasePacket(packets);
                }
			}
			else //If selected item do not have transaction messages.
			{
				//If we have read all items...
				if (TransactionObject == sender)
				{
					return false;
				}
				if (TransactionObject == null)
				{
					TransactionObject = sender;
				}
				if (sender is GXDevice)
				{
					//If we have read all items...
					if (TransactionObject is GXTable || TransactionObject is GXTable || TransactionObject is GXProperty)
					{
						return false;
					}
					bool bFound = false;
					if (TransactionObject == sender)
					{
						TransactionPos = 1;
						TransactionCount = this.Tables.Count;
						foreach (GXCategory cat in this.Categories)
						{
                            if (GXDeviceList.CanExecute(cat.DisabledActions, Status, read))
                            {
                                TransactionCount += cat.Properties.Count + 1;
                            }
						}
						NotifyTransactionProgress(this, new GXTransactionProgressEventArgs(sender, 0, TransactionCount, read ? DeviceStates.ReadStart : DeviceStates.WriteStart));
					}
					try
					{
						foreach (GXCategory it in this.Categories)
						{
                            if (GXDeviceList.CanExecute(it.DisabledActions, Status, read))
                            {
                                List<KeyValuePair<GXCommunicationMessageAttribute, object>> attributes = new List<KeyValuePair<GXCommunicationMessageAttribute, object>>();
                                GetCommunicationMessageAttributes(it, read ? typeof(GXReadMessage) : typeof(GXWriteMessage), attributes, false);
                                if (attributes.Count == 0)
                                {
                                    if (!Execute(it, client, null, initialTransaction, read))
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    foreach (var it2 in attributes)
                                    {
                                        TransactionObject = null;
                                        if (!Execute(it2.Value, this.GXClient, it2.Key, true, read))
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
						}
						foreach (GXTable it in this.Tables)
						{
                            if (GXDeviceList.CanExecute(it.DisabledActions, Status, read))
                            {
                                List<KeyValuePair<GXCommunicationMessageAttribute, object>> attributes = new List<KeyValuePair<GXCommunicationMessageAttribute, object>>();
                                GetCommunicationMessageAttributes(it, read ? typeof(GXReadMessage) : typeof(GXWriteMessage), attributes, false);
                                if (attributes.Count == 0)
                                {
                                    if (!Execute(it, client, null, initialTransaction, read))
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    foreach (var it2 in attributes)
                                    {
                                        TransactionObject = null;
                                        if (!Execute(it2.Value, this.GXClient, it2.Key, true, read))
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
						}
						if (read)
						{
							++this.Statistics.ReadCount;
						}
						else
						{
							++this.Statistics.WriteCount;
						}
					}
					catch (Exception Ex)
					{
						if (read)
						{
							++this.Statistics.ReadFailCount;
						}
						else
						{
							++this.Statistics.WriteFailCount;
						}
						throw Ex;
					}
					finally
					{
						if (TransactionObject == sender)
						{
							NotifyTransactionProgress(this, new GXTransactionProgressEventArgs(sender, TransactionCount, TransactionCount, read ? DeviceStates.ReadEnd : DeviceStates.WriteEnd));
							TransactionCount = TransactionPos = 1;
						}
					}
					return bFound;
				}
				if (sender is GXProperty)
				{
					if (TransactionObject is GXProperty)
					{
						try
						{
							NotifyTransactionProgress(this, new GXTransactionProgressEventArgs(sender, 0, 1, read ? DeviceStates.ReadStart : DeviceStates.ReadStart));
                            object it = (sender as GXProperty).Parent.Parent;
							List<KeyValuePair<GXCommunicationMessageAttribute, object>> attributes = new List<KeyValuePair<GXCommunicationMessageAttribute, object>>();
							GetCommunicationMessageAttributes(it, read ? typeof(GXReadMessage) : typeof(GXWriteMessage), attributes, false);
							foreach (var cAtt in attributes)
							{
								if (!Execute(it, client, cAtt.Key, initialTransaction, read))
								{
									return false;
								}
							}
							return true;
						}
						finally
						{
							NotifyTransactionProgress(this, new GXTransactionProgressEventArgs(sender, 1, 1, read ? DeviceStates.ReadEnd : DeviceStates.WriteEnd));
						}
					}
					return false;
				}
				if (sender is GXCategory)
				{
					if (TransactionObject is GXProperty)
					{
                        GXDevice device = (sender as GXCategory).Device;
						List<KeyValuePair<GXCommunicationMessageAttribute, object>> attributes = new List<KeyValuePair<GXCommunicationMessageAttribute, object>>();
						GetCommunicationMessageAttributes(device, read ? typeof(GXReadMessage) : typeof(GXWriteMessage), attributes, false);
						foreach (var cAtt in attributes)
						{
							if (!Execute(device, client, cAtt.Key, initialTransaction, read))
							{
								return false;
							}
						}
						return true;
					}
					bool bFound = false;
					if (TransactionObject == sender)
					{
						TransactionPos = 1;
                        TransactionCount = (sender as GXCategory).Properties.Count + 1;
						NotifyTransactionProgress(this, new GXTransactionProgressEventArgs(sender, 0, TransactionCount, read ? DeviceStates.ReadStart : DeviceStates.ReadStart));
					}
					try
					{
						bFound = true;
                        foreach (GXProperty it in (sender as GXCategory).Properties)
						{
                            if (GXDeviceList.CanExecute(it.DisabledActions, Status, read))
                            {
                                NotifyTransactionProgress(this, new GXTransactionProgressEventArgs(it, TransactionPos++, TransactionCount, read ? DeviceStates.ReadStart : DeviceStates.ReadStart));
                                List<KeyValuePair<GXCommunicationMessageAttribute, object>> attributes = new List<KeyValuePair<GXCommunicationMessageAttribute, object>>();
                                GetCommunicationMessageAttributes(it, read ? typeof(GXReadMessage) : typeof(GXWriteMessage), attributes, false);
                                foreach (var cAtt in attributes)
                                {
                                    if (!Execute(it, client, cAtt.Key, initialTransaction, read))
                                    {
                                        break;
                                    }
                                }                                
                                NotifyTransactionProgress(this, new GXTransactionProgressEventArgs(it, TransactionPos, TransactionCount, read ? DeviceStates.ReadEnd : DeviceStates.WriteEnd));
                            }
						}
						if (read)
						{
                            ++(sender as GXCategory).Statistics.ReadCount;
						}
						else
						{
                            ++(sender as GXCategory).Statistics.WriteCount;
						}
					}
					catch (Exception Ex)
					{
						if (read)
						{
                            ++(sender as GXCategory).Statistics.ReadFailCount;
						}
						else
						{
                            ++(sender as GXCategory).Statistics.WriteFailCount;
						}
						throw Ex;
					}
					finally
					{
						if (TransactionObject == sender)
						{
							NotifyTransactionProgress(this, new GXTransactionProgressEventArgs(sender, TransactionCount, TransactionCount, read ? DeviceStates.ReadEnd : DeviceStates.WriteEnd));
							TransactionCount = TransactionPos = 1;
						}
					}
					return bFound;
				}
				if (sender is GXTable)
				{
					try
					{
						GXDevice device = ((GXTable)sender).Device;
						List<KeyValuePair<GXCommunicationMessageAttribute, object>> attributes = new List<KeyValuePair<GXCommunicationMessageAttribute, object>>();
						GetCommunicationMessageAttributes(device, read ? typeof(GXReadMessage) : typeof(GXWriteMessage), attributes, false);
						foreach (var cAtt in attributes)
						{
							if (!Execute(device, client, cAtt.Key, initialTransaction, read))
							{
								return false;
							}
						}						
						if (read)
						{
							++((GXTable)sender).Statistics.ReadCount;
						}
						else
						{
							++((GXTable)sender).Statistics.WriteCount;
						}
						return true;
					}
					catch (Exception Ex)
					{
						if (read)
						{
							++((GXTable)sender).Statistics.ReadFailCount;
						}
						else
						{
							++((GXTable)sender).Statistics.WriteFailCount;
						}
						throw Ex;
					}
				}
			}
			return true;
		}

		/// <summary>
		/// Find communication attributes.
		/// </summary>
		void GetCommunicationMessageAttributes(object target, Type type, List<KeyValuePair<GXCommunicationMessageAttribute, object>> list, bool enableParentReadMessages)
		{

			SortedList<long, Attribute> attributes = new SortedList<long, Attribute>();
			long newIndex = int.MaxValue;
			long index;
			object[] attribs = target.GetType().GetCustomAttributes(type, true);
			foreach (GXCommunicationMessageAttribute it in attribs)
			{
				index = it.Index;
				//If index is not givent get new one.
				if (index == 0)
				{
					index = ++newIndex;
				}
				if (enableParentReadMessages && it.EnableParentRead)
				{
					continue;
				}
				if (attributes.ContainsKey(index))
				{
					throw new Exception("Failed to execute " + it.RequestMessageHandler + ". It already exists.");
				}
				attributes[index] = it;
			}
			foreach (GXCommunicationMessageAttribute it in attributes.Values)
			{
				list.Add(new KeyValuePair<GXCommunicationMessageAttribute, object>(it, target));
			}
			if (target is GXDevice)
			{
				GXDevice device = target as GXDevice;
				foreach (GXCategory it in device.Categories)
				{
					GetCommunicationMessageAttributes(it, type, list, true);
				}
				foreach (GXTable it in device.Tables)
				{
					GetCommunicationMessageAttributes(it, type, list, true);
				}
			}
			else if (target is GXCategory)
			{
				GXCategory cat = target as GXCategory;
				foreach (GXProperty it in cat.Properties)
				{
					GetCommunicationMessageAttributes(it, type, list, true);
				}
			}
		}

		/// <summary>
		/// Find communication attributes.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		Attribute[] GetInitialCommunicationMessageAttributes(object target, InitialActionType type)
		{
			SortedList<long, Attribute> attributes = new SortedList<long, Attribute>();
			object[] attribs = target.GetType().GetCustomAttributes(typeof(GXInitialActionMessage), true);
			long newIndex = int.MaxValue;
			long index;
			foreach (GXInitialActionMessage it in attribs)
			{
				if (it.Type == type)
				{
					index = it.Index;
					//If index is not givent get new one.
					if (index == 0)
					{
						index = ++newIndex;
					}
					if (attributes.ContainsKey(index))
					{
						GXInitialActionMessage existAttribute = attributes[index] as GXInitialActionMessage;
						throw new Exception("Failed to execute " + it.RequestMessageHandler + ". It's Index already exists in " + existAttribute.RequestMessageHandler + ".");
					}
					attributes[index] = it;
				}
			}
			Attribute[] tmp = new Attribute[attributes.Count];
			attributes.Values.CopyTo(tmp, 0);
			return tmp;
		}

		internal void Read(object sender)
		{
			//Wait until keepalive is ended.
			if (!System.Threading.Monitor.TryEnter(m_transactionsync, this.WaitTime))
			{
				throw new Exception("Transaction is already in progress.");
			}
			bool reading = false;
			try
			{
				reading = (this.Status & DeviceStates.Reading) != 0;
				if ((this.Status & (DeviceStates.Reading | DeviceStates.Writing)) != 0)
				{
					throw new Exception("Transaction is already in progress.");
				}
				if (!reading)
				{
					this.Status |= DeviceStates.Reading;
					if (sender is GXCategory)
					{
						GXCategory cat = sender as GXCategory;
						cat.NotifyUpdated(cat, new GXCategoryEventArgs(cat, CategoryStates.ReadStart));
					}
					else if (sender is GXTable)
					{
						GXTable table = sender as GXTable;
						table.NotifyUpdated(table, new GXTableEventArgs(table, TableStates.ReadStart, 0, null));
					}
					else if (sender is GXProperty)
					{
						GXProperty prop = sender as GXProperty;
						//Clear error flag when new read starts.
						prop.Status &= ~PropertyStates.Error;
						prop.NotifyUpdated(new GXPropertyEventArgs(prop, PropertyStates.ReadStart));
					}
					else if (sender is GXDevice)
					{
						//Clear error flag when new read starts.
						((GXDevice)sender).Status &= ~DeviceStates.Error;
						NotifyUpdated(this, new GXDeviceEventArgs(this, DeviceStates.ReadStart));
					}
				}
				List<KeyValuePair<GXCommunicationMessageAttribute, object>> attributes = new List<KeyValuePair<GXCommunicationMessageAttribute, object>>();
				GetCommunicationMessageAttributes(sender, typeof(GXReadMessage), attributes, false);
				if (attributes.Count == 0)
				{
					TransactionObject = null;
					Execute(sender, this.GXClient, null, true, true);
				}
				else
				{
					foreach (var it in attributes)
					{
						TransactionObject = null;
						if (!Execute(it.Value, this.GXClient, it.Key, true, true))
						{
							break;
						}
					}
				}
			}
			catch (Exception Ex)
			{
				this.Status |= DeviceStates.Error;
				throw Ex;
			}
			finally
			{
				System.Threading.Monitor.Exit(m_transactionsync);
				//Reset Reading flag.
				if (!reading)
				{
					this.Status &= ~DeviceStates.Reading;
					if (sender is GXCategory)
					{
						GXCategory cat = sender as GXCategory;
						cat.NotifyUpdated(cat, new GXCategoryEventArgs(cat, CategoryStates.ReadEnd));
					}
					else if (sender is GXTable)
					{
						GXTable table = sender as GXTable;
						table.NotifyUpdated(table, new GXTableEventArgs(table, TableStates.ReadEnd, 0, null));
					}
					else if (sender is GXProperty)
					{
						GXProperty prop = sender as GXProperty;
						prop.NotifyUpdated(new GXPropertyEventArgs(prop, PropertyStates.ReadEnd));
					}
					else if (sender is GXDevice)
					{
						NotifyUpdated(this, new GXDeviceEventArgs(this, DeviceStates.ReadEnd));
					}
                    NotifyTransactionProgress(this, new GXTransactionProgressEventArgs(sender, TransactionCount, TransactionCount, DeviceStates.ReadEnd));
                    TransactionCount = TransactionPos = 1;
				}
			}
		}

		internal void Write(object sender)
		{
			//Wait until keepalive is ended.
			if (!System.Threading.Monitor.TryEnter(m_transactionsync, this.WaitTime))
			{
				throw new Exception("Transaction is already in progress.");
			}
			bool writing = false;
			try
			{
				if ((this.Status & (DeviceStates.Reading | DeviceStates.Writing)) != 0)
				{
					throw new Exception("Transaction is already in progress.");
				}
				writing = (this.Status & DeviceStates.Writing) != 0;
				if (!writing)
				{
					this.Status |= DeviceStates.Writing;
					if (sender is GXCategory)
					{
						GXCategory cat = sender as GXCategory;
						cat.NotifyUpdated(cat, new GXCategoryEventArgs(cat, CategoryStates.WriteStart));
					}
					else if (sender is GXTable)
					{
						GXTable table = sender as GXTable;
						table.NotifyUpdated(table, new GXTableEventArgs(table, TableStates.WriteStart, 0, null));
					}
					else if (sender is GXProperty)
					{
						GXProperty prop = sender as GXProperty;
						prop.NotifyUpdated(new GXPropertyEventArgs(prop, PropertyStates.WriteStart));
					}
					else if (sender is GXDevice)
					{
						NotifyUpdated(this, new GXDeviceEventArgs(this, DeviceStates.WriteStart));
					}
				}
				List<KeyValuePair<GXCommunicationMessageAttribute, object>> attributes = new List<KeyValuePair<GXCommunicationMessageAttribute, object>>();
				GetCommunicationMessageAttributes(sender, typeof(GXWriteMessage), attributes, false);
				if (attributes.Count == 0)
				{
					TransactionObject = null;
					Execute(sender, this.GXClient, null, true, false);
				}
				else
				{
					foreach (var it in attributes)
					{
						TransactionObject = null;
						Execute(it.Value, this.GXClient, it.Key, true, false);
					}
				}
			}
			catch (Exception Ex)
			{
				this.Status |= DeviceStates.Error;
				throw Ex;
			}
			finally
			{
				System.Threading.Monitor.Exit(m_transactionsync);
				//Reset Reading flag.
				if (!writing)
				{
					this.Status &= ~DeviceStates.Writing;
					if (sender is GXCategory)
					{
						GXCategory cat = sender as GXCategory;
						cat.NotifyUpdated(cat, new GXCategoryEventArgs(cat, CategoryStates.WriteEnd));
					}
					else if (sender is GXTable)
					{
						GXTable table = sender as GXTable;
						table.NotifyUpdated(table, new GXTableEventArgs(table, TableStates.WriteEnd, 0, null));
					}
					else if (sender is GXProperty)
					{
						GXProperty prop = sender as GXProperty;
						prop.NotifyUpdated(new GXPropertyEventArgs(prop, PropertyStates.WriteEnd));
					}
					else if (sender is GXDevice)
					{
						NotifyUpdated(this, new GXDeviceEventArgs(this, DeviceStates.WriteEnd));
					}
				}
			}
		}

		internal void ExecuteInitialAction(InitialActionType type)
		{
			if (this.PacketHandler != null)
			{
				foreach (GXInitialActionMessage it in GetInitialCommunicationMessageAttributes(this, type))
				{
					TransactionObject = null;
					Execute(this, this.GXClient, it, true, true);
				}
			}
		}

		///<summary>
		/// Convert UI value to device value.
		///</summary>
		internal object UIValueToDeviceValue(GXProperty sender, object value)
		{
			return this.PacketHandler.UIValueToDeviceValue(sender, value);
		}

		///<summary>
		/// Convert device value to UI value.
		///</summary>
		internal object DeviceValueToUIValue(GXProperty sender, object value)
		{
			value = this.PacketHandler.DeviceValueToUIValue(sender, value);
			if (value is byte[])
			{
				value = BitConverter.ToString((byte[])value).Replace('-', ' ');
			}
			return value;
		}

		PropertyChangedEventHandler m_OnPropertyChanged;
        TraceEventHandler m_OnTrace;

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

        public event TraceEventHandler OnTrace
        {
            add
            {
                m_OnTrace += value;
            }
            remove
            {
                m_OnTrace -= value;
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
			if (FreezeEvents == 0 && OnClear != null)
			{
				OnClear(sender, e);
				if (this.DeviceList != null)
				{
					this.DeviceList.NotifyClear(sender, e);
				}
			}
		}

		internal virtual void NotifyRemoved(object sender, GXItemChangedEventArgs e)
		{
			if (FreezeEvents == 0 && OnRemoved != null)
			{
				OnRemoved(sender, e);
			}
			if (this.DeviceList != null)
			{
				this.DeviceList.NotifyRemoved(sender, e);
			}
		}

		internal virtual void NotifyUpdated(object sender, GXItemEventArgs e)
		{
			if (FreezeEvents == 0 && OnUpdated != null)
			{
				OnUpdated(sender, e);
			}
			if (this.DeviceList != null)
			{
				this.DeviceList.NotifyUpdated(sender, e);
			}
		}

		internal virtual void NotifyAdded(object sender, GXItemChangedEventArgs e)
		{
			if (FreezeEvents == 0 && OnAdded != null)
			{
				OnAdded(sender, e);
			}
			if (this.DeviceList != null)
			{
				this.DeviceList.NotifyAdded(sender, e);
			}
		}

		/// <summary>
		/// Notifies the current transaction.
		/// </summary>
		public void NotifyTransactionProgress(object sender, GXTransactionProgressEventArgs e)
		{
			if (FreezeEvents == 0 && OnTransactionProgress != null)
			{
				OnTransactionProgress(sender, e);
			}
			if (this.DeviceList != null)
			{
				DeviceList.NotifyTransactionProgress(sender, e);
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
		/// Represents the method that will handle the error event of a Gurux component.
		/// </summary>
		public event Gurux.Common.ErrorEventHandler OnError;
		/// <summary>
		/// Notifies, when the display type has changed.
		/// </summary>
		public event DisplayTypeChangedEventHandler OnDisplayTypeChanged;
		/// <summary>
		/// Notifies read and write transactions in the DeviceGroup, device, category, table and item.
		/// </summary>
		public event TransactionProgressEventHandler OnTransactionProgress;

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            if (GXClient != null)
            {
                GXClient.Dispose();
                GXClient = null;
            }
        }

        #endregion
    }
}
