using System;
using System.Windows.Forms;

namespace DACrux.Framework.Controls
{
    /// <summary>
    /// Class Name : frmErrorBox<br/>
    /// Summary    : DACrux Common Error Message Display Form Class<br/>
    /// Author     : Miracom David, Kwak<br/>
    /// First Date : 2009-02-23<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    internal partial class frmRetestMsgBoxControl : Form
    {
        #region Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        /// <param name="strMessage">Display Error Text</param>
        public frmRetestMsgBoxControl(string strMessage)
        {
            InitializeComponent();
            // DACrux.Base.MultiLanguage.Translation(this.Controls);

            this.DialogResult = System.Windows.Forms.DialogResult.OK;

            rtxtMessage.Text = strMessage;
            string[] Msg = new string[] { "현재 LOT ID로 입력한 데이터가  이미 있으니 Retetst 진행한 데이터인지 확인하세요.\n\n",
                                          "신규 데이터는 '신규', Retest 데이터는 '재입력' 클릭 !!!\n\n",
                                          "'재입력' 버튼을 누를 경우, 현재 데이터가 이전 입력 데이터를 대체합니다.\n\n",
                                          " - 상세정보\n{0}" };

            System.Drawing.Font orgFont = new System.Drawing.Font(this.Font.Name, this.Font.Size, this.Font.Style, this.Font.Unit); 
            System.Drawing.Font Big = new System.Drawing.Font(this.Font.Name, this.Font.Size + 2, System.Drawing.FontStyle.Bold, this.Font.Unit);

            rtxtMessage.Text = string.Empty;

            rtxtMessage.SelectionFont = orgFont;
            rtxtMessage.SelectedText = Msg[0];

            rtxtMessage.SelectionFont = Big;
            rtxtMessage.SelectedText = Msg[1];

            rtxtMessage.SelectionFont = orgFont;
            rtxtMessage.SelectedText = Msg[2];
            rtxtMessage.SelectedText = string.Format(Msg[3], strMessage);
        }

        #endregion

        private void btnRetest_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Retry;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        //2013 10 30 hcsim 엔터키 누를시 자동 신규버튼 클릭
        private void frmRetestMsgBoxControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }

        #region Button Event

        #endregion

    }
}