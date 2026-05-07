using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WpfPoint = System.Windows.Point;

namespace OcrTool.Services
{
    public partial class CaptureForm : Window
    {
        private WpfPoint _startPoint;
        private bool _isSelecting;
        private readonly Bitmap _originalBitmap;

        public Rectangle? SelectedArea { get; private set; }

        public CaptureForm(Bitmap bitmap)
        {
            InitializeComponent();
            _originalBitmap = bitmap;
            Loaded += CaptureForm_Loaded;
        }

        private void CaptureForm_Loaded(object sender, RoutedEventArgs e)
        {
            Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            Height = System.Windows.SystemParameters.PrimaryScreenHeight;
            Left = 0;
            Top = 0;

            ScreenImage.Source = ConvertBitmapToBitmapImage(_originalBitmap);
        }

        private BitmapImage ConvertBitmapToBitmapImage(Bitmap bitmap)
        {
            using (var memoryStream = new System.IO.MemoryStream())
            {
                bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                memoryStream.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                _startPoint = e.GetPosition(this);
                _isSelecting = true;
                
                SelectionRect.Visibility = Visibility.Visible;
                ShowBorders(true);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (_isSelecting)
            {
                WpfPoint currentPoint = e.GetPosition(this);
                
                double x = Math.Min(_startPoint.X, currentPoint.X);
                double y = Math.Min(_startPoint.Y, currentPoint.Y);
                double width = Math.Abs(currentPoint.X - _startPoint.X);
                double height = Math.Abs(currentPoint.Y - _startPoint.Y);

                SelectionRect.Margin = new Thickness(x, y, 0, 0);
                SelectionRect.Width = width;
                SelectionRect.Height = height;

                UpdateBorders(x, y, width, height);
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            if (_isSelecting && e.LeftButton == MouseButtonState.Released)
            {
                _isSelecting = false;
                
                if (SelectionRect.Width > 10 && SelectionRect.Height > 10)
                {
                    SelectedArea = new Rectangle(
                        (int)SelectionRect.Margin.Left,
                        (int)SelectionRect.Margin.Top,
                        (int)SelectionRect.Width,
                        (int)SelectionRect.Height
                    );
                    DialogResult = true;
                    Close();
                }
                else
                {
                    SelectionRect.Visibility = Visibility.Hidden;
                    ShowBorders(false);
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == Key.Escape)
            {
                SelectedArea = null;
                DialogResult = false;
                Close();
            }
            else if (e.Key == Key.Enter && SelectionRect.Visibility == Visibility.Visible)
            {
                if (SelectionRect.Width > 10 && SelectionRect.Height > 10)
                {
                    SelectedArea = new Rectangle(
                        (int)SelectionRect.Margin.Left,
                        (int)SelectionRect.Margin.Top,
                        (int)SelectionRect.Width,
                        (int)SelectionRect.Height
                    );
                    DialogResult = true;
                    Close();
                }
            }
        }

        private void ShowBorders(bool show)
        {
            TopBorder.Visibility = show ? Visibility.Visible : Visibility.Hidden;
            BottomBorder.Visibility = show ? Visibility.Visible : Visibility.Hidden;
            LeftBorder.Visibility = show ? Visibility.Visible : Visibility.Hidden;
            RightBorder.Visibility = show ? Visibility.Visible : Visibility.Hidden;
            TopLeftCorner.Visibility = show ? Visibility.Visible : Visibility.Hidden;
            TopRightCorner.Visibility = show ? Visibility.Visible : Visibility.Hidden;
            BottomLeftCorner.Visibility = show ? Visibility.Visible : Visibility.Hidden;
            BottomRightCorner.Visibility = show ? Visibility.Visible : Visibility.Hidden;
        }

        private void UpdateBorders(double x, double y, double width, double height)
        {
            TopBorder.Margin = new Thickness(x, y, 0, 0);
            TopBorder.Width = width;

            BottomBorder.Margin = new Thickness(x, y + height, 0, 0);
            BottomBorder.Width = width;

            LeftBorder.Margin = new Thickness(x, y, 0, 0);
            LeftBorder.Height = height;

            RightBorder.Margin = new Thickness(x + width, y, 0, 0);
            RightBorder.Height = height;

            TopLeftCorner.Margin = new Thickness(x - 3, y - 3, 0, 0);
            TopRightCorner.Margin = new Thickness(x + width - 3, y - 3, 0, 0);
            BottomLeftCorner.Margin = new Thickness(x - 3, y + height - 3, 0, 0);
            BottomRightCorner.Margin = new Thickness(x + width - 3, y + height - 3, 0, 0);
        }
    }
}