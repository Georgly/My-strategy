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
    /// Interaction logic for Storage.xaml
    /// </summary>
    public partial class Storage : UserControl, IBuilding
    {
        const int cost = 150;
        //const int worker = 10;
        //public bool needPeople = true;
        //int _bread;
        int _x, _y;
        public int Id { get; set; }

        public Storage(int x, int y)
        {
            InitializeComponent();
            //Bread = 0;
            _x = x;
            _y = y;
            Margin = new Thickness(x, y, 0, 0);
        }

        //public int Bread
        //{
        //    get { return _bread; }
        //    set { _bread = value; }
        //}

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

        //public int Worker
        //{
        //    get { return worker; }
        //}
    }
}
