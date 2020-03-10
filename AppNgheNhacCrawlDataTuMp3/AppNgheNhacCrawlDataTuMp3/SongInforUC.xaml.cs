using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AppNgheNhacCrawlDataTuMp3
{
    /// <summary>
    /// Interaction logic for SongInforUC.xaml
    /// </summary>
    public partial class SongInforUC : UserControl, INotifyPropertyChanged
    {
        private Song songInfo = new Song();

        public Song SongInfo {
            get => songInfo;
            set {
                songInfo = value;
                this.DataContext = SongInfo;
                DownloadSong(songInfo);
                OnPropertyChanged("SongInfo");
            }
        }
        DispatcherTimer timer;
        public SongInforUC()
        {
            InitializeComponent();
            this.DataContext = SongInfo;
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            SongInfo.Position++;
            sdDuration.Value = SongInfo.Position;
        }

        private event EventHandler backToMain;
        public event EventHandler BackToMain
        {
            add { backToMain += value; }
            remove { backToMain -= value; }
        }

        void DownloadSong(Song songInfo)
        {
            if (songInfo.DownloadURL == "")
            {
                return;
            }
            string songName = songInfo.SavePath;
            if (!File.Exists(songName))
            {
                WebClient wb = new WebClient();
                wb.DownloadFile(songInfo.DownloadURL, AppDomain.CurrentDomain.BaseDirectory + "Song\\" + songInfo.SongName + ".mp3");
            }
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (backToMain != null)
            {
                backToMain(this, new EventArgs());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string newName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(newName));
            }
        }

        private void MdAudio_MediaOpened(object sender, RoutedEventArgs e)
        {
            SongInfo.Duration = mdAudio.NaturalDuration.TimeSpan.TotalSeconds;
            sdDuration.Maximum = SongInfo.Duration;
            SongInfo.Position = 0;
            timer.Start();

        }
        bool isDraging = false;
        private void SdDuration_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (isDraging)
            {
                SongInfo.Position = sdDuration.Value;
                mdAudio.Position = new TimeSpan(0, 0, (int)SongInfo.Position);
            }
            
        }

        private void SdDuration_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            isDraging = false;
        }

        private void SdDuration_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            isDraging = true;
        }
    }
}
