using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using DACrux.Interface;

namespace DACrux.BStats.StatDialog
{
	/// <summary>
	/// 클래스  명: DlgFactorialDesign<br/>
	/// 클래스요약: 요인분석 설계의 대화창<br/>
	/// 작  성  자: MiracomInc<br/>
	/// 최초작성일: 2005-08-01<br/>
	/// 최종수정자: MiracomInc<br/>
	/// 최종수정일: 2005-12-31<br/>
	/// 상세  설명: 요인분석 설계의 대화창<br/>
	/// 변경  내용: <br/>
	/// </summary>
    public class DlgFactorialDesign : System.Windows.Forms.Form, iStatInformation
	{
		/// <summary>
		/// 요인 설계 확인 버튼
		/// </summary>
		private System.Windows.Forms.Button btnOK;

		/// <summary>
		/// 요인 설계 취소 버튼
		/// </summary>
		private System.Windows.Forms.Button btnCancel;

		/// <summary>
		/// 요인 설계 탭
		/// </summary>
		private System.Windows.Forms.TabControl tabControl1;

		/// <summary>
		/// 설계 탭 페이지
		/// </summary>
		private System.Windows.Forms.TabPage tabPage1;

		/// <summary>
		/// 스프레드 시트
		/// </summary>
		private FarPoint.Win.Spread.FpSpread fpSpread1;
		
		/// <summary>
		/// 스프레드 시트 뷰
		/// </summary>
		private FarPoint.Win.Spread.SheetView fpSpread1_Sheet1; 

		/// <summary>
		/// 그룹박스2
		/// </summary>
		private System.Windows.Forms.GroupBox groupBox2;

		/// <summary>
		/// 실험순서 그룹박스
		/// </summary>
		private System.Windows.Forms.GroupBox groupBox1;

		/// <summary>
		/// 반응변수의 개수
		/// </summary>
		private System.Windows.Forms.Label label2;

		/// <summary>
		/// 인자의 개수
		/// </summary>
		private System.Windows.Forms.Label label1;

		/// <summary>
		/// 그룹박스 3
		/// </summary>
		private System.Windows.Forms.GroupBox groupBox3;

		/// <summary>
		/// 이인자 실험수 목록
		/// </summary>
		private System.Windows.Forms.ListView listView4;

		/// <summary>
		/// 인자의 개수 
		/// </summary>
		private System.Windows.Forms.ComboBox comboBox1;
		/// <summary>
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// 요인 설계 columnHeader
		/// </summary>
		private System.Windows.Forms.ColumnHeader columnHeader1;

		
		/// <summary>
		/// 요인 설계 columnHeader
		/// </summary>
		private System.Windows.Forms.ColumnHeader columnHeader2;
		
		
		/// <summary>
		/// 요인 설계 columnHeader
		/// </summary>
		private System.Windows.Forms.ColumnHeader columnHeader3;

		
		/// <summary>
		/// 표준 실험 순서
		/// </summary>
		private System.Windows.Forms.RadioButton radioStandard;

		/// <summary>
		/// 램덤 실험 순서
		/// </summary>
		private System.Windows.Forms.RadioButton radioRand;

		/// <summary>
		/// 반응변수의 개수 NumericUpDown
		/// </summary>
		private System.Windows.Forms.NumericUpDown depCount;

		/// <summary>
		/// 데이터 소스
		/// </summary>
		private DataTable SheetTable;

        //Miracom.DACrux.Images.frmImage fmg = new Miracom.DACrux.Images.frmImage();


		/// <summary>
		/// DACrux 메인프레인 타이틀
		/// </summary>
		private string strDACruxTitle = string.Empty;

		/// <summary>
		/// DlgFactorialDesign 클래스의 생성자
		/// </summary>
		public DlgFactorialDesign()
		{
			//
			// Windows Form 디자이너 지원에 필요합니다.
			//
			InitializeComponent();
			CreateDataSet(Convert.ToInt32(comboBox1.SelectedItem.ToString()));
			setDataListView();
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
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.listView4 = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.radioStandard = new System.Windows.Forms.RadioButton();
			this.radioRand = new System.Windows.Forms.RadioButton();
			this.depCount = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.fpSpread1 = new FarPoint.Win.Spread.FpSpread();
			this.fpSpread1_Sheet1 = new FarPoint.Win.Spread.SheetView();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.depCount)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).BeginInit();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnOK.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
			this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.btnOK.Location = new System.Drawing.Point(344, 400);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 29;
			this.btnOK.Text = "확인";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnCancel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
			this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.btnCancel.Location = new System.Drawing.Point(432, 400);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 30;
			this.btnCancel.Text = "취소";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
			this.tabControl1.ItemSize = new System.Drawing.Size(58, 17);
			this.tabControl1.Location = new System.Drawing.Point(8, 8);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(508, 380);
			this.tabControl1.TabIndex = 28;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.groupBox3);
			this.tabPage1.Controls.Add(this.groupBox2);
			this.tabPage1.Controls.Add(this.fpSpread1);
			this.tabPage1.Location = new System.Drawing.Point(4, 21);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(500, 355);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "설계";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.listView4);
			this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox3.Location = new System.Drawing.Point(232, 16);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(248, 176);
			this.groupBox3.TabIndex = 27;
			this.groupBox3.TabStop = false;
			// 
			// listView4
			// 
			this.listView4.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						this.columnHeader1,
																						this.columnHeader2,
																						this.columnHeader3});
			this.listView4.FullRowSelect = true;
			this.listView4.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listView4.Location = new System.Drawing.Point(8, 16);
			this.listView4.Name = "listView4";
			this.listView4.Size = new System.Drawing.Size(232, 144);
			this.listView4.TabIndex = 0;
			this.listView4.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "실험수";
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "2**(k-p)";
			this.columnHeader2.Width = 105;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "해상도";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.comboBox1);
			this.groupBox2.Controls.Add(this.groupBox1);
			this.groupBox2.Controls.Add(this.depCount);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(16, 16);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(208, 176);
			this.groupBox2.TabIndex = 26;
			this.groupBox2.TabStop = false;
			// 
			// comboBox1
			// 
			this.comboBox1.Items.AddRange(new object[] {
														   "2",
														   "3",
														   "4",
														   "5",
														   "6",
														   "7",
														   "8",
														   "9",
														   "10",
														   "11",
														   "12",
														   "13",
														   "14",
														   "15"});
			this.comboBox1.Location = new System.Drawing.Point(152, 56);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(48, 23);
			this.comboBox1.TabIndex = 28;
			this.comboBox1.Text = "2";
			this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.radioStandard);
			this.groupBox1.Controls.Add(this.radioRand);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(8, 96);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(192, 64);
			this.groupBox1.TabIndex = 27;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "실험순서";
			// 
			// radioStandard
			// 
			this.radioStandard.Checked = true;
			this.radioStandard.Location = new System.Drawing.Point(104, 20);
			this.radioStandard.Name = "radioStandard";
			this.radioStandard.Size = new System.Drawing.Size(80, 24);
			this.radioStandard.TabIndex = 1;
			this.radioStandard.TabStop = true;
			this.radioStandard.Text = "표준";
			// 
			// radioRand
			// 
			this.radioRand.Location = new System.Drawing.Point(8, 20);
			this.radioRand.Name = "radioRand";
			this.radioRand.Size = new System.Drawing.Size(88, 24);
			this.radioRand.TabIndex = 0;
			this.radioRand.Text = "랜덤";
			// 
			// depCount
			// 
			this.depCount.Location = new System.Drawing.Point(152, 24);
			this.depCount.Maximum = new System.Decimal(new int[] {
																	 64,
																	 0,
																	 0,
																	 0});
			this.depCount.Minimum = new System.Decimal(new int[] {
																	 1,
																	 0,
																	 0,
																	 0});
			this.depCount.Name = "depCount";
			this.depCount.Size = new System.Drawing.Size(48, 21);
			this.depCount.TabIndex = 26;
			this.depCount.Value = new System.Decimal(new int[] {
																   1,
																   0,
																   0,
																   0});
			// 
			// label2
			// 
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Location = new System.Drawing.Point(16, 24);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(128, 23);
			this.label2.TabIndex = 24;
			this.label2.Text = "반응변수의 개수";
			// 
			// label1
			// 
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(16, 56);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(128, 23);
			this.label1.TabIndex = 23;
			this.label1.Text = "인자의 개수";
			// 
			// fpSpread1
			// 
			this.fpSpread1.Location = new System.Drawing.Point(16, 216);
			this.fpSpread1.Name = "fpSpread1";
			this.fpSpread1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
																				   this.fpSpread1_Sheet1});
			this.fpSpread1.Size = new System.Drawing.Size(464, 120);
			this.fpSpread1.TabIndex = 5;
			// 
			// fpSpread1_Sheet1
			// 
			this.fpSpread1_Sheet1.Reset();
			this.fpSpread1_Sheet1.SheetName = "Sheet1";
			// 
			// DlgFactorialDesign
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(528, 429);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.tabControl1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DlgFactorialDesign";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "요인 설계";
			this.Load += new System.EventHandler(this.DlgFactorialDesign_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.depCount)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion


		/// <summary>
		/// 인자의 개수 이벤트 함수
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			CreateDataSet(Convert.ToInt32(comboBox1.SelectedItem.ToString()));	
			this.listView4.Items.Clear();
			setDataListView();

		}

		#region createData

		/// <summary>
		/// 데이터 소스 생성 함수
		/// </summary>
		/// <param name="cols"></param>
		private void CreateDataSet(int cols) 
		{


            //string str1 = rr.GetString("STAT_DOE_FACTDESIGN_DIALOG_LBL08").ToString();
            //string str2 = rr.GetString("STAT_DOE_FACTDESIGN_DIALOG_LBL09").ToString();
            //string str3 = rr.GetString("STAT_DOE_FACTDESIGN_DIALOG_LBL10").ToString();

            //string str1 = DACrux.Languages.ILanguages.getResourceString("STAT_DOE_FACTDESIGN_DIALOG_LBL08").ToString();
            //string str2 = DACrux.Interface.ILanguages.getResourceString("STAT_DOE_FACTDESIGN_DIALOG_LBL09").ToString();
            //string str3 = DACrux.Interface.ILanguages.getResourceString("STAT_DOE_FACTDESIGN_DIALOG_LBL10").ToString();
            string str1 = "Point Type";
            string str2 = "Description";
            string str3 = "Number";

			SheetTable = new DataTable();
			SheetTable.Columns.Add(str1);
			SheetTable.Columns.Add(str2);
			SheetTable.Columns.Add(str3);
				
			for(int j=0 ; j < cols  ; j++)
			{	
				Object[] rows = new Object[3];
				rows[0] =  "Factor" + ChangeNumber2String(j);					
				rows[1] = "0";
				rows[2] = "1";
				SheetTable.Rows.Add(rows);				
			}

			fpSpread1_Sheet1.DataSource = SheetTable;
			fpSpread1.Show();
			

		}

		#endregion

		#region ChangeaNumber2String

		/// <summary>
		/// 아스키 코드에 따라 숫자를 문자로 바꾸어주는 함수
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		private string ChangeNumber2String( int i )
		{
			string strReturn = String.Empty ;
			int iQuotient = i / 26 ;
			int iRemainder = i % 26 + 65 ;

			strReturn = strReturn.PadLeft( iQuotient, 'A' ) ;
			strReturn += Convert.ToString( ( char )iRemainder ) ;
			return strReturn ;
		}

		#endregion

		#region setDataValue

		/// <summary>
		/// listView4를 초기화 하는 함수
		/// </summary>
		private void setDataListView()
		{
			int count = 0;
			string [,] factorDesign
				= new string[,] {	// 실험수 ,2^(K-p), 해상도
									{"2","2**(2)","Full"},	   // 0	

									{"3","2**(3-1)","III"},    // 1
									{"3","2**(3)","Full"},	   // 2								

									{"4","2**(4-1)","IV"},	   // 3
									{"4","2**(4)","Full"},	   // 4								

									{"5","2**(5-2)","III"},    // 5
									{"5","2**(5-1)","V"},      // 6
									{"5","2**(5)","Full"},	   // 7								

									{"6","2**(6-3)","III"},    // 8
									{"6","2**(6-2)","IV"},     // 9
									{"6","2**(6-1)","VI"},     // 10
									{"6","2**(6)","Full"},     // 11

									{"7","2**(7-4)","III"},	   // 12	
									{"7","2**(7-3)","IV"},	   // 13
									{"7","2**(7-2)","IV"},	   // 14	
									{"7","2**(7-1)","VII"},	   // 15
									{"7","2**(7)","Full"},	   // 16

									{"8","2**(8-4)","IV"},	   // 17	
									{"8","2**(8-3)","IV"},	   // 18
									{"8","2**(8-2)","V"},	   // 19
									{"8","2**(8-1)","VIII"},   // 20

									{"9","2**(9-5)","III"},	   // 21
									{"9","2**(9-4)","IV"},     // 22	
									{"9","2**(9-3)","IV"},	   // 23
									{"9","2**(9-2)","VI"},	   // 24	

									{"10","2**(10-6)","III"},  // 25
									{"10","2**(10-5)","IV"},   // 26
									{"10","2**(10-4)","IV"},   // 27	
									{"10","2**(10-3)","V"},	   // 28

									{"11","2**(11-7)","III"},  // 29
									{"11","2**(11-6)","IV"},   // 30
									{"11","2**(11-5)","IV"},   // 31
									{"11","2**(11-4)","V"},	   // 32									

									{"12","2**(12-8)","III"},  // 33
									{"12","2**(12-7)","IV"},   // 34	
									{"12","2**(12-6)","IV"},   // 35
									{"12","2**(12-5)","IV"},   // 36

									{"13","2**(13-9)","III"},  // 37	
									{"13","2**(13-8)","IV"},   // 38
									{"13","2**(13-7)","IV"},   // 39
									{"13","2**(13-6)","IV"},   // 40

									{"14","2**(14-10)","III"}, // 41
									{"14","2**(14-9)","IV"},   // 42	
									{"14","2**(14-8)","IV"},   // 43
									{"14","2**(14-7)","IV"},   // 44	

									{"15","2**(15-11)","III"}, // 45
									{"15","2**(15-10)","IV"},  // 46
									{"15","2**(15-9)","IV"},   // 47	
									{"15","2**(15-8)","IV"}	   // 48
								};							   
			
			
			string[] FactorAndRun = new string[3];


			switch(this.comboBox1.SelectedItem.ToString())
			{
				case "15":
					for(int k =0; k <4; k++)
					{
						string[] temp = new string[3];
						for(int j=0; j<3; j++)
							temp[j] = factorDesign[48-k,j];
						this.listView4.Items.Add((new ListViewItem(temp, count++ )));
					}
					break;

				case "14":
					for(int k =0; k <4; k++)
					{
						string[] temp = new string[3];
						for(int j=0; j<3; j++)
							temp[j] = factorDesign[44-k,j];
						this.listView4.Items.Add((new ListViewItem(temp, count++ )));
					}
					break;

				case "13":
					for(int k =0; k <4; k++)
					{
						string[] temp = new string[3];
						for(int j=0; j<3; j++)
							temp[j] = factorDesign[40-k,j];
						this.listView4.Items.Add((new ListViewItem(temp, count++ )));
					}
					break;

				case "12":
					for(int k =0; k <4; k++)
					{
						string[] temp = new string[3];
						for(int j=0; j<3; j++)
							temp[j] = factorDesign[36-k,j];
						this.listView4.Items.Add((new ListViewItem(temp, count++ )));
					}
					break;


				case "11":
					for(int k =0; k <4; k++)
					{
						string[] temp = new string[3];
						for(int j=0; j<3; j++)
							temp[j] = factorDesign[32-k,j];
						this.listView4.Items.Add((new ListViewItem(temp, count++ )));
					}
					break;


				case "10":
					for(int k =0; k <4; k++)
					{
						string[] temp = new string[3];
						for(int j=0; j<3; j++)
							temp[j] = factorDesign[28-k,j];
						this.listView4.Items.Add((new ListViewItem(temp, count++ )));
					}
					break;

				case "9":
					for(int k =0; k <4; k++)
					{
						string[] temp = new string[3];
						for(int j=0; j<3; j++)
							temp[j] = factorDesign[24-k,j];
						this.listView4.Items.Add((new ListViewItem(temp, count++ )));
					}
					break;

					
				case "8":
					for(int k =0; k <4; k++)
					{
						string[] temp = new string[3];
						for(int j=0; j<3; j++)
							temp[j] = factorDesign[20-k,j];
						this.listView4.Items.Add((new ListViewItem(temp, count++ )));
					}
					break;


				case "7":
					for(int k =0; k <5; k++)
					{
						string[] temp = new string[3];
						for(int j=0; j<3; j++)
							temp[j] = factorDesign[16-k,j];
						this.listView4.Items.Add((new ListViewItem(temp, count++ )));
					}
					break;


				case "6":
					for(int k =0; k <4; k++)
					{
						string[] temp = new string[3];
						for(int j=0; j<3; j++)
							temp[j] = factorDesign[11-k,j];
						this.listView4.Items.Add((new ListViewItem(temp, count++ )));
					}
					break;


				case "5":
					for(int k =0; k <3; k++)
					{
						string[] temp = new string[3];
						for(int j=0; j<3; j++)
							temp[j] = factorDesign[7-k,j];
						this.listView4.Items.Add((new ListViewItem(temp, count++ )));
					}
					break;

				case "4":
					for(int k =0; k <2; k++)
					{
						string[] temp = new string[3];
						for(int j=0; j<3; j++)
							temp[j] = factorDesign[4-k,j];
						this.listView4.Items.Add((new ListViewItem(temp, count++ )));
					}
					break;

				case "3":
					for(int k =0; k <2; k++)
					{
						string[] temp = new string[3];
						for(int j=0; j<3; j++)
							temp[j] = factorDesign[2-k,j];
						this.listView4.Items.Add((new ListViewItem(temp, count++ )));
					}
					break;

				case "2":
					for(int k =0; k <1; k++)
					{
						string[] temp = new string[3];
						for(int j=0; j<3; j++)
							temp[j] = factorDesign[0-k,j];
						this.listView4.Items.Add((new ListViewItem(temp, count++ )));
					}
					break;

			}						
			
		}

		#endregion

		/// <summary>
		/// 요인설계 확인 버튼
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnOK_Click(object sender, System.EventArgs e)
		{	
			
			int rows = Convert.ToInt32( this.comboBox1.SelectedItem.ToString() );

			string str = this.fpSpread1.ActiveSheet.GetValue(0,0).ToString();
			bool repeatName = false;

			for(int i = 1; i < rows ;i++)
			{
				if(str.Equals ( this.fpSpread1.ActiveSheet.GetValue(i,0).ToString() ) )
					repeatName = true;
				else
					str = this.fpSpread1.ActiveSheet.GetValue(i,0).ToString();
			}

			if(this.listView4.SelectedItems.Count == 0)
			{
				MessageBox.Show(DACrux.Interface.ILanguages.getResourceString("STAT_DOE_FACTDESIGN_DIALOG_MSG01"),this.strDACruxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if(repeatName)
			{
				MessageBox.Show(DACrux.Interface.ILanguages.getResourceString("STAT_DOE_FACTDESIGN_DIALOG_MSG02"),this.strDACruxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if(this.radioRand.Checked == true)
				Interface.FactorialDesign.ExcuteOrder = Interface.eExcuteOrder.Random;
			else
				Interface.FactorialDesign.ExcuteOrder = Interface.eExcuteOrder.Standard;

			Interface.FactorialDesign.Repeat = 1;
			Interface.FactorialDesign.NumberOfResponse = Convert.ToInt32( this.depCount.Value );
			this.selectDesignTable(this.listView4.SelectedItems[0].SubItems[1].Text);

			DataTable factor_sheet = new DataTable();

			factor_sheet.Columns.Add("index") ;				
			//factor_sheet.Columns.Add("실험순서") ;				
			for(int i = 0; i < rows ;i++)
			{
				factor_sheet.Columns.Add( this.fpSpread1.ActiveSheet.GetValue(i,0).ToString() ) ;				
			}

			//종속변수 수 크기만큼 컬럼을 추가한다.
			for(int k =0; k < this.depCount.Value; k++)
				factor_sheet.Columns.Add( "Variable Y" + k , Type.GetType("System.Double"));

			int[,] FactorModel = Interface.FactorialDesign.GetDefinitionArray();

			// 요인 설계 테이블을 생성한다.
			for(int i =0; i< FactorModel.GetLength(0); i++)
			{	
				ArrayList _rows = new ArrayList();

				for(int j=0; j<FactorModel.GetLength(1); j++)
					_rows.Add(FactorModel[i,j].ToString());
				
				for(int k =0; k < this.depCount.Value; k++)
					_rows.Add(null);

				factor_sheet.Rows.Add(_rows.ToArray() );
			}	
			
			DACrux.ExternalInterface.ExternalInterface.putData(factor_sheet);
			
			this.DialogResult = DialogResult.OK;
			this.Close();
		
		}


		/// <summary>
		/// 요인설계 취소 버튼
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();		
		}

		/// <summary>
		/// 요인 설계 폼 이벤트 함수
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DlgFactorialDesign_Load(object sender, System.EventArgs e)
		{
            //this.Icon = Icon.FromHandle( new Bitmap( fmg.imageList1.Images[Miracom.DACrux.Images.GetImage.FactorialDesign] ).GetHicon());
		}

		/// <summary>
		/// DesignTable을 초기화 한다.
		/// </summary>
		/// <param name="str"></param>
		private void selectDesignTable(string str)
		{
			switch(str) 
			{
				case "2**(2)":
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_2_Full;
					break;				 
				
				case "2**(3-1)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_3_Minus1;					
					break;	
				case "2**(3)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_3_Full;
					break;									

				case "2**(4-1)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_4_Minus1;
					break;	

				case "2**(4)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_4_Full;
					break;									

				case "2**(5-2)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_5_Minus2;
					break;	
				case "2**(5-1)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_5_Minus1;
					break;	
				case "2**(5)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_5_Full;
					break;									

				case "2**(6-3)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_6_Minus3;
					break;	
				case "2**(6-2)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_6_Minus2;
					break;	
				case "2**(6-1)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_6_Minus1;
					break;	
				case "2**(6)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_6_Full;
					break;		

				case "2**(7-4)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_7_Minus4;
					break;	
				case "2**(7-3)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_7_Minus3;
					break;	
				case "2**(7-2)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_7_Minus2;
					break;	
				case "2**(7-1)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_7_Minus1;
					break;	
				case "2**(7)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_7_Full;
					break;		

				case "2**(8-4)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_8_Minus4;
					break;	
				case "2**(8-3)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_8_Minus3;
					break;	
				case "2**(8-2)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_8_Minus2;
					break;	
				case "2**(8-1)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_8_Minus1;
					break;	

				case "2**(9-5)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_9_Minus5;
					break;	
				case "2**(9-4)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_9_Minus4;
					break;	
				case "2**(9-3)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_9_Minus3;
					break;	
				case "2**(9-2)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_9_Minus2;
					break;	

				case "2**(10-6)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_10_Minus6;
					break;	
				case "2**(10-5)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_10_Minus5;
					break;	
				case "2**(10-4)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_10_Minus4;
					break;	
				case "2**(10-3)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_10_Minus3;
					break;	

				case "2**(11-7)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_11_Minus7;
					break;	
				case "2**(11-6)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_11_Minus6;
					break;	
				case "2**(11-5)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_11_Minus5;
					break;	
				case "2**(11-4)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_11_Minus4;
					break;								

				case "2**(12-8)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_12_Minus8;
					break;	
				case "2**(12-7)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_12_Minus7;
					break;	
				case "2**(12-6)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_12_Minus6;
					break;	
				case "2**(12-5)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_12_Minus5;
					break;	

				case "2**(13-9)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_13_Minus9;
					break;	
				case "2**(13-8)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_13_Minus8;
					break;	
				case "2**(13-7)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_13_Minus7;
					break;	
				case "2**(13-6)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_13_Minus6;
					break;	

				case "2**(14-10)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_14_Minus10;
					break;
				case "2**(14-9)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_14_Minus9;
					break;	
				case "2**(14-8)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_14_Minus8;
					break;	
				case "2**(14-7)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_14_Minus7;
					break;	

				case "2**(15-11)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_15_Minus11;
					break;
				case "2**(15-10)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_15_Minus10;
					break;
				case "2**(15-9)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_15_Minus9;
					break;	
				case "2**(15-8)":  
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_15_Minus8;
					break;	

				default:
					Interface.FactorialDesign.DesignType = Interface.eDesignType.DesignType_2_2_Full;
					break;


			}

		}



        public ProjectManager.UI.GraphInformation[] GraphInformations
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public ProjectManager.UI.StatInformation StatInfo
        {
            get { throw new NotImplementedException(); }
        }
    }
}
