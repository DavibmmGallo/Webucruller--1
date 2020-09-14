using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Threading;
using webcrawler_zro_ichi;

namespace webview
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Recent_Animes : Window
    {
        public Recent_Animes()
        {
            InitializeComponent();
            Program program = new Program();
            program.execute();
            var animes = GetAnimes();
            if (animes != null)
                ListViewAnimes.ItemsSource = animes;
        }

        private List<Anime> GetAnimes()
        {
            AnimeContext ac = new AnimeContext();

            List<Anime> lista = new List<Anime>();

            ac.animes.ToList()
                .ForEach(p => lista.Add(p));

            return lista;
        }
        private static readonly Action EmptyDelegate = delegate { };
        private void refresh(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }
        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var image = btn.Content as Image;
            var str = image.ToolTip.ToString();
            try
            {
                Process.Start("explorer.exe",str);
            }
            catch(Exception er)
            {
                MessageBox.Show(er.Message);
            }
            
        }

    }
}
