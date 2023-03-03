using SymbolEdit.MyElemnent;
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

namespace SymbolEdit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.canvas.SizeChanged += MainWindow_SizeChanged;
            borderLine.OperationElement += MoveElement;
            borderLine.GetNearestPoint = GetNearestPoint;
            borderLine.SetCurElement(canvas);
            myElemnentBases.Add(line);
            myElemnentBases.Add(myTriangle);
            myElemnentBases.Add(myText);
        }

        private List<MyElemnentBase> myElemnentBases = new List<MyElemnentBase>();

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var width = e.NewSize.Width;
            var height = e.NewSize.Height;
            gridLine.Width = width;
            gridLine.Height = height;
            myElemnentBases.ForEach(x => x.SetRectRelativeLocation(width, height));
            if (ordMyElemnent is MyElemnentBase myElemnent)
            {
                var operation = myElemnent.GetLocationAndSize();
                borderLine.LeftTopX = operation.Left;
                borderLine.LeftTopY = operation.Top;
                borderLine.RightBottomX = operation.Left + operation.Width;
                borderLine.RightBottomY = operation.Top + operation.Height;
                return;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox combo && gridLine != null)
            {
                gridLine.GridLineStyle = (GridLineStyle)combo.SelectedIndex;
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (gridLine != null && (isLeftDown || isRightDown))
            {
                var ePoint = e.GetPosition(canvas);
                var point = gridLine.GetNearestPoint(ePoint);
                if (point.X == double.NaN || point.Y == double.NaN)
                {
                    return;
                }
                if (isLeftDown)
                {
                    line.X1 = point.X;
                    line.Y1 = point.Y;
                }
                else if (isRightDown)
                {
                    line.X2 = point.X;
                    line.Y2 = point.Y;
                }

            }
        }

        private bool isLeftDown = false;
        private bool isRightDown = false;
        private MyElemnentBase? ordMyElemnent;
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (e.Source is MyElemnentBase myElemnent)
                {
                    var x1 = Canvas.GetLeft(myElemnent);
                    var y1 = Canvas.GetTop(myElemnent);
                    borderLine.LeftTopX = x1;
                    borderLine.LeftTopY = y1;
                    borderLine.RightBottomX = x1 + myElemnent.Width;
                    borderLine.RightBottomY = y1 + myElemnent.Height;
                    borderLine.IsVisibility = true;
                    ordMyElemnent = myElemnent;
                }
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                if (ordMyElemnent != null)
                {
                    ordMyElemnent = null;
                }
                borderLine.IsVisibility = false;
            }

            //if (e.ButtonState == MouseButtonState.Pressed)
            //{
            //    isLeftDown = e.ChangedButton == MouseButton.Left;
            //    isRightDown = e.ChangedButton == MouseButton.Right;
            //}
        }

        private void MoveElement(OperationParam operationParam)
        {
            if (ordMyElemnent != null)
            {
                if (ordMyElemnent is MyElemnentBase myElemnent)
                {
                    myElemnent.SetLocationAndSize(operationParam);
                    return;
                }

                Canvas.SetLeft(ordMyElemnent, operationParam.Left);
                Canvas.SetTop(ordMyElemnent, operationParam.Top);
                ordMyElemnent.Width = operationParam.Width;
                ordMyElemnent.Height = operationParam.Height;
            }
        }

        private Point GetNearestPoint(Point point)
        {
            return gridLine.GetNearestPoint(point);
        }
    }
}
