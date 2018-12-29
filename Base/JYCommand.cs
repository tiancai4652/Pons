using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WpfApplication2
{
    public class JYCommand
    {
        public JYCommand()
        {
            ID = Guid.NewGuid().ToString("N");
        }
        public string ID { get; set; }
        public bool IsRunning { get; set; }
        public bool IsEnabled { get; set; }
        public virtual Key_Values KeyValue { get; set; }
    }
}
