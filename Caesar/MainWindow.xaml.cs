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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Player player;
        int nowBuild;
        bool draw;

        public MainWindow()
        {
            InitializeComponent();
            player = new Player(myCanvas/*, year, food*/);
            //gold.Content = player.Resources[0];
            //food.Content = player.Resources[1];
            //man.Content = player.Peoples;
            //Refresh();
            player.RefreshE += Refresh;
            player.Refresh(null, null);
            draw = false;
        }

        void Refresh()
        {
            if (player.Year > 0)
            {
                year.Content = player.Year + " AD";
            }
            else
            {
                year.Content = Math.Abs(player.Year) + " BC";
            }
            gold.Content = player.Resources[0];
            food.Content = player.Resources[1];
            man.Content = player.Peoples;
        }

        private void house_Click(object sender, RoutedEventArgs e)
        {
            nowBuild = 1;
        }

        private void myCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            switch (nowBuild)
            {
                case 1:
                    {
                        player.BuildHouse((int)e.GetPosition(myCanvas).X, (int)e.GetPosition(myCanvas).Y);
                        break;
                    }
                case 2:
                    {
                        player.BuildFarm((int)e.GetPosition(myCanvas).X, (int)e.GetPosition(myCanvas).Y);
                        break;
                    }
                case 3:
                    {
                        player.BuildStorage((int)e.GetPosition(myCanvas).X, (int)e.GetPosition(myCanvas).Y);
                        break;
                    }
                case 4:
                    {
                        player.BuildDraw_well((int)e.GetPosition(myCanvas).X, (int)e.GetPosition(myCanvas).Y);
                        break;
                    }
                case 5:
                    {
                        player.BuildRoad((int)e.GetPosition(myCanvas).X, (int)e.GetPosition(myCanvas).Y);
                        draw = true;
                        break;
                    }
                default:
                    break;
            }
            Refresh();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                nowBuild = 0;
            }
        }

        private void farm_Click(object sender, RoutedEventArgs e)
        {
            nowBuild = 2;
        }

        private void storage_Click(object sender, RoutedEventArgs e)
        {
            nowBuild = 3;
        }

        private void draw_well_Click(object sender, RoutedEventArgs e)
        {
            nowBuild = 4;
        }

        private void road_Click(object sender, RoutedEventArgs e)
        {
            nowBuild = 5;
        }

        private void myCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            switch (nowBuild)
            {
                case 1:
                case 4:
                    {
                        player.CanDraw((int)e.GetPosition(myCanvas).X, (int)e.GetPosition(myCanvas).Y, 40, 40);
                        break;
                    }
                case 5:
                    {
                        if (draw)
                        {
                            player.BuildRoad((int)e.GetPosition(myCanvas).X, (int)e.GetPosition(myCanvas).Y);
                        }
                        else
                        {
                            player.CanDraw((int)e.GetPosition(myCanvas).X, (int)e.GetPosition(myCanvas).Y, 40, 40);
                        }
                        break;
                    }
                case 2:
                case 3:
                    {
                        player.CanDraw((int)e.GetPosition(myCanvas).X, (int)e.GetPosition(myCanvas).Y, 80, 80);
                        break;
                    }
                default:
                    break;
            }
            Refresh();
        }

        private void myCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (nowBuild == 5)
            {
                //nowBuild = 0;
                draw = false;
                player.NewRoad();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
