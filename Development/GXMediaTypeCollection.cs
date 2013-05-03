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
using System.Runtime.Serialization;

namespace Gurux.Device
{
	/// <summary>
	/// A collection of GXMediaTypes.
	/// </summary>
    [Serializable]
    [System.ComponentModel.Editor(typeof(Gurux.Device.Editor.GXNoUITypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
    public class GXMediaTypeCollection : GenericList<GXMediaType>, System.Collections.IList        
    {
		/// <summary>
		/// Default constructor.
		/// </summary>
        public GXMediaTypeCollection()
        {

        }

        /// <summary>
        /// Add the specified value.
        /// </summary>
        /// <param name='value'>
        /// Value.
        /// </param>
        /// <remarks>
        /// Mono needs this. Do not remove!
        /// </remarks>
        int System.Collections.IList.Add(object value)
        {
            base.Add(value as GXMediaType);
            return this.Items.Count - 1;
        }

		/// <summary>
		/// Constructor.
		/// </summary>
        public GXMediaTypeCollection(GXDevice device)
        {
            Parent = device;
        }

		/// <summary>
		/// The parent GXDevice.
		/// </summary>
        [System.Xml.Serialization.XmlIgnore()]
        public GXDevice Parent
        {
            get;
            internal set;
        }

		/// <summary>
		/// Set the item parent to this collection.
		/// </summary>
        protected override void OnBeforeItemAdded(object sender, GenericItemEventArgs<GXMediaType> e)
        {            
            if (e.Item.Parent == null)
            {
                e.Item.Parent = this;
            }
        }

		/// <summary>
		/// Clears the item parent.
		/// </summary>
        protected override void OnBeforeItemRemoved(object sender, GenericItemEventArgs<GXMediaType> e)
        {
            if (e.Item.Parent == this)
            {                
                e.Item.Parent = null;
            }
        }

		/// <summary>
		/// Clears the every items parent and removes them from the collection.
		/// </summary>
        public override void Clear()
        {
            foreach (GXMediaType it in this)
            {
                if (it.Parent == this)
                {
                    it.Parent = null;
                }
            }
            base.Clear();
        }        

		/// <summary>
		/// Name based indexer.
		/// </summary>
        public GXMediaType this[string name]
        {
            get
            {
                foreach (GXMediaType it in this)
                {
                    if (string.Compare(it.Name, name, true) == 0)
                    {
                        return it;
                    }
                }
                return null;
            }
            set
            {
                int pos = 0;
                foreach (GXMediaType it in this)
                {
                    if (string.Compare(it.Name, name, true) == 0)
                    {
                        this[pos] = value;
                        break;
                    }
                    ++pos;
                }
            }
        }
    }
}
