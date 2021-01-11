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
            //Allow vertical scaling
            MaximumSize = new Size(int.MaxValue, MinimumSize.Height);

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
            if (!(tbEncodePassword.Enabled = cbEncrypt.Checked))
            {
                //It's a good idea to clear the password if the user disables encryption
                tbEncodePassword.Text = string.Empty;
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
                if (ImageWriter.HeaderMode.IsPNG(f))
                {
                    tbEncodeSource.Text = f;
                }
                else
                {
                    try
                    {
                        using (var FS = File.OpenRead(f))
                        {
                            //Minimum size is PNG header(8) + IEND chunk (12)
                            if (FS.Length > 8 + 12)
                            {
                                var data = new byte[4];
                                FS.Read(data, 0, data.Length);
                                var header = Encoding.Default.GetString(data);
                                //Header looks like PNG
                                if (header == "ëPNG")
                                {
                                    Warn("The selected file is probably corrupt");
                                }
                                else
                                {
                                    //TODO for the future: Detect different image types and convert to PNG
                                    Warn(@"This file is either corrupt or a different format but with .png file extension.
A PNG header starts with 'ëPNG' but this file starts with " + header);
                                }
                            }
                            else
                            {
                                Warn("File is too short to be a PNG image");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Warn($"Unable to check the selected file. Reason: {ex.Message}", ex.GetType().Name);
                    }
                }
            }
        }

        private void Info(string Message, string Title = null)
        {
            MessageBox.Show(
                Message,
                string.IsNullOrEmpty(Title) ? Text : Title,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Warn(string Message, string Title = null)
        {
            MessageBox.Show(
                Message,
                string.IsNullOrEmpty(Title) ? Text : Title,
                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void Err(string Message, string Title = null)
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
                if (string.IsNullOrEmpty(tbEncodePassword.Text))
                {
                    Warn("Please enter a password or disable encryption", "Missing password");
                    tbEncodePassword.Focus();
                    return;
                }
                using (var pw = new frmConfirmPassword(tbEncodePassword.Text))
                {
                    if (pw.ShowDialog() != DialogResult.OK)
                    {
                        tbEncodePassword.Focus();
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
                            var Encrypted = Tools.EncryptData(tbEncodePassword.Text, File.ReadAllBytes(tbEncodeInput.Text));
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
                        var Encrypted = Tools.EncryptData(tbEncodePassword.Text, File.ReadAllBytes(tbEncodeInput.Text));
                        using (var MS = new MemoryStream(Encrypted, false))
                        {
                            Data = ImageWriter.PixelMode.CreateImageFromFile(
                                MS, Path.GetFileName(tbEncodeInput.Text), isPngOutput);
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
            }
            Info("Your file has been encoded into an image", "File encoded");
        }

        private void btnDecodeBrowse_Click(object sender, EventArgs e)
        {
            var f = Tools.BrowseFile(
                "Source image",
                "Supported images|*.png;*.bmp|PNG files|*.png|Bitmap files|*.bmp",
                tbDecodeInput.Text);
            if (!string.IsNullOrEmpty(f))
            {
                tbDecodeInput.Text = f;
            }
        }

        private void btnDecodeStart_Click(object sender, EventArgs e)
        {
            var isPNG = ImageWriter.HeaderMode.IsPNG(tbDecodeInput.Text);
            var decodePixels = cbDecodePixels.Checked;
            var decodeHeaders = isPNG && cbDecodeHeaders.Checked;
            //If at least one file was skipped
            var skipped = false;
            //If at least one file was decoded
            var decoded = false;
            if (!decodeHeaders && !decodePixels)
            {
                Warn("Please select at least one decoding method");
                return;
            }
            if (decodeHeaders)
            {
                if (!isPNG)
                {
                    Info("Header decoding is only supported for PNG images. This mode will be skipped.");
                }
                else
                {
                    PNGHeader[] Headers = null;
                    try
                    {
                        Headers = ImageWriter.HeaderMode
                            .ReadPNG(tbDecodeInput.Text)
                            .Where(m => m.IsDataHeader)
                            .ToArray();
                    }
                    catch (Exception ex)
                    {
                        Err("Failed to read the image as PNG. Reason: " + ex.Message, ex.GetType().Name);
                        return;
                    }
                    foreach (var Header in Headers)
                    {
                        var Encrypted = false;
                        var Data = Header.FileData;
                        using (var MS = new MemoryStream(Data, false))
                        {
                            Encrypted = crypt.Crypt.IsEncrypted(MS);
                        }
                        if (Encrypted)
                        {
                            using (var PW = new frmConfirmPassword(null))
                            {
                                if (PW.ShowDialog() == DialogResult.OK)
                                {
                                    Data = Tools.DecryptData(PW.PW, Data);
                                    if (Data == null)
                                    {
                                        Warn("Unable to decrypt " + Header.FileName + "\r\nWrong password?");
                                    }
                                }
                                else
                                {
                                    Data = null;
                                }
                            }
                        }
                        if (Data != null)
                        {
                            var FN = Path.Combine(Path.GetDirectoryName(tbDecodeInput.Text), Header.FileName);
                            FN = Tools.BrowseFile("Save As...", Preselected: FN, IsSave: true);
                            if (!string.IsNullOrEmpty(FN))
                            {
                                File.WriteAllBytes(FN, Data);
                                decoded = true;
                            }
                            else
                            {
                                skipped = true;
                            }
                        }
                        else
                        {
                            skipped = true;
                        }
                    }
                }
            }
            if (decodePixels)
            {
                FileStream FS;
                try
                {
                    FS = File.OpenRead(tbDecodeInput.Text);
                }
                catch (Exception ex)
                {
                    Err("Unable to open source file for pixel data decoding. Reason: " + ex.Message, ex.GetType().Name);
                    return;
                }
                using (FS)
                {
                    var IF = ImageWriter.PixelMode.CreateFileFromImage(FS);
                    if (!IF.IsEmpty)
                    {
                        var Encrypted = false;
                        var Data = IF.Data;
                        using (var MS = new MemoryStream(Data, false))
                        {
                            Encrypted = crypt.Crypt.IsEncrypted(MS);
                        }
                        if (Encrypted)
                        {
                            using (var PW = new frmConfirmPassword(null))
                            {
                                if (PW.ShowDialog() == DialogResult.OK)
                                {
                                    Data = Tools.DecryptData(PW.PW, Data);
                                    if (Data == null)
                                    {
                                        Warn("Unable to decrypt " + IF.FileName + "\r\nWrong password?");
                                    }
                                }
                                else
                                {
                                    Data = null;
                                }
                            }
                        }
                        if (Data != null)
                        {
                            var FN = Path.Combine(Path.GetDirectoryName(tbDecodeInput.Text), IF.FileName);
                            FN = Tools.BrowseFile("Save As...", Preselected: FN, IsSave: true);
                            if (!string.IsNullOrEmpty(FN))
                            {
                                File.WriteAllBytes(FN, Data);
                                decoded = true;
                            }
                            else
                            {
                                skipped = true;
                            }
                        }
                        else
                        {
                            skipped = true;
                        }
                    }
                    else
                    {
                        Info("Pixel data does not contain an encoded file");
                    }
                }
            }
            if(decoded)
            {
                if(skipped)
                {
                    Warn("Image decoding complete, but at least one file was skipped");
                }
                else
                {
                    Info("Image decoding complete");
                }
            }
            else
            {
                Warn("No files were decoded from the image");
            }
        }
    }
}
