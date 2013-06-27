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
using System.IO;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Win32;
using System.Linq;
using ICSharpCode.SharpZipLib.Zip;
using System.Reflection;
using Gurux.Common;
using System.Collections.Generic;

namespace Gurux.Device.Editor
{
	/// <summary>
	/// To make a GXDevice accessible with other computers, the device template is exported 
    /// to a .gxz file. The template can then be opened with another computer, with all 
    /// original information, for example, a user made UI.
	/// </summary>
	public class GXZip
	{
		/// <summary>
		/// Imports and exports GXDeviceCollection from/to .gxz files.
		/// </summary>
		public GXZip()
		{
		}

		/// <summary>
		/// Extracts Zip file to a directory.
		/// </summary>
		public static string[] Extract(string zipFilePath, string targetDirPath)
		{
			List<string> files = new List<string>();
			ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath));
			ZipEntry theEntry;
			while ((theEntry = s.GetNextEntry()) != null)
			{
				string fileName = Path.Combine(targetDirPath, theEntry.Name);
				
				files.Add(fileName);
				WriteFile(s, fileName);
			}
			s.Close();

			return files.ToArray();
		}

        /// <summary>
        /// Imports a GXDevice from a .gxz file.
        /// </summary>
        /// <param name="parent">Parent window.</param>
        /// <param name="path">Path to the file to import.</param>
        public static GXDeviceType Import(IWin32Window parent, string path)
        {
            return Import(parent, path, null);
        }

        public static GXDeviceType Import(IWin32Window parent, string path, string target)
        {
            ZipInputStream s = new ZipInputStream(File.OpenRead(path));
            return Import(parent, s, target);
        }

        /// <summary>
        /// Imports a GXDevice from a byte array.
        /// </summary>
        public static GXDeviceType Import(IWin32Window parent, byte[] data, string target)
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(data, 0, data.Length);
            ms.Seek(0, SeekOrigin.Begin);
            using (ZipInputStream s = new ZipInputStream(ms))
            {
                return Import(parent, s, target);
            }
        }

		/// <summary>
		/// Imports a GXDevice from a stream.
		/// </summary>
        private static GXDeviceType Import(IWin32Window parent, ZipInputStream s, string target)
        {
            string TempPath = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString();
            Directory.CreateDirectory(TempPath);
            //Extract to windows temp directory
            ZipEntry theEntry;
            ArrayList FilePaths = new ArrayList();
            string DevicePath = "";
            while ((theEntry = s.GetNextEntry()) != null)
            {
                string FileName = theEntry.Name;
                FileName = Path.Combine(TempPath , FileName);
                if (string.Compare(Path.GetExtension(theEntry.Name), ".gxt", true) == 0)
                {
                    DevicePath = FileName; //Save .gxt file name for opening
                }
				FilePaths.Add(FileName);
				WriteFile(s, FileName);
            }
            s.Close();

            if (DevicePath.Trim().Length == 0)
            {
                throw new Exception("Device file does not exist in the packet.");
            }
            if (!File.Exists(DevicePath)) //Shouldn't happen
            {
                throw new Exception("Could not find unpacked device file");
            }

            string protocol = null, deviceType = null;
            bool preset;
            Guid deviceGuid;            
            GXDevice.GetProtocolInfo(DevicePath, out preset, out protocol, out deviceType, out deviceGuid);
            GXDeviceType type;
            if (preset)
            {
                Gurux.Device.PresetDevices.GXPublishedDeviceType t = new Gurux.Device.PresetDevices.GXPublishedDeviceType();
                t.DeviceGuid = t.Guid = deviceGuid;
                t.Protocol = protocol;
                t.Name = deviceType;
                type = t;
            }
            else
            {
                type = new GXDeviceType();
                type.Protocol = protocol;
                type.Name = deviceType;
            }
            
            string DeviceFilePath;
            string protocolPath;
            if (string.IsNullOrEmpty(target))
            {
                if (preset)
                {
                    DeviceFilePath = GXDevice.GetDeviceTemplatePath(deviceGuid);
                }
                else
                {
                    DeviceFilePath = GXDevice.GetDeviceTemplatePath(protocol, deviceType);
                }               
                protocolPath = Path.GetDirectoryName(DeviceFilePath);
            }
            else
            {
                if (string.IsNullOrEmpty(Path.GetFileName(target)))
                {
                    DeviceFilePath = target + Path.GetFileName(DevicePath);
                    protocolPath = Path.GetDirectoryName(target);
                }
                else
                {
                    DeviceFilePath = target;
                    protocolPath = Path.GetDirectoryName(DeviceFilePath);
                    target = null;
                }
            }
            if (!string.IsNullOrEmpty(protocolPath) && !Directory.Exists(protocolPath))
            {
                Directory.CreateDirectory(protocolPath);
            }
                       
            //Ask to overwrite if exists
            if (File.Exists(DeviceFilePath))
            {
				DialogResult retval = GXCommon.ShowQuestion(parent, "Do you want to replace existing device?");
                if (retval != DialogResult.Yes)
                {
                    throw new Exception("Device installation failed");
                }
            }
            bool restart = false;
            GXDevice Device = null;            
            //Copy to the final directory
            foreach (string FilePath in FilePaths)
            {
                FileInfo info = new FileInfo(FilePath);
                string FileName = info.Name;                
                if (string.Compare(info.Extension, ".gxt", true) == 0)
                {
                    if (!string.IsNullOrEmpty(Path.GetFileName(target)))
                    {
                        FileName = Path.GetFileName(target);
                    }
                    string tmp = Path.Combine(protocolPath, FileName);
                    File.Copy(FilePath, tmp, true);
                    Gurux.Common.GXFileSystemSecurity.UpdateFileSecurity(tmp);
                }
                else if (string.Compare(info.Extension, ".dll", true) == 0) // AddIn protocol file for GXDeviceEditor
                {                    
                    string targetPath;
                    if (string.IsNullOrEmpty(target))
                    {
                        targetPath = Path.Combine(GXCommon.ProtocolAddInsPath, Path.GetFileName(FilePath));
                    }
                    else
                    {
                        targetPath = Path.Combine(Path.GetDirectoryName(target), Path.GetFileName(FilePath));
                    } 
                    string existsVersion = "0.0.0.0", newVersion = "0.0.0.0";
                    if (File.Exists(targetPath))
                    {
                        System.Diagnostics.FileVersionInfo vInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(targetPath);
						existsVersion = vInfo.FileVersion;
                    }
                    System.Diagnostics.FileVersionInfo vInfo2 = System.Diagnostics.FileVersionInfo.GetVersionInfo(FilePath);
                    newVersion = vInfo2.FileVersion;
					if (IsGreaterVersion(newVersion, existsVersion))
                    {
                        string DllPath = Directory.GetParent(targetPath).FullName;
                        if (!Directory.Exists(DllPath))
                        {
                            Directory.CreateDirectory(DllPath);
                        }                        
                        try
                        {
                            File.Copy(FilePath, targetPath, true);
                            Gurux.Common.GXFileSystemSecurity.UpdateFileSecurity(targetPath);
                        }
                        //If file is in use.
                        catch (System.IO.IOException)
                        {
                            DllPath = Path.Combine(DllPath, "cached");
                            if (!Directory.Exists(DllPath))
                            {
                                Directory.CreateDirectory(DllPath);
                            }
                            File.Copy(FilePath, Path.Combine(DllPath, Path.GetFileName(targetPath)), true);
                            restart = true;
                        }
                    }
                }
                else
                {
                    throw new Exception(string.Format("Unknown data in exported file. {0}", FileName));
                }
            }

            //Delete temp files
            Directory.Delete(TempPath, true);
            //Register device
            if (string.IsNullOrEmpty(target))
            {
                GXDeviceList.Update();
                Device = GXDevice.Load(DeviceFilePath);
                if (preset)
                {
                    Device.PresetName = Path.GetFileNameWithoutExtension(target);
                }
                if (!preset)
                {
                    Device.Register();
                }
                if (Device.m_AddIn != null)
                {
                    Device.m_AddIn.InitializeAfterImport(Device);
                }
                if (restart)
                {
                    return null;
                }
                /*
                GXDeviceTypeCollection types = Gurux.Device.GXDeviceList.GetDeviceTypes(preset, protocol);
                return types[Device.DeviceType];
                 * */
            }
            return type;
        }

		private static void WriteFile(ZipInputStream s, string FileName)
		{
			if (!Directory.Exists(Path.GetDirectoryName(FileName)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(FileName));
			}
			using (Stream stream = File.OpenWrite(FileName))
			{
				BinaryWriter b = new BinaryWriter(stream);
				int size = 1024;
                byte[] data = new byte[size]; //1KB buffer
                while (true)
				{
					size = s.Read(data, 0, size);
					if (size > 0)
					{
						b.Write(data, 0, size);
					}
					else
					{
						b.Close();
						break;
					}
				}
			}
		}

        /// <summary>
        /// Load AddIns to own namespace.
        /// </summary>
        class GXProxyClass : MarshalByRefObject
        {
            public List<string> Assemblies = new List<string>();
            string TargetDirectory;

            public void FindAddInReferences(string assemblyName)
            {
                TargetDirectory = Path.GetDirectoryName(assemblyName);
                AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
                Assembly assembly = Assembly.LoadFile(assemblyName);                
                foreach (Type type in assembly.GetTypes())
                {
                    //Do not remove.
                    //Loop all types to load them.                    
                }               
                AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }

            System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
            {
                foreach (string it in Directory.GetFiles(TargetDirectory, "*.dll"))
                {
                    Assembly asm = Assembly.LoadFile(it);
                    if (asm.GetName().ToString() == args.Name)
                    {
                        Assemblies.Add(it);
                        return asm;
                    }
                }
                return null;
            }
        }

		/// <summary>
        /// Exports a device from GXDeviceEditor to a .gxz file. 
		/// </summary>
		/// <param name="device">Name of the exported device file.</param>
		/// <param name="path">Path to the file, where to export.</param>
        public static void Export(GXDevice device, string path)
        {
            device.Save(path);
            List<string> filenames = new List<string>();
            filenames.Add(device.DeviceTemplatePath);
            string DevFolder = System.IO.Path.GetDirectoryName(device.DeviceTemplatePath);
            if (!Directory.Exists(DevFolder))
            {
                Directory.CreateDirectory(DevFolder);
            }

            using (ZipOutputStream s = new ZipOutputStream(File.Create(path)))
            {
                s.SetLevel(9); // 0 - store only to 9 - means best compression
                //Save device files
                foreach (string file in filenames)
                {
                    using (FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        byte[] buffer = new byte[stream.Length];
                        stream.Read(buffer, 0, buffer.Length);
                        FileInfo fi = new FileInfo(file);
                        ZipEntry entry = new ZipEntry(fi.Name);
                        s.PutNextEntry(entry);
                        s.Write(buffer, 0, buffer.Length);
                    }
                }

                //Save Dll file
                string DLLFile = device.m_AddIn.GetType().Assembly.Location;
                FileStream DLLFS = File.OpenRead(DLLFile);
                byte[] DLLBuffer = new byte[DLLFS.Length];
                DLLFS.Read(DLLBuffer, 0, DLLBuffer.Length);
                FileInfo fi2 = new FileInfo(DLLFile);
                ZipEntry DLLEntry = new ZipEntry(fi2.Name);
                s.PutNextEntry(DLLEntry);
                s.Write(DLLBuffer, 0, DLLBuffer.Length);
                //////////////////////////////////
                //Add referenced assemblies from the same directory
                // Create an Application Domain:
                string pathToDll = typeof(GXZip).Assembly.CodeBase;
                AppDomainSetup domainSetup = new AppDomainSetup { PrivateBinPath = pathToDll };
                System.AppDomain td = AppDomain.CreateDomain("AddInReferencesDomain", null, domainSetup);
                try
                {
                    GXProxyClass pc = (GXProxyClass)(td.CreateInstanceFromAndUnwrap(pathToDll, typeof(GXProxyClass).FullName));
                    path = string.Empty;
                    List<string> medias = new List<string>();
                    pc.FindAddInReferences(DLLFile);

                    foreach (string asmPath in pc.Assemblies)
                    {
                        Assembly asm = Assembly.LoadFile(asmPath);
                        //Save Dll file
                        DLLFile = asm.Location;
                        DLLFS = File.OpenRead(DLLFile);
                        DLLBuffer = new byte[DLLFS.Length];
                        DLLFS.Read(DLLBuffer, 0, DLLBuffer.Length);
                        fi2 = new FileInfo(DLLFile);
                        DLLEntry = new ZipEntry(fi2.Name);
                        s.PutNextEntry(DLLEntry);
                        s.Write(DLLBuffer, 0, DLLBuffer.Length);
                    }
                }
                finally
                {
                    System.AppDomain.Unload(td);
                }                
            }
        }

		/// <summary>
		/// Checks if the new version number is bigger than the current version number.
		/// </summary>
		/// <param name="version1">Current version number.</param>
		/// <param name="version2">Version number compared to current one.</param>
		/// <returns>True if Version1 number is bigger than Version2 number.</returns>
		private static bool IsGreaterVersion(string version1, string version2)
		{
			try
			{
				string[] Ver1 = version1.Split('.');
				string[] Ver2 = version2.Split('.');

				for (int i = 0; i < Ver1.Length; i++)
				{
					if (Convert.ToInt32(Ver1[i]) > Convert.ToInt32(Ver2[i]))
					{
						return true;
					}
				}
				return false;
			}
			catch (Exception Ex)
			{
				throw new Exception("Version comparisation failed:\r\n" + Ex.Message);
			}
		}
	}
}
