using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fcfh
{
    public partial class frmMain : Form
    {
        private struct EncodeType
        {
            public string DisplayString;
            public OperationMode Mode;

            public override string ToString()
            {
                return DisplayString;
            }

            public static readonly EncodeType[] Types = new EncodeType[]
            {
                new EncodeType()
                {
                    DisplayString = "Encode into header",
                    Mode = OperationMode.UseHeader | OperationMode.Encode
                },
                new EncodeType()
                {
                    DisplayString = "Encode into pixeldata",
                    Mode = OperationMode.UsePixel | OperationMode.Encode
                }
            };
        }

        public frmMain()
        {
            InitializeComponent();

            cbEncodeType.Items.AddRange(EncodeType.Types.Cast<object>().ToArray());
            cbEncodeType.SelectedIndex = 0;
        }

        private void cbEncodeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbEncodeSource.Enabled =
                btnEncodeBrowseSource.Enabled = cbEncodeType.SelectedIndex != 1;
            cbReadable.Enabled = !tbEncodeSource.Enabled;
        }

        private void cbEncrypt_CheckedChanged(object sender, EventArgs e)
        {
            if (!(tbEncodeEncrypt.Enabled = cbEncrypt.Checked))
            {
                //It's a good idea to clear the password if the user disables encryption
                tbEncodeEncrypt.Text = string.Empty;
            }
        }

        private void btnEncodeBrowseInput_Click(object sender, EventArgs e)
        {
            var f = Tools.BrowseFile("Select file to encode", Preselected: tbEncodeInput.Text);
            if (!string.IsNullOrEmpty(f))
            {
                tbEncodeInput.Text = f;
            }
        }

        private void btnEncodeBrowseOutput_Click(object sender, EventArgs e)
        {
            var f = Tools.BrowseFile(
                "Save image as",
                "PNG files|*.png|Bitmap files|*.bmp",
                tbEncodeOutput.Text,
                true);
            if (!string.IsNullOrEmpty(f))
            {
                tbEncodeOutput.Text = f;
            }
        }

        private void btnEncodeBrowseSource_Click(object sender, EventArgs e)
        {
            var f = Tools.BrowseFile(
                "Source image",
                "PNG files|*.png",
                tbEncodeSource.Text);
            if (!string.IsNullOrEmpty(f))
            {
                tbEncodeSource.Text = f;
            }
        }

        private void Info(string Message, string Title)
        {
            MessageBox.Show(
                Message,
                string.IsNullOrEmpty(Title) ? Text : Title,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Warn(string Message, string Title)
        {
            MessageBox.Show(
                Message,
                string.IsNullOrEmpty(Title) ? Text : Title,
                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void Err(string Message, string Title)
        {
            MessageBox.Show(
                Message,
                string.IsNullOrEmpty(Title) ? Text : Title,
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnEncodeStart_Click(object sender, EventArgs e)
        {
            var mode = (EncodeType)cbEncodeType.SelectedItem;
            var isHeaderMode = mode.Mode.HasFlag(OperationMode.UseHeader);
            var isPngOutput = tbEncodeOutput.Text.ToLower().EndsWith(".png");
            var encrypt = cbEncrypt.Checked;

            if (!File.Exists(tbEncodeInput.Text))
            {
                Warn("Please select a file to encode", "Input file");
                btnEncodeBrowseInput.Focus();
                return;
            }

            if (encrypt)
            {
                if (string.IsNullOrEmpty(tbEncodeEncrypt.Text))
                {
                    Warn("Please enter a password or disable encryption", "Missing password");
                    tbEncodeEncrypt.Focus();
                    return;
                }
                using (var pw = new frmConfirmPassword(tbEncodeEncrypt.Text))
                {
                    if (pw.ShowDialog() != DialogResult.OK)
                    {
                        tbEncodeEncrypt.Focus();
                        return;
                    }
                }
            }

            if (isHeaderMode)
            {
                if (!isPngOutput)
                {
                    Warn("Header mode only works with png output files", "Output image");
                    btnEncodeBrowseOutput.Focus();
                    return;
                }
                if (!ImageWriter.HeaderMode.IsPNG(tbEncodeSource.Text))
                {
                    Warn("The selected source image doesn't appears to be a png", "Source image");
                    btnEncodeBrowseSource.Focus();
                    return;
                }
                if (!File.Exists(tbEncodeSource.Text))
                {
                    Warn("Header mode requires you to select an existing png image", "Source image");
                    btnEncodeBrowseSource.Focus();
                    return;
                }

                try
                {
                    byte[] Data;
                    if (encrypt)
                    {
                        using (var IS = File.OpenRead(tbEncodeSource.Text))
                        {
                            var Encrypted = Tools.EncryptData(tbEncodeEncrypt.Text, File.ReadAllBytes(tbEncodeInput.Text));
                            using (var MS = new MemoryStream(Encrypted, false))
                            {
                                Data = ImageWriter.HeaderMode.CreateImageFromFile(
                                    MS, Path.GetFileName(tbEncodeInput.Text),
                                    IS);
                            }
                        }
                    }
                    else
                    {
                        Data = ImageWriter.HeaderMode.CreateImageFromFile(tbEncodeInput.Text, tbEncodeSource.Text);
                    }
                    if (Data == null)
                    {
                        throw new Exception("Encoding image data failed");
                    }
                    File.WriteAllBytes(tbEncodeOutput.Text, Data);
                }
                catch (Exception ex)
                {
                    Err("Unable to encode data as image.\r\nReason: " + ex.Message, ex.GetType().Name);
                    return;
                }

            }
            else
            {
                try
                {
                    byte[] Data;
                    if (encrypt)
                    {
                        using (var IS = File.OpenRead(tbEncodeSource.Text))
                        {
                            var Encrypted = Tools.EncryptData(tbEncodeEncrypt.Text, File.ReadAllBytes(tbEncodeInput.Text));
                            using (var MS = new MemoryStream(Encrypted, false))
                            {
                                Data = ImageWriter.PixelMode.CreateImageFromFile(
                                    MS, Path.GetFileName(tbEncodeInput.Text), isPngOutput);
                            }
                        }
                    }
                    else
                    {
                        Data = ImageWriter.PixelMode.CreateImageFromFile(tbEncodeInput.Text, isPngOutput, cbReadable.Checked);
                    }
                    if (Data == null)
                    {
                        throw new Exception("Encoding image data failed");
                    }
                    File.WriteAllBytes(tbEncodeOutput.Text, Data);
                }
                catch (Exception ex)
                {
                    Err("Unable to encode data as image.\r\nReason: " + ex.Message, ex.GetType().Name);
                    return;
                }
                Info("Your file has been encoded into an image", "File encoded");
            }
        }
    }
}
