using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.Map;

namespace DACrux.SEMDMS.Control
{
    public partial class DPUCDefectMapGalleryItem : UserControl
    {
        string[] m_informationArr;

        public static readonly int FONT_MAX = 32;
        public static readonly int FONT_MIN = 6;

        public DPUCDefectMapGalleryItem()
        {
            InitializeComponent();
        }

        private void DPUCDefectMapGalleryItem_Load(object sender, EventArgs e)
        {
            label1.MouseWheel += new MouseEventHandler(label1_MouseWheel);
            UpdateLabel();
        }

        private void label1_MouseWheel(object sender, MouseEventArgs e)
        {
            // Ctrl + Mouse Wheel 시 X축 Font 크기 조정 2019.12.01 Taihi,Kim.
            if (System.Windows.Forms.Control.ModifierKeys == System.Windows.Forms.Keys.Control)
            {
                System.Drawing.Font currFont = label1.Font;
                float size = currFont.Size + (e.Delta > 0 ? 1f : -1f);
                size = Math.Max(size, FONT_MIN);
                size = Math.Min(size, FONT_MAX);

                label1.Font = new System.Drawing.Font(currFont.FontFamily, size);
                currFont.Dispose();
            }
        }

        private void UpdateLabel()
        {
            label1.Visible = m_informationArr != null && m_informationArr.Length > 0;

            if (label1.Visible)
            {
                label1.Text = null;

                foreach (string info in m_informationArr)
                    label1.Text += info;

                using (Graphics g = label1.CreateGraphics())
                {
                    float height = g.MeasureString(label1.Text, label1.Font, label1.Width - label1.Padding.Left - label1.Padding.Right ).Height;
                    height += label1.Padding.Top + label1.Padding.Bottom;
                    height = Math.Min(height, Height * 0.9f);
                    label1.Height = (int)Math.Ceiling(height);
                }
            }
        }

        public DefectMap DefectMap
        {
            get { return map; }
        }

        public string[] Information
        {
            get { return m_informationArr; }
            set { m_informationArr = value; UpdateLabel(); }
        }
    }
}
