using System;
using System.Data;
using System.Windows.Forms;
using DACrux.SEMDMS.RO;
using Infragistics.Win.Misc;
using System.Collections.Generic;
using DACrux.Common.RO;

namespace DACrux.SEMDMS.ENGUI
{
    public partial class frmConfiguration : DACrux.Framework.Base.DACruxUXBasic01
    {
        private enum ConfigTabOption { INTERFACE = 0, MAP_OPTION, PCM_OPTION, CP_OPTION, FOI_OPTION }
        private enum ConfigCategory { WAFER_OPTION = 0, TEST_OPTION, DEFECT_OPTION, WAFER_DIE_COLOR, TEST_OPTION_ENABLE, WAFER_OPTION_ENABLE, VIRTUAL_OPTION, WAFER_OPTION_M, TEST_OPTION_M, TEST_OPTION_M_ENABLE, WAFER_OPTION_M_ENABLE, WAFER_OPTION_IMAGE,  WAFER_IMAGE_ENABLE}
        private enum ConfigType { STRING = 0, NUMBER, INTEGER, HEXA }
        enum Col { Factory, Category, Name, Value, Type, UserID, Ordered, CreateUser, Comment };

        public frmConfiguration(
            )
        {
            InitializeComponent();
        }


        #region [ Event Hadler ]

        private void frmConfiguration_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            utControl.Tabs.Remove(utControl.Tabs[0]);
            DisplayInterfaceData();
        }

        //--

        private void btnDmOptionMoved_Click(
            object sender,
            EventArgs e
            )
        {
            int index = -1;
            UltraButton btn = sender as UltraButton;
            if (btn == btnToLeft)
            {
                foreach (ListViewItem item in lvSelectedDM.SelectedItems)
                {
                    lvSelectedDM.Items.RemoveAt(item.Index);
                    lvAllDM.Items.Add(item);
                }
                lvAllDM.Sorting = SortOrder.Ascending;
                lvAllDM.Sort();
            }
            else if (btn == btnToRight)
            {
                foreach (ListViewItem item in lvAllDM.SelectedItems)
                {
                    lvAllDM.Items.RemoveAt(item.Index);
                    lvSelectedDM.Items.Add(item);
                }
            }
            else if (btn == btnSeletedItemUp)
            {
                lvSelectedDM.BeginUpdate();
                foreach (ListViewItem item in lvSelectedDM.SelectedItems)
                {
                    if (item.Index > 0)
                    {
                        index = item.Index - 1;
                        lvSelectedDM.Items.RemoveAt(item.Index);
                        lvSelectedDM.Items.Insert(index, item);
                    }
                }
                lvSelectedDM.EndUpdate();
            }
            else if (btn == btnSeletedItemDown)
            {
                lvSelectedDM.BeginUpdate();
                foreach (ListViewItem item in lvSelectedDM.SelectedItems)
                {
                    if (item.Index < lvSelectedDM.Items.Count - 1)
                    {
                        index = item.Index + 1;
                        lvSelectedDM.Items.RemoveAt(item.Index);
                        lvSelectedDM.Items.Insert(index, item);
                    }
                }
                lvSelectedDM.EndUpdate();
            }

            //--

            SetDmOptionImageChangedByLVSelectedControl();
        }

        private void btnTESTOptionMoved_Click(
            object sender,
            EventArgs e
            )
        {
            int index = -1;
            UltraButton btn = sender as UltraButton;
            if (btn == btnToLeftTEST)
            {
                foreach (ListViewItem item in lvSelectedTEST.SelectedItems)
                {
                    lvSelectedTEST.Items.RemoveAt(item.Index);
                    lvAllTEST.Items.Add(item);
                }
                lvAllTEST.Sorting = SortOrder.Ascending;
                lvAllTEST.Sort();
            }
            else if (btn == btnToRightTEST)
            {
                foreach (ListViewItem item in lvAllTEST.SelectedItems)
                {
                    lvAllTEST.Items.RemoveAt(item.Index);
                    lvSelectedTEST.Items.Add(item);
                }
            }
            else if (btn == btnSeletedItemUpTEST)
            {
                lvSelectedTEST.BeginUpdate();
                foreach (ListViewItem item in lvSelectedTEST.SelectedItems)
                {
                    if (item.Index > 0)
                    {
                        index = item.Index - 1;
                        lvSelectedTEST.Items.RemoveAt(item.Index);
                        lvSelectedTEST.Items.Insert(index, item);
                    }
                }
                lvSelectedTEST.EndUpdate();
            }
            else if (btn == btnSeletedItemDownTEST)
            {
                lvSelectedTEST.BeginUpdate();
                foreach (ListViewItem item in lvSelectedTEST.SelectedItems)
                {
                    if (item.Index < lvSelectedTEST.Items.Count - 1)
                    {
                        index = item.Index + 1;
                        lvSelectedTEST.Items.RemoveAt(item.Index);
                        lvSelectedTEST.Items.Insert(index, item);
                    }
                }
                lvSelectedTEST.EndUpdate();
            }

            //--

            SetDmOptionImageChangedByLVSelectedControl();
        }

        private void butOK_Click(
            object sender,
            EventArgs e
            )
        {
            try
            {
                if (String.Equals(utControl.SelectedTab.Key, Enum.GetName(typeof(ConfigTabOption), ConfigTabOption.INTERFACE)))
                {
                    UpdateInterfaceData();
                    DisplayInterfaceData();
                }
                else if (String.Equals(utControl.SelectedTab.Key, Enum.GetName(typeof(ConfigTabOption), ConfigTabOption.MAP_OPTION)))
                {
                    UpdateDmOptionData();
                    DisplayDmOptionData();
                }

                DspMessage("Save completed.");
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        //--

        private void butCancel_Click(
            object sender,
            EventArgs e
            )
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        //--

        private void upbDieColor_Click(
            object sender,
            EventArgs e
            )
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = upbDieColor.BackColor;
            if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                upbDieColor.BackColor = dlg.Color;
        }

        private void upbInspectColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = upbInspectColor.BackColor;
            if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                upbInspectColor.BackColor = dlg.Color;
        }

        private void upbMapLineColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = upbMapLineColor.BackColor;
            if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                upbMapLineColor.BackColor = dlg.Color;
        }

        private void upbDefectDieColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = upbDefectDieColor.BackColor;
            if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                upbDefectDieColor.BackColor = dlg.Color;
        }

        private void upbTestDieColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = upbTestDieColor.BackColor;
            if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                upbTestDieColor.BackColor = dlg.Color;
        }

        private void upbTestMapLineColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = upbTestMapLineColor.BackColor;
            if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                upbTestMapLineColor.BackColor = dlg.Color;
        }


        private void upbVirtureColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = upbVirtureColor.BackColor;
            if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                upbVirtureColor.BackColor = dlg.Color;
        }

        //--

        private void utControl_SelectedTabChanged(
            object sender,
            Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e
            )
        {
            if (e.Tab == null)
                return;

            ConfigTabOption value = (ConfigTabOption)Enum.Parse(typeof(ConfigTabOption), e.Tab.Key);
            switch (value)
            {
                case ConfigTabOption.MAP_OPTION:
                    DisplayDmOptionData();
                    break;

                case ConfigTabOption.INTERFACE:
                default:
                    DisplayInterfaceData();
                    break;
            }
        }


        private void btnDmMultiOptionMoved_Click(
              object sender,
              EventArgs e
              ) 
        {
            int index = -1;
            UltraButton btn = sender as UltraButton;
            if (btn == btnToLeftDMM)
            {
                foreach (ListViewItem item in lvSelectedDMM.SelectedItems)
                {
                    lvSelectedDMM.Items.RemoveAt(item.Index);
                    lvAllDMM.Items.Add(item);
                }
                lvAllDMM.Sorting = SortOrder.Ascending;
                lvAllDMM.Sort();
            }
            else if (btn == btnToRightDMM)
            {
                foreach (ListViewItem item in lvAllDMM.SelectedItems)
                {
                    lvAllDMM.Items.RemoveAt(item.Index);
                    lvSelectedDMM.Items.Add(item);
                }
            }
            else if (btn == btnSeletedItemUpDMM)
            {
                lvSelectedDMM.BeginUpdate();
                foreach (ListViewItem item in lvSelectedDMM.SelectedItems)
                {
                    if (item.Index > 0)
                    {
                        index = item.Index - 1;
                        lvSelectedDMM.Items.RemoveAt(item.Index);
                        lvSelectedDMM.Items.Insert(index, item);
                    }
                }
                lvSelectedDMM.EndUpdate();
            }
            else if (btn == btnSeletedItemDownDMM)
            {
                lvSelectedDMM.BeginUpdate();
                foreach (ListViewItem item in lvSelectedDMM.SelectedItems)
                {
                    if (item.Index < lvSelectedDMM.Items.Count - 1)
                    {
                        index = item.Index + 1;
                        lvSelectedDMM.Items.RemoveAt(item.Index);
                        lvSelectedDMM.Items.Insert(index, item);
                    }
                }
                lvSelectedDMM.EndUpdate();
            }

            //--

            SetDmOptionImageChangedByLVSelectedControl();
        }


        private void btnTESTMultiOptionMoved_Click(
            object sender,
            EventArgs e
            )
        {
            int index = -1;
            UltraButton btn = sender as UltraButton;
            if (btn == btnToLeftTESTM)
            {
                foreach (ListViewItem item in lvSelectedTESTM.SelectedItems)
                {
                    lvSelectedTESTM.Items.RemoveAt(item.Index);
                    lvAllTESTM.Items.Add(item);
                }
                lvAllTESTM.Sorting = SortOrder.Ascending;
                lvAllTESTM.Sort();
            }
            else if (btn == btnToRightTESTM)
            {
                foreach (ListViewItem item in lvAllTESTM.SelectedItems)
                {
                    lvAllTESTM.Items.RemoveAt(item.Index);
                    lvSelectedTESTM.Items.Add(item);
                }
            }
            else if (btn == btnSeletedItemUpTESTM)
            {
                lvSelectedTESTM.BeginUpdate();
                foreach (ListViewItem item in lvSelectedTESTM.SelectedItems)
                {
                    if (item.Index > 0)
                    {
                        index = item.Index - 1;
                        lvSelectedTESTM.Items.RemoveAt(item.Index);
                        lvSelectedTESTM.Items.Insert(index, item);
                    }
                }
                lvSelectedTESTM.EndUpdate();
            }
            else if (btn == btnSeletedItemDownTESTM)
            {
                lvSelectedTESTM.BeginUpdate();
                foreach (ListViewItem item in lvSelectedTESTM.SelectedItems)
                {
                    if (item.Index < lvSelectedTESTM.Items.Count - 1)
                    {
                        index = item.Index + 1;
                        lvSelectedTESTM.Items.RemoveAt(item.Index);
                        lvSelectedTESTM.Items.Insert(index, item);
                    }
                }
                lvSelectedTESTM.EndUpdate();
            }

            //--

            SetDmOptionImageChangedByLVSelectedControl();
        }

        private void btnDmMultiOptionImageMoved_Click(
             object sender,
             EventArgs e
             )
        {
            int index = -1;
            UltraButton btn = sender as UltraButton;
            if (btn == btnToLeftDMIMAGE)
            {
                foreach (ListViewItem item in lvSelectedDMIMAGE.SelectedItems)
                {
                    lvSelectedDMIMAGE.Items.RemoveAt(item.Index);
                    lvAllDMIMAGE.Items.Add(item);
                }
                lvAllDMIMAGE.Sorting = SortOrder.Ascending;
                lvAllDMIMAGE.Sort();
            }
            else if (btn == btnToRightDMIMAGE)
            {
                foreach (ListViewItem item in lvAllDMIMAGE.SelectedItems)
                {
                    lvAllDMIMAGE.Items.RemoveAt(item.Index);
                    lvSelectedDMIMAGE.Items.Add(item);
                }
            }
            else if (btn == btnSeletedItemUpDMIMAGE)
            {
                lvSelectedDMIMAGE.BeginUpdate();
                foreach (ListViewItem item in lvSelectedDMIMAGE.SelectedItems)
                {
                    if (item.Index > 0)
                    {
                        index = item.Index - 1;
                        lvSelectedDMIMAGE.Items.RemoveAt(item.Index);
                        lvSelectedDMIMAGE.Items.Insert(index, item);
                    }
                }
                lvSelectedDMIMAGE.EndUpdate();
            }
            else if (btn == btnSeletedItemDownDMIMAGE)
            {
                lvSelectedDMIMAGE.BeginUpdate();
                foreach (ListViewItem item in lvSelectedDMIMAGE.SelectedItems)
                {
                    if (item.Index < lvSelectedDMIMAGE.Items.Count - 1)
                    {
                        index = item.Index + 1;
                        lvSelectedDMIMAGE.Items.RemoveAt(item.Index);
                        lvSelectedDMIMAGE.Items.Insert(index, item);
                    }
                }
                lvSelectedDMIMAGE.EndUpdate();
            }

            //--

            SetDmOptionImageChangedByLVSelectedControl();
        }


        #endregion [ Event Hadler ]

        //--

        #region [ Method ]

        /// <summary>
        /// Tqd_Config 테이블의 interfac 항목 display
        /// </summary>
        private void DisplayInterfaceData(
            )
        {
            ComConfiguration oConfig = null;
            DataRow[] dr = null;
            try
            {
                oConfig = new ComConfiguration();
                DataTable dt = oConfig.GetSEMDASConfigure();
                if (dt == null || dt.Rows.Count == 0) return;


                dr = dt.Select(String.Format("[CATEGORY] = '{0}' AND [NAME] = 'MAXDEFECTS'", ConfigTabOption.INTERFACE));
                if (dr != null && dr.Length > 0)
                {
                    txtMaxDefectCount.Text = dr[0]["VALUE"].ToString();
                }
                else
                {
                    //Default
                    txtMaxDefectCount.Text = "10000";
                }

                dr = dt.Select(String.Format("[CATEGORY] ='{0}' AND [NAME] = 'IMAGEPATH'", ConfigTabOption.INTERFACE));
                if (dr != null && dr.Length > 0)
                {
                    txtImagePath.Text = dr[0]["VALUE"].ToString();
                }

                dr = dt.Select(String.Format("[CATEGORY] = '{0}' AND [NAME] = 'UPDATESERVERIP'", ConfigTabOption.INTERFACE));
                if (dr != null && dr.Length > 0)
                {
                    txtUpdateServer.Text = dr[0]["VALUE"].ToString();
                }

                dr = dt.Select(String.Format("[CATEGORY] = '{0}' AND [NAME] = 'SOCKETPORT'", ConfigTabOption.INTERFACE));
                if (dr != null && dr.Length > 0)
                {
                    txtSocketPort.Text = dr[0]["VALUE"].ToString();
                }
                else
                {
                    //Default
                    txtSocketPort.Text = "21";
                }

                dr = dt.Select(String.Format("[CATEGORY] = '{0}' AND [NAME] = 'LOGGINGLEVEL'", ConfigTabOption.INTERFACE));
                if (dr != null && dr.Length > 0)
                {
                    cmbLogLevel.Text = dr[0]["VALUE"].ToString();
                }
                else
                {
                    //Default
                    cmbLogLevel.Text = "5";
                }

                dr = dt.Select(String.Format("[CATEGORY] = '{0}' AND [NAME] = 'BACKUPFILELIFEDATE'", ConfigTabOption.INTERFACE));
                if (dr != null && dr.Length > 0)
                {
                    txtBackPeriod.Text = dr[0]["VALUE"].ToString();
                }
                else
                {
                    //Default
                    txtBackPeriod.Text = "100";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //--

        /// <summary>
        /// Tqd_Config 테이블의 interface 항목 변경
        /// </summary>
        private void UpdateInterfaceData(
            )
        {
            int iRow = 0;
            String[,] parameters = null;
            ComConfiguration oConfig = null;
            try
            {
                parameters = new String[6, 5];
                oConfig = new ComConfiguration();

                //--

                parameters[iRow, 0] = Enum.GetName(typeof(ConfigTabOption), ConfigTabOption.INTERFACE);
                parameters[iRow, 1] = "MAXDEFECTS";
                parameters[iRow, 2] = txtMaxDefectCount.Text;
                parameters[iRow, 3] = Enum.GetName(typeof(ConfigType), ConfigType.STRING);
                parameters[iRow, 4] = "Parsing defect count from KLARF";
                iRow++;

                parameters[iRow, 0] = Enum.GetName(typeof(ConfigTabOption), ConfigTabOption.INTERFACE);
                parameters[iRow, 1] = "IMAGEPATH";
                parameters[iRow, 2] = txtImagePath.Text;
                parameters[iRow, 3] = Enum.GetName(typeof(ConfigType), ConfigType.STRING);
                parameters[iRow, 4] = "Image Path";
                iRow++;

                parameters[iRow, 0] = Enum.GetName(typeof(ConfigTabOption), ConfigTabOption.INTERFACE);
                parameters[iRow, 1] = "UPDATESERVERIP";
                parameters[iRow, 2] = txtUpdateServer.Text;
                parameters[iRow, 3] = Enum.GetName(typeof(ConfigType), ConfigType.STRING);
                parameters[iRow, 4] = "Parsing application Server IP";
                iRow++;

                parameters[iRow, 0] = Enum.GetName(typeof(ConfigTabOption), ConfigTabOption.INTERFACE);
                parameters[iRow, 1] = "SOCKETPORT";
                parameters[iRow, 2] = txtSocketPort.Text;
                parameters[iRow, 3] = Enum.GetName(typeof(ConfigType), ConfigType.INTEGER);
                parameters[iRow, 4] = "SocketPort of FTP";
                iRow++;

                parameters[iRow, 0] = Enum.GetName(typeof(ConfigTabOption), ConfigTabOption.INTERFACE);
                parameters[iRow, 1] = "LOGGINGLEVEL";
                parameters[iRow, 2] = cmbLogLevel.Text;
                parameters[iRow, 3] = Enum.GetName(typeof(ConfigType), ConfigType.INTEGER);
                parameters[iRow, 4] = "Logging Level";
                iRow++;

                parameters[iRow, 0] = Enum.GetName(typeof(ConfigTabOption), ConfigTabOption.INTERFACE);
                parameters[iRow, 1] = "BACKUPFILELIFEDATE";
                parameters[iRow, 2] = txtBackPeriod.Text;
                parameters[iRow, 3] = Enum.GetName(typeof(ConfigType), ConfigType.INTEGER);
                parameters[iRow, 4] = "Backup Period";

                oConfig.AddConfiguration(parameters);
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        //--

        private void SetDmOptionImageChangedByLVSelectedControl(
            )
        {
            if (lvSelectedDM.Items.Count <= 0)
            {
                btnSeletedItemDown.Appearance.Image = imageList1.Images[3];
                btnSeletedItemDown.Enabled = false;
                btnSeletedItemUp.Appearance.Image = imageList1.Images[7];
                btnSeletedItemUp.Enabled = false;
            }
            else
            {
                btnSeletedItemDown.Appearance.Image = imageList1.Images[2];
                btnSeletedItemDown.Enabled = true;
                btnSeletedItemUp.Appearance.Image = imageList1.Images[6];
                btnSeletedItemUp.Enabled = true;
            }

            if (lvSelectedTEST.Items.Count <= 0)
            {
                btnSeletedItemDownTEST.Appearance.Image = imageList1.Images[3];
                btnSeletedItemDownTEST.Enabled = false;
                btnSeletedItemUpTEST.Appearance.Image = imageList1.Images[7];
                btnSeletedItemUpTEST.Enabled = false;
            }
            else
            {
                btnSeletedItemDownTEST.Appearance.Image = imageList1.Images[2];
                btnSeletedItemDownTEST.Enabled = true;
                btnSeletedItemUpTEST.Appearance.Image = imageList1.Images[6];
                btnSeletedItemUpTEST.Enabled = true;
            }

            if (lvSelectedDMM.Items.Count <= 0)
            {
                btnSeletedItemDownDMM.Appearance.Image = imageList1.Images[3];
                btnSeletedItemDownDMM.Enabled = false;
                btnSeletedItemUpDMM.Appearance.Image = imageList1.Images[7];
                btnSeletedItemUpDMM.Enabled = false;
            }
            else
            {
                btnSeletedItemDownDMM.Appearance.Image = imageList1.Images[2];
                btnSeletedItemDownDMM.Enabled = true;
                btnSeletedItemUpDMM.Appearance.Image = imageList1.Images[6];
                btnSeletedItemUpDMM.Enabled = true;
            }

            if (lvSelectedTESTM.Items.Count <= 0)
            {
                btnSeletedItemDownTESTM.Appearance.Image = imageList1.Images[3];
                btnSeletedItemDownTESTM.Enabled = false;
                btnSeletedItemUpTESTM.Appearance.Image = imageList1.Images[7];
                btnSeletedItemUpTESTM.Enabled = false;
            }
            else
            {
                btnSeletedItemDownTESTM.Appearance.Image = imageList1.Images[2];
                btnSeletedItemDownTESTM.Enabled = true;
                btnSeletedItemUpTESTM.Appearance.Image = imageList1.Images[6];
                btnSeletedItemUpTESTM.Enabled = true;
            }

            if (lvSelectedDMIMAGE.Items.Count <= 0)
            {
                btnSeletedItemDownDMIMAGE.Appearance.Image = imageList1.Images[3];
                btnSeletedItemDownDMIMAGE.Enabled = false;
                btnSeletedItemUpDMIMAGE.Appearance.Image = imageList1.Images[7];
                btnSeletedItemUpDMIMAGE.Enabled = false;
            }
            else
            {
                btnSeletedItemDownDMIMAGE.Appearance.Image = imageList1.Images[2];
                btnSeletedItemDownDMIMAGE.Enabled = true;
                btnSeletedItemUpDMIMAGE.Appearance.Image = imageList1.Images[6];
                btnSeletedItemUpDMIMAGE.Enabled = true;
            }
        }

        private void DisplayDmOptionData(
            )
        {
            lvAllDM.Items.Clear();
            lvSelectedDM.Items.Clear();

            lvAllTEST.Items.Clear();
            lvSelectedTEST.Items.Clear();

            lvAllDMM.Items.Clear();
            lvSelectedDMM.Items.Clear();

            lvAllTESTM.Items.Clear();
            lvSelectedTESTM.Items.Clear();

            lvAllDMIMAGE.Items.Clear();
            lvSelectedDMIMAGE.Items.Clear();

            //--

            ComConfiguration oConfig = new ComConfiguration();
            DataTable dt = oConfig.GetConfigUser(
                DACrux.Base.GlobalVariable.Factory,
                Enum.GetName(typeof(ConfigCategory), ConfigCategory.WAFER_OPTION),
                DACrux.Base.GlobalVariable.UserID
                );

            foreach (DataRow dr in dt.Rows)
            {
                ListViewItem item = null;
                if (String.IsNullOrEmpty(dr["USER_ID"].ToString()))
                    item = lvAllDM.Items.Add(dr["VALUE"].ToString());
                else item = lvSelectedDM.Items.Add(dr["VALUE"].ToString());
                item.ImageIndex = 0;
                item.Tag = dr["NAME"];
            }

            //--

            dt = oConfig.GetConfigUser(
                DACrux.Base.GlobalVariable.Factory,
                Enum.GetName(typeof(ConfigCategory), ConfigCategory.TEST_OPTION),
                DACrux.Base.GlobalVariable.UserID
                );

            foreach (DataRow dr in dt.Rows)
            {
                ListViewItem item = null;
                if (String.IsNullOrEmpty(dr["USER_ID"].ToString()))
                    item = lvAllTEST.Items.Add(dr["VALUE"].ToString());
                else item = lvSelectedTEST.Items.Add(dr["VALUE"].ToString());
                item.ImageIndex = 0;
                item.Tag = dr["NAME"];
            }

            //--

            dt = oConfig.GetConfigUser(
                DACrux.Base.GlobalVariable.Factory,
                Enum.GetName(typeof(ConfigCategory), ConfigCategory.WAFER_OPTION_M),
                DACrux.Base.GlobalVariable.UserID
                );

            foreach (DataRow dr in dt.Rows)
            {
                ListViewItem item = null;
                if (String.IsNullOrEmpty(dr["USER_ID"].ToString()))
                    item = lvAllDMM.Items.Add(dr["VALUE"].ToString());
                else item = lvSelectedDMM.Items.Add(dr["VALUE"].ToString());
                item.ImageIndex = 0;
                item.Tag = dr["NAME"];
            }

            //--

            dt = oConfig.GetConfigUser(
                DACrux.Base.GlobalVariable.Factory,
                Enum.GetName(typeof(ConfigCategory), ConfigCategory.TEST_OPTION_M),
                DACrux.Base.GlobalVariable.UserID
                );

            foreach (DataRow dr in dt.Rows)
            {
                ListViewItem item = null;
                if (String.IsNullOrEmpty(dr["USER_ID"].ToString()))
                    item = lvAllTESTM.Items.Add(dr["VALUE"].ToString());
                else item = lvSelectedTESTM.Items.Add(dr["VALUE"].ToString());
                item.ImageIndex = 0;
                item.Tag = dr["NAME"];
            }

            //--

            dt = oConfig.GetConfigUser(
                DACrux.Base.GlobalVariable.Factory,
                Enum.GetName(typeof(ConfigCategory), ConfigCategory.WAFER_OPTION_IMAGE),
                DACrux.Base.GlobalVariable.UserID
                );

            foreach (DataRow dr in dt.Rows)
            {
                ListViewItem item = null;
                if (String.IsNullOrEmpty(dr["USER_ID"].ToString()))
                    item = lvAllDMIMAGE.Items.Add(dr["VALUE"].ToString());
                else item = lvSelectedDMIMAGE.Items.Add(dr["VALUE"].ToString());
                item.ImageIndex = 0;
                item.Tag = dr["NAME"];
            }

            //--

            upbDieColor.BackColor = oConfig.GetConfigUserColor(
                DACrux.Base.GlobalVariable.Factory,
                Enum.GetName(typeof(ConfigCategory), ConfigCategory.WAFER_DIE_COLOR),
                "WAFER_MAP_BG",
                DACrux.Base.GlobalVariable.UserID
                );

            upbInspectColor.BackColor = oConfig.GetConfigUserColor(
               DACrux.Base.GlobalVariable.Factory,
               Enum.GetName(typeof(ConfigCategory), ConfigCategory.WAFER_DIE_COLOR),
               "WAFER_MAP_INP",
               DACrux.Base.GlobalVariable.UserID);


            upbDefectDieColor.BackColor = oConfig.GetConfigUserColor(
               DACrux.Base.GlobalVariable.Factory,
               Enum.GetName(typeof(ConfigCategory), ConfigCategory.WAFER_DIE_COLOR),
               "WAFER_MAP_DEFECT",
               DACrux.Base.GlobalVariable.UserID);

            upbMapLineColor.BackColor = oConfig.GetConfigUserColor(
             DACrux.Base.GlobalVariable.Factory,
             Enum.GetName(typeof(ConfigCategory), ConfigCategory.WAFER_DIE_COLOR),
             "WAFER_MAP_LINE",
             DACrux.Base.GlobalVariable.UserID);

            upbTestDieColor.BackColor = oConfig.GetConfigUserColor(
            DACrux.Base.GlobalVariable.Factory,
            Enum.GetName(typeof(ConfigCategory), ConfigCategory.WAFER_DIE_COLOR),
            "WAFER_TEST_MAP_BG",
            DACrux.Base.GlobalVariable.UserID);

            upbTestMapLineColor.BackColor = oConfig.GetConfigUserColor(
            DACrux.Base.GlobalVariable.Factory,
            Enum.GetName(typeof(ConfigCategory), ConfigCategory.WAFER_DIE_COLOR),
            "WAFER_TEST_MAP_LINE",
            DACrux.Base.GlobalVariable.UserID);

            object obj = oConfig.GetConfigUserValue(
                DACrux.Base.GlobalVariable.Factory,
                Enum.GetName(typeof(ConfigCategory), ConfigCategory.DEFECT_OPTION),
                "DEFAULT_DEFECT_SIZE",
                DACrux.Base.GlobalVariable.UserID
                );
            if (obj == null)
                nudDefectSize.Value = 2;
            else
                nudDefectSize.Value = decimal.Parse(obj.ToString());


            dt = oConfig.GetConfigUser(
                DACrux.Base.GlobalVariable.Factory,
                Enum.GetName(typeof(ConfigCategory), ConfigCategory.TEST_OPTION_ENABLE),
                DACrux.Base.GlobalVariable.UserID
                );

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["VALUE"].ToString() == "Y")
                    chkTestNoDisplay.Checked = false;
                else
                    chkTestNoDisplay.Checked = true;
            }

            dt = oConfig.GetConfigUser(
               DACrux.Base.GlobalVariable.Factory,
               Enum.GetName(typeof(ConfigCategory), ConfigCategory.WAFER_OPTION_ENABLE),
               DACrux.Base.GlobalVariable.UserID
               );

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["VALUE"].ToString() == "Y")
                    chkDMNoDisplay.Checked = false;
                else
                    chkDMNoDisplay.Checked = true;
            }

            dt = oConfig.GetConfigUser(
                DACrux.Base.GlobalVariable.Factory,
                Enum.GetName(typeof(ConfigCategory), ConfigCategory.TEST_OPTION_M_ENABLE),
                DACrux.Base.GlobalVariable.UserID
                );

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["VALUE"].ToString() == "Y")
                    chkTESTNoDisplayM.Checked = false;
                else
                    chkTESTNoDisplayM.Checked = true;
            }

            dt = oConfig.GetConfigUser(
               DACrux.Base.GlobalVariable.Factory,
               Enum.GetName(typeof(ConfigCategory), ConfigCategory.WAFER_OPTION_M_ENABLE),
               DACrux.Base.GlobalVariable.UserID
               );

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["VALUE"].ToString() == "Y")
                    chkDMNoDisplayM.Checked = false;
                else
                    chkDMNoDisplayM.Checked = true;
            }

            dt = oConfig.GetConfigUser(
                DACrux.Base.GlobalVariable.Factory,
                Enum.GetName(typeof(ConfigCategory), ConfigCategory.WAFER_IMAGE_ENABLE),
                DACrux.Base.GlobalVariable.UserID
                );

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["VALUE"].ToString() == "Y")
                    chkDMNoDisplayIMAGE.Checked = false;
                else
                    chkDMNoDisplayIMAGE.Checked = true;
            }

            dt = oConfig.GetConfigUser(
              DACrux.Base.GlobalVariable.Factory,
              Enum.GetName(typeof(ConfigCategory), ConfigCategory.VIRTUAL_OPTION),
              DACrux.Base.GlobalVariable.UserID
              );

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["NAME"].ToString() == "COLOR")
                {
                    upbVirtureColor.BackColor = System.Drawing.ColorTranslator.FromHtml(dr["VALUE"].ToString());
                }

                if (dr["NAME"].ToString() == "VISIBLE")
                {
                    if (dr["VALUE"].ToString() == "Y")
                        chkVirtualVisible.Checked = true;
                    else
                        chkVirtualVisible.Checked = false;
                }
            }
        }

        //--

        private void UpdateDmOptionData(
            )
        {
            ComConfiguration oConfig = new ComConfiguration();

            string[] arr;
            int iResult = -1;

            List<string[]> list = new List<string[]>();

            arr = new string[Enum.GetNames(typeof(Col)).Length];
            list.Add(arr);

            arr[(int)Col.Factory] = DACrux.Base.GlobalVariable.Factory;
            arr[(int)Col.Category] = Enum.GetName(typeof(ConfigCategory), ConfigCategory.WAFER_DIE_COLOR);
            arr[(int)Col.Name] = "WAFER_MAP_BG"; // name
            arr[(int)Col.Value] = System.Drawing.ColorTranslator.ToHtml(upbDieColor.BackColor);
            arr[(int)Col.Type] = Enum.GetName(typeof(ConfigType), ConfigType.HEXA); // type
            arr[(int)Col.UserID] = DACrux.Base.GlobalVariable.UserID; // userid
            arr[(int)Col.Ordered] = "0"; // ordered
            arr[(int)Col.CreateUser] = DACrux.Base.GlobalVariable.UserID; // create user
            arr[(int)Col.Comment] = "Background color for wafer die"; // comment

            arr = new string[Enum.GetNames(typeof(Col)).Length];
            list.Add(arr);

            arr[(int)Col.Factory] = DACrux.Base.GlobalVariable.Factory;
            arr[(int)Col.Category] = Enum.GetName(typeof(ConfigCategory), ConfigCategory.WAFER_DIE_COLOR);
            arr[(int)Col.Name] = "WAFER_MAP_INP"; // name
            arr[(int)Col.Value] = System.Drawing.ColorTranslator.ToHtml(upbInspectColor.BackColor);
            arr[(int)Col.Type] = Enum.GetName(typeof(ConfigType), ConfigType.HEXA); // type
            arr[(int)Col.UserID] = DACrux.Base.GlobalVariable.UserID; // userid
            arr[(int)Col.Ordered] = "0"; // ordered
            arr[(int)Col.CreateUser] = DACrux.Base.GlobalVariable.UserID; // create user
            arr[(int)Col.Comment] = "DM Inspection Die Color"; // comment

            arr = new string[Enum.GetNames(typeof(Col)).Length];
            list.Add(arr);

            arr[(int)Col.Factory] = DACrux.Base.GlobalVariable.Factory;
            arr[(int)Col.Category] = Enum.GetName(typeof(ConfigCategory), ConfigCategory.WAFER_DIE_COLOR);
            arr[(int)Col.Name] = "WAFER_MAP_DEFECT"; // name
            arr[(int)Col.Value] = System.Drawing.ColorTranslator.ToHtml(upbDefectDieColor.BackColor);
            arr[(int)Col.Type] = Enum.GetName(typeof(ConfigType), ConfigType.HEXA); // type
            arr[(int)Col.UserID] = DACrux.Base.GlobalVariable.UserID; // userid
            arr[(int)Col.Ordered] = "0"; // ordered
            arr[(int)Col.CreateUser] = DACrux.Base.GlobalVariable.UserID; // create user
            arr[(int)Col.Comment] = "DM Defect Die Color"; // comment

            arr = new string[Enum.GetNames(typeof(Col)).Length];
            list.Add(arr);

            arr[(int)Col.Factory] = DACrux.Base.GlobalVariable.Factory;
            arr[(int)Col.Category] = Enum.GetName(typeof(ConfigCategory), ConfigCategory.WAFER_DIE_COLOR);
            arr[(int)Col.Name] = "WAFER_MAP_LINE"; // name
            arr[(int)Col.Value] = System.Drawing.ColorTranslator.ToHtml(upbMapLineColor.BackColor);
            arr[(int)Col.Type] = Enum.GetName(typeof(ConfigType), ConfigType.HEXA); // type
            arr[(int)Col.UserID] = DACrux.Base.GlobalVariable.UserID; // userid
            arr[(int)Col.Ordered] = "0"; // ordered
            arr[(int)Col.CreateUser] = DACrux.Base.GlobalVariable.UserID; // create user
            arr[(int)Col.Comment] = "DM Wafer Line Color"; // comment

            arr = new string[Enum.GetNames(typeof(Col)).Length];
            list.Add(arr);

            arr[(int)Col.Factory] = DACrux.Base.GlobalVariable.Factory;
            arr[(int)Col.Category] = Enum.GetName(typeof(ConfigCategory), ConfigCategory.DEFECT_OPTION);
            arr[(int)Col.Name] = "DEFAULT_DEFECT_SIZE"; // name
            arr[(int)Col.Value] = nudDefectSize.Value.ToString(); // value
            arr[(int)Col.Type] = Enum.GetName(typeof(ConfigType), ConfigType.NUMBER); // type
            arr[(int)Col.UserID] = DACrux.Base.GlobalVariable.UserID; // userid
            arr[(int)Col.Ordered] = "0"; // ordered
            arr[(int)Col.CreateUser] = DACrux.Base.GlobalVariable.UserID; // create user
            arr[(int)Col.Comment] = String.Empty; // comment

            arr = new string[Enum.GetNames(typeof(Col)).Length];
            list.Add(arr);

            arr[(int)Col.Factory] = DACrux.Base.GlobalVariable.Factory;
            arr[(int)Col.Category] = Enum.GetName(typeof(ConfigCategory), ConfigCategory.WAFER_DIE_COLOR);
            arr[(int)Col.Name] = "WAFER_TEST_MAP_BG"; // name
            arr[(int)Col.Value] = System.Drawing.ColorTranslator.ToHtml(upbTestDieColor.BackColor);
            arr[(int)Col.Type] = Enum.GetName(typeof(ConfigType), ConfigType.HEXA); // type
            arr[(int)Col.UserID] = DACrux.Base.GlobalVariable.UserID; // userid
            arr[(int)Col.Ordered] = "0"; // ordered
            arr[(int)Col.CreateUser] = DACrux.Base.GlobalVariable.UserID; // create user
            arr[(int)Col.Comment] = "Background color for Test wafer die"; // comment

            arr = new string[Enum.GetNames(typeof(Col)).Length];
            list.Add(arr);

            arr[(int)Col.Factory] = DACrux.Base.GlobalVariable.Factory;
            arr[(int)Col.Category] = Enum.GetName(typeof(ConfigCategory), ConfigCategory.WAFER_DIE_COLOR);
            arr[(int)Col.Name] = "WAFER_TEST_MAP_LINE"; // name
            arr[(int)Col.Value] = System.Drawing.ColorTranslator.ToHtml(upbTestMapLineColor.BackColor);
            arr[(int)Col.Type] = Enum.GetName(typeof(ConfigType), ConfigType.HEXA); // type
            arr[(int)Col.UserID] = DACrux.Base.GlobalVariable.UserID; // userid
            arr[(int)Col.Ordered] = "0"; // ordered
            arr[(int)Col.CreateUser] = DACrux.Base.GlobalVariable.UserID; // create user
            arr[(int)Col.Comment] = "Test Wafer Line Color"; // comment

            //--

            for (int idx = 0; idx < lvSelectedDM.Items.Count; idx++)
            {
                arr = new string[Enum.GetNames(typeof(Col)).Length];
                list.Add(arr);

                arr[(int)Col.Factory] = DACrux.Base.GlobalVariable.Factory;
                arr[(int)Col.Category] = Enum.GetName(typeof(ConfigCategory), ConfigCategory.WAFER_OPTION);
                arr[(int)Col.Name] = lvSelectedDM.Items[idx].Tag as String; // name
                arr[(int)Col.Value] = lvSelectedDM.Items[idx].Text; // value
                arr[(int)Col.Type] = Enum.GetName(typeof(ConfigType), ConfigType.STRING); // type
                arr[(int)Col.UserID] = DACrux.Base.GlobalVariable.UserID; // userid
                arr[(int)Col.Ordered] = (idx + 1).ToString(); // ordered
                arr[(int)Col.CreateUser] = DACrux.Base.GlobalVariable.UserID; // create user
                arr[(int)Col.Comment] = String.Empty; // comment
            }

            for (int idx = 0; idx < lvSelectedTEST.Items.Count; idx++)
            {
                arr = new string[Enum.GetNames(typeof(Col)).Length];
                list.Add(arr);

                arr[(int)Col.Factory] = DACrux.Base.GlobalVariable.Factory;
                arr[(int)Col.Category] = Enum.GetName(typeof(ConfigCategory), ConfigCategory.TEST_OPTION);
                arr[(int)Col.Name] = lvSelectedTEST.Items[idx].Tag as String; // name
                arr[(int)Col.Value] = lvSelectedTEST.Items[idx].Text; // value
                arr[(int)Col.Type] = Enum.GetName(typeof(ConfigType), ConfigType.STRING); // type
                arr[(int)Col.UserID] = DACrux.Base.GlobalVariable.UserID; // userid
                arr[(int)Col.Ordered] = (idx + 1).ToString(); // ordered
                arr[(int)Col.CreateUser] = DACrux.Base.GlobalVariable.UserID; // create user
                arr[(int)Col.Comment] = String.Empty; // comment
            }

            for (int idx = 0; idx < lvSelectedDMM.Items.Count; idx++)
            {
                arr = new string[Enum.GetNames(typeof(Col)).Length];
                list.Add(arr);

                arr[(int)Col.Factory] = DACrux.Base.GlobalVariable.Factory;
                arr[(int)Col.Category] = Enum.GetName(typeof(ConfigCategory), ConfigCategory.WAFER_OPTION_M);
                arr[(int)Col.Name] = lvSelectedDMM.Items[idx].Tag as String; // name
                arr[(int)Col.Value] = lvSelectedDMM.Items[idx].Text; // value
                arr[(int)Col.Type] = Enum.GetName(typeof(ConfigType), ConfigType.STRING); // type
                arr[(int)Col.UserID] = DACrux.Base.GlobalVariable.UserID; // userid
                arr[(int)Col.Ordered] = (idx + 1).ToString(); // ordered
                arr[(int)Col.CreateUser] = DACrux.Base.GlobalVariable.UserID; // create user
                arr[(int)Col.Comment] = "DM Multi Option"; // comment
            }

            for (int idx = 0; idx < lvSelectedDMIMAGE.Items.Count; idx++)
            {
                arr = new string[Enum.GetNames(typeof(Col)).Length];
                list.Add(arr);

                arr[(int)Col.Factory] = DACrux.Base.GlobalVariable.Factory;
                arr[(int)Col.Category] = Enum.GetName(typeof(ConfigCategory), ConfigCategory.WAFER_OPTION_IMAGE);
                arr[(int)Col.Name] = lvSelectedDMIMAGE.Items[idx].Tag as String; // name
                arr[(int)Col.Value] = lvSelectedDMIMAGE.Items[idx].Text; // value
                arr[(int)Col.Type] = Enum.GetName(typeof(ConfigType), ConfigType.STRING); // type
                arr[(int)Col.UserID] = DACrux.Base.GlobalVariable.UserID; // userid
                arr[(int)Col.Ordered] = (idx + 1).ToString(); // ordered
                arr[(int)Col.CreateUser] = DACrux.Base.GlobalVariable.UserID; // create user
                arr[(int)Col.Comment] = "Image Multi Option"; // comment
            }

            for (int idx = 0; idx < lvSelectedTESTM.Items.Count; idx++)
            {
                arr = new string[Enum.GetNames(typeof(Col)).Length];
                list.Add(arr);

                arr[(int)Col.Factory] = DACrux.Base.GlobalVariable.Factory;
                arr[(int)Col.Category] = Enum.GetName(typeof(ConfigCategory), ConfigCategory.TEST_OPTION_M);
                arr[(int)Col.Name] = lvSelectedTESTM.Items[idx].Tag as String; // name
                arr[(int)Col.Value] = lvSelectedTESTM.Items[idx].Text; // value
                arr[(int)Col.Type] = Enum.GetName(typeof(ConfigType), ConfigType.STRING); // type
                arr[(int)Col.UserID] = DACrux.Base.GlobalVariable.UserID; // userid
                arr[(int)Col.Ordered] = (idx + 1).ToString(); // ordered
                arr[(int)Col.CreateUser] = DACrux.Base.GlobalVariable.UserID; // create user
                arr[(int)Col.Comment] = "Test Multi Option"; // comment
            }

            //Test Wafer Info 관련 사용 여부
            arr = new string[Enum.GetNames(typeof(Col)).Length];
            list.Add(arr);

            arr[(int)Col.Factory] = DACrux.Base.GlobalVariable.Factory;
            arr[(int)Col.Category] = Enum.GetName(typeof(ConfigCategory), ConfigCategory.TEST_OPTION_ENABLE);
            arr[(int)Col.Name] = "VISIBLE"; // name
            arr[(int)Col.Value] = chkTestNoDisplay.Checked == true ? "N" : "Y";
            arr[(int)Col.Type] = Enum.GetName(typeof(ConfigType), ConfigType.STRING); // type
            arr[(int)Col.UserID] = DACrux.Base.GlobalVariable.UserID; // userid
            arr[(int)Col.Ordered] = "0"; // ordered
            arr[(int)Col.CreateUser] = DACrux.Base.GlobalVariable.UserID; // create user
            arr[(int)Col.Comment] = "Test Info Display"; // comment

            //DM Wafer Info 관련 사용 여부
            arr = new string[Enum.GetNames(typeof(Col)).Length];
            list.Add(arr);

            arr[(int)Col.Factory] = DACrux.Base.GlobalVariable.Factory;
            arr[(int)Col.Category] = Enum.GetName(typeof(ConfigCategory), ConfigCategory.WAFER_OPTION_ENABLE);
            arr[(int)Col.Name] = "VISIBLE"; // name
            arr[(int)Col.Value] = chkDMNoDisplay.Checked == true ? "N" : "Y";
            arr[(int)Col.Type] = Enum.GetName(typeof(ConfigType), ConfigType.STRING); // type
            arr[(int)Col.UserID] = DACrux.Base.GlobalVariable.UserID; // userid
            arr[(int)Col.Ordered] = "0"; // ordered
            arr[(int)Col.CreateUser] = DACrux.Base.GlobalVariable.UserID; // create user
            arr[(int)Col.Comment] = "DM Info Display"; // comment

            //Test Wafer Multi Info 관련 사용 여부
            arr = new string[Enum.GetNames(typeof(Col)).Length];
            list.Add(arr);

            arr[(int)Col.Factory] = DACrux.Base.GlobalVariable.Factory;
            arr[(int)Col.Category] = Enum.GetName(typeof(ConfigCategory), ConfigCategory.TEST_OPTION_M_ENABLE);
            arr[(int)Col.Name] = "VISIBLE"; // name
            arr[(int)Col.Value] = chkTESTNoDisplayM.Checked == true ? "N" : "Y";
            arr[(int)Col.Type] = Enum.GetName(typeof(ConfigType), ConfigType.STRING); // type
            arr[(int)Col.UserID] = DACrux.Base.GlobalVariable.UserID; // userid
            arr[(int)Col.Ordered] = "0"; // ordered
            arr[(int)Col.CreateUser] = DACrux.Base.GlobalVariable.UserID; // create user
            arr[(int)Col.Comment] = "Test Info Display"; // comment

            //DM Wafer Multi Info 관련 사용 여부
            arr = new string[Enum.GetNames(typeof(Col)).Length];
            list.Add(arr);

            arr[(int)Col.Factory] = DACrux.Base.GlobalVariable.Factory;
            arr[(int)Col.Category] = Enum.GetName(typeof(ConfigCategory), ConfigCategory.WAFER_OPTION_M_ENABLE);
            arr[(int)Col.Name] = "VISIBLE"; // name
            arr[(int)Col.Value] = chkDMNoDisplayM.Checked == true ? "N" : "Y";
            arr[(int)Col.Type] = Enum.GetName(typeof(ConfigType), ConfigType.STRING); // type
            arr[(int)Col.UserID] = DACrux.Base.GlobalVariable.UserID; // userid
            arr[(int)Col.Ordered] = "0"; // ordered
            arr[(int)Col.CreateUser] = DACrux.Base.GlobalVariable.UserID; // create user
            arr[(int)Col.Comment] = "DM Info Display"; // comment

            //DM Wafer Multi Info 관련 사용 여부
            arr = new string[Enum.GetNames(typeof(Col)).Length];
            list.Add(arr);

            arr[(int)Col.Factory] = DACrux.Base.GlobalVariable.Factory;
            arr[(int)Col.Category] = Enum.GetName(typeof(ConfigCategory), ConfigCategory.WAFER_IMAGE_ENABLE);
            arr[(int)Col.Name] = "VISIBLE"; // name
            arr[(int)Col.Value] = chkDMNoDisplayIMAGE.Checked == true ? "N" : "Y";
            arr[(int)Col.Type] = Enum.GetName(typeof(ConfigType), ConfigType.STRING); // type
            arr[(int)Col.UserID] = DACrux.Base.GlobalVariable.UserID; // userid
            arr[(int)Col.Ordered] = "0"; // ordered
            arr[(int)Col.CreateUser] = DACrux.Base.GlobalVariable.UserID; // create user
            arr[(int)Col.Comment] = "Image Info Display"; // comment

            //Virtual Die Color
            arr = new string[Enum.GetNames(typeof(Col)).Length];
            list.Add(arr);

            arr[(int)Col.Factory] = DACrux.Base.GlobalVariable.Factory;
            arr[(int)Col.Category] = Enum.GetName(typeof(ConfigCategory), ConfigCategory.VIRTUAL_OPTION);
            arr[(int)Col.Name] = "COLOR"; // name
            arr[(int)Col.Value] = System.Drawing.ColorTranslator.ToHtml(upbVirtureColor.BackColor);
            arr[(int)Col.Type] = Enum.GetName(typeof(ConfigType), ConfigType.HEXA); // type
            arr[(int)Col.UserID] = DACrux.Base.GlobalVariable.UserID; // userid
            arr[(int)Col.Ordered] = "0"; // ordered
            arr[(int)Col.CreateUser] = DACrux.Base.GlobalVariable.UserID; // create user
            arr[(int)Col.Comment] = "Virtual Die Color"; // comment

            //Virtual Die Visible
            arr = new string[Enum.GetNames(typeof(Col)).Length];
            list.Add(arr);

            arr[(int)Col.Factory] = DACrux.Base.GlobalVariable.Factory;
            arr[(int)Col.Category] = Enum.GetName(typeof(ConfigCategory), ConfigCategory.VIRTUAL_OPTION);
            arr[(int)Col.Name] = "VISIBLE"; // name
            arr[(int)Col.Value] = chkVirtualVisible.Checked == true ? "Y" : "N";
            arr[(int)Col.Type] = Enum.GetName(typeof(ConfigType), ConfigType.STRING); // type
            arr[(int)Col.UserID] = DACrux.Base.GlobalVariable.UserID; // userid
            arr[(int)Col.Ordered] = "1"; // ordered
            arr[(int)Col.CreateUser] = DACrux.Base.GlobalVariable.UserID; // create user
            arr[(int)Col.Comment] = "Virtual Die Display"; // comment

            string[,] parameters = new string[list.Count, Enum.GetNames(typeof(Col)).Length];

            for (int y = 0; y < parameters.GetLength(0); y++)
                for (int x = 0; x < parameters.GetLength(1); x++)
                    parameters[y, x] = list[y][x];

            iResult = oConfig.SetConfigUser(
                DACrux.Base.GlobalVariable.Factory,
                new string[] {
                    Enum.GetName(typeof(ConfigCategory), ConfigCategory.WAFER_OPTION), 
                    Enum.GetName(typeof(ConfigCategory), ConfigCategory.TEST_OPTION),
                    Enum.GetName(typeof(ConfigCategory), ConfigCategory.DEFECT_OPTION),
                    Enum.GetName(typeof(ConfigCategory), ConfigCategory.WAFER_DIE_COLOR),
                    Enum.GetName(typeof(ConfigCategory), ConfigCategory.TEST_OPTION_ENABLE),
                    Enum.GetName(typeof(ConfigCategory), ConfigCategory.WAFER_OPTION_ENABLE),
                    Enum.GetName(typeof(ConfigCategory), ConfigCategory.VIRTUAL_OPTION),
                    Enum.GetName(typeof(ConfigCategory), ConfigCategory.WAFER_OPTION_M), 
                    Enum.GetName(typeof(ConfigCategory), ConfigCategory.TEST_OPTION_M),
                    Enum.GetName(typeof(ConfigCategory), ConfigCategory.TEST_OPTION_M_ENABLE),
                    Enum.GetName(typeof(ConfigCategory), ConfigCategory.WAFER_OPTION_M_ENABLE),
                    Enum.GetName(typeof(ConfigCategory), ConfigCategory.WAFER_OPTION_IMAGE),
                    Enum.GetName(typeof(ConfigCategory), ConfigCategory.WAFER_IMAGE_ENABLE)
                },
                DACrux.Base.GlobalVariable.UserID,
                parameters
                );
        }

        #endregion [ Method ]



    }
}
