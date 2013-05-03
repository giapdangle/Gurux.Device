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
    /// Generates initial action messages.
    /// </summary>    
    public class GXInitialActionMessage : GXCommunicationMessageAttribute
    {
		/// <summary>
		/// When the initial action in executed.
		/// </summary>
        public InitialActionType Type
        {
            get;
            internal set;
        }

        /// <summary>
        /// Initializes a new instance of the GXWriteMessage class.
        /// </summary>
        /// <param name="requestHandler">The handler of request message.</param>
		/// <param name="type">Type of the message.</param>
        public GXInitialActionMessage(InitialActionType type, string requestHandler)
        {
            Type = type;
            m_RequestMessageHandler = requestHandler;
        }

        /// <summary>
        /// Initializes a new instance of the GXWriteMessage class.
        /// </summary>
		/// <param name="type">Type of the message</param>
        /// <param name="requestHandler">The handler of request message.</param>
        /// <param name="replyHandler">The handler of reply message.</param>
        public GXInitialActionMessage(InitialActionType type, string requestHandler, string replyHandler)
        {
            Type = type;
            m_RequestMessageHandler = requestHandler;
            m_ReplyMessageHandler = replyHandler;
        }

        /// <summary>
        /// Initializes a new instance of the GXWriteMessage class.
        /// </summary>
		/// <param name="type">Type of the message</param>
		/// <param name="requestHandler">The handler of request message.</param>
        /// <param name="replyHandler">The handler of reply message.</param>
        /// <param name="isAllSentHandler">The handler of IsAllSent message.</param>
		/// <param name="acknowledge">The handler of Acknowledge message.</param>
        public GXInitialActionMessage(InitialActionType type, string requestHandler, string replyHandler, string isAllSentHandler, string acknowledge)
        {
            Type = type;
            m_RequestMessageHandler = requestHandler;
            m_ReplyMessageHandler = replyHandler;
            m_IsAllSentMessageHandler = isAllSentHandler;
            m_AcknowledgeMessageHandler = acknowledge;
        }
    };
}
