/*----------------------------------------------------------------------------------------------------------
--  상기 프로그램에 대한 저작권을 포함한 지적재산권은 (주)미라콤아이앤씨에 있으며, (주)미라콤아이앤씨가
--  명시적으로 허용하지 않은 사용, 복사, 변경, 제3자에의 공개, 배포는 엄격히 금지되며, (주)미라콤아이앤씨의
--  지적재산권 침해에 해당됩니다.
--  (Copyright ⓒ 2014 Miracom Inc. All Rights Reserved | Confidential)
--
--  Program Id      : IniHandle.cs
--  Creator         : YoungShin Lim (YSIM)
--  Create Date     : 2014.11.14
--  Description     : DACrux Framework::DACrux 의 Login Window
--  History         : Created by YSIM at 2014.11.14
 * ********************************************************************************************************
 * 2014 년 DACrux V4 History Reset

----------------------------------------------------------------------------------------------------------*/

using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DACrux.Framework.Controls
{
    public static class WithThread
    {
        public static void EnabledDUCItemSelector(DACrux.Framework.Controls.DUCItemSelector selector, bool IsEnabled)
        {
            if (selector.Parent == null || selector.Parent.Disposing)
            {
                return;
            }

            if (selector.InvokeRequired)
            {
                selector.BeginInvoke(new MethodInvoker(
                    delegate()
                    {
                        EnabledDUCItemSelector(selector, IsEnabled);
                    }
                    ));
            }
            else
            {
                selector.Enabled = IsEnabled;
            }
        }

        public static void SetDUCItemSelector(DACrux.Framework.Controls.DUCItemSelector selector, DataTable dt)
        {
            if (selector.Parent == null || selector.Parent.Disposing)
            {
                return;
            }

            if (selector.InvokeRequired)
            {
                selector.BeginInvoke(new MethodInvoker(
                    delegate()
                    {
                        SetDUCItemSelector(selector, dt);
                    }
                    ));
            }
            else
            {
                selector.DataSource = dt;
            }
        }

        //public static void SetSpread(FarPoint.Win.Spread.FpSpread spread, DataTable dt)
        //{
        //    if (spread.Parent == null || spread.Parent.Disposing)
        //    {
        //        return;
        //    }

        //    if (spread.InvokeRequired)
        //    {
        //        spread.BeginInvoke(new MethodInvoker(
        //            delegate()
        //            {
        //                SetSpread(spread, dt);
        //            }
        //            ));
        //    }
        //    else
        //    {
        //        DACrux.Framework.Utillity.SetSpreadData(dt, spread);
        //    }
        //}

        public static void SetTextBox(TextBox txtbox, string strMsg)
        {
            if (txtbox.Parent.Disposing)
            {
                return;
            }

            if (txtbox.InvokeRequired)
            {
                txtbox.BeginInvoke(new MethodInvoker(
                    delegate()
                    {
                        SetTextBox(txtbox, strMsg);
                    }
                    ));
            }
            else
            {
                txtbox.Text = strMsg;
            }
        }

        public static void SetRadioButton(RadioButton rdobox, bool IsCheck)
        {
            if (rdobox.Parent.Disposing)
            {
                return;
            }

            if (rdobox.InvokeRequired)
            {
                rdobox.BeginInvoke(new MethodInvoker(
                    delegate()
                    {
                        SetRadioButton(rdobox, IsCheck);
                    }
                    ));
            }
            else
            {
                rdobox.Checked = IsCheck;
            }
        }

        public static void VisibleTextBox(TextBox txtbox, bool IsVisible)
        {
            if (txtbox.Parent.Disposing)
            {
                return;
            }

            if (txtbox.InvokeRequired)
            {
                txtbox.BeginInvoke(new MethodInvoker(
                    delegate()
                    {
                        VisibleTextBox(txtbox, IsVisible);
                    }
                    ));
            }
            else
            {
                txtbox.Visible = IsVisible;
            }
        }

        public static void EnableGroupBox(GroupBox grpbox, bool IsEnable)
        {
            if (grpbox.Parent.Disposing)
            {
                return;
            }

            if (grpbox.InvokeRequired)
            {
                grpbox.BeginInvoke(new MethodInvoker(
                    delegate()
                    {
                        EnableGroupBox(grpbox, IsEnable);
                    }
                    ));
            }
            else
            {
                grpbox.Enabled = IsEnable;
            }
        }


        #region ListView
        public static void AddListView(ListView lstView, ListViewItem ItemData)
        {
            if (lstView.Parent.Disposing)
            {
                return;
            }

            if (lstView.InvokeRequired)
            {
                lstView.BeginInvoke(new MethodInvoker(
                    delegate()
                    {
                        AddListView(lstView, ItemData);
                    }
                    ));
            }
            else
            {
                if (lstView.Items.Count == 200) lstView.Items.RemoveAt(0);
                lstView.Items.Add(ItemData);
            }
        }

        public static void RemoveAtListView(ListView lstView, int Index)
        {
            if (lstView.Parent.Disposing)
            {
                return;
            }

            if (lstView.InvokeRequired)
            {
                lstView.BeginInvoke(new MethodInvoker(
                    delegate()
                    {
                        RemoveAtListView(lstView, Index);
                    }
                    ));
            }
            else
            {
                lstView.Items.RemoveAt(Index);
            }
        }

        public static void RemoveListView(ListView lstView, ListViewItem ItemData)
        {
            if (lstView.Parent.Disposing)
            {
                return;
            }

            if (lstView.InvokeRequired)
            {
                lstView.BeginInvoke(new MethodInvoker(
                    delegate()
                    {
                        RemoveListView(lstView, ItemData);
                    }
                    ));
            }
            else
            {
                lstView.Items.Remove(ItemData);
            }
        }
        #endregion


        public static void SetLabel(Label Label, string strMsg)
        {
            if (Label.Parent.Disposing)
            {
                return;
            }

            if (Label.InvokeRequired)
            {
                Label.BeginInvoke(new MethodInvoker(
                    delegate()
                    {
                        SetLabel(Label, strMsg);
                    }
                    ));
            }
            else
            {
                Label.Text = strMsg;
            }
        }

        public static void SetButtonImage(Button button, Image img)
        {
            if (button.Parent.Disposing)
            {
                return;
            }

            if (button.InvokeRequired)
            {
                button.BeginInvoke(new MethodInvoker(
                    delegate()
                    {
                        SetButtonImage(button, img);
                    }
                    ));
            }
            else
            {
                button.Image = img;
            }
        }

        #region ListBox

        public static void AddListBox(ListBox lstBox, string ItemData)
        {
            if (lstBox.Parent.Disposing)
            {
                return;
            }

            if (lstBox.InvokeRequired)
            {
                lstBox.BeginInvoke(new MethodInvoker(
                    delegate()
                    {
                        AddListBox(lstBox, ItemData);
                    }
                    ));
            }
            else
            {
                lstBox.Items.Add(ItemData);
            }
        }

        public static void AddListBox(ListBox lstBox, string ItemData, int nLimitCount)
        {
            if (lstBox.Parent.Disposing)
            {
                return;
            }

            if (lstBox.InvokeRequired)
            {
                lstBox.BeginInvoke(new MethodInvoker(
                    delegate()
                    {
                        AddListBox(lstBox, ItemData);
                    }
                    ));
            }
            else
            {
                if (lstBox.Items.Count == nLimitCount) lstBox.Items.RemoveAt(0);
                lstBox.Items.Add(ItemData);
            }
        }

        public static void RemoveAtListBox(ListBox lstBox, int Index)
        {
            if (lstBox.Parent.Disposing)
            {
                return;
            }

            if (lstBox.InvokeRequired)
            {
                lstBox.BeginInvoke(new MethodInvoker(
                    delegate()
                    {
                        RemoveAtListBox(lstBox, Index);
                    }
                    ));
            }
            else
            {
                lstBox.Items.RemoveAt(Index);
            }
        }

        public static void RemoveListBox(ListBox lstBox, string ItemData)
        {
            if (lstBox.Parent.Disposing)
            {
                return;
            }

            if (lstBox.InvokeRequired)
            {
                lstBox.BeginInvoke(new MethodInvoker(
                    delegate()
                    {
                        RemoveListBox(lstBox, ItemData);
                    }
                    ));
            }
            else
            {
                lstBox.Items.Remove(ItemData);
            }
        }

        public static void ClearListBox(ListBox lstBox)
        {
            if (lstBox.Parent.Disposing)
            {
                return;
            }

            if (lstBox.InvokeRequired)
            {
                lstBox.BeginInvoke(new MethodInvoker(
                    delegate()
                    {
                        ClearListBox(lstBox);
                    }
                    ));
            }
            else
            {
                lstBox.Items.Clear();
            }
        }

        #endregion

    }
}
