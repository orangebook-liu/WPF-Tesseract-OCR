using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OcrTool.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace OcrTool.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IOcrService _ocrService;

        [ObservableProperty]
        private BitmapImage _selectedImage;

        [ObservableProperty]
        private string _recognizedText = string.Empty;

        [ObservableProperty]
        private bool _isProcessing;

        [ObservableProperty]
        private bool _isDragOver;

        public IRelayCommand SelectImageCommand { get; }
        public IRelayCommand CaptureScreenCommand { get; }
        public IRelayCommand ClearCommand { get; }
        public IRelayCommand CopyTextCommand { get; }

        public MainViewModel(IOcrService ocrService)
        {
            _ocrService = ocrService;
            SelectImageCommand = new RelayCommand(SelectImage);
            CaptureScreenCommand = new RelayCommand(CaptureScreen);
            ClearCommand = new RelayCommand(ClearAll);
            CopyTextCommand = new RelayCommand(CopyToClipboard);
        }

        private void SelectImage()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "图片文件 (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp|所有文件 (*.*)|*.*",
                Multiselect = false,
                Title = "选择图片"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ProcessImage(openFileDialog.FileName);
            }
        }

        private void CaptureScreen()
        {
            try
            {
                var screenCapture = new ScreenCapture();
                var capturedImage = screenCapture.Capture();
                
                if (capturedImage != null)
                {
                    SelectedImage = capturedImage;
                    RecognizeFromImage(capturedImage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"截图失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearAll()
        {
            SelectedImage = null;
            RecognizedText = string.Empty;
        }

        private void CopyToClipboard()
        {
            if (!string.IsNullOrWhiteSpace(RecognizedText))
            {
                Clipboard.SetText(RecognizedText);
                MessageBox.Show("已复制到剪贴板", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void ProcessImage(string filePath)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("文件不存在", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(filePath);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();

            SelectedImage = bitmap;
            RecognizeFromImage(bitmap);
        }

        public void ProcessImage(BitmapImage image)
        {
            SelectedImage = image;
            RecognizeFromImage(image);
        }

        private void RecognizeFromImage(BitmapImage image)
        {
            IsProcessing = true;
            RecognizedText = string.Empty;

            try
            {
                RecognizedText = _ocrService.RecognizeText(image);
            }
            catch (Exception ex)
            {
                RecognizedText = $"识别错误: {ex.Message}";
            }
            finally
            {
                IsProcessing = false;
            }
        }
    }
}