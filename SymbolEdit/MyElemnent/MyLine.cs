using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SymbolEdit.MyElemnent
{
    public class MyLine : MyElemnentBase
    {
        public bool IsGetPointRelativeLocation { get; private set; } = true;

        #region Dependent attribute

        public double X1
        {
            get { return (double)GetValue(X1Property); }
            set { SetValue(X1Property, value); }
        }

        // Using a DependencyProperty as the backing store for X1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty X1Property =
            DependencyProperty.Register("X1", typeof(double), typeof(MyLine), new PropertyMetadata(double.NaN, new PropertyChangedCallback(RefreshLineDraw)));

        public double Y1
        {
            get { return (double)GetValue(Y1Property); }
            set { SetValue(Y1Property, value); }
        }

        // Using a DependencyProperty as the backing store for Y1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Y1Property =
            DependencyProperty.Register("Y1", typeof(double), typeof(MyLine), new PropertyMetadata(double.NaN, new PropertyChangedCallback(RefreshLineDraw)));

        public double X2
        {
            get { return (double)GetValue(X2Property); }
            set { SetValue(X2Property, value); }
        }

        // Using a DependencyProperty as the backing store for X2.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty X2Property =
            DependencyProperty.Register("X2", typeof(double), typeof(MyLine), new PropertyMetadata(double.NaN, new PropertyChangedCallback(RefreshLineDraw)));

        public double Y2
        {
            get { return (double)GetValue(Y2Property); }
            set { SetValue(Y2Property, value); }
        }

        // Using a DependencyProperty as the backing store for Y2.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Y2Property =
            DependencyProperty.Register("Y2", typeof(double), typeof(MyLine), new PropertyMetadata(double.NaN, new PropertyChangedCallback(RefreshLineDraw)));


        #endregion

        #region Private  Methods and Properties

        /// <summary>
        /// 记录相对位置.
        /// </summary>
        private readonly double[] pointRelativeLocation = new double[4] { double.NaN, double.NaN, double.NaN, double.NaN, };

        private Geometry geometry = Geometry.Empty;

        private static void RefreshLineDraw(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MyLine myLine)
            {
                if (myLine.IsGetPointRelativeLocation)
                {
                    myLine.GetPointRelativeLocation();
                }
                myLine.GetGeometry();
                myLine.InvalidateVisual();
            }
        }

        /// <summary>
        /// 验证点.
        /// </summary>
        /// <returns></returns>
        private bool VerifyPoint()
        {
            if (double.IsNaN(X1) || double.IsNaN(Y1) || double.IsNaN(X2) || double.IsNaN(Y2))
            {
                return false;
            }

            return true;
        }

        #endregion


        #region protected Members 

        protected override Geometry DefiningGeometry => geometry;

        #endregion


        #region public Members 

        public override void PointAtuoAdaptation()
        {
            if (!this.VerifyDoubleArrayExistNaN(pointRelativeLocation))
            {
                return;
            }

            IsGetPointRelativeLocation = false;
            X1 = pointRelativeLocation[0] * Width;
            Y1 = pointRelativeLocation[1] * Height;
            X2 = pointRelativeLocation[2] * Width;
            Y2 = pointRelativeLocation[3] * Height;
            IsGetPointRelativeLocation = true;
        }

        public override void GetPointRelativeLocation()
        {
            if (!this.VerifyPoint())
            {
                return;
            }

            pointRelativeLocation[0] = X1 / Width;
            pointRelativeLocation[1] = Y1 / Height;
            pointRelativeLocation[2] = X2 / Width;
            pointRelativeLocation[3] = Y2 / Height;
        }

        public void GetGeometry()
        {
            //PathFigure myPathFigure = new PathFigure();
            //myPathFigure.StartPoint = new Point(10, 50);
            //myPathFigure.Segments.Add(
            //    new BezierSegment(
            //        new Point(100, 0),
            //        new Point(200, 200),
            //        new Point(300, 100),
            //        true /* IsStroked */  ));
            //myPathFigure.Segments.Add(
            //    new LineSegment(
            //        new Point(400, 100),
            //        true /* IsStroked */ ));
            //myPathFigure.Segments.Add(
            //    new ArcSegment(
            //        new Point(200, 100),
            //        new Size(50, 50),
            //        45,
            //        true, /* IsLargeArc */
            //        SweepDirection.Clockwise,
            //        true /* IsStroked */ ));

            ///// Create a PathGeometry to contain the figure.
            //PathGeometry myPathGeometry = new PathGeometry();
            //myPathGeometry.Figures.Add(myPathFigure);

            geometry = this.VerifyPoint() ? new LineGeometry(new Point(X1, Y1), new Point(X2, Y2)) : Geometry.Empty;
        }

        #endregion
    }
}
