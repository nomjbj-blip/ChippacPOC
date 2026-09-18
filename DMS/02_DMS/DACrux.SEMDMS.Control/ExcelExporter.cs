using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Miracom.DMS.LIB.Report
{
	/// <summary>
	/// ExcelExporter에 대한 요약 설명입니다.
	/// </summary>
	public class ExcelExporter : System.Windows.Forms.Form
	{
		private System.Windows.Forms.CheckBox chkExportOption;
		private System.Windows.Forms.CheckedListBox lbSheetList;
		private System.Windows.Forms.Button btnExport;
		private System.Windows.Forms.Button btnCancel;
		/// <summary>
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.Container components = null;
		public System.IntPtr iCallerFormPtr;
		public ArrayList arrSheetList;
		private System.Windows.Forms.GroupBox gbChartType;
		private System.Windows.Forms.RadioButton optExcel;
		private System.Windows.Forms.RadioButton optIMAGE;
		private ArrayList arrSheetSelectList;

		public ExcelExporter()
		{
			//
			// Windows Form 디자이너 지원에 필요합니다.
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent를 호출한 다음 생성자 코드를 추가합니다.
			//
			Initialize();
		}
		private void Initialize()
		{
			arrSheetList = new ArrayList();
			arrSheetSelectList = new ArrayList();
		}
		public void SetSheetList()
		{
			for(int i=0;i<this.arrSheetList.Count;i++)
			{
				this.lbSheetList.Items.Add(arrSheetList[i].ToString(),CheckState.Checked);
			}
		}
		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form 디자이너에서 생성한 코드
		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다.
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
			this.chkExportOption = new System.Windows.Forms.CheckBox();
			this.lbSheetList = new System.Windows.Forms.CheckedListBox();
			this.btnExport = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.gbChartType = new System.Windows.Forms.GroupBox();
			this.optExcel = new System.Windows.Forms.RadioButton();
			this.optIMAGE = new System.Windows.Forms.RadioButton();
			this.gbChartType.SuspendLayout();
			this.SuspendLayout();
			// 
			// chkExportOption
			// 
			this.chkExportOption.Font = new System.Drawing.Font("돋움", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.chkExportOption.Location = new System.Drawing.Point(16, 16);
			this.chkExportOption.Name = "chkExportOption";
			this.chkExportOption.Size = new System.Drawing.Size(288, 24);
			this.chkExportOption.TabIndex = 0;
			this.chkExportOption.Text = "차트포함 (Export시 차트포함여부)";
			// 
			// lbSheetList
			// 
			this.lbSheetList.Location = new System.Drawing.Point(16, 96);
			this.lbSheetList.Name = "lbSheetList";
			this.lbSheetList.Size = new System.Drawing.Size(240, 196);
			this.lbSheetList.TabIndex = 1;
			// 
			// btnExport
			// 
			this.btnExport.BackColor = System.Drawing.SystemColors.Desktop;
			this.btnExport.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.btnExport.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.btnExport.Location = new System.Drawing.Point(16, 304);
			this.btnExport.Name = "btnExport";
			this.btnExport.Size = new System.Drawing.Size(72, 24);
			this.btnExport.TabIndex = 2;
			this.btnExport.Text = "Export";
			this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.BackColor = System.Drawing.SystemColors.Desktop;
			this.btnCancel.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.btnCancel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.btnCancel.Location = new System.Drawing.Point(96, 304);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(72, 24);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// gbChartType
			// 
			this.gbChartType.Controls.Add(this.optIMAGE);
			this.gbChartType.Controls.Add(this.optExcel);
			this.gbChartType.Font = new System.Drawing.Font("돋움", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.gbChartType.Location = new System.Drawing.Point(24, 40);
			this.gbChartType.Name = "gbChartType";
			this.gbChartType.Size = new System.Drawing.Size(232, 40);
			this.gbChartType.TabIndex = 4;
			this.gbChartType.TabStop = false;
			this.gbChartType.Text = "차트타입";
			// 
			// optExcel
			// 
			this.optExcel.Checked = true;
			this.optExcel.Location = new System.Drawing.Point(16, 16);
			this.optExcel.Name = "optExcel";
			this.optExcel.Size = new System.Drawing.Size(80, 16);
			this.optExcel.TabIndex = 0;
			this.optExcel.TabStop = true;
			this.optExcel.Text = "Excel";
			// 
			// optIMAGE
			// 
			this.optIMAGE.Location = new System.Drawing.Point(128, 16);
			this.optIMAGE.Name = "optIMAGE";
			this.optIMAGE.Size = new System.Drawing.Size(80, 16);
			this.optIMAGE.TabIndex = 1;
			this.optIMAGE.Text = "Image";
			// 
			// ExcelExporter
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(264, 333);
			this.Controls.Add(this.gbChartType);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnExport);
			this.Controls.Add(this.lbSheetList);
			this.Controls.Add(this.chkExportOption);
			this.Name = "ExcelExporter";
			this.Text = "ExcelExporter";
			this.gbChartType.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void btnExport_Click(object sender, System.EventArgs e)
		{
			CrossOne oCrossOne = null;
			for(int i=0;i<lbSheetList.CheckedItems.Count;i++)
			{
				arrSheetSelectList.Add(lbSheetList.CheckedItems[i]);
			}
			System.Windows.Forms.Control oCrossOneControl = CrossOne.FromHandle(iCallerFormPtr);
			oCrossOne = (CrossOne)oCrossOneControl;

			if(this.chkExportOption.Checked == true)
			{
				if(this.optExcel.Checked == true)
				{oCrossOne.ExportSheetToExcel(arrSheetSelectList,true,true);}
				else
				{oCrossOne.ExportSheetToExcel(arrSheetSelectList,true,false);}
			}	
			else
			{
				if(this.optExcel.Checked == true)
				{oCrossOne.ExportSheetToExcel(arrSheetSelectList,false,false);}
				else
				{oCrossOne.ExportSheetToExcel(arrSheetSelectList,false,false);}
			}
			this.Close();
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}
	}
}
