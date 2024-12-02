namespace TinhTienTro
{
    partial class AddCongToDienForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtChiSoCu = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtChiSoMoi = new System.Windows.Forms.TextBox();
            this.chkNau = new System.Windows.Forms.CheckBox();
            this.chkTong = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.flowLayoutPanelPhongTro = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtChiSoCu
            // 
            this.txtChiSoCu.Location = new System.Drawing.Point(88, 19);
            this.txtChiSoCu.Name = "txtChiSoCu";
            this.txtChiSoCu.Size = new System.Drawing.Size(211, 20);
            this.txtChiSoCu.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Chỉ Số Cũ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Chỉ Số Mới";
            // 
            // txtChiSoMoi
            // 
            this.txtChiSoMoi.Location = new System.Drawing.Point(88, 45);
            this.txtChiSoMoi.Name = "txtChiSoMoi";
            this.txtChiSoMoi.Size = new System.Drawing.Size(211, 20);
            this.txtChiSoMoi.TabIndex = 2;
            // 
            // chkNau
            // 
            this.chkNau.AutoSize = true;
            this.chkNau.Location = new System.Drawing.Point(88, 82);
            this.chkNau.Name = "chkNau";
            this.chkNau.Size = new System.Drawing.Size(84, 17);
            this.chkNau.TabIndex = 4;
            this.chkNau.Text = "Công tơ nấu";
            this.chkNau.UseVisualStyleBackColor = true;
            // 
            // chkTong
            // 
            this.chkTong.AutoSize = true;
            this.chkTong.Location = new System.Drawing.Point(88, 105);
            this.chkTong.Name = "chkTong";
            this.chkTong.Size = new System.Drawing.Size(91, 17);
            this.chkTong.TabIndex = 5;
            this.chkTong.Text = "Công tơ Tổng";
            this.chkTong.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 137);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Phòng";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(71, 325);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(101, 22);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Lưu";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // flowLayoutPanelPhongTro
            // 
            this.flowLayoutPanelPhongTro.Location = new System.Drawing.Point(71, 137);
            this.flowLayoutPanelPhongTro.Name = "flowLayoutPanelPhongTro";
            this.flowLayoutPanelPhongTro.Size = new System.Drawing.Size(323, 182);
            this.flowLayoutPanelPhongTro.TabIndex = 9;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(212, 325);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(101, 22);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Đóng";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // AddCongToDienForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 401);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.flowLayoutPanelPhongTro);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chkTong);
            this.Controls.Add(this.chkNau);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtChiSoMoi);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtChiSoCu);
            this.Name = "AddCongToDienForm";
            this.Text = "AddCongToDienForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtChiSoCu;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtChiSoMoi;
        private System.Windows.Forms.CheckBox chkNau;
        private System.Windows.Forms.CheckBox chkTong;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelPhongTro;
        private System.Windows.Forms.Button btnCancel;
    }
}