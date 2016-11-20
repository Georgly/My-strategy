using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Caesar
{
    class Player
    {
        List<IBuilding> _buildings;
        Canvas _field = new Canvas();
        int _year;
        int _peoples;
        int _freePeoples;
        int[] _resources = new int[2]; //0-gold, 1-bread
        List<Road> roads;
        int lastXRoad, lastYRoad;
        Cell[,] _fieldMatrix;
        Rectangle testBuilding;
        int _id;
        //Label yearL, foodL, goldL, manL;
        //Graf _buildGraf;
        DispatcherTimer year = new DispatcherTimer();
        DispatcherTimer eat = new DispatcherTimer();

        public Player(Canvas myCanvas/*, Label yearLab, Label foodLab, Label goldLab, Label manLab*/)
        {
            Buildings = new List<IBuilding>();
            _id = 0;
            //_buildGraf = new Graf();
            _field = myCanvas;
            Year = -30;
            Peoples = 0;
            FreePeoples = 0;
            Resources[0] = 3500;
            Resources[1] = 150;
            roads = new List<Road>();
            FieldCoord();
            testBuilding = new Rectangle();
            StartTimers();
            //Refresh(null, null);
        }

        void StartTimers(/*Label yearLab, Label foodLab*/)
        {
            year.Interval = TimeSpan.FromMinutes(12);
            year.Tick += Year_Tick;
            eat.Interval = TimeSpan.FromMinutes(1.5);
            eat.Tick += Eat_Tick;
            //yearL = yearLab;
            //foodL = foodLab;
            year.Start();
            eat.Start();
        }

        private void Eat_Tick(object sender, EventArgs e)
        {
            Resources[1] -= (int)(0.5 * Peoples);
            //foodL.Dispatcher.InvokeAsync(() => { foodL.Content = Resources[1]; });
            Refresh(null, null);
        }

        private void Year_Tick(object sender, EventArgs e)
        {
            Year++;
            Refresh(null, null);
            //yearL.Dispatcher.InvokeAsync(() => { yearL.Content = Year; });
            TakeMoney();
        }

        public delegate void RefreshD();
        public event RefreshD RefreshE;
        public void Refresh(object sender, EventArgs e)
        {
            RefreshE();
        }
        //void Refresh()
        //{
        //    if (Year > 0)
        //    {
        //        yearL.Dispatcher.InvokeAsync(() => { yearL.Content = Year + " AD"; });
        //    }
        //    else
        //    {
        //        yearL.Dispatcher.InvokeAsync(() => { yearL.Content = Math.Abs(Year) + " BC"; });
        //    }
        //    goldL.Dispatcher.InvokeAsync(() => { goldL.Content = Resources[0]; });
        //    foodL.Dispatcher.InvokeAsync(() => { foodL.Content = Resources[1]; });
        //    manL.Dispatcher.InvokeAsync(() => { manL.Content = Peoples; });
        //}

        void TakeMoney()//сбор налогов в конце года
        {

        }

        //void TransportFood()//отправка доставщика с фермы в амбар и обратно
        //{ }

        //List<Cell> FindWay(int startPointX, int startPointY, int startWidth, /*int endPointX, int endPointY,*/ int endType, Cell[,] field)// not ready shit!
        //{
        //    Cell[,] map = field;
        //    Cell endCell = new Cell(endType);
        //    endCell.Weight = int.MaxValue;
        //    Cell startCell = new Cell();
        //    int weight = 0;
        //    List<Cell> oldCelles = new List<Cell>();// точки из которых распространяется волна
        //    List<Cell> findCelles = new List<Cell>();
        //    List<Cell> path = new List<Cell>();
        //    //Cell startCell = new Cell();
        //    if (map[startPointY / 40 + startWidth / 40, startPointX / 40].Type == 5)
        //    {
        //        map[startPointY / 40 + startWidth / 40, startPointX / 40].Weight = weight;
        //        startCell = map[startPointY / 40 + startWidth / 40, startPointX / 40];
        //        oldCelles.Add(startCell);
        //    }
        //    // распространение волны
        //    while (oldCelles.Count != 0)
        //    {
        //        weight++;
        //        for (int i = 0; i < oldCelles.Count; i++)
        //        {
        //            Cell check = oldCelles[i];
        //            //TODO посмотреть потом, как оптимизировать: убрать четыре ифа с одинаковым содержанием
        //            if (check.XIndex > 0 && check.XIndex < map.GetLength(1) 
        //                && check.YIndex - 1 > 0 && check.YIndex - 1 < map.GetLength(0))
        //            {
        //                if (!map[check.YIndex - 1, check.XIndex].Visit)
        //                {
        //                    if (map[check.YIndex - 1, check.XIndex].Type == endType)
        //                    {
        //                        map[check.YIndex - 1, check.XIndex].Weight = weight;
        //                        if (map[check.YIndex - 1, check.XIndex].Weight < endCell.Weight)
        //                        {
        //                            endCell = map[check.YIndex - 1, check.XIndex];
        //                        }
        //                    }
        //                    else if (map[check.YIndex - 1, check.XIndex].Type == 5)
        //                    {
        //                        map[check.YIndex - 1, check.XIndex].Weight = weight;
        //                        findCelles.Add(map[check.YIndex - 1, check.XIndex]);
        //                    }
        //                }
        //            }
        //            if (check.XIndex + 1 > 0 && check.XIndex + 1 < map.GetLength(1)
        //                && check.YIndex > 0 && check.YIndex < map.GetLength(0))
        //            {
        //                if (!map[check.YIndex, check.XIndex + 1].Visit)
        //                {
        //                    if (map[check.YIndex, check.XIndex + 1].Type == 5)
        //                    {
        //                        map[check.YIndex, check.XIndex + 1].Weight = weight;
        //                        findCelles.Add(map[check.YIndex, check.XIndex + 1]);
        //                    }
        //                }
        //            }
        //            if (check.XIndex > 0 && check.XIndex < map.GetLength(1)
        //                && check.YIndex + 1> 0 && check.YIndex + 1 < map.GetLength(0))
        //            {
        //                if (!map[check.YIndex + 1, check.XIndex].Visit)
        //                {
        //                    if (map[check.YIndex + 1, check.XIndex].Type == 5)
        //                    {
        //                        map[check.YIndex + 1, check.XIndex].Weight = weight;
        //                        findCelles.Add(map[check.YIndex + 1, check.XIndex]);
        //                    }
        //                }
        //            }
        //            if (check.XIndex - 1 > 0 && check.XIndex - 1 < map.GetLength(1)
        //                && check.YIndex > 0 && check.YIndex < map.GetLength(0))
        //            {
        //                if (!map[check.YIndex, check.XIndex - 1].Visit)
        //                {
        //                    if (map[check.YIndex, check.XIndex - 1].Type == 5)
        //                    {
        //                        map[check.YIndex, check.XIndex - 1].Weight = weight;
        //                        findCelles.Add(map[check.YIndex, check.XIndex - 1]);
        //                    }
        //                }
        //            }
        //        }
        //        oldCelles = findCelles;
        //    }
        //    // путь
        //    if (endCell.Weight < map.Length)
        //    {
        //        Cell pathCell = map[endCell.YIndex + 1, endCell.XIndex];
        //        //path.Add(pathCell);
        //        while (pathCell.Weight != 0)
        //        {
        //            if (map[endCell.YIndex + 1, endCell.XIndex].Weight == pathCell.Weight - 1)
        //            {
        //                path.Add(pathCell);
        //                pathCell = map[endCell.YIndex + 1, endCell.XIndex];
        //            }
        //            else if (map[endCell.YIndex, endCell.XIndex - 1].Weight == pathCell.Weight - 1)
        //            {
        //                path.Add(pathCell);
        //                pathCell = map[endCell.YIndex, endCell.XIndex - 1];
        //            }
        //            else if (map[endCell.YIndex - 1, endCell.XIndex].Weight == pathCell.Weight - 1)
        //            {
        //                path.Add(pathCell);
        //                pathCell = map[endCell.YIndex - 1, endCell.XIndex];
        //            }
        //            else if (map[endCell.YIndex, endCell.XIndex + 1].Weight == pathCell.Weight - 1)
        //            {
        //                path.Add(pathCell);
        //                pathCell = map[endCell.YIndex, endCell.XIndex + 1];
        //            }
        //        }
        //        path.Add(startCell);
        //        return path;
        //    }
        //    return null;
        //}

        void FieldCoord()
        {
            _fieldMatrix = new Cell[(int)_field.Height / 40,(int)_field.Width / 40];
            for (int i = 0; i < (int)_field.Height / 40; i++)
            {
                for (int j = 0; j < (int)_field.Width / 40; j++)
                {
                    _fieldMatrix[i, j] = new Cell();
                    _fieldMatrix[i, j].Type = 0;
                    _fieldMatrix[i, j].Visit = false;
                    _fieldMatrix[i, j].YIndex = i;
                    _fieldMatrix[i, j].XIndex = j;
                }
            }
        }

        bool CheckFree(int width, /*int height,*/ int x, int y)
        {
            if (x > _field.Width || y > _field.Height)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < width / 40; i++)
                {
                    for (int j = 0; j < width / 40; j++)
                    {
                        if (_fieldMatrix[y / 40 + i, x / 40 + j].Type != 0)
                            return false;
                    }
                }
                return true;
            }
        }

        void FillField(int width, int x, int y, int id, int type)
        {
            for (int i = 0; i < width / 40; i++)
            {
                for (int j = 0; j < width / 40; j++)
                {
                    _fieldMatrix[y / 40 + i, x / 40 + j].Type = type;
                    _fieldMatrix[y / 40 + i, x / 40 + j].Id = id;
                }
            }
        }

        int RoundPosition(int pos)
        {
            int delta = pos % 10;
            pos -= delta;
            if (pos % 40 < 30)
            {
                return pos - pos % 40;
            }
            else
            {
                return pos + 40 - pos % 40;
            }
        }

        //void FindRoad(int xCoord, int yCoord, int type, int width)
        //{
        //    GNode build = _buildGraf.FindNode(xCoord, yCoord);
        //    if (_buildGraf.FindNode(xCoord, yCoord - 40) != null && _buildGraf.FindNode(xCoord, yCoord - 40).type == 5)
        //    {
        //        _buildGraf.selectNode = build;
        //        _buildGraf.aimNode = _buildGraf.FindNode(xCoord, yCoord - 40);
        //        _buildGraf.SemiEdge();
        //    }
        //    else if (_buildGraf.FindNode(xCoord + width, yCoord) != null && _buildGraf.FindNode(xCoord + width, yCoord).type == 5)
        //    {
        //        _buildGraf.selectNode = build;
        //        _buildGraf.aimNode = _buildGraf.FindNode(xCoord + width, yCoord);
        //        _buildGraf.SemiEdge();
        //    }
        //    else if (_buildGraf.FindNode(xCoord, yCoord + width) != null && _buildGraf.FindNode(xCoord, yCoord + width).type == 5)
        //    {
        //        _buildGraf.selectNode = build;
        //        _buildGraf.aimNode = _buildGraf.FindNode(xCoord, yCoord + width);
        //        _buildGraf.SemiEdge();
        //    }
        //    else if (_buildGraf.FindNode(xCoord - width, yCoord) != null && _buildGraf.FindNode(xCoord - width, yCoord).type == 5)
        //    {
        //        _buildGraf.selectNode = build;
        //        _buildGraf.aimNode = _buildGraf.FindNode(xCoord - width, yCoord);
        //        _buildGraf.SemiEdge();
        //    }
        //}

        public void BuildHouse(int x, int y)
        {
            int xCoord = RoundPosition(x);
            int yCoord = RoundPosition(y);
            Home home = new Home(xCoord, yCoord);
            if (CheckFree((int)home.Width,/* + xCoord, (int)home.Height + yCoord,*/ xCoord, yCoord))
            {
                //_buildGraf.AddNode(xCoord, yCoord, 1);
                //FindRoad(xCoord, yCoord, 1, (int)home.Width);
                Buildings.Add(home);
                Peoples += home.MaxPeople;
                FreePeoples += home.MaxPeople;
                Resources[0] -= home.Cost;
                home.Id = _id;
                _field.Children.Add(home);
                FillField((int)home.Width/* + xCoord, (int)home.Height + yCoord*/, xCoord, yCoord, _id, 1);
                _id++;
                //FindRoad(home);
            }
        }

        public void BuildFarm(int x, int y)
        {
            int xCoord = RoundPosition(x);
            int yCoord = RoundPosition(y);
            Farm farm = new Farm(xCoord, yCoord);
            if (CheckFree((int)farm.Width,/* + xCoord, (int)farm.Height + yCoord,*/ xCoord, yCoord))
            {
                Buildings.Add(farm);
                //_buildGraf.AddNode(xCoord, yCoord, 2);
                //FindRoad(xCoord, yCoord, 2, (int)farm.Width);
                _field.Children.Add(farm);
                Resources[0] -= farm.Cost;
                farm.Id = _id;
                FillField((int)farm.Width/* + xCoord, (int)home.Height + yCoord*/, xCoord, yCoord, _id, 2);
                if (farm.Worker <= FreePeoples)
                {
                    FreePeoples -= farm.Worker;
                    farm.needPeople = false;
                    farm.Start(_fieldMatrix);
                    //farm.ReadyFood += TransportFood;
                }
                //farm.ReadyFood += TransportFood;
                _id++;
            }
        }

        public void BuildStorage(int x, int y)
        {
            int xCoord = RoundPosition(x);
            int yCoord = RoundPosition(y);
            Storage storage = new Storage(xCoord, yCoord);
            if (CheckFree((int)storage.Width,/* + xCoord, (int)storage.Height + yCoord,*/ xCoord, yCoord))
            {
                Buildings.Add(storage);
                //_buildGraf.AddNode(xCoord, yCoord, 3);
                //FindRoad(xCoord, yCoord, 3, (int)storage.Width);
                _field.Children.Add(storage);
                Resources[0] -= storage.Cost;
                storage.Id = _id;
                if (storage.Worker <= FreePeoples)
                {
                    FreePeoples -= storage.Worker;
                    storage.needPeople = false;
                }
                FillField((int)storage.Width/* + xCoord, (int)home.Height + yCoord*/, xCoord, yCoord, _id, 3);
                _id++;
            }
        }

        public void BuildDraw_well(int x, int y)
        {
            int xCoord = RoundPosition(x);
            int yCoord = RoundPosition(y);
            Draw_well draw_well = new Draw_well(xCoord, yCoord);
            if (CheckFree((int)draw_well.Width,/* + xCoord, (int)draw_well.Height + yCoord, */xCoord, yCoord))
            {
                Buildings.Add(draw_well);
                //_buildGraf.AddNode(xCoord, yCoord, 4);
                _field.Children.Add(draw_well);
                Resources[0] -= draw_well.Cost;
                draw_well.Id = _id;
                FillField((int)draw_well.Width/* + xCoord, (int)home.Height + yCoord*/, xCoord, yCoord, _id, 1);
                _id++;
            }
        }

        public void BuildRoad(int x, int y)
        {
            int xCoord = RoundPosition(x);
            int yCoord = RoundPosition(y);
            Road road = new Road(xCoord, yCoord);
            if (CheckFree((int)road.Width,/* + xCoord, (int)road.Height + yCoord, */xCoord, yCoord))
            {
                if (roads.Count != 0)
                {
                    if (lastXRoad == road.X && (lastYRoad + 40 >= road.Y || lastYRoad + 40 <= road.Y))
                    {
                        Buildings.Add(road);
                        _field.Children.Add(road);
                        Resources[0] -= road.Cost;
                        road.Id = _id;
                        FillField((int)road.Width/* + xCoord, (int)road.Height + yCoord*/, xCoord, yCoord, _id, 5);
                        _id++;
                        roads.Add(road);
                        //_buildGraf.AddNode(xCoord, yCoord, 5);
                    }
                    else if ((lastXRoad + 40 <= road.X || lastXRoad + 40 >= road.X) && lastYRoad == road.Y)
                    {
                        Buildings.Add(road);
                        _field.Children.Add(road);
                        Resources[0] -= road.Cost;
                        FillField((int)road.Width/* + xCoord, (int)road.Height + yCoord*/, xCoord, yCoord, _id, 5);
                        _id++;
                        roads.Add(road);
                        //_buildGraf.AddNode(xCoord, yCoord, 5);
                    }
                }
                else
                {
                    lastXRoad = xCoord;
                    lastYRoad = yCoord;
                    Buildings.Add(road);
                    _field.Children.Add(road);
                    Resources[0] -= road.Cost;
                    FillField((int)road.Width/* + xCoord, (int)road.Height + yCoord*/, xCoord, yCoord, _id, 5);
                    _id++;
                    roads.Add(road);
                }
            }
            //else
            //{
            //    if (_buildGraf.FindNode(xCoord, yCoord) != null && _buildGraf.FindNode(xCoord, yCoord).type == 5)
            //    {
            //        if (roads.Count == 0)
            //        {
            //            lastXRoad = xCoord;
            //            lastYRoad = yCoord;
            //        }
            //        roads.Add(road);
            //    }
            //}
        }

        public void NewRoad()
        {
            //if (roads.Count > 0)
            //{
            //    for (int i = 0; i < roads.Count - 1; i++)
            //    {
            //        _buildGraf.selectNode = _buildGraf.FindNode(roads[i].X, roads[i].Y, 5);
            //        _buildGraf.aimNode = _buildGraf.FindNode(roads[i + 1].X, roads[i + 1].Y, 5);
            //        _buildGraf.SemiEdge();
            //    }
            //}
            roads.Clear();
        }

        public void CanDraw(int x, int y, int width, int height)
        {
            _field.Children.Remove(testBuilding);
            int xCoord = RoundPosition(x);
            int yCoord = RoundPosition(y);
            if (xCoord + height < RoundPosition((int)_field.Width) && yCoord + width < RoundPosition((int)_field.Height))
            {
                //int xCoord = RoundPosition(x);
                //int yCoord = RoundPosition(y);
                testBuilding.Margin = new System.Windows.Thickness(xCoord, yCoord, 0, 0);
                testBuilding.Width = width;
                testBuilding.Height = height;
                testBuilding.Opacity = 0.75;
                if (CheckFree(width,/* + xCoord, height + yCoord,*/ xCoord, yCoord))
                {
                    testBuilding.Fill = Brushes.DarkSeaGreen;
                }
                else
                {
                    testBuilding.Fill = Brushes.DarkRed;
                }
                _field.Children.Add(testBuilding);
            }
        }

        public int Peoples
        {
            get { return _peoples; }
            set { _peoples = value; }
        }

        public int FreePeoples
        {
            get { return _freePeoples; }
            set { _freePeoples = value; }
        }

        public int[] Resources
        {
            get { return _resources; }
            set { _resources = value; }
        }

        public List<IBuilding> Buildings
        {
            get { return _buildings; }
            set { _buildings = value; }
        }

        public int Year
        {
            get { return _year; }
            set { _year = value; }
        }
    }
}
