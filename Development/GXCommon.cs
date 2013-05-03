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
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using Microsoft.Win32;
using System.Xml;
using System.IO;
using System.Security.Principal;
using System.Security.AccessControl;
using Gurux.Communication;

namespace Gurux.Device
{
	/// <summary>
	/// Common Gurux helpers. 
	/// </summary>
	public class GXCommon
	{
		/// <summary>
		/// Converts string to byte[].
		/// format: AB ba 01 1
		/// </summary>
		/// <param name="hexString">Hex string to convert.</param>
		/// <returns>Byte array.</returns>
		public static byte[] StringToByteArray(string hexString)
		{
            if (string.IsNullOrEmpty(hexString))
            {
                return null;
            }
			string[] splitted = hexString.Split(' ');
			byte[] retVal = new byte[splitted.Length];
			int i = -1;
			foreach (string hexStr in splitted)
			{
				retVal[++i] = Convert.ToByte(hexStr, 16);
			}
			return retVal;
		}

		/// <summary>
		/// Converts data to hex string.
		/// </summary>
		/// <param name="data">Data to convert.</param>
		/// <returns>Hex string.</returns>
		public static string ToHexString(object data)
		{
			string hex = string.Empty;
			if (data is Array)
			{
				Array arr = (Array)data;
				for (long pos = 0; pos != arr.Length; ++pos)
				{
					long val = Convert.ToInt64(arr.GetValue(pos));
					hex += Convert.ToString(val, 16) + " ";
				}
				return hex.TrimEnd();
			}
			hex = Convert.ToString(Convert.ToInt64(data), 16);
			return hex;
		}

		/// <summary>
		/// Compares two byte or byte array values.
		/// </summary>
		public static bool EqualBytes(object a, object b)
		{
			if (a == null)
			{
				return b == null;
			}
			if (b == null)
			{
				return a == null;
			}
			if (a is Array && b is Array)
			{
				int pos = 0;
				if (((Array)a).Length != ((Array)b).Length)
				{
					return false;
				}
				foreach (byte mIt in (byte[])a)
				{
					if ((((byte)((byte[])b).GetValue(pos++)) & mIt) != mIt)
					{
						return false;
					}
				}
			}
			else
			{
				return BitConverter.Equals(a, b);
			}
			return true;
		}

		/// <summary>
        /// Sends given message to window(s).  
		/// </summary>
        /// <param name="hWnd">Handle of the window, to whom the message is sent.</param>
        /// <param name="msg">Message to send.</param>
        /// <param name="wParam">Additional message specific information.</param>
        /// <param name="lParam">Additional message specific information.</param>
        /// <returns>The return value depends on the message sent.</returns>
		[DllImport("user32.dll")]
		public static extern bool SendMessage(IntPtr hWnd, int msg, Int32 wParam, Int32 lParam);
		const int LVSCW_AUTOSIZE = -1;
		const int LVSCW_AUTOSIZE_USEHEADER = -2;
		const int LVM_FIRST = 0x1000;      // ListView messages
		const int LVM_SETCOLUMNWIDTH = (LVM_FIRST + 30);
		/// <summary>
		/// Title of messagebox
		/// </summary>
		public static string Title = "";
		/// <summary>
		/// Parent window of messagebox
		/// </summary>
		public static IWin32Window Owner = null;

		/// <summary>
        /// Automatically resizes the column to fit the longest line in the collection of list items.
		/// </summary>
        /// <param name="list">The collection of list items.</param>
		public static void AutosizeListView(ListView list)
		{
			//this may fail some times. It is OK.
			if (list.Columns.Count > 0)
			{
				SendMessage(list.Handle, LVM_SETCOLUMNWIDTH, list.Columns.Count - 1, LVSCW_AUTOSIZE_USEHEADER);
			}
		}

		/// <summary>
		/// This must be called, if components are used.
		/// </summary>
		public static void CollectGarbage()
		{
			//One time just doesn't clear everything.
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}

		/// <summary>
		/// Shows an error message.
		/// </summary>
		public static void ShowError(Exception ex)
		{
			ShowError(Owner, ex);
		}

		/// <summary>
        /// Retrieves the path to application data.
		/// </summary>
		public static string ApplicationDataPath
		{
			get
			{
				string path = string.Empty;
				if (Environment.OSVersion.Platform == PlatformID.Unix)
				{					
					path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
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
				}
				return path;
			}
		}

        /// <summary>
        /// If we are runnign program from debugger, all protocol Add-Ins are loaded from child "Protocols"- directory. 
        /// </summary>
		public static string ProtocolAddInsPath
		{
			get
			{
				string strPath = "";
				if (Environment.OSVersion.Platform == PlatformID.Unix)
				{					
					strPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
					strPath = Path.Combine(strPath, ".Gurux");
				}
				else
				{				
	                if (Environment.OSVersion.Version.Major < 6)
					{
						strPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles);
					}
					else
					{
						strPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
					}
					strPath = Path.Combine(strPath, "Gurux");
				}                
                strPath = Path.Combine(strPath, "AddIns");
				return strPath;
			}
		}

		/// <summary>
		/// Retrieves application data path from environment variables.
		/// </summary>
		public static string UserDataPath
		{
			get
			{
				string path;
				if (System.Environment.OSVersion.Platform == PlatformID.Unix)
				{
					path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				}
				else
				{
					path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
				}					
				return path;
			}
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		private static extern IntPtr GetActiveWindow();

		/// <summary>
		/// Shows an error message.
		/// </summary>
		public static void ShowError(IWin32Window parent, Exception ex)
		{
			try
			{
				while (ex.InnerException != null)
				{
					ex = ex.InnerException;
				}

				if (ex.StackTrace != null)
				{
					System.Diagnostics.Debug.Write(ex.StackTrace.ToString());
				}
                string path = ApplicationDataPath;
				if (System.Environment.OSVersion.Platform == PlatformID.Unix)
				{
					path = Path.Combine (path, ".Gurux");
				}
				else
				{
					path = Path.Combine (path, "Gurux");
				}				
                path = Path.Combine(path, "LastError.txt");
                System.IO.TextWriter tw = System.IO.File.CreateText(path);
				tw.Write(ex.ToString());
                if (ex.StackTrace != null)
                {
                    tw.Write("----------------------------------------------------------\r\n");
                    tw.Write(ex.StackTrace.ToString());
                }
				tw.Close();
                if (parent != null && !((Control)parent).IsDisposed && !((Control)parent).Created && !((Control)parent).InvokeRequired)
				{
					MessageBox.Show(parent, ex.Message, Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				else
				{
					MessageBox.Show(ex.Message, Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			catch
			{
				//Do nothing. Fatal exception blew up messagebox.
			}
		}

		/// <summary>
		/// Shows an error question dialog.
		/// </summary>
		public static DialogResult ShowQuestion(string str)
		{
			return ShowQuestion(Owner, str);
		}

		/// <summary>
		/// Shows an error question dialog.
		/// </summary>
		public static DialogResult ShowQuestion(IWin32Window parent, string str)
		{
			try
			{
				//Mono returns false in Environment.UserInteractive.
				if (System.Environment.OSVersion.Platform == PlatformID.Unix || Environment.UserInteractive)
				{
					if (parent != null)
					{
						return MessageBox.Show(parent, str, Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
					}
					else if (Owner != null)
					{
						return MessageBox.Show(Owner, str, Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
					}
					else
					{
						return MessageBox.Show(str, Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
					}
				}
				else
				{
					return DialogResult.Yes;
				}
			}
			catch
			{
				//Do nothing. Fatal exception blew up messagebox.
				return DialogResult.Abort;
			}
		}

		/// <summary>
		/// Shows an error exclamation dialog.
		/// </summary>
		public static DialogResult ShowExclamation(string str)
		{
			return ShowExclamation(Owner, str);
		}

		/// <summary>
		/// Shows an error exclamation dialog.
		/// </summary>
		public static DialogResult ShowExclamation(IWin32Window parent, string str)
		{
			try
			{
				if (parent != null)
				{
					return MessageBox.Show(parent, str, Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
				}
				else
				{
					return MessageBox.Show(str, Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
				}
			}
			catch
			{
				//Do nothing. Fatal exception blew up messagebox.
				return DialogResult.Abort;
			}
		}

		/// <summary>
		/// Removes case sensitivity of given string.
		/// </summary>
        /// <param name="original">Original string.</param>
        /// <param name="pattern">String to replace.</param>
        /// <param name="replacement">Replacing string.</param>
        /// <returns>The replaced string.</returns>
		public static string ReplaceEx(string original, string pattern, string replacement)
		{
			int count, position0, position1;
			count = position0 = position1 = 0;
			string upperString = original.ToUpper();
			string upperPattern = pattern.ToUpper();
			int inc = (original.Length / pattern.Length) *
				(replacement.Length - pattern.Length);
			char[] chars = new char[original.Length + Math.Max(0, inc)];
			while ((position1 = upperString.IndexOf(upperPattern,
				position0)) != -1)
			{
				for (int i = position0; i < position1; ++i)
					chars[count++] = original[i];
				for (int i = 0; i < replacement.Length; ++i)
					chars[count++] = replacement[i];
				position0 = position1 + pattern.Length;
			}
			if (position0 == 0) return original;
			for (int i = position0; i < original.Length; ++i)
				chars[count++] = original[i];
			return new string(chars, 0, count);
		}

		/// <summary>
		/// Sets column widths of a list view control, according to registry values.
		/// </summary>
		/// <param name="regKeyPath">Registry path in CurrentUsers where the column widths are.</param>
		/// <param name="targetList">List view control to be modified.</param>
		public static void LoadListViewWidthsFromRegistry(string regKeyPath, ListView targetList)
		{
			try
			{
				RegistryKey RegKey = Registry.CurrentUser.OpenSubKey(regKeyPath);
				if (RegKey != null)
				{
					for (int i = 0; i < targetList.Columns.Count; ++i)
					{
						try
						{
							object Value = RegKey.GetValue("c" + i.ToString());
							if (Value != null)
							{
								targetList.Columns[i].Width = Convert.ToInt32(Value);
							}
						}
						catch
						{
							//Column count doesn't match
						}
					}
				}
			}
			catch (Exception Ex)
			{
				ShowError(Ex);
			}
		}

		/// <summary>
		/// Saves column widths of a list view control to registry.
		/// </summary>
		/// <param name="regKeyPath">Registry key path in CurrentUser where the column widths are put.</param>
		/// <param name="sourceList">The list view control to be used as source.</param>
		public static void SaveListViewWidthsToRegistry(string regKeyPath, ListView sourceList)
		{
			try
			{
				RegistryKey RegKey = Registry.CurrentUser.CreateSubKey(regKeyPath);
				for (int i = 0; i < sourceList.Columns.Count; ++i)
				{
					RegKey.SetValue("c" + i.ToString(), sourceList.Columns[i].Width);
				}
			}
			catch (Exception Ex)
			{
				ShowError(Ex);
			}
		}		
	}
}
