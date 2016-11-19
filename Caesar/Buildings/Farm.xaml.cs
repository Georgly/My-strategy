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
using System.Windows.Threading;

namespace Caesar
{
    /// <summary>
    /// Interaction logic for Farm.xaml
    /// </summary>
    public partial class Farm : UserControl, IBuilding
    {
        const int cost = 150;
        const int worker = 10;
        const int product = 150;
        //bool goPeople = true;
        public bool needPeople = true;
        int _x, _y;
        DispatcherTimer work;
        public int Id { get; set; }

        public Farm(int x, int y)
        {
            InitializeComponent();
            _x = x;
            _y = y;
            Margin = new Thickness(x, y, 0, 0);
            work = new DispatcherTimer();
            work.Interval = TimeSpan.FromSeconds(60);
            work.Tick += Work_Tick;
        }

        public void Start()
        {
            work.Start();
        }

        private void Work_Tick(object sender, EventArgs e)
        {
            //goPeople = false;
            //SentFood(null, null);
            work.Stop();
        }

        List<Cell> FindWay(int startPointX, int startPointY, int startWidth, /*int endPointX, int endPointY,*/ int endType, Cell[,] field)// not ready shit!
        {
            Cell[,] map = field;
            Cell endCell = new Cell(endType);
            endCell.Weight = int.MaxValue;
            Cell startCell = new Cell();
            int weight = 0;
            List<Cell> oldCelles = new List<Cell>();// точки из которых распространяется волна
            List<Cell> findCelles = new List<Cell>();
            List<Cell> path = new List<Cell>();
            //Cell startCell = new Cell();
            if (map[startPointY / 40 + startWidth / 40, startPointX / 40].Type == 5)
            {
                map[startPointY / 40 + startWidth / 40, startPointX / 40].Weight = weight;
                startCell = map[startPointY / 40 + startWidth / 40, startPointX / 40];
                oldCelles.Add(startCell);
            }
            // распространение волны
            while (oldCelles.Count != 0)
            {
                weight++;
                for (int i = 0; i < oldCelles.Count; i++)
                {
                    Cell check = oldCelles[i];
                    //TODO посмотреть потом, как оптимизировать: убрать четыре ифа с одинаковым содержанием
                    if (check.XIndex > 0 && check.XIndex < map.GetLength(1)
                        && check.YIndex - 1 > 0 && check.YIndex - 1 < map.GetLength(0))
                    {
                        if (!map[check.YIndex - 1, check.XIndex].Visit)
                        {
                            if (map[check.YIndex - 1, check.XIndex].Type == endType)
                            {
                                map[check.YIndex - 1, check.XIndex].Weight = weight;
                                if (map[check.YIndex - 1, check.XIndex].Weight < endCell.Weight)
                                {
                                    endCell = map[check.YIndex - 1, check.XIndex];
                                }
                            }
                            else if (map[check.YIndex - 1, check.XIndex].Type == 5)
                            {
                                map[check.YIndex - 1, check.XIndex].Weight = weight;
                                findCelles.Add(map[check.YIndex - 1, check.XIndex]);
                            }
                        }
                    }
                    if (check.XIndex + 1 > 0 && check.XIndex + 1 < map.GetLength(1)
                        && check.YIndex > 0 && check.YIndex < map.GetLength(0))
                    {
                        if (!map[check.YIndex, check.XIndex + 1].Visit)
                        {
                            if (map[check.YIndex, check.XIndex + 1].Type == 5)
                            {
                                map[check.YIndex, check.XIndex + 1].Weight = weight;
                                findCelles.Add(map[check.YIndex, check.XIndex + 1]);
                            }
                        }
                    }
                    if (check.XIndex > 0 && check.XIndex < map.GetLength(1)
                        && check.YIndex + 1 > 0 && check.YIndex + 1 < map.GetLength(0))
                    {
                        if (!map[check.YIndex + 1, check.XIndex].Visit)
                        {
                            if (map[check.YIndex + 1, check.XIndex].Type == 5)
                            {
                                map[check.YIndex + 1, check.XIndex].Weight = weight;
                                findCelles.Add(map[check.YIndex + 1, check.XIndex]);
                            }
                        }
                    }
                    if (check.XIndex - 1 > 0 && check.XIndex - 1 < map.GetLength(1)
                        && check.YIndex > 0 && check.YIndex < map.GetLength(0))
                    {
                        if (!map[check.YIndex, check.XIndex - 1].Visit)
                        {
                            if (map[check.YIndex, check.XIndex - 1].Type == 5)
                            {
                                map[check.YIndex, check.XIndex - 1].Weight = weight;
                                findCelles.Add(map[check.YIndex, check.XIndex - 1]);
                            }
                        }
                    }
                }
                oldCelles = findCelles;
            }
            // путь
            if (endCell.Weight < map.Length)
            {
                Cell pathCell = map[endCell.YIndex + 1, endCell.XIndex];
                //path.Add(pathCell);
                while (pathCell.Weight != 0)
                {
                    if (map[endCell.YIndex + 1, endCell.XIndex].Weight == pathCell.Weight - 1)
                    {
                        path.Add(pathCell);
                        pathCell = map[endCell.YIndex + 1, endCell.XIndex];
                    }
                    else if (map[endCell.YIndex, endCell.XIndex - 1].Weight == pathCell.Weight - 1)
                    {
                        path.Add(pathCell);
                        pathCell = map[endCell.YIndex, endCell.XIndex - 1];
                    }
                    else if (map[endCell.YIndex - 1, endCell.XIndex].Weight == pathCell.Weight - 1)
                    {
                        path.Add(pathCell);
                        pathCell = map[endCell.YIndex - 1, endCell.XIndex];
                    }
                    else if (map[endCell.YIndex, endCell.XIndex + 1].Weight == pathCell.Weight - 1)
                    {
                        path.Add(pathCell);
                        pathCell = map[endCell.YIndex, endCell.XIndex + 1];
                    }
                }
                path.Add(startCell);
                return path;
            }
            return null;
        }

        //public delegate void Send();
        //public event Send ReadyFood;
        //public void SentFood(object sender, EventArgs e)
        //{
        //    ReadyFood();
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

        public int Worker
        {
            get { return worker; }
        }

        public int Product
        {
            get { return product; }
        }
    }
}
