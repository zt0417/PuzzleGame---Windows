using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace puzzleGame
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class chooseMethod : Page
    {
        public chooseMethod()
        {
            this.InitializeComponent();
        }


        async private void CameraCaptureUI_Click(object sender, RoutedEventArgs e)
        {
           
            CameraCaptureUI cameraUI = new CameraCaptureUI();

            Windows.Storage.StorageFile capturedMedia =
            await cameraUI.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (capturedMedia != null)
            {
                var stream = await capturedMedia.OpenAsync(FileAccessMode.Read);

             
            }
            BitmapImage photo = new BitmapImage();
            photo = await LoadImage(capturedMedia);
            //mediaPreivew.Source = photo;
            this.Frame.Navigate(typeof(gamePage), capturedMedia);


        }

        private static async Task<BitmapImage> LoadImage(StorageFile file)
        {
            BitmapImage bitmapImage = new BitmapImage();
            FileRandomAccessStream stream = (FileRandomAccessStream)await file.OpenAsync(FileAccessMode.Read);

            bitmapImage.SetSource(stream);

            return bitmapImage;

        }


        private bool EnsureUnsnapped()
        {
            // using Windows.UI.ViewManagement;
            bool unsnapped = ((ApplicationView.Value != ApplicationViewState.Snapped) || ApplicationView.TryUnsnap());
            return unsnapped;
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {

            if (EnsureUnsnapped())
            {
                FileOpenPicker openPicker = new FileOpenPicker();
                openPicker.ViewMode = PickerViewMode.Thumbnail;
                openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                openPicker.FileTypeFilter.Add(".jpg");
                openPicker.FileTypeFilter.Add(".jpeg");
                openPicker.FileTypeFilter.Add(".png");

                StorageFile file = await openPicker.PickSingleFileAsync();
                if (file != null)
                {

                    BitmapImage photo = new BitmapImage();
                    photo = await LoadImage(file);
                    //mediaPreivew.Source = photo;
                    this.Frame.Navigate(typeof(gamePage), file);
          
                }
                else
                {
                    //OutputTextBlock.Text = "Operation cancelled.";
                }
            }
        }

        private void LeaderboardButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LeaderboardsPage));
        }
    }
}
