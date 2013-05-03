using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using Gurux.Common;
using System.Runtime.Serialization;
using Gurux.Device.Editor;

namespace Gurux.Device.PresetDevices
{
    /// <summary>
    /// List of manufacturers.
    /// </summary>
    [DataContract()]
    [Serializable]
    public class GXDeviceManufacturerCollection : ICollection<GXDeviceManufacturer>, IEnumerable<GXDeviceManufacturer>
    {
        [DataMember(Name="Items", IsRequired = false, EmitDefaultValue = false)]
        private List<GXDeviceManufacturer> m_List = new List<GXDeviceManufacturer>();

        /// <summary>
        /// Last updated time.
        /// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public DateTime LastUpdated;

        /// <summary>
        /// Add the specified value.
        /// </summary>
        /// <param name='value'>
        /// Value.
        /// </param>
        public void Add(GXDeviceManufacturer item)
        {
            GXDeviceManufacturer it = item as GXDeviceManufacturer;
            if (it.Parent == null)
            {
                it.Parent = this;
            }
            m_List.Add(it);
        }

        /// <summary>
        /// Find manufacturer by name.
        /// </summary>
        /// <param name="manufacturer">Name of the manufacturer.</param>
        /// <returns>Found manufacturer item.</returns>
        public GXDeviceManufacturer Find(string manufacturer)
        {
            foreach (GXDeviceManufacturer man in m_List)
            {
                if (string.Compare(manufacturer, man.Name, true) == 0)
                {
                    return man;
                }
            }
            return null;
        }

        /// <summary>
        /// Check is there preset device templates.
        /// </summary>
        /// <returns></returns>
        public bool IsPresetDevices()
        {
            foreach (GXDeviceManufacturer man in m_List)
            {
                foreach (GXDeviceModel model in man.Models)
                {
                    foreach (GXDeviceVersion dv in model.Versions)
                    {
                        if (dv.Templates.Count != 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Find manufacturer by guid.
        /// </summary>
        /// <param name="manufacturer">Name of the manufacturer.</param>
        /// <returns>Found manufacturer item.</returns>
        public GXDeviceManufacturer Find(GXDeviceManufacturer manufacturer)
        {
            foreach (GXDeviceManufacturer man in m_List)
            {
                if (manufacturer.Name == man.Name)
                {
                    return man;
                }
            }
            return null;
        }

        /// <summary>
        /// Find manufacturer by guid.
        /// </summary>
        /// <param name="target">Target to find.</param>
        /// <returns>Found manufacturer item.</returns>
        public object FindRecursively(object target)
        {
            string name = null;
            System.Guid guid = Guid.Empty;
            if (target is GXDeviceManufacturer)
            {
                name = (target as GXDeviceManufacturer).Name;
            }
            else if (target is GXDeviceModel)
            {
                name = (target as GXDeviceModel).Name;
            }
            else if (target is GXDeviceVersion)
            {
                name = (target as GXDeviceVersion).Name;
            }
            else if (target is GXPublishedDeviceType)
            {
                guid = (target as GXPublishedDeviceType).Guid;
            }
            else if (target is GXTemplateVersion)
            {
                guid = (target as GXTemplateVersion).Guid;
            }
            else
            {
                throw new Exception("Unknown target.");
            }

            foreach (GXDeviceManufacturer man in m_List)
            {
                if (name == man.Name)
                {
                    return man;
                }
                foreach (GXDeviceModel model in man.Models)
                {
                    if (name == model.Name)
                    {
                        return model;
                    }
                    foreach (GXDeviceVersion dv in model.Versions)
                    {
                        if (name == dv.Name)
                        {
                            return dv;
                        }
                        foreach (GXPublishedDeviceType dt in dv.Templates)
                        {
                            if (guid == dt.Guid)
                            {
                                return dt;
                            }
                            foreach (GXTemplateVersion tv in dt.Versions)
                            {
                                if (guid == tv.Guid)
                                {
                                    return tv;
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// Find preset device type.
        /// </summary>
        /// <param name="manufacturer">Name of the manufacturer.</param>
        /// <param name="model">Name of the model</param>
        /// <param name="deviceVersion">Device version.</param>
        /// <param name="presetName">Preset name.</param>
        /// <returns>Found GXDeviceType or null if not found.</returns>
        public GXDeviceType Find(string manufacturer, string model, string deviceVersion, string presetName)
        {
            GXDeviceManufacturer man = Find(manufacturer);
            if (man == null)
            {
                return null;
            }
            GXDeviceModel mod = man.Models.Find(model);
            if (mod == null)
            {
                return null;
            }
            GXDeviceVersion ver = mod.Versions.Find(deviceVersion);
            if (ver == null)
            {
                return null;
            }
            return ver.Templates.Find(presetName);
        }

        /// <summary>
        /// Find preset device type.
        /// </summary>
        /// <param name="manufacturer">Name of the manufacturer.</param>
        /// <param name="model">Name of the model</param>
        /// <param name="deviceVersion">Device version.</param>
        /// <param name="presetName">Preset name.</param>
        /// <returns>Found GXTemplateVersion or null if not found.</returns>
        public GXTemplateVersion Find(string manufacturer, string model, string deviceVersion, string presetName, int version)
        {
            GXPublishedDeviceType type = Find(manufacturer, model, deviceVersion, presetName) as GXPublishedDeviceType;
            if (type != null)
            {
                return type.Versions.Find(version);
            }
            return null;
        }

        /// <summary>
        /// Path where published templates are saved.
        /// </summary>
        public static string PublishedPath
        {
            get
            {
                string path = string.Empty;
                if (Environment.OSVersion.Platform == PlatformID.Unix)
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    path = Path.Combine(path, ".Gurux");
                }
                else
                {
                    //Vista: C:\ProgramData
                    //XP: c:\Program Files\Common Files                
                    //XP = 5.1 & Vista = 6.0
                    if (Environment.OSVersion.Version.Major >= 6)
                    {
                        path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                    }
                    else
                    {
                        path = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles);
                    }
                    path = Path.Combine(path, "Gurux");
                }
                path = Path.Combine(path, "PresetDevices");
                return Path.Combine(path, "Published.xml");
            }
        }

        /// <summary>
        /// Path where preset template settings are saved.
        /// </summary>
        public static string PresetDevicesPath
        {
            get
            {
                string path = string.Empty;
                if (Environment.OSVersion.Platform == PlatformID.Unix)
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    path = Path.Combine(path, ".Gurux");
                }
                else
                {
                    //Vista: C:\ProgramData
                    //XP: c:\Program Files\Common Files                
                    //XP = 5.1 & Vista = 6.0
                    if (Environment.OSVersion.Version.Major >= 6)
                    {
                        path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                    }
                    else
                    {
                        path = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles);
                    }
                    path = Path.Combine(path, "Gurux");
                }
                path = Path.Combine(path, "PresetDevices");
                return Path.Combine(path, "PresetDevices.xml");
            }
        }

        public static void Load(GXDeviceManufacturerCollection manufacturers)
        {
            Load(manufacturers, PresetDevicesPath);
        }

        public static void Load(GXDeviceManufacturerCollection Manufacturers, string path)
        {
            Manufacturers.Clear();
            if (File.Exists(path) && new FileInfo(path).Length != 0)
            {
                Type[] extraTypes = new Type[] { typeof(GXDeviceManufacturerCollection), typeof(GXDeviceManufacturer), typeof(GXDeviceModelCollection), typeof(GXDeviceModel), typeof(GXDeviceVersionCollection), typeof(GXPublishedDeviceType) };
                DataContractSerializer x = new DataContractSerializer(typeof(GXDeviceManufacturerCollection), extraTypes);
                lock (Manufacturers)
                {
                    using (FileStream reader = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        try
                        {
                            GXDeviceManufacturerCollection items = x.ReadObject(reader) as GXDeviceManufacturerCollection;
                            //Update parents
                            foreach (GXDeviceManufacturer it in items)
                            {
                                Manufacturers.Add(it);
                            }
                            Manufacturers.LastUpdated = items.LastUpdated;
                        }
                        catch (Exception Ex)
                        {
                            System.Diagnostics.Debug.WriteLine(Ex.Message);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Save device settings.
        /// </summary>
        public void Save()
        {
            Save(this, PresetDevicesPath);
        }

        public static void Save(GXDeviceManufacturerCollection manufacturers, string path)
        {
            //Do not save empty list.
            if (manufacturers.Count != 0)
            {
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }
                Type[] extraTypes = new Type[] { typeof(GXDeviceManufacturerCollection), typeof(GXDeviceManufacturer), typeof(GXDeviceModelCollection), typeof(GXDeviceModel), typeof(GXDeviceVersionCollection), typeof(GXPublishedDeviceType) };
                DataContractSerializer x = new DataContractSerializer(typeof(GXDeviceManufacturerCollection), extraTypes);
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.Encoding = System.Text.Encoding.UTF8;
                settings.CloseOutput = true;
                settings.CheckCharacters = false;
                lock (manufacturers)
                {
                    using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        using (XmlWriter writer = XmlWriter.Create(stream, settings))
                        {
                            x.WriteObject(writer, manufacturers);
                        }
                    }
                    
                }
                GXFileSystemSecurity.UpdateFileSecurity(path);
            }
            else
            {
                if (File.Exists(path))
                {
                    try
                    {
                        File.Delete(path);
                    }
                    catch
                    {
                        //Ignore if fails.
                    }
                }
            }
        }

        /// <summary>
        /// Get devices by publish date.
        /// </summary>
        /// <param name="dt">Lower publish date.</param>
        /// <returns>Collection of published devices.</returns>
        public GXDeviceManufacturerCollection GetDevicesByPublishDate(DateTime dt)
        {
            GXDeviceManufacturerCollection items = new GXDeviceManufacturerCollection();
            foreach (GXDeviceManufacturer man in this)
            {
                foreach (GXDeviceModel mod in man.Models)
                {
                    foreach (GXDeviceVersion ver in mod.Versions)
                    {
                        foreach (GXPublishedDeviceType type in ver.Templates)
                        {
                            foreach (GXTemplateVersion tv in type.Versions)
                            {
                                if (tv.Date > dt)
                                {
                                    GXDeviceManufacturer man2 = items.Find(man);
                                    if (man2 == null)
                                    {
                                        man2 = new GXDeviceManufacturer(man);
                                        man2.Models.Clear();
                                        items.Add(man2);
                                    }
                                    GXDeviceModel mod2 = man2.Models.Find(mod);
                                    if (mod2 == null)
                                    {
                                        mod2 = new GXDeviceModel(mod);
                                        mod2.Versions.Clear();
                                        man2.Models.Add(mod2);
                                    }
                                    GXDeviceVersion ver2 = mod2.Versions.Find(ver);
                                    if (ver2 == null)
                                    {
                                        ver2 = new GXDeviceVersion(ver);
                                        ver2.Templates.Clear();
                                        mod2.Versions.Add(ver2);
                                    }
                                    GXPublishedDeviceType type2 = ver2.Templates.Find(type);
                                    if (type2 == null)
                                    {
                                        type2 = new GXPublishedDeviceType(type);
                                        type2.Versions.Clear();
                                        ver2.Templates.Add(type2);
                                    }
                                    type2.Versions.Add(new GXTemplateVersion(tv));
                                }
                            }
                        }
                    }
                }
            }
            return items;
        }
        
        #region ICollection<GXDeviceManufacturer> Members


        public void Clear()
        {
            m_List.Clear();
        }

        public bool Contains(GXDeviceManufacturer item)
        {
            return m_List.Contains(item);
        }

        public void CopyTo(GXDeviceManufacturer[] array, int arrayIndex)
        {
            m_List.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get 
            {
                return m_List.Count;
            }
        }

        public bool IsReadOnly
        {
            get 
            {
                return false;
            }
        }

        public bool Remove(GXDeviceManufacturer item)
        {
            if (item.Parent == this)
            {
                item.Parent = null;
            }
            return m_List.Remove(item);
        }

        #endregion

        #region IEquatable<GXDeviceManufacturer> Members

        public bool Equals(GXDeviceManufacturer other)
        {
            return m_List.Equals(other);
        }

        #endregion

        public GXDeviceManufacturer this[int index]
        {
            get
            {
                return m_List[index];
            }
            set
            {
                m_List[index] = value;
            }
        }
        #region IEnumerable<GXDeviceManufacturer> Members

        public IEnumerator<GXDeviceManufacturer> GetEnumerator()
        {
            //m_List is nul after serialization.
            if (m_List == null)
            {
                m_List = new List<GXDeviceManufacturer>();
            }
            return m_List.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return m_List.GetEnumerator();
        }

        #endregion
     }
}
