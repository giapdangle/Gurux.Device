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

namespace Gurux.Device.Editor
{
    /// <summary>
    /// This attribute class is used to define read and write messages of a property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public abstract class GXCommunicationMessageAttribute : System.Attribute
    {

        internal string m_RequestMessageHandler = null;
        internal string m_ReplyMessageHandler = null;
        internal string m_IsAllSentMessageHandler = null;
        internal string m_AcknowledgeMessageHandler = null;
        internal bool m_EnableParentRead = true;


        /// <summary>
        /// Is parent read used.
        /// </summary>
        public bool EnableParentRead
        {
            get
            {
                return m_EnableParentRead;
            }
            set
            {
                m_EnableParentRead = value;
            }
        }

        /// <summary>
        /// Retrieves the order number. 
        /// </summary>
        public int Index
        {
            get;
            set;
        }

        /// <summary>
        /// Retrieves the handler of request message. 
        /// </summary>
        public string RequestMessageHandler
        {
            get
            {
                return m_RequestMessageHandler;
            }
        }

        /// <summary>
        /// Retrieves the handler of reply message.
        /// </summary>
        public string ReplyMessageHandler
        {
            get
            {
                return m_ReplyMessageHandler;
            }
        }

        /// <summary>
        /// Retrieves the handler of IsAllSent message.
        /// </summary>
        public string IsAllSentMessageHandler
        {
            get
            {
                return m_IsAllSentMessageHandler;
            }
        }

        /// <summary>
        /// Retrieves the handler of acknowledge message.
        /// </summary>
        public string AcknowledgeMessageHandler
        {
            get
            {
                return m_AcknowledgeMessageHandler;
            }
            set
            {
                m_AcknowledgeMessageHandler = value;
            }
        }

        /// <summary>
        /// Retrieves the handler of timeout message.
        /// </summary>
        public string TimeoutMessageHandler
        {
            get;
            set;
        }
    };
}
