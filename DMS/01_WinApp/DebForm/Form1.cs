using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DebForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            tpucqcInspection1.MAP_SERVER_IP = "12.230.55.151";
            //tpucqcInspection1.MAP_SERVER_IP = "localhost";
            tpucqcInspection1.MAP_SERVER_PORT = 7513;
            tpucqcInspection1.DrawWaferMap("IQC", "FCBPH8S1AB3B01", "AB101504170100", "AB1015041701-30");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tpucqcInspection1.SaveData();
            //tpucqcInsp1.SaveData("IQC","FCBPH8S1AB3B01", "AG575B00", "AG575B_17");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                ucDataEditor1.SPC_SERVER_IP = "localhost";
                ucDataEditor1.SPC_SERVER_PORT = 7517;

                //ucDataEditor1.SetPlan(DACrux.SPC.Base.PLAN_MODE.MES, "HMKB1", "Admin", "IQC", new string[] { "PB2B04150010", "PB2B04150011" }, "VISUAL");
                ucDataEditor1.SetPlan(DACrux.SPC.Base.PLAN_MODE.MES, "HMKB1", "Admin", "IQC_DESK", "", "IQC", "", "SB", "ABCD", new string[] { "접수번호3000001", "접수번호3000002" }, "VISUAL");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                ucDataEditor1.SaveData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ucDataEditor1_OnChangeSpecOutCount(object sender, int Count,double Sum)
        {
            textBox1.Text = Count.ToString() + "," + Sum.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                tpuBinMapView1.MAP_SERVER_IP = "localhost";
                tpuBinMapView1.MAP_SERVER_PORT = 7513;
                //tpuBinMapView1.DrawWaferMap("AB1015041701-02", "IQC");
                tpuBinMapView1.DrawWaferMap("B2000", "FCBPH8S1AB3B01", "AG575B00", "AG575B_14",null,0,true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                tpuBinMapView1.MAP_SERVER_IP = "localhost";
                tpuBinMapView1.MAP_SERVER_PORT = 7513;
                //tpuBinMapView1.DrawWaferMap("AB1015041701-02", "IQC");
                tpuBinMapView1.DrawWaferMap("IQC", "FCBPH8S1AB3B01", "AG575B00", "AG575B_30");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
