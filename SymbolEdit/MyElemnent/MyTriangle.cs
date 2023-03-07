using SymbolEdit.SelectableElement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SymbolEdit.MyElemnent
{
    public class MyTriangle : MyElemnentBase, IMoveElementPoint
    {
        #region Constructors

        public MyTriangle() : base(3)
        {

        }

        #endregion

        #region Dependent attribut

        public double X1
        {
            get { return (double)GetValue(X1Property); }
            set { SetValue(X1Property, value); }
        }

        // Using a DependencyProperty as the backing store for X1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty X1Property =
            DependencyProperty.Register("X1", typeof(double), typeof(MyTriangle), new PropertyMetadata(double.NaN, new PropertyChangedCallback(RefreshElemnentDraw)));

        public double Y1
        {
            get { return (double)GetValue(Y1Property); }
            set { SetValue(Y1Property, value); }
        }

        // Using a DependencyProperty as the backing store for Y1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Y1Property =
            DependencyProperty.Register("Y1", typeof(double), typeof(MyTriangle), new PropertyMetadata(double.NaN, new PropertyChangedCallback(RefreshElemnentDraw)));

        public double X2
        {
            get { return (double)GetValue(X2Property); }
            set { SetValue(X2Property, value); }
        }

        // Using a DependencyProperty as the backing store for X2.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty X2Property =
            DependencyProperty.Register("X2", typeof(double), typeof(MyTriangle), new PropertyMetadata(double.NaN, new PropertyChangedCallback(RefreshElemnentDraw)));


        public double Y2
        {
            get { return (double)GetValue(Y2Property); }
            set { SetValue(Y2Property, value); }
        }

        // Using a DependencyProperty as the backing store for Y2.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Y2Property =
            DependencyProperty.Register("Y2", typeof(double), typeof(MyTriangle), new PropertyMetadata(double.NaN, new PropertyChangedCallback(RefreshElemnentDraw)));


        public double X3
        {
            get { return (double)GetValue(X3Property); }
            set { SetValue(X3Property, value); }
        }

        // Using a DependencyProperty as the backing store for X3.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty X3Property =
            DependencyProperty.Register("X3", typeof(double), typeof(MyTriangle), new PropertyMetadata(double.NaN, new PropertyChangedCallback(RefreshElemnentDraw)));

        public double Y3
        {
            get { return (double)GetValue(Y3Property); }
            set { SetValue(Y3Property, value); }
        }


        // Using a DependencyProperty as the backing store for Y3.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Y3Property =
            DependencyProperty.Register("Y3", typeof(double), typeof(MyTriangle), new PropertyMetadata(double.NaN, new PropertyChangedCallback(RefreshElemnentDraw)));

        #endregion  Dependent attribut

        #region Public Methods and Properties

        /// <summary>
        /// 验证点.
        /// </summary>
        /// <returns></returns>
        public override bool VerifyPoint()
        {
            if (double.IsNaN(X1) || double.IsNaN(Y1) || double.IsNaN(X2) || double.IsNaN(Y2) || double.IsNaN(X3) || double.IsNaN(Y3))
            {
                return false;
            }

            return true;
        }

        public override void GetPointRelativeLocation()
        {
            if (!this.VerifyPoint())
            {
                return;
            }

            PointRelativeLocation[0].X = X1 / Width;
            PointRelativeLocation[0].Y = Y1 / Height;
            PointRelativeLocation[1].X = X2 / Width;
            PointRelativeLocation[1].Y = Y2 / Height;
            PointRelativeLocation[2].X = X3 / Width;
            PointRelativeLocation[2].Y = Y3 / Height;
        }

        public override void PointAtuoAdaptation()
        {
            if (!this.VerifyRelativePointArrayExistNaN(PointRelativeLocation))
            {
                return;
            }

            IsGetPointRelativeLocation = false;
            X1 = PointRelativeLocation[0].X * Width;
            Y1 = PointRelativeLocation[0].Y * Height;
            X2 = PointRelativeLocation[1].X * Width;
            Y2 = PointRelativeLocation[1].Y * Height;
            X3 = PointRelativeLocation[2].X * Width;
            Y3 = PointRelativeLocation[2].Y * Height;
            IsGetPointRelativeLocation = true;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (!this.VerifyPoint())
            {
                return;
            }

            drawingContext.DrawLine(new Pen(Stroke, StrokeThickness), new Point(X1, Y1), new Point(X2, Y2));
            drawingContext.DrawLine(new Pen(Stroke, StrokeThickness), new Point(X2, Y2), new Point(X3, Y3));
            drawingContext.DrawLine(new Pen(Stroke, StrokeThickness), new Point(X3, Y3), new Point(X1, Y1));
        }

        public (bool, int) IsSelectedPoint(Point curPoint, double siez)
        {
            var rect1 = new Rect(new Point(X1 - siez / 2, Y1 - siez / 2), new Size(siez, siez));
            if (PointIsInsideRectangle(rect1, curPoint))
            {
                return (true, 0);
            }

            rect1 = new Rect(new Point(X2 - siez / 2, Y2 - siez / 2), new Size(siez, siez));
            if (PointIsInsideRectangle(rect1, curPoint))
            {
                return (true, 1);
            }
            
            rect1 = new Rect(new Point(X3 - siez / 2, Y3 - siez / 2), new Size(siez, siez));
            if (PointIsInsideRectangle(rect1, curPoint))
            {
                return (true, 2);
            }

            return (false, -1);
        }

        public void MoveElementPoint(int ordPointIndex, Point curPoint)
        {
            if (ordPointIndex == 0)
            {
                X1 = curPoint.X;
                Y1 = curPoint.Y;
            }
            else if (ordPointIndex == 1)
            {
                X2 = curPoint.X;
                Y2 = curPoint.Y;
            }
            else if (ordPointIndex == 2)
            {
                X3 = curPoint.X;
                Y3 = curPoint.Y;
            }
        }

        public virtual List<Point> GetSelectablePoints()
        {
            return new List<Point>() { new Point(X1, Y1), new Point(X2, Y2), new Point(X3, Y3) };
        }

        #endregion
    }
}
