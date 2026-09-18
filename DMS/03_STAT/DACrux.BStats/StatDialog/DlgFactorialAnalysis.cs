using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using CenterSpace.NMath.Core;
using CenterSpace.NMath.Stats;



namespace DACrux.BStats.StatDialog
{
	/// <summary>
	/// 클래스  명: DlgFactorialAnalysis<br/>
	/// 클래스요약: 요인분석의 대화창<br/>
	/// 작  성  자: MiracomInc<br/>
	/// 최초작성일: 2005-08-01<br/>
	/// 최종수정자: MiracomInc<br/>
	/// 최종수정일: 2005-12-31<br/>
	/// 상세  설명: 요인분석의 대화창<br/>
	/// 변경  내용: <br/>
	/// </summary>
    public class DlgFactorialAnalysis : System.Windows.Forms.Form, iStatInformation
	{
		/// <summary>
		/// 요인분석의 입력구조 클래스
		/// </summary>
        public DACrux.BStats.StatisticsInput.inputFactorAnalysis m_Input = new DACrux.BStats.StatisticsInput.inputFactorAnalysis();		// 입력
		
		/// <summary>
		/// 요인 분석의 데이터 소스
		/// </summary>
		private DataTable _dataSource	= null;

		/// <summary>
		/// 요인분석의 분산분석 결과 테이블
		/// </summary>
		private DataTable anova = null;

		/// <summary>
		/// 요인분석의 ColumnHeader
		/// </summary>
		private System.Windows.Forms.ColumnHeader columnHeader2;

		/// <summary>
		/// 요인분석의 ColumnHeader
		/// </summary>
		private System.Windows.Forms.ColumnHeader columnHeader1;

		/// <summary>
		/// 취소버튼
		/// </summary>
		private System.Windows.Forms.Button btnCancel;

		/// <summary>
		/// 요인분석의 ColumnHeader
		/// </summary>
		private System.Windows.Forms.ColumnHeader columnHeader3;

		/// <summary>
		/// 확인 버튼
		/// </summary>
		private System.Windows.Forms.Button btnOK;

		/// <summary>
		/// 요인분석 탭 컨트롤
		/// </summary>
		private System.Windows.Forms.TabControl tabControl1;

		/// <summary>
		/// 요인분석 데이터 탭 페이지
		/// </summary>
		private System.Windows.Forms.TabPage tabPage1;

		/// <summary>
		/// 반응변수 리스트
		/// </summary>
		private System.Windows.Forms.ListView listView2;

		/// <summary>
		/// 변수목록 리스트
		/// </summary>
		private System.Windows.Forms.ListView listView1;

		/// <summary>
		/// 변수목록에서 반응변수로 선택된 리스트를 옮기는 버튼
		/// </summary>
		private System.Windows.Forms.Button button2;

		/// <summary>
		/// 반응변수에서 변수목록으로 선택된 리스트를 옮기는 버튼
		/// </summary>
		private System.Windows.Forms.Button button1;

		/// <summary>
		/// 반응변수 라벨
		/// </summary>
		private System.Windows.Forms.Label label2;

		/// <summary>
		/// 변수목록 라벨
		/// </summary>
		private System.Windows.Forms.Label label1;

		/// <summary>
		/// 통계량 탭페이지
		/// </summary>
		private System.Windows.Forms.TabPage tabPage2;

		/// <summary>
		/// 모형선택 탭 페이지
		/// </summary>
		private System.Windows.Forms.TabPage tabPage3;

		/// <summary>
		/// 인자 목록
		/// </summary>
		private System.Windows.Forms.ListView listView3;

		/// <summary>
		/// 요인분석 Columnheader
		/// </summary>
		private System.Windows.Forms.ColumnHeader columnHeader4;

		/// <summary>
		/// 변수목록에서 인자로 선택된 리스트를 옮기는 버튼
		/// </summary>
		private System.Windows.Forms.Button button3;

		/// <summary>
		/// 인자에서 변수목록으로 선택된 리스트를 옮기는 버튼
		/// </summary>
		private System.Windows.Forms.Button button4;

		/// <summary>
		/// 인자 라벨
		/// </summary>
		private System.Windows.Forms.Label label3;

		/// <summary>
		/// 주효과 체크박스
		/// </summary>
		private System.Windows.Forms.CheckBox checkBox3;
		
		/// <summary>
		/// 분산분석표 체크박스
		/// </summary>
		private System.Windows.Forms.CheckBox checkBox2;

		/// <summary>
		/// 모형정보 체크박스
		/// </summary>
		private System.Windows.Forms.CheckBox checkBox1;

		/// <summary>
		/// 요인분석 이미지 리스트
		/// </summary>
		private System.Windows.Forms.ImageList imageList1;

		/// <summary>
		/// 요인분석 Component Container
		/// </summary>
		private System.ComponentModel.IContainer components;

		/// <summary>
		/// 잔차도 체크박스
		/// </summary>
		private System.Windows.Forms.CheckBox checkBox7;
		
		/// <summary>
		/// 교호작용 그림 체크박스
		/// </summary>
		private System.Windows.Forms.CheckBox checkBox8;

		/// <summary>
		/// 주효과 그림 체크박스
		/// </summary>
		private System.Windows.Forms.CheckBox checkBox6;

		/// <summary>
		/// 교호작용 포함 체크박스
		/// </summary>
		private System.Windows.Forms.CheckBox checkBox4;

		/// <summary>
		/// 그래프 그룹박스
		/// </summary>
		private System.Windows.Forms.GroupBox groupBox2;

		/// <summary>
		/// 교호작용 체크박스
		/// </summary>
		private System.Windows.Forms.CheckBox checkBox9;

		/// <summary>
		/// 분산분석표 스프레드 시트
		/// </summary>
		private FarPoint.Win.Spread.FpSpread fpSpread1;

		/// <summary>
		/// 분산분석표 스프레드 시트 뷰
		/// </summary>
		private FarPoint.Win.Spread.SheetView fpSpread1_Sheet1;

		/// <summary>
		/// 풀링할 인자 목록
		/// </summary>
		private System.Windows.Forms.ListView listView4;

		/// <summary>
		/// 요인분석 ColumnHeader
		/// </summary>
		private System.Windows.Forms.ColumnHeader columnHeader5;

		/// <summary>
		/// 분산분석표에서 풀링할 인자 목록으로 선택된 인자를 옮기는 버튼
		/// </summary>
		private System.Windows.Forms.Button button5;

		/// <summary>
		/// 풀링한 인자 목록으로 선택된 인자를 분산분석표로 옮기는 버튼
		/// </summary>
		private System.Windows.Forms.Button button6;

		/// <summary>
		/// 분산분석표 라벨
		/// </summary>
		private System.Windows.Forms.Label label5;

		/// <summary>
		/// 풀링할 인자 라벨
		/// </summary>
		private System.Windows.Forms.Label label6;

		/// <summary>
		/// 풀링 그룹박스
		/// </summary>
		private System.Windows.Forms.GroupBox groupBox3;

		/// <summary>
		/// 사용자 정의 라디오 버튼
		/// </summary>
		private System.Windows.Forms.RadioButton radioButton1;

		/// <summary>
		/// 유의확률 라디오 버튼
		/// </summary>
		private System.Windows.Forms.RadioButton radioButton2;

		/// <summary>
		/// 유의확률 numericUpdown
		/// </summary>
		private System.Windows.Forms.NumericUpDown numericUpDown1;

		/// <summary>
		/// 풀링 버튼
		/// </summary>
		private System.Windows.Forms.Button button7;

		/// <summary>
		/// 반응변수 라벨 1
		/// </summary>
		private System.Windows.Forms.Label label4;

		/// <summary>
		/// 반응변수 라벨 2
		/// </summary>
		private System.Windows.Forms.Label label7;

		/// <summary>
		/// 통계량 그룹박스
		/// </summary>
		private System.Windows.Forms.GroupBox groupBox1;

        //Miracom.DACrux.Images.frmImage fmg = new Miracom.DACrux.Images.frmImage();


		/// <summary>
		/// DACrux 메인프레인 타이틀
		/// </summary>
		private string strDACruxTitle = string.Empty;


		/// <summary>
		/// DlgFactorialAnalysis 클래스의 생성자
		/// </summary>
		/// <param name="table"></param>
		/// <param name="IsWorkflow"></param>
		/// <param name="Path"></param>
		public DlgFactorialAnalysis(DataTable table, bool IsWorkflow, string Path)
		{
			_dataSource = table;

			InitializeComponent();
			initDataTableColumnInfor();

			foreach(Control c in this.tabPage1.Controls)
			{
				if (c is ListView)
				{
					((ListView)c).Columns[0].Width = 150;
                    //((ListView)c).SmallImageList = fmg.imageList1 ;
				}
			}
		}

		/// <summary>
		/// DlgFactorialAnalysis 클래스의 생성자
		/// </summary>
		/// <param name="table"></param>
		public DlgFactorialAnalysis(DataTable table)
		{

			_dataSource = table;
			InitializeComponent();
			InitializeForm();
			initDataTableColumnInfor();
		}

		/// <summary>
		/// 데이터 소스 초기화 함수
		/// </summary>
		private void initDataTableColumnInfor()
		{
			int cols = this._dataSource.Columns.Count;
			int count  =0;

			DataFrame dfTemp  = new DataFrame(this._dataSource);
			ArrayList array = new ArrayList();

			for(int i=0; i<dfTemp.Cols; i++)
			{
				array= this.CountLevelOfVar(((DFColumn)dfTemp[i]).ToStringArray() );

				//double temp = 0.0;

				//for(int j=0; j < array.Count; j++)
					//temp += Convert.ToDouble( array[j] );
					
				//if( array.Count == 2 && temp == 1.0)
				if(array.Count == 2)
					count++;
					
			}


			this.Input.rows = this._dataSource.Rows.Count;
			this.Input.cols = count;

		}


		/// <summary>
		/// 초기화 함수
		/// </summary>
		private void InitializeForm()
		{
			
			for(int i=0; i< _dataSource.Columns.Count ; i++ )
			{
				DataColumn col = _dataSource.Columns[i];
				ListViewItem item = null;
				
				if ( col.DataType == typeof(double) || col.DataType == typeof(int)  || col.DataType == typeof(System.Decimal) ) 
				{
					item = new ListViewItem(col.ColumnName); //,Miracom.DACrux.Images.GetImage.Type_Number);
                    item.Tag = "Miracom.DACrux.Images.GetImage.Type_Number";
				}
				else if ( col.DataType == typeof(string))
				{
					item = new ListViewItem(col.ColumnName);//,Miracom.DACrux.Images.GetImage.Type_Char);
                    item.Tag = "Miracom.DACrux.Images.GetImage.Type_Char";
				}
				else
				{
                    item = new ListViewItem(col.ColumnName); //,Miracom.DACrux.Images.GetImage.Type_DateTime);
                    item.Tag = "Miracom.DACrux.Images.GetImage.Type_DateTime";
				}
				
				listView1.Items.Add( item );
			}

			listView1.Columns[0].Width = 150;
            //listView1.SmallImageList = fmg.imageList1 ;
			
            //listView2.SmallImageList = fmg.imageList1 ; 
			listView2.Columns[0].Width = 150;

            //listView3.SmallImageList = fmg.imageList1 ;
			listView3.Columns[0].Width = 150;

			this.checkBox8.Enabled = false;
			this.checkBox9.Enabled = false;

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
			this.components = new System.ComponentModel.Container();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.btnCancel = new System.Windows.Forms.Button();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.btnOK = new System.Windows.Forms.Button();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.checkBox4 = new System.Windows.Forms.CheckBox();
			this.listView3 = new System.Windows.Forms.ListView();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.listView2 = new System.Windows.Forms.ListView();
			this.listView1 = new System.Windows.Forms.ListView();
			this.button2 = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.checkBox9 = new System.Windows.Forms.CheckBox();
			this.checkBox3 = new System.Windows.Forms.CheckBox();
			this.checkBox2 = new System.Windows.Forms.CheckBox();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.checkBox7 = new System.Windows.Forms.CheckBox();
			this.checkBox8 = new System.Windows.Forms.CheckBox();
			this.checkBox6 = new System.Windows.Forms.CheckBox();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.label7 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.button7 = new System.Windows.Forms.Button();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.radioButton2 = new System.Windows.Forms.RadioButton();
			this.radioButton1 = new System.Windows.Forms.RadioButton();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.listView4 = new System.Windows.Forms.ListView();
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.button5 = new System.Windows.Forms.Button();
			this.button6 = new System.Windows.Forms.Button();
			this.fpSpread1 = new FarPoint.Win.Spread.FpSpread();
			this.fpSpread1_Sheet1 = new FarPoint.Win.Spread.SheetView();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.groupBox3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).BeginInit();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnCancel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnCancel.Location = new System.Drawing.Point(429, 408);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 8;
			this.btnCancel.Text = "취소";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOK
			// 
			this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnOK.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnOK.Location = new System.Drawing.Point(341, 408);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 7;
			this.btnOK.Text = "확인";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(512, 400);
			this.tabControl1.TabIndex = 6;
			this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.checkBox4);
			this.tabPage1.Controls.Add(this.listView3);
			this.tabPage1.Controls.Add(this.button3);
			this.tabPage1.Controls.Add(this.button4);
			this.tabPage1.Controls.Add(this.label3);
			this.tabPage1.Controls.Add(this.listView2);
			this.tabPage1.Controls.Add(this.listView1);
			this.tabPage1.Controls.Add(this.button2);
			this.tabPage1.Controls.Add(this.button1);
			this.tabPage1.Controls.Add(this.label2);
			this.tabPage1.Controls.Add(this.label1);
			this.tabPage1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.tabPage1.Location = new System.Drawing.Point(4, 24);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(504, 372);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = " 데이터 ";
			// 
			// checkBox4
			// 
			this.checkBox4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBox4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.checkBox4.Location = new System.Drawing.Point(312, 336);
			this.checkBox4.Name = "checkBox4";
			this.checkBox4.Size = new System.Drawing.Size(184, 24);
			this.checkBox4.TabIndex = 17;
			this.checkBox4.Text = "교호작용 포함";
			this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
			// 
			// listView3
			// 
			this.listView3.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						this.columnHeader4});
			this.listView3.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.listView3.Location = new System.Drawing.Point(308, 128);
			this.listView3.Name = "listView3";
			this.listView3.Size = new System.Drawing.Size(176, 192);
			this.listView3.TabIndex = 11;
			this.listView3.View = System.Windows.Forms.View.Details;
			// 
			// button3
			// 
			this.button3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button3.Location = new System.Drawing.Point(224, 240);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(32, 23);
			this.button3.TabIndex = 10;
			this.button3.Text = "▶";
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// button4
			// 
			this.button4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button4.Location = new System.Drawing.Point(224, 267);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(32, 23);
			this.button4.TabIndex = 9;
			this.button4.Text = "◀";
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// label3
			// 
			this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(306, 112);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(100, 64);
			this.label3.TabIndex = 8;
			this.label3.Text = "인자";
			// 
			// listView2
			// 
			this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						this.columnHeader2});
			this.listView2.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.listView2.Location = new System.Drawing.Point(305, 48);
			this.listView2.Name = "listView2";
			this.listView2.Size = new System.Drawing.Size(176, 24);
			this.listView2.TabIndex = 7;
			this.listView2.View = System.Windows.Forms.View.Details;
			// 
			// listView1
			// 
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						this.columnHeader1});
			this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.listView1.Location = new System.Drawing.Point(16, 48);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(184, 272);
			this.listView1.TabIndex = 6;
			this.listView1.View = System.Windows.Forms.View.Details;
			// 
			// button2
			// 
			this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button2.Location = new System.Drawing.Point(226, 56);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(32, 23);
			this.button2.TabIndex = 5;
			this.button2.Text = "▶";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button1
			// 
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button1.Location = new System.Drawing.Point(226, 80);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(32, 23);
			this.button1.TabIndex = 4;
			this.button1.Text = "◀";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label2
			// 
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(306, 31);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(157, 23);
			this.label2.TabIndex = 1;
			this.label2.Text = "반응 변수";
			// 
			// label1
			// 
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(16, 31);
			this.label1.Name = "label1";
			this.label1.TabIndex = 0;
			this.label1.Text = "변수 목록";
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.groupBox1);
			this.tabPage2.Controls.Add(this.groupBox2);
			this.tabPage2.Location = new System.Drawing.Point(4, 24);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(504, 372);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = " 통계량 ";
			this.tabPage2.Visible = false;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.checkBox9);
			this.groupBox1.Controls.Add(this.checkBox3);
			this.groupBox1.Controls.Add(this.checkBox2);
			this.groupBox1.Controls.Add(this.checkBox1);
			this.groupBox1.Location = new System.Drawing.Point(24, 24);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(208, 288);
			this.groupBox1.TabIndex = 18;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "통계량";
			// 
			// checkBox9
			// 
			this.checkBox9.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBox9.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.checkBox9.Location = new System.Drawing.Point(32, 160);
			this.checkBox9.Name = "checkBox9";
			this.checkBox9.Size = new System.Drawing.Size(144, 24);
			this.checkBox9.TabIndex = 12;
			this.checkBox9.Text = "교호작용";
			// 
			// checkBox3
			// 
			this.checkBox3.Checked = true;
			this.checkBox3.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBox3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.checkBox3.Location = new System.Drawing.Point(32, 120);
			this.checkBox3.Name = "checkBox3";
			this.checkBox3.Size = new System.Drawing.Size(152, 24);
			this.checkBox3.TabIndex = 11;
			this.checkBox3.Text = "주효과";
			// 
			// checkBox2
			// 
			this.checkBox2.Checked = true;
			this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBox2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.checkBox2.Location = new System.Drawing.Point(32, 80);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.TabIndex = 10;
			this.checkBox2.Text = "분산 분석표";
			// 
			// checkBox1
			// 
			this.checkBox1.Checked = true;
			this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBox1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.checkBox1.Location = new System.Drawing.Point(32, 40);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(144, 24);
			this.checkBox1.TabIndex = 9;
			this.checkBox1.Text = "모형 정보";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.checkBox7);
			this.groupBox2.Controls.Add(this.checkBox8);
			this.groupBox2.Controls.Add(this.checkBox6);
			this.groupBox2.Location = new System.Drawing.Point(256, 24);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(224, 288);
			this.groupBox2.TabIndex = 17;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "그래프";
			// 
			// checkBox7
			// 
			this.checkBox7.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBox7.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.checkBox7.Location = new System.Drawing.Point(24, 96);
			this.checkBox7.Name = "checkBox7";
			this.checkBox7.Size = new System.Drawing.Size(168, 24);
			this.checkBox7.TabIndex = 15;
			this.checkBox7.Text = "잔차도";
			this.checkBox7.Visible = false;
			// 
			// checkBox8
			// 
			this.checkBox8.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBox8.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.checkBox8.Location = new System.Drawing.Point(24, 64);
			this.checkBox8.Name = "checkBox8";
			this.checkBox8.Size = new System.Drawing.Size(176, 24);
			this.checkBox8.TabIndex = 14;
			this.checkBox8.Text = "교호작용 그림";
			this.checkBox8.Visible = false;
			// 
			// checkBox6
			// 
			this.checkBox6.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBox6.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.checkBox6.Location = new System.Drawing.Point(24, 32);
			this.checkBox6.Name = "checkBox6";
			this.checkBox6.Size = new System.Drawing.Size(176, 24);
			this.checkBox6.TabIndex = 13;
			this.checkBox6.Text = "주효과 그림";
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.label7);
			this.tabPage3.Controls.Add(this.label4);
			this.tabPage3.Controls.Add(this.groupBox3);
			this.tabPage3.Controls.Add(this.label6);
			this.tabPage3.Controls.Add(this.label5);
			this.tabPage3.Controls.Add(this.listView4);
			this.tabPage3.Controls.Add(this.button5);
			this.tabPage3.Controls.Add(this.button6);
			this.tabPage3.Controls.Add(this.fpSpread1);
			this.tabPage3.Location = new System.Drawing.Point(4, 24);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Size = new System.Drawing.Size(504, 372);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "모형선택";
			this.tabPage3.Visible = false;
			// 
			// label7
			// 
			this.label7.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label7.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label7.Location = new System.Drawing.Point(376, 13);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(104, 23);
			this.label7.TabIndex = 39;
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label4
			// 
			this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label4.Location = new System.Drawing.Point(240, 14);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(128, 23);
			this.label4.TabIndex = 38;
			this.label4.Text = "반응 변수:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.button7);
			this.groupBox3.Controls.Add(this.numericUpDown1);
			this.groupBox3.Controls.Add(this.radioButton2);
			this.groupBox3.Controls.Add(this.radioButton1);
			this.groupBox3.Location = new System.Drawing.Point(216, 248);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(264, 96);
			this.groupBox3.TabIndex = 37;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "풀링";
			// 
			// button7
			// 
			this.button7.Enabled = false;
			this.button7.Location = new System.Drawing.Point(168, 56);
			this.button7.Name = "button7";
			this.button7.Size = new System.Drawing.Size(80, 23);
			this.button7.TabIndex = 3;
			this.button7.Text = "풀링";
			this.button7.Click += new System.EventHandler(this.button7_Click);
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.DecimalPlaces = 4;
			this.numericUpDown1.Increment = new System.Decimal(new int[] {
																			 5,
																			 0,
																			 0,
																			 131072});
			this.numericUpDown1.Location = new System.Drawing.Point(64, 56);
			this.numericUpDown1.Maximum = new System.Decimal(new int[] {
																		   1,
																		   0,
																		   0,
																		   0});
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size(80, 21);
			this.numericUpDown1.TabIndex = 2;
			this.numericUpDown1.Value = new System.Decimal(new int[] {
																		 15,
																		 0,
																		 0,
																		 131072});
			// 
			// radioButton2
			// 
			this.radioButton2.Location = new System.Drawing.Point(16, 56);
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.Size = new System.Drawing.Size(48, 24);
			this.radioButton2.TabIndex = 1;
			this.radioButton2.Text = "P >";
			this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
			// 
			// radioButton1
			// 
			this.radioButton1.Checked = true;
			this.radioButton1.Location = new System.Drawing.Point(16, 24);
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.Size = new System.Drawing.Size(152, 24);
			this.radioButton1.TabIndex = 0;
			this.radioButton1.TabStop = true;
			this.radioButton1.Text = "사용자정의";
			this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8, 216);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(120, 23);
			this.label6.TabIndex = 36;
			this.label6.Text = "풀링할 인자:";
			this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(18, 16);
			this.label5.Name = "label5";
			this.label5.TabIndex = 35;
			this.label5.Text = "분산분석표";
			// 
			// listView4
			// 
			this.listView4.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						this.columnHeader5});
			this.listView4.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.listView4.Location = new System.Drawing.Point(24, 248);
			this.listView4.Name = "listView4";
			this.listView4.Size = new System.Drawing.Size(176, 112);
			this.listView4.TabIndex = 6;
			this.listView4.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader5
			// 
			this.columnHeader5.Width = 170;
			// 
			// button5
			// 
			this.button5.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button5.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.button5.Location = new System.Drawing.Point(168, 216);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(24, 23);
			this.button5.TabIndex = 33;
			this.button5.Text = "▼";
			this.button5.Click += new System.EventHandler(this.button5_Click);
			// 
			// button6
			// 
			this.button6.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button6.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.button6.Location = new System.Drawing.Point(136, 216);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(24, 23);
			this.button6.TabIndex = 32;
			this.button6.Text = "▲";
			this.button6.Click += new System.EventHandler(this.button6_Click);
			// 
			// fpSpread1
			// 
			this.fpSpread1.Location = new System.Drawing.Point(18, 48);
			this.fpSpread1.Name = "fpSpread1";
			this.fpSpread1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
																				   this.fpSpread1_Sheet1});
			this.fpSpread1.Size = new System.Drawing.Size(470, 152);
			this.fpSpread1.TabIndex = 29;
			this.fpSpread1.TabStrip.ActiveSheetTab.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.fpSpread1.TabStrip.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(255)));
			this.fpSpread1.TabStrip.ButtonPolicy = FarPoint.Win.Spread.TabStripButtonPolicy.AsNeeded;
			this.fpSpread1.TabStrip.DefaultSheetTab.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.fpSpread1.TabStripPolicy = FarPoint.Win.Spread.TabStripPolicy.Never;
			this.fpSpread1.ColumnWidthChanged += new FarPoint.Win.Spread.ColumnWidthChangedEventHandler(this.fpSpread1_ColumnWidthChanged);
			this.fpSpread1.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpSpread1_CellClick);
			this.fpSpread1.RowHeightChanged += new FarPoint.Win.Spread.RowHeightChangedEventHandler(this.fpSpread1_RowHeightChanged);
			// 
			// fpSpread1_Sheet1
			// 
			this.fpSpread1_Sheet1.Reset();
			this.fpSpread1_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ExtendedSelect;
			this.fpSpread1_Sheet1.RowHeader.Columns.Default.Resizable = false;
			this.fpSpread1_Sheet1.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.MultiRange;
			this.fpSpread1_Sheet1.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Row;
			this.fpSpread1_Sheet1.SheetName = "Sheet1";
			// 
			// imageList1
			// 
			this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// DlgFactorialAnalysis
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(512, 435);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.tabControl1);
			this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DlgFactorialAnalysis";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "요인 분석";
			this.Load += new System.EventHandler(this.DlgFactorialAnalysis_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// 요인분석 클래스의 입력 프로퍼티
		/// </summary>
        public DACrux.BStats.StatisticsInput.inputFactorAnalysis Input
		{
			get 
			{
				return this.m_Input;
			}
		}

		/// <summary>
		/// 확인 버튼 이벤트함수
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnOK_Click(object sender, System.EventArgs e)
		{			
			this.m_Input.analysisParams.Clear();
			this.m_Input.analysisVariables.Clear();

			if (this.listView2.Items.Count < 1)
			{
                //MessageBox.Show(Miracom.DACrux.Interface.ILanguages.getResourceString("STAT_DOE_FACTANAL_DIALOG_MSG01"),this.strDACruxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show("STAT_DOE_FACTANAL_DIALOG_MSG01", this.strDACruxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return ;
			}

			if (this.listView3.Items.Count < 1)
			{
                //MessageBox.Show(Miracom.DACrux.Interface.ILanguages.getResourceString("STAT_DOE_FACTANAL_DIALOG_MSG02"),this.strDACruxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show("STAT_DOE_FACTANAL_DIALOG_MSG02", this.strDACruxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return ;

			}
		

			// 반응 변수
			for( int i =0; i < this.listView2.Items.Count; i++)
				this.m_Input.analysisVariables.Add(this.listView2.Items[i].Text);
			
			// 인자
			for( int i =0; i < this.listView3.Items.Count; i++)
				this.m_Input.analysisParams.Add(this.listView3.Items[i].Text);

			if(! this.setFactorType() )
			{
				MessageBox.Show("STAT_DOE_FACTANAL_DIALOG_MSG11",this.strDACruxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
			
			
			if( this.checkFactorType() )
			{
				MessageBox.Show("STAT_DOE_FACTANAL_DIALOG_MSG11",this.strDACruxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
			
			
			this.m_Input.bDescriptiveStatistics	= this.checkBox1.Checked;
			this.m_Input.bAnovaAnalysis			= this.checkBox2.Checked;	
			this.m_Input.bInteractionEffect      = this.checkBox9.Checked;
			this.m_Input.bMainEffect				= this.checkBox3.Checked;	

			this.m_Input.bInteractionDiagram     = this.checkBox8.Checked;
			this.m_Input.bMainEffectDiagram      = this.checkBox6.Checked;
			this.m_Input.bResidualPlot			= this.checkBox7.Checked;


			if(this.listView3.Items.Count > 0 )
			{
				this.Input.analysisParams.Clear();
				// 인자
				for( int i =0; i < this.listView3.Items.Count; i++)
				{
					bool bPool = false;

					for(int k=0; k<this.Input.pooledParams.Count;k++)
					{	
						if( this.listView3.Items[i].Text.Equals(this.Input.pooledParams[k].ToString()))
							bPool = true;															
					}

					if(!bPool)
						this.m_Input.analysisParams.Add(this.listView3.Items[i].Text);
				}
			}				

			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		/// <summary>
		/// 변수목록에서 반응변수로 선택된 리스트를 옮기는 함수
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button2_Click(object sender, System.EventArgs e)
		{
			if (this.listView2.Items.Count > 0)
			{
				MessageBox.Show("STAT_DOE_FACTANAL_DIALOG_MSG03",this.strDACruxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return ;
			}
			
			if ( this.listView1.SelectedItems.Count < 1) return ;

			for( int i =0; i < this.listView1.SelectedItems.Count; i++)
				if ( this.listView1.SelectedItems[i].Tag.ToString() != "Miracom.DACrux.Images.GetImage.Type_Number" ) // NumberCellType
				{
					MessageBox.Show("STAT_DOE_FACTANAL_DIALOG_MSG04",this.strDACruxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
					return ;
				}

//			for( int i =0; i < this.listView1.SelectedItems.Count; i++)
//			{
//				ListViewItem item = new ListViewItem(this.listView1.SelectedItems[i].Text,(int)this.listView1.SelectedItems[i].Tag);
//				item.Tag = this.listView1.SelectedItems[i].Tag;
//				this.listView2.Items.Add(item);
//			}

			ListViewItem item = new ListViewItem(this.listView1.SelectedItems[0].Text,(string)this.listView1.SelectedItems[0].Tag);
			item.Tag = this.listView1.SelectedItems[0].Tag;
			this.listView2.Items.Add(item);

			int selectCnt = this.listView1.SelectedItems.Count;
			for( int i =0; i < selectCnt; i++)
				listView1.Items.Remove(listView1.SelectedItems[selectCnt - i - 1] );

			this.label7.Text = this.listView2.Items[0].Text;
		}

		/// <summary>
		/// 반응변수에서 변수목록로  선택된 리스트를 옮기는 함수
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button1_Click(object sender, System.EventArgs e)
		{
			if (this.listView2.SelectedItems.Count < 1) return;

			ListViewItem item = null;

			for(int i=0 ;i < this.listView2.SelectedItems.Count ; i++)
			{
				item = new ListViewItem(this.listView2.SelectedItems[i].Text,(string)this.listView2.SelectedItems[i].Tag);
				item.Tag = this.listView2.SelectedItems[i].Tag;
				this.listView1.Items.Add(item);
			}
			
			int selectCnt = this.listView2.SelectedItems.Count;
			for( int i =0; i < selectCnt; i++)
				listView2.Items.Remove(listView2.SelectedItems[selectCnt - i - 1] );

			this.label7.Text = "";
		}


		/// <summary>
		/// 변수목록에서 인자로  선택된 리스트를 옮기는 함수
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button3_Click(object sender, System.EventArgs e)
		{
			
			for( int i =0; i < this.listView1.SelectedItems.Count; i++)
			{
				ListViewItem item = new ListViewItem(this.listView1.SelectedItems[i].Text,(string)this.listView1.SelectedItems[i].Tag);
				item.Tag = this.listView1.SelectedItems[i].Tag;
				this.listView3.Items.Add(item);
			}

			int selectCnt = this.listView1.SelectedItems.Count;
			for( int i =0; i < selectCnt; i++)
				listView1.Items.Remove(listView1.SelectedItems[selectCnt - i - 1] );


			if(this.listView3.Items.Count >= 2)
				this.checkBox8.Visible = true;
			else
				this.checkBox8.Visible = false;
		}

		/// <summary>
		/// 인자에서 변수목록로  선택된 리스트를 옮기는 함수
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button4_Click(object sender, System.EventArgs e)
		{
			if (this.listView3.SelectedItems.Count < 1) return;

			ListViewItem item = null;

			for(int i=0 ;i < this.listView3.SelectedItems.Count ; i++)
			{
				item = new ListViewItem(this.listView3.SelectedItems[i].Text,(string)this.listView3.SelectedItems[i].Tag);
				item.Tag = this.listView3.SelectedItems[i].Tag;
				this.listView1.Items.Add(item);
			}
			
			int selectCnt = this.listView3.SelectedItems.Count;
			for( int i =0; i < selectCnt; i++)
				listView3.Items.Remove(listView3.SelectedItems[selectCnt - i - 1] );

			if(this.listView3.Items.Count >= 2)
				this.checkBox8.Visible = true;
			else
				this.checkBox8.Visible = false;

		}



		/// <summary>
		/// 취소버튼 이벤트 함수
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}


		/// <summary>
		/// 그룹별 수가 동일한 여부를 판단하는 함수
		/// </summary>
		/// <param name="table"></param>
		/// <returns></returns>
		private bool isEqualOfGroup (DataTable table)
		{
			DataFrame df = new DataFrame(table);
			bool isEqual = false;
			ArrayList alGroupName = null;		// 그룹별 이름
			ArrayList alGroupSize = null;		// 그룹별 갯수	
			
						
			for(int i=0 ;i < this.listView3.Items.Count ; i++)
			{
				string colName = this.listView3.Items[i].Text;
				alGroupName = new ArrayList();
				alGroupSize = new ArrayList();

				df.SortRows(df.IndexOfColumn(colName));

				DFColumn dfgCol =  (DFColumn)df[colName];	
	
				string tempNameOld = dfgCol[0].ToString();
				string tempName    = string.Empty;
				int tempSize       = 0;

				for(int k = 0; k < dfgCol.Count; k++)
				{
					tempName = dfgCol[k].ToString();

					// 서브그룹 변수에 널이 있는 경우, 건너뛴다.
					if(tempName == ".") continue;
					if(tempName != tempNameOld)
					{
						if(tempNameOld != ".")
						{
							alGroupName.Add(tempNameOld);
							alGroupSize.Add(tempSize);					
							tempSize = 1;
						}
						else
						{
							tempSize++;
						}
					}
					else
					{
						tempSize++;
					}
					tempNameOld = tempName;
				}
				// 마지막 그룹 추가
				alGroupName.Add(tempNameOld);
				alGroupSize.Add(tempSize);												
				
				int repeatOfGroup =  Convert.ToInt32( alGroupSize[0].ToString());

				for(int p=0; p<alGroupSize.Count; p++)
				{
					if(repeatOfGroup !=  Convert.ToInt32( alGroupSize[i].ToString() ))
						return true;

					repeatOfGroup = Convert.ToInt32( alGroupSize[i].ToString());
				}

				alGroupName = null;
				alGroupSize = null;
			}

			return isEqual;
		}


		/// <summary>
		/// 결측치 여부를 판단하는 함수
		/// </summary>
		/// <param name="table"></param>
		/// <returns></returns>
		private bool IsMissingValue(DataTable table)
		{
			DataFrame df = new DataFrame(table);
			bool isMissing  = false;

						
			for(int i=0 ;i < this.listView2.Items.Count ; i++)
			{
				string colName = this.listView2.Items[i].Text;

				DFColumn dfgCol =  (DFColumn)df[colName];

				for(int k = 0; k < dfgCol.Count; k++)
				{
					isMissing = dfgCol.IsMissing(k);
					if(isMissing)
						return isMissing;
				}
			}
			
			
			for(int i=0 ;i < this.listView3.Items.Count ; i++)
			{
				string colName = this.listView3.Items[i].Text;

				DFColumn dfgCol =  (DFColumn)df[colName];

				for(int k = 0; k < dfgCol.Count; k++)
				{
					isMissing = dfgCol.IsMissing(k);

					if(isMissing)
						return isMissing;
				}

			}

			return isMissing;
			
		}


		/// <summary>
		/// 교호작용 포함 이벤트 함수
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void checkBox4_CheckedChanged(object sender, System.EventArgs e)
		{
			if(this.checkBox4.Checked == false)
			{
				this.checkBox8.Enabled = false;
				this.checkBox9.Enabled = false;
			}
			else
			{
				this.checkBox8.Enabled = true;
				this.checkBox9.Enabled = true;
			}			
		
		}

		/// <summary>
		/// 풀링할 인자에서 분산분석표로 인자를 옮기며 분산분석표를 재로딩 하는 함수
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button6_Click(object sender, System.EventArgs e)
		{
			if (this.listView4.SelectedItems.Count < 1) return;

			for(int i=0 ;i < this.listView4.SelectedItems.Count ; i++)
			{			
				Object[] array = this.Input.pooledParams.ToArray();			
		
				for(int k=0; k< array.Length; k++)
				{
					if(array[k].ToString().Equals(this.listView4.SelectedItems[i].Text))
					{
						this.Input.pooledParams.RemoveAt(k);
						
					}
				}

			}

			int selectCnt = this.listView4.SelectedItems.Count;
			for( int i =0; i < selectCnt; i++)
				listView4.Items.Remove(listView4.SelectedItems[selectCnt - i - 1] );


			if(this.listView3.Items.Count > 0 )
			{
				this.Input.analysisParams.Clear();
				// 인자
				for( int i =0; i < this.listView3.Items.Count; i++)
				{
					

					bool bPool = false;

					for(int k=0; k<this.Input.pooledParams.Count;k++)
					{	
						if( this.listView3.Items[i].Text.Equals(this.Input.pooledParams[k].ToString()))
							bPool = true;															
					}

					if(!bPool)
						this.m_Input.analysisParams.Add(this.listView3.Items[i].Text);
				}
			}				


			this.anova = this.getAnovaTable(this._dataSource,this.Input);	
		
			if(this.anova == null)
				return;
		
			fpSpread1_Sheet1.DataSource = this.anova;
			fpSpread1_Sheet1.Columns[0].Width = 80;
			fpSpread1_Sheet1.Columns[1].Width = 25;
			fpSpread1_Sheet1.Columns[2].Width = 110;
			fpSpread1_Sheet1.Columns[3].Width = 110;
			fpSpread1_Sheet1.Columns[4].Width = 60;
			fpSpread1_Sheet1.Columns[5].Width = 60;

			fpSpread1_Sheet1.RowHeaderVisible = false;
		
			fpSpread1.Show();
		
		}

		/// <summary>
		/// 분산분석표에서 풀링할 인자로 선택된 인자를 옮기며 분산분석표를 재계산하는 함수
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>

		private void button5_Click(object sender, System.EventArgs e)
		{
			FarPoint.Win.Spread.Model.CellRange[] cr;

			cr = fpSpread1_Sheet1.GetSelections();

			if (cr == null || cr.Length < 1)
				return;

			
			bool bErrorORToTal = false;

			for( int iCr = cr.Length - 1 ; iCr >= 0  ; iCr-- ) 
			{
				for( int i = cr[iCr].Row + cr[iCr].RowCount - 1 ; i >= cr[iCr].Row ; i-- ) 
				{	
					if(fpSpread1_Sheet1.GetValue(i,0).ToString().Equals("Error") || fpSpread1_Sheet1.GetValue(i,0).ToString().Equals("Total"))
						bErrorORToTal = true;					
				}
			}

			if(bErrorORToTal)
			{
				MessageBox.Show("STAT_DOE_FACTANAL_DIALOG_MSG05",this.strDACruxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			this.Input.bPooling = true;

			this.Input.pooledParams.Clear();

			for(int i=0; i< this.listView4.Items.Count; i++)
			{
				this.Input.pooledParams.Add(this.listView4.Items[i].Text);
			}

			for( int iCr = cr.Length - 1 ; iCr >= 0  ; iCr-- ) 
			{
				for( int i = cr[iCr].Row + cr[iCr].RowCount - 1 ; i >= cr[iCr].Row ; i-- ) 
				{	
					this.Input.pooledParams.Add(fpSpread1_Sheet1.GetValue(i,0).ToString());		
				}
			}

			if(this.Input.pooledParams.Count > 0 )
			{
				if(this.listView3.Items.Count > 0 )
				{
					this.Input.analysisParams.Clear();
					// 인자
					for( int i =0; i < this.listView3.Items.Count; i++)
					{
						bool bPool = false;

						for(int k=0; k<this.Input.pooledParams.Count;k++)
						{							
							if( this.listView3.Items[i].Text.Equals(this.Input.pooledParams[k].ToString()))
								bPool = true;															
						}

						if(!bPool)
							this.m_Input.analysisParams.Add(this.listView3.Items[i].Text);
					}
				}				
			}

			if(m_Input.analysisParams.Count <1)
			{
				MessageBox.Show("STAT_DOE_FACTANAL_DIALOG_MSG06",this.strDACruxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);

				this.Input.pooledParams.Clear();

				for(int i=0; i< this.listView4.Items.Count; i++)
					this.Input.pooledParams.Add(this.listView4.Items[i].Text);

				if(this.listView3.Items.Count > 0 )
				{
					this.Input.analysisParams.Clear();
					// 인자
					for( int i =0; i < this.listView3.Items.Count; i++)
					{
						bool bPool = false;

						for(int k=0; k<this.Input.pooledParams.Count;k++)
						{							
							if( this.listView3.Items[i].Text.Equals(this.Input.pooledParams[k].ToString()))
								bPool = true;															
						}

						if(!bPool)
							this.m_Input.analysisParams.Add(this.listView3.Items[i].Text);
					}
				}	

				

				return;
			}

			for( int iCr = cr.Length - 1 ; iCr >= 0  ; iCr-- ) 
			{
				for( int i = cr[iCr].Row + cr[iCr].RowCount - 1 ; i >= cr[iCr].Row ; i-- ) 
				{	
					this.listView4.Items.Add(fpSpread1_Sheet1.GetValue(i,0).ToString());
					
				}
			}

			this.anova = this.getAnovaTable(this._dataSource,this.Input);		
	
			if(this.anova == null)
				return;

					
			fpSpread1_Sheet1.DataSource = this.anova;
			fpSpread1_Sheet1.Columns[0].Width = 80;
			fpSpread1_Sheet1.Columns[1].Width = 25;
			fpSpread1_Sheet1.Columns[2].Width = 110;
			fpSpread1_Sheet1.Columns[3].Width = 110;
			fpSpread1_Sheet1.Columns[4].Width = 60;
			fpSpread1_Sheet1.Columns[5].Width = 60;

			fpSpread1_Sheet1.RowHeaderVisible = false;
		
			fpSpread1.Show();

		}


		/// <summary>
		/// 분산분석표 결과를 가져오는 함수
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="input"></param>
		/// <returns></returns>
        private DataTable getAnovaTable(DataTable dt, DACrux.BStats.StatisticsInput.inputFactorAnalysis input)
		{
			if(! this.setFactorType() )
				return null;

            DACrux.BStats.AnalysisFactorial analysis = new DACrux.BStats.AnalysisFactorial(dt, input);

			DataSet dtSet = analysis.GetFactorialAnalysis();

			if(dtSet== null)
				return null;
			
			return dtSet.Tables["Analysis of Variance For Pooling"];	
		}

		/// <summary>
		/// 요인분석 폼 이벤트 함수
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DlgFactorialAnalysis_Load(object sender, System.EventArgs e)
		{
            //this.Icon = Icon.FromHandle( new Bitmap( fmg.imageList1.Images[Miracom.DACrux.Images.GetImage.FactorialAnalysis] ).GetHicon());
		}

		/// <summary>
		/// 자동 풀링 여부 판단하는 함수
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void radioButton1_CheckedChanged(object sender, System.EventArgs e)
		{
			if(this.radioButton1.Checked == true)
				this.button7.Enabled = false;
		
		}

		/// <summary>
		/// 사용자 정의 형태로 풀링여부를 판단하는 함수
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void radioButton2_CheckedChanged(object sender, System.EventArgs e)
		{
			if(this.radioButton2.Checked == true)
				this.button7.Enabled = true;			
		}

		/// <summary>
		/// 자동으로 풀링 시킨다.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button7_Click(object sender, System.EventArgs e)
		{
					
			this.Input.bPooling = true;

			this.Input.pooledParams.Clear();

			bool fullModel = false;
			for( int i =0; i<this.fpSpread1_Sheet1.RowCount; i++)
			{
				if(fpSpread1_Sheet1.GetValue(i,0).ToString().Equals("Error"))
					fullModel = true;			
			}

			if(!fullModel)
			{
				MessageBox.Show("STAT_DOE_FACTANAL_DIALOG_MSG07",this.strDACruxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

 
			for(int i=0; i< this.listView4.Items.Count; i++)
			{
				this.Input.pooledParams.Add(this.listView4.Items[i].Text);
			}

			

			for( int i =0; i<this.fpSpread1_Sheet1.RowCount; i++)
			{
				string str = this.fpSpread1_Sheet1.GetValue(i,5).ToString();

				if(str.Equals(null) || str.Equals(string.Empty) || str.Equals("") || str.Equals("*"))
				{
			

				}
				else
				{
					if( Convert.ToDouble(str) > Convert.ToDouble(this.numericUpDown1.Value)) 
					{
						this.Input.pooledParams.Add(fpSpread1_Sheet1.GetValue(i,0).ToString());								

					}
				}				
			}

			
			
			
			if(this.listView3.Items.Count > 0 )
			{
				this.Input.analysisParams.Clear();
				// 인자
				for( int i =0; i < this.listView3.Items.Count; i++)
				{
					bool bPool = false;

					for(int k=0; k<this.Input.pooledParams.Count;k++)
					{							
						if( this.listView3.Items[i].Text.Equals(this.Input.pooledParams[k].ToString()))
							bPool = true;															
					}

					if(!bPool)
						this.m_Input.analysisParams.Add(this.listView3.Items[i].Text);
				}
			}				
			
			if(this.Input.analysisParams.Count < 1)
			{
				MessageBox.Show("STAT_DOE_FACTANAL_DIALOG_MSG08",this.strDACruxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
				this.Input.analysisParams.Clear();
				this.Input.pooledParams.Clear();


				for( int i =0; i < this.listView3.Items.Count; i++)
				{
					this.m_Input.analysisParams.Add(this.listView3.Items[i].Text);
				}

				for( int i =0; i < this.listView4.Items.Count; i++)
				{
					this.m_Input.pooledParams.Add(this.listView4.Items[i].Text);
				}

				return;
			}

			for( int i =0; i<this.fpSpread1_Sheet1.RowCount; i++)
			{
				string str = this.fpSpread1_Sheet1.GetValue(i,5).ToString();

				if(str.Equals(null) || str.Equals(string.Empty) ||str.Equals(""))
				{

				}
				else
				{
					if( Convert.ToDouble(str) > Convert.ToDouble(this.numericUpDown1.Value)) 
					{					
						this.listView4.Items.Add(fpSpread1_Sheet1.GetValue(i,0).ToString());

					}
				}				
			}


			this.anova = this.getAnovaTable(this._dataSource,this.Input);	

			if(this.anova == null)
				return;
		
		
			fpSpread1_Sheet1.DataSource = this.anova;
			fpSpread1_Sheet1.Columns[0].Width = 80;
			fpSpread1_Sheet1.Columns[1].Width = 25;
			fpSpread1_Sheet1.Columns[2].Width = 110;
			fpSpread1_Sheet1.Columns[3].Width = 110;
			fpSpread1_Sheet1.Columns[4].Width = 60;
			fpSpread1_Sheet1.Columns[5].Width = 60;

			fpSpread1_Sheet1.RowHeaderVisible = false;
		
			fpSpread1.Show();

		}



		/// <summary>
		/// tabControl1 이벤트 함수
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void tabControl1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			int index = tabControl1.SelectedIndex;

			if ( tabControl1.TabPages[ index ].Equals(this.tabPage3) )
			{ 
				
				if (this.listView2.Items.Count < 1)
				{
					MessageBox.Show("STAT_DOE_FACTANAL_DIALOG_MSG09",this.strDACruxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
					return ;
				}

				if (this.listView3.Items.Count < 1)
				{
					MessageBox.Show("STAT_DOE_FACTANAL_DIALOG_MSG10",this.strDACruxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
					return ;

				}

				this.m_Input.unPooledParams.Clear();
				this.m_Input.pooledParams.Clear();
				this.listView4.Items.Clear();
				

				this.getAnovaTable();



			}


		}


		/// <summary>
		/// 분산분석표 컬럼 크기 이벤트 함수
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void fpSpread1_ColumnWidthChanged(object sender, FarPoint.Win.Spread.ColumnWidthChangedEventArgs e)
		{

			//fpSpread1_Sheet1.Columns[0].Width = 80;
			fpSpread1_Sheet1.Columns[1].Width = 25;
			fpSpread1_Sheet1.Columns[2].Width = 110;
			fpSpread1_Sheet1.Columns[3].Width = 110;
			fpSpread1_Sheet1.Columns[4].Width = 60;
			fpSpread1_Sheet1.Columns[5].Width = 60;

			fpSpread1_Sheet1.RowHeaderVisible = false;

			fpSpread1.Show();

		
		}

		/// <summary>
		/// 스프레드 시트 클릭 함수
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void fpSpread1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
		{
			bool bCol = false;
			bool bRow = false;

			if (e.ColumnHeader)
			{
				bCol = true;
			}
			else if(e.RowHeader)
			{
				bRow = true;
			}
			else
			{	
				bCol = false;
				bRow = false;
				return;
			}

			if( bCol == true && bRow == true )
			{
				fpSpread1_Sheet1.ClearSelection();
				fpSpread1.Show();
				bCol = false;
				bRow = false;
			}
			else if(bRow == true)
			{
				
			}

			

		}


		/// <summary>
		/// 스프레드 시트 행변화 이벤트함수
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void fpSpread1_RowHeightChanged(object sender, FarPoint.Win.Spread.RowHeightChangedEventArgs e)
		{
			for(int i=0; i<anova.Rows.Count; i++)
			{
				fpSpread1_Sheet1.Rows[i].Height = 20;
			}

			fpSpread1.Show();			
		
		}


		/// <summary>
		/// 분산분석표 초기화 함수
		/// </summary>
		private void getAnovaTable()
		{
			this.m_Input.analysisParams.Clear();
			this.m_Input.analysisVariables.Clear();

			this.m_Input.bDescriptiveStatistics	= this.checkBox1.Checked;
			this.m_Input.bAnovaAnalysis			= this.checkBox2.Checked;	
			this.m_Input.bInteractionEffect      = this.checkBox9.Checked;
			this.m_Input.bMainEffect				= this.checkBox3.Checked;	
			//this.m_Input.bRegressionEquation		= this.checkBox10.Checked;	

			this.m_Input.bInteractionDiagram     = this.checkBox8.Checked;
			this.m_Input.bMainEffectDiagram      = this.checkBox6.Checked;
			this.m_Input.bResidualPlot			= this.checkBox7.Checked;

			
			fpSpread1.Enabled = false;
			
			fpSpread1_Sheet1.Reset();

			fpSpread1_Sheet1.DataSource = null;
			fpSpread1.Show();

			if(this.listView3.Items.Count > 0 && this.listView2.Items.Count > 0)
			{
				fpSpread1.Enabled = true;
				// 반응 변수
				for( int i =0; i < this.listView2.Items.Count; i++)
					this.m_Input.analysisVariables.Add(this.listView2.Items[i].Text);
			
				// 인자
				for( int i =0; i < this.listView3.Items.Count; i++)
					this.m_Input.analysisParams.Add(this.listView3.Items[i].Text);
			
				this.anova = this.getAnovaTable(this._dataSource,this.Input);

				if(this.anova == null)
				{
					fpSpread1.Enabled = false;
					return;
				}
				
				fpSpread1_Sheet1.DataSource = this.anova;

			}

			
			fpSpread1_Sheet1.Columns[0].Width = 80;
			fpSpread1_Sheet1.Columns[1].Width = 25;
			fpSpread1_Sheet1.Columns[2].Width = 110;
			fpSpread1_Sheet1.Columns[3].Width = 110;
			fpSpread1_Sheet1.Columns[4].Width = 60;
			fpSpread1_Sheet1.Columns[5].Width = 60;

			fpSpread1_Sheet1.RowHeaderVisible = false;
		
			fpSpread1.Show();


		}


		/// <summary>
		/// 그룹간 변수를 카운트 하는 함수
		/// </summary>
		/// <param name="strVar"></param>
		/// <returns></returns>
		private ArrayList CountLevelOfVar(string [] strVar)
		{
			ArrayList alLevel = new ArrayList();
			string tempName = string.Empty;

			for(int i=0; i<strVar.Length; i++)
			{
				tempName = strVar[i];
				
				// Missing값의 경우는 제외한다.
				if(tempName.Trim() == ".") continue;

				// 기존에 ArrayList에 있는 값이 아니면 추가
				if(alLevel.IndexOf(tempName) == -1)
				{
					alLevel.Add(tempName);
				}
			}

			return alLevel;
		}


		/// <summary>
		/// 배열 내부 데이터가 같은지 비교한다.
		/// </summary>
		/// <param name="array"></param>
		/// <returns></returns>
		private bool CompareInnerArrayCount(int[] array)
		{
			bool result = false;

			if(array.Equals(null) )
				return result;

			int temp = array[0];
			for(int i = 0; i < array.Length; i++)
			{

				if(temp == array[i])
					result = true;
				else
					return false;
			}

			return result;
		}




		/// <summary>
		/// 결측치 체크
		/// </summary>
		/// <param name="col"></param>
		/// <returns></returns>
		private bool isMissingValue(DFColumn col)
		{
			for(int i=0; i<col.Count; i++)
			{
				// Missing값의 경우는 제외한다.
				if( col.IsMissing(i)  ) 
					return true;
			}

			return false;
		}

		private bool checkFactorType()
		{
			bool result = false;

			DataFrame dfTemp  = new DataFrame(this._dataSource);
			ArrayList array = new ArrayList();

			for( int i =0; i < this.listView3.Items.Count; i++)
			{
				string colName = this.listView3.Items[i].Text;

				array= this.CountLevelOfVar( ((DFColumn)dfTemp[colName]).ToStringArray() );

				for(int k = 0; k < array.Count; k++)
				{
					try
					{
						Convert.ToDouble( array[k].ToString() );
					}
					catch(Exception e)
					{
						Console.WriteLine(e.ToString());
						result = true;
					}
				}
				
				
				
			}


			return result;

		}

		private bool setFactorType()
		{
			bool result = false;

			DataFrame dfTemp  = new DataFrame(this._dataSource);
			ArrayList array = new ArrayList();
			int [] colCount = new int [this.listView3.Items.Count]; 

			if(colCount.Length < 1)
				return false;

			for( int i =0; i < this.listView3.Items.Count; i++)
			{
				string colName = this.listView3.Items[i].Text;
				array= this.CountLevelOfVar( ((DFColumn)dfTemp[colName]).ToStringArray() );
				colCount[i] =  array.Count;
				
				
			}




			if(this.CompareInnerArrayCount(colCount) )
			{
				this.m_Input.factorType = colCount[0];

				Console.Out.WriteLine("colCount[0] v====== {0}",colCount[0]);
				result = true;
			}
			else
			{
				MessageBox.Show("STAT_DOE_FACTANAL_DIALOG_MSG11",this.strDACruxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return false;
			}

			
			if( this.isMissingValue((DFColumn)dfTemp[ this.listView2.Items[0].Text] ) )
			{
				MessageBox.Show("STAT_DOE_FACTANAL_DIALOG_MSG12",this.strDACruxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return false;
			}
			
      

			return result;

		}



        public ProjectManager.UI.GraphInformation[] GraphInformations
        {
            get
            {
                return m_Input.GraphInformations;
            }
            set
            {
                m_Input.GraphInformations = value;
            }
        }

        public ProjectManager.UI.StatInformation StatInfo
        {
            get {
                return m_Input.StatInfo;
            }
        }
    }
}
