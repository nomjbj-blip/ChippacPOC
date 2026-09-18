using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace DACrux.Mining
{
	/// <summary>
	/// uclLotGroup에 대한 요약 설명입니다.
	/// </summary>
    public partial class uclLotGroup : System.Windows.Forms.UserControl
	{
		public uclLotGroup()
		{
			
            try
            {
                // 이 호출은 Windows.Forms Form 디자이너에 필요합니다.
                InitializeComponent();

                // TODO: InitializeComponent를 호출한 다음 초기화 작업을 추가합니다.

                if (System.IO.File.Exists("LotGroup.XML"))
                {
                    DataTable dt = new DataTable("Lot Group");
                    dt.Columns.Add("GROUP_NAME", typeof(string));
                    dt.Columns.Add("LOT_LIST", typeof(string));

                    dt.ReadXml("LotGroup.XML");

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        treeView1.Nodes.Add(dt.Rows[i]["GROUP_NAME"].ToString());
                        string[] LotList = dt.Rows[i]["LOT_LIST"].ToString().Split(',');

                        for(int n = 0; n < LotList.Length; n++)
                        {
                            treeView1.Nodes[i].Nodes.Add(LotList[n]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DACrux.Base.MsgHelper.DspError(ex.Message);
            }
		}

        public void SaveXmlLotGroup()
        {
            try
            {
                DataTable dt = SaveLotGroup();

                if (dt != null && dt.Rows.Count > 0)
                {
                    dt.WriteXml("LotGroup.XML");
                }
            }
            catch (Exception ex)
            {
                DACrux.Base.MsgHelper.DspError(ex.Message);
            }
        }

        public DataTable SaveLotGroup()
        {
            try
            {
                DataTable dt = new DataTable("Lot Group");
                dt.Columns.Add("GROUP_NAME", typeof(string));
                dt.Columns.Add("LOT_LIST", typeof(string));

                string sGroupName = string.Empty;
                string sLotList = string.Empty;

                for (int i = 0; i < treeView1.Nodes.Count; i++)
                {
                    sGroupName = treeView1.Nodes[i].Text;

                    for (int n = 0; n < treeView1.Nodes[i].Nodes.Count; n++)
                    {
                        sLotList += "," + treeView1.Nodes[i].Nodes[n].Text;
                    }

                    DataRow dr = dt.NewRow();

                    dr["GROUP_NAME"] = sGroupName;
                    dr["LOT_LIST"] = sLotList.Substring(1);

                    dt.Rows.Add(dr);
                    sLotList = string.Empty;
                }

                if (dt == null || dt.Rows.Count < 1)
                    return null;

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void txtGroupName_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                    butAddGroup_Click(sender, e);
            }
            catch (Exception ex)
            {
                DACrux.Base.MsgHelper.DspError(ex.Message);
            }
        }

		private void butAddGroup_Click(object sender, System.EventArgs e)
		{
            try
            {
                if (txtGroupName.Text.Trim().Length == 0) return;
                for (int i = 0; i < treeView1.Nodes.Count; i++)
                {
                    if (treeView1.Nodes[i].Text == txtGroupName.Text)
                    {
                        MessageBox.Show("지정한 Group을 이미 사용중입니다.");
                        return;
                    }
                }

                treeView1.Nodes.Add(txtGroupName.Text);
                txtGroupName.Text = string.Empty;
            }
            catch (Exception ex)
            {
                DACrux.Base.MsgHelper.DspError(ex.Message);
            }
		}

        #region [ TreeView Event ]

        private void treeView1_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            try
            {
                e.Effect = DragDropEffects.Move;
            }
            catch (Exception ex)
            {
                DACrux.Base.MsgHelper.DspError(ex.Message);
            }
        }

        private void treeView1_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            string[] strRecive = null;
            try
            {
                strRecive = (string[])e.Data.GetData(System.Type.GetType("System.String[]"));

                AddLotID(strRecive);

                //switch (strRecive[0])
                //{
                //    case "LOT_ID":
                //        AddLotID(strRecive);
                //        break;
                //    case "WAFER_ID":
                //        AddWaferID(strRecive);
                //        break;
                //    case "DAY":
                //        MessageBox.Show("Key가 Date이므로 Groupping할 수 없습니다.");

                //        break;
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void treeView1_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
        {
            try
            {
                Point cp = treeView1.PointToClient(new Point(e.X, e.Y));

                //Console.WriteLine(string.Format("{0},{1}",cp.X,cp.Y));

                TreeNode d = treeView1.GetNodeAt(cp);
                if (d == null) return;
                m_strSelectedGroup = d.Text;

                for (int i = 0; i < treeView1.Nodes.Count; i++)
                {
                    if (treeView1.Nodes[i].Text == m_strSelectedGroup)
                    {
                        m_iSelectedGroupNodeIndex = i;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                DACrux.Base.MsgHelper.DspError(ex.Message);
            }
        }

        private void treeView1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    ctxMenu.Show(treeView1, new Point(e.X, e.Y));
                }
            }
            catch (Exception ex)
            {
                DACrux.Base.MsgHelper.DspError(ex.Message);
            }
        }

        private void treeView1_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {

            try
            {
                TreeNode d = e.Node.Parent;
                if (d == null)
                {
                    m_strSelectedGroup = e.Node.Text;
                    m_strSelectedLot = string.Empty;
                }
                else
                {
                    m_strSelectedGroup = d.Text;
                    m_strSelectedLot = e.Node.Text;
                }

                for (int i = 0; i < treeView1.Nodes.Count; i++)
                {
                    if (treeView1.Nodes[i].Text == m_strSelectedGroup)
                    {
                        m_iSelectedGroupNodeIndex = i;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                DACrux.Base.MsgHelper.DspError(ex.Message);
            }
        } 

        #endregion

        #region [ ContextMenu ]

        private void mnuAddLot_Click(object sender, System.EventArgs e)
        {
            frmAddLot oAddLot = null;
            try
            {
                oAddLot = new frmAddLot();
                if (oAddLot.ShowDialog(this) == DialogResult.OK)
                {
                    AddLotID(new string[] { oAddLot.GetLotID() });
                    //treeView1.Nodes[m_iSelectedGroupNodeIndex].Nodes.Add(oAddLot.GetLotID());
                }
            }
            catch (Exception ex)
            {
                DACrux.Base.MsgHelper.DspError(string.Format("Lot Add를 할 수 없습니다.[Err:{0}]", ex.Message));
            }
        }

        private void mnuDelLot_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (m_iSelectedGroupNodeIndex == -1) return;

                TreeNode d = treeView1.Nodes[m_iSelectedGroupNodeIndex];
                for (int i = 0; i < d.Nodes.Count; i++)
                {
                    if (d.Nodes[i].Text == m_strSelectedLot)
                    {
                        d.Nodes.RemoveAt(i);
                    }
                }
            }
            catch (Exception ex)
            {
                DACrux.Base.MsgHelper.DspError(string.Format("Delete Lot을 할 수 없습니다.[Err:{0}]", ex.Message));
            }
        }

        private void mnuDelGroup_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (m_iSelectedGroupNodeIndex == -1) return;
                treeView1.Nodes.RemoveAt(m_iSelectedGroupNodeIndex);
            }
            catch (Exception ex)
            {
                DACrux.Base.MsgHelper.DspError(string.Format("Delete Group을 할 수 없습니다.[Err:{0}]", ex.Message));
            }
        }

        private void mnuResetAll_Click(object sender, System.EventArgs e)
        {
            try
            {
                treeView1.Nodes.Clear();
            }
            catch (Exception ex)
            {
                DACrux.Base.MsgHelper.DspError(string.Format("트리를 Reset할 수 없습니다.[Err:{0}]", ex.Message));
            }
        } 

        #endregion

        private void AddLotID(string[] Recive)
        {
            try
            {
                bool bExist = false;
                for (int i = 0; i < Recive.Length; i++)
                {
                    bExist = false;
                    for (int n = 0; n < treeView1.Nodes[m_iSelectedGroupNodeIndex].Nodes.Count; n++)
                    {
                        if (treeView1.Nodes[m_iSelectedGroupNodeIndex].Nodes[n].Text == Recive[i])
                        {
                            bExist = true;
                            break;
                        }
                    }

                    if (bExist) continue;
                    treeView1.Nodes[m_iSelectedGroupNodeIndex].Nodes.Add(Recive[i]);
                }
                treeView1.Nodes[m_iSelectedGroupNodeIndex].Expand();

                //bool bExist = false;
                //for (int i = 1; i < Recive.Length; i++)
                //{
                //    bExist = false;
                //    for (int n = 0; n < treeView1.Nodes.Count; n++)
                //    {
                //        if (treeView1.Nodes[n].Nodes.IndexOf(new TreeNode(Recive[i])) > -1)
                //        {
                //            bExist = true;
                //            break;
                //        }
                //    }

                //    if (bExist) continue;
                //    treeView1.Nodes[m_iSelectedGroupNodeIndex].Nodes.Add(Recive[i]);
                //}
                //treeView1.Nodes[m_iSelectedGroupNodeIndex].Expand();
            }
            catch (Exception ex)
            {
                DACrux.Base.MsgHelper.DspError(ex.Message);
            }
        }

        private void AddWaferID(string[] Recive)
        {
            try
            {
                ArrayList arrDist = new ArrayList();
                string[] tmpLotID = null;
                for (int i = 1; i < Recive.Length; i++)
                {
                    tmpLotID = Recive[i].Split('-');
                    if (arrDist.IndexOf(tmpLotID[0]) > -1) continue;

                    arrDist.Add(tmpLotID[0]);
                    treeView1.Nodes[m_iSelectedGroupNodeIndex].Nodes.Add(tmpLotID[0]);
                }
            }
            catch (Exception ex)
            {
                DACrux.Base.MsgHelper.DspError(ex.Message);
            }
        }

		public string[] GetListInGroup(string GroupID)
		{
			TreeNode gNode = null;
			string[] oRtn = null;
			try
			{
				for(int i=0;i<treeView1.Nodes.Count;i++)
				{
					if(treeView1.Nodes[i].Text == GroupID)
					{
						gNode = treeView1.Nodes[i];
						break;
					}
				}

				if(gNode == null) return null;

				oRtn = new string[gNode.Nodes.Count];
				for(int i=0;i<gNode.Nodes.Count;i++)
				{
					oRtn[i] = gNode.Nodes[i].Text;
				}
				return oRtn;
				
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		public string[] GetGroupList()
		{
			string[] oRtn = null;
			if(treeView1.Nodes.Count==0) return null;
			try
			{
				oRtn = new string[treeView1.Nodes.Count];
				for(int i=0;i<treeView1.Nodes.Count;i++)
				{
					oRtn[i] = treeView1.Nodes[i].Text;
				}
				return oRtn;
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
	}
}
