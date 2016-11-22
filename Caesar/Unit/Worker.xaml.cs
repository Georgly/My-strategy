using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Caesar
{
    /// <summary>
    /// Interaction logic for Worker.xaml
    /// </summary>
    public partial class Worker : UserControl
    {
        public bool WithGoods { get; set; }
        public int Id { get; set; }
        //int Direction { get; set; }

        public Worker(int x, int y, int id)
        {
            InitializeComponent();
            WithGoods = true;
            Margin = new Thickness(x, y, 0, 0);
            Id = id;
        }
    }
}
