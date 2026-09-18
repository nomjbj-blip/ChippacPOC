using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace DACrux.TEST.Control
{
    public partial class TPUCMapEdit : DACrux.Framework.Base.DACruxCTLBasic01
    {
        #region [ Data Field ]
        private DataSet m_dsMap = null;
        private string sCurrentKey = string.Empty;
        private bool m_bReadOnlyBin = false;
        private bool m_bReadOnlyBinDesc = false;
        private bool m_bReSetKeyMap = true;
        private bool m_bShowUserInformation = false;
        private string[] m_sUserInformation = null;
        private int m_nDisplayFaltAngle = -1;

        public enum MapMenuMode { Create, Edit };
        public delegate string MapSave(object sender, object AllDies);
        public delegate string MapUpdate(object sender, object ChangeDies);
        public delegate bool MapCreate(object sender, object WaferIDs);
        public delegate DataTable GetMasterBinData(object sender, WaferInfoEventArgs e);
        //public event MapSave OnMapSave;
        //public event MapUpdate OnMapUpdate;
        //public event MapCreate OnMapCreate;
        public event GetMasterBinData OnGetMasterBinData;

       // private DataTable dtAVIImage = null;
        private string WAFER_SEQ = string.Empty;
        private string LOT_ID = string.Empty;
        private string PROGRAM = string.Empty;
        private string DEVICE = string.Empty;
        private string TESTAREA = string.Empty;
        private string WAFERID = string.Empty;

        private string strBacupPath = string.Empty;

        //File Name Rule :  string.Format("{0}_{1}_{2}_{3}.jpg", PROGRAM, WAFERID, txtXIndex.Text, txtYIndex.Text);
        private string strBacupFileName = string.Empty;

        private string Senddataonetime = "!R";
        private string SendDataContinuously = "!U";
        private string SendStopSendingData = "!S";
        private string SendInitX = "!X";
        private string SendInitY = "!Y";

        private bool bEndLocation = false;

        private double m_Xaxis = double.NaN;
        private double m_Yaxis = double.NaN;

        private int ReceivedPassingCount = 0;
        private int ReceivedCount;
        private int ReceivedPos;
        private byte[] ReceivedBuffer = new byte[50];
        private string ReceivedX = " X";
        private string ReceivedY = " Y";
        private string ReceivedEND = "\n";

        private Dictionary<Point, string> oSocpeImages = new Dictionary<Point, string>();

        DACrux.Base.TestImageList oImageList = new Base.TestImageList();

        public class WaferInfoEventArgs : EventArgs
        {
            public string Customer { get; private set; }
            public string CustDevice { get; private set; }
            public string Device { get; private set; }
            public string LotID { get; private set; }
            public string WaferID { get; private set; }
            public string Oper { get; private set; }
            public string TesterID { get; private set; }
            public string Program { get; private set; }
            public string ProgramVer { get; private set; }
            public string MapTableName { get; private set; }

            // Methods
            public WaferInfoEventArgs(string sCustomer, string sCustDevice, string sDevice, string sLotID, string sWaferID, string sOper, string sTesterID, string sProgramm, string sProgramVer, string sMapTableName)
            {
                this.Customer = sCustomer;
                this.CustDevice = sCustDevice;
                this.Device = sDevice;
                this.LotID = sLotID;
                this.WaferID = sWaferID;
                this.Oper = sOper;
                this.TesterID = sTesterID;
                this.Program = sProgramm;
                this.ProgramVer = sProgramVer;
                this.MapTableName = sMapTableName;
            }
        }

        // Wafer Map 정보를 사용자가 지정한 것으로 표시할지 여부를 설정하거나 가져온다.
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ShowUserInformation
        {
            set { m_bShowUserInformation = value; }
            get { return m_bShowUserInformation; }
        }

        // Wafer Map에 정보를 사용자가 지정하여 표시한다.
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string[] UserInformation
        {
            set
            {
                m_sUserInformation = value;

                if (m_sUserInformation == null)
                    m_bShowUserInformation = false;
                else
                    m_bShowUserInformation = true;
            }
        }

        [Category("Option"), Description("Mouse가 Drag 될때 자동으로 Focus를 갖는지 여부늘 설정하거나 가져옵니다.")]
        public bool AutoFocus
        {
            set
            {
                m_ewMap.AutoFocus = value;
            }
            get
            {
                return m_ewMap.AutoFocus;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual object DataSource
        {
            set
            {
                if (value == null)
                    m_dsMap = null;
                else
                    m_dsMap = (DataSet)value;

                chAlterLot.Checked = false;
            }
            get { return m_dsMap; }
        }

        [Category("Option"), DefaultValue(false)]
        public bool ReadOnlyBin
        {
            set
            {
                m_bReadOnlyBin = value;

                for (int i = 1; i < 20; i++)
                    ((TextBox)tpMapBalance.Controls[string.Format("txtVal{0}", i)]).ReadOnly = m_bReadOnlyBin;
            }
            get { return m_bReadOnlyBin; }
        }

        [Category("Option"), DefaultValue(false)]
        public bool ReadOnlyBinDesc
        {
            set
            {
                m_bReadOnlyBinDesc = value;

                for (int i = 1; i < 20; i++)
                {
                    if (m_bReadOnlyBinDesc == true)
                        ((ComboBox)tpMapBalance.Controls[string.Format("cbDesc{0}", i)]).DropDownStyle = ComboBoxStyle.DropDownList;
                    else
                        ((ComboBox)tpMapBalance.Controls[string.Format("cbDesc{0}", i)]).DropDownStyle = ComboBoxStyle.DropDown;
                }
            }
            get { return m_bReadOnlyBinDesc; }
        }


        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsModify
        {
            get { return m_ewMap.ModifyDies.Count > 0; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ResetKeyMap
        {
            set { m_bReSetKeyMap = value; }
            get { return m_bReSetKeyMap; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DACrux.Base.DisplayFlatZone DisplayFlatZone
        {
            // 맵은 저장된 Flat 위치로 그린 후 회전시켜 보여준다.
            set { m_nDisplayFaltAngle = (int)value; }
            get { return (DACrux.Base.DisplayFlatZone)m_nDisplayFaltAngle; }
        }

        #endregion

        public TPUCMapEdit()
        {
            InitializeComponent();
        }



        #region [ Property ]



        #endregion

        #region [ Event Handler ]

        private void TPUCMapEdit_Load(object sender, EventArgs e)
        {
            strBacupPath = System.IO.Path.Combine(Environment.CurrentDirectory, this.Name);

            foreach (string comport in SerialPort.GetPortNames())
            {
                //현재 사용 가능한 Comport 를 불러 온다.
                SP.PortName = comport;
            }

            TxtSPOption.Text = string.Format("{0},{1},{2},{3},{4}", SP.PortName, SP.BaudRate, SP.DataBits, SP.Parity, SP.StopBits);

            BindingMapList();

            BtnConnect_Click(null, null);
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dsMap != null && m_dsMap.Tables.Count > 0)
                {
                    if (MessageBox.Show("현재 설정을 초기화 하시겠습니까?", "Question", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                        Draw();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            int iXIndexMax = 0;
            int iYIndexMax = 0;

            string strAlterLotID = string.Empty;
            string strAlterWaferID = string.Empty;
            string strAlterDevice = string.Empty;

            DACrux.Base.DieList oAlterDieList = null;
            try
            {
                oAlterDieList = m_ewMap.Dies;

                if (chAlterLot.Checked == true)
                {
                    if (string.IsNullOrEmpty(TxtDevice.Text))
                    {
                        MessageBox.Show("Device 를 선택 후 Draw 해주세요.", "Editer Map", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    strAlterDevice = TxtDevice.Text;

                    //Base Wafer 가 체크 되어 있을 경우 Mark Die 에 대해서만 수정 한다.
                    //if (chkBasicMap.Checked == true)
                    //{
                    //    oAlterDieList = new Base.DieList();
                    //    foreach (DACrux.Base.Die oDie in m_ewMap.Dies)
                    //    {
                    //        // DieProp 이 2일 경우 Mark die
                    //        if (oDie.DieProp == 2)
                    //            oAlterDieList.Add(oDie);
                    //    }
                    //}

                    //0번 Bin 으로 초기화 한다.
                    //for(int ir = 0; ir < oAlterDieList.Count; ir++)
                    //{
                    //    DACrux.Base.Die oAlterDie = oAlterDieList[ir];
                    //    oAlterDie.BinNumber = 0;
                    //    oAlterDieList[ir] = oAlterDie;
                    //}

                    if (string.IsNullOrEmpty(TxtLotID.Text) == true)
                    {
                        MessageBox.Show("변경하려는 Lot ID 를 정확히 입력해주세요.", "Editer Map", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtLotID.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(TxtLotID.Text) == true)
                    {
                        MessageBox.Show("변경하려는 Lot ID 를 정확히 입력해주세요.", "Editer Map", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtLotID.Focus();
                        return;
                    }

                    if (oAlterDieList == null || oAlterDieList.Count <= 0)
                    {
                        MessageBox.Show("Die 의 정보가 없습니다.", "Editer Map", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }


                    if (m_ewMap.ModifyDies.Count <= 0)
                    {
                        MessageBox.Show("수정된 Die 가 없습니다.", "Editer Map", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    strAlterLotID = TxtLotID.Text.ToUpper().Trim();
                    strAlterWaferID = string.Format("{0}-{1:00}",strAlterLotID, Convert.ToInt32(cmbWaferNo.Text));

                    LOT_ID = strAlterLotID;

                    if (MessageBox.Show(string.Format("{0} 해당 Wafer 로 생성 됩니다. 계속진행 하시겠습니까?", strAlterWaferID), "생성", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        return;

                }

                DACrux.Base.DieList oModifyDies = new Base.DieList();
                if (m_dsMap != null && m_dsMap.Tables.Count > 0 && m_ewMap.ModifyDies.Count > 0)
                {
                    //Image 확인 하여 재 조정 한다. Image Capture 의 경우 비동기 
                    foreach (DACrux.Base.Die oDie in m_ewMap.ModifyDies)
                    {
                        DACrux.Base.Die oModifyDie = oDie;
                        Point oIndex = new Point(oDie.IndexX, oDie.IndexY);

                        if (oSocpeImages.ContainsKey(oIndex) == true)
                        {
                            if (string.IsNullOrEmpty(oSocpeImages[oIndex]) == true)
                            {
                                oModifyDie.ScopeImageLocal = string.Empty;
                                oModifyDie.ScopeImage = string.Empty;
                                oModifyDie.ScopeImagePath = string.Empty;
                            }
                            else
                            {

                                System.IO.FileInfo oScopeFile = new System.IO.FileInfo(oSocpeImages[oIndex]);
                                if (oScopeFile.Exists)
                                {
                                    oModifyDie.ScopeImageLocal = oScopeFile.FullName;
                                    oModifyDie.ScopeImage = oScopeFile.Name;
                                    oModifyDie.ScopeImagePath = string.Format("BACKUP/{0}", DACrux.Utility.Util.FTPBackupPath("FOI", "SCOPE", LOT_ID));
                                }
                                else
                                {
                                    oModifyDie.ScopeImageLocal = string.Empty;
                                    oModifyDie.ScopeImage = string.Empty;
                                    oModifyDie.ScopeImagePath = string.Empty;
                                }
                            }
                        }

                        //Bin 정보를 VIFail 로 변환 한다.
                        oModifyDie.BinNumber = oModifyDie.VIFail;
                        oModifyDies.Add(oModifyDie);

                        int iDieIndex = oAlterDieList.IndexOf(oDie);
                        oAlterDieList[iDieIndex] = oModifyDie;

                        iXIndexMax = Math.Max(iXIndexMax, oAlterDieList[iDieIndex].IndexX);
                        iYIndexMax = Math.Max(iYIndexMax, oAlterDieList[iDieIndex].IndexY);
                    }

                    if (oImageList != null && oImageList.Count > 0)
                    {
                        for(int ir = 0; ir < oImageList.Count;ir++)
                        {
                            if (string.IsNullOrEmpty(oImageList[ir].LocalImagePath) == false)
                            {
                                DACrux.Base.TestImage oImage = new Base.TestImage();
                                oImage.XY = oImageList[ir].XY;
                                oImage.IndexX = oImageList[ir].XY.X.ToString();
                                oImage.IndexY = oImageList[ir].XY.Y.ToString();
                                oImage.LocalImagePath = oImageList[ir].LocalImagePath;
                                oImage.LocalImageName = oImageList[ir].LocalImageName;
                                oImage.ServerImagePath = string.Format("BACKUP/{0}", DACrux.Utility.Util.FTPBackupPath("FOI", "SCOPE", LOT_ID));
                                oImage.BinNumber = oImageList[ir].BinNumber;
                                oImageList[ir] = oImage;
                            }
                        }
                    }

                    DACrux.TEST.Control.Image.PopUpAVIImageSave dlg = new Image.PopUpAVIImageSave(oModifyDies, oImageList);
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        StatusMessage("저장 중 입니다.");

                        DACrux.TEST.RO.TestCommon oTestComm = new DACrux.TEST.RO.TestCommon();
                       
                        if(chAlterLot.Checked)
                        {
                            oTestComm.ScopeNewLotDataSet(DACrux.Base.GlobalVariable.Factory, DACrux.Base.GlobalVariable.UserID, strAlterLotID, strAlterWaferID, strAlterDevice, oAlterDieList, m_ewMap.Dies.Count, iXIndexMax, iYIndexMax, oImageList);
                        }
                        else
                        {
                            oTestComm.ScopeDataSet(WAFER_SEQ, oAlterDieList, m_ewMap.Dies.Count, iXIndexMax, iYIndexMax, DACrux.Base.GlobalVariable.UserID, chAlterLot.Checked, strAlterLotID, strAlterWaferID, oImageList);
                        }
                        StatusMessage(null);
                        MessageBox.Show("저장이 완료 되었습니다.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        m_ewMap.DieClear();
                        m_ewMap.DataSource = null;
                        m_ewMap.WaferDrawMode = Map.MapMode.Fit;
                        m_ewMap.Redraw();

                        BtnSerialDisConnect_Click(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                StatusMessage(null);
            }
        }



        private void txtVal_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(((TextBox)sender).Text.Trim()) == true)
                {
                    MessageBox.Show("Bin 번호를 먼저 입력하세요.", "Editer Map", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ((TextBox)sender).Focus();
                    return;
                }

                ColorDialog dlg = new ColorDialog();
                dlg.Color = ((TextBox)sender).BackColor;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    ((TextBox)sender).BackColor = dlg.Color;
                    m_ewMap.SetVIColor(DACrux.Base.Convert.intParse(((TextBox)sender).Text), dlg.Color);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void ShortCutLabel_Click(object sender, EventArgs e)
        {
            try
            {
                TextBox BinNum = null;
                switch (((Label)sender).Name)
                {
                    case "lbNum1":
                        BinNum = txtVal1;
                        break;
                    case "lbNum2":
                        BinNum = txtVal2;
                        break;
                    case "lbNum3":
                        BinNum = txtVal3;
                        break;
                    case "lbNum4":
                        BinNum = txtVal4;
                        break;
                    case "lbNum5":
                        BinNum = txtVal5;
                        break;
                    case "lbNum6":
                        BinNum = txtVal6;
                        break;
                    case "lbNum7":
                        BinNum = txtVal7;
                        break;
                    case "lbNum8":
                        BinNum = txtVal8;
                        break;
                    case "lbNum9":
                        BinNum = txtVal9;
                        break;
                    case "lbNum0":
                        BinNum = txtVal10;
                        break;
                    case "lbF1":
                        BinNum = txtVal11;
                        break;
                    case "lbF2":
                        BinNum = txtVal12;
                        break;
                    case "lbF3":
                        BinNum = txtVal13;
                        break;
                    case "lbF4":
                        BinNum = txtVal14;
                        break;
                    case "lbF5":
                        BinNum = txtVal15;
                        break;
                    case "lbF6":
                        BinNum = txtVal16;
                        break;
                    case "lbF7":
                        BinNum = txtVal17;
                        break;
                    case "lbF8":
                        BinNum = txtVal18;
                        break;
                    case "lbF9":
                        BinNum = txtVal19;
                        break;
                    case "lbDel":
                        BinNum = txtDel;
                        break;
                }

                if (BinNum != null && string.IsNullOrEmpty(BinNum.Text) == true)
                {
                    MessageBox.Show("Bin 번호를 먼저 입력하세요.", "Editer Map", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    BinNum.Focus();
                    return;
                }

                // 현재 설정된 Key를 비활성화한다.
                if (string.IsNullOrEmpty(sCurrentKey) == false)
                {
                    System.Windows.Forms.Control[] ctrls = this.Controls.Find(sCurrentKey, true);

                    if (ctrls.Length > 0)
                    {
                        Label beforeLabel = (Label)ctrls[0];

                        if (beforeLabel.ImageIndex % 2 == 1)
                            beforeLabel.ImageIndex -= 1;
                    }
                }

                Label afterLabel = (Label)sender;
                if (afterLabel.ImageIndex % 2 == 0)
                    afterLabel.ImageIndex += 1;
                else
                    afterLabel.ImageIndex -= 1;

                sCurrentKey = afterLabel.Name;
                int nSelectedKey = 0;
                switch (afterLabel.Name)
                {
                    case "lbNum1":
                        if (int.TryParse(txtVal1.Text, out nSelectedKey) == false)
                            nSelectedKey = 0;
                        break;
                    case "lbNum2":
                        if (int.TryParse(txtVal2.Text, out nSelectedKey) == false)
                            nSelectedKey = 0;
                        break;
                    case "lbNum3":
                        if (int.TryParse(txtVal3.Text, out nSelectedKey) == false)
                            nSelectedKey = 0;
                        break;
                    case "lbNum4":
                        if (int.TryParse(txtVal4.Text, out nSelectedKey) == false)
                            nSelectedKey = 0;
                        break;
                    case "lbNum5":
                        if (int.TryParse(txtVal5.Text, out nSelectedKey) == false)
                            nSelectedKey = 0;
                        break;
                    case "lbNum6":
                        if (int.TryParse(txtVal6.Text, out nSelectedKey) == false)
                            nSelectedKey = 0;
                        break;
                    case "lbNum7":
                        if (int.TryParse(txtVal7.Text, out nSelectedKey) == false)
                            nSelectedKey = 0;
                        break;
                    case "lbNum8":
                        if (int.TryParse(txtVal8.Text, out nSelectedKey) == false)
                            nSelectedKey = 0;
                        break;
                    case "lbNum9":
                        if (int.TryParse(txtVal9.Text, out nSelectedKey) == false)
                            nSelectedKey = 0;
                        break;
                    case "lbNum0":
                        if (int.TryParse(txtVal10.Text, out nSelectedKey) == false)
                            nSelectedKey = 0;
                        break;
                    case "lbF1":
                        if (int.TryParse(txtVal11.Text, out nSelectedKey) == false)
                            nSelectedKey = 0;
                        break;
                    case "lbF2":
                        if (int.TryParse(txtVal12.Text, out nSelectedKey) == false)
                            nSelectedKey = 0;
                        break;
                    case "lbF3":
                        if (int.TryParse(txtVal13.Text, out nSelectedKey) == false)
                            nSelectedKey = 0;
                        break;
                    case "lbF4":
                        if (int.TryParse(txtVal14.Text, out nSelectedKey) == false)
                            nSelectedKey = 0;
                        break;
                    case "lbF5":
                        if (int.TryParse(txtVal15.Text, out nSelectedKey) == false)
                            nSelectedKey = 0;
                        break;
                    case "lbF6":
                        if (int.TryParse(txtVal16.Text, out nSelectedKey) == false)
                            nSelectedKey = 0;
                        break;
                    case "lbF7":
                        if (int.TryParse(txtVal17.Text, out nSelectedKey) == false)
                            nSelectedKey = 0;
                        break;
                    case "lbF8":
                        if (int.TryParse(txtVal18.Text, out nSelectedKey) == false)
                            nSelectedKey = 0;
                        break;
                    case "lbF9":
                        if (int.TryParse(txtVal19.Text, out nSelectedKey) == false)
                            nSelectedKey = 0;
                        break;
                    case "lbDel":
                        m_ewMap.SelectKey = 0;
                        break;
                }

                Point oDieIndex = new Point(m_ewMap.GetFocusDie().X, m_ewMap.GetFocusDie().Y);
                string oImageFile = AVIImageMultiCapture();

                //Del Key 누르면 기존 Image 삭제
                if (afterLabel.Name == "lbDel")
                {
                    //특정 Index 의 모든 정보 삭제
                    oImageList.Remove(oDieIndex);
                }
                else
                {
                    //oImageFile = AVIImageMultiCapture();
                    //현재 영상에 대한 Image Capture 를 미리 한다.

                    if (string.IsNullOrEmpty(oImageFile) == false)
                    {
                        System.IO.FileInfo oFile = new System.IO.FileInfo(oImageFile);
                        if (oFile.Exists)
                        {
                            oImageList.Add(oDieIndex, oFile.FullName, oFile.Name, nSelectedKey.ToString());
                        }
                        else
                        {

                            MessageBox.Show("Image Capture 실패.", "Editer Map", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }

                if (oSocpeImages.ContainsKey(oDieIndex) == false)
                {
                    //생성
                    oSocpeImages.Add(oDieIndex, oImageFile);

                }
                else
                {
                    //수정
                    oSocpeImages[oDieIndex] = oImageFile;
                }

                //if (oImageFile != null && oImageFile.Exists == true)
                //{
                //    m_ewMap.SelectScopeImageLocal = oImageFile.FullName;
                //    m_ewMap.SelectScopeImage = oImageFile.Name;
                //    m_ewMap.SelectScopeImagePath = DACrux.Utility.Util.FTPBackupPath(TESTAREA, "SCOPE", LOT_ID);

                //    TxtSerialStatus.Text = string.Format("BinNum : {0}, Image Capture 완료 {1}", BinNum.Text, oImageFile.Name);
                //}
                //else
                //{
                //    TxtSerialStatus.Text = string.Format("BinNum : {0}, Image Capture 실패", BinNum.Text);
                //}

                //m_ewMap.SelectScopeImageLocal = string.Empty;
                //m_ewMap.SelectScopeImage = string.Empty;
                //m_ewMap.SelectScopeImagePath = string.Empty;
                //if (oImageFile != null && oImageFile.Exists == true)
                //{
                //    m_ewMap.SelectScopeImageLocal = oImageFile.FullName;
                //    m_ewMap.SelectScopeImage = oImageFile.Name;
                //    m_ewMap.SelectScopeImagePath = DACrux.Utility.Util.FTPBackupPath(TESTAREA, "SCOPE", LOT_ID);

                //    TxtSerialStatus.Text = string.Format("BinNum : {0}, Image Capture 완료 {1}", BinNum.Text, oImageFile.Name);
                //}
                //else
                //{
                //    TxtSerialStatus.Text = string.Format("BinNum : {0}, Image Capture 실패", BinNum.Text);
                //}

                m_ewMap.SelectKey = nSelectedKey;
                m_ewMap.SelectKeyIn = afterLabel.Name;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void txtVal_Validated(object sender, EventArgs e)
        {
            try
            {
                return;
                //if (string.IsNullOrEmpty(((TextBox)sender).Text.Trim()) == true)
                //{
                //    ((TextBox)sender).Text = string.Empty;
                //    return;
                //}

                //int nInputBin = -1;

                //if (int.TryParse(((TextBox)sender).Text, out nInputBin) == false)
                //{
                //    MessageBox.Show("Bin 번호를 입력하세요.", "Editer Map", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    ((TextBox)sender).Text = string.Empty;
                //}
                //else
                //{
                //    DataRow[] drBinInfo = m_dsMap.Tables["MASTER_BIN"].Select(string.Format("[BIN] = '{0}'", nInputBin));
                //    if (drBinInfo.Length < 1)
                //    {
                //        MessageBox.Show(string.Format("Bin {0} 은/는 MES에 정의되어 있지 않습니다.", nInputBin), "Editer Map", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //        ((TextBox)sender).Text = string.Empty;
                //        return;
                //    }

                //    int nIndex = DACrux.Base.Convert.intParse(((TextBox)sender).Name.Replace("txtVal", ""));

                //    m_ewMap.SetVIValue(nIndex, DACrux.Base.Convert.intParse(((TextBox)sender).Text));
                //    m_ewMap.SetVIColor(DACrux.Base.Convert.intParse(((TextBox)sender).Text), ((TextBox)sender).BackColor);

                //    System.Windows.Forms.Control[] ctrl = this.Controls.Find(string.Format("cbDesc{0}", nIndex), true);
                //    if (ctrl.Length > 0)
                //        ((ComboBox)ctrl[0]).Text = drBinInfo[0]["BIN_DESC"].ToString();
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void cbDesc_DropDown(object sender, EventArgs e)
        {
            DataTable dt  = null;

            try
            {
                if (m_dsMap == null)
                    return;

                if (((ComboBox)sender).DataSource == null || ((ComboBox)sender).Items.Count < 1)
                {
                    if (OnGetMasterBinData != null)
                    {
                        if (m_dsMap.Tables.IndexOf("WAFER_INFO") > -1 && m_dsMap.Tables["WAFER_INFO"].Rows.Count > 0)
                        {


                            dt = OnGetMasterBinData(this, new WaferInfoEventArgs(m_dsMap.Tables["WAFER_INFO"].Rows[0]["CUSTOMER"].ToString()
                                                                                            , m_dsMap.Tables["WAFER_INFO"].Rows[0]["CUST_DEVICE"].ToString()
                                                                                            , m_dsMap.Tables["WAFER_INFO"].Rows[0]["DEVICE"].ToString()
                                                                                            , m_dsMap.Tables["WAFER_INFO"].Rows[0]["LOT_ID"].ToString()
                                                                                            , m_dsMap.Tables["WAFER_INFO"].Rows[0]["WAFER_ID"].ToString()
                                                                                            , m_dsMap.Tables["WAFER_INFO"].Rows[0]["TESTAREA"].ToString()
                                                                                            , m_dsMap.Tables["WAFER_INFO"].Rows[0]["TESTER_ID"].ToString()
                                                                                            , m_dsMap.Tables["WAFER_INFO"].Rows[0]["PROGRAM"].ToString()
                                                                                            , m_dsMap.Tables["WAFER_INFO"].Rows[0]["PROGRAM_REV"].ToString()
                                                                                            , m_dsMap.Tables["WAFER_INFO"].Rows[0]["MAP_TABLE_NAME"].ToString()));
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                dt.TableName = "ALTER_BIN";
                                m_dsMap.Tables.Remove("ALTER_BIN");
                                m_dsMap.Tables.Add(dt);

                                ((ComboBox)sender).BeginUpdate();
                                ((ComboBox)sender).DisplayMember = "BIN_DESC";
                                ((ComboBox)sender).ValueMember = "BIN_DESC";
                                ((ComboBox)sender).DataSource = dt.Copy();
                                ((ComboBox)sender).DropDownHeight = ((ComboBox)sender).ItemHeight * 10;
                                ((ComboBox)sender).DropDownWidth = ((ComboBox)sender).Size.Width * 2;
                                ((ComboBox)sender).EndUpdate();
                            }
                            else
                            {
                                ((ComboBox)sender).DataSource = null;
                                ((ComboBox)sender).DropDownHeight = ((ComboBox)sender).ItemHeight;
                                ((ComboBox)sender).DropDownWidth = ((ComboBox)sender).Size.Width;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void cbDesc_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (((ComboBox)sender).SelectedIndex < 0 ||
                    (m_dsMap == null || m_dsMap.Tables.IndexOf("ALTER_BIN") < 0 || m_dsMap.Tables["ALTER_BIN"].Rows.Count < 1))
                {
                    int nIndex = 0;
                    if (int.TryParse(((ComboBox)sender).Name.Replace("cbDesc", ""), out nIndex) == false)
                        return;

                    Label SelectedLabel = null;
                    if (nIndex > 10)
                        SelectedLabel = ((Label)tpMapBalance.Controls[string.Format("lbF{0}", nIndex % 10)]);
                    else
                        SelectedLabel = ((Label)tpMapBalance.Controls[string.Format("lbNum{0}", nIndex % 10)]);

                    ((TextBox)tpMapBalance.Controls[string.Format("txtVal{0}", nIndex)]).Text = "";
                    ((TextBox)tpMapBalance.Controls[string.Format("txtVal{0}", nIndex)]).BackColor = Color.White;

                    if (SelectedLabel.ImageIndex % 2 == 1)
                        SelectedLabel.ImageIndex -= 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void txtCnt_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (((NumericUpDown)sender).Value == 0)
                    //((TextBox)tpMapBalance.Controls[string.Format("txtVal{0}", ((NumericUpDown)sender).Name.Replace("txtCnt", ""))]).ReadOnly = false;
                    ((ComboBox)tpMapBalance.Controls[((NumericUpDown)sender).Name.Replace("txtCnt", "cbDesc")]).Enabled = true;
                else
                    //((TextBox)tpMapBalance.Controls[string.Format("txtVal{0}", ((NumericUpDown)sender).Name.Replace("txtCnt", ""))]).ReadOnly = true;
                    ((ComboBox)tpMapBalance.Controls[((NumericUpDown)sender).Name.Replace("txtCnt", "cbDesc")]).Enabled = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void cbDesc_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Color TempColor;
            try
            {
                if (((ComboBox)sender).SelectedIndex == -1)
                    return;

                string sBinNum = m_dsMap.Tables["ALTER_BIN"].Rows[((ComboBox)sender).SelectedIndex]["BIN"].ToString();
                string strColor = m_dsMap.Tables["ALTER_BIN"].Rows[((ComboBox)sender).SelectedIndex]["BIN_COLOR"].ToString();

                TempColor = ColorTranslator.FromHtml(strColor);

                if (strColor == "#FFFFFF")
                    TempColor = DACrux.TEST.Control.Util.GetColor(DACrux.Base.Convert.intParse(sBinNum));

                for (int i = 1; i < 20; i++)
                {
                    if (((ComboBox)tpMapBalance.Controls[string.Format("cbDesc{0}", i)]).Name == ((ComboBox)sender).Name)
                        continue;

                    if (((ComboBox)tpMapBalance.Controls[string.Format("cbDesc{0}", i)]).SelectedIndex == ((ComboBox)sender).SelectedIndex)
                    {
                        ((ComboBox)sender).SelectedIndex = -1;
                        MessageBox.Show("동일 빈번호가 입력되어 있습니다.", "Editer Map", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                int nIndex = 0;
                if (int.TryParse(((ComboBox)sender).Name.Replace("cbDesc", ""), out nIndex) == false)
                    return;

                Label SelectedLabel = null;
                if (nIndex > 10)
                    SelectedLabel = ((Label)tpMapBalance.Controls[string.Format("lbF{0}", nIndex % 10)]);
                else
                    SelectedLabel = ((Label)tpMapBalance.Controls[string.Format("lbNum{0}", nIndex % 10)]);

                ((TextBox)tpMapBalance.Controls[string.Format("txtVal{0}", nIndex)]).Text = sBinNum;


                ((TextBox)tpMapBalance.Controls[string.Format("txtVal{0}", nIndex)]).BackColor = TempColor;

                m_ewMap.SetVIValue(nIndex, DACrux.Base.Convert.intParse(sBinNum));
                m_ewMap.SetVIColor(DACrux.Base.Convert.intParse(sBinNum), TempColor);

                if (SelectedLabel.ImageIndex % 2 != 0)
                {
                    int nTemp = 0;
                    if (int.TryParse(((TextBox)tpMapBalance.Controls[string.Format("txtVal{0}", nIndex)]).Text, out nTemp) == false)
                        m_ewMap.SelectKey = 0;
                    else
                        m_ewMap.SelectKey = nTemp;
                }
            }
            catch (Exception) { }
        }


        private void m_ewMap_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                StatusMessage("Bin 수정 중입니다.");
                switch (e.KeyCode)
                {
                    case Keys.D1:
                    case Keys.NumPad1:
                        if (string.IsNullOrEmpty(txtVal1.Text) == false)
                            ShortCutLabel_Click(lbNum1, null);
                        else
                            e.Handled = true;
                        break;
                    case Keys.D2:
                    case Keys.NumPad2:
                        if (string.IsNullOrEmpty(txtVal2.Text) == false)
                            ShortCutLabel_Click(lbNum2, null);
                        else
                            e.Handled = true;
                        break;
                    case Keys.D3:
                    case Keys.NumPad3:
                        if (string.IsNullOrEmpty(txtVal3.Text) == false)
                            ShortCutLabel_Click(lbNum3, null);
                        else
                            e.Handled = true;
                        break;
                    case Keys.D4:
                    case Keys.NumPad4:
                        if (string.IsNullOrEmpty(txtVal4.Text) == false)
                            ShortCutLabel_Click(lbNum4, null);
                        else
                            e.Handled = true;
                        break;
                    case Keys.D5:
                    case Keys.NumPad5:
                        if (string.IsNullOrEmpty(txtVal5.Text) == false)
                            ShortCutLabel_Click(lbNum5, null);
                        else
                            e.Handled = true;
                        break;
                    case Keys.D6:
                    case Keys.NumPad6:
                        if (string.IsNullOrEmpty(txtVal6.Text) == false)
                            ShortCutLabel_Click(lbNum6, null);
                        else
                            e.Handled = true;
                        break;
                    case Keys.D7:
                    case Keys.NumPad7:
                        if (string.IsNullOrEmpty(txtVal7.Text) == false)
                            ShortCutLabel_Click(lbNum7, null);
                        else
                            e.Handled = true;
                        break;
                    case Keys.D8:
                    case Keys.NumPad8:
                        if (string.IsNullOrEmpty(txtVal8.Text) == false)
                            ShortCutLabel_Click(lbNum8, null);
                        else
                            e.Handled = true;
                        break;
                    case Keys.D9:
                          case Keys.NumPad9:
                        if (string.IsNullOrEmpty(txtVal9.Text) == false)
                            ShortCutLabel_Click(lbNum9, null);
                        else
                            e.Handled = true;
                        break;
                    case Keys.D0:
                    case Keys.NumPad0:
                        if (string.IsNullOrEmpty(txtVal10.Text) == false)
                            ShortCutLabel_Click(lbNum0, null);
                        else
                            e.Handled = true;
                        break;
                    case Keys.F1:
                        if (string.IsNullOrEmpty(txtVal11.Text) == false)
                            ShortCutLabel_Click(lbF1, null);
                        else
                            e.Handled = true;
                        break;
                    case Keys.F2:
                        if (string.IsNullOrEmpty(txtVal12.Text) == false)
                            ShortCutLabel_Click(lbF2, null);
                        else
                            e.Handled = true;
                        break;
                    case Keys.F3:
                        if (string.IsNullOrEmpty(txtVal13.Text) == false)
                            ShortCutLabel_Click(lbF3, null);
                        else
                            e.Handled = true;
                        break;
                    case Keys.F4:
                        if (string.IsNullOrEmpty(txtVal14.Text) == false)
                            ShortCutLabel_Click(lbF4, null);
                        else
                            e.Handled = true;
                        break;
                    case Keys.F5:
                        if (string.IsNullOrEmpty(txtVal15.Text) == false)
                            ShortCutLabel_Click(lbF5, null);
                        else
                            e.Handled = true;
                        break;
                    case Keys.F6:
                        if (string.IsNullOrEmpty(txtVal16.Text) == false)
                            ShortCutLabel_Click(lbF6, null);
                        else
                            e.Handled = true;
                        break;
                    case Keys.F7:
                        if (string.IsNullOrEmpty(txtVal17.Text) == false)
                            ShortCutLabel_Click(lbF7, null);
                        else
                            e.Handled = true;
                        break;
                    case Keys.F8:
                        if (string.IsNullOrEmpty(txtVal18.Text) == false)
                            ShortCutLabel_Click(lbF8, null);
                        else
                            e.Handled = true;
                        break;
                    case Keys.F9:
                        if (string.IsNullOrEmpty(txtVal19.Text) == false)
                            ShortCutLabel_Click(lbF9, null);
                        else
                            e.Handled = true;
                        break;
                    case Keys.Delete:
                        if (string.IsNullOrEmpty(txtDel.Text) == false)
                            ShortCutLabel_Click(lbDel, null);
                        else
                            e.Handled = true;
                        break;
                    case Keys.Subtract:
                        //좌표 초기화
                        if (SP.IsOpen == true)
                        {
                            m_ewMap.VisibleShotAlignPoint = true;
                            SP.Write(SendStopSendingData);
                            System.Threading.Thread.Sleep(10);
                            SP.WriteLine(SendInitX);
                            System.Threading.Thread.Sleep(10);
                            SP.WriteLine(SendInitY);
                            System.Threading.Thread.Sleep(10);
                            SP.Write(SendDataContinuously);
                            bEndLocation = false;
                            TxtSerialStatus.Text = "Step 1 : 초기화 완료 두번째 Shot에서 '+' 를 눌러 주세요.";
                            TxtSerialStatus.BackColor = Color.Yellow;
                        }

                        break;
                    case Keys.Add:
                        if (SP.IsOpen == true)
                        {
                            //측정 거리가 이론상의 거리보다 5% 안에 못들어오면 Error 처리함.
                            double dLimit = (double)numPer.Value;

                            //이론상 거리
                            double dOriWidth = (m_ewMap.DieSizeX * (m_ewMap.DieOfRightShotNotify.IndexX - m_ewMap.DieOfLeftShotNotify.IndexX));
                            //Percentage
                            double dOriWidthPer = dOriWidth * (dLimit / 100);
                            if (m_Xaxis > (dOriWidth + dOriWidthPer) || m_Xaxis < (dOriWidth - dOriWidthPer))
                            {
                                if (MessageBox.Show(string.Format("정상 범주안에 들어 오지 않습니다. 그래도 계속 진행 하시겠습니까?\nShot 간 거리 : {0}, 현재 거리 : {1}, 정상 범주 : {2} ~ {3}",
                                    dOriWidth, m_Xaxis, (dOriWidth - dOriWidthPer), (dOriWidth + dOriWidthPer)), "생성", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                                    return;
                            }

                            m_ewMap.VisibleShotAlignPoint = false;
                            m_ewMap.CalcShotNotifyAlignPoint(m_Xaxis, m_Yaxis);
                            bEndLocation = true;
                            TxtSerialStatus.Text = "Step 2 : 연동 완료.  Wafer 를 회전하지 마세요.";
                            TxtSerialStatus.BackColor = Color.Green;
                        }
                        break;
                }
                m_ewMap.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                StatusMessage(null);
            }

        }


        private void m_ewMap_OnTESTRedefineFirstDie(object sender, Base.Die NewDie)
        {

        }

        private void m_ewMap_OnTESTChangeDieProperty(object sender, Base.Die NewDie)
        {

        }

        private void m_ewMap_OnChangeCurrentDie(object sender, Base.Die NewDie)
        {
            txtXIndex.Text = NewDie.IndexX.ToString();
            txtYIndex.Text = NewDie.IndexY.ToString();
            txtBin.Text = NewDie.BinNumber.ToString();
            //m_ewMap.Focus();
            //string strImagePath = string.Empty;
            //try
            //{
            //    try
            //    {
            //        if (TESTAREA == "AVI" && dtAVIImage != null && dtAVIImage.Select(string.Format("X = {0} AND Y = {1}", NewDie.IndexX, NewDie.IndexY)).Length > 0)
            //        {
            //            DataRow[] drAVI = dtAVIImage.Select(string.Format("X = {0} AND Y = {1}", NewDie.IndexX, NewDie.IndexY));
            //            foreach (DataRow dr in drAVI)
            //            {
            //                strImagePath = dr["IMAGE_PATH"].ToString();
            //            }
            //        }

            //        tpuImageInfo1.ImageReflesh(
            //                strImagePath,
            //                NewDie.IndexX.ToString(),
            //                NewDie.IndexY.ToString(),
            //                NewDie.BinNumber.ToString(),
            //                LOT_ID,
            //                PROGRAM,
            //                DEVICE);
            //    }
            //    catch (Exception) { }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    m_ewMap.Focus();
            //    Application.DoEvents();
            //}
        }

        private void m_ewMap_OnChangeDieBinNumber(object sender, Base.ChangeDieBinNumberInfo e)
        {
            try
            {
                for (int i = 1; i < 20; i++)
                {
                    if (e.nPreDefectNumber >= 0)
                    {
                        if (((TextBox)tpMapBalance.Controls[string.Format("txtVal{0}", i)]).Text == e.nPreDefectNumber.ToString()
                            && ((NumericUpDown)tpMapBalance.Controls[string.Format("txtCnt{0}", i)]).Value > 0)
                        {
                            ((NumericUpDown)tpMapBalance.Controls[string.Format("txtCnt{0}", i)]).Value -= 1;
                            ((NumericUpDown)tpMapBalance.Controls[string.Format("txtCnt{0}", i)]).Refresh();
                        }
                    }

                    if (((TextBox)tpMapBalance.Controls[string.Format("txtVal{0}", i)]).Text == e.nDefectNumber.ToString())
                    {
                        ((NumericUpDown)tpMapBalance.Controls[string.Format("txtCnt{0}", i)]).Value += 1;
                        ((NumericUpDown)tpMapBalance.Controls[string.Format("txtCnt{0}", i)]).Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void m_ewMap_OnChangePosition(object sender, Base.PointD Currpoint, Base.PointD RealPoint)
        {

        }


        private void BtnStart_Click(object sender, EventArgs e)
        {
            m_ewMap.VisibleShotAlignPoint = true;

            txtR.Text = (m_ewMap.DieSizeX * (m_ewMap.DieOfRightShotNotify.IndexX - m_ewMap.DieOfLeftShotNotify.IndexX)).ToString();

            //좌표 초기화
            if (SP.IsOpen == true)
            {
                SP.Write(SendStopSendingData);
                System.Threading.Thread.Sleep(10);
                SP.WriteLine(SendInitX);
                System.Threading.Thread.Sleep(10);
                SP.WriteLine(SendInitY);
                System.Threading.Thread.Sleep(10);
                SP.Write(SendDataContinuously);
            }

            bEndLocation = false;
        }

        private void BtnEnd_Click(object sender, EventArgs e)
        {
            m_ewMap.VisibleShotAlignPoint = false;

            if (!Double.TryParse(TxtEndX.Text.Replace(" ", String.Empty), out m_Xaxis) || !Double.TryParse(TxtEndY.Text.Replace(" ", String.Empty), out m_Yaxis))
                return;

            //m_Xaxis = m_Xaxis * 1000d;
            //m_Yaxis = m_Yaxis * 1000d;

            m_ewMap.CalcShotNotifyAlignPoint(m_Xaxis, m_Yaxis);
            //m_ewMap.CalcShotNotifyAlignPoint(m_Xaxis, m_Yaxis);
            bEndLocation = true;
        }

        private void BtnSetFocus_Click(object sender, EventArgs e)
        {
            if (!Double.TryParse(TxtSetX.Text.Replace(" ", String.Empty), out m_Xaxis) || !Double.TryParse(TxtSetY.Text.Replace(" ", String.Empty), out m_Yaxis))
                return;

            //m_Xaxis = m_Xaxis * 1000d;
            //m_Yaxis = m_Yaxis * 1000d;

            m_ewMap.SetFocusDieByShotNotify(m_Xaxis, m_Yaxis);
            //m_ewMap.SetFocusDieByShotNotify(m_Xaxis, m_Yaxis);
            //m_ewMap.Focus();
        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            videoCaptureControls.Connect();
        }

        private void BtnDisconnect_Click(object sender, EventArgs e)
        {
            videoCaptureControls.Disconnect();
        }

        private void BtnCapture_Click(object sender, EventArgs e)
        {
            AVIImageCapture();
        }

        private void BtnSerialConnect_Click(object sender, EventArgs e)
        {
            try
            {
                //접속 시키기전에 기존 접속을 끓고 작업 한다.
                //재 접속을 하려고 할때 이미 Connect 되어 있다는 Error Message 발생.
                if (SP != null)
                {
                    SP.Close();
                }

                SP.Open();
                if (SP.IsOpen)
                {
                    SerialOptionEnable(false);
                    TxtSerialStatus.Text = "Connect!! 첫번째 Shot 에서 '-' 을 눌러주세요.";
                    TxtSerialStatus.BackColor = Color.Yellow;
                    SP.Write(SendDataContinuously);
                }
                else
                {
                    SerialOptionEnable(true);
                    TxtSerialStatus.Text = "[Fail] Port Open!";
                    TxtSerialStatus.BackColor = System.Drawing.SystemColors.Control;
                    SP.Write(SendStopSendingData);
                }

                bEndLocation = false;
                m_ewMap.Focus();

                if (SP.IsOpen)
                    m_ewMap.VisibleShotAlignPoint = true;
            }
            catch (Exception ex)
            {
                TxtSerialStatus.Text = string.Format("[Fail] : {0}", ex.Message);
                TxtSerialStatus.BackColor = Color.Red;
                bEndLocation = false;
                SerialOptionEnable(true);
            }
        }

        private void BtnSerialDisConnect_Click(object sender, EventArgs e)
        {
            if (SP != null && SP.IsOpen)
            {
                SP.Write(SendStopSendingData);
                SP.Close();
            }

            TxtSerialStatus.Text = "Not Connect!!";
            TxtSerialStatus.BackColor = System.Drawing.SystemColors.Control;
            SerialOptionEnable(true);
            bEndLocation = false;
            m_ewMap.VisibleShotAlignPoint = false;
        }

        private void BtnOption_Click(object sender, EventArgs e)
        {
            TPUCRS232Option oForm = new TPUCRS232Option();
            if (oForm.ShowDialog(this) == DialogResult.OK)
            {
                SP.PortName = oForm.SP_OPTION.PortName;
                SP.BaudRate = oForm.SP_OPTION.BaudRate;
                SP.DataBits = oForm.SP_OPTION.DataBits;
                SP.Parity = oForm.SP_OPTION.Parity;
                SP.StopBits = oForm.SP_OPTION.StopBits;
                TxtSPOption.Text = string.Format("{0},{1},{2},{3},{4}", SP.PortName, SP.BaudRate, SP.DataBits, SP.Parity, SP.StopBits);
            }
            m_ewMap.Focus();
        }

        private void SP_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (!SP.IsOpen)
                return;

            if (ReceivedPassingCount > ReceivedCount)
            {
                ReceivedCount++;
                return;
            }

            ReceivedPos = ReceivedPos + SP.Read(ReceivedBuffer, ReceivedPos, ReceivedBuffer.Length - ReceivedPos);

            if (ReceivedPos < ReceivedBuffer.Length)
                return;

            ReceivedPos = 0;
            ReceivedCount = 0;

            string data = SP.Encoding.GetString(ReceivedBuffer);
            int idxX = data.IndexOf(ReceivedX);
            int idxY = data.IndexOf(ReceivedY, idxX);
            int idxEnd = data.IndexOf(ReceivedEND, idxY);

            if (idxX == -1 || idxY == -1 || idxEnd == -1)
                return;

            string strX = data.Substring(idxX + ReceivedX.Length, idxY - idxX - ReceivedX.Length);
            string strY = data.Substring(idxY + ReceivedY.Length, idxEnd - idxY - ReceivedY.Length);

            if (!Double.TryParse(strX.Replace(" ", String.Empty), out m_Xaxis) || !Double.TryParse(strY.Replace(" ", String.Empty), out m_Yaxis))
                return;

            m_Xaxis = m_Xaxis * 1000d;
            m_Yaxis = m_Yaxis * 1000d;
            //System.Diagnostics.Debug.WriteLine(String.Format("X:{0}, Y:{1}", valX, valY));

            SP.DiscardInBuffer();

            SerialDieSelect(m_Xaxis, m_Yaxis);


            //string[] strValues = null;
            //string strValue = string.Empty;

            //try
            //{
            //    if (SP.IsOpen)
            //    {
            //        iCount++;
            //        // X+    0.000 Y-   31.597
            //        strValue = SP.ReadLine();
            //        //Data 양이 많아 3건 정도를 걸러 낸다.
            //        if (iCount > 3)
            //        {
            //            strValue = strValue.Trim().Replace("\r\n", "").Replace(" ", "").Trim();

            //            strValues = strValue.Split(new string[] { "X", "Y" }, StringSplitOptions.RemoveEmptyEntries);
            //            if (strValues.Length == 2)
            //            {
            //                if (double.TryParse(strValues[0], out m_Xaxis) == false)
            //                    m_Xaxis = double.NaN;

            //                if (double.TryParse(strValues[1], out m_Yaxis) == false)
            //                    m_Yaxis = double.NaN;
            //            }

            //            SerialDieSelect(m_Xaxis, m_Yaxis);
            //            //System.Diagnostics.Debug.WriteLine(strValue);
            //            //System.Diagnostics.Debugss.WriteLine("--------------------------------------");
            //            iCount = 0;
            //        }
                   
            //    }
            //}
            //catch (Exception) { }
        }

        private void SerialDieSelect(double dXAxis, double dYAxis)
        {
            if (m_ewMap.InvokeRequired)
            {
                m_ewMap.BeginInvoke(new MethodInvoker(
                   delegate()
                   {
                       SerialDieSelect(dXAxis, dYAxis);
                   }
                   ));
            }
            else
            {
                if (double.IsNaN(dXAxis) == false && double.IsNaN(dYAxis) == false && bEndLocation == true)
                {
                   // m_ewMap.FocusDiePassingCount = 5;
                    m_ewMap.SetFocusDieByShotNotify(dXAxis, dYAxis);
                    //m_ewMap.Focus();
                }

                SeiralTextX(dXAxis.ToString());
                SeiralTextY(dYAxis.ToString());
            }
        }

        private void SeiralTextX(string strVal)
        {
            if (TxtSerialX.InvokeRequired)
            {
                TxtSerialX.BeginInvoke(new MethodInvoker(
                    delegate()
                    {
                        SeiralTextX(strVal);
                    }
                    ));
            }
            else
            {
                TxtSerialX.Text = strVal;
            }
        }

        private void SeiralTextY(string strVal)
        {
            if (TxtSerialY.InvokeRequired)
            {
                TxtSerialY.BeginInvoke(new MethodInvoker(
                    delegate()
                    {
                        SeiralTextY(strVal);
                    }
                    ));
            }
            else
            {
                TxtSerialY.Text = strVal;
            }
        }


        private void BtnTest01_Click(object sender, EventArgs e)
        {
            if (SP.IsOpen)
            {
                SP.Write(Senddataonetime);
            }
        }

        private void BtnTest02_Click(object sender, EventArgs e)
        {
            if (SP.IsOpen)
            {
                SP.Write(SendDataContinuously);
            }
        }

        private void BtnTest03_Click(object sender, EventArgs e)
        {
            if (SP.IsOpen)
            {
                SP.Write(SendStopSendingData);
            }

        }

        private void SendInit_Click(object sender, EventArgs e)
        {
            if (SP.IsOpen)
            {
                SP.Write(SendStopSendingData);
                System.Threading.Thread.Sleep(10);
                SP.WriteLine(SendInitX);
                System.Threading.Thread.Sleep(10);
                SP.WriteLine(SendInitY);
                System.Threading.Thread.Sleep(10);
                SP.Write(SendDataContinuously);
            }
        }

        private void BtnShotMapDraw_Click(object sender, EventArgs e)
        {
            DACrux.TEST.RO.ProbeMapAnalysis oTestMap = new RO.ProbeMapAnalysis();

            try
            {
                if (dlbDeviceList.SelectedIndex < 0 || string.IsNullOrEmpty(dlbDeviceList.SelectedValue.ToString()))
                    return;

                StatusMessage("Data 를 조회 중입니다.");

                if (m_dsMap != null)
                    m_dsMap.Dispose();

                m_dsMap = null;

                TxtDevice.Text = dlbDeviceList.SelectedValue.ToString();

                m_dsMap = oTestMap.SelectWaferShotMapBasic(TxtDevice.Text);

                Draw();
            }
            finally
            {
            }
        }

        private void dlbDeviceList_OnSelectedValueDoubleClick(object sender, EventArgs e)
        {
            BtnShotMapDraw_Click(null, null);
        }


        #endregion

         
        #region [ Method ]  


        public void Draw()
        {
            DACrux.Common.RO.ComConfiguration oComConfig = null;

            try
            {
                if (m_dsMap == null)
                    return;

                oImageList = new Base.TestImageList();

                StatusMessage("Map 을 그리는 중입니다.");

                InitBinData();

                m_ewMap.VisibleShotAlignPoint = false;
                m_ewMap.SetDefaultColors();
                SetMapBinColor(m_dsMap);

                m_ewMap.WaferSize = DACrux.Base.Convert.doubleParse(m_dsMap.Tables["RECIPE"].Rows[0]["WAFER_SIZE"].ToString());

                m_ewMap.DieSizeX = DACrux.Base.Convert.doubleParse(m_dsMap.Tables["RECIPE"].Rows[0]["CHIP_SIZE_X"].ToString());
                m_ewMap.DieSizeY = DACrux.Base.Convert.doubleParse(m_dsMap.Tables["RECIPE"].Rows[0]["CHIP_SIZE_Y"].ToString());

                m_ewMap.OriginIndexX = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["ORIGIN_INDEX_X"].ToString());
                m_ewMap.OriginIndexY = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["ORIGIN_INDEX_Y"].ToString());

                m_ewMap.OriginX = DACrux.Base.Convert.doubleParse(m_dsMap.Tables["RECIPE"].Rows[0]["ORIGIN_MICRO_X"].ToString());
                m_ewMap.OriginY = DACrux.Base.Convert.doubleParse(m_dsMap.Tables["RECIPE"].Rows[0]["ORIGIN_MICRO_Y"].ToString());

                m_ewMap.FirstDieX = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["FIRST_INDEX_X"].ToString());
                m_ewMap.FirstDieY = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["FIRST_INDEX_Y"].ToString());

                m_ewMap.NotchAngle = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["ANGLE"].ToString());
                m_ewMap.EdgeSize = DACrux.Base.Convert.doubleParse(m_dsMap.Tables["RECIPE"].Rows[0]["EDGE_SIZE"].ToString());
                m_ewMap.NotchType = DACrux.Base.Notch.Notch; //m_dsMap.Tables["RECIPE"].Rows[0]["NOTCH_TYPE"].ToString().Equals("0") ? DACrux.Base.Notch.Flat : DACrux.Base.Notch.Notch;

                m_ewMap.DieMinX = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["DIE_INDEX_MIN_X"].ToString());
                m_ewMap.DieMinY = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["DIE_INDEX_MIN_Y"].ToString());
                m_ewMap.DieMaxX = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["DIE_INDEX_MAX_X"].ToString());
                m_ewMap.DieMaxY = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["DIE_INDEX_MAX_Y"].ToString());

                int iXYDir = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["XY_DIRECTION"].ToString());
                switch (iXYDir)
                {
                    case 0:
                        m_ewMap.XYDirect = DACrux.Base.XYDirection.LeftTop;
                        break;
                    case 1:
                        m_ewMap.XYDirect = DACrux.Base.XYDirection.LeftBottom;
                        break;
                    case 2:
                        m_ewMap.XYDirect = DACrux.Base.XYDirection.RightBottom;
                        break;
                    case 3:
                        m_ewMap.XYDirect = DACrux.Base.XYDirection.RightTop;
                        break;
                }
                m_ewMap.DieCalculation(true);

                // Map Bin Row Data ===================================================================================================
                m_ewMap.DieClear();
                m_ewMap.DataSource = m_dsMap.Tables["MAPDATA"];
                m_ewMap.VisibleVIFail = true;
                m_ewMap.VisibleFocusDie = true;
                m_ewMap.VIMember = "BIN";
                m_ewMap.DieFocusingType = Map.FocusType.Arraw;

                //Shot 관련 정보확인 및 Draw
                m_ewMap.VisibleShot = false;
                m_ewMap.ShotArrayX = 1;
                m_ewMap.ShotArrayY = 1;
                m_ewMap.ShotStartX = 1;
                m_ewMap.ShotStartY = 1;

                if (m_dsMap.Tables.IndexOf("SHOT_DEF") >= 0 && m_dsMap.Tables["SHOT_DEF"] != null && m_dsMap.Tables["SHOT_DEF"].Rows.Count > 0)
                {
                    int iTemp = 0;
                    m_ewMap.VisibleShot = true;
                    foreach (DataRow dr in m_dsMap.Tables["SHOT_DEF"].Rows)
                    {
                        if (int.TryParse(dr["ST_XCNT"].ToString(), out iTemp) == true)
                            m_ewMap.ShotArrayX = iTemp;

                        if (int.TryParse(dr["ST_YCNT"].ToString(), out iTemp) == true)
                            m_ewMap.ShotArrayY = iTemp;

                        if (int.TryParse(dr["ST_START_X"].ToString(), out iTemp) == true)
                            m_ewMap.ShotStartX = iTemp;

                        if (int.TryParse(dr["ST_START_Y"].ToString(), out iTemp) == true)
                            m_ewMap.ShotStartY = iTemp;
                    }
                }

                //Mark die 관련 정보 수집
                if (m_dsMap.Tables.IndexOf("MARKDATA") >= 0 && m_dsMap.Tables["MARKDATA"] != null && m_dsMap.Tables["MARKDATA"].Rows.Count > 0)
                {
                    foreach (DataRow dr in m_dsMap.Tables["MARKDATA"].Rows)
                    {
                        int iX = 0;
                        int iY = 0;

                        if (int.TryParse(dr["INDEX_X"].ToString(), out iX) == false)
                            continue;

                        if (int.TryParse(dr["INDEX_Y"].ToString(), out iY) == false)
                            continue;

                        if (m_ewMap.IndexOf(iX, iY) < 0)
                        {
                            Base.Die oDie = new Base.Die();
                            oDie.IndexX = iX;
                            oDie.IndexY = iY;
                            oDie.DieProp = 2; // 실제 있는 Die 는 1번 Mark Die 는 2 번
                            oDie.BinNumber = 0;
                            m_ewMap.AddDie(oDie);
                        }
                    }
                }

                //=================================================================================================================================
                //사용자 별로 정의된 색상 및 Wafer 정보를 출력 한다.
                //=================================================================================================================================
                oComConfig = new DACrux.Common.RO.ComConfiguration();
                DataTable dtInfo = oComConfig.GetConfigurationUser(DACrux.Base.GlobalVariable.Factory, "TEST_OPTION", DACrux.Base.GlobalVariable.UserID);
                if (dtInfo != null && dtInfo.Rows.Count > 0 && m_dsMap.Tables.IndexOf("WAFER_INFO") > -1 && m_dsMap.Tables["WAFER_INFO"] != null && m_dsMap.Tables["WAFER_INFO"].Rows.Count > 0)
                {
                    List<string> sWaferInfo = new List<string>();

                    foreach (DataRow drInfo in dtInfo.Rows)
                    {
                        if (m_dsMap.Tables["WAFER_INFO"].Columns.IndexOf(drInfo["NAME"].ToString()) > -1)
                            sWaferInfo.Add(string.Format("{0}  :{1}", drInfo["VALUE"], m_dsMap.Tables["WAFER_INFO"].Rows[0][drInfo["NAME"].ToString()].ToString()));
                    }

                    m_ewMap.SetInfomation(sWaferInfo.ToArray());
                }

                //Wafer Information 사용 여부
                dtInfo = oComConfig.GetConfigurationUser(DACrux.Base.GlobalVariable.Factory, "TEST_OPTION_ENABLE", DACrux.Base.GlobalVariable.UserID);
                if (dtInfo != null && dtInfo.Rows.Count > 0)
                {
                    if (dtInfo.Rows[0]["VALUE"].ToString() == "N")
                        m_ewMap.SetInfomation(null);
                }

                //사용자 별로 정의된 색상 및 Wafer 정보를 출력 한다.
                DataTable dtMapOption = oComConfig.SelectDefectMapConfig(DACrux.Base.GlobalVariable.Factory, DACrux.Base.GlobalVariable.UserID);
                if (dtMapOption != null && dtMapOption.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtMapOption.Rows)
                    {
                        string strType = dr["NAME"].ToString();
                        Color crType = ColorTranslator.FromHtml(dr["VALUE"].ToString());

                        switch (strType)
                        {
                            //Wafer Base Color
                            case "WAFER_TEST_MAP_BG":
                                m_ewMap.WaferColor = crType;
                                break;
                            //Wafer Border Line Color
                            case "WAFER_TEST_MAP_LINE":
                                m_ewMap.DieBorderColor = crType;
                                break;

                        }
                    }
                }

                FarPoint.Win.Spread.CellType.NumberCellType ct = new FarPoint.Win.Spread.CellType.NumberCellType();
                ct.DecimalPlaces = 0;
                m_ewMap.Redraw();
                m_ewMap.Refresh();
                m_ewMap.Width = m_ewMap.Width + 1; /// Size를 자동으로 조절하게 하는 Trip
                m_ewMap.WaferDrawMode = Map.MapMode.Fit;

                SetKeymap(m_dsMap.Tables["ALTER_BIN"]);

                if (m_dsMap.Tables.IndexOf("WAFER_INFO") > -1 && m_dsMap.Tables["WAFER_INFO"] != null && m_dsMap.Tables["WAFER_INFO"].Rows.Count > 0)
                {
                    WAFER_SEQ = m_dsMap.Tables["WAFER_INFO"].Rows[0]["WAFER_SEQ"].ToString();
                    LOT_ID = m_dsMap.Tables["WAFER_INFO"].Rows[0]["LOT_ID"].ToString();
                    PROGRAM = m_dsMap.Tables["WAFER_INFO"].Rows[0]["PROGRAM"].ToString();
                    DEVICE = m_dsMap.Tables["WAFER_INFO"].Rows[0]["PRODUCT"].ToString();
                    TESTAREA = m_dsMap.Tables["WAFER_INFO"].Rows[0]["TESTAREA"].ToString();
                    WAFERID = m_dsMap.Tables["WAFER_INFO"].Rows[0]["WAFER_ID"].ToString();
                }

                if (chAlterLot.Checked)
                {
                    TESTAREA = "AVI";
                    PROGRAM = "SCOPE";
                    DEVICE = TxtDevice.Text;
                }

                //Draw 시 Serial 통신 연결 시도 한다.
                if(chkAutoConnect.Checked)
                    BtnSerialConnect_Click(null, null);

                //조회 시 기존 Backup 경로 삭제 후 다시 만든다.
                System.IO.DirectoryInfo oDirectory = new System.IO.DirectoryInfo(strBacupPath);
                oDirectory = new System.IO.DirectoryInfo(strBacupPath);
                if (oDirectory.Exists)
                    oDirectory.Delete(true);

                System.Threading.Thread.Sleep(10);
                oDirectory.Create();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                StatusMessage(null);
            }
        }


        private void SetMapBinColor(DataSet dsMap)
        {
            try
            {
                if (dsMap.Tables.IndexOf("MASTER_BIN") < 0 || dsMap.Tables["MASTER_BIN"] == null)
                    return;

                DataTable dtBinSum = dsMap.Tables["MASTER_BIN"];

                for (int i = 0; i < dtBinSum.Rows.Count; i++)
                {
                    int nBin = -1;

                    if (int.TryParse(dtBinSum.Rows[i]["BIN"].ToString(), out nBin))
                    {
                        if (string.IsNullOrEmpty(dtBinSum.Rows[i]["BIN_COLOR"].ToString().Trim()) == false && dtBinSum.Rows[i]["BIN_COLOR"].ToString() != "#FFFFFF")
                        {
                            if (nBin > -1)
                            {
                                m_ewMap.SetColor(nBin, ColorTranslator.FromHtml(dtBinSum.Rows[i]["BIN_COLOR"].ToString()));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetKeymap(DataTable dtBin)
        {
            try
            {
                if (dtBin == null || dtBin.Rows.Count <= 0)
                    return;

                for (int i = 1; i < 20; i++)
                {
                    ((TextBox)tpMapBalance.Controls[string.Format("txtVal{0}", i)]).Text = "-1";
                    ((ComboBox)tpMapBalance.Controls[string.Format("cbDesc{0}", i)]).BeginUpdate();
                    ((ComboBox)tpMapBalance.Controls[string.Format("cbDesc{0}", i)]).DisplayMember = "DESCRIPTION";
                    ((ComboBox)tpMapBalance.Controls[string.Format("cbDesc{0}", i)]).ValueMember = "BIN_DESC";
                    ((ComboBox)tpMapBalance.Controls[string.Format("cbDesc{0}", i)]).DataSource = dtBin.Copy();
                    ((ComboBox)tpMapBalance.Controls[string.Format("cbDesc{0}", i)]).SelectedIndex = dtBin.Rows.Count <= i ? -1 : i;
                    ((ComboBox)tpMapBalance.Controls[string.Format("cbDesc{0}", i)]).DropDownHeight = ((ComboBox)tpMapBalance.Controls[string.Format("cbDesc{0}", i)]).ItemHeight * 10;
                    ((ComboBox)tpMapBalance.Controls[string.Format("cbDesc{0}", i)]).DropDownWidth = ((ComboBox)tpMapBalance.Controls[string.Format("cbDesc{0}", i)]).Size.Width * 5;
                    ((ComboBox)tpMapBalance.Controls[string.Format("cbDesc{0}", i)]).EndUpdate();

                    m_ewMap.SetVIValue(i, -1);

                    if(((ComboBox)tpMapBalance.Controls[string.Format("cbDesc{0}", i)]).SelectedIndex > -1)
                        cbDesc_SelectionChangeCommitted(((ComboBox)tpMapBalance.Controls[string.Format("cbDesc{0}", i)]), null);
                    
                    ((NumericUpDown)tpMapBalance.Controls[string.Format("txtCnt{0}", i)]).Value = 0;

                }

                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private string AVIImageCapture()
        {
            if (videoCaptureControls.ConnectState == false)
                return string.Empty;

            strBacupFileName = string.Format("{0}_{1}_{2}.jpg", DEVICE, txtXIndex.Text, txtYIndex.Text);

            System.Threading.Thread.Sleep(10);

            System.IO.FileInfo oFile = new System.IO.FileInfo(System.IO.Path.Combine(strBacupPath, strBacupFileName));
            if (oFile.Exists)
                oFile.Delete();

            videoCaptureControls.ImageCapture(System.IO.Path.Combine(strBacupPath, strBacupFileName));
            System.Threading.Thread.Sleep(10);
            return System.IO.Path.Combine(strBacupPath, strBacupFileName);
        }

        private string AVIImageMultiCapture()
        {
            DateTime dtTime = DateTime.Now;
            if (videoCaptureControls.ConnectState == false)
                return string.Empty;

            strBacupFileName = string.Format("{0}_{1}_{2}.jpg", txtXIndex.Text, txtYIndex.Text, DateTime.Now.ToString("yyyyMMddHHmmssFFF"));

            System.IO.FileInfo oFile = new System.IO.FileInfo(System.IO.Path.Combine(strBacupPath, strBacupFileName));
            if (oFile.Exists)
                oFile.Delete();

            videoCaptureControls.ImageCapture(System.IO.Path.Combine(strBacupPath, strBacupFileName));
            Application.DoEvents();
            System.Threading.Thread.Sleep(50);

            return System.IO.Path.Combine(strBacupPath, strBacupFileName);
        }

        private void SerialOptionEnable(bool bEnable)
        {
            BtnSerialConnect.Enabled = bEnable;
            BtnOption.Enabled = bEnable;
            BtnSerialDisConnect.Enabled = !bEnable;
            tpMapBalance.Enabled = bEnable;
            numPer.Enabled = bEnable;
            //chkBasicMap.Enabled = bEnable;
        }


        private void chAlterLot_CheckedChanged(object sender, EventArgs e)
        {
            TxtLotID.ReadOnly = !chAlterLot.Checked;
            cmbWaferNo.Enabled = chAlterLot.Checked;
            dlbDeviceList.Enabled = chAlterLot.Checked;
            BtnShotMapDraw.Enabled = chAlterLot.Checked;

            if (chAlterLot.Checked == false)
            {
                TxtLotID.Text = "";
                cmbWaferNo.Text = "1";
                TxtDevice.Text = "";
            }
        }

        public void InitBinData()
        {
            for (int i = 1; i < 20; i++)
            {
                ((TextBox)tpMapBalance.Controls[string.Format("txtVal{0}", i)]).Text = "";
                ((TextBox)tpMapBalance.Controls[string.Format("txtVal{0}", i)]).BackColor = System.Drawing.SystemColors.Control;

                ((ComboBox)tpMapBalance.Controls[string.Format("cbDesc{0}", i)]).DataSource = null;

                ((NumericUpDown)tpMapBalance.Controls[string.Format("txtCnt{0}", i)]).Value = 0;
                ((NumericUpDown)tpMapBalance.Controls[string.Format("txtCnt{0}", i)]).Refresh();
            }

        }

        private void BindingMapList()
        {
            DACrux.TEST.RO.ProbeAdmin oProbe = new DACrux.TEST.RO.ProbeAdmin();
            DataTable dt = oProbe.GetMapIDListNotDelete();

            if (dt != null && dt.Rows.Count > 0)
            {
                dlbDeviceList.DisplayMember = "DISP_MAPID";
                dlbDeviceList.ValueMember = "MAPID";
                dlbDeviceList.DataSource = dt;
            }
        }

        #endregion
      
    }
}
