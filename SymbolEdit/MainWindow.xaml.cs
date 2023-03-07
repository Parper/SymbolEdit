using SymbolEdit.MyElemnent;
using SymbolEdit.SelectableElement;
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
            myElemnentBases.Add(moveElementPointShow);
            myElemnentBases.Add(line);
            myElemnentBases.Add(myTriangle);
            myElemnentBases.Add(myText);
        }

        private bool IsMoveElementPoint = false;

        private List<MyElemnentBase> myElemnentBases = new List<MyElemnentBase>();

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox combo && gridLine != null)
            {
                gridLine.GridLineStyle = (GridLineStyle)combo.SelectedIndex;
            }
        }

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
                if (moveElementPointShow.IsVisibility && ordMyElemnent is IMoveElementPoint moveElement)
                {
                    moveElementPointShow.Points = moveElement.GetSelectablePoints();
                    OperationParam operationParam = new()
                    {
                        Left = Canvas.GetLeft(myElemnent),
                        Top = Canvas.GetTop(myElemnent),
                        Width = myElemnent.Width,
                        Height = myElemnent.Height,
                    };
                    moveElementPointShow.SetLocationAndSize(operationParam);
                }
                return;
            }
        }


        private int selectedIndex = -1;
        private MyElemnentBase? ordMyElemnent;
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (IsMoveElementPoint && ordMyElemnent is IMoveElementPoint moveElement)
                {
                    var isSelected = moveElement.IsSelectedPoint(e.GetPosition(ordMyElemnent), moveElementPointShow.EllipseSize);
                    if (isSelected.Item1)
                    {
                        selectedIndex = isSelected.Item2;
                    }
                    e.Handled = true;
                }
                else if (e.Source is MyElemnentBase myElemnent)
                {
                    borderLine.LeftTopX = Canvas.GetLeft(myElemnent);
                    borderLine.LeftTopY = Canvas.GetTop(myElemnent);
                    borderLine.RightBottomX = borderLine.LeftTopX + myElemnent.Width;
                    borderLine.RightBottomY = borderLine.LeftTopY + myElemnent.Height;
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

            SetMoveElementPointStart();
        }

        private void Canvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (ordMyElemnent is MyElemnentBase myElemnent && ordMyElemnent is IMoveElementPoint moveElement && selectedIndex != -1)
            {
                var point = e.GetPosition(ordMyElemnent);
                var operation = myElemnent.GetLocationAndSize();
                if (borderLine.PointIsInsideRectangle(new Point(operation.Left + point.X, operation.Top + point.Y)))
                {
                    moveElement.MoveElementPoint(selectedIndex, point);
                    moveElementPointShow.Points = moveElement.GetSelectablePoints();
                }
            }
        }

        private void Canvas_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (selectedIndex != -1)
            {
                selectedIndex = -1;
                e.Handled = true;
            }
        }

        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F4)
            {

            }
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

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox check)
            {
                IsMoveElementPoint = (bool)check.IsChecked!;
            }
            SetMoveElementPointStart();
        }

        private void SetMoveElementPointStart()
        {
            if (IsMoveElementPoint &&
                ordMyElemnent is MyElemnentBase myElemnent &&
                ordMyElemnent is IMoveElementPoint moveElementPoint &&
                moveElementPoint.GetSelectablePoints().Any())
            {
                moveElementPointShow.Points = moveElementPoint.GetSelectablePoints();
                OperationParam operationParam = new()
                {
                    Left = Canvas.GetLeft(myElemnent),
                    Top = Canvas.GetTop(myElemnent),
                    Width = myElemnent.Width,
                    Height = myElemnent.Height,
                };
                moveElementPointShow.SetLocationAndSize(operationParam);
                moveElementPointShow.IsVisibility = true;
            }
            else
            {
                // 显示点元素隐藏
                moveElementPointShow.IsVisibility = false;
                checkBox.IsChecked = false;
                IsMoveElementPoint = false;
            }
        }

    }
}
