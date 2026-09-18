/*----------------------------------------------------------------------------------------------------------
--  상기 프로그램에 대한 저작권을 포함한 지적재산권은 (주)미라콤아이앤씨에 있으며, (주)미라콤아이앤씨가
--  명시적으로 허용하지 않은 사용, 복사, 변경, 제3자에의 공개, 배포는 엄격히 금지되며, (주)미라콤아이앤씨의
--  지적재산권 침해에 해당됩니다.
--  (Copyright ⓒ 2014 Miracom Inc. All Rights Reserved | Confidential)
--
--  Program Id      : frmSystemOption.cs
--  Creator         : YoungShin Lim (YSIM)
--  Create Date     : 2014.12.25
--  Description     : DACrux V5 System Option Dialog
--  History         : Created by YSIM at 2014.12.25
 * ********************************************************************************************************
----------------------------------------------------------------------------------------------------------*/
using System;
using System.Data;
using System.Windows.Forms;

namespace DACrux.Framework
{
    public partial class frmSystemOption : DACrux.Framework.Base.DACruxUXBasic00
    {

        #region Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public frmSystemOption()
        {
            InitializeComponent();
        }

        #endregion


        #region Button Click Event

        #region btnSave_Click

        /// <summary>
        /// Save User Information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DACrux.Base.GlobalVariable.Language = cmbLaguage.Text;
                DACrux.Base.GlobalVariable.EAIServerIP = txtH101ServerIP.Text;
                DACrux.Base.GlobalVariable.EAIServerPort = txtH101ServerPort.Text;
                DACrux.Base.GlobalVariable.EAIChannel = txtH101ServerChannel.Text;
                DACrux.Base.GlobalVariable.SaveGlobalVariable();
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        #endregion

        #region btnClose_Click

        /// <summary>
        /// Exit Function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        #endregion


        #endregion

        private void frmSystemOption_Load(object sender, EventArgs e)
        {
            try
            {
                cmbLaguage.Items.Clear();
                cmbLaguage.DataSource = DACrux.Base.MultiLanguage.LanguageList;
                cmbLaguage.Text = DACrux.Base.GlobalVariable.Language;


                txtAppServer.Text = DACrux.Base.GlobalVariable.ServerIP;
                txtH101ServerIP.Text = DACrux.Base.GlobalVariable.EAIServerIP;
                txtH101ServerPort.Text = DACrux.Base.GlobalVariable.EAIServerPort;
                txtH101ServerChannel.Text = DACrux.Base.GlobalVariable.EAIChannel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        private void cmbLaguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblTitle.Text = "     " + DACrux.Base.MultiLanguage.Translation("System Option");
                lblLaguage.Text = "     " + DACrux.Base.MultiLanguage.Translation("Language");
                lblAppServerIP.Text = "     " + DACrux.Base.MultiLanguage.Translation("Server IP");

                lblServerIP.Text = "     " + DACrux.Base.MultiLanguage.Translation("Server IP");
                lblServerPort.Text = "     " + DACrux.Base.MultiLanguage.Translation("Server Port");
                lblServerChannel.Text = "     " + DACrux.Base.MultiLanguage.Translation("Channel");

                grpAppServer.Text = DACrux.Base.MultiLanguage.Translation("Application Server");

            }catch(Exception ex)
            {
                DspError(ex);
            }
        }

        private void cmbLaguage_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                DACrux.Base.GlobalVariable.Language = cmbLaguage.Text;
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }
    }
}