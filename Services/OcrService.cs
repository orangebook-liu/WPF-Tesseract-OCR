using System;
using System.IO;
using System.Windows.Media.Imaging;
using Tesseract;

namespace OcrTool.Services
{
    public interface IOcrService
    {
        string RecognizeText(string imagePath);
        string RecognizeText(BitmapImage image);
    }

    public class OcrService : IOcrService
    {
        private readonly string _tessdataPath;

        public OcrService()
        {
            _tessdataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata");
            EnsureTessdataExists();
        }

        private void EnsureTessdataExists()
        {
            if (!Directory.Exists(_tessdataPath))
            {
                Directory.CreateDirectory(_tessdataPath);
            }
            
            CopyTessdataIfNotExists("chi_sim.traineddata");
            CopyTessdataIfNotExists("eng.traineddata");
        }

        private void CopyTessdataIfNotExists(string fileName)
        {
            string destPath = Path.Combine(_tessdataPath, fileName);
            if (!File.Exists(destPath))
            {
                using (var resourceStream = typeof(OcrService).Assembly.GetManifestResourceStream($"OcrTool.Resources.tessdata.{fileName}"))
                {
                    if (resourceStream != null)
                    {
                        using (var fileStream = new FileStream(destPath, FileMode.Create))
                        {
                            resourceStream.CopyTo(fileStream);
                        }
                    }
                }
            }
        }

        public string RecognizeText(string imagePath)
        {
            try
            {
                using (var engine = new TesseractEngine(_tessdataPath, "chi_sim+eng", EngineMode.Default))
                {
                    using (var pix = Pix.LoadFromFile(imagePath))
                    {
                        using (var page = engine.Process(pix))
                        {
                            return page.GetText();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return $"识别失败: {ex.Message}";
            }
        }

        public string RecognizeText(BitmapImage image)
        {
            try
            {
                using (var engine = new TesseractEngine(_tessdataPath, "chi_sim+eng", EngineMode.Default))
                {
                    using (var pix = ConvertBitmapImageToPix(image))
                    {
                        using (var page = engine.Process(pix))
                        {
                            return page.GetText();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return $"识别失败: {ex.Message}";
            }
        }

        private Pix ConvertBitmapImageToPix(BitmapImage bitmapImage)
        {
            using (var memoryStream = new MemoryStream())
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                encoder.Save(memoryStream);
                memoryStream.Position = 0;
                return Pix.LoadFromMemory(memoryStream.ToArray());
            }
        }
    }
}