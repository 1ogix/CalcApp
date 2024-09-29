using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Linq.Expressions;

namespace CalcApp
{
    public partial class MainWindow : Window
    {
        //Properties
        private string currentInput = "0";
        private string previousInput = "";
        private string operation = "";
        private bool isNewInput = true;
        private double result = 0;

        //Constructor
        public MainWindow()
        {
            InitializeComponent();
        }



        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string? number = button.Content?.ToString();

            if (number == null)
                return;

            // If it's a new input (after an operation or calculation), reset the display
            if (isNewInput)
            {
                currentInput = number;
                isNewInput = false;
            }
            else
            {
                // multiple digits to be inputted
                if (currentInput == "0")
                    currentInput = number;  // Replace default zero
                else
                    currentInput += number;
            }

            // Update the display with the current input
            ResultDisplay.Text = currentInput;
        }

        // Operator Button Click Events Methods
        private void OperatorButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string? operatorClicked = button.Content?.ToString();

            if (operatorClicked == null)
            {
                return;
            }


            // If an operation is already in progress, calculate the result before applying the new operator
            if (!isNewInput)
            {
                Calculate();
            }

            // Store the current input and the operator
            previousInput = currentInput;
            operation = operatorClicked;
            if (operation == "+/-")
            {
                ResultDisplay.Text = (double.Parse(currentInput) * -1).ToString();
                currentInput = (double.Parse(currentInput) * -1).ToString();
            }
            else if (operation == "%")
            {
                ResultDisplay.Text = (double.Parse(currentInput) / 100).ToString();
                currentInput = (double.Parse(currentInput) / 100).ToString();
            }
            else
            {
                ResultDisplay.Text = operation;
            }
            isNewInput = true;
        }

        // Calculate Method
        private void Calculate()
        {
            try
            {
                double firstNumber = double.Parse(previousInput);
                double secondNumber = double.Parse(currentInput);

                switch (operation)
                {
                    case "+":
                        result = firstNumber + secondNumber;
                        break;
                    case "-":
                        result = firstNumber - secondNumber;
                        break;
                    case "*":
                        result = firstNumber * secondNumber;
                        break;
                    case "+/-":
                        result = firstNumber * -1;
                        break;
                    case "/":
                        Console.WriteLine($"firstNumber: {firstNumber}, secondNumber: {secondNumber}"); //debugging
                        Console.WriteLine($"firstNumber: {firstNumber}, secondNumber: {secondNumber}"); // Debugging

                        if (secondNumber == 0)
                        {
                            throw new DivideByZeroException();
                            // // Instead of throwing, handle floating-point division by zero.
                            // result = firstNumber / secondNumber;

                            // if (double.IsInfinity(result) || double.IsNaN(result))
                            // {
                            //     ResultDisplay.Text = "either infinity or NAN";
                            //     throw new DivideByZeroException();
                            //     // Manually trigger exception if result is infinity or NaN
                            // }
                        }
                        else
                        {
                            result = firstNumber / secondNumber;
                        }
                        break;
                    default:
                        throw new InvalidOperationException("Invalid operator");
                }
            }
            catch (FormatException)
            {
                // Handle format issues, such as entering non-numeric values
                ResultDisplay.Text = "Error";
            }
            catch (DivideByZeroException)
            {
                // Handle division by zero explicitly
                ResultDisplay.Text = "Cannot divide by zero";
            }
            catch (Exception ex)
            {
                // Catch any other unforeseen exceptions
                ResultDisplay.Text = $"Error: {ex.Message}";
            }
        }


        // Result Button Click Event Method
        private void EqualsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Calculate();
                ResultDisplay.Text = result.ToString();
            }
            catch (FormatException)
            {
                // Handle potential input format issues
                ResultDisplay.Text = "Error";
            }

            // Prepare for the next input (reset state)
            currentInput = result.ToString();
            isNewInput = true;
        }

        // Clear Button Click Event Method
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            currentInput = "0";
            previousInput = "";
            operation = "";
            result = 0;
            isNewInput = true;
            ResultDisplay.Text = "0";
        }
    }
}
