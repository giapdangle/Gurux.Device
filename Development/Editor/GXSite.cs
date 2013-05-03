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
using System.ComponentModel;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Gurux.Device.Editor
{
	/// <summary>
    /// With GXSite class, component design time support is added.
	/// </summary>
    [ToolboxItem(false)]
    [System.Runtime.Serialization.DataContract()]
    public abstract class GXSite : IComponent
	{
        [System.Xml.Serialization.XmlIgnore()]
		private ISite m_Site = null;
		/// <summary>
		/// Keeps a list of items that need this component.
		/// </summary>
        [System.Xml.Serialization.XmlIgnore()]
        private System.Collections.Hashtable m_DependencyList = new System.Collections.Hashtable();

		/// <summary>
		/// Reserved for inner use. Do not use.
		/// </summary>
        [ValueAccess(ValueAccessType.None, ValueAccessType.None)]
        [System.Xml.Serialization.XmlIgnore()]
        public System.Collections.Hashtable DependencyList
        {
            get
            {
                return m_DependencyList;
            }
        }

        internal void NotifySerialized(bool serialized)
        {
            if (serialized)
            {
                OnSerialized(m_Site != null);
            }
            else
            {
                OnDeserialized(m_Site != null);
            }             
        }

		/// <summary>
		/// This is reserved to set default values in serializing in Linux because OnDeserializing do not work.
		/// </summary>
		/// <remarks>
		/// Do not use!.
		/// </remarks>
		[DataMember(Name = "Init", IsRequired = false, EmitDefaultValue = true)]
		protected string Init
		{
			get
			{
                OnSerializing(m_Site != null);
				return "";
			}
			set
			{
                OnDeserializing(m_Site != null);
			}
		}
		
		/// <summary>
		/// Override this to made changes before device save.
		/// </summary>
        protected virtual void OnSerializing(bool designMode)
		{			
		}

		/// <summary>
		/// Override this to made changes before device load.
		/// </summary>
        protected virtual void OnDeserializing(bool designMode)
		{			
		}
		
		/// <summary>
		/// Override this to made changes after device save.
		/// </summary>
        protected virtual void OnSerialized(bool designMode)
		{			
		}

		/// <summary>
		/// Override this to made changes after device load.
		/// </summary>
		protected virtual void OnDeserialized(bool designMode)
		{			
		}
		
		#region IComponent Members
		System.EventHandler m_Disposed;

        /// <summary> 
        /// Occurs when the component is disposed by a call to the Dispose method.
        /// </summary>
		public event System.EventHandler Disposed
		{
			add
			{
				m_Disposed += value;
			}
			remove
			{
				m_Disposed -= value;
			}
		}

		/// <summary>
        /// Gets or sets the ISite of the Component.
		/// </summary>
        [ValueAccess(ValueAccessType.None, ValueAccessType.None)]
        [Browsable(false)]
        [System.Xml.Serialization.XmlIgnore()]
        public ISite Site
		{
			get
			{
				return m_Site;
			}
			set
			{
				m_Site = value;
			}
		}

		/// <summary>
		/// Notifies, when the value of an object changes.
		/// </summary>
		/// <param name="comp">The object.</param>
		/// <param name="name">Name of the object.</param>
		/// <param name="oldValue">Previous value of the object.</param>
		/// <param name="newValue">Changed value of the object.</param>
		internal void NotifyChange(object comp, string name, object oldValue, object newValue)
		{
			if (m_Site != null)
			{
				System.ComponentModel.Design.IComponentChangeService ccsChanger = (System.ComponentModel.Design.IComponentChangeService)m_Site.GetService(typeof(System.ComponentModel.Design.IComponentChangeService));
				if (ccsChanger != null)
				{
					MemberDescriptor md = TypeDescriptor.GetProperties(comp).Find(name, true);
					if (md == null) //MemberDescriptor might be null in beginner user mode.
					{
						System.Diagnostics.Debug.WriteLine(string.Format("GXSite NotifyChange failed. Propertys '{0}' name '{1}' is unknown", m_Site.Name, name));
						return;
					}
					ccsChanger.OnComponentChanging(m_Site.Component, md);
					ccsChanger.OnComponentChanged(m_Site.Component, md, oldValue, newValue);
				}
			}
		}

		#endregion

		#region IDisposable Members

        /// <summary> 
        /// Cleans up any resources being used.
        /// </summary>
		public virtual void Dispose()
		{
		}

		#endregion
	}
}
