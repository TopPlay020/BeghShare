using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeghShare.Events
{
    public class MouseMoveEvent
    {
        public MouseEventArgs e;
        public MouseMoveEvent(MouseEventArgs e)
        {
            this.e = e;
        }
    }
}
