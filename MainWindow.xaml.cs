using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Lab5
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Calculation> Log { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Log = new ObservableCollection<Calculation>();
            this.DataContext = this;
        }
        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            string command = inputTextBox.Text;
            Calculator calculator = new Calculator();
            string[] commands = command.Split(' ');
            try
            {
                foreach (string c in commands)
                {
                    double number;
                    if (double.TryParse(c, out number))
                    {
                        calculator.AddOperand(number);
                    }
                    else if (c == "+" || c == "-" || c == "*" || c == "/") 
                    {
                        char operation = c.ToCharArray()[0];
                        calculator.AddOperation(operation);
                    }
                    else
                    {
                        throw new ArgumentException("Неверная комманда: " + c);
                    }
                }
                double result = calculator.GetResult();
                Log.Add(new Calculation { Operation = command, Result = result.ToString(), IsSuccess = true });
            }
            catch (Exception ex)
            {
                Log.Add(new Calculation { Operation = command, Result = "Ошибка: " + ex.Message, IsSuccess = false });
            }
        }
        private void inputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Calculate_Click(sender, e);
            }
        }
        private void resultListView_MouseDoubleClick(object sender, MouseButtonEventArgs e) 
        {
            if (resultListView.SelectedItem != null) 
            {
                Calculation selectedCalculation = (Calculation)resultListView.SelectedItem; 
                if (selectedCalculation.IsSuccess)
                {
                    var result = Convert.ToDouble(selectedCalculation.Result);
                }
            }
        }

        private void inputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }

    public class Calculator
    {
        private List<double> numbers;
        private List<char> operations;
        public Calculator()
        {
            numbers = new List<double>();
            operations = new List<char>();
        }
        public void AddOperand(double operand)
        {
            numbers.Add(operand);
        }
        public void AddOperation(char operation)
        {
            operations.Add(operation);
        }
        public double GetResult()
        {
            if (operations.Count == 0)
            {
                return numbers.Count > 0 ? numbers[0] : 0;
            }
            for (int i = 0; i < operations.Count; i++)
            {
                char operation = operations[i];
                double num1 = numbers[i];
                double num2 = numbers[i + 1];
                double result = 0;
                if (operation == '+')
                {
                    result = num1 + num2;
                }
                else if (operation == '-')
                {
                    result = num1 - num2;
                }
                else if (operation == '*')
                {
                    result = num1 * num2;
                }
                else if (operation == '/')
                {
                    if (num2 != 0)
                    {
                        result = num1 / num2;
                    }
                    else
                    {
                        throw new DivideByZeroException("Нельзя на 0 делить");
                    }
                }
                numbers[i + 1] = result;
            }

            return numbers[numbers.Count - 1];
        }
    }

    public class Calculation
    {
        public string Operation { get; set; }
        public string Result { get; set; }
        public bool IsSuccess { get; set; }
    }
}