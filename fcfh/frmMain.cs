using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            if (!(tbEncrypt.Enabled = cbEncrypt.Checked))
            {
                //It's a good idea to clear the password if the user disables encryption
                tbEncrypt.Text = string.Empty;
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
    }
}
