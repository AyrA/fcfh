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
                new EncodeType(){ DisplayString="Encode into header",Mode=OperationMode.UseHeader },
                new EncodeType(){ DisplayString="Encode into pixeldata",Mode=OperationMode.UsePixel }
            };
        }

        public frmMain()
        {
            InitializeComponent();

            cbEncodeType.Items.AddRange(EncodeType.Types.Cast<object>().ToArray());
            cbEncodeType.SelectedIndex = 0;
        }
    }
}
