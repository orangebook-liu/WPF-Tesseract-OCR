using OcrTool.Services;
using OcrTool.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace OcrTool
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel(new OcrService());
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0)
                {
                    string filePath = files[0];
                    string extension = Path.GetExtension(filePath).ToLower();
                    
                    if (extension == ".png" || extension == ".jpg" || extension == ".jpeg" || extension == ".bmp")
                    {
                        var viewModel = (MainViewModel)DataContext;
                        viewModel.ProcessImage(filePath);
                    }
                    else
                    {
                        MessageBox.Show("请拖放图片文件", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            
            ((MainViewModel)DataContext).IsDragOver = false;
            e.Handled = true;
        }

        private void Window_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
                ((MainViewModel)DataContext).IsDragOver = true;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }

        private void Window_DragLeave(object sender, DragEventArgs e)
        {
            ((MainViewModel)DataContext).IsDragOver = false;
        }
    }
}