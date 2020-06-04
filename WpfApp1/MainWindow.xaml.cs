using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
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
using Microsoft.Win32;
using Newtonsoft.Json;

namespace WpfApp1
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
            openFileDialog.Filter = "Json and Text file format (*.json, *.txt)|*.json;*.txt";
            if (openFileDialog.ShowDialog() != true)
                return;

            var path = openFileDialog.FileName;

            if (path.EndsWith(".json"))
            {
                var fileData = File.ReadAllText(path);
                wholesomeData = JsonConvert.DeserializeObject<List<SplitData>>(fileData);
            }
            else
            {
                var fileData = File.ReadAllLines(path);
                foreach (var row in fileData)
                {
                    var data = new SplitData(row);
                    wholesomeData.Add(data);
                }

            }

            //var firstYear = wholesomeData.Select(x => x.year).OrderByDescending(x => x).Last();
            //var lastYear = wholesomeData.Select(x => x.year).OrderByDescending(x => x).First();

            var years = wholesomeData
                .Select(x => x.year)
                .OrderByDescending(x => x);
            DateTo.DisplayDateEnd = new DateTime(years.First(), 12, 31);
            
            DateFrom.DisplayDateStart = new DateTime(years.Last(), 1, 1);
        }

        public void UpdateStatistics()
        {
            if (DateFrom.SelectedDate == null || DateTo.SelectedDate == null || wholesomeData.Count == 0 ||
                wholesomeData == null)
                return;



            var dataDraw = wholesomeData
                .Where(x => x.year > DateFrom.SelectedDate?.Year ||
                            (x.year == DateFrom.SelectedDate?.Year && x.week >= GetWeek(DateFrom.SelectedDate.Value))
                            && (x.year < DateTo.SelectedDate?.Year ||
                                (x.year == DateTo.SelectedDate?.Year && x.week <= GetWeek(DateTo.SelectedDate.Value))))
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
                .GroupBy(grp => grp)
                .OrderByDescending(grp => grp.Count())
                .Select(grp => grp.Key)
                .FirstOrDefault();

            MostText.Text = most.ToString();

            var least = draw1values
                .GroupBy(grp => grp)
                .OrderByDescending(grp => grp.Count())
                .Select(grp => grp.Key)
                .LastOrDefault();

            LeastText.Text = least.ToString();

            var draw2ordered = draw2values
                .GroupBy(grp => grp)
                .OrderByDescending(grp => grp.Count())
                .Select(grp => grp.Key)
                .ToList();

            ListBoxDraw2.Items.Clear();

            draw2ordered.RemoveAll(x => x == 0);
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

        public static int GetWeek(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
    }


    public class SplitData
    {
        public int year { get; set; }
        public int week { get; set; }
        public List<int> draw1 = new List<int>();
        public List<int> draw2 = new List<int>();

        /// <summary>
        /// Creates new instance of SplitData
        /// </summary>
        
        public SplitData()
        {

        }

        /// <summary>
        /// Creates new instance of SplitData from row
        /// </summary>
        /// <param name="row">One row from text file</param>
        public SplitData(string row)
        {
            var splitted = row.Split(';');

            year = Convert.ToInt32(splitted[0]);
            week = Convert.ToInt32(splitted[1]);
            for (int i = 2; i <= 8; i++)
            {
                draw1.Add(Convert.ToInt32(splitted[i]));
            }
            for (int i = 9; i < splitted.Length; i++)
            {
                draw2.Add(Convert.ToInt32(splitted[i]));
            }

        }

    }




}

