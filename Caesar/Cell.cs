using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caesar
{
    class Cell
    {
        public int Type { get; set; }
        public int Id { get; set; }
        public bool Visit { get; set; }
        public int Weight { get; set; }
        public int XIndex { get; set; }
        public int YIndex { get; set; }

        public Cell()
        {}

        public Cell(int id, int type)
        {
            Id = id;
            Type = type;
        }

        public Cell(int type)
        {
            Type = type;
        }
    }
}
