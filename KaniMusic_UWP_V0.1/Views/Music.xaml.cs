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
    public sealed partial class Music : Page
    {
        private bool isPlaying = false;

        int onPlay = 0;

        TimeSpan _position;

        DispatcherTimer _timer = new DispatcherTimer();

        private ObservableCollection<Song> listSong;

        internal ObservableCollection<Song> ListSong { get => listSong; set => listSong = value; }


        public Music()
        {
            this.ListSong = new ObservableCollection<Song>();
            this.InitializeComponent();
            this.VolumeSlider.Value = 100;
            _timer.Interval = TimeSpan.FromMilliseconds(1000);
            _timer.Tick += ticktock;
            _timer.Start();
            this.InitializeComponent();
        }

        //Add bài hát.
        private async void MyMusicLoaded(object sender, RoutedEventArgs e)
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

        private void ticktock(object sender, object e)
        {
            MinDuration.Text = MediaPlayer.Position.Minutes + ":" + MediaPlayer.Position.Seconds;
            Progress.Minimum = 0;
            Progress.Maximum = MediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
            MaxDuration.Text = MediaPlayer.NaturalDuration.TimeSpan.Minutes + ":" + MediaPlayer.NaturalDuration.TimeSpan.Seconds;
            Progress.Value = MediaPlayer.Position.TotalSeconds;
        }


        private void StackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            StackPanel panel = sender as StackPanel;
            Song selectedSong = panel.Tag as Song;
            Debug.WriteLine(ListSong[0].name);
            onPlay = MenuList.SelectedIndex;
            LoadSong(selectedSong);
            PlaySong();
        }
        private void PlaySong()
        {
            MediaPlayer.Play();
            PlayButton.Icon = new SymbolIcon(Symbol.Pause);
            isPlaying = true;
        }
        private void PauseSong()
        {
            MediaPlayer.Pause();
            PlayButton.Icon = new SymbolIcon(Symbol.Play);
            isPlaying = false;
        }
        private void PlayClick(object sender, RoutedEventArgs e)
        {
            if (isPlaying)
            {
                PauseSong();
            }
            else
            {
                PlaySong();
            }
        }
        private void PlayBack(object sender, RoutedEventArgs e)
        {
            MediaPlayer.Stop();
            if (onPlay > 0)
            {
                onPlay = onPlay - 1;
            }
            else
            {
                onPlay = ListSong.Count - 1;
            }
            LoadSong(ListSong[onPlay]);
            PlaySong();
            MenuList.SelectedIndex = onPlay;
        }

        private void PlayNext(object sender, RoutedEventArgs e)
        {
            MediaPlayer.Stop();
            if (onPlay < ListSong.Count - 1)
            {
                onPlay = onPlay + 1;
            }
            else
            {
                onPlay = 0;
            }
            LoadSong(ListSong[onPlay]);
            PlaySong();
            MenuList.SelectedIndex = onPlay;
        }
        private void LoadSong(Entity.Song currentSong)
        {
            this.NowPlaying.Text = "Loading";
            MediaPlayer.Source = new Uri(currentSong.link);
            Debug.WriteLine(MediaPlayer.NaturalDuration.TimeSpan.TotalSeconds);
            this.NowPlaying.Text = currentSong.name + " - " + currentSong.singer;
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (MediaPlayer.Source != null && MediaPlayer.NaturalDuration.HasTimeSpan)
            {
                Progress.Minimum = 0;
                Progress.Maximum = MediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                Progress.Value = MediaPlayer.Position.TotalSeconds;

            }
        }

        private void VolumeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            Slider vol = sender as Slider;
            if (vol != null)
            {
                MediaPlayer.Volume = vol.Value / 100;
                this.volume.Text = vol.Value.ToString();
            }
        }


    }
}