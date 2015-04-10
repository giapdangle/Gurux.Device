using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using Gurux.Common;
using System.Runtime.Serialization;
using Gurux.Device.Editor;
using Gurux.Device.Properties;
using Gurux.Common.JSon;

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
        /// <param name='value'>Value.</param>
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
        /// Add the specified value.
        /// </summary>
        /// <param name='collection'>Collection of device manufacturers.</param>
        public void AddRange(GXDeviceManufacturerCollection collection)
        {
            foreach (GXDeviceManufacturer it in collection)
            {
                if (it.Parent == null)
                {
                    it.Parent = this;
                }
            }
            m_List.AddRange(collection);
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
            else if (target is GXDeviceProfile)
            {
                guid = (target as GXDeviceProfile).Guid;
            }
            else if (target is Guid)
            {
                guid = (Guid)target;
            }
            else
            {
                throw new Exception(Resources.UnknownTarget);
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
                        foreach (GXDeviceProfile dt in dv.Profiles)
                        {
                            if (guid == dt.Guid)
                            {
                                return dt;
                            }                           
                        }
                    }
                }
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
                path = Path.Combine(path, "Devices");
                return Path.Combine(path, "Published.json");
            }
        }

        /// <summary>
        /// Load published device profiles.
        /// </summary>
        /// <param name="manufacturers"></param>
        public static void Load(GXDeviceManufacturerCollection manufacturers)
        {
            Load(manufacturers, PublishedPath);
        }

        /// <summary>
        /// Load preset or published device profiles.
        /// </summary>
        /// <param name="Manufacturers"></param>
        /// <param name="path"></param>
        public static void Load(GXDeviceManufacturerCollection Manufacturers, string path)
        {
            Manufacturers.Clear();            
            if (File.Exists(path) && new FileInfo(path).Length != 0)
            {
                GXDeviceManufacturerCollection items = GXJsonParser.Load<GXDeviceManufacturerCollection>(path);
                Manufacturers.AddRange(items);
                Manufacturers.LastUpdated = items.LastUpdated;
            }
        }

        /// <summary>
        /// Save device settings.
        /// </summary>
        public void Save()
        {
            Save(this, PublishedPath);
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
                GXJsonParser.Save(manufacturers, path);
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
                        foreach (GXDeviceProfile type in ver.Profiles)
                        {
                            if (type.Date > dt)
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
                                    ver2.Profiles.Clear();
                                    mod2.Versions.Add(ver2);
                                }                                
                                GXDeviceProfile type2 = ver2.Profiles.Find(type.Guid);
                                if (type2 == null)
                                {
                                    type2 = new GXDeviceProfile(type);
                                    ver2.Profiles.Add(type2);
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
