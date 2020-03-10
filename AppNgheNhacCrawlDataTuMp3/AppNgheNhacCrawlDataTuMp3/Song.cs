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
        private string lydric;
        private string downloadURL;
        private string photoURL;
        private string savePath;
        private double duration;
        private double position;

        public int STT { get => sTT; set => sTT = value; }
        public string SongName { get => songName; set => songName = value; }
        public string SingerName { get => singerName; set => singerName = value; }
        public string SongURL { get => songURL; set => songURL = value; }
        public string DownloadURL { get => downloadURL; set => downloadURL = value; }
        public string Lydric { get => lydric; set => lydric = value; }
        public string PhotoURL { get => photoURL; set => photoURL = value; }
        public string SavePath { get => savePath; set => savePath = value; }
        public double Duration { get => duration; set => duration = value; }
        public double Position { get => position; set => position = value; }
    }
}
