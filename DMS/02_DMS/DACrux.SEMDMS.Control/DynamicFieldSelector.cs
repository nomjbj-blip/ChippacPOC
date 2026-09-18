using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Miracom.DMS.LIB.Report
{
	/// <summary>
	/// DynamicFieldSelector에 대한 요약 설명입니다.
	/// </summary>
	public enum SELECTOR_TYPE
	{
		DEFECT_FIELD_SHEET,
		DEFECT_FIELD_CHART
	}
	public class DynamicFieldSelector : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnApply;
		private System.Windows.Forms.Button btnConfirm;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.ListView lvDynamicSelector;
		private bool bExist = false;

		#region  ■ 사용자 정의 변수
		public SELECTOR_TYPE eSELECTOR_TYPE;
		public System.IntPtr iCallerFormPtr;
		public string strUserID = "";

		#endregion
		private System.Windows.Forms.Button btnSelectAll;
		private System.Windows.Forms.Button btnSelectNone;
		private System.Windows.Forms.Label lblTitle;
		
		#region  ■ 사용자 정의 상수
		#endregion

		/// <summary>
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public DynamicFieldSelector()
		{
			//
			// Windows Form 디자이너 지원에 필요합니다.
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent를 호출한 다음 생성자 코드를 추가합니다.
			//
		}

		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		#region  ■ 초기화 영역

		#endregion
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
			this.btnApply = new System.Windows.Forms.Button();
			this.btnConfirm = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lvDynamicSelector = new System.Windows.Forms.ListView();
			this.btnSelectAll = new System.Windows.Forms.Button();
			this.btnSelectNone = new System.Windows.Forms.Button();
			this.lblTitle = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnApply
			// 
			this.btnApply.BackColor = System.Drawing.SystemColors.Desktop;
			this.btnApply.Font = new System.Drawing.Font("돋움", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.btnApply.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.btnApply.Location = new System.Drawing.Point(8, 304);
			this.btnApply.Name = "btnApply";
			this.btnApply.Size = new System.Drawing.Size(64, 24);
			this.btnApply.TabIndex = 0;
			this.btnApply.Text = "적용";
			this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
			// 
			// btnConfirm
			// 
			this.btnConfirm.BackColor = System.Drawing.SystemColors.Desktop;
			this.btnConfirm.Font = new System.Drawing.Font("돋움", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.btnConfirm.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.btnConfirm.Location = new System.Drawing.Point(80, 304);
			this.btnConfirm.Name = "btnConfirm";
			this.btnConfirm.Size = new System.Drawing.Size(64, 24);
			this.btnConfirm.TabIndex = 1;
			this.btnConfirm.Text = "확인";
			this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
			// 
			// btnSave
			// 
			this.btnSave.BackColor = System.Drawing.SystemColors.Desktop;
			this.btnSave.Font = new System.Drawing.Font("돋움", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.btnSave.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.btnSave.Location = new System.Drawing.Point(152, 304);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(64, 24);
			this.btnSave.TabIndex = 2;
			this.btnSave.Text = "저장";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.BackColor = System.Drawing.SystemColors.Desktop;
			this.btnCancel.Font = new System.Drawing.Font("돋움", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.btnCancel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.btnCancel.Location = new System.Drawing.Point(224, 304);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(64, 24);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "취소";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// lvDynamicSelector
			// 
			this.lvDynamicSelector.CheckBoxes = true;
			this.lvDynamicSelector.Location = new System.Drawing.Point(8, 40);
			this.lvDynamicSelector.Name = "lvDynamicSelector";
			this.lvDynamicSelector.Size = new System.Drawing.Size(280, 256);
			this.lvDynamicSelector.TabIndex = 4;
			this.lvDynamicSelector.View = System.Windows.Forms.View.Details;
			// 
			// btnSelectAll
			// 
			this.btnSelectAll.BackColor = System.Drawing.Color.LightPink;
			this.btnSelectAll.Font = new System.Drawing.Font("돋움", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.btnSelectAll.ForeColor = System.Drawing.SystemColors.ControlText;
			this.btnSelectAll.Location = new System.Drawing.Point(184, 8);
			this.btnSelectAll.Name = "btnSelectAll";
			this.btnSelectAll.Size = new System.Drawing.Size(48, 24);
			this.btnSelectAll.TabIndex = 5;
			this.btnSelectAll.Text = "All";
			this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
			// 
			// btnSelectNone
			// 
			this.btnSelectNone.BackColor = System.Drawing.Color.LightPink;
			this.btnSelectNone.Font = new System.Drawing.Font("돋움", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.btnSelectNone.Location = new System.Drawing.Point(240, 8);
			this.btnSelectNone.Name = "btnSelectNone";
			this.btnSelectNone.Size = new System.Drawing.Size(48, 24);
			this.btnSelectNone.TabIndex = 6;
			this.btnSelectNone.Text = "None";
			this.btnSelectNone.Click += new System.EventHandler(this.btnSelectNone_Click);
			// 
			// lblTitle
			// 
			this.lblTitle.Font = new System.Drawing.Font("돋움", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.lblTitle.Location = new System.Drawing.Point(16, 16);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(128, 16);
			this.lblTitle.TabIndex = 7;
			// 
			// DynamicFieldSelector
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(296, 341);
			this.Controls.Add(this.lblTitle);
			this.Controls.Add(this.btnSelectNone);
			this.Controls.Add(this.btnSelectAll);
			this.Controls.Add(this.lvDynamicSelector);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.btnConfirm);
			this.Controls.Add(this.btnApply);
			this.Name = "DynamicFieldSelector";
			this.Text = "DynamicFieldSelector";
			this.Load += new System.EventHandler(this.DynamicFieldSelector_Load);
			this.Closed += new System.EventHandler(this.DynamicFieldSelector_Closed);
			this.ResumeLayout(false);

		}
		#endregion


		private void DynamicFieldSelector_Load(object sender, System.EventArgs e)
		{
			if(DesignMode) return;

			DataSet dsConfig = null;
			DataSet dsUserConfig = null;
			string strParseTmp = string.Empty;
			string[] arrParseTmpSplits = null;
			ListViewItem oListViewItem = null;
			
			try
			{
				this.lvDynamicSelector.Columns.Add("Defect Class",100, HorizontalAlignment.Center);
				this.lvDynamicSelector.Columns.Add("Description",200, HorizontalAlignment.Left);

				switch(eSELECTOR_TYPE)
				{
					case SELECTOR_TYPE.DEFECT_FIELD_CHART:
						lblTitle.Text = "차트출력 동적구성 항목";
						
						dsConfig = GetConfig();
						for(int i=0;i<dsConfig.Tables[0].Rows.Count;i++)
						{
							oListViewItem = new ListViewItem(dsConfig.Tables[0].Rows[i]["NAME"].ToString());
							oListViewItem.SubItems.Add(dsConfig.Tables[0].Rows[i]["COMMENT"].ToString());
							lvDynamicSelector.Items.Add(oListViewItem);
						}
						dsUserConfig = GetUserConfig();
						if(dsUserConfig == null || dsUserConfig.Tables[0].Rows.Count == 0)
						{bExist=false;}
						else
						{
							strParseTmp = dsUserConfig.Tables[0].Rows[0]["VALUE_A"].ToString();
							if(strParseTmp.Trim().Length >0)
							{
								arrParseTmpSplits = strParseTmp.Split(Convert.ToChar(";"));
								if(arrParseTmpSplits != null)
								{
									for(int i=0;i<arrParseTmpSplits.Length;i++)
									{
										for(int j=0;j<this.lvDynamicSelector.Items.Count;j++)
										{
											if(this.lvDynamicSelector.Items[j].Text == arrParseTmpSplits[i])
											{lvDynamicSelector.Items[j].Checked = true;}
										}
									}					
								}
							}

							bExist=true;
						}
						break;
					case SELECTOR_TYPE.DEFECT_FIELD_SHEET:
						lblTitle.Text = "시트필드 동적구성 항목";
						dsConfig = GetConfig();
						for(int i=0;i<dsConfig.Tables[0].Rows.Count;i++)
						{
							oListViewItem = new ListViewItem(dsConfig.Tables[0].Rows[i]["NAME"].ToString());
							oListViewItem.SubItems.Add(dsConfig.Tables[0].Rows[i]["COMMENT"].ToString());
							lvDynamicSelector.Items.Add(oListViewItem);
							
						}
						dsUserConfig = GetUserConfig();
						if(dsUserConfig == null || dsUserConfig.Tables[0].Rows.Count == 0)
						{bExist=false;}
						else
						{
							strParseTmp = dsUserConfig.Tables[0].Rows[0]["VALUE_A"].ToString();
							if(strParseTmp.Trim().Length >0)
							{
								arrParseTmpSplits = strParseTmp.Split(Convert.ToChar(";"));
								if(arrParseTmpSplits != null)
								{
									for(int i=0;i<arrParseTmpSplits.Length;i++)
									{
										for(int j=0;j<this.lvDynamicSelector.Items.Count;j++)
										{
											if(this.lvDynamicSelector.Items[j].Text == arrParseTmpSplits[i])
											{lvDynamicSelector.Items[j].Checked = true;}
										}
									}
								}
							}
							bExist=true;
						}
						break;
				}
			}
			catch(Exception Ex)
			{
				MessageBox.Show(Ex.Message);
			}
		}

		private DataSet GetConfig()
		{
			DataSet dsDfClsConfig = null;
			Remoting oRemoting = null;
			string[] paramValues = null;

			paramValues = new string[1];

			switch(eSELECTOR_TYPE)
			{
				case SELECTOR_TYPE.DEFECT_FIELD_CHART:
					//paramValues[0] = "CROSSRPT_ONE_FIELD_CHART";
					paramValues[0] = "CROSSRPT_ONE_DEFECT_FIELD_SHEET";
					oRemoting = new Remoting();
					break;
				case SELECTOR_TYPE.DEFECT_FIELD_SHEET:
					//DEFECT CLASS FIELD DYNAMIC CONFIGURATION INFO QUERY
					paramValues[0] = "CROSSRPT_ONE_DEFECT_FIELD_SHEET";
					oRemoting = new Remoting();
					break;
			}
			dsDfClsConfig = oRemoting.GetConfig(strUserID,"GetConfig",paramValues);
			return dsDfClsConfig;
		}

		private void SetUserConfig(USER_CONFIG_QUERY_TYPE eUSER_CONFIG_QUERY_TYPE)
		{
			Remoting oRemoting = null;
			string[] paramValues = null;
			string strDynamicElements = string.Empty;
			// INSERT = 1.CATEGORY 2.USER_ID 3.NAME 4.VALUE_A 5.VALUE_B
			// UPDATE = 1.VALUE_A  2.CATEGORY 3.USER_ID 4.NAME 
			try
			{
				for(int i=0;i<this.lvDynamicSelector.Items.Count;i++)
				{
					if(lvDynamicSelector.Items[i].Checked == true)
					{
						strDynamicElements += lvDynamicSelector.Items[i].Text + ";";
					}
				}

				if(strDynamicElements.Length >0)
				{
					strDynamicElements = strDynamicElements.Substring(0,strDynamicElements.Length -1);
				}
				switch(eUSER_CONFIG_QUERY_TYPE)
				{
					case USER_CONFIG_QUERY_TYPE.INSERT:
					switch(eSELECTOR_TYPE)
					{
						case SELECTOR_TYPE.DEFECT_FIELD_CHART:
							paramValues = new string[5];
							paramValues[0] = "CROSSRPT_ONE";
							paramValues[1] = strUserID;
							paramValues[2] = "CHART_CLASS";
							paramValues[3] = strDynamicElements;
							paramValues[4] = " ";
							oRemoting = new Remoting();
							oRemoting.InsertUserConfig(strUserID,"InsertUserConfig",paramValues);
							break;
						case SELECTOR_TYPE.DEFECT_FIELD_SHEET:
							//DEFECT CLASS FIELD DYNAMIC CONFIGURATION INFO QUERY
							paramValues = new string[5];
							paramValues[0] = "CROSSRPT_ONE";
							paramValues[1] = strUserID;
							paramValues[2] = "DF_CLASS";
							paramValues[3] = strDynamicElements;
							paramValues[4] = "  ";
							oRemoting = new Remoting();
							oRemoting.InsertUserConfig(strUserID,"InsertUserConfig",paramValues);
							break;
					}
						break;
					case USER_CONFIG_QUERY_TYPE.UPDATE:
					switch(eSELECTOR_TYPE)
					{
						case SELECTOR_TYPE.DEFECT_FIELD_CHART:
							paramValues = new string[4];
							paramValues[0] = strDynamicElements;
							paramValues[1] = "CROSSRPT_ONE";
							paramValues[2] = strUserID;
							paramValues[3] = "CHART_CLASS";
							oRemoting = new Remoting();
							oRemoting.UpdateUserConfig(strUserID,"UpdateUserConfig",paramValues);
							break;
						case SELECTOR_TYPE.DEFECT_FIELD_SHEET:
							//DEFECT CLASS FIELD DYNAMIC CONFIGURATION INFO QUERY
							paramValues = new string[4];
							paramValues[0] = strDynamicElements;
							paramValues[1] = "CROSSRPT_ONE";
							paramValues[2] = strUserID;
							paramValues[3] = "DF_CLASS";
							oRemoting = new Remoting();
							oRemoting.UpdateUserConfig(strUserID,"UpdateUserConfig",paramValues);
							break;
					}
					break;
				}
			}
			catch(Exception ex)
			{MessageBox.Show(ex.Message);}
		}
		private DataSet GetUserConfig()
		{
			DataSet dsDfClsConfig = null;
			Remoting oRemoting = null;
			string[] paramValues = null;

			// 1.CATEGORY 2.USER_ID 3.NAME
			paramValues = new string[3];

			switch(eSELECTOR_TYPE)
			{
				case SELECTOR_TYPE.DEFECT_FIELD_CHART:
					paramValues[0] = "CROSSRPT_ONE";
					paramValues[1] = strUserID;
					paramValues[2] = "CHART_CLASS";
					
					break;
				case SELECTOR_TYPE.DEFECT_FIELD_SHEET:
					//DEFECT CLASS FIELD DYNAMIC CONFIGURATION INFO QUERY
					paramValues[0] = "CROSSRPT_ONE";
					paramValues[1] = strUserID;
					paramValues[2] = "DF_CLASS";	
					break;
			}
			oRemoting = new Remoting();
			dsDfClsConfig = oRemoting.GetUserConfig(strUserID,"GetUserConfig",paramValues);
			return dsDfClsConfig;
		}
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			//업데이트 없으면 인서트
			switch(bExist)
			{
				case true:  //설정데이타가 존재함
					SetUserConfig(USER_CONFIG_QUERY_TYPE.UPDATE);
					break;
				case false: //설정데이타가 존재하지 않음
					SetUserConfig(USER_CONFIG_QUERY_TYPE.INSERT);
					break;
			}
			SetApply();
			MessageBox.Show("완료!");
		}

		private void btnApply_Click(object sender, System.EventArgs e)
		{
			SetApply();
		}

		private void SetApply()
		{
			// 시트 또는 차트에 선택사항 반영
			string strDynamicElements = string.Empty;
			string[] arrDynamicElements = null;
			CrossOne oCrossOne = null;
			System.Windows.Forms.Control oCrossOneControl = CrossOne.FromHandle(iCallerFormPtr);
			oCrossOne = (CrossOne)oCrossOneControl;
			// CROSS ONE 메소드 호출
			for(int i=0;i<this.lvDynamicSelector.Items.Count;i++)
			{
				if(lvDynamicSelector.Items[i].Checked == true)
				{
					strDynamicElements += lvDynamicSelector.Items[i].Text + ";";
				}
			}
			if(strDynamicElements.Length >0)
			{
				strDynamicElements = strDynamicElements.Substring(0,strDynamicElements.Length -1);
				arrDynamicElements = strDynamicElements.Split(Convert.ToChar(";"));
			}
			switch(eSELECTOR_TYPE)
			{
				case SELECTOR_TYPE.DEFECT_FIELD_CHART:
					oCrossOne.SetChartDynamicConfiguration(arrDynamicElements);
					break;
				case SELECTOR_TYPE.DEFECT_FIELD_SHEET:
					oCrossOne.SetSheetDynamicConfiguration(arrDynamicElements);
					break;
			}
		}
		private void btnConfirm_Click(object sender, System.EventArgs e)
		{
			SetApply();
			// 적용이 되어서 창이 닫힘
			this.Close();
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			// 창만 닫힌다.
			this.Close();
		}

		private void btnSelectAll_Click(object sender, System.EventArgs e)
		{
			for(int i=0;i<lvDynamicSelector.Items.Count;i++)
			{
				lvDynamicSelector.Items[i].Checked = true;
			}
		}

		private void btnSelectNone_Click(object sender, System.EventArgs e)
		{
			for(int i=0;i<lvDynamicSelector.Items.Count;i++)
			{
				lvDynamicSelector.Items[i].Checked = false;
			}
		}

		private void DynamicFieldSelector_Closed(object sender, System.EventArgs e)
		{
			CrossOne oCrossOne = null;
			System.Windows.Forms.Control oCrossOneControl = CrossOne.FromHandle(iCallerFormPtr);
			oCrossOne = (CrossOne)oCrossOneControl;
			switch(eSELECTOR_TYPE)
			{
				case SELECTOR_TYPE.DEFECT_FIELD_CHART:
					oCrossOne.bChartDynamicFieldSelector = false;
					break;
				case SELECTOR_TYPE.DEFECT_FIELD_SHEET:
					oCrossOne.bSheetDynamicFieldSelector = false;
					break;
			}
		}
	}
}
