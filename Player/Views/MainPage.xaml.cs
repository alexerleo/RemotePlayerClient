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
using Windows.Networking.Sockets;

using Windows.Storage;
using Windows.Storage.Streams;

using Player.Models;
using Player.Database;
using Player.Controllers;

using Windows.UI.Core;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Player.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private CoreDispatcher dispatcher;
        public MainController controller { get; set; } = new MainController();

        public MainPage()
        {
            InitializeComponent();

            dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
        }
        
        private async void OnPlaying(IRandomAccessStream stream, string contentType)
        {
            if (stream == null || String.IsNullOrEmpty(contentType))
            {
                return;
            }
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Display.SetSource(stream, contentType);
                Display.Play();
            });            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            controller.Library.Playing += OnPlaying; 
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {           
            if (Display.CurrentState == MediaElementState.Playing)
            {
                Display.Pause();
                Play.Icon = new SymbolIcon(Symbol.Play);
                Play.Label = "Play";
            }
            else if(Display.CurrentState == MediaElementState.Paused || Display.CurrentState == MediaElementState.Stopped)
            {
                Display.Play();
                Play.Icon = new SymbolIcon(Symbol.Pause);
                Play.Label = "Pause";
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            Play.Icon = new SymbolIcon(Symbol.Play);
            Play.Label = "Play";
            Display.Stop();
        }

        private void Display_MediaOpened(object sender, RoutedEventArgs e)
        {
            Position.Maximum = (int)Display.NaturalDuration.TimeSpan.TotalMilliseconds;
            Display.Play();
            Play.Icon = new SymbolIcon(Symbol.Pause);
            Play.Label = "Pause";
        }

        private void Display_MediaEnded(object sender, RoutedEventArgs e)
        {            
            controller.NextTrack();
            if (controller.current.track == null)
            {
                Play.Icon = new SymbolIcon(Symbol.Play);
                Play.Label = "Play";
                Display.Stop();
                Position.Value = 0;
            }
        }

        private void Display_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            if (Display.CurrentState == MediaElementState.Opening || Display.CurrentState == MediaElementState.Stopped)
            {
                Position.Value = 0;
            }
        }       

        private void TrackList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {                             
                var track = (Track)(e.AddedItems.First());
                controller.SelectTrack(track);    
            }
            catch(Exception ex)
            {
                
            }
        }

        private void BandsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            try
            {
                var band = (Band)(e.AddedItems.FirstOrDefault());
                controller.SelectBand(band);
            }
            catch(Exception ex)
            {

            }
        }

        private void AlbumsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var album = (Album)(e.AddedItems.First());
                controller.SelectAlbum(album);
            }
            catch(Exception)
            {

            }
        }

        private void DeviceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var device = (Device)(e.AddedItems.First());
                controller.SelectDevice(device);
            }
            catch (Exception)
            {

            }
        }
    }    
}
