using System;
using System.Collections.Generic;
using System.IO;
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
using BS.Models;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace BS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = @"D:\Program Files (x86)\Steam\steamapps\common\Beat Saber\Beat Saber_Data\CustomLevels\Jaroslav Beck - Beat Saber (Built in)";
            openFileDialog.Filter = "DAT file format (*.dat)|*.dat";
            if (openFileDialog.ShowDialog() != true)
                return;

            var fileData = File.ReadAllText(openFileDialog.FileName);

            try
            {
                var data = JsonConvert.DeserializeObject<MapData>(fileData);

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }

        }
    }
}
