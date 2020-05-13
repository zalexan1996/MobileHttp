using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HttpServer;

namespace MobileHttp
{
    public partial class App : Application
    {
        MobileHttpServer server;

        public App()
        {
            server = new MobileHttpServer();

            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            server.BeginListening();
        }

        protected override void OnSleep()
        {
            server.StopListening();
        }

        protected override void OnResume()
        {
            server.BeginListening();
        }
    }
}
