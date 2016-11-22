using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        int _freePeople;
        int[] _resources = new int[2]; //0-gold, 1-bread
        List<Road> roads;
        int lastXRoad, lastYRoad;
        Cell[,] _fieldMatrix;
        Rectangle testBuilding;
        int _id;
        DispatcherTimer year = new DispatcherTimer();
        DispatcherTimer eat = new DispatcherTimer();
        BackgroundWorker freeMan = new BackgroundWorker();

        public Player(Canvas myCanvas)
        {
            Buildings = new List<IBuilding>();
            _id = 0;
            _field = myCanvas;
            Year = -30;
            People = 0;
            FreePeople = 0;
            Resources[0] = 3500;
            Resources[1] = 150;
            roads = new List<Road>();
            FieldCoord();
            testBuilding = new Rectangle();
            StartTimers();
            freeMan.DoWork += FreeMan_DoWork;
            freeMan.WorkerSupportsCancellation = true;
        }

        private void FreeMan_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < _fieldMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < _fieldMatrix.GetLength(1); j++)
                {
                    if (_fieldMatrix[i, j].Type == 2)
                    {
                        Farm farm = (Buildings[_fieldMatrix[i, j].Id] as Farm);
                        if (farm.needPeople)
                        {
                            farm.needPeople = false;
                            FreePeople -= farm.WorkerCount;
                            farm.Start(_fieldMatrix);
                            freeMan.CancelAsync();
                            break;
                        }
                    }
                }
            }
            freeMan.CancelAsync();
        }

        void StartTimers()
        {
            year.Interval = TimeSpan.FromMinutes(12);
            year.Tick += Year_Tick;
            eat.Interval = TimeSpan.FromMinutes(1.5);
            eat.Tick += Eat_Tick;
            year.Start();
            eat.Start();
        }

        private void Eat_Tick(object sender, EventArgs e)
        {
            Resources[1] -= People;
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

        void TakeMoney()
        {
            Resources[0] += 2 * (People - FreePeople) + FreePeople;
            Refresh(null, null);
        }

        void TransportFood(Farm farm)
        {
            Resources[1] += farm.Product;
            Refresh(null, null);
        }

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
                People += home.MaxPeople;
                FreePeople += home.MaxPeople;
                Resources[0] -= home.Cost;
                home.Id = _id;
                _field.Children.Add(home);
                FillField((int)home.Width/* + xCoord, (int)home.Height + yCoord*/, xCoord, yCoord, _id, 1);
                _id++;
                freeMan.RunWorkerAsync();
                //FindRoad(home);
            }
        }

        public void BuildFarm(int x, int y)
        {
            int xCoord = RoundPosition(x);
            int yCoord = RoundPosition(y);
            Farm farm = new Farm(xCoord, yCoord, _id);
            if (CheckFree((int)farm.Width,/* + xCoord, (int)farm.Height + yCoord,*/ xCoord, yCoord))
            {
                Buildings.Add(farm);
                //_buildGraf.AddNode(xCoord, yCoord, 2);
                //FindRoad(xCoord, yCoord, 2, (int)farm.Width);
                _field.Children.Add(farm);
                Resources[0] -= farm.Cost;
                //farm.Id = _id;
                FillField((int)farm.Width/* + xCoord, (int)home.Height + yCoord*/, xCoord, yCoord, _id, 2);
                if (farm.WorkerCount <= FreePeople)
                {
                    FreePeople -= farm.WorkerCount;
                    farm.needPeople = false;
                    farm.Start(_fieldMatrix);
                    //farm.ReadyFood += TransportFood;
                }
                farm.Create += AddToCanvas;
                farm.Remove += RemoveFromCanvas;
                farm.ReadyFood += TransportFood;
                //farm.ReadyFood += TransportFood;
                _id++;
            }
        }

        void AddToCanvas(Worker worker)
        {
            _field.Children.Add(worker);
            Panel.SetZIndex(worker, int.MaxValue);
        }

        void RemoveFromCanvas(Worker worker)
        {
            _field.Children.Remove(worker);
            (/*(Farm)*/Buildings[worker.Id] as Farm).Start(_fieldMatrix);
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
                //if (storage.Worker <= FreePeoples)
                //{
                //    FreePeoples -= storage.Worker;
                //    storage.needPeople = false;
                //}
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
            if (xCoord + height <= RoundPosition((int)_field.Width) && yCoord + width <= RoundPosition((int)_field.Height))
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

        public int People
        {
            get { return _peoples; }
            set { _peoples = value; }
        }

        public int FreePeople
        {
            get { return _freePeople; }
            set { _freePeople = value; }
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
