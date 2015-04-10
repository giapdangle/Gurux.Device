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
using System.Drawing;
using System.Windows.Forms;
using Gurux.Common;
using Gurux.Device.PresetDevices;
using Gurux.Common.JSon;

namespace Gurux.Device
{        
    /// <summary>
    /// Show all available device profiles where user can select profile that he wants to use.
    /// </summary>
    partial class GXDeviceProfileDlg : Form
    {
        /// <summary>
        /// Is target device or device profile.
        /// </summary>
        bool Device;

        GXSelectDeviceProfileEventArgs Arguments;

        Gurux.Common.GXAsyncWork Work;
        GXDeviceManufacturerCollection items = new GXDeviceManufacturerCollection();

        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDeviceProfileDlg(bool device, GXSelectDeviceProfileEventArgs arguments)
        {            
            InitializeComponent();
            Device = device;
            Arguments = arguments;
            Bitmap bm = PublisherImage.BackgroundImage as Bitmap;
            bm.MakeTransparent();
            PublisherImage.BackgroundImage = bm;
            CustomCB.Enabled = DownloadCB.Enabled = ShowEarlierVersionsCB.Enabled = OKBtn.Enabled = Arguments.Edit;
            SearchTB.ReadOnly = !Arguments.Edit;
            //Add search to device profile text box.
            bm = Gurux.Device.Properties.Resources.Search;
            bm.MakeTransparent();
            PictureBox pic = new PictureBox();
            pic.BackgroundImage = bm;
            pic.Width = pic.BackgroundImage.Width;
            pic.Height = pic.BackgroundImage.Height;
            pic.Dock = DockStyle.Right;
            pic.Cursor = Cursors.Arrow;
            pic.Click += new EventHandler(ShowProfiles);
            SearchTB.Controls.Add(pic);            
        }

        /// <summary>
        /// Show device profiles.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ShowProfiles(object sender, EventArgs e)
        {
            ShowTemplates();
        }

        /// <summary>
        /// Get showed device profiles.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeviceDlg_Load(object sender, EventArgs e)
        {
            if (Arguments.OnInitialize != null)
            {
                Arguments.OnInitialize(this, Arguments);
            }
            if (Arguments.DeviceProfiles.Count != 0)
            {
                ShowEarlierVersionsCB.Enabled = DownloadCB.Enabled = CustomCB.Enabled = false;
                if (Arguments.DeviceProfiles.Count == 0)
                {
                    throw new Exception(Gurux.Device.Properties.Resources.NoDevicesPublishedTxt);
                }
            }
            else
            {
                GXDeviceManufacturerCollection.Load(items);
                if (GXDeviceList.DeviceProfiles.Count == 0 && items.Count == 0)
                {
                    throw new Exception(Gurux.Device.Properties.Resources.NoDevicesPublishedTxt);
                }
            }
            if (!string.IsNullOrEmpty(Arguments.Settings))
            {
                try
                {
                    GXJsonParser parser = new GXJsonParser();
                    GXDeviceProfileFormSettings settings = parser.Deserialize<GXDeviceProfileFormSettings>(Arguments.Settings);
                    CustomCB.Checked = settings.Custom;
                    DownloadCB.Checked = settings.Download;
                    SearchTB.Text = settings.SearchText;
                    ShowEarlierVersionsCB.Checked = settings.Earlier;

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    Arguments.Settings = null;
                }
            }
            ShowTemplates();
            if (DeviceProfiles.SelectedItems.Count == 0)
            {
                SearchTB.Select();
            }
        }

        void AddProfile(GXDeviceProfile it, string searchText)
        {
            if (Arguments.Edit || !(Arguments.Target is GXDevice))
            {
                if (searchText == null || it.Name.ToLower().Contains(searchText) || 
                    it.Protocol.ToLower().Contains(searchText) ||
                    (it.DeviceManufacturer != null && it.DeviceManufacturer.ToLower().Contains(searchText)) ||
                    (it.DeviceModel != null && it.DeviceModel.ToLower().Contains(searchText)) ||
                    (it.DeviceVersion != null && it.DeviceVersion.ToLower().Contains(searchText)))
                {

                    ListViewItem li = new ListViewItem(new string[] { it.Name, it.Protocol, 
                        it.DeviceManufacturer, it.DeviceModel, it.DeviceVersion, it.VersionToString() });
                    li.Tag = it;
                    DeviceProfiles.Items.Add(li);
                    if (Arguments.Target != null && (Arguments.Target as GXDeviceProfile).ProfileGuid == it.ProfileGuid)
                    {
                        li.Selected = true;
                    }
                }
            }
            else if ((Arguments.Target as GXDevice).Guid == it.Guid)
            {
                //If edit is not allowed only selected device is shown.
                ListViewItem li = new ListViewItem(new string[] { it.Name, it.Protocol, 
                        it.DeviceManufacturer, it.DeviceModel, it.DeviceVersion, it.VersionToString() });
                li.Tag = it;
                DeviceProfiles.Items.Add(li);
                li.Selected = true;
                return;
            }
        }

        void ShowTemplates()
        {
            DeviceProfiles.Items.Clear();
            string searchText = null;
            if (SearchTB.Text.Length != 0)
            {
                searchText = SearchTB.Text.ToLower();
            }
            if (Arguments.DeviceProfiles.Count != 0)
            {
                foreach (GXDeviceProfile it in Arguments.DeviceProfiles)
                {
                    AddProfile(it, searchText);
                }
            }
            else
            {
                //Get custom device profiles.
                if (CustomCB.Checked)
                {
                    foreach (GXDeviceProfile it in GXDeviceList.DeviceProfiles)
                    {
                        AddProfile(it, searchText);
                    }
                }

                //Get published device profiles.
                if (DownloadCB.Checked)
                {
                    foreach (GXDeviceManufacturer man in items)
                    {
                        foreach (GXDeviceModel model in man.Models)
                        {
                            foreach (GXDeviceVersion version in model.Versions)
                            {
                                foreach (GXDeviceProfile it in version.Profiles)
                                {
                                    AddProfile(it, searchText);
                                }
                            }
                        }
                    }
                }
            }    
        }
        /*
        void Download()
        {
             TODO:
            string "http://www.gurux.org/GuruxDevicePublisher";
            HttpWebRequest request = WebRequest.Create("http:/www.gurux.org/") as HttpWebRequest;
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception(String.Format(
                    "Server error (HTTP {0}: {1}).",
                    response.StatusCode,
                    response.StatusDescription));
                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Response));
                object objResponse = jsonSerializer.ReadObject(response.GetResponseStream());
                Response jsonResponse
                = objResponse as Response;
                return jsonResponse;
            }

            
            JsonServiceClient Client = new JsonServiceClient(Gurux.DeviceSuite.Properties.Settings.Default.UpdateServer);
            GXDownloadRequest download = new GXDownloadRequest();
            download.Template = target as GXDeviceProfileVersion;
            GXDownloadResponse ret = Client.Get(download);
            GXDeviceProfile type = UpdatePublishedDeviceType(download.Template, ret.Data);
            (target as GXDeviceProfileVersion).Status = DownloadStates.Installed;
            //If restart is needed.
            if (type == null)
            {
                return Guid.Empty;
            }
             
        }
         * */

        void OnAsyncStateChange(object sender, GXAsyncWork work, object[] parameters, AsyncState state, string text)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new AsyncStateChangeEventHandler(this.OnAsyncStateChange), sender, work, state, text);
            }
            else
            {
                bool start = state == AsyncState.Start;
                progressBar1.Visible = start;
                panel2.Enabled = DeviceProfiles.Enabled = OKBtn.Enabled = !start;
                if (state == AsyncState.Start)
                {                    
                    progressBar1.Value = 0;
                    progressBar1.Maximum = 10;
                }
                else if (state == AsyncState.Finish)
                {
                    if (parameters != null)
                    {
                        GXDeviceProfile dp = parameters[0] as GXDeviceProfile;
                        ApplyChanges(dp);
                    }
                    else
                    {
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                }
                if (start)
                {
                    ProgresssTimer.Start();
                }
                else
                {
                    ProgresssTimer.Stop();
                }
            }
        }

        void Download(object sender, GXAsyncWork work, object[] parameters)
        {
            //TODO:
            GXDeviceProfile dp = parameters[0] as GXDeviceProfile;
            GXJsonClient parser = new GXJsonClient(Arguments.DownloadServer, Arguments.UserName, Arguments.Password);
            GXDownloadRequest req = new GXDownloadRequest(dp);
            GXDownloadResponse res = parser.Post(req);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //If user try to find new device.
            if (keyData == Keys.Enter && this.AcceptButton == null)
            {
                ShowTemplates();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Accept changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OKBtn_Click(object sender, EventArgs e)
        {
            try
            {                
                if (DeviceProfiles.SelectedItems.Count != 1)
                {
                    throw new Exception("No device profile is selected.");
                }     
                GXDeviceProfile dp = DeviceProfiles.SelectedItems[0].Tag as GXDeviceProfile;
                //Start thread and load protocol if protocol is not loaded yet.
                if (!GXDeviceList.Protocols.ContainsKey(dp.Protocol))
                {
                    Work = new GXAsyncWork(this, OnAsyncStateChange, Download, null, "Downloading", new object[]{dp});
                    Work.Start();
                    return;
                }
                ApplyChanges(dp);
            }
            catch (Exception ex)
            {
                DialogResult = DialogResult.None;
                Gurux.Common.GXCommon.ShowError(this, ex);
            }
        }

        void OnSelect(object sender, GXAsyncWork work, object[] parameters)
        {
            Arguments.OnSelect(Arguments.Target, Arguments);
        }

        void ApplyChanges(GXDeviceProfile dp)
        {
            if (Device)
            {
                Arguments.Target = GXDevice.Create(dp.Protocol, dp.Name, "");
            }
            else
            {
                Arguments.Target = dp;
            }
            try
            {
                GXJsonParser parser = new GXJsonParser();
                GXDeviceProfileFormSettings settings = new GXDeviceProfileFormSettings();
                settings.Custom = CustomCB.Checked;
                settings.Download = DownloadCB.Checked;
                settings.SearchText = SearchTB.Text;
                settings.Earlier = ShowEarlierVersionsCB.Checked;
                Arguments.Settings = parser.Serialize(settings);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            if (Arguments.OnSelect != null)
            {
                if (Arguments.UseThread)
                {
                    DialogResult = DialogResult.None;
                    Work = new GXAsyncWork(this, OnAsyncStateChange, OnSelect, null, "Selecting", null);
                    Work.Start();
                    return;
                }
                else
                {
                    Arguments.OnSelect(Arguments.Target, Arguments);
                }
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void DeviceProfiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (DeviceProfiles.SelectedItems.Count == 1)
            {
                OKBtn_Click(null, null);
            }
        }

        /// <summary>
        /// Show device templates.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowTemplates(object sender, EventArgs e)
        {
            ShowTemplates();
        }

        /// <summary>
        /// Cancel download if running.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelBtn_Click(object sender, EventArgs e)
        {
            if (Work != null)
            {
                Work.Cancel();
            }
        }

        /// <summary>
        /// Remove selected items.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveMenu_Click(object sender, EventArgs e)
        {
            try
            {
                if (DeviceProfiles.SelectedItems.Count != 0)
                {
                    DialogResult dr = GXCommon.ShowQuestion(this, Gurux.Device.Properties.Resources.DoYouWantToRemoveTxt);
                    if (dr != DialogResult.Yes)
                    {
                        return;
                    }
                    while (DeviceProfiles.SelectedItems.Count != 0)
                    {
                        ListViewItem li = DeviceProfiles.SelectedItems[0];
                        GXDeviceProfile dp = (GXDeviceProfile)li.Tag;
                        GXDevice.Unregister(dp);
                        DeviceProfiles.Items.Remove(li);
                    }
                }
            }
            catch (Exception Ex)
            {
                GXCommon.ShowError(this, Ex);
            }
        }

        private void SearchTB_Enter(object sender, EventArgs e)
        {
            this.AcceptButton = null;
        }

        private void SearchTB_Leave(object sender, EventArgs e)
        {
            this.AcceptButton = OKBtn;
        }

        /// <summary>
        /// Update new progress bar value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProgresssTimer_Tick(object sender, EventArgs e)
        {
            progressBar1.Value = (progressBar1.Value + 1) % 10;
        }
        
    }
}
