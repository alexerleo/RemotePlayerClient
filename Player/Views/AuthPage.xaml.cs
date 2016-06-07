using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Windows.UI.Core;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

using Player.Controllers;

namespace Player.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AuthPage : Page
    {
        private CoreDispatcher dispatcher;
        public AuthController auth { get; set; }
        
        public AuthPage()
        {
            dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            auth = new AuthController(OnSuccess, OnFailed);

            this.InitializeComponent();            
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            auth.Submit();
        }

        private async void OnSuccess()
        {
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Frame.Navigate(typeof(MainPage));
            });            
        }

        private void OnFailed()
        {
            
        }
    }
}
