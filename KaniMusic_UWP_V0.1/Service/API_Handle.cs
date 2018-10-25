﻿using KaniMusic_UWP_V0._1.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace KaniMusic_UWP_V0._1.Service
{
    class API_Handle
    {
        public static String GET_UPLOAD_TOKEN = "https://2-dot-backup-server-002.appspot.com/get-upload-token";
        public static String API_REGISTER = "https://2-dot-backup-server-002.appspot.com/_api/v2/members";
        private static String API_LOGIN = "https://2-dot-backup-server-002.appspot.com/_api/v2/members/authentication";
        public static String API_INFORMATION = "https://2-dot-backup-server-002.appspot.com/_api/v2/members/information";
        public static String API_CREATE_SONG = "http://2-dot-backup-server-002.appspot.com/_api/v2/songs";
        public static String API_MY_SONG = "http://2-dot-backup-server-002.appspot.com/_api/v2/songs/get-mine";


        public async static Task<string> Sign_Up(Member member)
        {
            HttpClient httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(member), System.Text.Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync(API_REGISTER, content);
            var contents = await response.Result.Content.ReadAsStringAsync();
            Debug.WriteLine(contents);
            return contents;
        }

        public async static Task<HttpResponseMessage> Sign_In(string email, string password)
        {
            Dictionary<String, String> LoginInfor = new Dictionary<string, string>();
            LoginInfor.Add("email", email);
            LoginInfor.Add("password", password);

            HttpClient httpClient = new HttpClient();
            StringContent content = new StringContent(JsonConvert.SerializeObject(LoginInfor), System.Text.Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync(API_LOGIN, content).Result;
            return response;
        }

        public async static Task<HttpResponseMessage> GetData(string api_url, string scheme, string token)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, token);
            var response = httpClient.GetAsync(api_url);
            return response.Result;
        }

        public async static Task<string> Create_Song(Song song)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = await folder.GetFileAsync("token.txt");
            var tokenContent = await FileIO.ReadTextAsync(file);

            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(tokenContent);
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + token.token);
            StringContent content = new StringContent(JsonConvert.SerializeObject(song), System.Text.Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync(API_CREATE_SONG, content);
            var contents = await response.Result.Content.ReadAsStringAsync();
            Debug.WriteLine(contents);
            Debug.WriteLine("Action success");
            return contents;
        }
    }
}
