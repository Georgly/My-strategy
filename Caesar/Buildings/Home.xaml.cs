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
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl, IBuilding
    {
        const int people = 10;
        const int cost = 50;
        int _level;
        int _maxPeople;
        bool _haveWell;
        int _x, _y;
        public int Id { get; set; }

        public Home(int x, int y)
        {
            InitializeComponent();
            Level = 1;
            MaxPeople = Level * people;
            HaveWell = false;
            _x = x;
            _y = y;
            Margin = new Thickness(X, Y, 0, 0);
        }

        public void CheckWell()//nothing
        {

        }

        public int Level
        {
            get { return _level; }
            set { _level = value; }
        }

        public int MaxPeople
        {
            get { return _maxPeople; }
            set { _maxPeople = value; }
        }

        public bool HaveWell
        {
            get { return _haveWell; }
            set { _haveWell = value; }
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
