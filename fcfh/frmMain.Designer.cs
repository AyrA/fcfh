
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
            this.btnEncodeBrowseInput = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cbEncodeType = new System.Windows.Forms.ComboBox();
            this.cbReadable = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbEncodeOutput = new System.Windows.Forms.TextBox();
            this.btnEncodeBrowseOutput = new System.Windows.Forms.Button();
            this.tbEncrypt = new System.Windows.Forms.TextBox();
            this.cbEncrypt = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbEncodeSource = new System.Windows.Forms.TextBox();
            this.btnEncodeBrowseSource = new System.Windows.Forms.Button();
            this.btnEncodeStart = new System.Windows.Forms.Button();
            this.gbEncode.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbEncode
            // 
            this.gbEncode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbEncode.Controls.Add(this.btnEncodeStart);
            this.gbEncode.Controls.Add(this.cbEncrypt);
            this.gbEncode.Controls.Add(this.tbEncrypt);
            this.gbEncode.Controls.Add(this.btnEncodeBrowseSource);
            this.gbEncode.Controls.Add(this.btnEncodeBrowseOutput);
            this.gbEncode.Controls.Add(this.tbEncodeSource);
            this.gbEncode.Controls.Add(this.label5);
            this.gbEncode.Controls.Add(this.tbEncodeOutput);
            this.gbEncode.Controls.Add(this.label4);
            this.gbEncode.Controls.Add(this.cbReadable);
            this.gbEncode.Controls.Add(this.cbEncodeType);
            this.gbEncode.Controls.Add(this.label3);
            this.gbEncode.Controls.Add(this.btnEncodeBrowseInput);
            this.gbEncode.Controls.Add(this.tbEncodeInput);
            this.gbEncode.Controls.Add(this.label2);
            this.gbEncode.Controls.Add(this.label1);
            this.gbEncode.Location = new System.Drawing.Point(12, 12);
            this.gbEncode.Name = "gbEncode";
            this.gbEncode.Size = new System.Drawing.Size(573, 302);
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
            this.tbEncodeInput.Location = new System.Drawing.Point(55, 57);
            this.tbEncodeInput.Name = "tbEncodeInput";
            this.tbEncodeInput.Size = new System.Drawing.Size(431, 20);
            this.tbEncodeInput.TabIndex = 2;
            // 
            // btnEncodeBrowseInput
            // 
            this.btnEncodeBrowseInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEncodeBrowseInput.Location = new System.Drawing.Point(492, 55);
            this.btnEncodeBrowseInput.Name = "btnEncodeBrowseInput";
            this.btnEncodeBrowseInput.Size = new System.Drawing.Size(75, 23);
            this.btnEncodeBrowseInput.TabIndex = 3;
            this.btnEncodeBrowseInput.Text = "Browse...";
            this.btnEncodeBrowseInput.UseVisualStyleBackColor = true;
            this.btnEncodeBrowseInput.Click += new System.EventHandler(this.btnEncodeBrowseInput_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 96);
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
            this.cbEncodeType.Location = new System.Drawing.Point(55, 93);
            this.cbEncodeType.Name = "cbEncodeType";
            this.cbEncodeType.Size = new System.Drawing.Size(431, 21);
            this.cbEncodeType.TabIndex = 5;
            this.cbEncodeType.SelectedIndexChanged += new System.EventHandler(this.cbEncodeType_SelectedIndexChanged);
            // 
            // cbReadable
            // 
            this.cbReadable.AutoSize = true;
            this.cbReadable.Location = new System.Drawing.Point(55, 123);
            this.cbReadable.Name = "cbReadable";
            this.cbReadable.Size = new System.Drawing.Size(93, 17);
            this.cbReadable.TabIndex = 6;
            this.cbReadable.Text = "Sort pixel data";
            this.cbReadable.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 231);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Output";
            // 
            // tbEncodeOutput
            // 
            this.tbEncodeOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbEncodeOutput.Location = new System.Drawing.Point(55, 228);
            this.tbEncodeOutput.Name = "tbEncodeOutput";
            this.tbEncodeOutput.Size = new System.Drawing.Size(431, 20);
            this.tbEncodeOutput.TabIndex = 13;
            // 
            // btnEncodeBrowseOutput
            // 
            this.btnEncodeBrowseOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEncodeBrowseOutput.Location = new System.Drawing.Point(492, 226);
            this.btnEncodeBrowseOutput.Name = "btnEncodeBrowseOutput";
            this.btnEncodeBrowseOutput.Size = new System.Drawing.Size(75, 23);
            this.btnEncodeBrowseOutput.TabIndex = 14;
            this.btnEncodeBrowseOutput.Text = "Browse...";
            this.btnEncodeBrowseOutput.UseVisualStyleBackColor = true;
            this.btnEncodeBrowseOutput.Click += new System.EventHandler(this.btnEncodeBrowseOutput_Click);
            // 
            // tbEncrypt
            // 
            this.tbEncrypt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbEncrypt.Enabled = false;
            this.tbEncrypt.Location = new System.Drawing.Point(123, 192);
            this.tbEncrypt.Name = "tbEncrypt";
            this.tbEncrypt.Size = new System.Drawing.Size(363, 20);
            this.tbEncrypt.TabIndex = 11;
            this.tbEncrypt.UseSystemPasswordChar = true;
            // 
            // cbEncrypt
            // 
            this.cbEncrypt.AutoSize = true;
            this.cbEncrypt.Location = new System.Drawing.Point(55, 194);
            this.cbEncrypt.Name = "cbEncrypt";
            this.cbEncrypt.Size = new System.Drawing.Size(62, 17);
            this.cbEncrypt.TabIndex = 10;
            this.cbEncrypt.Text = "Encrypt";
            this.cbEncrypt.UseVisualStyleBackColor = true;
            this.cbEncrypt.CheckedChanged += new System.EventHandler(this.cbEncrypt_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 159);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Source";
            // 
            // tbEncodeSource
            // 
            this.tbEncodeSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbEncodeSource.Location = new System.Drawing.Point(55, 156);
            this.tbEncodeSource.Name = "tbEncodeSource";
            this.tbEncodeSource.Size = new System.Drawing.Size(431, 20);
            this.tbEncodeSource.TabIndex = 8;
            // 
            // btnEncodeBrowseSource
            // 
            this.btnEncodeBrowseSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEncodeBrowseSource.Location = new System.Drawing.Point(492, 154);
            this.btnEncodeBrowseSource.Name = "btnEncodeBrowseSource";
            this.btnEncodeBrowseSource.Size = new System.Drawing.Size(75, 23);
            this.btnEncodeBrowseSource.TabIndex = 9;
            this.btnEncodeBrowseSource.Text = "Browse...";
            this.btnEncodeBrowseSource.UseVisualStyleBackColor = true;
            this.btnEncodeBrowseSource.Click += new System.EventHandler(this.btnEncodeBrowseSource_Click);
            // 
            // btnEncodeStart
            // 
            this.btnEncodeStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEncodeStart.Location = new System.Drawing.Point(492, 265);
            this.btnEncodeStart.Name = "btnEncodeStart";
            this.btnEncodeStart.Size = new System.Drawing.Size(75, 23);
            this.btnEncodeStart.TabIndex = 15;
            this.btnEncodeStart.Text = "Encode";
            this.btnEncodeStart.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 339);
            this.Controls.Add(this.gbEncode);
            this.Name = "frmMain";
            this.Text = "fcfh Image Encoder";
            this.gbEncode.ResumeLayout(false);
            this.gbEncode.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbEncode;
        private System.Windows.Forms.Button btnEncodeBrowseInput;
        private System.Windows.Forms.TextBox tbEncodeInput;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbEncodeType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cbReadable;
        private System.Windows.Forms.CheckBox cbEncrypt;
        private System.Windows.Forms.TextBox tbEncrypt;
        private System.Windows.Forms.Button btnEncodeBrowseOutput;
        private System.Windows.Forms.TextBox tbEncodeOutput;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnEncodeBrowseSource;
        private System.Windows.Forms.TextBox tbEncodeSource;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnEncodeStart;
    }
}