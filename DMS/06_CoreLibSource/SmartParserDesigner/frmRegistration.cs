using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using DACrux.SP.Common;
using System.Resources;
using DACrux.SP.Controls;
using System.IO;
using System.Data.OleDb;
using System.Data.Common;

namespace SmartParser.Designer
{
    public partial class frmRegistration : Form
    {
        #region " Member Field & Property "

        private bool isDragging = false;
        private int iPos = -1;

        private Analysis analysis = null;
        static public string GoodBin = "1";
        static public string NullBin = " ";
        static public string FailBin = "F";
        static public string strTop = "Top";
        static public string strBottom = "Bottom";
        static public string strLeft = "Left";
        static public string strRight = "Right";
        string WaferIdRegex = null;
        string WaferNumRegex = null;
        string RunIdRegex = null;
        string[] ListBaseNum = { "bin", "Wafer_Num", "Product", "FlatZone"};
        string[] ListBaseId = { "bin", "Wafer_ID", "Product", "FlatZone" };
        // 구성요소 창 기본 속성 List 셋팅용
        string[] ListSec = { "bin", "Wafer_ID", "Wafer_Num", "Product", "FlatZone", "Run_ID" };
        //string[] ListBaseNum = { "bin", "Wafer_Num", "Product", "FlatZone", "X", "Y", "Start_Time", "End_TIme", "Total", "Program_Name", "Probe_Card", "Operator", "Tester_ID" };
        //string[] ListBaseId = { "bin", "Wafer_ID", "Product", "FlatZone", "X", "Y", "Start_Time", "End_TIme", "Total", "Program_Name", "Probe_Card", "Operator", "Tester_ID" };
        //// 구성요소 창 기본 속성 List 셋팅용
        //string[] ListSec = { "bin", "Wafer_ID", "Wafer_Num", "Product", "FlatZone", "X", "Y", "Total", "Start_Time", "End_TIme", "Program_Name", "Probe_Card", "Operator", "Tester_ID", "Run_ID" };
        static private Analysis analysisConvert = null;
        static private RegexEntity entitys;
        static private string strFilename;
        public RegexEntity entity
        {
            get { return entitys; }
            set { entitys = value; }
        }
        public string Filename
        {
            get { return strFilename; }
            set { strFilename = value; }
        }
        public Analysis analy
        {
            get { return analysisConvert; }
            set { analysisConvert = value; }
        }
        #endregion

        #region " Creator "

        public frmRegistration(ActiveTabItems activeTab)
        {
            DACrux.SP.Common.MultiLang funclang = new MultiLang();
            funclang.CheckLang();
            InitializeComponent();
            usercontrollang();
            ResourceManager rm = new ResourceManager("DACrux.SP.Controls.res", System.Reflection.Assembly.Load("DACrux.SP.Controls"));

            ImageList imgList = new ImageList();
            imgList.ImageSize = new Size(15, 15);
            imgList.Images.Add((Image)rm.GetObject("entity"));
            imgList.Images.Add((Image)rm.GetObject("section"));

            lvEntity.SmallImageList = imgList;
            tvExplorer.ImageList = imgList;

            lvEntity.Columns.Add("Name", 40);
            lvEntity.Columns.Add("Default", 50);
            lvEntity.Columns.Add("Force", 30);

            //lvEntity.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(lvEntity_ItemSelectionChanged);
            lvMatch.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(lvMatch_ItemSelectionChanged);

            switch (activeTab)
            {
                case ActiveTabItems.Entity:
                    tabSetup.SelectTab(tabPageEntity.Name);
                    break;
            }

            analysis = Analysis.GetInstance();
            analysis.CharCount = 1;
            analysis.Entities.ItemAdded += new ItemAddedEventHandler<RegexEntity>(EntitiesItemAdded);
            analysis.Entities.ItemRemoved += new ItemRemovedEventHandler<RegexEntity>(EntitiesItemRemoved);

            uclRegexEntity.FindRequested += new DACrux.SP.Controls.EntityFindRequestedEventHandler(uclRegexEntity_EntityFindRequested);
            uclRegexEntity.SaveRequested += new DACrux.SP.Controls.RegexEntitySaveRequestedEventHandler(uclRegexEntity_SaveRequested);
            uclRegexEntity.RemoveRequested += new DACrux.SP.Controls.RegexEntityRemoveRequestedEventHandler(uclRegexEntity_RemoveRequested);

            tabSetup.SizeChanged += new EventHandler(tabSetup_SizeChanged);
        }

        void fm_changecount(int i)
        {
            analysis.CharCount = i;
        }

        void usercontrollang()
        {
            uclRegexEntity.array = DACrux.SP.Common.MultiLang.SelectLang["array"];
            uclRegexEntity.find = DACrux.SP.Common.MultiLang.SelectLang["Convert"];
            uclRegexEntity.label = DACrux.SP.Common.MultiLang.SelectLang["Label"];
            uclRegexEntity.name = DACrux.SP.Common.MultiLang.SelectLang["Name"];
            uclRegexEntity.remove = DACrux.SP.Common.MultiLang.SelectLang["Remove"];
            uclRegexEntity.save = DACrux.SP.Common.MultiLang.SelectLang["Save"];
            uclRegexEntity.sep = DACrux.SP.Common.MultiLang.SelectLang["Separator"];
            uclRegexEntity.streammode = DACrux.SP.Common.MultiLang.SelectLang["Stream"];
            uclRegexEntity.valuegroup = DACrux.SP.Common.MultiLang.SelectLang["Value Group"];
            uclRegexEntity.valuemust = DACrux.SP.Common.MultiLang.SelectLang["Value must exists"];
            uclRegexEntity.valuetype = DACrux.SP.Common.MultiLang.SelectLang["Value Type"];
            uclRegexEntity.dec = DACrux.SP.Common.MultiLang.SelectLang["\\d"];
            uclRegexEntity.endline = DACrux.SP.Common.MultiLang.SelectLang["$"];
            uclRegexEntity.etc = DACrux.SP.Common.MultiLang.SelectLang["Etc"];
            uclRegexEntity.grouping = DACrux.SP.Common.MultiLang.SelectLang["Grouping"];
            uclRegexEntity.groupname = DACrux.SP.Common.MultiLang.SelectLang["(?<name>subexpression)"];
            uclRegexEntity.groupequal = DACrux.SP.Common.MultiLang.SelectLang["k<name>"];
            uclRegexEntity.Ignorecase = DACrux.SP.Common.MultiLang.SelectLang["Ignore Case"];
            uclRegexEntity.newline = DACrux.SP.Common.MultiLang.SelectLang["\\n"];
            uclRegexEntity.nfromtime = DACrux.SP.Common.MultiLang.SelectLang["{n,m}"];
            uclRegexEntity.nleasttime = DACrux.SP.Common.MultiLang.SelectLang["{n, }"];
            uclRegexEntity.nondec = DACrux.SP.Common.MultiLang.SelectLang["\\D "];
            uclRegexEntity.nonspace = DACrux.SP.Common.MultiLang.SelectLang["\\S"];
            uclRegexEntity.nonword = DACrux.SP.Common.MultiLang.SelectLang["\\W"];
            uclRegexEntity.ntime = DACrux.SP.Common.MultiLang.SelectLang["{n}"];
            uclRegexEntity.one = DACrux.SP.Common.MultiLang.SelectLang["+"];
            uclRegexEntity.quanti = DACrux.SP.Common.MultiLang.SelectLang["Quantifiers"];
            uclRegexEntity.regular = DACrux.SP.Common.MultiLang.SelectLang["Regular Expression"];
            uclRegexEntity.returnhome = DACrux.SP.Common.MultiLang.SelectLang["\\r"];
            uclRegexEntity.space = DACrux.SP.Common.MultiLang.SelectLang["\\s"];
            uclRegexEntity.startline = DACrux.SP.Common.MultiLang.SelectLang["^"];
            uclRegexEntity.tab = DACrux.SP.Common.MultiLang.SelectLang["\\t"];
            uclRegexEntity.word = DACrux.SP.Common.MultiLang.SelectLang["\\s"];
            uclRegexEntity.zero = DACrux.SP.Common.MultiLang.SelectLang["*"];
            uclRegexEntity.zerone = DACrux.SP.Common.MultiLang.SelectLang["?"];
            uclRegexEntity.Def = DACrux.SP.Common.MultiLang.SelectLang["Default"];
        }

        #endregion

        #region " Event Handler "

        protected override void OnLoad(EventArgs e)
        {
            tabTextViewer.TabPages.Clear();
            DACrux.SP.Controls.SmartTextViewer stv = null;

            analysis.Navigators.Clear();
            try
            {
                foreach (KeyValuePair<string, string> file in analysis.SampleFiles)
                {
                    try
                    {
                        tabTextViewer.TabPages.Add(file.Key, file.Key);
                        stv = new DACrux.SP.Controls.SmartTextViewer();
                        stv.TotalContent = file.Value;
                        analysis.Navigators.Add(file.Key, new RegexNavigator(stv.TotalContent));

                        stv.PageChanged += new DACrux.SP.Controls.PageLister.PageChangeHandler(PageChanged);
                        tabTextViewer.TabPages[file.Key].Controls.Add(stv);
                        stv.Dock = DockStyle.Fill;
                        stv.BringToFront();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error occured while loading " + file.Key + ". (" + ex.Message + ")", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        continue;
                    }
                }

                analysis.CurrentFile = tabTextViewer.SelectedTab.Text;
                tabTextViewer.SelectedIndexChanged += new EventHandler(tabTextViewer_SelectedIndexChanged);
                tabSetup.SelectedIndexChanged += new EventHandler(tabSetup_SelectedIndexChanged);

                LoadEntityList(analysis);
                RefreshTreeView(analysis);

                foreach (string id in ListSec) //구성요소 창 기본 속성
                {
                    DACrux.SP.Common.RegexEntity ReCon = new RegexEntity();
                    ReCon.StreamMode = StreamModeItem.Stream;
                    ReCon.Name = id;
                    if (!analysis.Entities.Exists(ReCon.Name))
                    {
                        analysis.Entities.Add(ReCon);
                    }
                }

                uclTitleMatch.MouseDown += new MouseEventHandler(uclTitle_MouseDown);
                uclTitleMatch.MouseMove += new MouseEventHandler(uclTitleMatch_MouseMove);
                uclTitleMatch.MouseUp += new MouseEventHandler(uclTitle_MouseUp);

                uclTitleEntity.MouseDown += new MouseEventHandler(uclTitle_MouseDown);
                uclTitleEntity.MouseMove += new MouseEventHandler(uclTitleEntity_MouseMove);
                uclTitleEntity.MouseUp += new MouseEventHandler(uclTitle_MouseUp);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void stvHana_PageChanged(int currentPage)
        {
            throw new NotImplementedException();
        }

        void tabSetup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabSetup.SelectedTab == tabPageExplorer)
                RefreshTreeView(analysis);
            else if (tabSetup.SelectedTab == tabPageConvert)
                ConvertAnalysis();
        }

        void tabTextViewer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (tabTextViewer.TabCount < 1)
                {
                    analysis.CurrentFile = string.Empty;
                    return;
                }

                analysis.CurrentFile = tabTextViewer.SelectedTab.Text;

                if (tabSetup.SelectedTab == tabPageExplorer && tvExplorer.SelectedNode != null)
                    SetExplorerMatchListViewItems(tvExplorer.SelectedNode.Tag as ISectionItem);
            }
            catch (Exception ex)
            {
                DACrux.SP.Common.Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        void tabSetup_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                if (tabSetup.Width < 100)
                    return;

                lvEntity.Columns[1].Width = 50;
                lvEntity.Columns[2].Width = 40;
                lvEntity.Columns[0].Width = tabSetup.Width - lvEntity.Columns[1].Width - lvEntity.Columns[2].Width - 20;
            }
            catch (Exception ex)
            {
                DACrux.SP.Common.Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        void PageChanged(int currentPage)
        {
            if (tabTextViewer.TabCount < 1)
                return;
            try
            {
                //SelectText();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region [ Entity ]

        private void lvEntity_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    ListViewHitTestInfo lvhti = lvEntity.HitTest(e.Location);
                    if (lvhti.Item == null)
                    {
                        uclRegexEntity.EntityContent = null;
                        SelectText(string.Empty, RegexOptions.None);
                    }
                    else
                    {
                        if (lvhti.Item.Tag is RegexEntity)
                        {
                            RegexEntity entity = lvhti.Item.Tag as RegexEntity;
                            uclRegexEntity.EntityContent = entity;
                            //frmConvert frm = new frmConvert();
                            //frm.entity = entity;
                            //frm.analy = analysis;
                            //frm.Filename = tabTextViewer.SelectedTab.Text;
                            entitys = entity;
                            analysisConvert = analysis;
                            strFilename = tabTextViewer.SelectedTab.Text;
                            var lst = from DACrux.SP.Common.Token mt in analysis.Navigators[tabTextViewer.SelectedTab.Text].EntityCaptureCollection[entity]
                                      select new int[] { mt.Index, mt.Length };

                            SelectText(lst.ToList());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DACrux.SP.Common.Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        void lvEntity_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            try
            {
                if (!e.IsSelected)
                {
                    uclRegexEntity.EntityContent = null;
                    SelectText(string.Empty, RegexOptions.None);
                    return;
                }

                if (e.Item.Tag is RegexEntity)
                {
                    RegexEntity entity = e.Item.Tag as RegexEntity;
                    uclRegexEntity.EntityContent = entity;
                    //frmConvert frm = new frmConvert();
                    //frm.entity = entity;
                    //frm.analy = analysis;
                    //frm.Filename = tabTextViewer.SelectedTab.Text;
                    entitys = entity;
                    analysisConvert = analysis;
                    strFilename = tabTextViewer.SelectedTab.Text;
                    var lst = from DACrux.SP.Common.Token mt in analysis.Navigators[tabTextViewer.SelectedTab.Text].EntityCaptureCollection[entity]
                              select new int[] { mt.Index, mt.Length };

                    SelectText(lst.ToList());

                }
            }
            catch (Exception ex)
            {
                DACrux.SP.Common.Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        void uclRegexEntity_EntityFindRequested(RegexEntity entity)
        {
            int WaferCount = 0;
            int WaferIDCk = 0;
            int WaferIDCount = 0;
            int WaferNumCk = 0;
            int WaferNumCount = 0;
            int BinCount = 0;
            int TotalCkn = 0;
            string strCurrentPageText = string.Empty;
            StringBuilder sb = null;
            string strCurrent = null;

            try
            {
                if (analysis.Entities[lvEntity.Items[0].Name] == null)
                    return;
                foreach (TabPage tp in tabTextViewer.TabPages)
                {
                    if (tp.Text == analysis.CurrentFile + "_HANA")
                    {
                        tabTextViewer.TabPages.Remove(tabTextViewer.TabPages[analysis.CurrentFile + "_HANA"]);
                        break;
                    }
                }

                var dic = analysis.Navigators[analysis.CurrentFile.Replace("_HANA", "")].EntityCaptureCollection;
                Dictionary<RegexEntity, List<Token>> dictemp = new Dictionary<RegexEntity, List<Token>>();
                strCurrentPageText = analysis.HanaSampleFiles[analysis.CurrentFile + "_HANA"];

                sb = new StringBuilder();

                DataTable dt = new DataTable();
                DataTable ndt = new DataTable();
                DataTable bindt = new DataTable();
                DataTable wadt = new DataTable();
                DataTable prdt = new DataTable();
                DataTable xdt = new DataTable();
                DataTable ydt = new DataTable();
                DataTable fldt = new DataTable();
                DataTable todt = new DataTable();
                DataTable wandt = new DataTable();
                DataTable rundt = new DataTable();

                DataRow bindr = null;
                DataRow wadr = null;
                DataRow prdr = null;
                DataRow xdr = null;
                DataRow ydr = null;
                DataRow fldr = null;
                DataRow todr = null;
                DataRow wandr = null;

                dt.Columns.Add("name");
                dt.Columns.Add("Start", typeof(int));
                dt.Columns.Add("End", typeof(int));
                ndt.Columns.Add("name");
                ndt.Columns.Add("Start", typeof(int));
                ndt.Columns.Add("End", typeof(int));
                bindt.Columns.Add("name");
                bindt.Columns.Add("Start", typeof(int));
                bindt.Columns.Add("End", typeof(int));
                wadt.Columns.Add("name");
                wadt.Columns.Add("Start", typeof(int));
                wadt.Columns.Add("End", typeof(int));
                prdt.Columns.Add("name");
                prdt.Columns.Add("Start", typeof(int));
                prdt.Columns.Add("End", typeof(int));
                xdt.Columns.Add("name");
                xdt.Columns.Add("Start", typeof(int));
                xdt.Columns.Add("End", typeof(int));
                ydt.Columns.Add("name");
                ydt.Columns.Add("Start", typeof(int));
                ydt.Columns.Add("End", typeof(int));
                fldt.Columns.Add("name");
                fldt.Columns.Add("Start", typeof(int));
                fldt.Columns.Add("End", typeof(int));
                todt.Columns.Add("name");
                todt.Columns.Add("Start", typeof(int));
                todt.Columns.Add("End", typeof(int));
                wandt.Columns.Add("name");
                wandt.Columns.Add("Start", typeof(int));
                wandt.Columns.Add("End", typeof(int));
                rundt.Columns.Add("name");
                rundt.Columns.Add("Start", typeof(int));
                rundt.Columns.Add("End", typeof(int));

                foreach (KeyValuePair<RegexEntity, List<Token>> k in dic)
                {
                    if (k.Value.Count == 0 || k.Value[0].Length <= 0)
                    {
                        foreach (KeyValuePair<RegexEntity, List<Token>> m in dic)
                        {
                            if (m.Key.Name == "Wafer_ID" && m.Value[0].Length <= 0)
                            {
                                dictemp.Add(k.Key, dic[lvEntity.Items["Wafer_Num"].Tag as RegexEntity]);
                                WaferNumCk++;
                            }
                            if (m.Key.Name == "Wafer_Num" && m.Value[0].Length <= 0)
                            {
                                dictemp.Add(k.Key, dic[lvEntity.Items["Wafer_ID"].Tag as RegexEntity]);
                                WaferIDCk++;
                            }
                        }
                    }
                    else
                        dictemp.Add(k.Key, k.Value);
                }

                DataSet ds = new DataSet();
                ds = SortDataTable(dt, dictemp, ListBaseNum, ListBaseId, WaferNumCk);
                ndt = ds.Tables[0];
                dt = ds.Tables[1];
                rundt = ds.Tables[2];

                for (int k = 0; k < dt.Rows.Count; k++)
                {
                    if (WaferIDCk > 0)
                    {
                        if (dt.Rows[k]["name"].Equals("Wafer_ID"))
                            WaferIDCount += 1;
                    }
                    if (WaferNumCk > 0)
                    {
                        if (dt.Rows[k]["name"].Equals("Wafer_Num"))
                            WaferNumCount += 1;
                    }
                    if (dt.Rows[k]["name"].Equals("bin"))
                        BinCount += 1;
                }

                WaferCount = Math.Max(WaferIDCount, WaferNumCount);



                int bintp = 0, tp1 = 0, tp2 = 0, tp3 = 0, tp4 = 0, tp5 = 0, tp6 = 0, tp7 = 0;

                for (int ecnt = 0; ecnt < WaferCount; ecnt++)
                {
                    wadr = wadt.NewRow();
                    wadr["name"] = string.Empty;
                    wadr["Start"] = 0;
                    wadr["End"] = 0;
                    prdr = prdt.NewRow();
                    prdr["name"] = string.Empty;
                    prdr["Start"] = 0;
                    prdr["End"] = 0;
                    xdr = xdt.NewRow();
                    xdr["name"] = string.Empty;
                    xdr["Start"] = 0;
                    xdr["End"] = 0;
                    ydr = ydt.NewRow();
                    ydr["name"] = string.Empty;
                    ydr["Start"] = 0;
                    ydr["End"] = 0;
                    fldr = fldt.NewRow();
                    fldr["name"] = string.Empty;
                    fldr["Start"] = 0;
                    fldr["End"] = 0;
                    todr = todt.NewRow();
                    todr["name"] = string.Empty;
                    todr["Start"] = 0;
                    todr["End"] = 0;
                    wandr = wandt.NewRow();
                    wandr["name"] = string.Empty;
                    wandr["Start"] = 0;
                    wandr["End"] = 0;

                    wadt.Rows.Add(wadr);
                    prdt.Rows.Add(prdr);
                    xdt.Rows.Add(xdr);
                    ydt.Rows.Add(ydr);
                    fldt.Rows.Add(fldr);
                    todt.Rows.Add(todr);
                    wandt.Rows.Add(wandr);
                }

                for (int bcnt = 0; bcnt < BinCount; bcnt++) //bin dt 생성
                {
                    bindr = bindt.NewRow();
                    bindr["name"] = string.Empty;
                    bindr["Start"] = 0;
                    bindr["End"] = 0;

                    bindt.Rows.Add(bindr);
                }

                for (int i = 0; i < dt.Rows.Count; i++) //dt에 값이 있는 것만 생성
                {
                    string tempNm = dt.Rows[i]["name"].ToString();
                    switch (tempNm)
                    {
                        case "bin":
                            for (int j = 0; j < 3; j++)
                                bindt.Rows[bintp][j] = dt.Rows[i][j];
                            bintp++;
                            break;
                        case "Wafer_ID":
                            for (int j = 0; j < 3; j++)
                                wadt.Rows[tp1][j] = dt.Rows[i][j];
                            tp1++;
                            break;
                        case "Product":
                            for (int j = 0; j < 3; j++)
                                prdt.Rows[tp2][j] = dt.Rows[i][j];
                            tp2++;
                            break;
                        case "X":
                            for (int j = 0; j < 3; j++)
                                xdt.Rows[tp3][j] = dt.Rows[i][j];
                            tp3++;
                            break;
                        case "Y":
                            for (int j = 0; j < 3; j++)
                                ydt.Rows[tp4][j] = dt.Rows[i][j];
                            tp4++;
                            break;
                        case "FlatZone":
                            for (int j = 0; j < 3; j++)
                                fldt.Rows[tp5][j] = dt.Rows[i][j];
                            tp5++;
                            break;
                        case "Total":
                            for (int j = 0; j < 3; j++)
                                todt.Rows[tp6][j] = dt.Rows[i][j];
                            tp6++;
                            break;
                        case "Wafer_Num":
                            for (int j = 0; j < 3; j++)
                                wandt.Rows[tp7][j] = dt.Rows[i][j];
                            tp7++;
                            break;
                    }
                }

                for (int i = 0; i < WaferCount; i++) // 정규식 내용이 없는 속성들은 웨이퍼 아이디 또는 웨이퍼 넘버의 갯수만큼 생성
                {
                    if (WaferIDCount > WaferNumCount)
                    {
                        if (wadt.Rows[i]["Start"].ToString() == prdt.Rows[i]["Start"].ToString())
                        {
                            prdt.Rows[i]["Start"] = 0;
                            prdt.Rows[i]["End"] = 0;
                        }
                        if (wadt.Rows[i]["Start"].ToString() == xdt.Rows[i]["Start"].ToString())
                        {
                            xdt.Rows[i]["Start"] = 0;
                            xdt.Rows[i]["End"] = 0;
                        }
                        if (wadt.Rows[i]["Start"].ToString() == ydt.Rows[i]["Start"].ToString())
                        {
                            ydt.Rows[i]["Start"] = 0;
                            ydt.Rows[i]["End"] = 0;
                        }
                        if (wadt.Rows[i]["Start"].ToString() == fldt.Rows[i]["Start"].ToString())
                        {
                            fldt.Rows[i]["Start"] = 0;
                            fldt.Rows[i]["End"] = 0;
                        }
                        if (wadt.Rows[i]["Start"].ToString() == todt.Rows[i]["Start"].ToString())
                        {
                            todt.Rows[i]["Start"] = 0;
                            todt.Rows[i]["End"] = 0;
                        }
                    }
                    else if (WaferIDCount < WaferNumCount)
                    {
                        if (wandt.Rows[i]["Start"].ToString() == prdt.Rows[i]["Start"].ToString())
                        {
                            prdt.Rows[i]["Start"] = 0;
                            prdt.Rows[i]["End"] = 0;
                        }
                        if (wandt.Rows[i]["Start"].ToString() == xdt.Rows[i]["Start"].ToString())
                        {
                            xdt.Rows[i]["Start"] = 0;
                            xdt.Rows[i]["End"] = 0;
                        }
                        if (wandt.Rows[i]["Start"].ToString() == ydt.Rows[i]["Start"].ToString())
                        {
                            ydt.Rows[i]["Start"] = 0;
                            ydt.Rows[i]["End"] = 0;
                        }
                        if (wandt.Rows[i]["Start"].ToString() == fldt.Rows[i]["Start"].ToString())
                        {
                            fldt.Rows[i]["Start"] = 0;
                            fldt.Rows[i]["End"] = 0;
                        }
                        if (wandt.Rows[i]["Start"].ToString() == todt.Rows[i]["Start"].ToString())
                        {
                            todt.Rows[i]["Start"] = 0;
                            todt.Rows[i]["End"] = 0;
                        }
                    }
                }

                DataRow[] dtsort = null;
                DataRow[] wafsort = null;
                DataRow[] flatsort = null;
                DataRow[] prosort = null;
                DataRow[] xsort = null;
                DataRow[] ysort = null;
                DataRow[] tosort = null;
                DataRow[] binsort = null;
                DataRow[] wafnsort = null;

                dtsort = dt.Select("1=1", "Start ASC");
                wafsort = wadt.Select("1=1", "Start ASC");
                flatsort = fldt.Select("1=1", "Start ASC");
                prosort = prdt.Select("1=1", "Start ASC");
                xsort = xdt.Select("1=1", "Start ASC");
                ysort = ydt.Select("1=1", "Start ASC");
                tosort = todt.Select("1=1", "Start ASC");
                binsort = bindt.Select("1=1", "Start ASC");
                wafnsort = wandt.Select("1=1", "Start ASC");

                int binTemp = BinCount / WaferCount;
                int ck1 = 0, ck2 = 0, ck3 = 0, ck4 = 0, ck5 = 0, ck6 = 0, ck7 = 0, ck8 = 0;

                for (int i = 1; i <= WaferCount; i++)
                {
                    dtsort[0 + ck4] = prosort[i - 1];

                    if (WaferIDCount > WaferNumCount)
                        dtsort[1 + ck4] = wafsort[i - 1];
                    if (WaferIDCount < WaferNumCount)
                        dtsort[1 + ck4] = wafnsort[i - 1];

                    dtsort[2 + ck4] = xsort[i - 1];
                    dtsort[3 + ck4] = ysort[i - 1];
                    ck6 = 3 + ck4;
                    for (int j = 0; j < binTemp; j++)
                    {
                        dtsort[4 + ck6 + j - 3] = binsort[j + ck1];
                    }
                    ck5 = binTemp;
                    ck1 += binTemp;
                    ck2 += 1;
                    dtsort[5 + ck5 + ck7 + ck8 - ck2] = tosort[i - 1];
                    dtsort[6 + ck5 + ck7 + ck8 - ck2] = flatsort[i - 1];
                    ck3 += 6;
                    ck7 += ck5;
                    ck4 += ck5 + 6;
                    ck8 += 7;
                }

                for (int i = 0; i < dtsort.Length; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        ndt.Rows[i][j] = dtsort[i].ItemArray[j];
                    }
                    if (ndt.Rows[i]["name"].Equals("FlatZone"))
                    {
                        if (ndt.Rows[i]["Start"].Equals("0"))
                            strCurrent = analysis.Entities["FlatZone"].DefaultValue;
                        else
                            strCurrent = strCurrentPageText.Substring(int.Parse(ndt.Rows[i][1].ToString()), int.Parse(ndt.Rows[i][2].ToString()));
                    }
                }

                sb = ParserStringBuilder(ndt, sb, binTemp, binsort, TotalCkn, strCurrentPageText, dt, strCurrent, rundt, WaferNumCount);

                if (analysis.Navigators.ContainsKey(analysis.CurrentFile + "_HANA"))
                    analysis.Navigators.Remove(analysis.CurrentFile + "_HANA");

                foreach (Form fm in this.ParentForm.MdiChildren)
                {
                    if (fm.Name == "frmHanaMap")
                    {
                        fm.Close();
                        break;
                    }
                }
                frmHanaMap frm = new frmHanaMap(sb);
                frm.Owner = this;
                frm.MdiParent = this.ParentForm;
                frm.Size = new Size(400, 800);
                frm.WindowState = FormWindowState.Normal;
                this.Size = new Size(this.Parent.Size.Width - 420, this.Parent.Size.Height);
                frm.Show();
                frm.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        void uclRegexEntity_SaveRequested(RegexEntity entity)
        {
            try
            {
                WaferIdRegex = analysis.Entities["Wafer_ID"].RegexString;
                WaferNumRegex = analysis.Entities["Wafer_Num"].RegexString;
                RunIdRegex = analysis.Entities["Run_ID"].RegexString;

                if (!WaferIdRegex.Equals("") && !WaferNumRegex.Equals(""))
                {
                    MessageBox.Show("Input one of the Wafer_ID or Wafer_Num value");
                    analysis.Entities[entity.ToString()].RegexString = "";
                }
                else
                {
                    if (!analysis.Entities.Exists(entity.Name))
                        analysis.Entities.Add(entity);
                    else
                    {
                        for (int i = 0; i < lvEntity.Items.Count; i++)
                        {
                            if (lvEntity.Items[i].Text == entity.Name && string.IsNullOrEmpty(entity.RegexString))
                                lvEntity.Items[i].SubItems[1].Text = entity.DefaultValue;
                        }
                        analysis.CurrentFileNavigator.RefreshSelection(entity);
                    }
                    lvEntity.Items[entity.Name].Selected = true;
                    SelectText(entity);
                }

                if (!WaferNumRegex.Equals("") && RunIdRegex.Equals(""))
                    MessageBox.Show("Please input RUN ID." + '\n' + "( Run_ID + Wafer_Num = Wafer_ID )");
            }
            catch (Exception ex)
            {
                DACrux.SP.Common.Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        void uclRegexEntity_RemoveRequested(RegexEntity entity)
        {
            try
            {
                analysis.Entities.Remove(entity);
            }
            catch (Exception ex)
            {
                DACrux.SP.Common.Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        void EntitiesItemAdded(RegexEntity entity)
        {
            try
            {
                lvEntity.Items.Add(GetEntityListViewItem(entity));
                entity.EntityRenamed += new ItemRenamedEventHandler<RegexEntity>(EntityRenamed);
            }
            catch (Exception ex)
            {
                DACrux.SP.Common.Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        void EntitiesItemRemoved(RegexEntity entity)
        {
            try
            {
                lvEntity.Items.RemoveByKey(entity.Name);
                entity.EntityRenamed -= new ItemRenamedEventHandler<RegexEntity>(EntityRenamed);

                SelectText(string.Empty, RegexOptions.None);
            }
            catch (Exception ex)
            {
                DACrux.SP.Common.Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region [ Explorer ]

        void EntityRenamed(Entity item, string prevName)
        {
            ListViewItem lvItem = lvEntity.Items[prevName];

            try
            {
                if (lvItem != null)
                    lvItem.Name = lvItem.Text = item.Name;
            }
            catch (Exception ex)
            {
                DACrux.SP.Common.Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        void lvMatch_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            try
            {
                if (e.IsSelected)
                {
                    DACrux.SP.Common.Token cp = e.Item.Tag as DACrux.SP.Common.Token;
                    SelectText(cp.Index, cp.Length);
                }
            }
            catch (Exception ex)
            {
                DACrux.SP.Common.Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region [ Title Moving ]

        private void uclTitle_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            iPos = e.Y;
            isDragging = true;
        }

        private void uclTitleMatch_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDragging)
                return;

            if (pnlTreeView.Size.Height < 1)
            {
                if ((e.Y - iPos) < 0) return;
            }

            if (pnlMatch.Size.Height < 1)
            {
                if ((e.Y - iPos) > 0) return;
            }

            pnlTreeView.Size = new Size(pnlTreeView.Width, pnlTreeView.Height + (e.Y - iPos));
        }

        private void uclTitleEntity_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDragging)
                return;

            if (uclRegexEntity.Size.Height < 1)
            {
                if ((e.Y - iPos) < 0) return;
            }

            if (lvEntity.Size.Height < 1)
            {
                if ((e.Y - iPos) > 0) return;
            }

            uclRegexEntity.Size = new Size(uclRegexEntity.Width, uclRegexEntity.Height + (e.Y - iPos));
        }

        private void uclTitle_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            isDragging = false;
        }

        #endregion

        #endregion

        #region " Method "

        #region [ SelectText ]

        private void SelectText(RegexEntity entity)
        {

            SelectText(analysis.Navigators[tabTextViewer.SelectedTab.Text].GetPositionInfo(entity));
        }

        private void SelectText(RegexSection section)
        {
            SelectText(analysis.Navigators[tabTextViewer.SelectedTab.Text].GetPositionInfo(section));
        }

        private void SelectText(List<int[]> lst)
        {
            SelectText((tabTextViewer.SelectedTab.Controls[0] as SmartTextViewer), lst);
        }

        private void SelectText(ISectionItem item, int matchIndex)
        {
            if (item == null)
                return;

            DACrux.SP.Common.Token cp = analysis.GetCapture(item, matchIndex);
            List<int[]> lst = new List<int[]>();
            lst.Add(new int[] { cp.Index, cp.Length });

            SelectText((tabTextViewer.SelectedTab.Controls[0] as SmartTextViewer), lst);
        }

        private void SelectText(SmartTextViewer textViewer, List<int[]> lst)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                textViewer.SelectText(lst);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void SelectText(int start, int length)
        {
            (tabTextViewer.SelectedTab.Controls[0] as SmartTextViewer).SelectText(start, length);
        }

        private void SelectText(string strRegex, RegexOptions option)
        {
            try
            {
                if (string.IsNullOrEmpty(strRegex))
                {
                    SelectText((tabTextViewer.SelectedTab.Controls[0] as SmartTextViewer), null);
                    return;
                }

                string strContent = (tabTextViewer.SelectedTab.Controls[0] as SmartTextViewer).TotalContent;
                List<DACrux.SP.Common.Token> captures = RegexNavigator.GetCaptures(ref strContent, strRegex, string.Empty, option).ToList();

                var lst = from DACrux.SP.Common.Token capture in captures
                          select new int[] { capture.Index, capture.Length };

                SelectText(lst.ToList());

                #region " Old Version "
                //foreach (Match m in ms)
                //{
                //    textBox.Select(m.Index, m.Length);
                //    textBox.SelectionBackColor = Color.LightPink;
                //    //sb.AppendLine(m.ToString());
                //    //string[] names = expr.GetGroupNames();
                //    //foreach (string group in names)
                //    //{
                //    //    int val;
                //    //    if (int.TryParse(group, out val)) continue;
                //    //    sb.Append("<" + group + ">=");
                //    //    sb.Append(m.Groups[group].Value);
                //    //    sb.Append("\r\n");
                //    //}
                //}
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        private ListViewItem GetEntityListViewItem(Entity entity)
        {
            ListViewItem item = new ListViewItem();
            item.Text = item.Name = entity.Name;

            if (string.IsNullOrEmpty(analysis.Entities[entity.Name].RegexString))
                item.SubItems.Add(entity.DefaultValue);
            else
                item.SubItems.Add(string.Empty);
            item.SubItems.Add(entity.Force ? "Y" : "N");
            item.ImageIndex = 0;
            item.Tag = entity;

            return item;
        }

        private ListViewItem GetSectionListViewItem(Section section)
        {
            ListViewItem item = new ListViewItem();
            item.Text = item.Name = section.Name;
            item.SubItems.Add(section.Force ? "Y" : "N");
            item.ImageIndex = 1;
            item.Tag = section;

            return item;
        }

        private void RefreshTreeView(Analysis analysis)
        {
            tvExplorer.Nodes.Clear();

            TreeNode node = null;

            foreach (Section section in analysis.Sections)
            {
                node = new TreeNode(section.Name, 1, 1);
                node.Tag = section;

                tvExplorer.Nodes.Add(node);

                AddChildTreeNode(node, section.Items);
            }

            foreach (Entity entity in analysis.Entities)
            {
                if (entity.Parent != null)
                    continue;

                node = new TreeNode(entity.Name, 0, 0);
                node.Tag = entity;

                tvExplorer.Nodes.Add(node);
            }

            tvExplorer.NodeMouseClick += new TreeNodeMouseClickEventHandler(tvExplorer_NodeMouseClick);
        }

        private void AddChildTreeNode(TreeNode parent, ISectionItemCollection items)
        {
            int imageIndex = 0;
            foreach (ISectionItem item in items)
            {
                imageIndex = (item is Entity) ? 0 : 1;

                TreeNode node = parent.Nodes.Add(item.Name, item.Name, imageIndex, imageIndex);
                node.Tag = item;

                if (item is Section)
                    AddChildTreeNode(node, (item as Section).Items);
            }
        }

        private void tvExplorer_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                SetExplorerMatchListViewItems(e.Node.Tag as ISectionItem);
            }
            catch (Exception ex)
            {
                DACrux.SP.Common.Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void SetExplorerMatchListViewItems(ISectionItem selectedItem)
        {
            try
            {
                lvMatch.Items.Clear();

                List<DACrux.SP.Common.Token> captures = analysis.GetCaptures(selectedItem);

                string strTargetText = analysis.CurrentFileNavigator.TargetText;

                if (string.IsNullOrEmpty(strTargetText))
                    return;


                ListViewItem item;
                foreach (DACrux.SP.Common.Token cp in captures)
                {
                    item = new ListViewItem();
                    item.Text = item.Name = captures.IndexOf(cp).ToString();
                    item.SubItems.Add(strTargetText.Substring(cp.Index, cp.Length));
                    item.Tag = cp;

                    lvMatch.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadEntityList(Analysis analysis)
        {
            lvEntity.Items.Clear();

            foreach (RegexEntity entity in analysis.Entities)
            {
                lvEntity.Items.Add(GetEntityListViewItem(entity));
                entity.EntityRenamed += new ItemRenamedEventHandler<RegexEntity>(EntityRenamed);

                if (!(entity is RegexEntity))
                    continue;

                foreach (KeyValuePair<string, RegexNavigator> navi in analysis.Navigators)
                    navi.Value.SetEntity(entity as RegexEntity);
            }

            foreach (ColumnHeader col in lvEntity.Columns)
                col.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
        }
        private DataSet SortDataTable(DataTable dts, Dictionary<RegexEntity, List<Token>> dictemp, string[] strListNum, string[] strListId, int WaferNumCk)
        {
            DataTable ndt = null;
            DataSet ds = null;
            try
            {
                ndt = new DataTable();
                string key = null;
                List<Token> val = new List<Token>();
                DataRow ndr = null;
                DataRow dr = null;
                DataRow rundr = null;
                DataTable dt = new DataTable();
                DataTable rundt = new DataTable();
                ds = new DataSet();
                ndt.Columns.Add("name");
                ndt.Columns.Add("Start");
                ndt.Columns.Add("End");
                dt.Columns.Add("name");
                dt.Columns.Add("Start");
                dt.Columns.Add("End");
                rundt.Columns.Add("name");
                rundt.Columns.Add("Start", typeof(int));
                rundt.Columns.Add("End", typeof(int));
                foreach (KeyValuePair<RegexEntity, List<Token>> k in dictemp) // 이 부분 if문 설정
                {
                    if (k.Key.Name.Equals("Run_ID"))
                    {
                        key = k.Key.Name;
                        val = k.Value.ToList<Token>();
                        for (int valcount = 0; valcount < val.Count; valcount++)
                        {
                            rundr = rundt.NewRow();
                            rundr["name"] = key;
                            rundr["Start"] = val[valcount].Index.ToString();
                            rundr["End"] = val[valcount].Length.ToString();

                            rundt.Rows.Add(rundr);
                        }
                    }

                    if (WaferNumCk >= 1) // 웨이퍼 아이디가 없을 때
                    {
                        for (int i = 0; i < strListNum.Length; i++)
                        {
                            if (k.Key.Name == strListNum[i]) // 구성요소만 생성
                            {
                                key = k.Key.Name;
                                val = k.Value.ToList<Token>();
                                for (int nvalcount = 0; nvalcount < val.Count; nvalcount++)
                                {
                                    ndr = ndt.NewRow();
                                    ndr["name"] = string.Empty;
                                    ndr["Start"] = 0;
                                    ndr["End"] = 0;

                                    if (val[nvalcount].Length != 0)
                                        ndt.Rows.Add(ndr);
                                }
                                if (val[0].Length == 0)
                                {
                                    ndr = ndt.NewRow();
                                    ndr["name"] = string.Empty;
                                    ndr["Start"] = 0;
                                    ndr["End"] = 0;
                                    ndt.Rows.Add(ndr);
                                }

                                for (int valcount = 0; valcount < val.Count; valcount++)
                                {
                                    dr = dt.NewRow();
                                    dr["name"] = key;
                                    dr["Start"] = val[valcount].Index.ToString();
                                    dr["End"] = val[valcount].Length.ToString();

                                    if (val[valcount].Length != 0)
                                        dt.Rows.Add(dr);
                                }
                            }
                        }
                    }
                    else if (WaferNumCk <= 0) //웨이퍼 아이디가 있을 때
                    {
                        for (int i = 0; i < strListId.Length; i++)
                        {
                            if (k.Key.Name == strListId[i]) // 구성요소만 생성
                            {
                                key = k.Key.Name;
                                val = k.Value.ToList<Token>();
                                for (int nvalcount = 0; nvalcount < val.Count; nvalcount++)
                                {
                                    ndr = ndt.NewRow();
                                    ndr["name"] = string.Empty;
                                    ndr["Start"] = 0;
                                    ndr["End"] = 0;

                                    if (val[nvalcount].Length != 0)
                                        ndt.Rows.Add(ndr);
                                }
                                if (val[0].Length == 0)
                                {
                                    ndr = ndt.NewRow();
                                    ndr["name"] = string.Empty;
                                    ndr["Start"] = 0;
                                    ndr["End"] = 0;
                                    ndt.Rows.Add(ndr);
                                }

                                for (int valcount = 0; valcount < val.Count; valcount++)
                                {
                                    dr = dt.NewRow();
                                    dr["name"] = key;
                                    dr["Start"] = val[valcount].Index.ToString();
                                    dr["End"] = val[valcount].Length.ToString();

                                    if (val[valcount].Length != 0)
                                        dt.Rows.Add(dr);
                                }
                            }
                        }
                    }
                }
                ds.Tables.Add(ndt);
                ds.Tables.Add(dt); // 정규식으로 포함된 내용만 이동
                ds.Tables.Add(rundt);
                return ds;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private StringBuilder ParserStringBuilder(DataTable ndt, StringBuilder sb, int binTemp, DataRow[] binsort, int TotalCkn, string strCurrentPageText, DataTable bdt, string strCurrent, DataTable rundt, int WaferNumCount)
        {
            StringBuilder sbRePoint = null;
            string strCurrentVal = null;
            try
            {
                strCurrentVal = strCurrent;
                sbRePoint = new StringBuilder();
                string ProTemp = null;
                string WaferTemp = null;
                string Xcoordinate = null;
                string Ycoordinate = null;
                string binTemper = null;
                string TotalTemp = null;
                string Wafer_Ck = null;
                string Wafer_Rmv = null;
                string FlatTemp = null;
                string Xtemp = null;
                string Ytemp = null;
                string Run_ID = null;
                string WaferNumTemp = null;
                int MapCount = 0;
                int RunidCnt = 0;

                Dictionary<string, string> test = new Dictionary<string, string>();
                int strWaferCount = 0;

                for (int f = 0; f < ndt.Rows.Count; f++)
                {
                    string tempsNm = ndt.Rows[f]["name"].ToString();

                    ProTemp = analysis.Entities["Product"].DefaultValue;
                    WaferTemp = analysis.Entities["Wafer_ID"].DefaultValue;
                    Xcoordinate = binsort[0].ItemArray[2].ToString();
                    Ycoordinate = binTemp.ToString();
                    Xtemp = analysis.Entities["X"].DefaultValue;
                    Ytemp = analysis.Entities["Y"].DefaultValue;
                    binTemper = analysis.Entities["bin"].DefaultValue;
                    TotalTemp = analysis.Entities["Total"].DefaultValue;
                    FlatTemp = analysis.Entities["FlatZone"].DefaultValue;
                    WaferNumTemp = analysis.Entities["Wafer_Num"].DefaultValue;
                    analysis.strBottom = strBottom;
                    analysis.strLeft = strLeft;
                    analysis.strRight = strRight;
                    analysis.strTop = strTop;
                    switch (tempsNm)
                    {
                        case "Product":
                            if (ndt.Rows[f]["Start"].Equals("0"))
                                sb.Append(ndt.Rows[f][0] = "DEVICE:" + ProTemp);
                            else
                                sb.Append(ndt.Rows[f][0] = "DEVICE:");
                            MapCount++;
                            break;
                        case "Wafer_ID":
                            TotalCkn = f; //TOTAL 기준값 카운트
                            if (ndt.Rows[f]["Start"].Equals("0"))
                                sb.Append(ndt.Rows[f][0] = "WAFERID:" + WaferTemp);
                            else
                                sb.Append(ndt.Rows[f][0] = "WAFERID:");
                            //strWafer = strCurrentPageText.Substring(int.Parse(ndt.Rows[f][1].ToString()), int.Parse(ndt.Rows[f][2].ToString())); //기본키를 웨이퍼 아이디로 주기위한 구문
                            strWaferCount += 1; //기본키를 숫자로 생성하기 위한 구문
                            MapCount++;
                            break;

                        case "Wafer_Num":
                            TotalCkn = f; //TOTAL 기준값 카운트
                            Run_ID = (strCurrentPageText.Substring(int.Parse(rundt.Rows[RunidCnt][1].ToString()), int.Parse(rundt.Rows[RunidCnt][2].ToString())));
                            if (ndt.Rows[f]["Start"].Equals("0"))
                                sb.Append(ndt.Rows[f][0] = "WAFERID:" + Run_ID + "-");
                            else
                                sb.Append(ndt.Rows[f][0] = "WAFERID:" + Run_ID + "-");
                            strWaferCount += 1; //기본키를 숫자로 생성하기 위한 구문
                            RunidCnt++;
                            MapCount++;
                            break;

                        case "X":
                            if (ndt.Rows[f]["Start"].Equals("0"))
                            {
                                if (lvEntity.Items["X"].SubItems[1].Text == "")
                                    sb.Append(ndt.Rows[f][0] = "X:" + Xcoordinate);
                                else
                                    sb.Append(ndt.Rows[f][0] = "X:" + Xtemp);
                            }
                            else
                                sb.Append(ndt.Rows[f][0] = "X:");
                            MapCount++;
                            break;
                        case "Y":
                            if (ndt.Rows[f]["Start"].Equals("0"))
                            {
                                if (lvEntity.Items["Y"].SubItems[1].Text == "")
                                    sb.Append(ndt.Rows[f][0] = "Y:" + Ycoordinate);
                                else
                                    sb.Append(ndt.Rows[f][0] = "Y:" + Ytemp);
                            }
                            else
                                sb.Append(ndt.Rows[f][0] = "Y:");
                            MapCount++;
                            break;
                        case "bin":
                            if (ndt.Rows[f - 1]["name"] != ndt.Rows[f]["name"])
                            {
                                if (ndt.Rows[f]["Start"].Equals("0"))
                                    sb.Append(ndt.Rows[0][0] = "REFDIE:" + "\n" + binTemper);
                                else
                                    sb.Append(ndt.Rows[0][0] = "REFDIE:" + "\n");
                            }
                            MapCount++;
                            break;
                        case "Total":

                            if (WaferNumCount > 0)
                            {
                                Wafer_Ck = (strCurrentPageText.Substring(int.Parse(ndt.Rows[TotalCkn][1].ToString()), int.Parse(ndt.Rows[TotalCkn][2].ToString())));
                                Run_ID = (strCurrentPageText.Substring(int.Parse(rundt.Rows[RunidCnt - 1][1].ToString()), int.Parse(rundt.Rows[RunidCnt - 1][2].ToString())));
                                if (Wafer_Ck.Substring(0, 1).Equals("#"))
                                {
                                    Wafer_Rmv = Wafer_Ck.Replace("\n", null).Remove(0, 1);
                                    if (ndt.Rows[f]["Start"].Equals("0"))
                                        sb.Append(ndt.Rows[f][0] = "#" + Run_ID + "-" + Wafer_Rmv + ":    " + TotalTemp);
                                    else
                                        sb.Append(ndt.Rows[f][0] = "#" + Run_ID + "-" + Wafer_Rmv + ":    ");
                                }
                                else
                                {
                                    if (ndt.Rows[f]["Start"].Equals("0"))
                                        sb.Append(ndt.Rows[f][0] = "#" + Run_ID + "-" + Wafer_Ck.Replace("\n", null) + ":    " + TotalTemp);
                                    else
                                        sb.Append(ndt.Rows[f][0] = "#" + Run_ID + "-" + Wafer_Ck.Replace("\n", null) + ":    ");
                                }
                                MapCount++;
                            }
                            else if (WaferNumCount <= 0)
                            {
                                Wafer_Ck = (strCurrentPageText.Substring(int.Parse(ndt.Rows[TotalCkn][1].ToString()), int.Parse(ndt.Rows[TotalCkn][2].ToString())));
                                //Run_ID = (strCurrentPageText.Substring(int.Parse(rundt.Rows[RunidCnt - 1][1].ToString()), int.Parse(rundt.Rows[RunidCnt - 1][2].ToString())));
                                if (Wafer_Ck.Substring(0, 1).Equals("#"))
                                {
                                    Wafer_Rmv = Wafer_Ck.Replace("\n", null).Remove(0, 1);
                                    if (ndt.Rows[f]["Start"].Equals("0"))
                                        sb.Append(ndt.Rows[f][0] = "#" + Wafer_Rmv + ":    " + TotalTemp);
                                    else
                                        sb.Append(ndt.Rows[f][0] = "#" + Wafer_Rmv + ":    ");
                                }
                                else
                                {
                                    if (ndt.Rows[f]["Start"].Equals("0"))
                                        sb.Append(ndt.Rows[f][0] = "#" + Wafer_Ck.Replace("\n", null) + ":    " + TotalTemp);
                                    else
                                        sb.Append(ndt.Rows[f][0] = "#" + Wafer_Ck.Replace("\n", null) + ":    ");
                                }
                                MapCount++;
                            }
                            break;


                        case "FlatZone":
                            if (ndt.Rows[f]["Start"].Equals("0"))
                                sb.Append(ndt.Rows[f][0] = "FLAT ZONE : " + FlatTemp);
                            else
                                sb.Append(ndt.Rows[f][0] = "FLAT ZONE : ");
                            MapCount++;
                            break;
                    }

                    if (ndt.Rows[f]["name"] as string == "bin")
                    {
                        int tempf = f;
                        string[] GoodBinArr = null;
                        string[] GoodBinChar = null;
                        string[] NullBinArr = null;
                        string[] NullBinChar = null;
                        string[] FailBinArr = null;
                        string[] FailBinChar = null;

                        string strCurrentText = null;
                        string GoodBinFir = string.Empty;
                        string GoodBinSec = string.Empty;
                        string NullBinFir = string.Empty;
                        string NullBinSec = string.Empty;
                        string FailBinFir = string.Empty;
                        string FailBinSec = string.Empty;

                        GoodBinArr = GoodBin.Split(',');
                        NullBinArr = NullBin.Split(',');
                        FailBinArr = FailBin.Split(',');

                        for (int i = tempf; i < ndt.Rows.Count; i++)
                        {
                            strCurrentText = strCurrentPageText.Substring(int.Parse(ndt.Rows[i][1].ToString()), int.Parse(ndt.Rows[i][2].ToString()));

                            if (!ndt.Rows[i]["name"].Equals("bin"))
                                break;

                            if (GoodBinArr.Length > 1)
                            {
                                for (int Cnt = 0; Cnt < GoodBinArr.Length; Cnt++)
                                {

                                    GoodBinChar = GoodBinArr[Cnt].Split('=');
                                    GoodBinFir = GoodBinChar[0];
                                    GoodBinSec = GoodBinChar[1];

                                    strCurrentText = strCurrentText.Replace(GoodBinFir, GoodBinSec);
                                }
                            }

                            if (NullBinArr.Length > 1)
                            {
                                for (int Cnt = 0; Cnt < NullBinArr.Length; Cnt++)
                                {

                                    NullBinChar = NullBinArr[Cnt].Split('=');
                                    NullBinFir = NullBinChar[0];
                                    NullBinSec = NullBinChar[1];

                                    strCurrentText = strCurrentText.Replace(NullBinFir, NullBinSec);
                                }
                            }
                            if (FailBinArr.Length > 1)
                            {
                                for (int Cnt = 0; Cnt < FailBinArr.Length; Cnt++)
                                {

                                    FailBinChar = FailBinArr[Cnt].Split('=');
                                    FailBinFir = FailBinChar[0];
                                    FailBinSec = FailBinChar[1];

                                    strCurrentText = strCurrentText.Replace(FailBinFir, FailBinSec);
                                }
                            }
                            //sb.Append(strCurrentPageText.Substring(int.Parse(ndt.Rows[i][1].ToString()), int.Parse(ndt.Rows[i][2].ToString())).Replace(NullBin, ".").Replace(GoodBin, "1"));
                            sb.Append(strCurrentText);
                            sb.Append('\n').Replace("\n\n", "\n");

                            sbRePoint.Append(strCurrentPageText.Substring(int.Parse(ndt.Rows[i][1].ToString()), int.Parse(ndt.Rows[i][2].ToString())));
                            sbRePoint.Append('\n').Replace("\n\n", "\n");
                            f++;
                        }
                        f--;

                    }
                    else if (ndt.Rows[f]["name"] as string == "FLAT ZONE : ")
                    {
                        sb.Append(strCurrentPageText.ToUpper().Substring(int.Parse(ndt.Rows[f][1].ToString()), int.Parse(ndt.Rows[f][2].ToString())).Replace(strTop.ToUpper(), "TOP").Replace(strBottom.ToUpper(), "BOTTOM").Replace(strLeft.ToUpper(), "LEFT").Replace(strRight.ToUpper(), "RIGHT"));
                    }
                    else
                    {
                        sb.Append(strCurrentPageText.Substring(int.Parse(ndt.Rows[f][1].ToString()), int.Parse(ndt.Rows[f][2].ToString())));
                    }

                    sb.Append('\n').Replace("\n\n", "\n");
                    sbRePoint.Append('\n').Replace("\n\n", "\n");

                    if (MapCount >= 7)
                    {
                        test.Add(strWaferCount.ToString(), sbRePoint.ToString());
                        sbRePoint = new StringBuilder();
                        MapCount = 0;
                    }
                }
                ReferencePoint(strCurrentPageText, ndt, strCurrentVal, Xcoordinate, Ycoordinate, binsort, test);
                return sb;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ReferencePoint(string strCurrentPageText, DataTable ndt, string strCurrentVal, string Xcoordinate, string Ycoordinate, DataRow[] binsort, Dictionary<string, string> test)
        { // 삭제할 내용 포함 ndt, Xcoordinate, Ycoordinate, binsort
            int firChk = 0;
            int ReferX = 0;
            int ReferY = 0;
            //int STP_X = 0;
            //int STP_Y = 0;
            int dataCount = 0;
            int DataRowp = 0;
            string[] DRArr = null;
            string[] newDRArr = null;
            string[] ArrTempXYs = null;
            string DRArrString = null;
            string DRArrBU = null;
            string DRArrStr = null;
            string ReferXY = null;
            string tempStr = null;

            try
            {
                //if (analysis.Entities["STP_X"].DefaultValue.Equals(string.Empty) || analysis.Entities["STP_Y"].DefaultValue.Equals(string.Empty))
                //{
                //    STP_X = 0;
                //    STP_Y = 0;
                //}
                //else
                //{
                //    STP_X = int.Parse(analysis.Entities["STP_X"].DefaultValue);
                //    STP_Y = int.Parse(analysis.Entities["STP_Y"].DefaultValue);
                //}
                ArrTempXYs = new string[test.Count];

                for (int DataRowj = 0; DataRowj < test.Count; DataRowj++)
                {
                    DRArrBU = test[(DataRowj + 1).ToString()];

                    newDRArr = DRArrBU.Split('\n');
                    for (DataRowp = 0; DataRowp < newDRArr.Length; DataRowp++)
                    {
                        if (!newDRArr[DataRowp].Length.Equals(0))
                        {
                            dataCount++;
                            DRArrString = newDRArr[DataRowp];
                            if (dataCount.Equals(1))
                            {
                                tempStr += DRArrString;
                            }
                            else if (dataCount > 1)
                            {
                                tempStr += '\n' + DRArrString;
                            }
                        }
                    }
                    DRArr = tempStr.Split('\n');

                    //---------------------  Bottom 일 경우 START FlatZone : 0도  ----------------------
                    if (strCurrentVal.ToUpper().Equals("BOTTOM") || strCurrentVal.ToUpper().Equals("DOWN") || strCurrentVal.ToUpper().Equals("0"))
                    {
                        for (int DRArri = 0; DRArri < DRArr.Length; DRArri++)
                        {
                            for (int DRArrj = 0; DRArrj < DRArr[DRArri].Length; DRArrj++)
                            {
                                DRArrStr = DRArr[DRArri].Substring(DRArrj, 1);

                                if (firChk < 1)
                                {
                                    if (!DRArrStr.Equals(".") && !DRArrStr.Equals(" "))
                                    {
                                        ReferX = DRArrj;
                                        ReferY = DRArri;
                                        firChk++;
                                    }
                                }
                            }
                        }
                    }

                    //---------------------  Left 일 경우 START FlatZone : 90도  ----------------------
                    if (strCurrentVal.ToUpper().Equals("LEFT") || strCurrentVal.ToUpper().Equals("90"))
                    {
                        for (int DRArri = DRArr.Length - 1; DRArri >= 0; DRArri--)
                        {
                            DRArrStr = DRArr[DRArri].Substring(0, 1);
                            if (firChk < 1)
                            {
                                if (!DRArrStr.Equals(".") && !DRArrStr.Equals(" "))
                                {
                                    ReferX = 0;
                                    ReferY = DRArri;
                                    firChk++;
                                }
                            }

                        }
                    }
                    //---------------------  UP 일 경우 START FlatZone : 180도  ----------------------
                    if (strCurrentVal.ToUpper().Equals("UP") || strCurrentVal.ToUpper().Equals("180") || strCurrentVal.ToUpper().Equals("TOP"))
                    {
                        for (int DRArri = DRArr.Length - 1; DRArri >= 0; DRArri--)
                        {
                            for (int DRArrj = int.Parse(DRArr[DRArri].Length.ToString()) - 1; DRArrj >= 0; DRArrj--)
                            {
                                DRArrStr = DRArr[DRArri].Substring(DRArrj, 1);

                                if (firChk < 1)
                                {
                                    if (!DRArrStr.Equals(".") && !DRArrStr.Equals(" "))
                                    {
                                        ReferX = DRArrj;
                                        ReferY = DRArri;
                                        firChk++;
                                    }
                                }
                            }
                        }
                    }

                    //---------------------  RIGHT 일 경우 START FlatZone : 270도  ----------------------
                    if (strCurrentVal.ToUpper().Equals("RIGHT") || strCurrentVal.ToUpper().Equals("270"))
                    {
                        for (int DRArri = 0; DRArri < DRArr.Length; DRArri++)
                        {
                            for (int DRArrj = int.Parse(DRArr[DRArri].Length.ToString()) - 1; DRArrj >= int.Parse(DRArr[DRArri].Length.ToString()) - 1; DRArrj--)
                            {
                                DRArrStr = DRArr[DRArri].Substring(DRArrj, 1);
                                if (firChk < 1)
                                {
                                    if (!DRArrStr.Equals(".") && !DRArrStr.Equals(" "))
                                    {
                                        ReferX = DRArrj;
                                        ReferY = DRArri;
                                        firChk++;
                                    }
                                }
                            }
                        }
                    }
                    ReferXY = ReferX + " " + ReferY;
                    ArrTempXYs[DataRowj] = ReferXY; //레퍼런스 포인트 저장 배열
                    firChk = 0;
                    DataRowp = 0;
                    dataCount = 0;
                    DRArr = null;
                    tempStr = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BinCountCheck(int length)
        {
            try
            {
                BinCheckList.Items.Clear();
                var lst = from DACrux.SP.Common.Token mt in analysis.Navigators[strFilename].EntityCaptureCollection[entitys]
                          select new int[] { mt.Index, mt.Length };

                String strTempText = null;
                List<int[]> list = new List<int[]>();
                list = lst.ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    int[] test = list[i];
                    strTempText = analysis.SampleFiles[strFilename].ToString().Substring(test[0], test[1]);
                    if (strTempText.Length % length > 0)
                    {
                        MessageBox.Show(string.Format("This map is not divided by {0}.", length.ToString()), "Infomation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    analysis.CharCount = int.Parse(textBox1.Text);
                    for (int strCount = 0; strCount < strTempText.Length; strCount += length)
                    {
                        if (strCount + length < strTempText.Length)
                        {
                            if (BinCheckList.Items.Count <= 0)
                            {
                                if (!(strTempText.Substring(strCount, length) == "\n"))
                                    BinCheckList.Items.Add(strTempText.Substring(strCount, length));
                            }
                            else
                            {
                                bool checkadd = true;
                                for (int j = 0; j < BinCheckList.Items.Count; j++)
                                {
                                    if (BinCheckList.Items[j].ToString().Equals(strTempText.Substring(strCount, length)))
                                    {
                                        checkadd = false;
                                        break;
                                    }
                                }
                                if (checkadd)
                                    BinCheckList.Items.Add(strTempText.Substring(strCount, length));

                            }
                        }
                    }

                }
                fm_changecount(length);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ConvertAnalysis()
        {
            try
            {
                if (!string.IsNullOrEmpty(strFilename) && analysisConvert != null && entitys != null)
                {
                    BinCountCheck(1);
                    textBox1.TextChanged += new EventHandler(textBox1_TextChanged);
                }
                else
                {
                    MessageBox.Show("Bin 구성요소의 정규식이나 기본값을 입력하시오.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tabSetup.SelectedTab = tabPageEntity;
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region " TreeView "

        private void DrawTreeViewNode(TreeView tvExplorer, ITreeViewDrawable drawable)
        {
            TreeNode tnParent = tvExplorer.Nodes.Find(drawable.Parent, true)[0];
            int iParentNodeIndex = tnParent.Index;

            DrawTreeViewNode(tvExplorer, iParentNodeIndex, drawable);
        }

        private void DrawTreeViewNode(TreeView tvExplorer, int iParentNodeIndex, ITreeViewDrawable drawable)
        {
            TreeNode tnNode;
            TreeNode tnParent;

            try
            {
                tvExplorer.BeginUpdate();

                tnNode = new TreeNode(drawable.Name);
                tnNode.Name = drawable.Name;
                tnNode.Tag = drawable;

                switch (drawable.DrawableType)
                {
                    case TreeviewItemType.Entity:
                        tnNode.SelectedImageIndex = 0;
                        tnNode.ImageIndex = 1;
                        break;
                    case TreeviewItemType.Section:
                        tnNode.SelectedImageIndex = 2;
                        tnNode.ImageIndex = 3;
                        break;
                    default:
                        break;
                }

                if (iParentNodeIndex < 0)
                {
                    iParentNodeIndex = tvExplorer.Nodes.Add(tnNode);
                    tnParent = tnNode;
                }
                else
                {
                    tnParent = tvExplorer.Nodes[iParentNodeIndex];
                    tnParent.Nodes.Add(tnNode);
                }

                for (int i = 0; i < drawable.ItemCount; i++)
                {
                    DrawTreeViewNode(tvExplorer, iParentNodeIndex, drawable.GetItemAt(i));
                }

                tnParent.Expand();
                tvExplorer.SelectedNode = tnNode;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message == "MIRACOM")
                {
                    MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    throw (new Exception(ex.Message + " \r\n\t: ProjectManager.DrawTreeViewNode(int iParentNodeIndex, ITreeViewDrawable drawable)"));
                }
            }
            finally
            {
                tvExplorer.EndUpdate();
            }
        }

        #endregion

        private void btnCount_Click(object sender, EventArgs e)
        {
            if (BinCheckList.CheckedItems.Count <= 0)
                return;

            try
            {

                string[] strCountValue = new string[BinCheckList.CheckedItems.Count];
                for (int listcount = 0; listcount < BinCheckList.CheckedItems.Count; listcount++)
                {
                    strCountValue[listcount] = BinCheckList.CheckedItems[listcount].ToString();
                }
                var lst = from DACrux.SP.Common.Token mt in analysis.Navigators[strFilename].EntityCaptureCollection[entitys]
                          select new int[] { mt.Index, mt.Length };

                String strTempText = null;
                List<int[]> list = new List<int[]>();
                list = lst.ToList();
                DataTable dt = new DataTable();
                dt.Columns.Add("Bin");
                dt.Columns.Add("Count", typeof(int));
                int totalcount = 0;
                for (int CountValue = 0; CountValue < strCountValue.Length; CountValue++)
                {
                    DataRow dr = dt.NewRow();
                    int ValueLength = strCountValue[CountValue].Length;
                    int BinCount = 0;
                    for (int i = 0; i < list.Count; i++)
                    {
                        int[] test = list[i];
                        strTempText = analysis.SampleFiles[strFilename].ToString().Substring(test[0], test[1]);
                        for (int strCount = 0; strCount < strTempText.Length; strCount += ValueLength)
                        {
                            if (strTempText.Substring(strCount, ValueLength) == strCountValue[CountValue])
                                BinCount += 1;
                        }

                    }
                    dr["Bin"] = strCountValue[CountValue].ToString();
                    dr["Count"] = BinCount;
                    totalcount += BinCount;
                    dt.Rows.Add(dr);

                }
                DataRow drs = dt.NewRow();
                drs["Bin"] = "Sum";
                drs["Count"] = totalcount;
                dt.Rows.Add(drs);
                BinCountView.DataSource = dt;
                //MessageBox.Show(string.Format("BinCount {0}", BinCount.ToString()));
            }
            catch (Exception ex)
            {

                DACrux.SP.Common.Utility.ShowMessageBox(ex.ToString(), MessageBoxIcon.Error);
            }
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            try
            {
                string goodbin = textBox2.Text;
                string nullbin = textBox3.Text;
                string failbin = textBox4.Text;
                if (!string.IsNullOrEmpty(goodbin))
                    frmRegistration.GoodBin = goodbin;
                if (!string.IsNullOrEmpty(nullbin))
                    frmRegistration.NullBin = nullbin;
                if (!string.IsNullOrEmpty(failbin))
                    frmRegistration.FailBin = failbin;
                if (!string.IsNullOrEmpty(txtTop.Text))
                    frmRegistration.strTop = txtTop.Text;
                if (!string.IsNullOrEmpty(txtBottom.Text))
                    frmRegistration.strBottom = txtBottom.Text;
                if (!string.IsNullOrEmpty(txtLeft.Text))
                    frmRegistration.strLeft = txtLeft.Text;
                if (!string.IsNullOrEmpty(txtRight.Text))
                    frmRegistration.strRight = txtRight.Text;
                this.Close();
            }
            catch (Exception ex)
            {

                DACrux.SP.Common.Utility.ShowMessageBox(ex.ToString(), MessageBoxIcon.Error);
            }
        }

        void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                    return;

                BinCountCheck(int.Parse(textBox1.Text));
            }
            catch (FormatException)
            {
                textBox1.Text = "1";
                DACrux.SP.Common.Utility.ShowMessageBox("the value is not correct", MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                DACrux.SP.Common.Utility.ShowMessageBox(ex.ToString(), MessageBoxIcon.Error);
            }
        }

    };
}