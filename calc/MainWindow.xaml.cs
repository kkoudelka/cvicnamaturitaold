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
using calc.Models;

namespace calc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Operand _lastOperation = Operand.None;
        private double _answer = 0;
        private double _lastNumber = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Number_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var number = btn.Content.ToString();
            if (TextBlockOutput.Text[0] == '0')
            {
                TextBlockOutput.Text = number;
            }
            else TextBlockOutput.Text += number;
        }

        private void CE_Click(object sender, RoutedEventArgs e)
        {
            TextBlockOutput.Text = "0";
        }

        private void BKSP_Click(object sender, RoutedEventArgs e)
        {
            TextBlockOutput.Text = TextBlockOutput.Text.Length > 1
                ? TextBlockOutput.Text.Remove(TextBlockOutput.Text.Length - 1, 1)
                : "0";
        }

        private void Operand_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var content = btn.Content.ToString();


            _lastNumber = double.Parse(TextBlockOutput.Text);
            if(_lastOperation != Operand.None) Calc();


            TextBlockOutput.Text = "0";


            _lastOperation = (Operand)content[0];

        }

        public void Calc()
        {
            switch (_lastOperation)
            {
                case Operand.Add:
                    _answer += _lastNumber;
                    break;
                case Operand.Subtract:
                    _answer -= _lastNumber;
                    break;
                case Operand.Multiply:
                    _answer *= _lastNumber;
                    break;
                case Operand.Divide:
                    _answer /= _lastNumber;
                    break;
            }

            TextBlockAnswer.Text = _answer.ToString();
        }



    }
}
