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

using System.Collections;
using System.Xml;
using System;
using Gurux.Common;

namespace Gurux.Device.Editor
{

	/// <summary>
    /// GXTemplateManager keeps a managed list of GXDevice template objects.
	/// </summary>
	public class GXTemplateManager
	{		
		Hashtable List = new Hashtable();

		/// <summary>
		/// GXDevice template item. GXDevice template name and path are kept in this object.
		/// </summary>
        internal class GXTemplateManagerItem
		{
			/// <summary>
			/// Initializes a new instance of the GXTemplateManagerItem class.
			/// </summary>
			/// <param name="parent">Parent collection of the new instance.</param>
			/// <param name="name">Name of the new instance.</param>
			/// <param name="path">Path to the GXDevice template file.</param>
			public GXTemplateManagerItem(GXTemplateManagerItems parent, string name, string path)
			{
				Parent = parent;
				Name = name;
				Path = path;
			}

			/// <summary>
			/// Parent collection.
			/// </summary>
			public GXTemplateManagerItems Parent = null;

			/// <summary>
			/// Retrieves or sets the name of the GXTemplateManager object.
			/// </summary>
			public string Name;

			/// <summary>
            /// Retrieves or sets the path to the GXDevice template.
			/// </summary>
			public string Path;
		}

		/// <summary>
		/// List of GXDevice template items.
		/// </summary>
		public class GXTemplateManagerItems
		{
			/// <summary>
			/// Initializes a new instance of the GXTemplateManagerItems class.
			/// </summary>
			/// <param name="protocol">Protocol of the new instance.</param>
			public GXTemplateManagerItems(string protocol)
			{
				Protocol = protocol;
			}

			/// <summary>
            /// Retrieves or sets the protocol of the template manager item.
			/// </summary>
			public string Protocol;

			/// <summary>
            /// Retrieves or sets an array list of template manager items.
			/// </summary>
			public ArrayList List = new ArrayList();
		}

		/// <summary>
		/// Initializes a new instance of the GXTemplateManager class.
		/// </summary>
		public GXTemplateManager()
		{
			Load();
		}

		/// <summary>
		/// Adds a new template to GXTemplateManager.
		/// </summary>
		/// <param name="protocol">Protocol of the new template.</param>
		/// <param name="name">Name of the new template.</param>
        /// <param name="path">Path to GXTemplateManager.</param>
		public void AddTemplate(string protocol, string name, string path)
		{
			//Remove if exists.
			RemoveTemplate(protocol, name);
			GXTemplateManagerItems item = (GXTemplateManagerItems)List[protocol];
			if (item == null)
			{
				item = new GXTemplateManagerItems(protocol);
				List.Add(protocol, item);
			}
			item.List.Add(new GXTemplateManagerItem(item, name, path));
		}

		/// <summary>
		/// Removes a template from template manager list.
		/// </summary>
		/// <param name="protocol">Protocol of the template to remove.</param>
		/// <param name="template">Name of the template to remove.</param>
		public void RemoveTemplate(string protocol, string template)
		{
			GXTemplateManagerItems items = (GXTemplateManagerItems)List[protocol];
			if (items != null)
			{
				foreach (GXTemplateManagerItem it in items.List)
				{
					if (string.Compare(it.Name, template, true) == 0)
					{
						items.List.Remove(it);
						return;
					}
				}
			}
		}

		/// <summary>
		/// Retrieves collection of available templates.
		/// </summary>
		/// <param name="protocol">Protocol of the template.</param>
        /// <returns>Collection of available templates.</returns>
		public GXTemplateManagerItems AvailableTemplates(string protocol)
		{
			//Return all available protocols.
			if (string.IsNullOrEmpty(protocol))
			{
				GXTemplateManagerItems items = new GXTemplateManagerItems("");
				foreach (DictionaryEntry it in List)
				{
					GXTemplateManagerItems tmp = (GXTemplateManagerItems)it.Value;
					items.List.AddRange(tmp.List);
				}
				return items;

			}
			return (GXTemplateManagerItems)List[protocol];
		}

		/// <summary>
		/// Checks if the template exists.
		/// </summary>
		/// <param name="protocol">Protocol of the template to check.</param>
		/// <param name="template">Name of the template to check.</param>
		/// <returns>The template, if found.</returns>
		public bool IsExists(string protocol, string template)
		{
			return GetPath(protocol, template) != "";

		}

		/// <summary>
        /// Retrieves the path to template manager list.
		/// </summary>
		/// <param name="protocol">Protocol of the template.</param>
		/// <param name="template">Name of the template.</param>
        /// <returns>The path to template manager list.</returns>
		public string GetPath(string protocol, string template)
		{
			GXTemplateManagerItems items = (GXTemplateManagerItems)List[protocol];
			if (items != null)
			{
				foreach (GXTemplateManagerItem it in items.List)
				{
					if (string.Compare(it.Name, template, true) == 0)
					{
						return it.Path;
					}
				}
			}
			return string.Empty;
		}

		/// <summary>
		/// Load settings from the xml file.
		/// </summary>
		private void Load()
		{			
			string TemplatePath = GXCommon.ApplicationDataPath;
			if (Environment.OSVersion.Platform == PlatformID.Unix)
			{	
				TemplatePath = System.IO.Path.Combine(TemplatePath, ".Gurux");
			}				
			else
			{
				TemplatePath = System.IO.Path.Combine(TemplatePath, "Gurux");
			}
            TemplatePath = System.IO.Path.Combine(TemplatePath, "Devices");
			TemplatePath = System.IO.Path.Combine(TemplatePath, "templates.xml");
			if (!System.IO.File.Exists(TemplatePath))
			{
				return;
			}
			string Protocol, Path, Name;
			using(XmlTextReader xmlReader = new XmlTextReader(TemplatePath))
			{
				XmlDataDocument myXmlDocument = new XmlDataDocument();				
				myXmlDocument.Load (xmlReader);
				foreach (XmlNode prot in myXmlDocument.DocumentElement.ChildNodes)
				{
					if (string.Compare(prot.Name, "Protocol", true) == 0)
					{
						XmlAttribute att = prot.Attributes["Name"];
						if (att != null)
						{
							Protocol = att.InnerText;
							foreach (XmlNode templ in prot.ChildNodes)
							{
								if (string.Compare(templ.Name, "Template", true) == 0)
								{
									Name = templ.Attributes["Name"].InnerText;
									Path = templ.Attributes["Path"].InnerText;
									AddTemplate(Protocol, Name, Path);
								}
							}
						}
					}
				}				
			}
		}

		/// <summary>
		/// Save settings to the xml file.
		/// </summary>
		public void Save()
		{
			string TemplatePath = GXCommon.ApplicationDataPath;
			if (Environment.OSVersion.Platform == PlatformID.Unix)
			{
				TemplatePath = System.IO.Path.Combine(TemplatePath, ".Gurux");
			}
			else
			{
				TemplatePath = System.IO.Path.Combine(TemplatePath, "Gurux");
			}
			
            TemplatePath = System.IO.Path.Combine(TemplatePath, "Devices");
			TemplatePath = System.IO.Path.Combine(TemplatePath, "templates.xml");
			using(XmlTextWriter xmlWriter = new XmlTextWriter(TemplatePath, System.Text.Encoding.Default))
			{
				// Format automatically
				xmlWriter.Formatting = Formatting.Indented;
				xmlWriter.WriteStartDocument();
				xmlWriter.WriteStartElement("GXTemplates");
				foreach (DictionaryEntry it in List)
				{
					if (((GXTemplateManagerItems)it.Value).List.Count == 0)
					{
						continue;
					}
					xmlWriter.WriteStartElement("Protocol");
					xmlWriter.WriteAttributeString("Name", it.Key.ToString());
					foreach (GXTemplateManagerItem item in ((GXTemplateManagerItems)it.Value).List)
					{
						xmlWriter.WriteStartElement("Template");
						xmlWriter.WriteAttributeString("Name", item.Name);
						xmlWriter.WriteAttributeString("Path", item.Path);
						// write end elements
						xmlWriter.WriteEndElement();
					}
					// write end elements
					xmlWriter.WriteEndElement();
				}
				// write end elements
				xmlWriter.WriteEndElement();
				// close the writer
				xmlWriter.Close();
				}
            Gurux.Common.GXFileSystemSecurity.UpdateFileSecurity(TemplatePath);
		}
	}
}
