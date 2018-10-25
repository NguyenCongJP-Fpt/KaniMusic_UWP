using KaniMusic_UWP_V0._1.Entity;
using KaniMusic_UWP_V0._1.Service;
using KaniMusic_UWP_V0._1.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace KaniMusic_UWP_V0._1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private static string API_LOGIN = "http://2-dot-backup-server-002.appspot.com/_api/v2/members/authentication";

        public MainPage()
        {
            this.InitializeComponent();
        }

        // List of ValueTuple holding the Navigation Tag and the relative Navigation Page 
        private readonly IList<(string Tag, Type Page)> _pages = new List<(string Tag, Type Page)>
        {
            ("home", typeof(Views.HomePage)),
            ("rank", typeof(Views.Rank)),
            ("musics", typeof(Views.Music)),
            ("playlists", typeof(Views.PlayList)),
            ("videos", typeof(Views.MyVideo)),
            ("listened", typeof(Views.Listened)),
            ("favorite", typeof(Views.Favorite)),
            ("myaccount", typeof(Views.MyAccount)),

        };

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            // You can also add items in code behind
            NavView.MenuItems.Add(new NavigationViewItemSeparator());
            //NavView.MenuItems.Add(new NavigationViewItem
            //{
            //    Content = "My content",
            //    Icon = new SymbolIcon(Symbol.Folder),
            //    Tag = "content"
            //});
            //_pages.Add(("content", typeof(MainPage)));

            ContentFrame.Navigated += On_Navigated;

            // NavView doesn't load any page by default: you need to specify it
            NavView_Navigate("home");

            // Add keyboard accelerators for backwards navigation
            var goBack = new KeyboardAccelerator { Key = VirtualKey.GoBack };
            goBack.Invoked += BackInvoked;
            this.KeyboardAccelerators.Add(goBack);

            // ALT routes here
            var altLeft = new KeyboardAccelerator
            {
                Key = VirtualKey.Left,
                Modifiers = VirtualKeyModifiers.Menu
            };
            altLeft.Invoked += BackInvoked;
            this.KeyboardAccelerators.Add(altLeft);
        }

        //Nút setting
        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                //ContentFrame.Navigate(typeof(MainPage));
            }
            else
            {
                // Getting the Tag from Content (args.InvokedItem is the content of NavigationViewItem)
                var navItemTag = NavView.MenuItems
                    .OfType<NavigationViewItem>()
                    .First(i => args.InvokedItem.Equals(i.Content))
                    .Tag.ToString();

                NavView_Navigate(navItemTag);
            }
        }

        private void NavView_Navigate(string navItemTag)
        {
            var item = _pages.First(p => p.Tag.Equals(navItemTag));
            ContentFrame.Navigate(item.Page);
        }

        private void NavView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            On_BackRequested();
        }

        private void BackInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            On_BackRequested();
            args.Handled = true;
        }

        private bool On_BackRequested()
        {
            if (!ContentFrame.CanGoBack)
                return false;

            // Don't go back if the nav pane is overlayed
            if (NavView.IsPaneOpen &&
                (NavView.DisplayMode == NavigationViewDisplayMode.Compact ||
                NavView.DisplayMode == NavigationViewDisplayMode.Minimal))
                return false;

            ContentFrame.GoBack();
            return true;
        }

        private void On_Navigated(object sender, NavigationEventArgs e)
        {
            NavView.IsBackEnabled = ContentFrame.CanGoBack;

            if (ContentFrame.SourcePageType == typeof(MainPage))
            {
                // SettingsItem is not part of NavView.MenuItems, and doesn't have a Tag
                NavView.SelectedItem = (NavigationViewItem)NavView.SettingsItem;
            }
            else
            {
                var item = _pages.First(p => p.Page == e.SourcePageType);

                NavView.SelectedItem = NavView.MenuItems
                    .OfType<NavigationViewItem>()
                    .First(n => n.Tag.Equals(item.Tag));
            }
        }

        //Sử lý show dialog form Login khi bấm nút đăng nhập
        private async void btnLogin(object sender, RoutedEventArgs e)
        {
            Login loginShow = new Login();
            await loginShow.ShowAsync();

        }

        //Sử lý show dialog form Login khi bấm nút upload
        private async void btnUploadSong(object sender, RoutedEventArgs e)
        {
            CreateSong uploadSong = new CreateSong();
            await uploadSong.ShowAsync();
        }

        //Sử lý show dialog form Login khi bấm nút đăng ký
        private async void btnRegister(object sender, RoutedEventArgs e)
        {
            Register registerShow = new Register();
            await registerShow.ShowAsync();
        }

        //Sử lý Sign out khi bấm nút đăng ký
        private async void btnSignout(object sender, RoutedEventArgs e)
        {
            ContentDialog noWifiDialog = new ContentDialog()
            {
                Title = "Thông Báo",
                Content = "Chức năng đang phát triển..!! Đề nghị tắt chương trình và bật lại.!!",
                CloseButtonText = "Ok"
            };
            await noWifiDialog.ShowAsync();
        }

    }
}
