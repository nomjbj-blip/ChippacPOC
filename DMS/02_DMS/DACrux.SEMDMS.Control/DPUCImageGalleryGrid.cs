using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.Base;
using DACrux.Common.RO;
using DACrux.Framework.Base;
using DACrux.SEMDMS.RO;
using DACrux.Utility;
using FarPoint.Win;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using FarPoint.Win.Spread.Model;

namespace DACrux.SEMDMS.Control
{
    public partial class DPUCImageGalleryGrid
        : UserControl, ISendDefect, IExportExcel
    {
        //------------------------------------------------------------------------

        #region [ Data Field ]

        private DataTable dtDefectType = null;
        private DefectList SelectedDefectList = null;
        private RemoveDefectImageList removeDefects = null;
        //--
        ImagePopUp dlg = null;
        private bool SelectedCellHeader = false;
        private List<long> steps = null;

        private enum DefectTypeColumn
        {
            CLASSNUMBER = 0,
            NAME,
            DESCRIPTION,
            GROUP_ID,
            DELETE_FLAG,
            DEFECT_COLOR
        }
        private enum DeleteDefectColIndex
        {
            STEP_SEQ = 0,
            DEFECTID,
            WAFER_SEQ,
            IMAGE_ID,
            FILENAME
        }
        private enum ReclassifiedNewDefectColIndex
        {
            STEP_SEQ = 0,
            DEFECTID,
            WAFER_SEQ,
            CLASSNUMBER,
            NEW_DEFECT_CLASS
        }
        private enum RemoveSheetColumnIndex
        {
            DEFECTID = 0,
            DEFECTCLASS,
            STEPID,
            IMAGEID,
            FILENAME
        }

        #endregion [ Data Field ]

        //------------------------------------------------------------------------

        #region [ Constructor ]

        public DPUCImageGalleryGrid(
            )
        {
            InitializeComponent();
            this.Load += new EventHandler(
                DPUCImageGallery_Load
                );

            SelectedDefectList = new DefectList();
            removeDefects = new RemoveDefectImageList();
            dpucReclassify1.OnReclassifyApply += new EventHandler(dpucReclassify1_OnReclassifyApply);
        }

        //--

        #endregion [ Constructor ]

        //------------------------------------------------------------------------

        #region [ Event Handler ]

        //--

        void DPUCImageGallery_Load(
            object sender,
            EventArgs e
            )
        {
            if (DesignMode)
                return;
        }

        //--

        private void btnCountApply_Click(
            object sender,
            EventArgs e
            )
        {
            DefectList defects = ImageDataSource as DefectList;
            if (defects == null)
                return;

            if (chkNone.Checked)
                DrawImages(defects);
            else
                DrawImages(defects, true);
        }

        //--

        private void btnDelete_Click(
            object sender,
            EventArgs e
            )
        {
            if (removeDefects == null || removeDefects.Count <= 0)
                return;

            if (MessageBox.Show("이미지를 삭제하시겠습니까?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                return;

            string[,] Params = new string[removeDefects.Count, 4];
            steps = new List<long>();
            for (int idx = 0; idx < removeDefects.Count; idx++)
            {
                int lstIdx = steps.BinarySearch(removeDefects[idx].STEP_SEQ);
                if (lstIdx < 0)
                    steps.Insert(~lstIdx, removeDefects[idx].STEP_SEQ);

                Params[idx, (int)DeleteDefectColIndex.STEP_SEQ] = removeDefects[idx].STEP_SEQ.ToString();
                Params[idx, (int)DeleteDefectColIndex.DEFECTID] = removeDefects[idx].DEFECTID.ToString();
                Params[idx, (int)DeleteDefectColIndex.WAFER_SEQ] = removeDefects[idx].WAFER_SEQ.ToString();
                Params[idx, (int)DeleteDefectColIndex.IMAGE_ID] = removeDefects[idx].IMAGE_ID.ToString();
            }

            // 이미지 삭제 로직 적용,
            DefectMapAnalysis obj = new DefectMapAnalysis();
            ImageDataSource = obj.SetRemoveDefectImageList(
                steps.ToArray(),
                Params,
                Base.GlobalVariable.UserID,
                Base.GlobalVariable.LocalIP
                );
            DataBinding();
        }

        //--

        private void chkNoDisplay_CheckedChanged(
            object sender,
            EventArgs e
            )
        {
            DefectList defects = ImageDataSource as DefectList;
            if (defects == null)
                return;

            if (chkNone.Checked)
                DrawImages(defects);
            else
                DrawImages(defects, true);
        }

        //--

        private void chkNone_CheckedChanged(
            object sender,
            EventArgs e
            )
        {
            DefectList defects = ImageDataSource as DefectList;
            if (defects == null)
                return;

            if ((sender as CheckBox).Checked)
                DrawImages(defects);
            else
                DrawImages(defects, true);
        }

        //--

        void dpucReclassify1_OnReclassifyApply(
            object sender,
            EventArgs e
            )
        {
            if (dpucReclassify1.DataSource == null || dpucReclassify1.DataSource.Count <= 0)
                return;

            string[,] Params = new string[SelectedDefectList.Count, 5];
            object item = dpucReclassify1.SelectedNewDefectClass;
            if (item == null) return;

            if (MessageBox.Show("Reclassify를 진행하시겠습니까?", "Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) != DialogResult.OK)
                return;

            DefectList defects = dpucReclassify1.DataSource;
            steps = new List<long>();
            for (int idx = 0; idx < defects.Count; idx++)
            {
                int lstIdx = steps.BinarySearch(defects[idx].STEP_SEQ);
                if (lstIdx < 0)
                    steps.Insert(~lstIdx, defects[idx].STEP_SEQ);

                Params[idx, (int)ReclassifiedNewDefectColIndex.STEP_SEQ] = defects[idx].STEP_SEQ.ToString();
                Params[idx, (int)ReclassifiedNewDefectColIndex.DEFECTID] = defects[idx].DEFECTID.ToString();
                Params[idx, (int)ReclassifiedNewDefectColIndex.WAFER_SEQ] = defects[idx].WAFER_SEQ.ToString();
                Params[idx, (int)ReclassifiedNewDefectColIndex.CLASSNUMBER] = defects[idx].CLASSNUMBER.ToString();
                Params[idx, (int)ReclassifiedNewDefectColIndex.NEW_DEFECT_CLASS] = item.ToString();
            }

            // 권한 체크하는 로직 추가 필요
            DefectMapAnalysis obj = new DefectMapAnalysis();
            ImageDataSource = obj.SetReclassifyDefectImageList(
                steps.ToArray(),
                Params,
                Base.GlobalVariable.UserID,
                Base.GlobalVariable.LocalIP
                );

            DataBinding();
        }

        //--

        private void fpsDefectImages_CellClick(
            object sender,
            FarPoint.Win.Spread.CellClickEventArgs e
            )
        {
            ImageGalleryControl igc = null;
            if (e.ColumnHeader && e.RowHeader)
            {
                DefectList defects = ImageDataSource as DefectList;
                dpucInformation1.DataSource = DefectDataSource;
                dpucReclassify1.DataSource = defects;

                SelectedDefectList.Clear();

                foreach (Defect defect in defects)
                    AppendSelectedDefect(defect);

                SelectedCellHeader = true;
            }
            else if (e.ColumnHeader)
            {
                /// 키보드에서 Control 키 비활성화 시 초기화
                if (ModifierKeys != Keys.Control)
                {
                    SelectedDefectList.Clear();
                    removeDefects.Clear();
                }

                for (int idx = 0; idx < fpsDefectImages_Sheet1.Rows.Count; idx++)
                {
                    igc = fpsDefectImages_Sheet1.Cells[idx, e.Column].Tag as ImageGalleryControl;

                    if (igc == null)
                        continue;

                    AppendSelectedDefect(igc.Defect);
                    
                    removeDefects.Add(new RemoveDefectImage()
                    {
                        STEP_SEQ = igc.Defect.STEP_SEQ,
                        WAFER_SEQ = igc.Defect.WAFER_SEQ,
                        DEFECTID = igc.Defect.DEFECTID,
                        CLASSNUMBER = igc.Defect.CLASSNUMBER,
                        IMAGE_ID = igc.ImageID,
                        FILENAME = igc.FileName
                    });
                }
                dpucInformation1.DataSource = SelectedDefectList;
                dpucReclassify1.DataSource = SelectedDefectList;
                SelectedCellHeader = true;
            }
            else if (e.RowHeader)
            {
                /// 키보드에서 Control 키 비활성화 시 초기화
                if (ModifierKeys != Keys.Control)
                {
                    SelectedDefectList.Clear();
                    removeDefects.Clear();
                }

                for (int idx = 0; idx < fpsDefectImages_Sheet1.Columns.Count; idx++)
                {
                    igc = fpsDefectImages_Sheet1.Cells[e.Row, idx].Tag as ImageGalleryControl;
                    if (igc == null)
                        continue;

                    AppendSelectedDefect(igc.Defect);

                    removeDefects.Add(new RemoveDefectImage()
                    {
                        STEP_SEQ = igc.Defect.STEP_SEQ,
                        WAFER_SEQ = igc.Defect.WAFER_SEQ,
                        DEFECTID = igc.Defect.DEFECTID,
                        CLASSNUMBER = igc.Defect.CLASSNUMBER,
                        IMAGE_ID = igc.ImageID,
                        FILENAME = igc.FileName
                    });
                }
                dpucInformation1.DataSource = SelectedDefectList;
                dpucReclassify1.DataSource = SelectedDefectList;
                SelectedCellHeader = true;
            }
            else
            {
                SelectedCellHeader = false;
            }
        }

        private void AppendSelectedDefect(Defect defect)
        {
            int idx = SelectedDefectList.BinarySearch(defect);

            if (idx < 0)
                SelectedDefectList.Insert(~idx, defect);
        }

        //--

        private void fpsDefectImages_CellDoubleClick(
            object sender,
            FarPoint.Win.Spread.CellClickEventArgs e
            )
        {
            if (e.ColumnHeader || e.RowHeader) return;

            FpSpread spread = sender as FpSpread;
            if (spread == null) return;

            ImageGalleryControl igc = spread.ActiveSheet.Cells[e.Row, e.Column].Tag as ImageGalleryControl;
            if (igc == null) return;

            DefectImagePopup(igc);
        }

        //--

        private void fpsDefectImages_ColumnWidthChanged(
            object sender,
            FarPoint.Win.Spread.ColumnWidthChangedEventArgs e
            )
        {
            if (e.ColumnList.Count == 0)
                return;

            ColumnWidthChangeExtents val = e.ColumnList[0] as ColumnWidthChangeExtents;
            for (int col = val.FirstColumn; col <= val.LastColumn; col++)
            {
                int width = (int)(fpsDefectImages_Sheet1.Columns[col].Width * fpsDefectImages.ZoomFactor);

                for (int row = 0; row < fpsDefectImages_Sheet1.RowCount; row++)
                {
                    int height = (int)(fpsDefectImages_Sheet1.Rows[row].Height * fpsDefectImages.ZoomFactor);
                    if (fpsDefectImages_Sheet1.Cells[row, col].Tag is ImageGalleryControl)
                    {
                        ImageGalleryControl control = (ImageGalleryControl)fpsDefectImages_Sheet1.Cells[row, col].Tag;
                        DrawingByDefectImage(control.Defect, control.ImageID, row, col, width, height);
                    }
                }
            }
        }

        //--

        private void fpsDefectImages_EnterCell(
            object sender,
            EnterCellEventArgs e
            )
        {
            if (!chkZoomable.Checked)
                return;

            ImageGalleryControl igc = fpsDefectImages_Sheet1.Cells[e.Row, e.Column].Tag as ImageGalleryControl;
            if (igc == null)
                return;

            DefectImagePopup(igc);
        }

        //--

        private void fpsDefectImages_KeyDown(
            object sender,
            KeyEventArgs e
            )
        {
            FpSpread spread = sender as FpSpread;
            if (spread == null)
                return;

            SheetView sheet = spread.ActiveSheet;
            ImageGalleryControl igc = sheet.Cells[sheet.ActiveRowIndex, sheet.ActiveColumnIndex].Tag as ImageGalleryControl;
            if (igc == null)
                return;

            // 이미지 복사
            if (e.Control && e.KeyCode == Keys.C)
            {
                e.Handled = true;
                Clipboard.Clear();
                Clipboard.SetImage(igc.DefectImage);
            }
            else if (e.KeyCode == Keys.Enter)
            {
                // Defect Image에 대한 Popup 창을 띄운다.
                DefectImagePopup(igc);
            }
        }

        //--

        private void fpsDefectImages_MouseUp(
            object sender,
            MouseEventArgs e
            )
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left && !SelectedCellHeader)
            {
                SheetView sheet = fpsDefectImages_Sheet1;
                ImageGalleryControl igc = null;

                if (ModifierKeys != Keys.Control)
                {
                    SelectedDefectList.Clear();
                    removeDefects.Clear();
                }

                foreach (CellRange range in sheet.GetSelections())
                {
                    for (int rowidx = range.Row; rowidx < range.Row + range.RowCount; rowidx++)
                    {
                        for (int colidx = range.Column; colidx < range.Column + range.ColumnCount; colidx++)
                        {
                            igc = sheet.Cells[rowidx, colidx].Tag as ImageGalleryControl;
                            if (igc == null)
                                continue;

                            AppendSelectedDefect(igc.Defect);

                            int idx = removeDefects.FindIndex(delegate(RemoveDefectImage rdi) {
                                return rdi.STEP_SEQ == igc.Defect.STEP_SEQ 
                                    && rdi.WAFER_SEQ == igc.Defect.WAFER_SEQ 
                                    && rdi.DEFECTID == igc.Defect.DEFECTID 
                                    && rdi.IMAGE_ID == igc.ImageID;
                            });

                            if (idx < 0)
                            {
                                removeDefects.Insert(~idx, new RemoveDefectImage()
                                {
                                    STEP_SEQ = igc.Defect.STEP_SEQ,
                                    WAFER_SEQ = igc.Defect.WAFER_SEQ,
                                    DEFECTID = igc.Defect.DEFECTID,
                                    CLASSNUMBER = igc.Defect.CLASSNUMBER,
                                    IMAGE_ID = igc.ImageID,
                                    FILENAME = igc.FileName
                                });
                            }
                        }
                    }
                }
                if (igc != null)
                    dpucInformation1.DataSource = igc.Defect;
                else
                    dpucInformation1.DataSource = null;
                dpucReclassify1.DataSource = SelectedDefectList;
                FillRemoveDefectList();
            }
        }

        //--

        private void fpsDefectImages_RowHeightChanged(
            object sender,
            FarPoint.Win.Spread.RowHeightChangedEventArgs e
            )
        {
            if (e.RowList.Count == 0)
                return;

            RowHeightChangeExtents val = e.RowList[0] as RowHeightChangeExtents;
            for (int row = val.FirstRow; row <= val.LastRow; row++)
            {
                int height = (int)(fpsDefectImages_Sheet1.Rows[row].Height * fpsDefectImages.ZoomFactor);

                for (int col = 0; col < fpsDefectImages_Sheet1.ColumnCount; col++)
                {
                    int width = (int)(fpsDefectImages_Sheet1.Columns[col].Width * fpsDefectImages.ZoomFactor);
                    if (fpsDefectImages_Sheet1.Cells[row, col].Tag is ImageGalleryControl)
                    {
                        ImageGalleryControl control = (ImageGalleryControl)fpsDefectImages_Sheet1.Cells[row, col].Tag;
                        DrawingByDefectImage(control.Defect, control.ImageID, row, col, width, height);
                    }
                }
            }
        }

        //--

        private void fpsDefectImages_UserZooming(
            object sender,
            ZoomEventArgs e
            )
        {
            if (fpsDefectImages_Sheet1.RowCount == 0 || fpsDefectImages_Sheet1.ColumnCount == 0)
                return;

            GeneralCellType genCellType = null;
            ImageGalleryControl igc = null;
            for (int r = 0; r < fpsDefectImages_Sheet1.RowCount; r++)
            {
                int height = (int)(fpsDefectImages_Sheet1.Rows[r].Height * e.NewZoomFactor);

                for (int c = 0; c < fpsDefectImages_Sheet1.ColumnCount; c++)
                {
                    int width = (int)(fpsDefectImages_Sheet1.Columns[c].Width * e.NewZoomFactor);

                    genCellType = fpsDefectImages_Sheet1.Cells[r, c].CellType as GeneralCellType;
                    igc = fpsDefectImages_Sheet1.Cells[r, c].Tag as ImageGalleryControl;
                    if (igc != null)
                    {
                        igc.Size = new Size(width, height);
                        genCellType.BackgroundImage = new Picture(DrawControlToBitmap(igc));
                    }
                }
            }
        }

        #endregion [ Event Handler ]

        //------------------------------------------------------------------------

        #region [ Method ]

        //--

        public void Closed(
            )
        {
            DefectMapDraw.fnFTPImageDownloadClear();

            if (dtDefectType != null)
                dtDefectType.Dispose();

            if (SelectedDefectList != null)
            {
                SelectedDefectList.Clear();
                SelectedDefectList = null;
            }
        }

        //--

        public void DataBinding(
            )
        {
            Utility.FPSpreadUtil.InitSpread(fpsDefectImages);
            dpucInformation1.Wafer = Wafer;
            dpucInformation1.DataSource = DefectDataSource;

            //--

            fpsDefectImages.DataSource = null;
            fpsDefectImages_Sheet1.DataSource = null;
            fpsDefectImages_Sheet1.RowCount = 0;

            //--

            DefectList defects = ImageDataSource as DefectList;
            if (defects == null || defects.Count <= 0)
                return;

            //--

            dpucReclassify1.DataSource = null;

            //--

            ComConfiguration oComConfig = new ComConfiguration();
            DataSource_Config = oComConfig.GetConfigUser(
                Base.GlobalVariable.Factory,
                "WAFER_OPTION_IMAGE",
                Base.GlobalVariable.UserID
                );

            //Wafer Information 사용 여부
            DataTable dt = oComConfig.GetConfigurationUser(
                  DACrux.Base.GlobalVariable.Factory,
                  "WAFER_IMAGE_ENABLE",
                  DACrux.Base.GlobalVariable.UserID
                  );

            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["VALUE"].ToString() == "N")
                    DataSource_Config = null;
            }

            //--

            if (chkNone.Checked)
                DrawImages(defects);
            else
                DrawImages(defects, true);
        }

        //--

        private void DefectImagePopup(
            ImageGalleryControl igc
            )
        {
            DefectMapAnalysis obj = new DefectMapAnalysis();
            string imagePath = obj.GetImagePath(
                igc.Defect.STEP_SEQ,
                igc.Defect.WAFER_SEQ,
                igc.Defect.DEFECTID,
                igc.ImageID
                );
            if (String.IsNullOrEmpty(imagePath))
                return;

            Stream stream = DefectMapDraw.fnFTPGetStream(imagePath);
            if (stream == null)
                return;

            if (dlg == null || !dlg.Visible)
                dlg = new ImagePopUp();

            dlg.DefectImage = Image.FromStream(stream);
            if (igc.Description != null)
            {
                string[] desc = igc.Description.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (desc != null && desc.Length > 0)
                    dlg.Text = String.Format("{0}", String.Join(", ", desc));
                else dlg.Text = String.Format("Defect ID: {0}", igc.Defect.DEFECTID);
            }
            else
            {
                dlg.Text = String.Format("Defect ID: {0}", igc.Defect.DEFECTID);
            }
            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.Visible = false;
            dlg.Show(this);
            fpsDefectImages.Focus();
        }

        //--

        private void DrawImages(
            )
        {
            DefectList defects = ImageDataSource as DefectList;
            if (defects == null)
                return;

            if (chkNone.Checked)
                DrawImages(defects);
            else
                DrawImages(defects, false);
        }

        //--

        private void DrawImages(
            DefectList defects,
            bool resizeFlag
            )
        {
            Dictionary<long, SortedDictionary<long, DefectList>> dicStepDefects = GetDictionaryData(
                defects
                );

            InitGrid(dicStepDefects);

            if (resizeFlag)
                ResizeControl();

            foreach (KeyValuePair<long, SortedDictionary<long, DefectList>> pk in dicStepDefects)
            {
                foreach (KeyValuePair<long, DefectList> ppk in pk.Value)
                {
                    for (int idx = 0; idx < fpsDefectImages_Sheet1.ColumnCount; idx++)
                    {
                        long l = (long)fpsDefectImages_Sheet1.Columns[idx].Tag;
                        if (l == ppk.Key)
                        {
                            DrawingByDefectImage(ppk.Value, idx);
                            break;
                        }
                    }
                }
            }
        }

        private void DrawImages(
            DefectList defects
            )
        {
            Utility.FPSpreadUtil.InitSpread(fpsDefectImages);
            fpsDefectImages_Sheet1.OperationMode = OperationMode.Normal;
            fpsDefectImages_Sheet1.ColumnCount = (int)numColCount.Value;
            fpsDefectImages_Sheet1.RowCount = GetRowCount(GetTotalImageCnt(defects));
            ResizeControl();

            int rowIdx = 0;
            int colIdx = 0;
            float fWidth = 0;
            float fHeight = 0;

            foreach (Defect d in defects)
            {
                if (d.IMAGECOUNT <= 0)
                    continue;

                for (int imgIdx = 0; imgIdx < d.Images.Count; imgIdx++)
                {
                    if (String.IsNullOrEmpty(d.Images[imgIdx]))
                        continue;

                    fpsDefectImages_Sheet1.Columns[colIdx].Locked = true;
                    fWidth = fpsDefectImages_Sheet1.Columns[colIdx].Width;
                    fHeight = fpsDefectImages_Sheet1.Rows[rowIdx].Height;

                    DrawingByDefectImage(d, imgIdx, rowIdx, colIdx++, fWidth, fHeight);
                    if (colIdx == (int)numColCount.Value)
                    {
                        rowIdx++;
                        colIdx = 0;
                    }
                }

            }
        }

        //--

        private void DrawingByDefectImage(
            DefectList defects,
            int ColumnIndex
            )
        {
            float fHeight = 0;
            float fWidth = fpsDefectImages_Sheet1.Columns[ColumnIndex].Width;
            fpsDefectImages_Sheet1.Columns[ColumnIndex].Locked = true;
            Dictionary<long, int> dicRowIndex = new Dictionary<long, int>();

            foreach (Defect d in defects)
            {
                if (d.IMAGECOUNT <= 0) continue;

                for (int imgIdx = 0; imgIdx < d.Images.Count; imgIdx++)
                {
                    DefectImage defectImage = d.Images.GetDefectImage(imgIdx);
                    for (int idx = 0; idx < fpsDefectImages_Sheet1.Rows.Count; idx++)
                    {
                        long stepseq = (long)fpsDefectImages_Sheet1.Rows[idx].Tag;
                        if (stepseq == (long)d.STEP_SEQ)
                        {
                            if (dicRowIndex.ContainsKey(stepseq))
                                dicRowIndex[stepseq]++;
                            else dicRowIndex.Add(stepseq, 0);

                            fHeight = fpsDefectImages_Sheet1.Rows[dicRowIndex[stepseq]].Height;
                            DrawingByDefectImage(d, defectImage.IMAGESEQ, idx + dicRowIndex[stepseq], ColumnIndex, fWidth, fHeight);
                            break;
                        }
                    }
                }
            }
        }

        private void DrawingByDefectImage(
            Defect defect,
            int imgID,
            int rowIdx,
            int colIdx,
            float Width,
            float Height
            )
        {
            GeneralCellType genCellType = null;
            ImageGalleryControl imageCtrl = null;
            if (fpsDefectImages_Sheet1.Cells[rowIdx, colIdx].CellType == null)
            {
                //fpsDefectImages_Sheet1.Cells[rowIdx, colIdx].Locked = true;
                string fileName = Path.GetFileNameWithoutExtension(defect.Images[imgID]);
                imageCtrl = new ImageGalleryControl();
                imageCtrl.SetImage(defect.Images[imgID]);
                imageCtrl.ImageID = imgID;
                imageCtrl.FileName = fileName;
                imageCtrl.Description = chkNoDisplay.Checked ? null : GetDescription(defect);
                imageCtrl.Defect = defect;
                imageCtrl.OwnerCell = fpsDefectImages_Sheet1.Cells[rowIdx, colIdx];

                genCellType = new GeneralCellType();
                fpsDefectImages_Sheet1.Cells[rowIdx, colIdx].CellType = genCellType;
                fpsDefectImages_Sheet1.Cells[rowIdx, colIdx].Tag = imageCtrl;
            }
            else
            {
                genCellType = fpsDefectImages_Sheet1.Cells[rowIdx, colIdx].CellType as GeneralCellType;
                imageCtrl = fpsDefectImages_Sheet1.Cells[rowIdx, colIdx].Tag as ImageGalleryControl;
            }

            imageCtrl.Size = new Size(
                (int)Width,
                (int)Height
                );


            genCellType.BackgroundImage = new Picture(DrawControlToBitmap(imageCtrl));
        }

        //--

        private Bitmap DrawControlToBitmap(
            System.Windows.Forms.Control control
            )
        {
            Bitmap bitmap = new Bitmap(
                control.Width,
                control.Height
                );

            control.DrawToBitmap(bitmap, control.ClientRectangle);
            return bitmap;
        }

        //--

        private void FillRemoveDefectList(
            )
        {
            Utility.FPSpreadUtil.InitSpread(fpsRemoveImages);
            if (removeDefects == null || removeDefects.Count <= 0)
                return;

            fpsRemoveImages_Sheet1.ColumnCount = 5;
            fpsRemoveImages_Sheet1.RowCount = removeDefects.Count;

            fpsRemoveImages_Sheet1.Columns[(int)RemoveSheetColumnIndex.DEFECTID].Label = "Defect ID";
            fpsRemoveImages_Sheet1.Columns[(int)RemoveSheetColumnIndex.DEFECTID].Width = 60;
            fpsRemoveImages_Sheet1.Columns[(int)RemoveSheetColumnIndex.DEFECTCLASS].Label = "Defect Class";
            fpsRemoveImages_Sheet1.Columns[(int)RemoveSheetColumnIndex.DEFECTCLASS].Width = 100;
            fpsRemoveImages_Sheet1.Columns[(int)RemoveSheetColumnIndex.STEPID].Label = "Step ID";
            fpsRemoveImages_Sheet1.Columns[(int)RemoveSheetColumnIndex.STEPID].Width = 60;
            fpsRemoveImages_Sheet1.Columns[(int)RemoveSheetColumnIndex.IMAGEID].Label = "Image ID";
            fpsRemoveImages_Sheet1.Columns[(int)RemoveSheetColumnIndex.IMAGEID].Width = 60;
            fpsRemoveImages_Sheet1.Columns[(int)RemoveSheetColumnIndex.FILENAME].Label = "FileName";
            fpsRemoveImages_Sheet1.Columns[(int)RemoveSheetColumnIndex.FILENAME].Width = 75;

            for (int idx = 0; idx < removeDefects.Count; idx++)
            {
                fpsRemoveImages_Sheet1.Cells[idx, (int)RemoveSheetColumnIndex.DEFECTID].Value = removeDefects[idx].DEFECTID;
                fpsRemoveImages_Sheet1.Cells[idx, (int)RemoveSheetColumnIndex.DEFECTCLASS].Value = DmsCache.Instance.ClassLookup[removeDefects[idx].CLASSNUMBER];
                DmsWaferDieInfo info = DmsCache.Instance[removeDefects[idx].STEP_SEQ];
                fpsRemoveImages_Sheet1.Cells[idx, (int)RemoveSheetColumnIndex.STEPID].Value = info.StepInfo.StepID;
                fpsRemoveImages_Sheet1.Cells[idx, (int)RemoveSheetColumnIndex.IMAGEID].Value = removeDefects[idx].IMAGE_ID;
                fpsRemoveImages_Sheet1.Cells[idx, (int)RemoveSheetColumnIndex.FILENAME].Value = removeDefects[idx].FILENAME;
            }
        }

        //--

        private string GetDescription(
            Defect defect
            )
        {
            StringBuilder sbText = new StringBuilder();
            DataTable dt = DataSource_Config as DataTable;
            DmsStepInfo inspStepInfo = DmsCache.Instance[defect.STEP_SEQ].StepInfo;
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (String.IsNullOrEmpty(dr["USER_ID"].ToString()))
                        continue;

                    String strName = dr["NAME"] as String;
                    switch (strName)
                    {
                        case "CLASSNUMBER":
                            sbText.AppendLine(String.Format("DEFECT CLASS: {0}", defect.CLASSNUMBER));
                            break;
                        case "DEFECTID":
                            sbText.AppendLine(String.Format("DEFECT ID: {0}", defect.DEFECTID));
                            break;
                        case "INSPECTION_EQ":
                            sbText.AppendLine(String.Format("INSPECTOR: {0}", inspStepInfo.Inspector));
                            break;
                        case "LOT_ID":
                            sbText.AppendLine(String.Format("LOT: {0}", inspStepInfo.LotID));
                            break;
                        case "MAIN_EQ":
                            sbText.AppendLine(String.Format("MAIN EQ: {0}", inspStepInfo.Equip));
                            break;
                        case "PRODUCT":
                            sbText.AppendLine(String.Format("DEVICE: {0}", inspStepInfo.DeviceID));
                            break;
                        case "RESULTTIMESTAMP":
                            sbText.AppendLine(String.Format("INSPECTION TIME: {0}", inspStepInfo.ResultTimestamp));
                            break;
                        case "SETUP_ID":
                            sbText.AppendLine(String.Format("SETUP ID: {0}", inspStepInfo.SetupID));
                            break;
                        case "SETUP_TIME":
                            sbText.AppendLine(String.Format("SETUP TIME: {0}", inspStepInfo.SetupTimestamp));
                            break;
                        case "SLOT_ID":
                            sbText.AppendLine(String.Format("SLOT ID: {0}", inspStepInfo.Slot));
                            break;
                        case "STEP_ID":
                            sbText.AppendLine(String.Format("LAYER ID: {0}", inspStepInfo.StepID));
                            break;
                        case "WAFER_ID":
                            sbText.AppendLine(String.Format("WAFER: {0}", inspStepInfo.WaferID));
                            break;
                        default:
                            break;
                    }
                }

                if (sbText.Length <= 0)
                {
                    sbText.AppendLine(String.Format("LOT: {0}", inspStepInfo.LotID));
                    sbText.AppendLine(String.Format("SLOT ID: {0}", inspStepInfo.Slot));
                    sbText.AppendLine(String.Format("LAYER ID: {0}", inspStepInfo.StepID));
                }
            }

            return sbText.ToString();
        }

        //--

        private Dictionary<long, SortedDictionary<long, DefectList>> GetDictionaryData(
            DefectList defects
            )
        {
            Dictionary<long, SortedDictionary<long, DefectList>> dicStepDefects = new Dictionary<long, SortedDictionary<long, DefectList>>();
            SortedDictionary<long, DefectList> dicDefects = null;
            DefectList tmpDefects = null;

            foreach (Defect d in defects)
            {
                if (d.IMAGECOUNT <= 0)
                    continue;

                if (!dicStepDefects.ContainsKey(d.STEP_SEQ))
                {
                    dicDefects = new SortedDictionary<long, DefectList>();
                    tmpDefects = new DefectList();
                    tmpDefects.Add(d);
                    dicDefects.Add(d.CLASSNUMBER, tmpDefects);
                    dicStepDefects.Add(d.STEP_SEQ, dicDefects);
                }
                else
                {
                    dicDefects = dicStepDefects[d.STEP_SEQ];
                    if (!dicDefects.ContainsKey(d.CLASSNUMBER))
                    {
                        tmpDefects = new DefectList();
                        tmpDefects.Add(d);
                        dicDefects.Add(d.CLASSNUMBER, tmpDefects);
                    }
                    else
                    {
                        tmpDefects = dicDefects[d.CLASSNUMBER];
                        tmpDefects.Add(d);
                        dicDefects[d.CLASSNUMBER] = tmpDefects;
                    }
                    dicStepDefects[d.STEP_SEQ] = dicDefects;
                }
            }
            return dicStepDefects;
        }

        private int GetRowCount(
            int totalCnt
            )
        {
            if (totalCnt % (int)numColCount.Value != 0)
                return (totalCnt / (int)numColCount.Value) + 1;
            else
                return (totalCnt / (int)numColCount.Value);
        }


        private int GetTotalImageCnt(
            DefectList defects
            )
        {
            int totalCnt = 0;
            foreach (Defect d in defects)
            {
                if (d.Images.Count <= 0)
                    continue;
                totalCnt += d.Images.Count;
            }

            return totalCnt;
        }

        //--

        private void ResizeControl(
            )
        {
            if (fpsDefectImages_Sheet1.ColumnCount == 0 || fpsDefectImages_Sheet1.RowCount == 0)
                return;

            fpsDefectImages.ZoomFactor = 1;
            float width = fpsDefectImages.Width - SystemInformation.VerticalScrollBarWidth;

            foreach (Column col in fpsDefectImages_Sheet1.RowHeader.Columns)
                width -= (int)col.Width;

            width = (float)(width / (int)numColCount.Value);

            //--

            float height = fpsDefectImages.Height - SystemInformation.HorizontalScrollBarHeight;

            foreach (Row row in fpsDefectImages_Sheet1.ColumnHeader.Rows)
                height -= (float)row.Height;

            height = (float)(height / (int)numRowCount.Value);

            //--

            for (int i = 0; i < fpsDefectImages_Sheet1.ColumnCount; i++)
            {
                fpsDefectImages_Sheet1.ColumnHeader.Columns[i].Width = width;
                fpsDefectImages_Sheet1.Columns[i].Width = width;
            }

            for (int i = 0; i < fpsDefectImages_Sheet1.RowCount; i++)
            {
                fpsDefectImages_Sheet1.RowHeader.Rows[i].Height = height;
                fpsDefectImages_Sheet1.Rows[i].Height = height;
            }

            fpsDefectImages.Refresh();
        }

        //--

        public Defect[] GetSelectedDefect(
            )
        {
            if (SelectedDefectList == null || SelectedDefectList.Count <= 0)
            {
                DefectList defects = ImageDataSource as DefectList;
                return defects.ToArray();
            }

            return SelectedDefectList.ToArray();
        }

        //--

        public void ExportExcel(
            )
        {
            ExcelSheet sheet = new ExcelSheet();
            sheet.Add(fpsDefectImages_Sheet1.FpSpread);

            ExcelExportArgs e = new ExcelExportArgs();
            e.SheetList.Add(sheet);

            ExcelExportManager.Export(e);
        }

        //--

        private void InitGrid(
            Dictionary<long, SortedDictionary<long, DefectList>> dicStepDefects
            )
        {
            List<long> stepKeys = null;
            List<long> classKeys = null;
            List<long> tmpKeys = null;
            int iTmp = 0;
            int iSearchIndex = -1;

            Utility.FPSpreadUtil.InitSpread(fpsDefectImages);
            fpsDefectImages_Sheet1.OperationMode = OperationMode.Normal;

            Dictionary<long, int> dicStepClassMax = new Dictionary<long, int>();
            stepKeys = new List<long>(dicStepDefects.Keys);
            classKeys = new List<long>();
            foreach (long step in stepKeys)
            {
                tmpKeys = new List<long>(dicStepDefects[step].Keys);
                tmpKeys.Sort();
                iTmp = 0;
                foreach (long tk in tmpKeys)
                {
                    iSearchIndex = classKeys.FindIndex(x => long.Equals(x, tk));
                    if (iSearchIndex == -1)
                        classKeys.Add(tk);

                    DefectList dslist = dicStepDefects[step][tk];
                    iTmp = Math.Max(iTmp, dslist.Sum(x => x.IMAGECOUNT));
                }

                dicStepClassMax.Add(step, iTmp);
            }

            steps = stepKeys;
            classKeys.Sort();

            //--

            fpsDefectImages_Sheet1.ColumnCount = classKeys.Count;
            for (int idx = 0; idx < fpsDefectImages_Sheet1.ColumnCount; idx++)
            {
                fpsDefectImages_Sheet1.Columns[idx].Label = DmsCache.Instance.ClassLookup[Base.Convert.intParse(classKeys[idx].ToString())];
                fpsDefectImages_Sheet1.Columns[idx].Tag = classKeys[idx];
            }

            //--

            foreach (KeyValuePair<long, int> pk in dicStepClassMax)
            {
                fpsDefectImages_Sheet1.RowCount += pk.Value;
                for (int idx = fpsDefectImages_Sheet1.Rows.Count - pk.Value; idx < fpsDefectImages_Sheet1.Rows.Count; idx++)
                {
                    DmsWaferDieInfo dmswaferinfo = DmsCache.Instance[pk.Key];
                    fpsDefectImages_Sheet1.Rows[idx].Label = String.Format("{0}:{1}", dmswaferinfo.StepInfo.LotID, dmswaferinfo.StepInfo.Slot);
                    fpsDefectImages_Sheet1.Rows[idx].Tag = (long)dmswaferinfo.StepSeq;
                }
            }
        }

        #endregion [ Method ]

        //------------------------------------------------------------------------

        #region [ Properties ]
        public DPWafer[] Wafer
        {
            get;
            set;
        }

        [Browsable(false)]
        public object DefectDataSource
        {
            get;
            set;
        }

        //--

        [Browsable(false)]
        public object ImageDataSource
        {
            get;
            set;
        }

        //--

        public object DataSource_Config
        {
            get;
            private set;
        }

        //--

        public String LotID
        {
            get;
            private set;
        }

        //--

        public String WaferID
        {
            get;
            private set;
        }

        //--

        public String StepID
        {
            get;
            private set;
        }

        //--

        public long[] Steps
        {
            get;
            set;
        }

        //--

        public decimal ColCount
        {
            get { return numColCount.Value; }
            set { numColCount.Value = value; }
        }

        public decimal RowCount
        {
            get { return numRowCount.Value; }
            set { numRowCount.Value = value; }
        }

        public bool NONE
        {
            get { return chkNone.Checked; }
            set { chkNone.Checked = value; }
        }

        public bool Display
        {
            get { return chkNoDisplay.Checked; }
            set { chkNoDisplay.Checked = value; }
        }

        public bool Zoomable
        {
            get { return chkZoomable.Checked; }
            set { chkZoomable.Checked = value; }
        }

        public bool IsReclassifiedPermesion
        {
            get { return tabControl1.TabPages["RECLASSIFY"].Visible; }
            set { tabControl1.TabPages["RECLASSIFY"].Visible = value; }
        }

        public bool IsImageDeletePermission
        {
            get { return tabControl1.TabPages["REMOVEIMG"].Visible; }
            set { tabControl1.TabPages["REMOVEIMG"].Visible = value; }
        }

        #endregion [ Properties ]
    }

    internal class RemoveDefectImage : IComparable
    {
        public long STEP_SEQ;
        public long WAFER_SEQ;
        public int DEFECTID;
        public int CLASSNUMBER;
        public int IMAGE_ID;
        public string FILENAME;

        public int CompareTo(object obj)
        {
            RemoveDefectImage value = obj as RemoveDefectImage;
            int cmp1 = this.STEP_SEQ.CompareTo(value.STEP_SEQ);
            int cmp2 = this.DEFECTID.CompareTo(value.DEFECTID);
            int cmp3 = this.IMAGE_ID.CompareTo(value.IMAGE_ID);

            if (cmp1 == 0 && cmp2 == 0 && cmp3 == 0)
                return 0;
            else if (cmp1 == 0 && cmp2 == 0)
                return cmp3;
            else if (cmp2 == 0 && cmp3 == 0)
                return cmp2;
            else
                return cmp1;
        }
    }

    internal class RemoveDefectImageList : List<RemoveDefectImage>, IComparer<RemoveDefectImage>
    {
        public int Compare(RemoveDefectImage x, RemoveDefectImage y)
        {
            int cmp1 = x.STEP_SEQ.CompareTo(y.STEP_SEQ);
            int cmp2 = x.DEFECTID.CompareTo(y.DEFECTID);
            int cmp3 = x.IMAGE_ID.CompareTo(y.IMAGE_ID);

            if (cmp1 == 0 && cmp2 == 0 && cmp3 == 0)
                return 0;
            else if (cmp1 == 0 && cmp2 == 0)
                return cmp3;
            else if (cmp1 == 0 && cmp3 == 0)
                return cmp2;
            else if (cmp2 == 0 && cmp3 == 0)
                return cmp1;
            else
                return cmp1;
        }

        public int BinarySearch(
            long stepSeq,
            long waferSeq,
            int defectId,
            int imageId
            )
        {
            RemoveDefectImage obj = new RemoveDefectImage();
            obj.STEP_SEQ = stepSeq;
            obj.WAFER_SEQ = waferSeq;
            obj.DEFECTID = defectId;
            obj.IMAGE_ID = imageId;
            return this.BinarySearch(obj);
        }
    }
}
