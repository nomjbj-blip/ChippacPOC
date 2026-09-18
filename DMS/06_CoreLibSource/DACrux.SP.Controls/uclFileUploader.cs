using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace DACrux.SP.Controls
{
    /// <summary>
    /// Class Name : uclFileUploader<br/>
    /// Summary    : File uploading Control Class<br/>
    /// Author     : 미라콤 양형석<br/>
    /// First Date : 2010-01-10<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public partial class uclFileUploader : UserControl
    {
        #region " ENUM "

        /// <summary>
        /// File type.
        /// </summary>
        public enum FileTypeItems { All, Image, Document };

        /// <summary>
        /// Uploading result type.
        /// </summary>
        [Flags]
        public enum UploadingResultItems { Success = 1, NoFileExists = 2, CreateDirectoryDenied= 4, Error = 8 }

        #endregion

        #region " MEMBER FIELD "

        private string strHomePath = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
        private string strTargetPath = string.Empty;
        private int iMaxCount = 5;
        private OpenFileDialog dlg = new OpenFileDialog();
        private List<string> lstFiles = new List<string>();
        private FileTypeItems fileType = FileTypeItems.All;
        private List<string> lstCopiedFiles = new List<string>();
        private bool bOverwrite = false;
        private DataTable dtList = new DataTable();

        #endregion

        #region " PROPERTY "

        /// <summary>
        /// Gets or sets whether show the Label image.
        /// </summary>
        [Category("Setup")]
        public bool ShowImage
        {
            get { return lblTitle.ShowImage; }
            set { lblTitle.ShowImage = value; }
        }

        /// <summary>
        /// Gets or sets whether show the Open button.
        /// </summary>
        [Category("Setup")]
        public bool ShowOpenButton
        {
            get { return btnOpen.Visible; }
            set { btnOpen.Visible = value; }
        }

        /// <summary>
        /// Gets or sets whether show the caption.
        /// </summary>
        [Category("Setup")]
        public bool ShowCaption
        {
            get { return lblTitle.Visible; }
            set { lblTitle.Visible = value; }
        }

        /// <summary>
        /// Gets or Sets title string for Listbox and Caption of this Selector.
        /// </summary>
        [Category("Setup")]
        public string Caption
        {
            get { return lblTitle.Text.TrimStart(' '); }
            set
            {
                lblTitle.Text = value;
                lblTitle.ShowImage = lblTitle.ShowImage;
            }
        }

        /// <summary>
        /// Gets or Sets the width of Caption.
        /// </summary>
        [Category("Setup")]
        public int CaptionWidth
        {
            get { return lblTitle.Width; }
            set
            {
                lblTitle.Width = value;
                //int iRight = lvList.Right;

                //if (value < 1)
                //    lblTitle.Width = DEFAULT_CAPTION_WIDTH;
                //else
                //    lblTitle.Width = value;

                //lvList.Left = lblTitle.Width;
                //lvList.Width = iRight - lvList.Left;

                //pnlButtons.Left = lvList.Left;
                //pnlButtons.Width = lvList.Width;
            }
        }

        /// <summary>
        /// Gets or Sets the style of the header.
        /// </summary>
        [Category("Setup")]
        public ColumnHeaderStyle HeaderStyle
        {
            get { return lvList.HeaderStyle; }
            set { lvList.HeaderStyle = value; }
        }

        /// <summary>
        /// Gets or Sets the type of the border.
        /// </summary>
        [Category("Setup")]
        public new System.Windows.Forms.BorderStyle BorderStyle
        {
            get { return lvList.BorderStyle; }
            set { lvList.BorderStyle = value; }
        }

        /// <summary>
        /// Gets or Sets the Home directory path.
        /// </summary>
        [Category("Setup")]
        public string HomePath
        {
            get { return strHomePath; }
            set { strHomePath = value; }
        }

        /// <summary>
        /// Gets or Sets the target path.
        /// </summary>
        [Category("Setup")]
        public string TargetPath
        {
            get { return strTargetPath; }
            set { strTargetPath = value; }
        }

        /// <summary>
        /// Gets or Sets the max file count.
        /// </summary>
        [Category("Setup")]
        public int MaxFileCount
        {
            get { return iMaxCount; }
            set
            {
                if (value < 1)
                    value = 1;

                iMaxCount = value;
            }
        }

        /// <summary>
        /// Gets or Sets whether allow overwrite or not.
        /// </summary>
        [Category("Setup")]
        public bool AllowOverwrite
        {
            get { return bOverwrite; }
            set { bOverwrite = value; }
        }

        /// <summary>
        /// Gets or Sets the file type.
        /// </summary>
        [Category("Setup")]
        public FileTypeItems FileType
        {
            get { return fileType; }
            set
            {
                fileType = value;

                switch (fileType)
                {
                    case FileTypeItems.All:
                        dlg.Filter = "All files (*.*)|*.*";
                        break;
                    case FileTypeItems.Document:
                        dlg.Filter = "Documents (*.ppt;*.docx;*.doc;*.xls;*.xlsx;*.hwp;*.xml;*.htm;*.html;*.txt)|*.ppt;*.docx;*.doc;*.xls;*.xlsx;*.hwp;*.xml;*.htm;*.html;*.txt";
                        break;
                    case FileTypeItems.Image:
                        dlg.Filter = "Images (*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF";
                        break;
                }
            }
        }

        /// <summary>
        /// Gets the origin file path array.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string[] SourceFiles
        {
            get { return (lstFiles.Count < 1) ? null : lstFiles.ToArray(); }
        }

        /// <summary>
        /// Gets the copied file path array.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string[] CopiedFiles
        {
            get { return (lstCopiedFiles.Count < 1) ? null : lstCopiedFiles.ToArray(); }
        }

        /// <summary>
        /// Gets the selected file path array.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string[] SelectedFiles
        {
            get 
            {
                string[] files = lvList.GetSelectedValueString(1).Split(',');

                if (files == null || files.Length < 1)
                    return null;

                for (int i = 0; i < files.Length; i++)
                    files[i] = System.IO.Path.Combine(strTargetPath, files[i]);

                return files; 
            }
        }

        /// <summary>
        /// Gets or sets whether the caption is bold or normal.
        /// </summary>
        [Category("Setup")]
        public bool HighLight
        {
            get { return lblTitle.HighLight; }
            set { lblTitle.HighLight = value; }
        }

        #endregion

        #region " CREATOR "

        /// <summary>
        /// Initialize.
        /// </summary>
        public uclFileUploader()
        {
            InitializeComponent();

            
            dtList.Columns.Add("FOLDER", typeof(string));
            dtList.Columns.Add("FILE_NAME", typeof(string));
            dtList.Columns.Add("FULL_PATH", typeof(string));

            lvList.MultiSelect = true;

            dlg.FileOk += new CancelEventHandler(dlg_FileOk);
            btnAdd.Click += new EventHandler(btnAdd_Click);
            btnRemove.Click += new EventHandler(btnRemove_Click);
        }

        #endregion

        #region " EVENT "

        public event FileUploaderOpenEventHandler FileUploaderOpenClicked = null;

        #endregion

        #region " EVENT HANDLER "

        void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                dlg.InitialDirectory = strHomePath;

                dlg.Multiselect = true;
                dlg.Title = this.lblTitle.Text.Trim();

                if (dlg.ShowDialog() == DialogResult.OK)
                    AddFiles(dlg.FileNames);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void dlg_FileOk(object sender, CancelEventArgs e)
        {
            if (lstFiles.Count + dlg.FileNames.Length > iMaxCount)
            {
                MessageBox.Show(string.Format("{0}개 이하의 파일을 선택할 수 있습니다.", iMaxCount), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Cancel = true;
            }
        }

        void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (lvList.CheckedItem.Count < 1)
                    return;

                int[] indices = ((List<int>)lvList.CheckedIndices).ToArray();

                List<string> lstRemainedFIles = new List<string>();
                DataTable dtList = lvList.DataSource;

                for (int i = indices.Length - 1; i >= 0; i--)
                {
                    dtList.Rows.RemoveAt(indices[i]);
                    lvList.Items.RemoveAt(indices[i]);
                }
                lvList.CheckedItem.Clear();

                dtList.AcceptChanges();

                foreach (DataRow row in dtList.Rows)
                    lstRemainedFIles.Add(row["FULL_PATH"].ToString());

                lstFiles = lstRemainedFIles;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void uclFileUploader_Resize(object sender, EventArgs e)
        {
            if(this.Width > 30)
                lvList.Columns[1].Width = this.Width - 30; 

            //lvList.Columns[1].Width = (this.Width < 200) ? 100 : System.Convert.ToInt32(this.Width / 2);
            //lvList.Columns[2].Width = this.Width - lvList.Columns[1].Width - 30;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if(FileUploaderOpenClicked == null)
                return;

            try
            {
                if (lvList.CheckedItem.Count > 1)
                {
                    MessageBox.Show("하나의 파일만 열 수 있습니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                string [] arrPath = SelectedFiles;
                string strFilePath = (arrPath != null && arrPath.Length > 0) ? arrPath[0] : null;

                FileUploaderOpenClicked(sender, new FileUploaderOpenEventArgs(strFilePath));
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region " METHOD "

        /// <summary>
        /// Clear the file list.
        /// </summary>
        public void ClearList()
        {
            lstFiles.Clear();
            lstCopiedFiles.Clear();

            dtList.Rows.Clear();
            dtList.AcceptChanges();

            lvList.DataSource = dtList;
        }

        /// <summary>
        /// Add the file list.
        /// </summary>
        /// <param name="files">file path array</param>
        public void AddFiles(string[] files)
        {
            foreach (string file in files)
            {
                if (!string.IsNullOrEmpty(file) && !lstFiles.Contains(file))
                {
                    lstFiles.Add(file);
                    lstCopiedFiles.Add(file);
                }
            }

            dtList.Rows.Clear();
            dtList.AcceptChanges();

            foreach (string file in lstFiles)
                dtList.Rows.Add(new object[] { Path.GetDirectoryName(file), Path.GetFileName(file), file });

            lvList.DataSource = dtList;
        }

        /// <summary>
        /// Upload files and return the result.
        /// </summary>
        /// <returns>result type</returns>
        public UploadingResultItems Upload()
        {
            string strReturnMessage = string.Empty;
            string strFileName = string.Empty;

            StringBuilder sb = null;

            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (lstFiles == null || lstFiles.Count < 1)
                    return UploadingResultItems.NoFileExists;

                lstCopiedFiles.Clear();

                if (bOverwrite)
                {
                    for (int i = 0; i < lstFiles.Count; i++)
                    {
                        strFileName = Path.Combine(strTargetPath, Path.GetFileName(lstFiles[i]));

                        try
                        {
                            File.Copy(lstFiles[i], strFileName, true);
                        }
                        catch
                        {
                            if (sb == null)
                                sb = new StringBuilder();

                            sb.AppendFormat(", {0}", Path.GetFileName(lstFiles[i]));
                        }

                        lstCopiedFiles.Add(strFileName);
                    }

                    if (sb.Length > 0)
                    {
                        MessageBox.Show(string.Format("파일이 이미 사용중이거나 접근이 불가능하여 다음 파일(들)을 덮어쓰지 못하였습니다. ({0})", sb.ToString(2, sb.Length-2)), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    if (!Directory.Exists(strTargetPath))
                    {
                        try
                        {
                            Directory.CreateDirectory(strTargetPath);
                        }
                        catch //(Exception ex)
                        {
                            return UploadingResultItems.CreateDirectoryDenied;
                        }
                    }

                    for (int i = 0; i < lstFiles.Count; i++)
                    {
                        strFileName = Path.GetFileName(lstFiles[i]);

                        if (File.Exists(Path.Combine(strTargetPath, strFileName)))
                        {
                            int iCnt = 1;
                            strFileName = GetAlteredName(strTargetPath, strFileName, iCnt);
                        }
                        else
                            strFileName = Path.Combine(strTargetPath, strFileName);

                        File.Copy(lstFiles[i], strFileName);
                        lstCopiedFiles.Add(strFileName);
                    }
                }

                return UploadingResultItems.Success;
            }
            catch
            {
                return UploadingResultItems.Error;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private string GetAlteredName(string strTargetPath, string strFileName, int iCnt)
        {
            string strName = System.IO.Path.GetFileNameWithoutExtension(strFileName);
            string strExtension = System.IO.Path.GetExtension(strFileName);

            string strTempName = Path.Combine(strTargetPath, strName + "_" + iCnt.ToString() + strExtension);

            if (File.Exists(strTempName))
                return GetAlteredName(strTargetPath, strFileName, iCnt + 1);
            else
                return strTempName;
        }

        #endregion
    }

    #region " Delegate "

    public delegate void FileUploaderOpenEventHandler(object sender, FileUploaderOpenEventArgs args);

    #endregion

    #region " Inner Class "

    /// <summary>
    /// Event argument Class.
    /// </summary>
    public class FileUploaderOpenEventArgs : EventArgs
    {
        private string strFile = string.Empty;

        public string FilePath
        {
            get { return strFile; }
        }

        public FileUploaderOpenEventArgs(string filePath)
        {
            this.strFile = filePath;
        }
    }

    #endregion
}
