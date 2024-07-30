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
using System.Xml.Linq;

namespace SimpleCalculatorWPF
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
        
        private double firstOperand;
        private double secondOperand;
        private string currentOperator;

        private void digitButton_Click(object sender, RoutedEventArgs e)
        {
            display.Text = $"{display.Text}{((Button)sender).Content}"; 
        }

        private void operatorButton_Click(object sender, RoutedEventArgs e)
        {
            firstOperand = double.Parse(display.Text);
            currentOperator = ((Button)sender).Content.ToString();
            display.Text = string.Empty;
        }
        
        private void resultButton_Click(object sender, RoutedEventArgs e)
        {
            secondOperand = double.Parse(display.Text);

            double result;
            switch (currentOperator)
            {
                case "+":
                    result = firstOperand + secondOperand;
                    break;
                case "-":
                    result = firstOperand - secondOperand;
                    break;
                case "*":
                    result = firstOperand * secondOperand;
                    break;
                case "/":
                    if (secondOperand != 0)
                        result = firstOperand / secondOperand;
                    else
                    {
                        display.Text = "Can not divide by zero!";
                        return;
                    }
                    break;
                default:
                    return;
            }

            display.Text = result.ToString();
        }

        private void cleanButton_Click(object sender, RoutedEventArgs e)
        {
            display.Text = "";
            firstOperand = 0;
            secondOperand = 0;
            currentOperator = string.Empty;
        }
    }
}
