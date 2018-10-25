using KaniMusic_UWP_V0._1.Entity;
using KaniMusic_UWP_V0._1.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace KaniMusic_UWP_V0._1.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MyAccount : Page
    {
        private Member currentMember;
        public MyAccount()
        {
            this.InitializeComponent();
        }

        //private async void GetInfo()
        //{
        //    string text = await API_Handle.CheckCredential();
        //    if (text != "")
        //    {
        //        string token = await API_Handle.GetToken();
        //        Debug.WriteLine(token);
        //        if (token != "")
        //        {
        //            var httpResponseMessage = API_Handle.GetData(API_Handle.API_INFORMATION, "Basic", token);
        //            Debug.WriteLine(httpResponseMessage.Result.StatusCode);
        //            //Debug.WriteLine(httpResponseMessage);
        //            if (httpResponseMessage.Result.StatusCode == HttpStatusCode.Created)
        //            {
        //                var informationJson = await httpResponseMessage.Result.Content.ReadAsStringAsync();
        //                Debug.WriteLine(HttpStatusCode.Created);
        //                currentMember = JsonConvert.DeserializeObject<Member>(informationJson);
        //                string[] date = currentMember.birthday.Split('T');
        //                this.txt_fullname.Text = (currentMember.firstName + " " + currentMember.lastName).ToUpper();
        //                this.txt_birthday.Text = "Birthday: " + date[0];
        //                this.txt_email.Text = "Email: " + currentMember.email;
        //                this.txt_phone.Text = "Phone: " + currentMember.phone;
        //                try
        //                {
        //                    this.img_avatar.ProfilePicture = new BitmapImage(new Uri(currentMember.avatar, UriKind.Absolute));
        //                }
        //                catch
        //                {

        //                }
        //            }
        //        }
        //        Debug.WriteLine(text);
        //    }
        //}
    }
}
