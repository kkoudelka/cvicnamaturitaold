using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace Sportka
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        List<SplitData> wholesomeData = new List<SplitData>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = @"C:\Users\kkoud\Desktop";
            openFileDialog.Filter = "Text file format (*.txt)|*.txt|Json file format (*.json)|*.json";
            if (openFileDialog.ShowDialog() != true)
                return;

            var path = openFileDialog.FileName;

            if (path.Contains(".json"))
            {
                var fileData = File.ReadAllText(path);
                wholesomeData = JsonConvert.DeserializeObject<List<SplitData>>(fileData);
            }
            else
            {
                var fileData = File.ReadAllLines(path);

                foreach (var row in fileData)
                {
                    var splitted = row.Split(';');
                    var data = new SplitData();
                    data.year = Convert.ToInt32(splitted[0]);
                    data.week = Convert.ToInt32(splitted[1]);
                    for (int i = 2; i <= 8; i++)
                    {
                        data.draw1.Add(Convert.ToInt32(splitted[i]));
                    }
                    for (int i = 9; i < splitted.Length; i++)
                    {
                        data.draw2.Add(Convert.ToInt32(splitted[i]));
                    }
                    wholesomeData.Add(data);
                }
            }




            //var start = wholesomeData.OrderBy(x => x.year).ThenBy(x => x.week).First();
            //DateFrom.DisplayDateStart = new DateTime(start.year, 1,1);



        }



        public void UpdateStatistics()
        {

            if (DateFrom.SelectedDate == null || DateTo.SelectedDate == null || wholesomeData.Count == 0 || wholesomeData == null)
                return;

            var dataDraw = wholesomeData
                .Where(x => x.year >= DateFrom.SelectedDate?.Year
                            /*&& x.week >= DateFrom.SelectedDate?.DayOfYear / 7*/
                            && x.year <= DateTo.SelectedDate?.Year
                           /* && x.week <= DateTo.SelectedDate?.DayOfYear / 7*/)
                .ToList();

            var draw1values = new List<int>();
            var draw2values = new List<int>();

            foreach (var d in dataDraw)
            {
                draw1values = draw1values.Concat(d.draw1).ToList();
            }

            foreach (var d in dataDraw)
            {
                draw2values = draw2values.Concat(d.draw2).ToList();
            }


            draw1values.RemoveAll(x => x == 0);

            var most = draw1values
                .GroupBy(i => i)
                .OrderByDescending(grp => grp.Count())
                .Select(grp => grp.Key)
                .First();

            MostText.Text = most.ToString();

            var least = draw1values
                .GroupBy(i => i)
                .OrderByDescending(grp => grp.Count())
                .Select(grp => grp.Key)
                .Last();

            LeastText.Text = least.ToString();

            var draw2ordered = draw2values
                .GroupBy(i => i)
                .OrderByDescending(grp => grp.Count())
                .Select(grp => grp.Key)
                .ToList();

            draw2ordered.RemoveAll(x => x == 0);

            

            ListBoxDraw2.Items.Clear();

            if (draw2ordered.Count == 0)
                return;
            

            foreach (var number in draw2ordered)
            {
                ListBoxDraw2.Items.Add(number.ToString());
            }
        }

        private void DateTo_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateStatistics();
        }

        private void DateFrom_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateStatistics();

        }


        public List<int> GenerateRandomNumbers()
        {
            List<int> numbers = new List<int>();

            Random rnd = new Random();

            while (numbers.Count < 7)
            {
                var randomNumber = rnd.Next(1,50);
                if (!numbers.Contains(randomNumber))
                    numbers.Add(randomNumber);
            }

            return numbers;
        }

        private void ButtonRandomGenerate_Click(object sender, RoutedEventArgs e)
        {
            var randomNumbers = GenerateRandomNumbers();
        }

    }

    public class SplitData
    {
        public int year { get; set; }
        public int week { get; set; }
        public List<int> draw1 = new List<int>();
        public List<int> draw2 = new List<int>();


    }
}
