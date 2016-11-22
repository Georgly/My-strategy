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
    /// Interaction logic for Draw_well.xaml
    /// </summary>
    public partial class Draw_well : UserControl, IBuilding
    {
        const int cost = 20;
        int _x, _y;
        public int Id { get; set; }

        public Draw_well(int x, int y)
        {
            InitializeComponent();
            _x = x;
            _y = y;
            Margin = new Thickness(x, y, 0, 0);
        }

        public void GiveWater()//nothing
        {

        }

        public int X
        {
            get { return _x; }
            set { _x = value; }
        }

        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public int Cost
        {
            get { return cost; }
        }
    }
}
