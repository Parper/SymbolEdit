using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Printing.IndexedProperties;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SymbolEdit.MyElemnent
{
    public class MoveElementPointShow : MyElemnentBase
    {
        #region Constructors

        public MoveElementPointShow()
        {
            Points = new();
        }

        #endregion

        #region Dependent attribute

        public List<Point> Points
        {
            get { return (List<Point>)GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Points.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register("Points", typeof(List<Point>), typeof(MoveElementPointShow), new PropertyMetadata(null, new PropertyChangedCallback(RefreshMoveElementPointShowDraw)));

        public bool IsVisibility
        {
            get { return (bool)GetValue(IsVisibilityProperty); }
            set { SetValue(IsVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsVisibilityProperty =
            DependencyProperty.Register("IsVisibility", typeof(bool), typeof(MoveElementPointShow), new PropertyMetadata(false, new PropertyChangedCallback(RefreshMoveElementPointShowDraw)));

        public double EllipseSize
        {
            get { return (double)GetValue(EllipseSizeProperty); }
            set { SetValue(EllipseSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RectSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EllipseSizeProperty =
            DependencyProperty.Register("EllipseSize", typeof(double), typeof(MoveElementPointShow), new PropertyMetadata(3.0, new PropertyChangedCallback(RefreshMoveElementPointShowDraw)));


        private static void RefreshMoveElementPointShowDraw(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MoveElementPointShow moveElementPointShow)
            {
                if (moveElementPointShow.Points.Count != moveElementPointShow.PointRelativeLocation.Length)
                {
                    moveElementPointShow.SetPointRelativeLocation(moveElementPointShow.Points.Count);
                }
                RefreshElemnentDraw(d, e);
            }
        }


        #endregion

        public override void GetPointRelativeLocation()
        {
            if (!this.VerifyPoint())
            {
                return;
            }

            for (int i = 0; i < Points.Count; i++)
            {
                PointRelativeLocation[i].X = Points[i].X / Width;
                PointRelativeLocation[i].Y = Points[i].Y / Height;
            }
        }

        public override void PointAtuoAdaptation()
        {
            if (!this.VerifyRelativePointArrayExistNaN(PointRelativeLocation))
            {
                return;
            }

            IsGetPointRelativeLocation = false;
            for (int i = 0; i < PointRelativeLocation.Length; i++)
            {
                var point = Points[i];
                point.X = PointRelativeLocation[i].X * Width;
                point.Y = PointRelativeLocation[i].Y * Height;
            }
            IsGetPointRelativeLocation = true;
        }

        public override bool VerifyPoint()
        {
            if (Points == null || Points.Count == 0)
            {
                return false;
            }

            foreach (var point in Points)
            {
                if (double.IsNaN(point.X) || double.IsNaN(point.Y))
                {
                    return false;
                }
            }

            return true;
        }


        protected override void OnRender(DrawingContext drawingContext)
        {
            if (!this.VerifyPoint() || !IsVisibility)
            {
                return;
            }

            for (int i = 0; i < Points.Count; i++)
            {
                drawingContext.DrawEllipse(Stroke, new Pen(Stroke, StrokeThickness), new Point(Points[i].X, Points[i].Y), EllipseSize, EllipseSize);
            }
        }

    }
}
