using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace jpeg_to_pdf
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private readonly PdfManager _pdfManager;

    public MainWindow()
    {
      InitializeComponent();
      _pdfManager = new PdfManager();
    }

    private void Open_Click(object sender, RoutedEventArgs e)
    {
      var openFileDialog = new OpenFileDialog { Multiselect = true };

      if (openFileDialog.ShowDialog() != true)
      {
        return;
      }

      string imagePath = openFileDialog.FileNames[0];

      TestImage.Source = new BitmapImage(new Uri(imagePath));

      //_pdfManager.SavePdf(imagePath);
    }

    private void Save_Click(object sender, ExecutedRoutedEventArgs e)
    {
      throw new NotImplementedException();
    }
  }
}