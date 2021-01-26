using System;
using System.Diagnostics;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Newtonsoft.Json.Linq;
using RestSharp;    

namespace DSApps.Views
{
    public class About : UserControl
    {
        
        
        private readonly Model.About aboutModel = new Model.About();


        public About()
        {
            DataContext = aboutModel;
            // aboutModel.VersionName
            aboutModel.VersionName = "🍌";
            aboutModel.ActualVersion = "V0.0.2";
            // Bind(aboutModel.VersionName, "🍌🍌🍌");
            InitializeComponent();
            new Thread(() =>
            {
                try
                {
                    var restClient =
                        new RestClient("https://ds5.codemaker.cl/version.json");
                    var restRequest = new RestRequest(Method.GET);
                    var restResponse = restClient.Execute(restRequest);
                    var jsonReply = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(restResponse.Content);
                    var VersionName = jsonReply["ActualVersion"].Value<string>();
                    Debug.WriteLine("Version Name: " + VersionName);
                    aboutModel.VersionName = VersionName;
                    aboutModel.VersionUrl = jsonReply["DownloadUrl"].ToString();
                    if (aboutModel.ActualVersion != aboutModel.VersionName)
                    {
                        aboutModel.IsNewUpdate = true;
                    }
                }
                catch
                {
                    Debug.WriteLine("Error on get Version");
                }
            }).Start();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            Utils.OpenUrl($"https://github.com/Mxater/DualSenseSupport/releases/tag/{aboutModel.ActualVersion}");
        }

  

        private void TextNewVersion_OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            Utils.OpenUrl(aboutModel.VersionUrl);
        }
    }
}