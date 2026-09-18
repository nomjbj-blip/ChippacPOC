using NexplantQMS.GdsMap.Oracle;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NexplantQMS.GdsMap
{
    public partial class Form1 : Form
    {
		public static readonly string SAMPLE_FILE = @"D:\PROJECT\스태츠칩팩\Defect E-Test\sample\OMM all layer2\OMM all layer2.gds";
        public Form1()
        {
            InitializeComponent();
        }

		private void Form1_Load(object sender, EventArgs e)
		{
			map.OnlySelectSquareItems = true;

			//button1.PerformClick();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			// C# 7.3 환경에서도 파일 선택 창 자원을 자동 해제하기 위해 using 블록을 사용한다.
			using (var dlg = new OpenFileDialog())
			{
			dlg.Filter = "GDS files (*.gds)|*.gds|All files (*.*)|*.*";

			if (dlg.ShowDialog() != DialogResult.OK)
				return;

			GdsReader reader = new GdsReader();
			reader.LengthUnit = LengthUnit.Micrometer;
			var lib = reader.Read(dlg.FileName);

			var str = lib.Structures.First();

			// defect
			str.DefectList.Add(new Defect() { X = 0, Y = 0, Width = 15, Height = 20 });
			str.DefectList.Add(new Defect() { X = 1000, Y = 1500, Width = 30, Height = 20 });

			var layerColor = new Dictionary<int, Color> {
				{ 1, Color.FromArgb(80, 180, 80) },
				{ 2, Color.FromArgb(80, 140, 220) },
				{ 3, Color.FromArgb(220, 200, 60) },
				{ 4, Color.FromArgb(200, 90, 200) },
				{ 5, Color.FromArgb(70, 200, 200) },
				{ 6, Color.FromArgb(230, 140, 60) },
				{ 7, Color.FromArgb(150, 150, 150) },
				{ 8, Color.FromArgb(220, 80, 80) }
			};

			map.SetLayerColor(layerColor);

			map.ShowStructure(lib);

			// list
			var list = str.Layers.SelectMany(s => s.Elements).Select(element =>
			{
				var etc = element is GdsText ? (element as GdsText).Text : element is GdsPath ? (element as GdsPath).Width.ToString() : String.Empty;

				return new
				{
					element.LayerID,
					element.ElementName,
					element.Bounds,
					etc
				};
			});

			dataGridView1.DataSource = list;

			// layer list
			checkedListBox1.Items.Clear();

			foreach (var layer in str.Layers.Select(a => a.LayerID))
				checkedListBox1.Items.Add(layer, true);
			}
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			if (map.Structure == null)
				return;

			// 테스트로 Element만 저장
			foreach (var layer in map.Structure.Layers)
				OracleSaveUtil.SaveElements(layer.Elements);
		}

		private void btnLoad_Click(object sender, EventArgs e)
		{
			GdsLibrary lib = new GdsLibrary();
			GdsStructure str = new GdsStructure();
			str.Name = "TOP";

			foreach (var el in OracleLoadUtil.LoadElements(2, str.Name))
				str.Layers.AddElement(el);

			lib.Structures.Add(str);

			map.ShowStructure(lib);
		}

		private void btnLayer_Click(object sender, EventArgs e)
		{
			if (map.Structure == null)
				return;
						
			for (int i = 0; i < checkedListBox1.Items.Count; i++)
			{
				var layer = map.Structure.Layers.GetLayer((int)checkedListBox1.Items[i]);
				layer.Visible = checkedListBox1.GetItemChecked(i);
			}

			map.UpdateVisibleLayer();
		}
	}
}
