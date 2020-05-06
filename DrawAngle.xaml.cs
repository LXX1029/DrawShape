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
using System.Windows.Shapes;

namespace Canvas画线2
{
    /// <summary>
    /// DrawAngle.xaml 的交互逻辑
    /// </summary>
    public partial class DrawAngle : Window
    {
        public DrawAngle()
        {
            InitializeComponent();
        }
        private Point startPoint = new Point();
        private List<Point> points = new List<Point>();
        private List<Line> lines = new List<Line>();
        private List<Button> btns = new List<Button>();

        private Ellipse ell = new Ellipse();
        private Line line = new Line();

        #region Canvas面板操作
        private void canvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource.GetType() != typeof(Canvas))
                return;
            startPoint = e.GetPosition(this.canvas);
            this.canvas.CaptureMouse();
            if ((bool)rdioAngle.IsChecked)
            {
                if (this.points.Count == 0 && this.lines.Count == 0 && this.btns.Count == 0)
                {
                    points.Add(startPoint);
                    var firstLine = new Line();
                    var secondLine = new Line();
                    var txt = new TextBlock();
                    var btnBegin = new Button() { Name = "btnBegin", Visibility = Visibility.Collapsed };
                    btnBegin.PreviewMouseDown -= BtnBegin_PreviewMouseDown;
                    btnBegin.PreviewMouseMove -= BtnBegin_PreviewMouseMove;
                    btnBegin.PreviewMouseUp -= BtnBegin_PreviewMouseUp;
                    btnBegin.PreviewMouseDown += BtnBegin_PreviewMouseDown;
                    btnBegin.PreviewMouseMove += BtnBegin_PreviewMouseMove;
                    btnBegin.PreviewMouseUp += BtnBegin_PreviewMouseUp;
                    btnBegin.Tag = new Tuple<Line, Line, TextBlock>(firstLine, secondLine, txt);
                    var btnMid = new Button() { Name = "btnMid", Visibility = Visibility.Collapsed };
                    btnMid.PreviewMouseDown -= BtnBegin_PreviewMouseDown;
                    btnMid.PreviewMouseMove -= BtnBegin_PreviewMouseMove;
                    btnMid.PreviewMouseUp -= BtnBegin_PreviewMouseUp;
                    btnMid.PreviewMouseDown += BtnBegin_PreviewMouseDown;
                    btnMid.PreviewMouseMove += BtnBegin_PreviewMouseMove;
                    btnMid.PreviewMouseUp += BtnBegin_PreviewMouseUp;

                    btnMid.Tag = new Tuple<Line, Line, TextBlock>(firstLine, secondLine, txt);
                    var btnEnd = new Button() { Name = "btnEnd", Visibility = Visibility.Collapsed };
                    btnEnd.PreviewMouseDown -= BtnBegin_PreviewMouseDown;
                    btnEnd.PreviewMouseMove -= BtnBegin_PreviewMouseMove;
                    btnEnd.PreviewMouseUp -= BtnBegin_PreviewMouseUp;
                    btnEnd.PreviewMouseDown += BtnBegin_PreviewMouseDown;
                    btnEnd.PreviewMouseMove += BtnBegin_PreviewMouseMove;
                    btnEnd.PreviewMouseUp += BtnBegin_PreviewMouseUp;
                    btnEnd.Tag = new Tuple<Line, Line, TextBlock>(firstLine, secondLine, txt);
                    this.canvas.Children.Add(btnBegin);
                    this.canvas.Children.Add(btnMid);
                    this.canvas.Children.Add(btnEnd);
                    this.canvas.Children.Add(firstLine);
                    this.canvas.Children.Add(secondLine);
                    this.canvas.Children.Add(txt);
                    this.lines.Add(firstLine);
                    this.lines.Add(secondLine);
                    this.btns.Add(btnBegin);
                    this.btns.Add(btnMid);
                    this.btns.Add(btnEnd);
                }
                else
                {
                    this.points.Clear();
                    this.lines.Clear();
                    this.btns.Clear();
                    this.canvas.ReleaseMouseCapture();
                }
                return;

            }
            if ((bool)rdioLine.IsChecked)
            {
                line = new Line();
                line.Stroke = Brushes.Red;
                line.StrokeThickness = 1;
                line.X1 = startPoint.X;
                line.Y1 = startPoint.Y;
                line.X2 = startPoint.X;
                line.Y2 = startPoint.Y;
                //Canvas.SetLeft(line, point.X);
                //Canvas.SetTop(line, point.Y);
                this.canvas.Children.Add(line);
                return;
            }
            if ((bool)rdioEllipse.IsChecked)
            {
                ell = new Ellipse();
                ell.Stroke = Brushes.Red;
                ell.StrokeThickness = 1;
                Canvas.SetLeft(ell, startPoint.X);
                Canvas.SetTop(ell, startPoint.Y);
                this.canvas.Children.Add(ell);
                return;

            }

        }

        private void canvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.OriginalSource.GetType() != typeof(Canvas))
                return;
            if (!this.canvas.IsMouseCaptured)
                return;
            var currentPoint = e.GetPosition(this.canvas);
            var renderSize = this.canvas.RenderSize;
            if (currentPoint.X < 0)
                currentPoint.X = 5;
            if (currentPoint.Y < 0)
                currentPoint.Y = 5;
            if (currentPoint.X > renderSize.Width)
                currentPoint.X = renderSize.Width - 5;
            if (currentPoint.Y > renderSize.Height)
                currentPoint.Y = renderSize.Height - 5;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if ((bool)rdioEllipse.IsChecked)
                {
                    var width = Math.Abs(currentPoint.X - startPoint.X);
                    var height = Math.Abs(currentPoint.Y - startPoint.Y);
                    ell.Width = width;
                    ell.Height = height;
                }
                if ((bool)rdioLine.IsChecked)
                {
                    line.X2 = currentPoint.X;
                    line.Y2 = currentPoint.Y;
                }




            }
            if ((bool)rdioAngle.IsChecked)
            {
                if (this.canvas.IsMouseCaptured == false)
                    return;
                if (e.LeftButton == MouseButtonState.Pressed)
                {

                    btns[0].SetValue(Canvas.TopProperty, startPoint.Y - 5);
                    btns[0].SetValue(Canvas.LeftProperty, startPoint.X - 5);
                    btns[0].Visibility = Visibility.Visible;
                    this.lines[0].X1 = startPoint.X;
                    this.lines[0].Y1 = startPoint.Y;
                    this.lines[0].X2 = currentPoint.X;
                    this.lines[0].Y2 = currentPoint.Y;
                    btns[1].SetValue(Canvas.TopProperty, currentPoint.Y - 5);
                    btns[1].SetValue(Canvas.LeftProperty, currentPoint.X - 5);
                    btns[1].Visibility = Visibility.Visible;
                }
                else
                {
                    var midPoint = new Point() { X = this.lines[0].X2, Y = this.lines[0].Y2 };// this.points.LastOrDefault();
                    this.lines[1].X1 = midPoint.X;
                    this.lines[1].Y1 = midPoint.Y;
                    this.lines[1].X2 = currentPoint.X;
                    this.lines[1].Y2 = currentPoint.Y;
                    btns[2].SetValue(Canvas.TopProperty, currentPoint.Y - 5);
                    btns[2].SetValue(Canvas.LeftProperty, currentPoint.X - 5);
                    btns[2].Visibility = Visibility.Visible;

                    var tuple = (Tuple<Line, Line, TextBlock>)btns[1].Tag;
                    var angle = this.Angle(midPoint, new Point() { X = tuple.Item1.X1, Y = tuple.Item1.Y1 }, currentPoint);
                    SetAnglePosition(tuple.Item3, midPoint, angle);
                }
            }
        }

        private void canvas_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource.GetType() != typeof(Canvas))
                return;
            var currentPoint = e.GetPosition(this.canvas);

            if ((bool)this.rdioEllipse.IsChecked || (bool)this.rdioLine.IsChecked)
            {
                this.canvas.ReleaseMouseCapture();
            }
        }

        private void SetAnglePosition(TextBlock tb, Point locationPoint, double angle = 0.0)
        {
            if (tb == null) return;
            tb.SetValue(Canvas.TopProperty, locationPoint.Y + 10);
            tb.SetValue(Canvas.LeftProperty, locationPoint.X + 10);
            tb.Text = angle.ToString("0.0");
        }
        public double Angle(Point cen, Point first, Point second)
        {
            const double M_PI = 3.1415926535897;

            double ma_x = first.X - cen.X;
            double ma_y = first.Y - cen.Y;
            double mb_x = second.X - cen.X;
            double mb_y = second.Y - cen.Y;
            double v1 = (ma_x * mb_x) + (ma_y * mb_y);
            double ma_val = Math.Sqrt(ma_x * ma_x + ma_y * ma_y);
            double mb_val = Math.Sqrt(mb_x * mb_x + mb_y * mb_y);
            double cosM = v1 / (ma_val * mb_val);
            double angleAMB = Math.Acos(cosM) * 180 / M_PI;

            return angleAMB;
        }
        #endregion Canvas面板操作

        #region Canvas按钮移动拉伸
        private void BtnBegin_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if ((bool)rdioAngle.IsChecked)
            {
                if (sender is Button btn)
                {
                    btn.ReleaseMouseCapture();
                }
            }

        }

        private void BtnBegin_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((bool)rdioAngle.IsChecked)
            {
                if (sender is Button btn)
                    btn.CaptureMouse();
            }
        }

        private void BtnBegin_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if ((bool)rdioAngle.IsChecked)
            {
                if (sender is Button btn)
                {
                    if (btn.IsMouseCaptured == false)
                        return;
                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        var pt = e.GetPosition(this.canvas);
                        Canvas.SetLeft(btn, pt.X - 5);
                        Canvas.SetTop(btn, pt.Y - 5);
                        var tuple = (Tuple<Line, Line, TextBlock>)btn.Tag;
                        if (btn.Name == "btnBegin")
                        {
                            tuple.Item1.X1 = pt.X;
                            tuple.Item1.Y1 = pt.Y;
                        }
                        if (btn.Name == "btnMid")
                        {
                            tuple.Item1.X2 = pt.X;
                            tuple.Item1.Y2 = pt.Y;
                            tuple.Item2.X1 = pt.X;
                            tuple.Item2.Y1 = pt.Y;
                        }

                        if (btn.Name == "btnEnd")
                        {
                            tuple.Item2.X2 = pt.X;
                            tuple.Item2.Y2 = pt.Y;
                        }
                        var fpt = new Point { X = tuple.Item1.X1, Y = tuple.Item1.Y1 };
                        var cpt = new Point { X = tuple.Item1.X2, Y = tuple.Item1.Y2 };
                        var spt = new Point { X = tuple.Item2.X2, Y = tuple.Item2.Y2 };
                        var angle = this.Angle(cpt, fpt, spt);
                        tuple.Item3.Text = angle.ToString("0.0");
                        if (btn.Name == "btnMid")
                        {
                            SetAnglePosition(tuple.Item3, pt, angle);
                        }
                    }
                }
            }
        }
        #endregion Canvas按钮移动拉伸

        #region Grid面板
        private void gridRegion_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource.GetType() != typeof(Grid))
                return;
            startPoint = e.GetPosition(this.gridRegion);
            this.gridRegion.CaptureMouse();
            if (this.rdioBlowUp.IsChecked == false)
            {
                if (this.points.Count == 0 && this.lines.Count == 0 && this.btns.Count == 0)
                {
                    points.Add(startPoint);
                    var firstLine = new Line();
                    var secondLine = new Line();
                    firstLine.MouseEnter -= FirstLine_MouseEnter;
                    secondLine.MouseEnter -= FirstLine_MouseEnter;
                    firstLine.MouseEnter += FirstLine_MouseEnter;
                    secondLine.MouseEnter += FirstLine_MouseEnter;

                    firstLine.MouseLeave -= FirstLine_MouseLeave;
                    secondLine.MouseLeave -= FirstLine_MouseLeave;
                    firstLine.MouseLeave += FirstLine_MouseLeave;
                    secondLine.MouseLeave += FirstLine_MouseLeave;

                    var txt = new TextBlock() { Visibility = Visibility.Collapsed };
                    var btnBegin = new Button() { Name = "btnBegin", Visibility = Visibility.Collapsed };
                    btnBegin.PreviewMouseDown -= BtnBeginGrid_PreviewMouseDown;
                    btnBegin.PreviewMouseMove -= BtnBeginGrid_PreviewMouseMove;
                    btnBegin.PreviewMouseUp -= BtnBeginGrid_PreviewMouseUp;
                    btnBegin.PreviewMouseDown += BtnBeginGrid_PreviewMouseDown;
                    btnBegin.PreviewMouseMove += BtnBeginGrid_PreviewMouseMove;
                    btnBegin.PreviewMouseUp += BtnBeginGrid_PreviewMouseUp;
                    btnBegin.Tag = new Tuple<Line, Line, TextBlock>(firstLine, secondLine, txt);
                    var btnMid = new Button() { Name = "btnMid", Visibility = Visibility.Collapsed };
                    btnMid.PreviewMouseDown -= BtnBeginGrid_PreviewMouseDown;
                    btnMid.PreviewMouseMove -= BtnBeginGrid_PreviewMouseMove;
                    btnMid.PreviewMouseUp -= BtnBeginGrid_PreviewMouseUp;
                    btnMid.PreviewMouseDown += BtnBeginGrid_PreviewMouseDown;
                    btnMid.PreviewMouseMove += BtnBeginGrid_PreviewMouseMove;
                    btnMid.PreviewMouseUp += BtnBeginGrid_PreviewMouseUp;

                    btnMid.Tag = new Tuple<Line, Line, TextBlock>(firstLine, secondLine, txt);
                    var btnEnd = new Button() { Name = "btnEnd", Visibility = Visibility.Collapsed };
                    btnEnd.PreviewMouseDown -= BtnBeginGrid_PreviewMouseDown;
                    btnEnd.PreviewMouseMove -= BtnBeginGrid_PreviewMouseMove;
                    btnEnd.PreviewMouseUp -= BtnBeginGrid_PreviewMouseUp;
                    btnEnd.PreviewMouseDown += BtnBeginGrid_PreviewMouseDown;
                    btnEnd.PreviewMouseMove += BtnBeginGrid_PreviewMouseMove;
                    btnEnd.PreviewMouseUp += BtnBeginGrid_PreviewMouseUp;
                    btnEnd.Tag = new Tuple<Line, Line, TextBlock>(firstLine, secondLine, txt);
                    var gridOwner = new Grid();
                    //gridOwner.Background = Brushes.Purple;
                    //gridOwner.HorizontalAlignment = HorizontalAlignment.Center;
                    //gridOwner.VerticalAlignment = VerticalAlignment.Center;
                    gridOwner.Children.Add(btnBegin);
                    gridOwner.Children.Add(btnMid);
                    gridOwner.Children.Add(btnEnd);
                    gridOwner.Children.Add(firstLine);
                    gridOwner.Children.Add(secondLine);
                    gridOwner.Children.Add(txt);


                    this.gridRegion.Children.Add(gridOwner);
                    //this.gridRegion.Children.Add(btnBegin);
                    //this.gridRegion.Children.Add(btnMid);
                    //this.gridRegion.Children.Add(btnEnd);
                    //this.gridRegion.Children.Add(firstLine);
                    //this.gridRegion.Children.Add(secondLine);
                    //this.gridRegion.Children.Add(txt);


                    this.lines.Add(firstLine);
                    this.lines.Add(secondLine);
                    this.btns.Add(btnBegin);
                    this.btns.Add(btnMid);
                    this.btns.Add(btnEnd);
                }
                else
                {
                    this.points.Clear();
                    this.lines.Clear();
                    this.btns.Clear();
                    this.gridRegion.ReleaseMouseCapture();
                }
            }

        }



        private void FirstLine_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Line line)
            {
                var grid = (Grid)line.Parent;
                foreach (var item in grid.Children)
                {
                    if (item is TextBlock tb)
                        tb.Foreground = Brushes.White;
                    if (item is Line l)
                    {
                        l.Stroke = Brushes.Orange;
                        l.StrokeThickness = 1.0;
                    }
                }
                line.ReleaseMouseCapture();
            }
        }

        private void FirstLine_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Line line)
            {
                var grid = (Grid)line.Parent;
                foreach (var item in grid.Children)
                {
                    if (item is TextBlock tb)
                        tb.Foreground = Brushes.Red;
                    if (item is Line l)
                        l.Stroke = Brushes.Red;
                }
            }
        }


        private void gridRegion_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.OriginalSource.GetType() != typeof(Grid))
                return;
            if (!this.gridRegion.IsMouseCaptured)
                return;
            var size = this.gridRegion.RenderSize;
            var currentPoint = e.GetPosition(this.gridRegion);

            if (currentPoint.X < 0)
                currentPoint.X = 5;
            if (currentPoint.Y < 0)
                currentPoint.Y = 5;
            if (currentPoint.X > size.Width)
                currentPoint.X = size.Width - 5;
            if (currentPoint.Y > size.Height)
                currentPoint.Y = size.Height - 5;
            if (e.LeftButton == MouseButtonState.Pressed)
            {

                if (this.rdioAngle.IsChecked == true)
                {
                    btns[0].SetValue(Button.MarginProperty, new Thickness(startPoint.X - 5, startPoint.Y - 5, 0, 0));
                    btns[0].Visibility = Visibility.Visible;
                    this.lines[0].X1 = startPoint.X;
                    this.lines[0].Y1 = startPoint.Y;
                    this.lines[0].X2 = currentPoint.X;
                    this.lines[0].Y2 = currentPoint.Y;
                    btns[1].SetValue(Button.MarginProperty, new Thickness(currentPoint.X - 5, currentPoint.Y - 5, 0, 0));
                    btns[1].Visibility = Visibility.Visible;
                }
            }
            else
            {
                if (this.rdioBlowUp.IsChecked == true)
                {

                }
                if (this.rdioAngle.IsChecked == true)
                {
                    var midPoint = new Point() { X = this.lines[0].X2, Y = this.lines[0].Y2 };
                    this.lines[1].X1 = midPoint.X;
                    this.lines[1].Y1 = midPoint.Y;
                    this.lines[1].X2 = currentPoint.X;
                    this.lines[1].Y2 = currentPoint.Y;
                    btns[2].SetValue(Button.MarginProperty, new Thickness(currentPoint.X - 5, currentPoint.Y - 5, 0, 0));
                    btns[2].Visibility = Visibility.Visible;

                    var tuple = (Tuple<Line, Line, TextBlock>)btns[1].Tag;
                    var angle = this.Angle(midPoint, new Point() { X = tuple.Item1.X1, Y = tuple.Item1.Y1 }, currentPoint);
                    SetGridAnglePosition(tuple.Item3, midPoint, angle);
                }
            }


        }

        private void gridRegion_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this.rdioAngle.IsChecked == true)
            {
                if (this.lines.Count > 0 && this.lines[0].X1 == 0 && this.lines[0].Y1 == 0)
                {
                    this.points.Clear();
                    this.lines.Clear();
                    this.btns.Clear();
                    this.gridRegion.ReleaseMouseCapture();
                }
            }


        }

        private void SetGridAnglePosition(TextBlock tb, Point locationPoint, double angle = 0.0)
        {
            if (tb == null) return;
            if (tb.Visibility == Visibility.Collapsed) tb.Visibility = Visibility.Visible;
            tb.SetValue(TextBlock.MarginProperty, new Thickness(locationPoint.X + 10, locationPoint.Y + 10, 0, 0));
            tb.Text = angle.ToString("0.0");
        }



        private void BtnBeginGrid_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Button btn)
            {
                btn.ReleaseMouseCapture();
            }
        }


        private void BtnBeginGrid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Button btn)
                btn.CaptureMouse();
        }

        private void BtnBeginGrid_PreviewMouseMove(object sender, MouseEventArgs e)
        {

            if (sender is Button btn)
            {
                if (btn.IsMouseCaptured == false)
                    return;
                var pt = e.GetPosition(this.gridRegion);
                var size = this.gridRegion.RenderSize;
                if (pt.X < 0)
                    pt.X = 5;
                if (pt.Y < 0)
                    pt.Y = 5;
                if (pt.X > size.Width)
                    pt.X = size.Width - 5;
                if (pt.Y > size.Height)
                    pt.Y = size.Height - 5;

                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    btn.SetValue(Button.MarginProperty, new Thickness(pt.X - 5, pt.Y - 5, 0, 0));
                    var tuple = (Tuple<Line, Line, TextBlock>)btn.Tag;
                    if (btn.Name == "btnBegin")
                    {
                        tuple.Item1.X1 = pt.X;
                        tuple.Item1.Y1 = pt.Y;
                    }
                    if (btn.Name == "btnMid")
                    {
                        tuple.Item1.X2 = pt.X;
                        tuple.Item1.Y2 = pt.Y;
                        tuple.Item2.X1 = pt.X;
                        tuple.Item2.Y1 = pt.Y;
                    }

                    if (btn.Name == "btnEnd")
                    {
                        tuple.Item2.X2 = pt.X;
                        tuple.Item2.Y2 = pt.Y;
                    }
                    var fpt = new Point { X = tuple.Item1.X1, Y = tuple.Item1.Y1 };
                    var cpt = new Point { X = tuple.Item1.X2, Y = tuple.Item1.Y2 };
                    var spt = new Point { X = tuple.Item2.X2, Y = tuple.Item2.Y2 };
                    var angle = this.Angle(cpt, fpt, spt);
                    tuple.Item3.Text = angle.ToString("0.0");
                    if (btn.Name == "btnMid")
                    {
                        SetGridAnglePosition(tuple.Item3, pt, angle);
                    }
                }
            }
        }




        private void img_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource.GetType() != typeof(Image))
                return;
            if (this.rdioBlowUp.IsChecked == true)
            {
                startPoint = e.GetPosition(this.img);
                this.img.CaptureMouse();
                rect.Visibility = Visibility.Visible;
            }
        }

        private void img_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.OriginalSource.GetType() != typeof(Image))
                return;
            if (!this.img.IsMouseCaptured) return;
            if (this.rdioBlowUp.IsChecked == true)
            {
                var rectWidth = rect.Width;
                var rectHeight = rect.Height;
                var renderSize = this.img.RenderSize;
                var currentPoint = e.GetPosition(this.img);
                if (currentPoint.X > renderSize.Width)
                    currentPoint.X = renderSize.Width;
                if (currentPoint.X < rectWidth)
                    currentPoint.X = rectWidth;
                if (currentPoint.Y > renderSize.Height)
                    currentPoint.Y = renderSize.Height;
                if (currentPoint.Y < rectHeight)
                    currentPoint.Y = rectHeight;
                var viewBox = vb.Viewbox;
                viewBox.X = currentPoint.X - 100;
                viewBox.Y = currentPoint.Y - 100;
                vb.Viewbox = viewBox;
                rect.SetValue(Rectangle.MarginProperty, new Thickness(currentPoint.X - rect.Width, currentPoint.Y - rect.Height, 0, 0));
            }
        }

        private void img_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this.rdioBlowUp.IsChecked == true)
            {
                this.img.ReleaseMouseCapture();
            }
        }
        #endregion Grid面板

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var path = new Path();
            var geometry = new PathGeometry();
            path.Stroke = Brushes.Red;
            path.StrokeThickness = 2;
            path.SnapsToDevicePixels = true;
            //path.Fill = Brushes.Orange;
            var segment1 = new LineSegment(new Point(0, 11), true);
            var segment2 = new LineSegment(new Point(11, 11), true);
            var seg = new ArcSegment(new Point(6, 11), new Size(1, 1), 0, false, SweepDirection.Clockwise, true);

            var figure1 = new PathFigure(new Point(11, 0), new PathSegment[] { segment1, segment2 }, false);
            var figure2 = new PathFigure(new Point(0, 5), new PathSegment[] { seg }, false);
            geometry.Figures.Add(figure1);
            geometry.Figures.Add(figure2);
            path.Data = geometry;
            Canvas.SetTop(path, 100);
            Canvas.SetLeft(path, 100);
            this.canvas.Children.Add(path);
            string filePath = "D:\\1.png";

            var size = new Size(100, 100);
            //var rect = new Rectangle() { Width = 100, Height = 100, Fill = Brushes.Red, Stroke = Brushes.Orange, StrokeThickness = 1 };
            //rect.Measure(size);
            //rect.Arrange(new Rect(0, 0, size.Width, size.Height));
            ////var dv = new DrawingVisual();
            ////using (var ctx = dv.RenderOpen())
            ////{
            ////    var vb = new VisualBrush(grid);
            ////    ctx.DrawRectangle(vb, null, new Rect(0, 0, 100, 100));
            ////}
            //var render = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96, 96, PixelFormats.Pbgra32);
            //render.Render(rect);

            size = new Size(13, 13);
            path.Measure(size);
            path.Arrange(new Rect(0, 0, size.Width, size.Height));
            var render = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96, 96, PixelFormats.Pbgra32);
            render.Render(path);
            var frames = BitmapFrame.Create(render);
            var pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(frames);
            using (var stream = System.IO.File.Create(filePath))
            {
                pngEncoder.Save(stream);
            }
            MessageBox.Show("save successfully");
            System.Diagnostics.Process.Start(filePath);
        }
    }
}
