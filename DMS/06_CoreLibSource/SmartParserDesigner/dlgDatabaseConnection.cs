using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Security.Cryptography;
using DACrux.SP.Common;

namespace SmartParser.Designer
{
    public partial class dlgDatabaseConnection : Form
    {
        #region " MEMBER FIELD "

        List<ConnectionInformation> lstConnInfo = new List<ConnectionInformation>();
        Hashtable htConninfo = new Hashtable();
        DataTable dtResult = new DataTable();
        System.Threading.Thread thread;

        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        #endregion

        #region " PROPERTY "

        public DataTable ResultDatatable
        {
            get { return dtResult; }
            set { dtResult = value; }
        }

        public ConnectionInformation ConnectionInfo
        {
            get { return GetConnInfo(); }
            set { SetConnInfo(value); }
        }

        #endregion

        #region " EVENT "

        private delegate void ThreadStateChangedHandler(bool isStart);
        private event ThreadStateChangedHandler ThreadStateChanged;

        #endregion

        #region " CREATOR "

        public dlgDatabaseConnection()
        {
            DACrux.SP.Common.MultiLang funclang = new MultiLang();

            funclang.CheckLang();
            InitializeComponent();
        }

        #endregion

        #region " METHOD "

        private void SetConnInfo(ConnectionInformation connInfo)
        {
            cboServer.Text = connInfo.ServerType.ToString();
            rbtIP.Checked = (connInfo.HostType == HostType.IP) ? true : false;
            ipaHost.Enabled = rbtIP.Checked;
            txtHost.Enabled = !rbtIP.Checked;
            ipaHost.Text = connInfo.HostIP;
            txtHost.Text = connInfo.HostName;
            txtDatabaseName.Text = connInfo.DatabaseName;
            txtPort.Text = connInfo.Port;
            txtUserID.Text = connInfo.UserID;
            txtPassword.Text = connInfo.Password;
        }

        private void EditConnInfo(ConnectionInformation connInfo)
        {
            connInfo.ServerType = (ServerType)Enum.Parse(typeof(ServerType), cboServer.Text, true);
            connInfo.HostType = (rbtIP.Checked) ? HostType.IP : HostType.Alias;
            connInfo.HostIP = ipaHost.Text;
            connInfo.HostName = txtHost.Text;
            connInfo.DatabaseName = txtDatabaseName.Text;
            connInfo.Port = txtPort.Text;
            connInfo.UserID = txtUserID.Text;
            connInfo.Password = txtPassword.Text;
        }

        private ConnectionInformation GetConnInfo()
        {
            ConnectionInformation connInfo = new ConnectionInformation();

            try
            {
                connInfo.ServerType = (ServerType)Enum.Parse(typeof(ServerType), cboServer.Text, true);
                connInfo.HostType = (rbtIP.Checked) ? HostType.IP : HostType.Alias;
                connInfo.HostIP = ipaHost.Text;
                connInfo.HostName = txtHost.Text;
                connInfo.DatabaseName = txtDatabaseName.Text;
                connInfo.Port = txtPort.Text;
                connInfo.UserID = txtUserID.Text;
                connInfo.Password = txtPassword.Text;
            }
            catch
            {
                return null;
            }

            return connInfo;
        }

        private bool CheckValidInput()
        {
            if (ipaHost.Text.Trim() == string.Empty && txtHost.Text.Trim() == string.Empty)
                return false;

            if (txtDatabaseName.Text.Trim() == string.Empty)
                return false;

            if (txtPort.Text.Trim() == string.Empty)
                return false;

            if (txtUserID.Text.Trim() == string.Empty)
                return false;

            if (txtPassword.Text.Trim() == string.Empty)
                return false;

            return true;
        }

        private void LoadConnectionList()
        {
            ListViewItem lvItem;
            BinaryFormatter bf;
            FileStream fs = null;
            CryptoStream cs = null;
            object obj;
            FileInfo fi;

            try
            {
                fi = new FileInfo(Path.Combine(Application.StartupPath, "Connection.dat"));

                if (!fi.Exists)
                    return;

                bf = new BinaryFormatter();
                fs = File.Open(fi.FullName, FileMode.Open);

                int iKey = fs.ReadByte();
                byte[] keys = new byte[iKey];
                for (int i = 0; i < iKey; i++)
                    keys[i] = Convert.ToByte(fs.ReadByte());

                int iIV = fs.ReadByte();
                byte[] IV = new byte[iIV];
                for (int i = 0; i < iIV; i++)
                    IV[i] = Convert.ToByte(fs.ReadByte());

                Rijndael alg = Rijndael.Create();
                cs = new CryptoStream(fs, alg.CreateDecryptor(keys, IV), CryptoStreamMode.Read);

                if (fs.Length > 0)
                {
                    obj = bf.Deserialize(cs);

                    lstConnInfo = (List<ConnectionInformation>)obj;

                    foreach (ConnectionInformation ci in lstConnInfo)
                    {
                        lvItem = new ListViewItem(ci.Name);
                        lvItem.Name = ci.Name;
                        htConninfo.Add(lvItem, ci);
                        htConninfo.Add(ci, lvItem);

                        lvConnection.Items.Add(lvItem);
                    }
                }
            }
            catch//(Exception ex)
            {
                //MessageBox.Show("Loading failed.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                if (cs != null)
                    cs.Close();
                if (fs != null)
                    fs.Close();
            }
        }

        private void SaveConnectionList()
        {
            BinaryFormatter bf;
            FileStream fs = null;
            CryptoStream cs = null;
            FileInfo fi;

            try
            {
                lstConnInfo.Clear();
                foreach (ListViewItem lvItem in lvConnection.Items)
                {
                    lstConnInfo.Add((ConnectionInformation)htConninfo[lvItem]);
                }

                fi = new FileInfo(Path.Combine(Application.StartupPath, "Connection.dat"));

                if (fi.Exists)
                    fi.Delete();

                bf = new BinaryFormatter();
                Rijndael alg = Rijndael.Create();

                fs = File.Open(fi.FullName, FileMode.Create);

                fs.WriteByte(Convert.ToByte(alg.Key.Length));
                foreach (byte value in alg.Key)
                    fs.WriteByte(value);

                fs.WriteByte(Convert.ToByte(alg.IV.Length));
                foreach (byte value in alg.IV)
                    fs.WriteByte(value);

                fs.Close();

                fs = File.Open(fi.FullName, FileMode.Append);
                cs = new CryptoStream(fs, alg.CreateEncryptor(alg.Key, alg.IV), CryptoStreamMode.Write);
                bf.Serialize(cs, lstConnInfo);
            }
            catch
            {
                MessageBox.Show("Failed to save.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                if (cs != null)
                    cs.Close();
                if (fs != null)
                    fs.Close();
            }
        }

        private void ProcessTestConnection(object param)
        {
            ConnectionInformation connInfo = (ConnectionInformation)((object[])param)[0];
            string strQuery = (string)((object[])param)[1];

            switch (connInfo.ServerType)
            {
                case ServerType.Oracle:
                    dtResult = OracleConnector.GetDataTable(connInfo, strQuery);
                    break;
            }
        }

        private void ProcessQuery(object param)
        {
            ConnectionInformation connInfo = (ConnectionInformation)((object[])param)[0];
            string strQuery = (string)((object[])param)[1];
            bool isTest = (bool)((object[])param)[2];

            switch (connInfo.ServerType)
            {
                case ServerType.Oracle:
                    dtResult = OracleConnector.GetDataTable(connInfo, strQuery);
                    break;
            }

            if (ThreadStateChanged != null)
                ThreadStateChanged(false);

            if (!isTest)
            {
                if (dtResult == null || dtResult.Rows.Count < 1)
                {
                    MessageBox.Show("No data received.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    this.DialogResult = DialogResult.OK;
            }
        }

        #endregion

        #region " EVENT HANDLER "

        private void DlgOracleConnection_Load(object sender, EventArgs e)
        {
            try
            {
                timer.Interval = 10000;
                timer.Tick += new EventHandler(timer_Tick);

                rbtIP.Checked = true;
                ipaHost.Enabled = rbtIP.Checked;
                txtHost.Enabled = !rbtIP.Checked;

                SetServerType();
                cboServer.SelectedIndex = 0;
                txtPort.Text = "1521";

                LoadConnectionList();

                if (lvConnection.Items.Count > 0)
                {
                    lvConnection.Focus();
                    lvConnection.Items[0].Selected = true;
                }

                //ThreadStateChanged += new ThreadStateChangedHandler(OnThreadStateChanged);
            }
            catch
            {
                MessageBox.Show("Loading connection information file failed.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SetServerType()
        {
            foreach (string serverType in Enum.GetNames(typeof(ServerType)))
                cboServer.Items.Add(serverType);
        }

        private void cboServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboServer.SelectedText == ServerType.Oracle.ToString())
                txtPort.Text = "1521";
        }

        private void rbtIP_CheckedChanged(object sender, EventArgs e)
        {
            rbtName.Checked = !rbtIP.Checked;
            ipaHost.Enabled = rbtIP.Checked;
            txtHost.Enabled = !rbtIP.Checked;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            btnTest.Enabled = false;

            string strOraClientPath = string.Empty;
            ConnectionInformation connInfo = null;
            object[] param = null;

            try
            {
                strOraClientPath = Application.StartupPath;


                if (!CheckValidInput())
                {
                    MessageBox.Show("Input all information about the connection.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //try
                //{
                //    if (Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("ORACLE").SubKeyCount < 1)
                //        throw new Exception();
                //}
                //catch
                //{
                //    if (!Environment.GetEnvironmentVariable("Path").Contains(strOraClientPath))
                //        Environment.SetEnvironmentVariable("Path", Environment.ExpandEnvironmentVariables("%Path%;") + strOraClientPath, EnvironmentVariableTarget.Machine);
                //}

                connInfo = GetConnInfo();
                param = new object[] { connInfo, "SELECT SYSDATE FROM DUAL" };

                thread = new Thread(new ParameterizedThreadStart(ProcessTestConnection));
                thread.Start(param);

                if (!thread.Join(5000))
                {
                    thread.Abort();
                    MessageBox.Show("Connection failed. \r\nPlease check your network connection is available \r\nand server which you are trying to connect has no problem.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (dtResult != null)
                    MessageBox.Show("Connection successed.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnTest.Enabled = true;
                dtResult = null;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ConnectionInformation connInfo;

            if (txtConnectionName.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Input the connection name.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!CheckValidInput())
            {
                MessageBox.Show("Input all information about the connection.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (lvConnection.Items.ContainsKey(txtConnectionName.Text.Trim()))
            {
                connInfo = (ConnectionInformation)htConninfo[lvConnection.Items[txtConnectionName.Text.Trim()]];
                EditConnInfo(connInfo);
            }
            else
            {
                ListViewItem lvItem = new ListViewItem(txtConnectionName.Text.Trim());
                lvItem.Name = txtConnectionName.Text.Trim();
                lvConnection.Items.Add(lvItem);

                connInfo = GetConnInfo();
                connInfo.Name = lvItem.Name;
                htConninfo.Add(lvItem, connInfo);
                htConninfo.Add(connInfo, lvItem);
            }

            SaveConnectionList();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lvConnection.SelectedItems.Count < 1)
                return;

            ConnectionInformation connInfo;
            ListViewItem lvItem;

            lvItem = lvConnection.SelectedItems[0];
            connInfo = (ConnectionInformation)htConninfo[lvItem];

            htConninfo.Remove(connInfo);
            htConninfo.Remove(lvItem);

            lvConnection.Items.Remove(lvItem);

            SaveConnectionList();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!CheckValidInput())
            {
                MessageBox.Show("Input all information about the connection.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void lvConnection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvConnection.SelectedItems.Count < 1)
                return;

            txtConnectionName.Text = lvConnection.SelectedItems[0].Text;
            SetConnInfo((ConnectionInformation)htConninfo[lvConnection.SelectedItems[0]]);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();

            if (thread != null && thread.ThreadState == System.Threading.ThreadState.Running
                && MessageBox.Show("The specified query takes a long time to proceed. It may cause an out of memory error. Do you want to cancel and try another query?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                thread.Abort();
                btnOk.Enabled = true;
            }
        }

        #endregion
    }
}