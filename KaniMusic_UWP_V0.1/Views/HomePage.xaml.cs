using KaniMusic_UWP_V0._1.Entity;
using KaniMusic_UWP_V0._1.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace KaniMusic_UWP_V0._1.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        private ObservableCollection<Song> listSong;

        internal ObservableCollection<Song> ListSong { get => listSong; set => listSong = value; }

        public HomePage()
        {
            this.ListSong = new ObservableCollection<Song>();
            this.InitializeComponent();


        }

        private async void HomePageLoaded(object sender, RoutedEventArgs e)
        {
            var folder = ApplicationData.Current.LocalFolder;
            StorageFile file = await folder.GetFileAsync("token.txt");
            var tokenContent = await FileIO.ReadTextAsync(file);
            var token = JsonConvert.DeserializeObject<TokenResponse>(tokenContent);

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + token.token);
            var responseMessage = httpClient.GetAsync(API_Handle.API_CREATE_SONG);
            var content = await responseMessage.Result.Content.ReadAsStringAsync();
            Debug.WriteLine(content);
            if (responseMessage.Result.StatusCode == HttpStatusCode.OK)
            {
                var songResponse = JsonConvert.DeserializeObject<List<Song>>(content);
                foreach (var song in songResponse)
                {
                    ListSong.Add(new Song
                    {
                        name = song.name,
                        thumbnail = song.thumbnail,
                        author = song.author,
                        singer = song.singer,
                        description = song.description,
                        link = song.link
                    });
                }
                Debug.WriteLine("Oke, da tao thanh cong.");
            }
            else
            {
                ErrorResponse errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(content);
                foreach (var key in errorResponse.error.Keys)
                {
                    if (this.FindName(key) is TextBlock textBlock)
                    {
                        textBlock.Text = errorResponse.error[key];
                    }
                }
            }
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
