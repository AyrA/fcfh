
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
            this.btnEncodeStart = new System.Windows.Forms.Button();
            this.cbEncrypt = new System.Windows.Forms.CheckBox();
            this.tbEncodePassword = new System.Windows.Forms.TextBox();
            this.btnEncodeBrowseSource = new System.Windows.Forms.Button();
            this.btnEncodeBrowseOutput = new System.Windows.Forms.Button();
            this.tbEncodeSource = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbEncodeOutput = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbReadable = new System.Windows.Forms.CheckBox();
            this.cbEncodeType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnEncodeBrowseInput = new System.Windows.Forms.Button();
            this.tbEncodeInput = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.gbDecode = new System.Windows.Forms.GroupBox();
            this.btnDecodeBrowse = new System.Windows.Forms.Button();
            this.tbDecodeInput = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbDecodeHeaders = new System.Windows.Forms.CheckBox();
            this.cbDecodePixels = new System.Windows.Forms.CheckBox();
            this.btnDecodeStart = new System.Windows.Forms.Button();
            this.gbEncode.SuspendLayout();
            this.gbDecode.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbEncode
            // 
            this.gbEncode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbEncode.Controls.Add(this.btnEncodeStart);
            this.gbEncode.Controls.Add(this.cbEncrypt);
            this.gbEncode.Controls.Add(this.tbEncodePassword);
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
            this.gbEncode.Size = new System.Drawing.Size(568, 302);
            this.gbEncode.TabIndex = 0;
            this.gbEncode.TabStop = false;
            this.gbEncode.Text = "Encode";
            // 
            // btnEncodeStart
            // 
            this.btnEncodeStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEncodeStart.Location = new System.Drawing.Point(487, 265);
            this.btnEncodeStart.Name = "btnEncodeStart";
            this.btnEncodeStart.Size = new System.Drawing.Size(75, 23);
            this.btnEncodeStart.TabIndex = 15;
            this.btnEncodeStart.Text = "Encode";
            this.btnEncodeStart.UseVisualStyleBackColor = true;
            this.btnEncodeStart.Click += new System.EventHandler(this.btnEncodeStart_Click);
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
            // tbEncodePassword
            // 
            this.tbEncodePassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbEncodePassword.Enabled = false;
            this.tbEncodePassword.Location = new System.Drawing.Point(123, 192);
            this.tbEncodePassword.Name = "tbEncodePassword";
            this.tbEncodePassword.Size = new System.Drawing.Size(358, 20);
            this.tbEncodePassword.TabIndex = 11;
            this.tbEncodePassword.UseSystemPasswordChar = true;
            // 
            // btnEncodeBrowseSource
            // 
            this.btnEncodeBrowseSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEncodeBrowseSource.Location = new System.Drawing.Point(487, 154);
            this.btnEncodeBrowseSource.Name = "btnEncodeBrowseSource";
            this.btnEncodeBrowseSource.Size = new System.Drawing.Size(75, 23);
            this.btnEncodeBrowseSource.TabIndex = 9;
            this.btnEncodeBrowseSource.Text = "Browse...";
            this.btnEncodeBrowseSource.UseVisualStyleBackColor = true;
            this.btnEncodeBrowseSource.Click += new System.EventHandler(this.btnEncodeBrowseSource_Click);
            // 
            // btnEncodeBrowseOutput
            // 
            this.btnEncodeBrowseOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEncodeBrowseOutput.Location = new System.Drawing.Point(487, 226);
            this.btnEncodeBrowseOutput.Name = "btnEncodeBrowseOutput";
            this.btnEncodeBrowseOutput.Size = new System.Drawing.Size(75, 23);
            this.btnEncodeBrowseOutput.TabIndex = 14;
            this.btnEncodeBrowseOutput.Text = "Browse...";
            this.btnEncodeBrowseOutput.UseVisualStyleBackColor = true;
            this.btnEncodeBrowseOutput.Click += new System.EventHandler(this.btnEncodeBrowseOutput_Click);
            // 
            // tbEncodeSource
            // 
            this.tbEncodeSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbEncodeSource.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tbEncodeSource.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.tbEncodeSource.Location = new System.Drawing.Point(55, 156);
            this.tbEncodeSource.Name = "tbEncodeSource";
            this.tbEncodeSource.Size = new System.Drawing.Size(426, 20);
            this.tbEncodeSource.TabIndex = 8;
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
            // tbEncodeOutput
            // 
            this.tbEncodeOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbEncodeOutput.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tbEncodeOutput.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.tbEncodeOutput.Location = new System.Drawing.Point(55, 228);
            this.tbEncodeOutput.Name = "tbEncodeOutput";
            this.tbEncodeOutput.Size = new System.Drawing.Size(426, 20);
            this.tbEncodeOutput.TabIndex = 13;
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
            // cbEncodeType
            // 
            this.cbEncodeType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbEncodeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEncodeType.FormattingEnabled = true;
            this.cbEncodeType.Location = new System.Drawing.Point(55, 93);
            this.cbEncodeType.Name = "cbEncodeType";
            this.cbEncodeType.Size = new System.Drawing.Size(426, 21);
            this.cbEncodeType.TabIndex = 5;
            this.cbEncodeType.SelectedIndexChanged += new System.EventHandler(this.cbEncodeType_SelectedIndexChanged);
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
            // btnEncodeBrowseInput
            // 
            this.btnEncodeBrowseInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEncodeBrowseInput.Location = new System.Drawing.Point(487, 55);
            this.btnEncodeBrowseInput.Name = "btnEncodeBrowseInput";
            this.btnEncodeBrowseInput.Size = new System.Drawing.Size(75, 23);
            this.btnEncodeBrowseInput.TabIndex = 3;
            this.btnEncodeBrowseInput.Text = "Browse...";
            this.btnEncodeBrowseInput.UseVisualStyleBackColor = true;
            this.btnEncodeBrowseInput.Click += new System.EventHandler(this.btnEncodeBrowseInput_Click);
            // 
            // tbEncodeInput
            // 
            this.tbEncodeInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbEncodeInput.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tbEncodeInput.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.tbEncodeInput.Location = new System.Drawing.Point(55, 57);
            this.tbEncodeInput.Name = "tbEncodeInput";
            this.tbEncodeInput.Size = new System.Drawing.Size(426, 20);
            this.tbEncodeInput.TabIndex = 2;
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Encode a file inside of an image";
            // 
            // gbDecode
            // 
            this.gbDecode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbDecode.Controls.Add(this.btnDecodeStart);
            this.gbDecode.Controls.Add(this.cbDecodePixels);
            this.gbDecode.Controls.Add(this.cbDecodeHeaders);
            this.gbDecode.Controls.Add(this.btnDecodeBrowse);
            this.gbDecode.Controls.Add(this.tbDecodeInput);
            this.gbDecode.Controls.Add(this.label6);
            this.gbDecode.Location = new System.Drawing.Point(12, 320);
            this.gbDecode.Name = "gbDecode";
            this.gbDecode.Size = new System.Drawing.Size(568, 117);
            this.gbDecode.TabIndex = 1;
            this.gbDecode.TabStop = false;
            this.gbDecode.Text = "Decode";
            // 
            // btnDecodeBrowse
            // 
            this.btnDecodeBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDecodeBrowse.Location = new System.Drawing.Point(487, 17);
            this.btnDecodeBrowse.Name = "btnDecodeBrowse";
            this.btnDecodeBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnDecodeBrowse.TabIndex = 2;
            this.btnDecodeBrowse.Text = "Browse...";
            this.btnDecodeBrowse.UseVisualStyleBackColor = true;
            this.btnDecodeBrowse.Click += new System.EventHandler(this.btnDecodeBrowse_Click);
            // 
            // tbDecodeInput
            // 
            this.tbDecodeInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDecodeInput.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tbDecodeInput.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.tbDecodeInput.Location = new System.Drawing.Point(55, 19);
            this.tbDecodeInput.Name = "tbDecodeInput";
            this.tbDecodeInput.Size = new System.Drawing.Size(426, 20);
            this.tbDecodeInput.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(23, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "File";
            // 
            // cbDecodeHeaders
            // 
            this.cbDecodeHeaders.AutoSize = true;
            this.cbDecodeHeaders.Checked = true;
            this.cbDecodeHeaders.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDecodeHeaders.Location = new System.Drawing.Point(55, 55);
            this.cbDecodeHeaders.Name = "cbDecodeHeaders";
            this.cbDecodeHeaders.Size = new System.Drawing.Size(202, 17);
            this.cbDecodeHeaders.TabIndex = 3;
            this.cbDecodeHeaders.Text = "Search and ecode data from headers";
            this.cbDecodeHeaders.UseVisualStyleBackColor = true;
            // 
            // cbDecodePixels
            // 
            this.cbDecodePixels.AutoSize = true;
            this.cbDecodePixels.Checked = true;
            this.cbDecodePixels.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDecodePixels.Location = new System.Drawing.Point(55, 88);
            this.cbDecodePixels.Name = "cbDecodePixels";
            this.cbDecodePixels.Size = new System.Drawing.Size(184, 17);
            this.cbDecodePixels.TabIndex = 4;
            this.cbDecodePixels.Text = "Search and decode data in pixels";
            this.cbDecodePixels.UseVisualStyleBackColor = true;
            // 
            // btnDecodeStart
            // 
            this.btnDecodeStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDecodeStart.Location = new System.Drawing.Point(487, 84);
            this.btnDecodeStart.Name = "btnDecodeStart";
            this.btnDecodeStart.Size = new System.Drawing.Size(75, 23);
            this.btnDecodeStart.TabIndex = 5;
            this.btnDecodeStart.Text = "Decode";
            this.btnDecodeStart.UseVisualStyleBackColor = true;
            this.btnDecodeStart.Click += new System.EventHandler(this.btnDecodeStart_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 453);
            this.Controls.Add(this.gbDecode);
            this.Controls.Add(this.gbEncode);
            this.MinimumSize = new System.Drawing.Size(400, 480);
            this.Name = "frmMain";
            this.Text = "fcfh Image Encoder";
            this.gbEncode.ResumeLayout(false);
            this.gbEncode.PerformLayout();
            this.gbDecode.ResumeLayout(false);
            this.gbDecode.PerformLayout();
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
        private System.Windows.Forms.TextBox tbEncodePassword;
        private System.Windows.Forms.Button btnEncodeBrowseOutput;
        private System.Windows.Forms.TextBox tbEncodeOutput;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnEncodeBrowseSource;
        private System.Windows.Forms.TextBox tbEncodeSource;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnEncodeStart;
        private System.Windows.Forms.GroupBox gbDecode;
        private System.Windows.Forms.Button btnDecodeBrowse;
        private System.Windows.Forms.TextBox tbDecodeInput;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnDecodeStart;
        private System.Windows.Forms.CheckBox cbDecodePixels;
        private System.Windows.Forms.CheckBox cbDecodeHeaders;
    }
}