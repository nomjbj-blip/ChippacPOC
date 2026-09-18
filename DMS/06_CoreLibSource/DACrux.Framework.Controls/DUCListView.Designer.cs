namespace DACrux.Framework.Controls
{
    partial class DUCListView
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // DUCListView
            // 
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FullRowSelect = true;
            this.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.HideSelection = false;
            this.Size = new System.Drawing.Size(100, 150);
            this.View = System.Windows.Forms.View.Details;
            this.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.DUCListView_ItemSelectionChanged);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DUCListView_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion

    }
}
