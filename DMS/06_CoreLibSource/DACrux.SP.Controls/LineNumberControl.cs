using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace DACrux.SP.Controls
{
   
    [System.ComponentModel.DefaultProperty("ParentRichTextBox")]
    public class LineNumbersPanel : System.Windows.Forms.Control
    {
        #region " Member Field & Property "

        private int iLineOffset = 0;
        public int LineOffset
        {
            get { return iLineOffset; }
            set { iLineOffset = value; }
        }

        private RichTextBox rtbParentWithEventsField = null;

        private RichTextBox rtbParent
        {
            get { return rtbParentWithEventsField; }
            set
            {
                if (rtbParentWithEventsField != null)
                {
                    rtbParentWithEventsField.LocationChanged -= zParent_Changed;
                    rtbParentWithEventsField.Move -= zParent_Changed;
                    rtbParentWithEventsField.Resize -= zParent_Changed;
                    rtbParentWithEventsField.DockChanged -= zParent_Changed;
                    rtbParentWithEventsField.TextChanged -= zParent_Changed;
                    rtbParentWithEventsField.MultilineChanged -= zParent_Changed;
                    rtbParentWithEventsField.HScroll -= zParent_Scroll;
                    rtbParentWithEventsField.VScroll -= zParent_Scroll;
                    rtbParentWithEventsField.ContentsResized -= zParent_ContentsResized;
                    rtbParentWithEventsField.Disposed -= zParent_Disposed;
                }
                rtbParentWithEventsField = value;
                if (rtbParentWithEventsField != null)
                {
                    rtbParentWithEventsField.LocationChanged += zParent_Changed;
                    rtbParentWithEventsField.Move += zParent_Changed;
                    rtbParentWithEventsField.Resize += zParent_Changed;
                    rtbParentWithEventsField.DockChanged += zParent_Changed;
                    rtbParentWithEventsField.TextChanged += zParent_Changed;
                    rtbParentWithEventsField.MultilineChanged += zParent_Changed;
                    rtbParentWithEventsField.HScroll += zParent_Scroll;
                    rtbParentWithEventsField.VScroll += zParent_Scroll;
                    rtbParentWithEventsField.ContentsResized += zParent_ContentsResized;
                    rtbParentWithEventsField.Disposed += zParent_Disposed;
                }
            }
        }
        private Timer tmrWithEventsField = new Timer();
        private Timer timer
        {
            get { return tmrWithEventsField; }
            set
            {
                if (tmrWithEventsField != null)
                {
                    tmrWithEventsField.Tick -= zTimer_Tick;
                }
                tmrWithEventsField = value;
                if (tmrWithEventsField != null)
                {
                    tmrWithEventsField.Tick += zTimer_Tick;
                }
            }

        }
        private bool bAutoSizing = true;
        private Size szAutoSizing = new Size(0, 0);
        private Rectangle recContent = new Rectangle();
        private LineNumberDockSide dockSide = LineNumberDockSide.Left;
        private bool bParentIsScrolling = false;

        private bool bSeeThroughMode = false;
        private bool bGradient_Show = true;
        private LinearGradientMode gradientDirection = LinearGradientMode.Horizontal;
        private Color colorGradientStart = Color.FromArgb(0, 0, 0, 0);

        private Color colorGradientEnd = Color.LightSteelBlue;
        private bool bGridLinesShow = true;
        private float fGridLinesThickness = 1;
        private DashStyle dashStyleGridLines = DashStyle.Dot;

        private Color colorGridLines = Color.SlateGray;
        private bool bBorderLines_Show = true;
        private float fBorderLinesThickness = 1;
        private DashStyle dashStyleBorderLines = DashStyle.Dot;

        private Color colorBorderLines = Color.SlateGray;
        private bool bMarginLinesShow = true;
        private LineNumberDockSide dockSideMarginLines = LineNumberDockSide.Right;
        private float fMarginLinesThickness = 1;
        private DashStyle dashStyleMarginLines = DashStyle.Solid;

        private Color colorMarginLines = Color.SlateGray;
        private bool bLineNumbersShow = true;
        private bool bLineNumbersShowLeadingZeroes = true;
        private bool bLineNumbersShowAsHexadecimal = false;
        private bool bLineNumbersClipByItemRectangle = true;
        private Size szLineNumbers_Offset = new Size(0, 0);
        private string strLineNumbersFormat = "0";
        private ContentAlignment lineNumbersAlignment = ContentAlignment.TopRight;

        private bool bLineNumbersAntiAlias = true;

        private List<LineNumberItem> lstLineNumberItem = new List<LineNumberItem>();
        private Point ptInParent = new Point(0, 0);
        private Point ptInMe = new Point(0, 0);
        private int iParentInMe = 0;

        #endregion

        #region " Design-Mode Property "

        [System.ComponentModel.Browsable(false)]
        public override bool AutoSize
        {
            get { return base.AutoSize; }
            set
            {
                base.AutoSize = value;
                this.Invalidate();
            }
        }

        [System.ComponentModel.Description("Use this property to automatically resize the control (and reposition it if needed).")]
        [System.ComponentModel.Category("Additional Behavior")]
        public bool AutoSizing
        {
            get { return bAutoSizing; }
            set
            {
                bAutoSizing = value;
                this.Refresh();
                this.Invalidate();
            }
        }

        [System.ComponentModel.Description("Use this property to enable LineNumbers for the chosen RichTextBox.")]
        [System.ComponentModel.Category("Add LineNumbers to")]
        public RichTextBox ParentRichTextBox
        {
            get { return rtbParent; }
            set
            {
                rtbParent = value;
                if (rtbParent != null)
                {
                    this.Parent = rtbParent.Parent;
                    rtbParent.Refresh();
                }
                this.Text = "";
                this.Refresh();
                this.Invalidate();
            }
        }

        [System.ComponentModel.Description("Use this property to dock the LineNumbers to a chosen side of the chosen RichTextBox.")]
        [System.ComponentModel.Category("Additional Behavior")]
        public LineNumberDockSide DockSide
        {
            get { return dockSide; }
            set
            {
                dockSide = value;
                this.Refresh();
                this.Invalidate();
            }
        }

        [System.ComponentModel.Description("Use this property to enable the control to act as an overlay ontop of the RichTextBox.")]
        [System.ComponentModel.Category("Additional Behavior")]
        public bool SeeThroughMode
        {
            get { return bSeeThroughMode; }
            set
            {
                bSeeThroughMode = value;
                this.Invalidate();
            }
        }

        [System.ComponentModel.Description("BorderLines are shown on all sides of the LineNumber control.")]
        [System.ComponentModel.Category("Additional Behavior")]
        public bool ShowBorderLines
        {
            get { return bBorderLines_Show; }
            set
            {
                bBorderLines_Show = value;
                this.Invalidate();
            }
        }

        [System.ComponentModel.Category("Additional Appearance")]
        public Color BorderLinesColor
        {
            get { return colorBorderLines; }
            set
            {
                colorBorderLines = value;
                this.Invalidate();
            }
        }

        [System.ComponentModel.Category("Additional Appearance")]
        public float BorderLinesThickness
        {
            get { return fBorderLinesThickness; }
            set
            {
                fBorderLinesThickness = Math.Max(1, Math.Min(255, value));
                this.Invalidate();
            }
        }

        [System.ComponentModel.Category("Additional Appearance")]
        public DashStyle BorderLinesStyle
        {
            get { return dashStyleBorderLines; }
            set
            {
                if (value == DashStyle.Custom)
                    value = DashStyle.Solid;
                dashStyleBorderLines = value;
                this.Invalidate();
            }
        }

        [System.ComponentModel.Description("GridLines are the horizontal divider-lines shown above each LineNumber.")]
        [System.ComponentModel.Category("Additional Behavior")]
        public bool ShowGridLines
        {
            get { return bGridLinesShow; }
            set
            {
                bGridLinesShow = value;
                this.Invalidate();
            }
        }

        [System.ComponentModel.Category("Additional Appearance")]
        public Color GridLinesColor
        {
            get { return colorGridLines; }
            set
            {
                colorGridLines = value;
                this.Invalidate();
            }
        }

        [System.ComponentModel.Category("Additional Appearance")]
        public float GridLinesThickness
        {
            get { return fGridLinesThickness; }
            set
            {
                fGridLinesThickness = Math.Max(1, Math.Min(255, value));
                this.Invalidate();
            }
        }

        [System.ComponentModel.Category("Additional Appearance")]
        public DashStyle GridLinesStyle
        {
            get { return dashStyleGridLines; }
            set
            {
                if (value == DashStyle.Custom)
                    value = DashStyle.Solid;
                dashStyleGridLines = value;
                this.Invalidate();
            }
        }

        [System.ComponentModel.Description("MarginLines are shown on the Left or Right (or both in Height-mode) of the LineNumber control.")]
        [System.ComponentModel.Category("Additional Behavior")]
        public bool ShowMarginLines
        {
            get { return bMarginLinesShow; }
            set
            {
                bMarginLinesShow = value;
                this.Invalidate();
            }
        }

        [System.ComponentModel.Category("Additional Appearance")]
        public LineNumberDockSide MarginLinesSide
        {
            get { return dockSideMarginLines; }
            set
            {
                dockSideMarginLines = value;
                this.Invalidate();
            }
        }

        [System.ComponentModel.Category("Additional Appearance")]
        public Color MarginLinesColor
        {
            get { return colorMarginLines; }
            set
            {
                colorMarginLines = value;
                this.Invalidate();
            }
        }

        [System.ComponentModel.Category("Additional Appearance")]
        public float MarginLinesThickness
        {
            get { return fMarginLinesThickness; }
            set
            {
                fMarginLinesThickness = Math.Max(1, Math.Min(255, value));
                this.Invalidate();
            }
        }

        [System.ComponentModel.Category("Additional Appearance")]
        public DashStyle MarginLinesStyle
        {
            get { return dashStyleMarginLines; }
            set
            {
                if (value == DashStyle.Custom)
                    value = DashStyle.Solid;
                dashStyleMarginLines = value;
                this.Invalidate();
            }
        }

        [System.ComponentModel.Description("The BackgroundGradient is a gradual blend of two colors, shown in the back of each LineNumber's item-area.")]
        [System.ComponentModel.Category("Additional Behavior")]
        public bool ShowBackgroundGradient
        {
            get { return bGradient_Show; }
            set
            {
                bGradient_Show = value;
                this.Invalidate();
            }
        }

        [System.ComponentModel.Category("Additional Appearance")]
        public Color BackgroundGradientAlphaColor
        {
            get { return colorGradientStart; }
            set
            {
                colorGradientStart = value;
                this.Invalidate();
            }
        }

        [System.ComponentModel.Category("Additional Appearance")]
        public Color BackgroundGradientBetaColor
        {
            get { return colorGradientEnd; }
            set
            {
                colorGradientEnd = value;
                this.Invalidate();
            }
        }

        [System.ComponentModel.Category("Additional Appearance")]
        public LinearGradientMode BackgroundGradientDirection
        {
            get { return gradientDirection; }
            set
            {
                gradientDirection = value;
                this.Invalidate();
            }
        }

        [System.ComponentModel.Category("Additional Behavior")]
        public bool ShowLineNumbers
        {
            get { return bLineNumbersShow; }
            set
            {
                bLineNumbersShow = value;
                this.Invalidate();
            }
        }

        [System.ComponentModel.Description("Use this to set whether the LineNumbers are allowed to spill out of their item-area, or should be clipped by it.")]
        [System.ComponentModel.Category("Additional Behavior")]
        public bool LineNumbersClippedByItemRectangle
        {
            get { return bLineNumbersClipByItemRectangle; }
            set
            {
                bLineNumbersClipByItemRectangle = value;
                this.Invalidate();
            }
        }

        [System.ComponentModel.Description("Use this to set whether the LineNumbers should have leading zeroes (based on the total amount of textlines).")]
        [System.ComponentModel.Category("Additional Behavior")]
        public bool LineNumbersLeadingZeroes
        {
            get { return bLineNumbersShowLeadingZeroes; }
            set
            {
                bLineNumbersShowLeadingZeroes = value;
                this.Refresh();
                this.Invalidate();
            }
        }

        [System.ComponentModel.Description("Use this to set whether the LineNumbers should be shown as hexadecimal values.")]
        [System.ComponentModel.Category("Additional Behavior")]
        public bool LineNumbersAsHexadecimal
        {
            get { return bLineNumbersShowAsHexadecimal; }
            set
            {
                bLineNumbersShowAsHexadecimal = value;
                this.Refresh();
                this.Invalidate();
            }
        }

        [System.ComponentModel.Description("Use this property to manually reposition the LineNumbers, relative to their current location.")]
        [System.ComponentModel.Category("Additional Behavior")]
        public Size LineNumbersOffset
        {
            get { return szLineNumbers_Offset; }
            set
            {
                szLineNumbers_Offset = value;
                this.Invalidate();
            }
        }

        [System.ComponentModel.Description("Use this to align the LineNumbers to a chosen corner (or center) within their item-area.")]
        [System.ComponentModel.Category("Additional Behavior")]
        public System.Drawing.ContentAlignment LineNumbersAlignment
        {
            get { return lineNumbersAlignment; }
            set
            {
                lineNumbersAlignment = value;
                this.Invalidate();
            }
        }

        [System.ComponentModel.Description("Use this to apply Anti-Aliasing to the LineNumbers (high quality). Some fonts will look better without it, though.")]
        [System.ComponentModel.Category("Additional Behavior")]
        public bool LineNumbersAntiAlias
        {
            get { return bLineNumbersAntiAlias; }
            set
            {
                bLineNumbersAntiAlias = value;
                this.Refresh();
                this.Invalidate();
            }
        }

        [System.ComponentModel.Browsable(true)]
        public override System.Drawing.Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                this.Refresh();
                this.Invalidate();
            }
        }

        [System.ComponentModel.DefaultValue("")]
        [System.ComponentModel.AmbientValue("")]
        [System.ComponentModel.Browsable(false)]
        public override string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = "";
                this.Invalidate();
            }
        }

        #endregion

        #region " Inner Class "

        private class LineNumberItem
        {
            internal int LineNumber;
            internal Rectangle Rectangle;
            internal LineNumberItem(int iLineNumber, Rectangle recRectangle)
            {
                this.LineNumber = iLineNumber;
                this.Rectangle = recRectangle;
            }
        }

        #endregion

        #region " Enum "

        public enum LineNumberDockSide : byte
        {
            None = 0,
            Left = 1,
            Right = 2,
            Height = 4
        }

        #endregion
        
        #region " Creator "

        public LineNumbersPanel()
        {
            {
                this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
                this.SetStyle(ControlStyles.ResizeRedraw, true);
                this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                this.SetStyle(ControlStyles.UserPaint, true);
                this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                this.Margin = new Padding(0);
                this.Padding = new Padding(0, 0, 2, 0);
            }
            {
                timer.Enabled = true;
                timer.Interval = 200;
                timer.Stop();
            }
            this.Update_SizeAndPosition();
            this.Invalidate();
        }

        #endregion

        #region " Method "
        /// <summary>
        /// This Sub will run whenever Me.Refresh() is called. It applies the AutoSizing and DockSide settings.
        /// </summary>
        /// <remarks></remarks>
        private void Update_SizeAndPosition()
        {
            if (this.AutoSize == true)
                return;
            if (this.Dock == DockStyle.Bottom | this.Dock == DockStyle.Fill | this.Dock == DockStyle.Top)
                return;
            Point ptNewLocation = this.Location;
            Size szNewSize = this.Size;

            if (bAutoSizing == true)
            {
                if (rtbParent == null)
                {
                    // --- ReminderMessage sizing
                    if (szAutoSizing.Width > 0)
                        szNewSize.Width = szAutoSizing.Width;
                    if (szAutoSizing.Height > 0)
                        szNewSize.Height = szAutoSizing.Height;
                    this.Size = szNewSize;
                }
                //--- zParent isNot Nothing for the following cases
                else if (this.Dock == DockStyle.Left | this.Dock == DockStyle.Right)
                {
                    if (szAutoSizing.Width > 0)
                        szNewSize.Width = szAutoSizing.Width;
                    this.Width = szNewSize.Width;

                }
                // --- DockSide is active L/R/H
                else if (dockSide != LineNumberDockSide.None)
                {
                    if (szAutoSizing.Width > 0)
                        szNewSize.Width = szAutoSizing.Width;
                    szNewSize.Height = rtbParent.Height;
                    if (dockSide == LineNumberDockSide.Left)
                        ptNewLocation.X = rtbParent.Left - szNewSize.Width - 1;
                    if (dockSide == LineNumberDockSide.Right)
                        ptNewLocation.X = rtbParent.Right + 1;
                    ptNewLocation.Y = rtbParent.Top;
                    this.Location = ptNewLocation;
                    this.Size = szNewSize;
                }
                // --- DockSide = None, but AutoSizing is still setting the Width
                else if (dockSide == LineNumberDockSide.None)
                {
                    if (szAutoSizing.Width > 0)
                        szNewSize.Width = szAutoSizing.Width;
                    this.Size = szNewSize;

                }

            }
            else
            {
                // --- No AutoSizing
                if (rtbParent == null)
                {
                    // --- ReminderMessage sizing
                    if (szAutoSizing.Width > 0)
                        szNewSize.Width = szAutoSizing.Width;
                    if (szAutoSizing.Height > 0)
                        szNewSize.Height = szAutoSizing.Height;
                    this.Size = szNewSize;
                }
                // --- No AutoSizing, but DockSide L/R/H is active so height and position need updates.
                else if (dockSide != LineNumberDockSide.None)
                {
                    szNewSize.Height = rtbParent.Height;
                    if (dockSide == LineNumberDockSide.Left)
                        ptNewLocation.X = rtbParent.Left - szNewSize.Width - 1;
                    if (dockSide == LineNumberDockSide.Right)
                        ptNewLocation.X = rtbParent.Right + 1;
                    ptNewLocation.Y = rtbParent.Top;
                    this.Location = ptNewLocation;
                    this.Size = szNewSize;
                }
            }
        }

        /// <summary>
        /// This Sub determines which textlines are visible in the ParentRichTextBox, and makes LineNumberItems (LineNumber + ItemRectangle)
        /// for each visible line. They are put into the zLNIs List that will be used by the OnPaint event to draw the LineNumberItems. 
        /// </summary>
        /// <remarks></remarks>
        private void Update_VisibleLineNumberItems()
        {
            lstLineNumberItem.Clear();
            szAutoSizing = new Size(0, 0);
            strLineNumbersFormat = "0";
            //initial setting
            //   To measure the LineNumber's width, its Format 0 is replaced by w as that is likely to be one of the widest characters in non-monospace fonts. 
            if (bAutoSizing == true)
                szAutoSizing = new Size(TextRenderer.MeasureText(strLineNumbersFormat.Replace("0", "W"), this.Font).Width, 0);

            if (rtbParent == null || string.IsNullOrEmpty(rtbParent.Text))
                return;

            // --- Make sure the LineNumbers are aligning to the same height as the zParent textlines by converting to screencoordinates
            //   and using that as an offset that gets added to the points for the LineNumberItems
            ptInParent = rtbParent.PointToScreen(rtbParent.ClientRectangle.Location);
            ptInMe = this.PointToScreen(new Point(0, 0));
            //   zParentInMe is the vertical offset to make the LineNumberItems line up with the textlines in zParent.
            iParentInMe = ptInParent.Y - ptInMe.Y + 1;
            //   The first visible LineNumber may not be the first visible line of text in the RTB if the LineNumbercontrol's .Top is lower on the form than
            //   the .Top of the parent RichTextBox. Therefor, ptPointInParent will now be used to find ptPointInMe's equivalent height in zParent, 
            //   which is needed to find the best StartIndex later on.
            ptInParent = rtbParent.PointToClient(ptInMe);

            // --- NOTES: 
            //   Additional complication is the fact that when wordwrap is enabled on the RTB, the wordwrapped text spills into the RTB.Lines collection, 
            //   so we need to split the text into lines ourselves, and use the Index of each zSplit-line's first character instead of the RTB's.
            //string[] zSplit = rtbParent.Text.Split(Environment.NewLine.ToCharArray());
            string[] zSplit = rtbParent.Text.Split("\n".ToCharArray());

            if (zSplit.Length < 2)
            {
                //   Just one line in the text = one linenumber
                //   NOTE:  zContentRectangle is built by the zParent.ContentsResized event.
                Point ptPoint = rtbParent.GetPositionFromCharIndex(0);
                lstLineNumberItem.Add(new LineNumberItem(1 + iLineOffset, new Rectangle(new Point(0, ptPoint.Y - 1 + iParentInMe), new Size(this.Width, recContent.Height - ptPoint.Y))));


            }
            else
            {
                //   Multiple lines, but store only those LineNumberItems for lines that are visible.
                TimeSpan zTimeSpan = new TimeSpan(DateTime.Now.Ticks);
                Point ptPoint = new Point(0, 0);
                int iStartIndex = 0;
                int iSplitStartLine = 0;
                int iParentTextLength = rtbParent.Text.Length - 1;

                int iTarget = ptInParent.Y;
                this.FindStartIndex(ref iStartIndex, ref iParentTextLength, ref iTarget);
                ptInParent.Y = iTarget;

                //   iStartIndex now holds the index of a character in the first visible line from zParent.Text
                //   Now it will be pointed at the first character of that line (chr(10) = Linefeed part of the vbCrLf constant)
                iStartIndex = Math.Max(0, Math.Min(rtbParent.Text.Length - 1, rtbParent.Text.Substring(0, iStartIndex).LastIndexOf((char)10) + 1));

                //   We now need to find out which zSplit-line that character is in, by counting the vbCrlf appearances that come before it.
                iSplitStartLine = Math.Max(0, rtbParent.Text.Substring(0, iStartIndex).Split("\n".ToCharArray()).Length - 1);
                //iSplitStartLine = Math.Max(0, rtbParent.Text.Substring(0, iStartIndex).Split(Environment.NewLine.ToCharArray()).Length - 1);

                //   iStartIndex starts off pointing at the first character of the first visible line, and will be then be pointed to 
                //   the index of the first character on the next line.
                for (iParentTextLength = iSplitStartLine; iParentTextLength <= zSplit.Length - 1; iParentTextLength++)
                {
                    ptPoint = rtbParent.GetPositionFromCharIndex(iStartIndex);
                    iStartIndex += Math.Max(1, zSplit[iParentTextLength].Length + 1);
                    if (ptPoint.Y + iParentInMe > this.Height)
                        break; // TODO: might not be correct. Was : Exit For
                    //   For performance reasons, the list of LineNumberItems (zLNIs) is first built with only the location of its 
                    //   itemrectangle being used. The height of those rectangles will be computed afterwards by comparing the items' Y coordinates.
                    lstLineNumberItem.Add(new LineNumberItem(iParentTextLength + 1 + iLineOffset, new Rectangle(0, ptPoint.Y - 1 + iParentInMe, this.Width, 1)));
                    if (bParentIsScrolling == true && DateTime.Now.Ticks > zTimeSpan.Ticks + 500000)
                    {
                        //   The more lines there are in the RTB, the slower the RTB's .GetPositionFromCharIndex() method becomes
                        //   To avoid those delays from interfering with the scrollingspeed, this speedbased exit for is applied (0.05 sec)
                        //   zLNIs will have at least 1 item, and if that's the only one, then change its location to 0,0 to make it readable
                        if (lstLineNumberItem.Count == 1)
                            lstLineNumberItem[0].Rectangle.Y = 0;
                        bParentIsScrolling = false;
                        timer.Start();
                        break; // TODO: might not be correct. Was : Exit For
                    }
                }

                if (lstLineNumberItem.Count == 0)
                    return;

                //   Add an extra placeholder item to the end, to make the heightcomputation easier
                if (iParentTextLength < zSplit.Length)
                {
                    //   getting here means the for/next loop was exited before reaching the last zSplit textline
                    //   iStartIndex will still be pointing to the startcharacter of the next line, so we can use that:
                    ptPoint = rtbParent.GetPositionFromCharIndex(Math.Min(iStartIndex, rtbParent.Text.Length - 1));
                    lstLineNumberItem.Add(new LineNumberItem(-1, new Rectangle(0, ptPoint.Y - 1 + iParentInMe, 0, 0)));
                }
                else
                {
                    //   getting here means the for/next loop ran to the end (zA is now zSplit.Length). 
                    lstLineNumberItem.Add(new LineNumberItem(-1, new Rectangle(0, recContent.Bottom, 0, 0)));
                }

                //   And now we can easily compute the height of the LineNumberItems by comparing each item's Y coordinate with that of the next line.
                //   There's at least two items in the list, and the last item is a "nextline-placeholder" that will be removed.
                for (iParentTextLength = 0; iParentTextLength <= lstLineNumberItem.Count - 2; iParentTextLength++)
                {
                    lstLineNumberItem[iParentTextLength].Rectangle.Height = Math.Max(1, lstLineNumberItem[iParentTextLength + 1].Rectangle.Y - lstLineNumberItem[iParentTextLength].Rectangle.Y);
                }
                //   Removing the placeholder item
                lstLineNumberItem.RemoveAt(lstLineNumberItem.Count - 1);

                // Set the Format to the width of the highest possible number so that LeadingZeroes shows the correct amount of zeroes.
                if (bLineNumbersShowAsHexadecimal == true)
                {
                    strLineNumbersFormat = "".PadRight(zSplit.Length.ToString("X").Length, '0');
                }
                else
                {
                    strLineNumbersFormat = "".PadRight(zSplit.Length.ToString().Length, '0');
                }
            }

            //   To measure the LineNumber's width, its Format 0 is replaced by w as that is likely to be one of the widest characters in non-monospace fonts. 
            if (bAutoSizing == true)
                szAutoSizing = new Size(TextRenderer.MeasureText(strLineNumbersFormat.Replace("0", "W"), this.Font).Width, 0);
        }

        /// <summary>
        /// FindStartIndex is a recursive Sub (one that calls itself) to compute the first visible line that should have a LineNumber.
        /// </summary>
        /// <param name="zMin"> this will hold the eventual BestStartIndex when the Sub has completed its run.</param>
        /// <param name="zMax"></param>
        /// <param name="zTarget"></param>
        /// <remarks></remarks>
        private void FindStartIndex(ref int zMin, ref int zMax, ref int zTarget)
        {
            //   Recursive Sub to compute best starting index - only run when zParent is known to exist
            if (zMax == zMin + 1 | zMin == (zMax + zMin) / 2)
                return;

            int iTemp = rtbParent.GetPositionFromCharIndex((zMax + zMin) / 2).Y;

            if (iTemp == zTarget)
                zMin = (zMax + zMin) / 2;
            else if (iTemp > zTarget)
            {
                zMax = (zMax + zMin) / 2;
                FindStartIndex(ref zMin, ref zMax, ref zTarget);
            }
            else if (iTemp < 0)
            {
                zMin = (zMax + zMin) / 2;
                FindStartIndex(ref zMin, ref zMax, ref zTarget);
            }
        }

        #endregion

        #region " Event Handler "

        private void zTimer_Tick(object sender, System.EventArgs e)
        {
            bParentIsScrolling = false;
            timer.Stop();
            this.Invalidate();
        }

        private void zParent_Changed(object sender, System.EventArgs e)
        {
            this.Refresh();
            this.Invalidate();
        }

        private void zParent_Scroll(object sender, System.EventArgs e)
        {
            bParentIsScrolling = true;
            this.Invalidate();
        }

        private void zParent_ContentsResized(object sender, System.Windows.Forms.ContentsResizedEventArgs e)
        {
            recContent = e.NewRectangle;
            this.Refresh();
            this.Invalidate();
        }

        private void zParent_Disposed(object sender, System.EventArgs e)
        {
            this.ParentRichTextBox = null;
            this.Refresh();
            this.Invalidate();
        }

        #endregion

        #region " Overrided Handler Method "

        protected override void OnHandleCreated(System.EventArgs e)
        {
            base.OnHandleCreated(e);
            this.AutoSize = false;
        }

        protected override void OnSizeChanged(System.EventArgs e)
        {
            if (this.DesignMode == true)
                this.Refresh();
            base.OnSizeChanged(e);
            this.Invalidate();
        }

        protected override void OnLocationChanged(System.EventArgs e)
        {
            if (this.DesignMode == true)
                this.Refresh();
            base.OnLocationChanged(e);
            this.Invalidate();
        }

        public override void Refresh()
        {
            //   Note: don't change the order here, first the Mybase.Refresh, then the Update_SizeAndPosition.
            base.Refresh();
            this.Update_SizeAndPosition();
        }

        /// <summary>
        /// OnPaint will go through the enabled elements (vertical ReminderMessage, GridLines, LineNumbers, BorderLines, MarginLines) and will
        /// draw them if enabled. At the same time, it will build GraphicsPaths for each of those elements (that are enabled), which will be used 
        /// in SeeThroughMode (if it's active): the figures in the GraphicsPaths will form a customized outline for the control by setting them as the 
        /// Region of the LineNumber control. Note: the vertical ReminderMessages are only drawn during designtime. 
        /// </summary>
        /// <param name="e"></param>
        /// <remarks></remarks>
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            //   Build the list of visible LineNumberItems (= zLNIs) first. (doesn't take long, so it can stay in OnPaint)
            this.Update_VisibleLineNumberItems();
            base.OnPaint(e);

            // --- QualitySettings
            if (bLineNumbersAntiAlias == true)
            {
                e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            }
            else
            {
                e.Graphics.TextRenderingHint = TextRenderingHint.SystemDefault;
            }

            // --- Local Declarations
            string zTextToShow = "";
            string zReminderToShow = "";
            StringFormat zSF = new StringFormat();
            SizeF zTextSize = default(SizeF);
            Pen zPen = new Pen(this.ForeColor);
            SolidBrush zBrush = new SolidBrush(this.ForeColor);
            Point ptPoint = new Point(0, 0);
            Rectangle zItemClipRectangle = new Rectangle(0, 0, 0, 0);

            //   NOTE: The GraphicsPaths are only used for SeeThroughMode
            //   FillMode.Winding: combined outline ( Alternate: XOR'ed outline )
            GraphicsPath zGP_GridLines = new GraphicsPath(FillMode.Winding);
            GraphicsPath zGP_BorderLines = new GraphicsPath(FillMode.Winding);
            GraphicsPath zGP_MarginLines = new GraphicsPath(FillMode.Winding);
            GraphicsPath zGP_LineNumbers = new GraphicsPath(FillMode.Winding);
            Region zRegion = new Region(base.ClientRectangle);


            // ----------------------------------------------
            // --- DESIGNTIME / NO VISIBLE ITEMS
            if (this.DesignMode == true)
            {
                //   Show a vertical reminder message
                if (rtbParent == null)
                {
                    zReminderToShow = "-!- Set ParentRichTextBox -!-";
                }
                else
                {
                    if (lstLineNumberItem.Count == 0)
                        zReminderToShow = "LineNumbers (  " + rtbParent.Name + "  )";
                }
                if (zReminderToShow.Length > 0)
                {
                    // --- Centering and Rotation for the reminder message
                    e.Graphics.TranslateTransform(this.Width / 2, this.Height / 2);
                    e.Graphics.RotateTransform(-90);
                    zSF.Alignment = StringAlignment.Center;
                    zSF.LineAlignment = StringAlignment.Center;
                    // --- Show the reminder message (with small shadow)
                    zTextSize = e.Graphics.MeasureString(zReminderToShow, this.Font, ptPoint, zSF);
                    e.Graphics.DrawString(zReminderToShow, this.Font, Brushes.WhiteSmoke, 1, 1, zSF);
                    e.Graphics.DrawString(zReminderToShow, this.Font, Brushes.Firebrick, 0, 0, zSF);
                    e.Graphics.ResetTransform();

                    Rectangle zReminderRectangle = new Rectangle((int)(this.Width / 2 - zTextSize.Height / 2), (int)(this.Height / 2 - (int)(zTextSize.Width / 2)), (int)zTextSize.Height, (int)zTextSize.Width);
                    zGP_LineNumbers.AddRectangle(zReminderRectangle);
                    zGP_LineNumbers.CloseFigure();

                    if (bAutoSizing == true)
                    {
                        zReminderRectangle.Inflate((int)(zTextSize.Height * 0.2), (int)(zTextSize.Width * 0.1));
                        szAutoSizing = new Size(zReminderRectangle.Width, zReminderRectangle.Height);
                    }
                }
            }


            // ----------------------------------------------
            // --- DESIGN OR RUNTIME / WITH VISIBLE ITEMS (which means zParent exists)
            if (lstLineNumberItem.Count > 0)
            {
                //   The visible LineNumberItems with their BackgroundGradient and GridLines
                //   Loop through every visible LineNumberItem
                LinearGradientBrush zLGB = null;
                zPen = new Pen(colorGridLines, fGridLinesThickness);
                zPen.DashStyle = dashStyleGridLines;
                zSF.Alignment = StringAlignment.Near;
                zSF.LineAlignment = StringAlignment.Near;
                zSF.FormatFlags = StringFormatFlags.FitBlackBox | StringFormatFlags.NoClip | StringFormatFlags.NoWrap;


                for (int zA = 0; zA <= lstLineNumberItem.Count - 1; zA++)
                {
                    // --- BackgroundGradient
                    if (bGradient_Show == true)
                    {
                        zLGB = new LinearGradientBrush(lstLineNumberItem[zA].Rectangle, colorGradientStart, colorGradientEnd, gradientDirection);
                        e.Graphics.FillRectangle(zLGB, lstLineNumberItem[zA].Rectangle);
                    }

                    // --- GridLines
                    if (bGridLinesShow == true)
                    {
                        e.Graphics.DrawLine(zPen, new Point(0, lstLineNumberItem[zA].Rectangle.Y), new Point(this.Width, lstLineNumberItem[zA].Rectangle.Y));

                        //   NOTE: Every item in a GraphicsPath is a closed figure, so instead of adding gridlines as lines, we'll add them
                        //   as rectangles that loop out of sight. Their height uses the zContentRectangle which is the maxsize of 
                        //   the ParentRichTextBox's contents. 
                        //   NOTE: Slight adjustment needed when the first item has a negative Y coordinate. 
                        //   This explains the " - zLNIs(0).Rectangle.Y" (which adds the negative size to the height 
                        //   to make sure the rectangle's bottompart stays out of sight) 
                        zGP_GridLines.AddRectangle(new Rectangle((int)(-fGridLinesThickness), (int)(lstLineNumberItem[zA].Rectangle.Y), (int)(this.Width + fGridLinesThickness * 2), (int)(this.Height - lstLineNumberItem[0].Rectangle.Y + fGridLinesThickness)));
                        zGP_GridLines.CloseFigure();
                    }

                    // --- LineNumbers
                    if (bLineNumbersShow == true)
                    {
                        //   TextFormatting
                        if (bLineNumbersShowLeadingZeroes == true)
                        {
                            zTextToShow = (bLineNumbersShowAsHexadecimal ? lstLineNumberItem[zA].LineNumber.ToString("X") : lstLineNumberItem[zA].LineNumber.ToString(strLineNumbersFormat));
                        }
                        else
                        {
                            zTextToShow = (bLineNumbersShowAsHexadecimal ? lstLineNumberItem[zA].LineNumber.ToString("X") : lstLineNumberItem[zA].LineNumber.ToString());
                        }
                        //   TextSizing
                        zTextSize = e.Graphics.MeasureString(zTextToShow, this.Font, ptPoint, zSF);
                        //   TextAlignment and positioning   (ptPoint = TopLeftCornerPoint of the text)
                        //   TextAlignment, padding, manual offset (via LineNumbers_Offset) and zTextSize are all included in the calculation of ptPoint. 
                        switch (lineNumbersAlignment)
                        {
                            case ContentAlignment.TopLeft:
                                ptPoint = new Point(lstLineNumberItem[zA].Rectangle.Left + this.Padding.Left + szLineNumbers_Offset.Width, lstLineNumberItem[zA].Rectangle.Top + this.Padding.Top + szLineNumbers_Offset.Height);
                                break;
                            case ContentAlignment.MiddleLeft:
                                ptPoint = new Point(lstLineNumberItem[zA].Rectangle.Left + this.Padding.Left + szLineNumbers_Offset.Width, lstLineNumberItem[zA].Rectangle.Top + (lstLineNumberItem[zA].Rectangle.Height / 2) + szLineNumbers_Offset.Height - (int)(zTextSize.Height / 2));
                                break;
                            case ContentAlignment.BottomLeft:
                                ptPoint = new Point(lstLineNumberItem[zA].Rectangle.Left + this.Padding.Left + szLineNumbers_Offset.Width, lstLineNumberItem[zA].Rectangle.Bottom - this.Padding.Bottom + 1 + szLineNumbers_Offset.Height - (int)zTextSize.Height);
                                break;
                            case ContentAlignment.TopCenter:
                                ptPoint = new Point(lstLineNumberItem[zA].Rectangle.Width / 2 + szLineNumbers_Offset.Width - (int)((int)(zTextSize.Width / 2)), lstLineNumberItem[zA].Rectangle.Top + this.Padding.Top + szLineNumbers_Offset.Height);
                                break;
                            case ContentAlignment.MiddleCenter:
                                ptPoint = new Point(lstLineNumberItem[zA].Rectangle.Width / 2 + szLineNumbers_Offset.Width - (int)(zTextSize.Width / 2), lstLineNumberItem[zA].Rectangle.Top + (lstLineNumberItem[zA].Rectangle.Height / 2) + szLineNumbers_Offset.Height - (int)(zTextSize.Height / 2));
                                break;
                            case ContentAlignment.BottomCenter:
                                ptPoint = new Point(lstLineNumberItem[zA].Rectangle.Width / 2 + szLineNumbers_Offset.Width - (int)(zTextSize.Width / 2), lstLineNumberItem[zA].Rectangle.Bottom - this.Padding.Bottom + 1 + szLineNumbers_Offset.Height - (int)zTextSize.Height);
                                break;
                            case ContentAlignment.TopRight:
                                ptPoint = new Point(lstLineNumberItem[zA].Rectangle.Right - this.Padding.Right + szLineNumbers_Offset.Width - (int)(zTextSize.Width), lstLineNumberItem[zA].Rectangle.Top + this.Padding.Top + szLineNumbers_Offset.Height);
                                break;
                            case ContentAlignment.MiddleRight:
                                ptPoint = new Point(lstLineNumberItem[zA].Rectangle.Right - this.Padding.Right + szLineNumbers_Offset.Width - (int)(zTextSize.Width), lstLineNumberItem[zA].Rectangle.Top + (int)(lstLineNumberItem[zA].Rectangle.Height / 2) + szLineNumbers_Offset.Height - (int)(zTextSize.Height / 2));
                                break;
                            case ContentAlignment.BottomRight:
                                ptPoint = new Point(lstLineNumberItem[zA].Rectangle.Right - this.Padding.Right + szLineNumbers_Offset.Width - (int)(zTextSize.Width), lstLineNumberItem[zA].Rectangle.Bottom - this.Padding.Bottom + 1 + szLineNumbers_Offset.Height - (int)zTextSize.Height);
                                break;
                        }
                        //   TextClipping
                        zItemClipRectangle = new Rectangle(ptPoint, zTextSize.ToSize());
                        if (bLineNumbersClipByItemRectangle == true)
                        {
                            //   If selected, the text will be clipped so that it doesn't spill out of its own LineNumberItem-area.
                            //   Only the part of the text inside the LineNumberItem.Rectangle should be visible, so intersect with the ItemRectangle
                            //   The SetClip method temporary restricts the drawing area of the control for whatever is drawn next.
                            zItemClipRectangle.Intersect(lstLineNumberItem[zA].Rectangle);
                            e.Graphics.SetClip(zItemClipRectangle);
                        }
                        //   TextDrawing
                        e.Graphics.DrawString(zTextToShow, this.Font, zBrush, ptPoint, zSF);
                        e.Graphics.ResetClip();
                        //   The GraphicsPath for the LineNumber is just a rectangle behind the text, to keep the paintingspeed high and avoid ugly artifacts.
                        zGP_LineNumbers.AddRectangle(zItemClipRectangle);
                        zGP_LineNumbers.CloseFigure();
                    }
                }

                // --- GridLinesThickness and Linestyle in SeeThroughMode. All GraphicsPath lines are drawn as solid to keep the paintingspeed high.
                if (bGridLinesShow == true)
                {
                    zPen.DashStyle = DashStyle.Solid;
                    zGP_GridLines.Widen(zPen);
                }

                // --- Memory CleanUp
                if (zLGB != null)
                    zLGB.Dispose();
            }


            // ----------------------------------------------
            // --- DESIGN OR RUNTIME / ALWAYS
            Point zP_Left = new Point((int)Math.Floor(fBorderLinesThickness / 2), (int)Math.Floor(fBorderLinesThickness / 2));
            Point zP_Right = new Point(this.Width - (int)Math.Ceiling(fBorderLinesThickness / 2), this.Height - (int)Math.Ceiling(fBorderLinesThickness / 2));

            // --- BorderLines 
            Point[] zBorderLines_Points = {
			    new Point(zP_Left.X, zP_Left.Y),
			    new Point(zP_Right.X, zP_Left.Y),
			    new Point(zP_Right.X, zP_Right.Y),
			    new Point(zP_Left.X, zP_Right.Y),
			    new Point(zP_Left.X, zP_Left.Y)
		    };

            if (bBorderLines_Show == true)
            {
                zPen = new Pen(colorBorderLines, fBorderLinesThickness);
                zPen.DashStyle = dashStyleBorderLines;
                e.Graphics.DrawLines(zPen, zBorderLines_Points);
                zGP_BorderLines.AddLines(zBorderLines_Points);
                zGP_BorderLines.CloseFigure();
                //   BorderThickness and Style for SeeThroughMode
                zPen.DashStyle = DashStyle.Solid;
                zGP_BorderLines.Widen(zPen);
            }


            // --- MarginLines 
            if (bMarginLinesShow == true && dockSideMarginLines > LineNumberDockSide.None)
            {
                zP_Left = new Point((int)-fMarginLinesThickness, (int)-fMarginLinesThickness);
                zP_Right = new Point(this.Width + (int)fMarginLinesThickness, this.Height + (int)fMarginLinesThickness);
                zPen = new Pen(colorMarginLines, fMarginLinesThickness);
                zPen.DashStyle = dashStyleMarginLines;
                if (dockSideMarginLines == LineNumberDockSide.Left | dockSideMarginLines == LineNumberDockSide.Height)
                {
                    e.Graphics.DrawLine(zPen, new Point((int)Math.Floor(fMarginLinesThickness / 2), 0), new Point((int)Math.Floor(fMarginLinesThickness / 2), this.Height - 1));
                    zP_Left = new Point((int)Math.Ceiling(fMarginLinesThickness / 2), (int)-fMarginLinesThickness);
                }
                if (dockSideMarginLines == LineNumberDockSide.Right | dockSideMarginLines == LineNumberDockSide.Height)
                {
                    e.Graphics.DrawLine(zPen, new Point(this.Width - (int)Math.Ceiling(fMarginLinesThickness / 2), 0), new Point(this.Width - (int)Math.Ceiling(fMarginLinesThickness / 2), this.Height - 1));
                    zP_Right = new Point(this.Width - (int)Math.Ceiling(fMarginLinesThickness / 2), this.Height + (int)fMarginLinesThickness);
                }
                //   GraphicsPath for the MarginLines(s):
                //   MarginLines(s) are drawn as a rectangle connecting the zP_Left and zP_Right points, which are either inside or 
                //   outside of sight, depending on whether the MarginLines at that side is visible. zP_Left: TopLeft and ZP_Right: BottomRight
                zGP_MarginLines.AddRectangle(new Rectangle(zP_Left, new Size(zP_Right.X - zP_Left.X, zP_Right.Y - zP_Left.Y)));
                zPen.DashStyle = DashStyle.Solid;
                zGP_MarginLines.Widen(zPen);
            }


            // ----------------------------------------------
            // --- SeeThroughMode
            //   combine all the GraphicsPaths (= zGP_... ) and set them as the region for the control.
            if (bSeeThroughMode == true)
            {
                zRegion.MakeEmpty();
                zRegion.Union(zGP_BorderLines);
                zRegion.Union(zGP_MarginLines);
                zRegion.Union(zGP_GridLines);
                zRegion.Union(zGP_LineNumbers);
            }

            // --- Region
            if (zRegion.GetBounds(e.Graphics).IsEmpty == true)
            {
                //   Note: If the control is in a condition that would show it as empty, then a border-region is still drawn regardless of it's borders on/off state.
                //   This is added to make sure that the bounds of the control are never lost (it would remain empty if this was not done).
                zGP_BorderLines.AddLines(zBorderLines_Points);
                zGP_BorderLines.CloseFigure();
                zPen = new Pen(colorBorderLines, 1);
                zPen.DashStyle = DashStyle.Solid;
                zGP_BorderLines.Widen(zPen);

                zRegion = new Region(zGP_BorderLines);
            }
            this.Region = zRegion;


            // ----------------------------------------------
            // --- Memory CleanUp
            if (zPen != null)
                zPen.Dispose();
            if (zBrush != null)
                zPen.Dispose();
            if (zRegion != null)
                zRegion.Dispose();
            if (zGP_GridLines != null)
                zGP_GridLines.Dispose();
            if (zGP_BorderLines != null)
                zGP_BorderLines.Dispose();
            if (zGP_MarginLines != null)
                zGP_MarginLines.Dispose();
            if (zGP_LineNumbers != null)
                zGP_LineNumbers.Dispose();

        }

        #endregion

        #region " Designer Code "

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion

        #endregion
    }
}