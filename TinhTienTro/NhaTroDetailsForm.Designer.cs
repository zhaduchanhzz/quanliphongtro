﻿namespace TinhTienTro
{
    partial class NhaTroDetailsForm
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
            this.dataGridViewThanhVien = new System.Windows.Forms.DataGridView();
            this.lblDiaChi = new System.Windows.Forms.Label();
            this.AddThanhVien = new System.Windows.Forms.Button();
            this.addPhongTro = new System.Windows.Forms.Button();
            this.dataGridViewCongto = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewThanhVien)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCongto)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewThanhVien
            // 
            this.dataGridViewThanhVien.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewThanhVien.Location = new System.Drawing.Point(12, 123);
            this.dataGridViewThanhVien.Name = "dataGridViewThanhVien";
            this.dataGridViewThanhVien.Size = new System.Drawing.Size(554, 436);
            this.dataGridViewThanhVien.TabIndex = 0;
            // 
            // lblDiaChi
            // 
            this.lblDiaChi.AutoSize = true;
            this.lblDiaChi.Location = new System.Drawing.Point(39, 9);
            this.lblDiaChi.Name = "lblDiaChi";
            this.lblDiaChi.Size = new System.Drawing.Size(35, 13);
            this.lblDiaChi.TabIndex = 1;
            this.lblDiaChi.Text = "label1";
            // 
            // AddThanhVien
            // 
            this.AddThanhVien.Location = new System.Drawing.Point(30, 38);
            this.AddThanhVien.Name = "AddThanhVien";
            this.AddThanhVien.Size = new System.Drawing.Size(115, 23);
            this.AddThanhVien.TabIndex = 2;
            this.AddThanhVien.Text = "Thêm Thành Viên";
            this.AddThanhVien.UseVisualStyleBackColor = true;
            this.AddThanhVien.Click += new System.EventHandler(this.AddThanhVien_Click);
            // 
            // addPhongTro
            // 
            this.addPhongTro.Location = new System.Drawing.Point(163, 38);
            this.addPhongTro.Name = "addPhongTro";
            this.addPhongTro.Size = new System.Drawing.Size(142, 23);
            this.addPhongTro.TabIndex = 3;
            this.addPhongTro.Text = "Thêm Phòng Trọ";
            this.addPhongTro.UseVisualStyleBackColor = true;
            this.addPhongTro.Click += new System.EventHandler(this.addPhongTro_Click);
            // 
            // dataGridViewCongto
            // 
            this.dataGridViewCongto.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCongto.Location = new System.Drawing.Point(608, 123);
            this.dataGridViewCongto.Name = "dataGridViewCongto";
            this.dataGridViewCongto.Size = new System.Drawing.Size(265, 436);
            this.dataGridViewCongto.TabIndex = 4;
            // 
            // NhaTroDetailsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(893, 565);
            this.Controls.Add(this.dataGridViewCongto);
            this.Controls.Add(this.addPhongTro);
            this.Controls.Add(this.AddThanhVien);
            this.Controls.Add(this.lblDiaChi);
            this.Controls.Add(this.dataGridViewThanhVien);
            this.Name = "NhaTroDetailsForm";
            this.Text = "NhaTroDetailsForm";
            this.Load += new System.EventHandler(this.NhaTroDetailsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewThanhVien)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCongto)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewThanhVien;
        private System.Windows.Forms.Label lblDiaChi;
        private System.Windows.Forms.Button AddThanhVien;
        private System.Windows.Forms.Button addPhongTro;
        private System.Windows.Forms.DataGridView dataGridViewCongto;
    }
}