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
using System.Windows.Controls.Primitives;
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
        private bool isPlaying;

        public bool IsPlaying
        {
            get { return isPlaying; }
            set {
                isPlaying = value;
                if (isPlaying)
                {
                    mdAudio.Play();
                    timer.Start();
                    btnPlayAndPause.Content = "Pause";
                }
                else
                {
                    mdAudio.Pause();
                    timer.Stop();
                    btnPlayAndPause.Content = "Play";
                }
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
            SongInfo.Position += SpeedRatio;
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
            IsPlaying = true;
            SongInfo.Duration = mdAudio.NaturalDuration.TimeSpan.TotalSeconds;
            sdDuration.Maximum = SongInfo.Duration;
            SongInfo.Position = 0;
            txbDuration.Text = new TimeSpan(0, (int)(SongInfo.Duration / 60), (int)(SongInfo.Duration % 60)).ToString(@"mm\:ss");
            SpeedRatio = 1;
        }
        private bool isDraging = false;
        public bool IsDraging { get => isDraging; set => isDraging = value; }

        private void SdDuration_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (IsDraging)
            {
                SongInfo.Position = sdDuration.Value;
                mdAudio.Position = new TimeSpan(0, 0, (int)SongInfo.Position);
            }
            txbPosition.Text = new TimeSpan(0, (int)(SongInfo.Position / 60), (int)(SongInfo.Position % 60)).ToString(@"mm\:ss");
        }

        private void SdDuration_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            IsDraging = false;
        }

        private void SdDuration_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            IsDraging = true;
        }

        private void btnPlayAndPause_Click(object sender, RoutedEventArgs e)
        {
            IsPlaying = !IsPlaying;
        }

        private event EventHandler previousClicked;
        public event EventHandler PreviousClicked
        {
            add { previousClicked += value; }
            remove { previousClicked -= value; }
        }

        private event EventHandler nextClicked;
        public event EventHandler NextClicked
        {
            add { nextClicked += value; }
            remove { nextClicked -= value; }
        }
        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (previousClicked!= null)
            {
                previousClicked(this, new EventArgs());
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (nextClicked != null)
            {
                nextClicked(this, new EventArgs());
            }
        }
        private double speedRatio;
        public double SpeedRatio { get => speedRatio; set => speedRatio = value; }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton toggle = sender as ToggleButton;
            if (toggle.IsChecked == true)
            {
                SpeedRatio = 2;
            }
            else
            {
                SpeedRatio = 1;
            }
            mdAudio.SpeedRatio = SpeedRatio;
            toggle.Content = string.Format("{0}.0", SpeedRatio);
        }
    }
}
