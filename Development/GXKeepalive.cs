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
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;
using Gurux.Device.Editor;
using System.Threading;

namespace Gurux.Device
{   
    /// <summary>
    /// This class adds Keepalive functionality to the device.
    /// </summary>
    [DataContract(Name = "Keepalive")]
    [Serializable]
    public class GXKeepalive
    {
        [IgnoreDataMember()]
        internal AutoResetEvent m_Keepalive;
        [IgnoreDataMember()]
        internal GXDevice Parent;
        Thread m_Thread = null;
        /// <summary>
        /// Internal constructor.
        /// </summary>
        internal GXKeepalive(GXDevice parent)
        {
            Parent = parent;
            TransactionResets = true;
            m_Keepalive = new AutoResetEvent(false);
        }

        /// <summary>
        /// This is reserved to set default values after serialize in Linux because OnDeserializing do not work.
        /// </summary>
        /// <remarks>
        /// Do not use!.
        /// </remarks>
        [DataMember(Name = "Init")]
        private string Init
        {
            get
            {
                return "";
            }
            set
            {
                TransactionResets = true;
                m_Keepalive = new AutoResetEvent(false);
            }
        }

        /// <summary>
        /// Ignored keepalive fields.
        /// </summary>
        [DataMember(Order = 1)]
        [Browsable(false)]
        public KeepaliveFieldsIgnored Ignore
        {
            get;
            set;
        }

        /// <summary>
        /// Keepalive interval in ms.
        /// </summary>
        [Description("Keepalive interval in ms.")]
        [DataMember(Order = 2, IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(0)]
        public int Interval
        {
            get;
            set;
        }

        /// <summary>
        /// Is transaction reseting keepalive.
        /// </summary>
        /// <remarks>
        /// This is a normal case.
        /// </remarks>
        [Description("Is transaction reseting keepalive.")]
        [DataMember(Name="Reset", Order = 3, IsRequired = false, EmitDefaultValue = false)]
        [System.Xml.Serialization.XmlElement("Reset")]
        public bool TransactionResets
        {
            get;
            set;
        }

        /// <summary>
        /// Is keepalive running.
        /// </summary>
        [Browsable(false)]
        public bool IsRunning
        {
            get
            {
                if (m_Thread == null)
                {
                    return false;
                }
                return m_Thread.IsAlive;
            }
        }

        /// <summary>
        /// Target property to read.
        /// </summary>
        /// <remarks>
        /// This can be used if there is no keepalive in protocol.
        /// </remarks>
        [Description("Target property to read.")]
        [Editor(typeof(GXKeepaliveEditor), typeof(System.Drawing.Design.UITypeEditor))]        
        public object Target
        {
            get;
            set;
        }

        [DataMember(Name = "SerializedTarget", Order = 4, IsRequired = false, EmitDefaultValue = false)]
        ulong m_SerializedTarget;

        internal ulong SerializedTarget
        {
            get
            {
                return m_SerializedTarget;
            }
            set
            {
                if (value != 0)
                {
                    if (Parent != null)
                    {
                        Target = Parent.FindItemByID(value);
                    }
                    else
                    {
                        Target = null;
                    }
                }
                else
                {
                    Target = null;
                }
                m_SerializedTarget = value;
            }
        }

        /// <summary>
        /// Reset keepalive.
        /// </summary>
        public void Reset()
        {            
            Reseting = true;
            m_Keepalive.Set();            
        }

        /// <summary>
        /// Force keepalive.
        /// </summary>
        public void Force()
        {
            m_Keepalive.Set();
        }

        /// <summary>
        /// Keepalive started
        /// </summary>
        public void Start()
        {
            if (Interval != 0)
            {
                bool bFound = false;                
                foreach (Attribute it in Parent.GetType().GetCustomAttributes(true))
                {
                    if (it is GXInitialActionMessage && ((GXInitialActionMessage)it).Type == InitialActionType.KeepAlive)
                    {
                        bFound = true;
                        break;
                    }
                }
                if (bFound)
                {
                    Stopping = false;
                    ThreadStart job = new ThreadStart(this.Keepalive);
                    m_Thread = new Thread(job);
                    m_Thread.IsBackground = true;
                    m_Thread.Start();
                }
            }
        }

        /// <summary>
        /// Keepalive stopped.
        /// </summary>
        public void Stop()
        {
            if (m_Thread != null)
            {
                Stopping = true;
                m_Keepalive.Set();
                m_Thread.Join();                
                m_Thread = null;
            }
        }

        /// <summary>
        /// Keepalive is stopping.
        /// </summary>
        /// <remarks>
        /// Reserved for inner use only.
        /// </remarks>
        bool Stopping
        {
            get;
            set;
        }

        /// <summary>
        /// Keepalive is stopping.
        /// </summary>
        /// <remarks>
        /// Reserved for inner use only.
        /// </remarks>
        bool Reseting
        {
            get;
            set;
        }

        internal void Keepalive()
        {
            do
            {
                m_Keepalive.WaitOne(Interval);
                if (Stopping)
                {
                    break;
                }
                if (Reseting)
                {
                    Reseting = false;
                    continue;
                }
                try
                {
                    Gurux.Common.GXCommon.TraceWriteLine(DateTime.Now.ToShortTimeString() + " Keepalive generated.");
                    Parent.Status |= DeviceStates.Keepalive;
                    Parent.ExecuteInitialAction(InitialActionType.KeepAlive);
                }
                catch (Exception Ex)
                {
                    Parent.NotifyError(Parent, Ex);
                    Parent.Disconnect();
                }
                finally
                {
                    Parent.Status &= ~DeviceStates.Keepalive;
                }
            }
            while (true);
        }
    }
}
