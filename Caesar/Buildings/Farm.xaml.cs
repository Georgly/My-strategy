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
        const int workerCount = 10;
        const int product = 50;
        //bool goPeople = true;
        public bool needPeople = true;//
        int _x, _y;
        Worker worker;
        DispatcherTimer work;
        public int Id { get; set; }
        Cell[,] map;
        List<Cell> _path;
        int distance;
        DispatcherTimer toStorage;//TODO !!!надо срочно сделать перемещение!!!
        DispatcherTimer fromStorage;

        public Farm(int x, int y)
        {
            InitializeComponent();
            _path = new List<Cell>();
            _x = x;
            _y = y;
            int y2 = (int)(y + Height);
            worker = new Worker(x, y2, Id);
            Margin = new Thickness(x, y, 0, 0);
            work = new DispatcherTimer();

            work.Interval = TimeSpan.FromSeconds(30);
            work.Tick += Work_Tick;
        }

        public void Start(Cell[,] field)
        {
            work.Start();
            map = field;
            //FindWay(_x, _y, (int)/*this.*/Width, 3/*, field*/);
        }

        private void Work_Tick(object sender, EventArgs e)
        {
            //goPeople = false;
            //SentFood(null, null);
            work.Stop();
            _path = FindWay(_x, _y, (int)this.Width, 3/*, field*/);
            map = null;
            if (_path != null)
            {
                toStorage = new DispatcherTimer();
                toStorage.Interval = TimeSpan.FromMilliseconds(25);
                toStorage.Tick += ToStorage_Tick;
                distance = _path.Count - 1;
                AddWorker(null, null);
                toStorage.Start();
            }
        }

        private void ToStorage_Tick(object sender, EventArgs e)
        {
            if (distance == 0)
            {
                toStorage.Stop();
                fromStorage = new DispatcherTimer();
                fromStorage.Interval = TimeSpan.FromMilliseconds(10);
                fromStorage.Tick += FromStorage_Tick;
                fromStorage.Start();
            }
            else
            {
                worker.Margin = new Thickness(worker.Margin.Left + (_path[distance - 1].XIndex * 40 - _path[distance].XIndex * 40) / 20,
                    worker.Margin.Top + (_path[distance - 1].YIndex * 40 - _path[distance].YIndex * 40) / 20, 0, 0);
                if (worker.Margin.Left == _path[distance - 1].XIndex * 40 && worker.Margin.Top == _path[distance - 1].YIndex * 40)
                {
                    distance--;
                }
            }
        }

        private void FromStorage_Tick(object sender, EventArgs e)
        {
            if (distance == _path.Count)
            {
                fromStorage.Stop();
                DeleteWorker(null, null);
            }
            else
            {
                worker.Margin = new Thickness(Margin.Left + (_path[distance].XIndex * 40 - _path[distance - 1].XIndex * 40) / 20,
                    Margin.Top + (_path[distance].YIndex * 40 - _path[distance - 1].YIndex * 40) / 20, 0, 0);
                if (worker.Margin.Left == _path[distance].XIndex * 40 && worker.Margin.Top == _path[distance].YIndex * 40)
                {
                    distance++;
                }
            }
        }

        List<Cell> FindWay(int startPointX, int startPointY, int startWidth, int endType/*, Cell[,] field*/)
        {
            Cell endCell = new Cell(endType);
            endCell.Weight = int.MaxValue;
            Cell startCell = new Cell();
            int weight = 0;
            List<Cell> oldCelles = new List<Cell>();// точки из которых распространяется волна
            List<Cell> findCelles = new List<Cell>();
            List<Cell> path = new List<Cell>();
            if (map[startPointY / 40 + startWidth / 40, startPointX / 40].Type == 5)
            {
                map[startPointY / 40 + startWidth / 40, startPointX / 40].Weight = weight;
                map[startPointY / 40 + startWidth / 40, startPointX / 40].Visit = true;
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
                    if (check.XIndex >= 0 && check.XIndex < map.GetLength(1)
                        && check.YIndex - 1 >= 0 && check.YIndex - 1 < map.GetLength(0))
                    {
                        if (!map[check.YIndex - 1, check.XIndex].Visit)
                        {
                            map[check.YIndex - 1, check.XIndex].Visit = true;
                            if (map[check.YIndex - 1, check.XIndex].Type == endType)
                            {
                                if (check.XIndex - 1 >= 0 && map[check.YIndex - 1, check.XIndex - 1].Type == endType)
                                { }
                                else
                                {
                                    map[check.YIndex - 1, check.XIndex].Weight = weight;
                                    endCell = map[check.YIndex - 1, check.XIndex];
                                    findCelles.Clear();
                                    break;
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
                        && check.YIndex >= 0 && check.YIndex < map.GetLength(0))
                    {
                        if (!map[check.YIndex, check.XIndex + 1].Visit)
                        {
                            map[check.YIndex, check.XIndex + 1].Visit = true;
                            if (map[check.YIndex, check.XIndex + 1].Type == 5)
                            {
                                map[check.YIndex, check.XIndex + 1].Weight = weight;
                                findCelles.Add(map[check.YIndex, check.XIndex + 1]);
                            }
                        }
                    }
                    if (check.XIndex >= 0 && check.XIndex < map.GetLength(1)
                        && check.YIndex + 1 > 0 && check.YIndex + 1 < map.GetLength(0))
                    {
                        if (!map[check.YIndex + 1, check.XIndex].Visit)
                        {
                            map[check.YIndex + 1, check.XIndex].Visit = true;
                            if (map[check.YIndex + 1, check.XIndex].Type == 5)
                            {
                                map[check.YIndex + 1, check.XIndex].Weight = weight;
                                findCelles.Add(map[check.YIndex + 1, check.XIndex]);
                            }
                        }
                    }
                    if (check.XIndex - 1 >= 0 && check.XIndex - 1 < map.GetLength(1)
                        && check.YIndex >= 0 && check.YIndex < map.GetLength(0))
                    {
                        if (!map[check.YIndex, check.XIndex - 1].Visit)
                        {
                            map[check.YIndex, check.XIndex - 1].Visit = true;
                            if (map[check.YIndex, check.XIndex - 1].Type == 5)
                            {
                                map[check.YIndex, check.XIndex - 1].Weight = weight;
                                findCelles.Add(map[check.YIndex, check.XIndex - 1]);
                            }
                        }
                    }
                }
                oldCelles.Clear();
                for (int i = 0; i < findCelles.Count; i++)
                {
                    oldCelles.Add(findCelles[i]);
                }
            }
            // путь
            if (endCell.Weight < map.Length)
            {
                Cell pathCell = map[endCell.YIndex + 1, endCell.XIndex];
                while (pathCell.Weight != 0)
                {
                    if (pathCell.XIndex >= 0 && pathCell.XIndex < map.GetLength(1)
                        && pathCell.YIndex + 1 > 0 && pathCell.YIndex + 1 < map.GetLength(0) && 
                        map[pathCell.YIndex + 1, pathCell.XIndex].Weight == pathCell.Weight - 1)
                    {
                        path.Add(pathCell);
                        pathCell = map[pathCell.YIndex + 1, pathCell.XIndex];
                    }
                    else if (pathCell.XIndex - 1 >= 0 && pathCell.XIndex - 1 < map.GetLength(1)
                        && pathCell.YIndex >= 0 && pathCell.YIndex < map.GetLength(0) &&
                        map[pathCell.YIndex, pathCell.XIndex - 1].Weight == pathCell.Weight - 1)
                    {
                        path.Add(pathCell);
                        pathCell = map[pathCell.YIndex, pathCell.XIndex - 1];
                    }
                    else if (pathCell.XIndex >= 0 && pathCell.XIndex < map.GetLength(1)
                        && pathCell.YIndex - 1 >= 0 && pathCell.YIndex - 1 < map.GetLength(0) &&
                        map[pathCell.YIndex - 1, pathCell.XIndex].Weight == pathCell.Weight - 1)
                    {
                        path.Add(pathCell);
                        pathCell = map[pathCell.YIndex - 1, pathCell.XIndex];
                    }
                    else if (pathCell.XIndex + 1 > 0 && pathCell.XIndex + 1 < map.GetLength(1)
                        && pathCell.YIndex >= 0 && pathCell.YIndex < map.GetLength(0) &&
                        map[pathCell.YIndex, pathCell.XIndex + 1].Weight == pathCell.Weight - 1)
                    {
                        path.Add(pathCell);
                        pathCell = map[pathCell.YIndex, pathCell.XIndex + 1];
                    }
                }
                path.Add(startCell);
                return path;
            }
            return null;
        }//идеал неоптимальности

        public delegate void CanvasWorker(Worker worker);
        public event CanvasWorker Create;
        public void AddWorker(object sender, EventArgs e)
        {
            Create(worker);
        }

        //public delegate void Go(Worker worker);
        public event CanvasWorker Remove;
        public void DeleteWorker(object sender, EventArgs e)
        {
            Remove(worker);
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

        public int WorkerCount
        {
            get { return workerCount; }
        }

        public int Product
        {
            get { return product; }
        }
    }
}
