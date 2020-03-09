using System;
using System.Collections.Generic;
using System.Linq;
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

namespace AppNgheNhacCrawlDataTuMp3
{
    /// <summary>
    /// Interaction logic for SongInforUC.xaml
    /// </summary>
    public partial class SongInforUC : UserControl
    {
        public SongInforUC()
        {
            InitializeComponent();
        }

        private event EventHandler backToMain;
        public event EventHandler BackToMain
        {
            add { backToMain += value; }
            remove { backToMain -= value; }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (backToMain != null)
            {
                backToMain(this, new EventArgs());
            }
        }
    }
}
