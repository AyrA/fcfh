
namespace fcfh
{
    partial class frmMain
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
            this.gbEncode = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbEncodeInput = new System.Windows.Forms.TextBox();
            this.btnEncodeBrowse = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cbEncodeType = new System.Windows.Forms.ComboBox();
            this.gbEncode.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbEncode
            // 
            this.gbEncode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbEncode.Controls.Add(this.cbEncodeType);
            this.gbEncode.Controls.Add(this.label3);
            this.gbEncode.Controls.Add(this.btnEncodeBrowse);
            this.gbEncode.Controls.Add(this.tbEncodeInput);
            this.gbEncode.Controls.Add(this.label2);
            this.gbEncode.Controls.Add(this.label1);
            this.gbEncode.Location = new System.Drawing.Point(12, 12);
            this.gbEncode.Name = "gbEncode";
            this.gbEncode.Size = new System.Drawing.Size(681, 247);
            this.gbEncode.TabIndex = 0;
            this.gbEncode.TabStop = false;
            this.gbEncode.Text = "Encode";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Encode a file inside of an image";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "File";
            // 
            // tbEncodeInput
            // 
            this.tbEncodeInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbEncodeInput.Location = new System.Drawing.Point(50, 57);
            this.tbEncodeInput.Name = "tbEncodeInput";
            this.tbEncodeInput.Size = new System.Drawing.Size(544, 20);
            this.tbEncodeInput.TabIndex = 2;
            // 
            // btnEncodeBrowse
            // 
            this.btnEncodeBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEncodeBrowse.Location = new System.Drawing.Point(600, 55);
            this.btnEncodeBrowse.Name = "btnEncodeBrowse";
            this.btnEncodeBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnEncodeBrowse.TabIndex = 3;
            this.btnEncodeBrowse.Text = "Browse...";
            this.btnEncodeBrowse.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Mode";
            // 
            // cbEncodeType
            // 
            this.cbEncodeType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbEncodeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEncodeType.FormattingEnabled = true;
            this.cbEncodeType.Location = new System.Drawing.Point(50, 94);
            this.cbEncodeType.Name = "cbEncodeType";
            this.cbEncodeType.Size = new System.Drawing.Size(544, 21);
            this.cbEncodeType.TabIndex = 5;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(705, 421);
            this.Controls.Add(this.gbEncode);
            this.Name = "frmMain";
            this.Text = "frmMain";
            this.gbEncode.ResumeLayout(false);
            this.gbEncode.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbEncode;
        private System.Windows.Forms.Button btnEncodeBrowse;
        private System.Windows.Forms.TextBox tbEncodeInput;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbEncodeType;
        private System.Windows.Forms.Label label3;
    }
}