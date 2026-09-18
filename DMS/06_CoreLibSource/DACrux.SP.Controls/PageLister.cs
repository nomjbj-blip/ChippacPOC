using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace DACrux.SP.Controls
{
    /// <summary>
    /// User control which is used to simplify page representation of data.
    /// </summary>
    [ToolboxItem(true)]
    public class PageLister : System.Windows.Forms.UserControl
    {
        #region " Member Fields "

        private System.Windows.Forms.Label labelPage;
        private System.Windows.Forms.LinkLabel linkLabelFirst;
        private System.Windows.Forms.LinkLabel linkLabelPrev;
        private System.Windows.Forms.LinkLabel linkLabelNext;
        private System.Windows.Forms.LinkLabel linkLabelLast;

        private int pagesCount;

        // number of pixels between links
        private const int gap = 3;

        private int currentPage = 1;
        private int numPagesShown = 3;

        private ArrayList pages = new ArrayList();

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;


        #endregion

        #region " Property "

        /// <summary>
        /// Gets or sets the total number of pages.
        /// </summary>
        [Bindable(true),
        DefaultValue(5),
        Description("Total number of pages.")]
        public int PagesCount
        {
            get
            {
                return pagesCount;
            }
            set
            {
                pagesCount = value;

                this.Visible = !(pagesCount == 0);

                PopulateLinks();
            }
        }

        /// <summary>
        /// Gets or sets a number of the current page.
        /// </summary>
        [Bindable(true),
        DefaultValue(1),
        Description("Specifies current page's number")]
        public int CurrentPage
        {
            get
            {
                return currentPage;
            }
            set
            {
                currentPage = value;

                PopulateLinks();
            }
        }

        /// <summary>
        /// Gets or sets the number of pages shown on the
        /// each side of the current page. So, the maximal total number
        /// of numbered page links is NumPagesShown * 2 + 1.
        /// </summary>
        [Bindable(true),
        DefaultValue(3),
        Description("Specifies the number of pages shown on the each "
        + "side of the current page")]
        public int NumPagesShown
        {
            get
            {
                return numPagesShown;
            }
            set
            {
                numPagesShown = value;

                PopulateLinks();
            }
        }

        #endregion

        #region " Creator "

        public PageLister()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
        }

        #endregion

        #region " Event & Delegate "

        /// <summary>
        /// This delegate is called when current page of lister is
        /// changed.
        /// </summary>
        public delegate void PageChangeHandler(int currentPage);

        /// <summary>
        /// Event fired when the current page is changed.
        /// </summary>
        public event PageChangeHandler PageChanged;

        #endregion

        #region " Method "

        private void PopulateLinks()
        {
            if (pagesCount < currentPage
                || pagesCount == 0
                || currentPage == 0)
            {
                return;
            }

            labelPage.Text = currentPage.ToString()
                + " / " + pagesCount.ToString();

            linkLabelFirst.Location = new Point(labelPage.Location.X + labelPage.Width + gap, labelPage.Location.Y);
            linkLabelPrev.Location = new Point(linkLabelFirst.Location.X + linkLabelFirst.Width + gap, linkLabelFirst.Location.Y);

            int pagesLeft = numPagesShown;
            int pagesRight = numPagesShown;
            int pagesShownTotal = (int)Math.Min(2 * numPagesShown + 1, pagesCount);

            if (currentPage <= numPagesShown)
            {
                pagesLeft = currentPage - 1;
                pagesRight = pagesShownTotal - pagesLeft - 1;
            }
            else
                if (pagesCount - currentPage <= numPagesShown)
                {
                    pagesRight = pagesCount - currentPage;
                    pagesLeft = pagesShownTotal - pagesRight - 1;
                }

            for (int i = 0; i < Math.Min(pagesShownTotal, pages.Count); i++)
            {
                int pageNum = (int)(currentPage - pagesLeft + i);
                LinkLabel page = (LinkLabel)pages[i];
                page.Tag = pageNum;
                page.Text = page.Tag.ToString();
                page.Font = pageNum == currentPage ? new Font(this.Font, FontStyle.Bold) : null;

                LinkLabel pagePrev = i > 0 ? (LinkLabel)pages[i - 1] : linkLabelPrev;
                page.Location = new Point(pagePrev.Location.X + pagePrev.Width + gap, pagePrev.Location.Y);
            }

            if (pages.Count > pagesShownTotal)
            {
                for (int i = pages.Count - 1; i >= pagesShownTotal; i--)
                {
                    LinkLabel page = (LinkLabel)pages[i];
                    page.Parent = null;
                    page.Visible = false;
                    pages.RemoveAt(i);
                    page.Dispose();
                }
            }
            else
                if (pages.Count < pagesShownTotal)
                {
                    for (int i = pages.Count; i < pagesShownTotal; i++)
                    {
                        LinkLabel page = new LinkLabel();
                        page.AutoSize = true;
                        page.Tag = (int)(currentPage - pagesLeft + i);
                        page.Text = page.Tag.ToString();

                        page.Height = linkLabelFirst.Height;

                        LinkLabel pagePrev = pages.Count > 0 ? (LinkLabel)pages[i - 1] : linkLabelPrev;

                        page.Font = i == currentPage ? new Font(this.Font, FontStyle.Bold) : null;

                        page.Parent = this;

                        page.Location = new Point(pagePrev.Location.X + pagePrev.Width + gap, pagePrev.Location.Y);

                        pages.Add(page);
                        page.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkClicked);
                    }
                }

            Debug.Assert(pages.Count > 0);

            LinkLabel pageLast = (LinkLabel)pages[pages.Count - 1];
            linkLabelNext.Location = new Point(pageLast.Location.X + pageLast.Width + gap, linkLabelNext.Location.Y);
            linkLabelLast.Location = new Point(linkLabelNext.Location.X + linkLabelNext.Width, linkLabelLast.Location.Y);

            CheckPages();
        }

        private void CheckPages()
        {
            linkLabelFirst.Enabled = linkLabelPrev.Enabled = currentPage > 1;
            linkLabelNext.Enabled = linkLabelLast.Enabled = currentPage < pagesCount;
        }

        private void linkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel page = (LinkLabel)sender;
            switch (page.Text)
            {
                case "<<":
                    if (currentPage == 1)
                        return;
                    currentPage = 1;
                    break;
                case "<":
                    if (currentPage == 1)
                        return;
                    currentPage--;
                    break;
                case ">":
                    if (currentPage == pagesCount)
                        return;
                    currentPage++;
                    break;
                case ">>":
                    if (currentPage == pagesCount)
                        return;
                    currentPage = pagesCount;
                    break;
                default:
                    currentPage = (int)page.Tag;
                    break;
            }

            PopulateLinks();

            if (PageChanged != null)
                PageChanged(currentPage);
        }

        #endregion

        #region " Designer Generated "

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelPage = new System.Windows.Forms.Label();
            this.linkLabelFirst = new System.Windows.Forms.LinkLabel();
            this.linkLabelPrev = new System.Windows.Forms.LinkLabel();
            this.linkLabelNext = new System.Windows.Forms.LinkLabel();
            this.linkLabelLast = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // labelPage
            // 
            this.labelPage.AutoSize = true;
            this.labelPage.Location = new System.Drawing.Point(8, 8);
            this.labelPage.Name = "labelPage";
            this.labelPage.Size = new System.Drawing.Size(80, 18);
            this.labelPage.TabIndex = 0;
            this.labelPage.Text = "Page ... of ...";
            // 
            // linkLabelFirst
            // 
            this.linkLabelFirst.AutoSize = true;
            this.linkLabelFirst.Location = new System.Drawing.Point(152, 8);
            this.linkLabelFirst.Name = "linkLabelFirst";
            this.linkLabelFirst.Size = new System.Drawing.Size(20, 18);
            this.linkLabelFirst.TabIndex = 1;
            this.linkLabelFirst.TabStop = true;
            this.linkLabelFirst.Text = "<<";
            this.linkLabelFirst.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkClicked);
            // 
            // linkLabelPrev
            // 
            this.linkLabelPrev.AutoSize = true;
            this.linkLabelPrev.Location = new System.Drawing.Point(187, 8);
            this.linkLabelPrev.Name = "linkLabelPrev";
            this.linkLabelPrev.Size = new System.Drawing.Size(13, 18);
            this.linkLabelPrev.TabIndex = 2;
            this.linkLabelPrev.TabStop = true;
            this.linkLabelPrev.Text = "<";
            this.linkLabelPrev.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkClicked);
            // 
            // linkLabelNext
            // 
            this.linkLabelNext.AutoSize = true;
            this.linkLabelNext.Location = new System.Drawing.Point(280, 8);
            this.linkLabelNext.Name = "linkLabelNext";
            this.linkLabelNext.Size = new System.Drawing.Size(13, 18);
            this.linkLabelNext.TabIndex = 3;
            this.linkLabelNext.TabStop = true;
            this.linkLabelNext.Text = ">";
            this.linkLabelNext.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkClicked);
            // 
            // linkLabelLast
            // 
            this.linkLabelLast.AutoSize = true;
            this.linkLabelLast.Location = new System.Drawing.Point(315, 8);
            this.linkLabelLast.Name = "linkLabelLast";
            this.linkLabelLast.Size = new System.Drawing.Size(20, 18);
            this.linkLabelLast.TabIndex = 4;
            this.linkLabelLast.TabStop = true;
            this.linkLabelLast.Text = ">>";
            this.linkLabelLast.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkClicked);
            // 
            // PageLister
            // 
            this.Controls.Add(this.linkLabelLast);
            this.Controls.Add(this.linkLabelNext);
            this.Controls.Add(this.linkLabelPrev);
            this.Controls.Add(this.linkLabelFirst);
            this.Controls.Add(this.labelPage);
            this.Name = "PageLister";
            this.Size = new System.Drawing.Size(640, 32);
            this.ResumeLayout(false);

        }
        #endregion

        #endregion
    }
}
