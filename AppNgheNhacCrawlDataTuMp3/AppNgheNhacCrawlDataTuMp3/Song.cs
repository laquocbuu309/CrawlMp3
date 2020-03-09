using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppNgheNhacCrawlDataTuMp3
{
    
    public class Song
    {
        private int sTT;
        private string songName;
        private string singerName;
        private string songURL;
        private string downloadURL;

        public int STT { get => sTT; set => sTT = value; }
        public string SongName { get => songName; set => songName = value; }
        public string SingerName { get => singerName; set => singerName = value; }
        public string SongURL { get => songURL; set => songURL = value; }
        public string DownloadURL { get => downloadURL; set => downloadURL = value; }
    }
}
