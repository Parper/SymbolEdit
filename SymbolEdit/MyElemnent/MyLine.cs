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
        #region Constructors

        public MyLine() : base(2)
        {

        }

        #endregion

        #region Dependent attribute

        public double X1
        {
            get { return (double)GetValue(X1Property); }
            set { SetValue(X1Property, value); }
        }

        // Using a DependencyProperty as the backing store for X1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty X1Property =
            DependencyProperty.Register("X1", typeof(double), typeof(MyLine), new PropertyMetadata(double.NaN, new PropertyChangedCallback(RefreshElemnentDraw)));

        public double Y1
        {
            get { return (double)GetValue(Y1Property); }
            set { SetValue(Y1Property, value); }
        }

        // Using a DependencyProperty as the backing store for Y1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Y1Property =
            DependencyProperty.Register("Y1", typeof(double), typeof(MyLine), new PropertyMetadata(double.NaN, new PropertyChangedCallback(RefreshElemnentDraw)));

        public double X2
        {
            get { return (double)GetValue(X2Property); }
            set { SetValue(X2Property, value); }
        }

        // Using a DependencyProperty as the backing store for X2.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty X2Property =
            DependencyProperty.Register("X2", typeof(double), typeof(MyLine), new PropertyMetadata(double.NaN, new PropertyChangedCallback(RefreshElemnentDraw)));

        public double Y2
        {
            get { return (double)GetValue(Y2Property); }
            set { SetValue(Y2Property, value); }
        }

        // Using a DependencyProperty as the backing store for Y2.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Y2Property =
            DependencyProperty.Register("Y2", typeof(double), typeof(MyLine), new PropertyMetadata(double.NaN, new PropertyChangedCallback(RefreshElemnentDraw)));


        #endregion

        #region Private  Methods and Properties


        #endregion

        #region Protected Members 


        #endregion

        #region public Members 

        /// <summary>
        /// 验证点.
        /// </summary>
        /// <returns></returns>
        public override bool VerifyPoint()
        {
            if (double.IsNaN(X1) || double.IsNaN(Y1) || double.IsNaN(X2) || double.IsNaN(Y2))
            {
                return false;
            }

            return true;
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
            IsGetPointRelativeLocation = true;
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
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (!this.VerifyPoint())
            {
                return;
            }

            drawingContext.DrawLine(new Pen(Stroke, StrokeThickness), new Point(X1, Y1), new Point(X2, Y2));
        }

        #endregion
    }
}
