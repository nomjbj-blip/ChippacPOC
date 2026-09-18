using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.Base;

namespace DACrux.FPDMap
{
    public partial class DefectImageList : UserControl
    {
        #region 이벤트

        /// <summary>
        /// 선택된 Defect Item 이 변경되면 발생하는 이벤트 입니다.
        /// </summary>
        public event EventHandler SelectedDefectChanged;

        #endregion

        #region 멤버 변수

        public static readonly int ITEM_MARGIN = 6;

        private Size _itemSize;
        
        private Dictionary<string, Color> _backColorDic = new Dictionary<string, Color>();

        #endregion

        #region 생성자

        public DefectImageList()
        {
            InitializeComponent();
            DefectList = new List<Defect>();
            
            DefectImageItem item = new DefectImageItem();
            _itemSize = item.Size;
        }

        #endregion

        #region 메서드

        /// <summary>
        /// Defect 정보를 로드합니다.
        /// </summary>
        public void LoadDefect()
        {
            flowLayoutPanel1.Controls.Clear();

            lblCount.Text = String.Format("{0:N0}", DefectList.Count);

            UpdateBindingCount();
            BindingData();
        }

        protected virtual void OnSelectedDefectChanged(EventArgs e)
        {
            if (SelectedDefectChanged != null)
                SelectedDefectChanged(this, e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateBindingCount();
        }

        private void DefectImageControl_Enter(object sender, EventArgs e)
        {
            SelectedItem = sender as DefectImageItem;
            OnSelectedDefectChanged(EventArgs.Empty);
        }

        private void ClearItems()
        {
            foreach (Control ctl in flowLayoutPanel1.Controls)
            {
                ctl.Enter -= new EventHandler(DefectImageControl_Enter);
                ctl.Dispose();
            }

            flowLayoutPanel1.Controls.Clear();
        }

        private void UpdateItemSize()
        {
            foreach (DefectImageItem item in flowLayoutPanel1.Controls)
                item.Size = _itemSize;
        }

        private void UpdateBindingCount()
        {
            if (ItemSize == Size.Empty)
                return;

            Size s = GetPreferredSize(ClientSize);
            
            int x = (int)(flowLayoutPanel1.Width / (ItemSize.Width + ITEM_MARGIN));
            int y = (int)(flowLayoutPanel1.Height / (ItemSize.Height + ITEM_MARGIN));

            CountPerPage = x * y;

            if (CountPerPage == 0)
                CountPerPage = 1;

            TotalPage = (int)Math.Ceiling(DefectList.Count / (double)CountPerPage);
            CurrentPage = 1;
            UpdateButtonEnabled();
        }

        private void BindingData()
        {
            ClearItems();

            for (int i = 0; i < CountPerPage; i++)
            {
                int index = i + (CurrentPage - 1) * CountPerPage;

                if (index >= DefectList.Count)
                    break;

                DefectImageItem ctl = new DefectImageItem(DefectList[index]);
                ctl.Size = _itemSize;
                ctl.Enter += new EventHandler(DefectImageControl_Enter);

                if (!String.IsNullOrEmpty(DefectList[index].DEFECT_TYPE) && _backColorDic.ContainsKey(DefectList[index].DEFECT_TYPE))
                    ctl.BackColor = _backColorDic[DefectList[index].DEFECT_TYPE];

                flowLayoutPanel1.Controls.Add(ctl);
            }
        }
        
        /// <summary>
        /// Defect Type에 따른 Item의 BackColor를 설정합니다.
        /// </summary>
        public void SetItemBackColorByDefectType(string defectType, Color backColor)
        {
            _backColorDic[defectType] = backColor;
        }

        #endregion

        #region 프로퍼티

        /// <summary>
        /// Defect 리스트를 가져옵니다.
        /// </summary>
        [Browsable(false)]
        public List<Defect> DefectList
        {
            get;
            private set;
        } 

        /// <summary>
        /// 선택된 DefectImageItem 을 가져옵니다.
        /// </summary>
        [Browsable(false)]
        public DefectImageItem SelectedItem
        {
            get;
            private set;
        }

        /// <summary>
        /// DefectImageItem 컨트롤의 Size를 설정하거나 가져옵니다.
        /// </summary>
        [DefaultValue(typeof(Size), "120, 150")]
        [Description("Defect Image 컨트롤의 Size를 나타냅니다.")]
        public Size ItemSize
        {
            get { return _itemSize; }
            set { _itemSize = value; UpdateItemSize(); }
        }

        [Browsable(false)]
        [DefaultValue(1)]
        public int CurrentPage
        {
            get;
            private set;
        }

        [Browsable(false)]
        [DefaultValue(1)]
        public int TotalPage
        {
            get;
            private set;
        }

        [Browsable(false)]
        [DefaultValue(1)]
        public int CountPerPage
        {
            get;
            private set;
        }

        //public DefectImageItem this[Defect defect]
        //{
        //    get
        //    {
        //    }
        //}

        #endregion

        private void btnPrev_Click(object sender, EventArgs e)
        {
            CurrentPage--;
            BindingData();
            UpdateButtonEnabled();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            CurrentPage++;
            BindingData();
            UpdateButtonEnabled();
        }

        private void UpdateButtonEnabled()
        {
            btnNext.Enabled = CurrentPage != TotalPage;
            btnPrev.Enabled = CurrentPage > 1;
            lblPage.Text = String.Format("{0} / {1}", CurrentPage, TotalPage);
        }
    }
}
