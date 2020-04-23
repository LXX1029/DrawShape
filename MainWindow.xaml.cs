using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Canvas画线2
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }
        /// <summary>
        /// 起始位置
        /// </summary>
        Point startPoint;
        /// <summary>
        /// 点集合
        /// </summary>
        List<Point> pointList = new List<Point>();

        /// <summary>
        /// 选择的颜色
        /// </summary>
        public string selectedColor { get; set; }
        /// <summary>
        /// 要画的线
        /// </summary>
        Polyline line;
        /// <summary>
        /// 鼠标左键按下获取开始Point
        /// </summary>
        private void Canvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(myCanvas);
            // 每次鼠标按下 创建新的Polyline对象，并添加到Canvas中
            line = new Polyline();
            myCanvas.Children.Add(line);
        }

        /// <summary>
        /// 鼠标左键释放清空pointList
        /// </summary>
        private void myCanvas_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            pointList.Clear();
        }

        /// <summary>
        /// 按下鼠标左键移动
        /// </summary>
        private void Canvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // 返回指针相对于Canvas的位置
                Point point = e.GetPosition(myCanvas);
                if (pointList.Count == 0)
                {
                    // 加入起始点
                    pointList.Add(new Point(this.startPoint.X, this.startPoint.Y));
                }
                else
                {
                    // 加入移动过程中的point
                    pointList.Add(point);
                }
                var count = pointList.Count(); // 总点数
                if (point != this.startPoint && this.startPoint != null)
                {
                    if (selectedColor == "默认")
                    {
                        line.Stroke = Brushes.Black;
                    }
                    if (selectedColor == "红色")
                    {
                        line.Stroke = Brushes.Red;
                    }
                    if (selectedColor == "绿色")
                    {
                        line.Stroke = Brushes.Green;
                    }
                    line.StrokeThickness = Convert.ToInt32(cboThickness.Text);
                    line.SnapsToDevicePixels = true;

                    if (count < 2)
                        return;

                    Point fpoint = new Point(pointList[count - 2].X, pointList[count - 2].Y);
                    Point tpoint = new Point(point.X, point.Y);
                    line.Points.Add(fpoint);
                    line.Points.Add(tpoint);
                }
            }
        }

        /// <summary>
        /// 选择颜色
        /// </summary>
        private void cboColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedColor = (cboColor.SelectedItem as ComboBoxItem).Content as string;
        }

        /// <summary>
        /// 选择style
        /// </summary>
        private void cboStyle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string style = (cboStyle.SelectedItem as ComboBoxItem).Content as string;
            if (this.myCanvas == null)
            {
                return;
            }
            var list = GetChildObjects<Polyline>(this.myCanvas);
            if (list.Count > 0)
            {
                list.ForEach(l =>
                {
                    if (style == "默认")
                    {
                        l.StrokeDashArray = new DoubleCollection(new List<double>() { });
                    }
                    if (style == "虚线")
                    {
                        l.StrokeDashArray = new DoubleCollection(new List<double>() {
                     1,1,1,1
                    });
                    }
                });
                list.Clear();
            }
        }
        /// <summary>  
        /// 获得指定元素的所有子元素  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="obj"></param>  
        /// <returns></returns>  
        public List<T> GetChildObjects<T>(DependencyObject obj) where T : FrameworkElement
        {
            DependencyObject child = null;
            List<T> childList = new List<T>();

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);

                if (child is T)
                {
                    childList.Add((T)child);
                }
                childList.AddRange(GetChildObjects<T>(child));
            }
            return childList;
        }

        /// <summary>
        ///  导出成图片
        /// </summary>
        private void btnExportImg_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog()
            {
                FileName = "testbitmap",
                DefaultExt = ".bmp",
                Filter = "Windows Bitmap (.bmp)|*.bmp|Portable Network Graphics (.png)|*.png|JPEG (.jpg, .jpeg)|*.jpg;*.jpeg",
                AddExtension = true,
                OverwritePrompt = true,
                ValidateNames = true
            };

            if (dlg.ShowDialog() == true)
            {
                string ext = System.IO.Path.GetExtension(dlg.FileName);
                BitmapEncoder enc = null;
                switch (ext)
                {
                    case ".bmp":
                        enc = new BmpBitmapEncoder();
                        break;
                    case ".jpg":
                    case ".jpeg":
                        enc = new JpegBitmapEncoder();
                        break;
                    case ".png":
                        enc = new PngBitmapEncoder();
                        break;
                    default:
                        return;
                }

                if (enc != null)
                {
                    RenderTargetBitmap rtb;
                    if (CaptureToBitmap(myCanvas, PixelFormats.Pbgra32, out rtb))
                    {
                        BitmapFrame bmf = BitmapFrame.Create(rtb);
                        enc.Frames.Add(bmf);
                        using (Stream stm = File.Create(dlg.FileName))
                        {
                            enc.Save(stm);
                        }

                    }
                }
            }
            MessageBox.Show("导出成功");
        }

        public static bool CaptureToBitmap(Visual SourceVisual, PixelFormat PixelFormat, out RenderTargetBitmap RenderedBitmap)
        {
            Window rootWindow = Window.GetWindow(SourceVisual);
            bool hasWindow = rootWindow != null;
            if (hasWindow)
            {
                double[] dpi = GetApplicationDPI(rootWindow);

                Rect bounds = VisualTreeHelper.GetDescendantBounds(SourceVisual);
                RenderTargetBitmap rtb = new RenderTargetBitmap((int)(bounds.Width), (int)(bounds.Height), dpi[0], dpi[1], PixelFormat);

                DrawingVisual dv = new DrawingVisual();
                using (DrawingContext ctx = dv.RenderOpen())
                {
                    VisualBrush vb = new VisualBrush(SourceVisual);
                    ctx.PushOpacityMask(Brushes.White);
                    ctx.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
                }
                
                try
                {
                    rtb.Render(dv);
                }
                catch (Exception ex)
                {
                    foreach (System.Collections.DictionaryEntry de in ex.Data)
                    {
                        object a = de.Key;
                        object b = de.Value;
                    }
                }
                RenderedBitmap = rtb;
            }
            else
                RenderedBitmap = null;
            return hasWindow;
        }
        /// <summary>
        /// To return the application DPI (should be 96 anyway, but just in case)
        /// </summary>
        public static double[] GetApplicationDPI(Window w)
        {
            Matrix m = PresentationSource.FromVisual(w).CompositionTarget.TransformToDevice;
            return new double[] { m.M11 * 96, m.M22 * 96 };
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            myCanvas.Children.Clear();
        }
    }
}
