using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ANote_v2
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("版权所有 （c） 2022，Austin_Xu\r\n保留所有权利。\r\n\r\n以源代码和二进制形式重新分发和使用，有或没有\r\n允许修改，前提是满足以下条件：\r\n\r\n1. 重新分发源代码必须保留上述版权声明，本\r\n条件列表和以下免责声明。\r\n\r\n2. 二进制形式的再分发必须复制上述版权声明，\r\n此条件列表和文档中的以下免责声明\r\n和/或随分发提供的其他材料。\r\n\r\n本软件由版权所有者和贡献者“按原样”提供\r\n以及任何明示或暗示的保证，包括但不限于\r\n对适销性和特定用途适用性的默示保证是\r\n否认。在任何情况下，版权所有者或贡献者均不承担任何责任\r\n对于任何直接、间接、附带、特殊、惩戒性或后果性\r\n损害赔偿（包括但不限于采购替代商品或\r\n服务业;使用、数据或利润损失;或业务中断）但是\r\n引起和基于任何责任理论，无论是在合同中，严格责任，\r\n或以任何方式因使用而产生的侵权行为（包括疏忽或其他行为）\r\n本软件，即使已被告知此类损坏的可能性。\r\n", "BSD 2 条款许可证");
        }
        private void button2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Austixu/ANote");
        }
    }
}
