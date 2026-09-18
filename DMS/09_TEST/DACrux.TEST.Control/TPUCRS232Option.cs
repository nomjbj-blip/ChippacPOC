using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace DACrux.TEST.Control
{
    public delegate void SPOption(object sender, System.IO.Ports.SerialPort SP);

    public partial class TPUCRS232Option : Form
    {
        public SerialPort SP_OPTION
        {
            get { return m_SP; }
        }

        public TPUCRS232Option()
        {
            InitializeComponent();
        }

        public event SPOption OnSPOption;

        System.IO.Ports.SerialPort m_SP = null;

        private void TPUCRS232Option_Load(object sender, EventArgs e)
        {
            //Serial 관련 정보를 미리 선택하여 놓는다.
            cmbPort.BeginUpdate();
            foreach (string comport in SerialPort.GetPortNames())
            {
                //현재 사용 가능한 Comport 를 불러 온다.
                cmbPort.Items.Add(comport);
            }
            cmbPort.EndUpdate();

            if (cmbPort.Items.Count > 0)
                cmbPort.SelectedIndex = 0;

            cmbBoud.SelectedIndex = 0;
            cmbDataBits.SelectedIndex = 0;
            cmbParity.SelectedIndex = 0;
            cmbStopBits.SelectedIndex = 0;

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            m_SP = new SerialPort();

            if (string.IsNullOrEmpty(cmbPort.Text))
            {
                MessageBox.Show("COM Port 가 선택되지 않았습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(cmbBoud.Text))
            {
                MessageBox.Show("Baud Rate 가 선택되지 않았습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(cmbDataBits.Text))
            {
                MessageBox.Show("Data bits 가 선택되지 않았습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(cmbParity.Text))
            {
                MessageBox.Show("Parity 가 선택되지 않았습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(cmbStopBits.Text))
            {
                MessageBox.Show("Stop Bits 가 선택되지 않았습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            m_SP.PortName = cmbPort.SelectedItem.ToString();

            switch (cmbBoud.SelectedIndex)
            {
                case 0:
                    m_SP.BaudRate = (int)9600;
                    break;
                case 1:
                    m_SP.BaudRate = (int)14400;
                    break;
                case 2:
                    m_SP.BaudRate = (int)19200;
                    break;
                case 3:
                    m_SP.BaudRate = (int)38400;
                    break;
                case 4:
                    m_SP.BaudRate = (int)57600;
                    break;
                case 5:
                    m_SP.BaudRate = (int)115200;
                    break;
                default:
                    m_SP.BaudRate = (int)9600;
                    break;
            }

            switch (cmbDataBits.SelectedIndex)
            {
                case 0:
                    m_SP.DataBits = 8;
                    break;
                case 1:
                    m_SP.DataBits = 7;
                    break;
                default:
                    m_SP.DataBits = 8;
                    break;
            }

            switch (cmbParity.SelectedIndex)
            {
                case 0:
                    m_SP.Parity = Parity.None;
                    break;
                case 1:
                    m_SP.Parity = Parity.Even;
                    break;
                case 2:
                    m_SP.Parity = Parity.Mark;
                    break;
                case 3:
                    m_SP.Parity = Parity.Odd;
                    break;
                case 4:
                    m_SP.Parity = Parity.Space;
                    break;
                default:
                    m_SP.Parity = Parity.None;
                    break;
            }

            switch (cmbStopBits.SelectedIndex)
            {
                case 0:
                    m_SP.StopBits = StopBits.One;
                    break;
                case 1:
                    m_SP.StopBits = StopBits.OnePointFive;
                    break;
                case 2:
                    m_SP.StopBits = StopBits.Two;
                    break;
                default:
                    m_SP.StopBits = StopBits.One;
                    break;
            }

            if (OnSPOption != null)
            {
                OnSPOption(this, m_SP);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
