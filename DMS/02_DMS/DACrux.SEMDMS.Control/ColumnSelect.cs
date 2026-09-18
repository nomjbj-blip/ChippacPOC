using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Miracom.DMS.LIB.Report
{
	/// <Summary>
	/// <b>■ Defect Report 의 Custom 컬럼선택 컨트롤</b><br>
	/// - 작  성  자 : 미라콤 곽동일<br>
	/// - 최초작성일 : 2004년 08월 27일<br>
	/// - 최종수정자 : <br>
	/// - 최종수정일 : <br>
	/// - 주요변경로그<br>
	///   2004.08.27 생성<br>
	/// </Summary>
	/// <Remarks>없음</Remarks>
	/// 
	public class ColumnSelect : System.Windows.Forms.UserControl
	{
		public System.Windows.Forms.CheckedListBox clbColumn;
		public System.Windows.Forms.CheckBox chkColumn;
		private System.ComponentModel.Container components = null;
		public string[,] strColumnList;
		private System.Windows.Forms.Label lblColumnSelect;
		public int intColumnCount=31;

		#region ■ 생성자 - 모든 컬럼을 배열에 Setting 함.

		/// <summary>
		/// 사용자가 원하는 컬럼을 선택할 수 있도록하기 위하여 가능한 모든 컬럼을 배열에 저장한다.
		/// </summary>
		/// 
		public ColumnSelect()
		{
			// 이 호출은 Windows.Forms Form 디자이너에 필요합니다.
			InitializeComponent();

			// TODO: InitializeComponent를 호출한 다음 초기화 작업을 추가합니다.

            // Defect Count Report Default 선택 Column Setting.
			strColumnList=new string[intColumnCount,2];

			strColumnList[0,0]="Scan Date";
			strColumnList[0,1]="TO_CHAR(T_DMS_STEP.RESULTTIMESTAMP,'YYYY-MM-DD HH24:MI:SS') AS Scan_Date";

			strColumnList[1,0]="Lot ID";
			strColumnList[1,1]="T_DMS_LOT.LOT_ID AS LOT_ID";

			strColumnList[2,0]="Wafer ID";
			strColumnList[2,1]="T_DMS_WAFER.WAFER_ID AS WAFER_ID";

			strColumnList[3,0]="Setup ID";
			strColumnList[3,1]="T_DMS_SETUP.SETUP_ID AS SETUP_ID";

			strColumnList[4,0]="Step ID";
			strColumnList[4,1]="T_DMS_STEP.STEP_ID AS STEP_ID";

			strColumnList[5,0]="S Cls";
			strColumnList[5,1]="substr(T_DMS_LOT.LOT_ID,3,1) AS S_Cls";

			strColumnList[6,0]="Main_Eq";
			strColumnList[6,1]="T_DMS_STEP.MAIN_EQ AS Main_Eq";

//			strColumnList[7,0]="M_Eq_2";
//			strColumnList[7,1]="'' AS M_Eq_2";
//
//			strColumnList[8,0]="M_Eq_3";
//			strColumnList[8,1]="'' AS M_Eq_3";
//
//			strColumnList[9,0]="M_Eq_4";
//			strColumnList[9,1]="'' AS M_Eq_4";

			strColumnList[7,0]="Insp-EQ";
			strColumnList[7,1]="T_DMS_STEP.INSPECTION_EQ AS EQ_ID";

			strColumnList[8,0]="Tot_Def_Cnt";
			strColumnList[8,1]="T_DMS_STEP_SUM.TOTALDEFECT AS Tot_Def_Cnt";

			strColumnList[9,0]="Tot_Def_0.5";
			strColumnList[9,1]="(T_DMS_STEP_SUM.TOTALDEFECT-T_DMS_STEP_SUM.TOTAL5UM) AS Tot_Def_05";

			strColumnList[10,0]="DI";
			strColumnList[10,1]="'' AS DI";

			strColumnList[11,0]="DR";
			strColumnList[11,1]="T_DMS_STEP_SUM.TOTALDEFECTDIE AS DR";

			strColumnList[12,0]="DR_0.5";
			strColumnList[12,1]="(T_DMS_STEP_SUM.TOTALDEFECTDD-T_DMS_STEP_SUM.TOTALRANDOMDEFDIE5UM) AS DR_05";

			strColumnList[13,0]="Density";
			strColumnList[13,1]="T_DMS_STEP_SUM.TOTALDEFECTDD AS Density";

			strColumnList[14,0]="NORV";
			strColumnList[14,1]="'' AS NORV";

			strColumnList[15,0]="NOIS";
			strColumnList[15,1]="'' AS NOIS";

			strColumnList[16,0]="SPOT";
			strColumnList[16,1]="'' AS SPOT";

			strColumnList[17,0]="SBPT";
			strColumnList[17,1]="'' AS SBPT";

			strColumnList[18,0]="BRGE";
			strColumnList[18,1]="'' AS BRGE";

			strColumnList[19,0]="BFBG";
			strColumnList[19,1]="'' AS BFBG";

			strColumnList[20,0]="PABG";
			strColumnList[20,1]="'' AS PABG";

			strColumnList[21,0]="DISC";
			strColumnList[21,1]="'' AS DISC";

			strColumnList[22,0]="GRSF";
			strColumnList[22,1]="'' AS GRSF";

			strColumnList[23,0]="WSSF";
			strColumnList[23,1]="'' AS WSSF";

			strColumnList[24,0]="UPPR";
			strColumnList[24,1]="'' AS UPPR";

			strColumnList[25,0]="NOCP";
			strColumnList[25,1]="'' AS NOCP";

			strColumnList[26,0]="APPD";
			strColumnList[26,1]="'' AS APPD";

			strColumnList[27,0]="SCRA";
			strColumnList[27,1]="'' AS SCRA";

			strColumnList[28,0]="CMSC";
			strColumnList[28,1]="'' AS CMSC";

			strColumnList[29,0]="USER";
			strColumnList[29,1]="'' AS USER";

			strColumnList[30,0]="REMARK";
			strColumnList[30,1]="'' AS Remark";

			for(int i=0;i<intColumnCount;i++)
			{
				if(strColumnList[i,1].Substring(0,2)=="''")
				{
					clbColumn.Items.Add(strColumnList[i,0],CheckState.Unchecked);
				}
				else
				{
					clbColumn.Items.Add(strColumnList[i,0],CheckState.Checked);
				}
			}

		}
		#endregion

		#region Dispose

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
		#endregion

		#region 구성 요소 디자이너에서 생성한 코드
		/// <summary> 
		/// 디자이너 지원에 필요한 메서드입니다. 
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
			this.clbColumn = new System.Windows.Forms.CheckedListBox();
			this.chkColumn = new System.Windows.Forms.CheckBox();
			this.lblColumnSelect = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// clbColumn
			// 
			this.clbColumn.CheckOnClick = true;
			this.clbColumn.Location = new System.Drawing.Point(0, 38);
			this.clbColumn.Name = "clbColumn";
			this.clbColumn.Size = new System.Drawing.Size(193, 212);
			this.clbColumn.TabIndex = 0;
			// 
			// chkColumn
			// 
			this.chkColumn.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.chkColumn.Location = new System.Drawing.Point(1, 20);
			this.chkColumn.Name = "chkColumn";
			this.chkColumn.Size = new System.Drawing.Size(165, 15);
			this.chkColumn.TabIndex = 1;
			this.chkColumn.Text = "Selected Column";
			this.chkColumn.CheckedChanged += new System.EventHandler(this.chkColumn_CheckedChanged);
			// 
			// lblColumnSelect
			// 
			this.lblColumnSelect.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.lblColumnSelect.Location = new System.Drawing.Point(-1, 1);
			this.lblColumnSelect.Name = "lblColumnSelect";
			this.lblColumnSelect.Size = new System.Drawing.Size(100, 14);
			this.lblColumnSelect.TabIndex = 2;
			this.lblColumnSelect.Text = "Column Select";
			// 
			// ColumnSelect
			// 
			this.Controls.Add(this.lblColumnSelect);
			this.Controls.Add(this.clbColumn);
			this.Controls.Add(this.chkColumn);
			this.Name = "ColumnSelect";
			this.Size = new System.Drawing.Size(197, 255);
			this.ResumeLayout(false);

		}
		#endregion

		#region ■ 컬럼선택의 Select All 체크박스 클릭 이벤트

		/// <summary>
		/// 1) Select All 체크박스의 상태가 변경되면 컬럼선택 체크리스트박스 Item을 모두 선택하거나 해제한다.
		/// </summary>
		/// 
		private void chkColumn_CheckedChanged(object sender, System.EventArgs e)
		{
			if(clbColumn.Items.Count==0)
			{
				chkColumn.Checked=false;
			}
			else
			{
				if(chkColumn.Checked==true)
				{
					for(int i=0;i<clbColumn.Items.Count;i++)
					{
						clbColumn.SetItemCheckState(i,CheckState.Checked);
					}
				}
				else
				{
					for(int i=0;i<clbColumn.Items.Count;i++)
					{
						clbColumn.SetItemCheckState(i,CheckState.Unchecked);
					}
				}
			}		
		}
		#endregion
	}
}
