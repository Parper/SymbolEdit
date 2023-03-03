using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolEdit.MyElemnent
{
    public class RelativePoint
    {
        public RelativePoint()
        {
            this.X = double.NaN; 
            this.Y = double.NaN;
        }

        public RelativePoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; set; }

        public double Y { get; set; }
    }
}
