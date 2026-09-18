namespace DACrux.SP.Controls
{
    partial class RegexBuilder
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.txtRegex = new System.Windows.Forms.TextBox();
            this.menuCounter = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuGroup = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEtc = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripMenuItem();
            this.vToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wMatchesANonwordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sMatchesAWhiteSpaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sMatchesANonwhiteSpaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dMatchesANumberToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dMatchesANonnumberToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripMenuItem();
            this.matchesEndOfTheStringOrLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnQuantifiers = new System.Windows.Forms.Button();
            this.btnGrouping = new System.Windows.Forms.Button();
            this.btnEtc = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.chkIgnoreCase = new System.Windows.Forms.CheckBox();
            this.menuCounter.SuspendLayout();
            this.menuGroup.SuspendLayout();
            this.menuEtc.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtRegex
            // 
            this.txtRegex.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRegex.Font = new System.Drawing.Font("Consolas", 8F);
            this.txtRegex.HideSelection = false;
            this.txtRegex.Location = new System.Drawing.Point(4, 22);
            this.txtRegex.Multiline = true;
            this.txtRegex.Name = "txtRegex";
            this.txtRegex.Size = new System.Drawing.Size(252, 44);
            this.txtRegex.TabIndex = 0;
            // 
            // menuCounter
            // 
            this.menuCounter.Font = new System.Drawing.Font("Tahoma", 8F);
            this.menuCounter.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.toolStripMenuItem6});
            this.menuCounter.Name = "menuCounter";
            this.menuCounter.Size = new System.Drawing.Size(231, 136);
            this.menuCounter.MouseLeave += new System.EventHandler(this.menuContext_MouseLeave);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Font = new System.Drawing.Font("Tahoma", 8F);
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(230, 22);
            this.toolStripMenuItem1.Tag = "*";
            this.toolStripMenuItem1.Text = "*   Match zero or more times.";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Font = new System.Drawing.Font("Tahoma", 8F);
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(230, 22);
            this.toolStripMenuItem2.Tag = "+";
            this.toolStripMenuItem2.Text = "+   Match one or more times.";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Font = new System.Drawing.Font("Tahoma", 8F);
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(230, 22);
            this.toolStripMenuItem3.Tag = "?";
            this.toolStripMenuItem3.Text = "?   Match zero or one time.";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Font = new System.Drawing.Font("Tahoma", 8F);
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(230, 22);
            this.toolStripMenuItem4.Tag = "{n}";
            this.toolStripMenuItem4.Text = "{n}   Match exactly n times.";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Font = new System.Drawing.Font("Tahoma", 8F);
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(230, 22);
            this.toolStripMenuItem5.Tag = "{n,}";
            this.toolStripMenuItem5.Text = "{n,}   Match at least n times.";
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Font = new System.Drawing.Font("Tahoma", 8F);
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(230, 22);
            this.toolStripMenuItem6.Tag = "{n,m}";
            this.toolStripMenuItem6.Text = "{n,m}   Match from n to m times.";
            // 
            // menuGroup
            // 
            this.menuGroup.Font = new System.Drawing.Font("Tahoma", 8F);
            this.menuGroup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem7,
            this.toolStripMenuItem8});
            this.menuGroup.Name = "menuCounter";
            this.menuGroup.Size = new System.Drawing.Size(433, 48);
            this.menuGroup.MouseLeave += new System.EventHandler(this.menuContext_MouseLeave);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Font = new System.Drawing.Font("Tahoma", 8F);
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(432, 22);
            this.toolStripMenuItem7.Tag = "(?<name>subexpression)";
            this.toolStripMenuItem7.Text = "(?<name>subexpression)   Captures a matched subexpression with name.";
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Font = new System.Drawing.Font("Tahoma", 8F);
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(432, 22);
            this.toolStripMenuItem8.Tag = "\\k<name>";
            this.toolStripMenuItem8.Text = "\\k<name>   Match the string from the captured group that is named.";
            // 
            // menuEtc
            // 
            this.menuEtc.Font = new System.Drawing.Font("Tahoma", 8F);
            this.menuEtc.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem9,
            this.toolStripMenuItem10,
            this.vToolStripMenuItem,
            this.wToolStripMenuItem,
            this.wMatchesANonwordToolStripMenuItem,
            this.sMatchesAWhiteSpaceToolStripMenuItem,
            this.sMatchesANonwhiteSpaceToolStripMenuItem,
            this.dMatchesANumberToolStripMenuItem,
            this.dMatchesANonnumberToolStripMenuItem,
            this.toolStripMenuItem12,
            this.matchesEndOfTheStringOrLineToolStripMenuItem});
            this.menuEtc.Name = "menuCounter";
            this.menuEtc.Size = new System.Drawing.Size(449, 246);
            this.menuEtc.MouseLeave += new System.EventHandler(this.menuContext_MouseLeave);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Font = new System.Drawing.Font("Tahoma", 8F);
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(448, 22);
            this.toolStripMenuItem9.Tag = "\\t";
            this.toolStripMenuItem9.Text = "\\t   Matches a tab.";
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.Font = new System.Drawing.Font("Tahoma", 8F);
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new System.Drawing.Size(448, 22);
            this.toolStripMenuItem10.Tag = "\\r";
            this.toolStripMenuItem10.Text = "\\r   Matches a tab.";
            // 
            // vToolStripMenuItem
            // 
            this.vToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 8F);
            this.vToolStripMenuItem.Name = "vToolStripMenuItem";
            this.vToolStripMenuItem.Size = new System.Drawing.Size(448, 22);
            this.vToolStripMenuItem.Tag = "\\n";
            this.vToolStripMenuItem.Text = "\\n   Matches a new line.";
            // 
            // wToolStripMenuItem
            // 
            this.wToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 8F);
            this.wToolStripMenuItem.Name = "wToolStripMenuItem";
            this.wToolStripMenuItem.Size = new System.Drawing.Size(448, 22);
            this.wToolStripMenuItem.Tag = "\\w";
            this.wToolStripMenuItem.Text = "\\w   Matches a word.";
            // 
            // wMatchesANonwordToolStripMenuItem
            // 
            this.wMatchesANonwordToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 8F);
            this.wMatchesANonwordToolStripMenuItem.Name = "wMatchesANonwordToolStripMenuItem";
            this.wMatchesANonwordToolStripMenuItem.Size = new System.Drawing.Size(448, 22);
            this.wMatchesANonwordToolStripMenuItem.Tag = "\\W";
            this.wMatchesANonwordToolStripMenuItem.Text = "\\W   Matches a non-word.";
            // 
            // sMatchesAWhiteSpaceToolStripMenuItem
            // 
            this.sMatchesAWhiteSpaceToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 8F);
            this.sMatchesAWhiteSpaceToolStripMenuItem.Name = "sMatchesAWhiteSpaceToolStripMenuItem";
            this.sMatchesAWhiteSpaceToolStripMenuItem.Size = new System.Drawing.Size(448, 22);
            this.sMatchesAWhiteSpaceToolStripMenuItem.Tag = "\\s";
            this.sMatchesAWhiteSpaceToolStripMenuItem.Text = "\\s   Matches a white space.";
            // 
            // sMatchesANonwhiteSpaceToolStripMenuItem
            // 
            this.sMatchesANonwhiteSpaceToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 8F);
            this.sMatchesANonwhiteSpaceToolStripMenuItem.Name = "sMatchesANonwhiteSpaceToolStripMenuItem";
            this.sMatchesANonwhiteSpaceToolStripMenuItem.Size = new System.Drawing.Size(448, 22);
            this.sMatchesANonwhiteSpaceToolStripMenuItem.Tag = "\\S";
            this.sMatchesANonwhiteSpaceToolStripMenuItem.Text = "\\S   Matches a non-white space";
            // 
            // dMatchesANumberToolStripMenuItem
            // 
            this.dMatchesANumberToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 8F);
            this.dMatchesANumberToolStripMenuItem.Name = "dMatchesANumberToolStripMenuItem";
            this.dMatchesANumberToolStripMenuItem.Size = new System.Drawing.Size(448, 22);
            this.dMatchesANumberToolStripMenuItem.Tag = "\\d";
            this.dMatchesANumberToolStripMenuItem.Text = "\\d   Matches a number.";
            // 
            // dMatchesANonnumberToolStripMenuItem
            // 
            this.dMatchesANonnumberToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 8F);
            this.dMatchesANonnumberToolStripMenuItem.Name = "dMatchesANonnumberToolStripMenuItem";
            this.dMatchesANonnumberToolStripMenuItem.Size = new System.Drawing.Size(448, 22);
            this.dMatchesANonnumberToolStripMenuItem.Tag = "\\D";
            this.dMatchesANonnumberToolStripMenuItem.Text = "\\D   Matches a non-number.";
            // 
            // toolStripMenuItem12
            // 
            this.toolStripMenuItem12.Font = new System.Drawing.Font("Tahoma", 8F);
            this.toolStripMenuItem12.Name = "toolStripMenuItem12";
            this.toolStripMenuItem12.Size = new System.Drawing.Size(448, 22);
            this.toolStripMenuItem12.Tag = "^";
            this.toolStripMenuItem12.Text = "^   Matches start of the string or line. Use at the beginning of the expression.";
            // 
            // matchesEndOfTheStringOrLineToolStripMenuItem
            // 
            this.matchesEndOfTheStringOrLineToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 8F);
            this.matchesEndOfTheStringOrLineToolStripMenuItem.Name = "matchesEndOfTheStringOrLineToolStripMenuItem";
            this.matchesEndOfTheStringOrLineToolStripMenuItem.Size = new System.Drawing.Size(448, 22);
            this.matchesEndOfTheStringOrLineToolStripMenuItem.Tag = "$";
            this.matchesEndOfTheStringOrLineToolStripMenuItem.Text = "$   Matches end of the string or line. Use at the end of the expression.";
            // 
            // btnQuantifiers
            // 
            this.btnQuantifiers.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnQuantifiers.Font = new System.Drawing.Font("Tahoma", 8F);
            this.btnQuantifiers.Location = new System.Drawing.Point(132, 68);
            this.btnQuantifiers.Name = "btnQuantifiers";
            this.btnQuantifiers.Size = new System.Drawing.Size(63, 24);
            this.btnQuantifiers.TabIndex = 3;
            this.btnQuantifiers.Text = "Quantifiers";
            this.btnQuantifiers.UseVisualStyleBackColor = true;
            this.btnQuantifiers.Click += new System.EventHandler(this.btnQuantifiers_Click);
            // 
            // btnGrouping
            // 
            this.btnGrouping.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnGrouping.Font = new System.Drawing.Font("Tahoma", 8F);
            this.btnGrouping.Location = new System.Drawing.Point(193, 68);
            this.btnGrouping.Name = "btnGrouping";
            this.btnGrouping.Size = new System.Drawing.Size(64, 24);
            this.btnGrouping.TabIndex = 4;
            this.btnGrouping.Text = "Grouping";
            this.btnGrouping.UseVisualStyleBackColor = true;
            this.btnGrouping.Click += new System.EventHandler(this.btnGrouping_Click);
            // 
            // btnEtc
            // 
            this.btnEtc.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnEtc.Font = new System.Drawing.Font("Tahoma", 8F);
            this.btnEtc.Location = new System.Drawing.Point(86, 68);
            this.btnEtc.Name = "btnEtc";
            this.btnEtc.Size = new System.Drawing.Size(48, 24);
            this.btnEtc.TabIndex = 2;
            this.btnEtc.Text = "Etc";
            this.btnEtc.UseVisualStyleBackColor = true;
            this.btnEtc.Click += new System.EventHandler(this.btnEtc_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8F);
            this.label2.Location = new System.Drawing.Point(3, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Regular Expression";
            // 
            // chkIgnoreCase
            // 
            this.chkIgnoreCase.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.chkIgnoreCase.AutoSize = true;
            this.chkIgnoreCase.Font = new System.Drawing.Font("Tahoma", 8F);
            this.chkIgnoreCase.Location = new System.Drawing.Point(1, 73);
            this.chkIgnoreCase.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkIgnoreCase.Name = "chkIgnoreCase";
            this.chkIgnoreCase.Size = new System.Drawing.Size(85, 17);
            this.chkIgnoreCase.TabIndex = 1;
            this.chkIgnoreCase.Text = "Ignore Case";
            this.chkIgnoreCase.UseVisualStyleBackColor = true;
            // 
            // RegexBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkIgnoreCase);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnEtc);
            this.Controls.Add(this.btnGrouping);
            this.Controls.Add(this.txtRegex);
            this.Controls.Add(this.btnQuantifiers);
            this.Font = new System.Drawing.Font("Tahoma", 8F);
            this.Name = "RegexBuilder";
            this.Size = new System.Drawing.Size(258, 116);
            this.menuCounter.ResumeLayout(false);
            this.menuGroup.ResumeLayout(false);
            this.menuEtc.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtRegex;
        private System.Windows.Forms.ContextMenuStrip menuCounter;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ContextMenuStrip menuGroup;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem8;
        private System.Windows.Forms.ContextMenuStrip menuEtc;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem9;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem10;
        private System.Windows.Forms.Button btnQuantifiers;
        private System.Windows.Forms.Button btnGrouping;
        private System.Windows.Forms.Button btnEtc;
        private System.Windows.Forms.ToolStripMenuItem vToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wMatchesANonwordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sMatchesAWhiteSpaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sMatchesANonwhiteSpaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dMatchesANumberToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dMatchesANonnumberToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkIgnoreCase;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem12;
        private System.Windows.Forms.ToolStripMenuItem matchesEndOfTheStringOrLineToolStripMenuItem;
    }
}
