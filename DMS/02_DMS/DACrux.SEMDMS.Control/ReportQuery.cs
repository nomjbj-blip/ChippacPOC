using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Text;

namespace DMSPlus.DefectMapAnalysis.Control
{
	/// <summary>
	/// Query에 대한 요약 설명입니다.
	/// </summary>
	public class ReportQuery : System.Windows.Forms.UserControl
	{
		public static string strFromDate="";
		public static string strToDate="";
		public string strUserID="";
		public System.Windows.Forms.CheckedListBox clbEQ;
		public System.Windows.Forms.CheckedListBox clbStep;
		public System.Windows.Forms.CheckedListBox clbDevice;
		public System.Windows.Forms.Button btnGetStep;
		public System.Windows.Forms.Button btnGetDevice;
		public System.Windows.Forms.CheckBox chkEQ;
		public System.Windows.Forms.CheckBox chkDevice;
		public System.Windows.Forms.CheckBox chkStep;
		private System.Windows.Forms.Label lblEQ;
		private System.Windows.Forms.Label lblDevice;
		private System.Windows.Forms.Label lblStep;

		private System.ComponentModel.Container components = null;

		#region ■ 생성자

		public ReportQuery()
		{
			InitializeComponent();
		}
		#endregion

		#region ■ Dispose

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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Query));
			this.clbEQ = new System.Windows.Forms.CheckedListBox();
			this.clbStep = new System.Windows.Forms.CheckedListBox();
			this.clbDevice = new System.Windows.Forms.CheckedListBox();
			this.btnGetStep = new System.Windows.Forms.Button();
			this.btnGetDevice = new System.Windows.Forms.Button();
			this.chkEQ = new System.Windows.Forms.CheckBox();
			this.chkDevice = new System.Windows.Forms.CheckBox();
			this.chkStep = new System.Windows.Forms.CheckBox();
			this.lblEQ = new System.Windows.Forms.Label();
			this.lblDevice = new System.Windows.Forms.Label();
			this.lblStep = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// clbEQ
			// 
			this.clbEQ.CheckOnClick = true;
			this.clbEQ.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.clbEQ.Location = new System.Drawing.Point(4, 33);
			this.clbEQ.Name = "clbEQ";
			this.clbEQ.Size = new System.Drawing.Size(92, 164);
			this.clbEQ.TabIndex = 3;
			// 
			// clbStep
			// 
			this.clbStep.CheckOnClick = true;
			this.clbStep.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.clbStep.Location = new System.Drawing.Point(216, 33);
			this.clbStep.Name = "clbStep";
			this.clbStep.Size = new System.Drawing.Size(173, 164);
			this.clbStep.TabIndex = 5;
			// 
			// clbDevice
			// 
			this.clbDevice.CheckOnClick = true;
			this.clbDevice.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.clbDevice.Location = new System.Drawing.Point(112, 33);
			this.clbDevice.Name = "clbDevice";
			this.clbDevice.Size = new System.Drawing.Size(88, 164);
			this.clbDevice.TabIndex = 6;
			// 
			// btnGetStep
			// 
			this.btnGetStep.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(224)), ((System.Byte)(224)), ((System.Byte)(224)));
			this.btnGetStep.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnGetStep.BackgroundImage")));
			this.btnGetStep.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnGetStep.Location = new System.Drawing.Point(201, 105);
			this.btnGetStep.Name = "btnGetStep";
			this.btnGetStep.Size = new System.Drawing.Size(14, 13);
			this.btnGetStep.TabIndex = 7;
			this.btnGetStep.Click += new System.EventHandler(this.btnGetStep_Click);
			// 
			// btnGetDevice
			// 
			this.btnGetDevice.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(224)), ((System.Byte)(224)), ((System.Byte)(224)));
			this.btnGetDevice.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnGetDevice.BackgroundImage")));
			this.btnGetDevice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnGetDevice.Location = new System.Drawing.Point(97, 105);
			this.btnGetDevice.Name = "btnGetDevice";
			this.btnGetDevice.Size = new System.Drawing.Size(14, 13);
			this.btnGetDevice.TabIndex = 8;
			this.btnGetDevice.Click += new System.EventHandler(this.btnGetDevice_Click);
			// 
			// chkEQ
			// 
			this.chkEQ.Font = new System.Drawing.Font("돋움", 9F);
			this.chkEQ.Location = new System.Drawing.Point(4, 17);
			this.chkEQ.Name = "chkEQ";
			this.chkEQ.Size = new System.Drawing.Size(88, 16);
			this.chkEQ.TabIndex = 12;
			this.chkEQ.Text = "Select All";
			this.chkEQ.CheckedChanged += new System.EventHandler(this.chkEQ_CheckedChanged);
			// 
			// chkDevice
			// 
			this.chkDevice.Font = new System.Drawing.Font("돋움", 9F);
			this.chkDevice.Location = new System.Drawing.Point(112, 17);
			this.chkDevice.Name = "chkDevice";
			this.chkDevice.Size = new System.Drawing.Size(82, 16);
			this.chkDevice.TabIndex = 13;
			this.chkDevice.Text = "Select All";
			this.chkDevice.CheckedChanged += new System.EventHandler(this.chkDevice_CheckedChanged);
			// 
			// chkStep
			// 
			this.chkStep.Font = new System.Drawing.Font("돋움", 9F);
			this.chkStep.Location = new System.Drawing.Point(216, 17);
			this.chkStep.Name = "chkStep";
			this.chkStep.Size = new System.Drawing.Size(104, 16);
			this.chkStep.TabIndex = 14;
			this.chkStep.Text = "Select All";
			this.chkStep.CheckedChanged += new System.EventHandler(this.chkStep_CheckedChanged);
			// 
			// lblEQ
			// 
			this.lblEQ.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.lblEQ.Location = new System.Drawing.Point(3, 1);
			this.lblEQ.Name = "lblEQ";
			this.lblEQ.Size = new System.Drawing.Size(100, 13);
			this.lblEQ.TabIndex = 15;
			this.lblEQ.Text = "Equipment";
			// 
			// lblDevice
			// 
			this.lblDevice.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.lblDevice.Location = new System.Drawing.Point(107, 1);
			this.lblDevice.Name = "lblDevice";
			this.lblDevice.Size = new System.Drawing.Size(100, 13);
			this.lblDevice.TabIndex = 16;
			this.lblDevice.Text = "Device";
			// 
			// lblStep
			// 
			this.lblStep.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.lblStep.Location = new System.Drawing.Point(210, 1);
			this.lblStep.Name = "lblStep";
			this.lblStep.Size = new System.Drawing.Size(100, 13);
			this.lblStep.TabIndex = 17;
			this.lblStep.Text = "Step";
			// 
			// Query
			// 
			this.Controls.Add(this.chkStep);
			this.Controls.Add(this.chkDevice);
			this.Controls.Add(this.chkEQ);
			this.Controls.Add(this.btnGetDevice);
			this.Controls.Add(this.btnGetStep);
			this.Controls.Add(this.clbDevice);
			this.Controls.Add(this.clbStep);
			this.Controls.Add(this.clbEQ);
			this.Controls.Add(this.lblStep);
			this.Controls.Add(this.lblDevice);
			this.Controls.Add(this.lblEQ);
			this.Name = "Query";
			this.Size = new System.Drawing.Size(392, 208);
			this.Load += new System.EventHandler(this.Query_Load);
			this.ResumeLayout(false);

		}
		#endregion

		#region ■ Query_Load

		/// <summary>
		/// 1) 장비목록 상자 초기화
		/// 2) Device 목록 상자 초기화
		/// 3) Step 선택상자 초기화
		/// </summary>
		/// 
		private void Query_Load(object sender, System.EventArgs e)
		{
			if(DesignMode) return;

			clbEQ.Items.Clear();
			clbDevice.Items.Clear();
			clbStep.Items.Clear();
		}
		#endregion

		#region ■ btnGetDevice_Click 클릭 이벤트

		/// <summary>
		/// 클릭하면 좌측에 선택된 장비에서 해당기간에 진행된 Product List 를 가져와 바인딩하는 서비스를 호출한다.
		/// </summary>
		/// 
		private void btnGetDevice_Click(object sender, System.EventArgs e)
		{
			this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
			clbDevice.Items.Clear();
			chkDevice.Checked=false;
			clbStep.Items.Clear();
			chkStep.Checked=false;

			if (clbEQ.CheckedItems.Count>0)
			{
				GetDeviceList();
			}
			this.Cursor = System.Windows.Forms.Cursors.Default;
		}
		#endregion

		#region ■ btnGetStep_Click 클릭 이벤트

		/// <summary>
		/// 해당기간, 해당장비, 해당 Device 에 속한 Step 을 Listup 하여 바인딩하는 서비스를 호출한다.
		/// </summary>
		/// 
		private void btnGetStep_Click(object sender, System.EventArgs e)
		{
			this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
			clbStep.Items.Clear();

			if (clbEQ.CheckedItems.Count>0 && clbDevice.CheckedItems.Count>0)
			{
				GetStepList();
			}
			this.Cursor = System.Windows.Forms.Cursors.Default;
		}
		#endregion

		#region ■ GetEQList

		/// <summary>
		/// 설정한 기간에 해당하는 장비 목록을 가져와 바인딩한다.
		/// </summary>
		/// <param name="strFrom">Start Date Time</param>
		/// <param name="strTo">End Date Time</param></param>
		public void GetEQList(string strFrom, string strTo)
		{
			DataSet dsReturn=null;
			Remoting oEQList=null;

			try
			{
				clbEQ.Items.Clear();
				chkEQ.Checked=false;

				clbDevice.Items.Clear();
				chkDevice.Checked=false;

				clbStep.Items.Clear();
				chkStep.Checked=false;

				strFromDate=strFrom;
				strToDate=strTo;

				oEQList=new Remoting();

				dsReturn=oEQList.GetEQList(strUserID, "GetEQList", strFromDate, strToDate);

				for(int i=0;i<dsReturn.Tables[0].Rows.Count;i++)
				{
					clbEQ.Items.Add(dsReturn.Tables[0].Rows[i][0]);
				}
			}

			catch(Exception ex)
			{
				throw ex;
				//Miracom.DMS.Config.
			}

			finally
			{
				if(dsReturn != null) dsReturn.Dispose();
			}
		}
		#endregion

		#region ■ GetDeviceList

		/// <summary>
		/// 설정한 기간에 선택한 장비에서 진행한 Product 목록을 가져와 바인딩한다.
		/// </summary>
		private void GetDeviceList()
		{
			DataSet dsReturn=null;
			Remoting oDeviceList=null;
			StringBuilder builder=new StringBuilder();

			string[] strEQList;

			try
			{
				oDeviceList=new Remoting();

				strEQList= new string [clbEQ.CheckedItems.Count];

				for(int k=0;k< clbEQ.CheckedItems.Count;k++)
				{
					strEQList[k]=clbEQ.CheckedItems[k].ToString();
				}

				dsReturn=oDeviceList.GetDeviceList(strUserID, "GetDeviceList", strFromDate, strToDate, strEQList);

				for(int i=0;i<dsReturn.Tables[0].Rows.Count;i++)
				{
					clbDevice.Items.Add(dsReturn.Tables[0].Rows[i][0]);
				}
			}

			catch(Exception ex)
			{
				throw ex;
				//Miracom.DMS.Config.
			}

			finally
			{
				if(dsReturn != null) dsReturn.Dispose();
			}
		}
		#endregion

		#region ■ GetStepList

		/// <summary>
		/// 설정한 기간에 선택한 장비에서 진행한 Product 에 해당하는 Step 목록을 가져와 바인딩한다.
		/// </summary>
		private void GetStepList()
		{
			DataSet dsReturn=null;
			Remoting oStepList=null;
			string[] strEQList;
			string[] strDeviceList;

			try
			{
				oStepList=new Remoting();

				strEQList= new string [clbEQ.CheckedItems.Count];

				for(int k=0;k< clbEQ.CheckedItems.Count;k++)
				{
					strEQList[k]=clbEQ.CheckedItems[k].ToString();
				}

				strDeviceList= new string [clbDevice.CheckedItems.Count];

				for(int k=0;k< clbDevice.CheckedItems.Count;k++)
				{
					strDeviceList[k]=clbDevice.CheckedItems[k].ToString();
				}

				dsReturn=oStepList.GetStepList(strUserID, "GetStepList", strFromDate, strToDate, strEQList, strDeviceList);

				for(int i=0;i<dsReturn.Tables[0].Rows.Count;i++)
				{
					clbStep.Items.Add(dsReturn.Tables[0].Rows[i][0]);
				}
			}

			catch(Exception ex)
			{
				throw ex;
				//Miracom.DMS.Config.
			}

			finally
			{
				if(dsReturn != null) dsReturn.Dispose();
			}
		}
		#endregion

		#region ■ 장비 선택상자의 Select All 체크박스 클릭 이벤트

		/// <summary>
		/// Select All 체크박스의 상태가 변경되면 체크리스트박스 Item을 모두 선택하거나 해제한다.
		/// </summary>
		/// 
		private void chkEQ_CheckedChanged(object sender, System.EventArgs e)
		{
			if(clbEQ.Items.Count==0)
			{
				chkEQ.Checked=false;
			}
			else
			{
				if(chkEQ.Checked==true)
				{
					for(int i=0;i<clbEQ.Items.Count;i++)
					{
						clbEQ.SetItemCheckState(i,CheckState.Checked);
					}
				}
				else
				{
					for(int i=0;i<clbEQ.Items.Count;i++)
					{
						clbEQ.SetItemCheckState(i,CheckState.Unchecked);
					}
				}
			}
		}
		#endregion

		#region ■ Device 선택상자의 Select All 체크박스 클릭 이벤트

		/// <summary>
		/// Select All 체크박스의 상태가 변경되면 체크리스트박스 Item을 모두 선택하거나 해제한다.
		/// </summary>
		/// 
		private void chkDevice_CheckedChanged(object sender, System.EventArgs e)
		{
			if(clbDevice.Items.Count==0)
			{
				chkDevice.Checked=false;
			}
			else
			{
				if(chkDevice.Checked==true)
				{
					for(int i=0;i<clbDevice.Items.Count;i++)
					{
						clbDevice.SetItemCheckState(i,CheckState.Checked);
					}
				}
				else
				{
					for(int i=0;i<clbDevice.Items.Count;i++)
					{
						clbDevice.SetItemCheckState(i,CheckState.Unchecked);
					}
				}
			}
		}
		#endregion

		#region ■ Step 선택상자의 Select All 체크박스 클릭 이벤트

		/// <summary>
		/// Select All 체크박스의 상태가 변경되면 체크리스트박스 Item을 모두 선택하거나 해제한다.
		/// </summary>
		/// 
		private void chkStep_CheckedChanged(object sender, System.EventArgs e)
		{
			if(clbStep.Items.Count==0)
			{
				chkStep.Checked=false;
			}
			else
			{
				if(chkStep.Checked==true)
				{
					for(int i=0;i<clbStep.Items.Count;i++)
					{
						clbStep.SetItemCheckState(i,CheckState.Checked);
					}
				}
				else
				{
					for(int i=0;i<clbStep.Items.Count;i++)
					{
						clbStep.SetItemCheckState(i,CheckState.Unchecked);
					}
				}
			}
		}
		#endregion

	}
}
