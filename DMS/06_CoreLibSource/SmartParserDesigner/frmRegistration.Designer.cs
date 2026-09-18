namespace SmartParser.Designer
{
    partial class frmRegistration
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRegistration));
            this.tabTextViewer = new System.Windows.Forms.TabControl();
            this.tabSetup = new System.Windows.Forms.TabControl();
            this.tabPageEntity = new System.Windows.Forms.TabPage();
            this.lvEntity = new System.Windows.Forms.ListView();
            this.uclTitleEntity = new DACrux.SP.Controls.uclTitle();
            this.uclRegexEntity = new DACrux.SP.Controls.RegexEntityControl();
            this.uclTitle1 = new DACrux.SP.Controls.uclTitle();
            this.tabPageExplorer = new System.Windows.Forms.TabPage();
            this.pnlMatch = new System.Windows.Forms.Panel();
            this.lvMatch = new System.Windows.Forms.ListView();
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.uclTitleMatch = new DACrux.SP.Controls.uclTitle();
            this.pnlTreeView = new System.Windows.Forms.Panel();
            this.tvExplorer = new System.Windows.Forms.TreeView();
            this.uclTitle3 = new DACrux.SP.Controls.uclTitle();
            this.tabPageConvert = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BinCheckList = new System.Windows.Forms.CheckedListBox();
            this.BinCountView = new System.Windows.Forms.DataGridView();
            this.btnCount = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtRight = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtLeft = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtBottom = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTop = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnConvert = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.uclTitle2 = new DACrux.SP.Controls.uclTitle();
            this.tabSetup.SuspendLayout();
            this.tabPageEntity.SuspendLayout();
            this.tabPageExplorer.SuspendLayout();
            this.pnlMatch.SuspendLayout();
            this.pnlTreeView.SuspendLayout();
            this.tabPageConvert.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BinCountView)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabTextViewer
            // 
            this.tabTextViewer.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabTextViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabTextViewer.Location = new System.Drawing.Point(295, 0);
            this.tabTextViewer.Multiline = true;
            this.tabTextViewer.Name = "tabTextViewer";
            this.tabTextViewer.SelectedIndex = 0;
            this.tabTextViewer.Size = new System.Drawing.Size(331, 543);
            this.tabTextViewer.TabIndex = 11;
            // 
            // tabSetup
            // 
            this.tabSetup.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabSetup.Controls.Add(this.tabPageEntity);
            this.tabSetup.Controls.Add(this.tabPageExplorer);
            this.tabSetup.Controls.Add(this.tabPageConvert);
            this.tabSetup.Dock = System.Windows.Forms.DockStyle.Left;
            this.tabSetup.Font = new System.Drawing.Font("Tahoma", 8F);
            this.tabSetup.Location = new System.Drawing.Point(0, 0);
            this.tabSetup.Name = "tabSetup";
            this.tabSetup.SelectedIndex = 0;
            this.tabSetup.Size = new System.Drawing.Size(295, 543);
            this.tabSetup.TabIndex = 14;
            // 
            // tabPageEntity
            // 
            this.tabPageEntity.Controls.Add(this.lvEntity);
            this.tabPageEntity.Controls.Add(this.uclTitleEntity);
            this.tabPageEntity.Controls.Add(this.uclRegexEntity);
            this.tabPageEntity.Controls.Add(this.uclTitle1);
            this.tabPageEntity.Location = new System.Drawing.Point(4, 4);
            this.tabPageEntity.Name = "tabPageEntity";
            this.tabPageEntity.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEntity.Size = new System.Drawing.Size(287, 517);
            this.tabPageEntity.TabIndex = 0;
            this.tabPageEntity.Text = "Entity";
            this.tabPageEntity.UseVisualStyleBackColor = true;
            // 
            // lvEntity
            // 
            this.lvEntity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvEntity.GridLines = true;
            this.lvEntity.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvEntity.HideSelection = false;
            this.lvEntity.Location = new System.Drawing.Point(3, 463);
            this.lvEntity.MultiSelect = false;
            this.lvEntity.Name = "lvEntity";
            this.lvEntity.Size = new System.Drawing.Size(281, 51);
            this.lvEntity.TabIndex = 21;
            this.lvEntity.UseCompatibleStateImageBehavior = false;
            this.lvEntity.View = System.Windows.Forms.View.Details;
            this.lvEntity.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lvEntity_MouseClick);
            // 
            // uclTitleEntity
            // 
            this.uclTitleEntity.BackColor = System.Drawing.Color.Transparent;
            this.uclTitleEntity.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("uclTitleEntity.BackgroundImage")));
            this.uclTitleEntity.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.uclTitleEntity.Dock = System.Windows.Forms.DockStyle.Top;
            this.uclTitleEntity.EndColor = System.Drawing.Color.White;
            this.uclTitleEntity.Font = new System.Drawing.Font("Tahoma", 8F);
            this.uclTitleEntity.GradientStyle = DACrux.SP.Controls.GradientMode.Horizontal;
            this.uclTitleEntity.Location = new System.Drawing.Point(3, 443);
            this.uclTitleEntity.Margin = new System.Windows.Forms.Padding(0);
            this.uclTitleEntity.Name = "uclTitleEntity";
            this.uclTitleEntity.Size = new System.Drawing.Size(281, 20);
            this.uclTitleEntity.StartColor = System.Drawing.Color.LightSteelBlue;
            this.uclTitleEntity.TabIndex = 19;
            this.uclTitleEntity.Title = "Entity Component";
            // 
            // uclRegexEntity
            // 
            this.uclRegexEntity.array = "Array";
            this.uclRegexEntity.dec = "\\d   Matches a number.";
            this.uclRegexEntity.Def = "Default";
            this.uclRegexEntity.defaults = "Default";
            this.uclRegexEntity.Dock = System.Windows.Forms.DockStyle.Top;
            this.uclRegexEntity.endline = "$   Matches end of the string or line. Use at the end of the expression.";
            this.uclRegexEntity.etc = "Etc";
            this.uclRegexEntity.find = "Find";
            this.uclRegexEntity.Font = new System.Drawing.Font("Tahoma", 8F);
            this.uclRegexEntity.groupequal = "\\k<name>   Match the string from the captured group that is named.";
            this.uclRegexEntity.grouping = "Grouping";
            this.uclRegexEntity.groupname = "(?<name>subexpression)   Captures a matched subexpression with name.";
            this.uclRegexEntity.Ignorecase = "Ignore Case";
            this.uclRegexEntity.label = "Label";
            this.uclRegexEntity.Location = new System.Drawing.Point(3, 23);
            this.uclRegexEntity.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.uclRegexEntity.name = "Name";
            this.uclRegexEntity.Name = "uclRegexEntity";
            this.uclRegexEntity.newline = "\\n   Matches a new line.";
            this.uclRegexEntity.nfromtime = "{n,m}   Match from n to m times.";
            this.uclRegexEntity.nleasttime = "{n,}   Match at least n times.";
            this.uclRegexEntity.nondec = "\\D   Matches a non-number.";
            this.uclRegexEntity.nonspace = "\\S   Matches a non-white space";
            this.uclRegexEntity.nonword = "\\W   Matches a non-word.";
            this.uclRegexEntity.ntime = "{n}   Match exactly n times.";
            this.uclRegexEntity.one = "+   Match one or more times.";
            this.uclRegexEntity.quanti = "Quantifiers";
            this.uclRegexEntity.RegexOption = ((System.Text.RegularExpressions.RegexOptions)((System.Text.RegularExpressions.RegexOptions.ExplicitCapture | System.Text.RegularExpressions.RegexOptions.Singleline)));
            this.uclRegexEntity.RegexString = "";
            this.uclRegexEntity.regular = "Regular Expression";
            this.uclRegexEntity.remove = "Remove";
            this.uclRegexEntity.returnhome = "\\r   Matches a tab.";
            this.uclRegexEntity.save = "Save";
            this.uclRegexEntity.sep = "Separator";
            this.uclRegexEntity.ShowIgnoreCase = false;
            this.uclRegexEntity.Size = new System.Drawing.Size(281, 420);
            this.uclRegexEntity.space = "\\s   Matches a white space.";
            this.uclRegexEntity.startline = "^   Matches start of the string or line. Use at the beginning of the expression.";
            this.uclRegexEntity.streammode = "Stream Mode";
            this.uclRegexEntity.tab = "\\t   Matches a tab.";
            this.uclRegexEntity.TabIndex = 18;
            this.uclRegexEntity.valuegroup = "Value Group";
            this.uclRegexEntity.valuemust = "Value must exists.";
            this.uclRegexEntity.valuetype = "Value Type";
            this.uclRegexEntity.word = "\\w   Matches a word.";
            this.uclRegexEntity.zero = "*   Match zero or more times.";
            this.uclRegexEntity.zerone = "?   Match zero or one time.";
            // 
            // uclTitle1
            // 
            this.uclTitle1.BackColor = System.Drawing.Color.Transparent;
            this.uclTitle1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("uclTitle1.BackgroundImage")));
            this.uclTitle1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.uclTitle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.uclTitle1.EndColor = System.Drawing.Color.White;
            this.uclTitle1.Font = new System.Drawing.Font("Tahoma", 8F);
            this.uclTitle1.GradientStyle = DACrux.SP.Controls.GradientMode.Horizontal;
            this.uclTitle1.Location = new System.Drawing.Point(3, 3);
            this.uclTitle1.Margin = new System.Windows.Forms.Padding(0);
            this.uclTitle1.Name = "uclTitle1";
            this.uclTitle1.Size = new System.Drawing.Size(281, 20);
            this.uclTitle1.StartColor = System.Drawing.Color.LightSteelBlue;
            this.uclTitle1.TabIndex = 15;
            this.uclTitle1.Title = "Regex Entity";
            // 
            // tabPageExplorer
            // 
            this.tabPageExplorer.Controls.Add(this.pnlMatch);
            this.tabPageExplorer.Controls.Add(this.uclTitleMatch);
            this.tabPageExplorer.Controls.Add(this.pnlTreeView);
            this.tabPageExplorer.Controls.Add(this.uclTitle3);
            this.tabPageExplorer.Location = new System.Drawing.Point(4, 4);
            this.tabPageExplorer.Name = "tabPageExplorer";
            this.tabPageExplorer.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageExplorer.Size = new System.Drawing.Size(287, 517);
            this.tabPageExplorer.TabIndex = 2;
            this.tabPageExplorer.Text = "Explorer";
            this.tabPageExplorer.UseVisualStyleBackColor = true;
            // 
            // pnlMatch
            // 
            this.pnlMatch.Controls.Add(this.lvMatch);
            this.pnlMatch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMatch.Location = new System.Drawing.Point(3, 392);
            this.pnlMatch.Name = "pnlMatch";
            this.pnlMatch.Size = new System.Drawing.Size(281, 122);
            this.pnlMatch.TabIndex = 24;
            // 
            // lvMatch
            // 
            this.lvMatch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvMatch.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader7,
            this.columnHeader1});
            this.lvMatch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvMatch.FullRowSelect = true;
            this.lvMatch.GridLines = true;
            this.lvMatch.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvMatch.Location = new System.Drawing.Point(0, 0);
            this.lvMatch.MultiSelect = false;
            this.lvMatch.Name = "lvMatch";
            this.lvMatch.Size = new System.Drawing.Size(281, 122);
            this.lvMatch.TabIndex = 3;
            this.lvMatch.UseCompatibleStateImageBehavior = false;
            this.lvMatch.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "";
            this.columnHeader7.Width = 70;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "";
            this.columnHeader1.Width = 190;
            // 
            // uclTitleMatch
            // 
            this.uclTitleMatch.BackColor = System.Drawing.Color.Transparent;
            this.uclTitleMatch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("uclTitleMatch.BackgroundImage")));
            this.uclTitleMatch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.uclTitleMatch.Dock = System.Windows.Forms.DockStyle.Top;
            this.uclTitleMatch.EndColor = System.Drawing.Color.White;
            this.uclTitleMatch.Font = new System.Drawing.Font("Tahoma", 8F);
            this.uclTitleMatch.GradientStyle = DACrux.SP.Controls.GradientMode.Horizontal;
            this.uclTitleMatch.Location = new System.Drawing.Point(3, 372);
            this.uclTitleMatch.Margin = new System.Windows.Forms.Padding(0);
            this.uclTitleMatch.Name = "uclTitleMatch";
            this.uclTitleMatch.Size = new System.Drawing.Size(281, 20);
            this.uclTitleMatch.StartColor = System.Drawing.Color.LightSteelBlue;
            this.uclTitleMatch.TabIndex = 23;
            this.uclTitleMatch.Title = "";
            // 
            // pnlTreeView
            // 
            this.pnlTreeView.Controls.Add(this.tvExplorer);
            this.pnlTreeView.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTreeView.Location = new System.Drawing.Point(3, 23);
            this.pnlTreeView.Name = "pnlTreeView";
            this.pnlTreeView.Size = new System.Drawing.Size(281, 349);
            this.pnlTreeView.TabIndex = 22;
            // 
            // tvExplorer
            // 
            this.tvExplorer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvExplorer.Location = new System.Drawing.Point(0, 0);
            this.tvExplorer.Name = "tvExplorer";
            this.tvExplorer.Size = new System.Drawing.Size(281, 349);
            this.tvExplorer.TabIndex = 22;
            // 
            // uclTitle3
            // 
            this.uclTitle3.BackColor = System.Drawing.Color.Transparent;
            this.uclTitle3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("uclTitle3.BackgroundImage")));
            this.uclTitle3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.uclTitle3.Dock = System.Windows.Forms.DockStyle.Top;
            this.uclTitle3.EndColor = System.Drawing.Color.White;
            this.uclTitle3.Font = new System.Drawing.Font("Tahoma", 8F);
            this.uclTitle3.GradientStyle = DACrux.SP.Controls.GradientMode.Horizontal;
            this.uclTitle3.Location = new System.Drawing.Point(3, 3);
            this.uclTitle3.Margin = new System.Windows.Forms.Padding(0);
            this.uclTitle3.Name = "uclTitle3";
            this.uclTitle3.Size = new System.Drawing.Size(281, 20);
            this.uclTitle3.StartColor = System.Drawing.Color.LightSteelBlue;
            this.uclTitle3.TabIndex = 20;
            this.uclTitle3.Title = "";
            // 
            // tabPageConvert
            // 
            this.tabPageConvert.Controls.Add(this.splitContainer1);
            this.tabPageConvert.Controls.Add(this.uclTitle2);
            this.tabPageConvert.Location = new System.Drawing.Point(4, 4);
            this.tabPageConvert.Name = "tabPageConvert";
            this.tabPageConvert.Size = new System.Drawing.Size(287, 517);
            this.tabPageConvert.TabIndex = 3;
            this.tabPageConvert.Text = "Convert";
            this.tabPageConvert.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 20);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Size = new System.Drawing.Size(287, 497);
            this.splitContainer1.SplitterDistance = 249;
            this.splitContainer1.TabIndex = 22;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.BinCheckList);
            this.groupBox1.Controls.Add(this.BinCountView);
            this.groupBox1.Controls.Add(this.btnCount);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(287, 249);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Bin Count";
            // 
            // BinCheckList
            // 
            this.BinCheckList.CheckOnClick = true;
            this.BinCheckList.FormattingEnabled = true;
            this.BinCheckList.Location = new System.Drawing.Point(28, 25);
            this.BinCheckList.Name = "BinCheckList";
            this.BinCheckList.Size = new System.Drawing.Size(178, 64);
            this.BinCheckList.TabIndex = 5;
            // 
            // BinCountView
            // 
            this.BinCountView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BinCountView.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BinCountView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.BinCountView.Location = new System.Drawing.Point(2, 95);
            this.BinCountView.Name = "BinCountView";
            this.BinCountView.RowTemplate.Height = 23;
            this.BinCountView.Size = new System.Drawing.Size(283, 224);
            this.BinCountView.TabIndex = 4;
            // 
            // btnCount
            // 
            this.btnCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCount.Location = new System.Drawing.Point(212, 67);
            this.btnCount.Name = "btnCount";
            this.btnCount.Size = new System.Drawing.Size(67, 22);
            this.btnCount.TabIndex = 3;
            this.btnCount.Text = "Count";
            this.btnCount.UseVisualStyleBackColor = true;
            this.btnCount.Click += new System.EventHandler(this.btnCount_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(212, 25);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(95, 20);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Bin";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox4);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.txtRight);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.txtLeft);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtBottom);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtTop);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.btnConvert);
            this.groupBox2.Controls.Add(this.textBox3);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.textBox2);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(287, 244);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Bin Convert";
            // 
            // textBox4
            // 
            this.textBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox4.Location = new System.Drawing.Point(68, 67);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(100, 20);
            this.textBox4.TabIndex = 16;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 75);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Fail Bin";
            // 
            // txtRight
            // 
            this.txtRight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRight.Location = new System.Drawing.Point(68, 170);
            this.txtRight.Name = "txtRight";
            this.txtRight.Size = new System.Drawing.Size(100, 20);
            this.txtRight.TabIndex = 14;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 173);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Right";
            // 
            // txtLeft
            // 
            this.txtLeft.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLeft.Location = new System.Drawing.Point(68, 144);
            this.txtLeft.Name = "txtLeft";
            this.txtLeft.Size = new System.Drawing.Size(100, 20);
            this.txtLeft.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 148);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(26, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Left";
            // 
            // txtBottom
            // 
            this.txtBottom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBottom.Location = new System.Drawing.Point(68, 118);
            this.txtBottom.Name = "txtBottom";
            this.txtBottom.Size = new System.Drawing.Size(100, 20);
            this.txtBottom.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 123);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Bottom";
            // 
            // txtTop
            // 
            this.txtTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTop.Location = new System.Drawing.Point(68, 92);
            this.txtTop.Name = "txtTop";
            this.txtTop.Size = new System.Drawing.Size(100, 20);
            this.txtTop.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Top";
            // 
            // btnConvert
            // 
            this.btnConvert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConvert.Location = new System.Drawing.Point(188, 173);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(93, 21);
            this.btnConvert.TabIndex = 6;
            this.btnConvert.Text = "Convert";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // textBox3
            // 
            this.textBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox3.Location = new System.Drawing.Point(68, 43);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 20);
            this.textBox3.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Null Bin";
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.Location = new System.Drawing.Point(68, 17);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Good Bin";
            // 
            // uclTitle2
            // 
            this.uclTitle2.BackColor = System.Drawing.Color.Transparent;
            this.uclTitle2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("uclTitle2.BackgroundImage")));
            this.uclTitle2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.uclTitle2.Dock = System.Windows.Forms.DockStyle.Top;
            this.uclTitle2.EndColor = System.Drawing.Color.White;
            this.uclTitle2.Font = new System.Drawing.Font("Tahoma", 8F);
            this.uclTitle2.GradientStyle = DACrux.SP.Controls.GradientMode.Horizontal;
            this.uclTitle2.Location = new System.Drawing.Point(0, 0);
            this.uclTitle2.Margin = new System.Windows.Forms.Padding(0);
            this.uclTitle2.Name = "uclTitle2";
            this.uclTitle2.Size = new System.Drawing.Size(287, 20);
            this.uclTitle2.StartColor = System.Drawing.Color.LightSteelBlue;
            this.uclTitle2.TabIndex = 21;
            this.uclTitle2.Title = "Convert";
            // 
            // frmRegistration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 543);
            this.Controls.Add(this.tabTextViewer);
            this.Controls.Add(this.tabSetup);
            this.Font = new System.Drawing.Font("Tahoma", 9F);
            this.Name = "frmRegistration";
            this.Text = "Entity & Section Registration";
            this.tabSetup.ResumeLayout(false);
            this.tabPageEntity.ResumeLayout(false);
            this.tabPageExplorer.ResumeLayout(false);
            this.pnlMatch.ResumeLayout(false);
            this.pnlTreeView.ResumeLayout(false);
            this.tabPageConvert.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BinCountView)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabTextViewer;
        private System.Windows.Forms.TabControl tabSetup;
        private System.Windows.Forms.TabPage tabPageEntity;
        private DACrux.SP.Controls.uclTitle uclTitleEntity;
        private DACrux.SP.Controls.RegexEntityControl uclRegexEntity;
        private DACrux.SP.Controls.uclTitle uclTitle1;
        private System.Windows.Forms.ListView lvEntity;
        private System.Windows.Forms.TabPage tabPageExplorer;
        private System.Windows.Forms.Panel pnlTreeView;
        private DACrux.SP.Controls.uclTitle uclTitle3;
        private System.Windows.Forms.TreeView tvExplorer;
        private DACrux.SP.Controls.uclTitle uclTitleMatch;
        private System.Windows.Forms.Panel pnlMatch;
        private System.Windows.Forms.ListView lvMatch;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.TabPage tabPageConvert;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private DACrux.SP.Controls.uclTitle uclTitle2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox BinCheckList;
        private System.Windows.Forms.DataGridView BinCountView;
        private System.Windows.Forms.Button btnCount;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtRight;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtLeft;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtBottom;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtTop;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
    }
}