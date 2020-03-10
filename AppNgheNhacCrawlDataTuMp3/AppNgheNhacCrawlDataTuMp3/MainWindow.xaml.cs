using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using xNet;

namespace AppNgheNhacCrawlDataTuMp3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private bool isCheckVN;
        private bool isCheckEU;
        private bool isCheckKO;
        private List<Song> listBXHVN;
        private List<Song> listBXHEU;
        private List<Song> listBXHKO;
        public bool IsCheckVN { get => isCheckVN; set { isCheckVN = value; lsbTopSong.ItemsSource = listBXHVN; isCheckEU = false; isCheckKO = false; OnPropertyChanged("IsCheckVN"); OnPropertyChanged("IsCheckEU"); OnPropertyChanged("IsCheckKO"); } }
        public bool IsCheckEU { get => isCheckEU; set { isCheckEU = value; lsbTopSong.ItemsSource = listBXHEU; isCheckVN = false; isCheckKO = false; OnPropertyChanged("IsCheckVN"); OnPropertyChanged("IsCheckEU"); OnPropertyChanged("IsCheckKO"); } }
        public bool IsCheckKO { get => isCheckKO; set { isCheckKO = value; lsbTopSong.ItemsSource = listBXHKO; isCheckEU = false; isCheckVN = false; OnPropertyChanged("IsCheckVN"); OnPropertyChanged("IsCheckEU"); OnPropertyChanged("IsCheckKO"); } }

        public List<Song> ListBXHVN { get => listBXHVN; set => listBXHVN = value; }
        public List<Song> ListBXHEU { get => listBXHEU; set => listBXHEU = value; }
        public List<Song> ListBXHKO { get => listBXHKO; set => listBXHKO = value; }

        public MainWindow()
        {
            InitializeComponent();
            ucSongInfo.BackToMain += UcSongInfo_BackToMain;
            this.DataContext = this;
            listBXHVN = new List<Song>();
            listBXHEU = new List<Song>();
            listBXHKO = new List<Song>();
            CrawlData();
            
            IsCheckVN = true;
            
        }

        void CrawlData()
        {
            HttpRequest http = new HttpRequest();
            string htmlBXH = http.Get(@"https://mp3.zing.vn/zing-chart-tuan/").ToString();
            string regexPattern = @"<div class=""box-chart-ov bordered non-bg-rank""(.*?)</ul>";
            var htmlListBXH = Regex.Matches(htmlBXH, regexPattern, RegexOptions.Singleline);

            string bxhVN = htmlListBXH[0].ToString();
            string bxhEU = htmlListBXH[1].ToString();
            string bxhKO = htmlListBXH[2].ToString();

            AddInfoSongIntoListSong(ListBXHVN, bxhVN);
            AddInfoSongIntoListSong(ListBXHEU, bxhEU);
            AddInfoSongIntoListSong(ListBXHKO, bxhKO);

        }

        void AddInfoSongIntoListSong(List<Song> listSong, string htmlsong)
        {
            var listSongHtml = Regex.Matches(htmlsong, @"<h3 class=""(.*?)</a>", RegexOptions.Singleline);
            for (int i = 0; i < listSongHtml.Count; i++)
            {
                string dataSong = listSongHtml[i].ToString();

                string SongAndSinger = Regex.Match(dataSong, @"title=""(.*?)""", RegexOptions.Singleline).ToString();
                int indexBreak = SongAndSinger.IndexOf(" - ");
                string song = SongAndSinger.Substring(0, indexBreak).Replace("title=\"","");
                string singer = SongAndSinger.Substring(indexBreak, SongAndSinger.Length - indexBreak - 1).Replace(" - ","");

                int startIndexURL = dataSong.IndexOf("href=\"");
                int endIndexURL = dataSong.IndexOf(".html");
                string songURL = dataSong.Substring(startIndexURL, endIndexURL - startIndexURL+5).Replace("href=\"","");

                HttpRequest http = new HttpRequest();
                string htmlSourceSong = http.Get(@"https://mp3.zing.vn" + songURL + @"?play_song=1").ToString().Replace("<br>", "");

                var lydric = Regex.Match(htmlSourceSong, @"<p class=""fn-wlyrics fn-content""(.*?)</p>",RegexOptions.Singleline);
                string tempLydric = "Chưa Có Lydric";
                if (lydric != null)
                {
                    tempLydric = Regex.Match(lydric.ToString(), @""">(.*?)</p>", RegexOptions.Singleline).ToString();
                    tempLydric = tempLydric.Replace(@""">", "").Replace("</p>", "");
                }

                string getJsonURL = Regex.Match(htmlSourceSong, @"<div id=""zplayerjs-wrapper"" class=""player mt0"" data-xml=""(.*?)""", RegexOptions.Singleline).ToString().Replace(@"<div id=""zplayerjs-wrapper"" class=""player mt0"" data-xml=""", "").Replace(@"""", "");
                string jSonInfo = http.Get(@"https://mp3.zing.vn/xhr" + getJsonURL).ToString();

                JObject jObject = JObject.Parse(jSonInfo);
                string downLoadURL;

                try
                {
                    downLoadURL = "http:" + jObject["data"]["source"]["128"].ToString();
                }
                catch (Exception)
                {

                    downLoadURL = "http:" + jObject["data"]["source"].ToString();
                }
                
                string photoURL = jObject["data"]["thumbnail"].ToString();

                string savePath = AppDomain.CurrentDomain.BaseDirectory + "Song\\" + song + ".mp3";

                listSong.Add(new Song() { STT = i + 1, SongName = song, SingerName = singer, SongURL = songURL, DownloadURL = downLoadURL, Lydric = tempLydric, PhotoURL = photoURL, SavePath = savePath });
            }
        }
        private void UcSongInfo_BackToMain(object sender, EventArgs e)
        {
            GridTop10.Visibility = Visibility.Visible;
            ucSongInfo.Visibility = Visibility.Hidden;
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            Song song = (sender as Button).DataContext as Song;
            ucSongInfo.SongInfo = song;
            GridTop10.Visibility = Visibility.Hidden;
            ucSongInfo.Visibility = Visibility.Visible;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string newName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(newName));
            }
        }
    }
}
