using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Media.Imaging;

namespace OcrTool.Services
{
    public class ScreenCapture
    {
        public BitmapImage Capture()
        {
            //获取屏幕尺寸
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;

            using (var bitmap = new Bitmap((int)screenWidth, (int)screenHeight))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
                }

                var captureForm = new CaptureForm(bitmap);
                captureForm.ShowDialog();

                if (captureForm.SelectedArea.HasValue)
                {
                    using (var croppedBitmap = bitmap.Clone(captureForm.SelectedArea.Value, bitmap.PixelFormat))
                    {
                        return ConvertToBitmapImage(croppedBitmap);
                    }
                }
            }

            return null;
        }

        private BitmapImage ConvertToBitmapImage(Bitmap bitmap)
        {
            using (var memoryStream = new System.IO.MemoryStream())
            {
                bitmap.Save(memoryStream, ImageFormat.Png);// 保存为PNG到内存流
                memoryStream.Position = 0;// 重置流位置

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream; // 从内存流加载
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;// 立即加载到内存
                bitmapImage.EndInit();
                bitmapImage.Freeze(); // 冻结以便跨线程使用

                return bitmapImage;
            }
        }
    }
}