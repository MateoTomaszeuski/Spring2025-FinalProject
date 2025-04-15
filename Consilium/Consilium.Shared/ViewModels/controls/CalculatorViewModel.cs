using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Consilium.Shared.ViewModels.controls;

public partial class CalculatorViewModel : ObservableObject {
    [ObservableProperty]
    private string displayText = "0";

    private string currentInput = "";
    private double previousValue = 0;
    private string operation = "";
    private bool isNewInput = false;
    private string expression = "";


    [RelayCommand]
    public void Digit(string digit) {
        if (isNewInput) {
            currentInput = "";
            expression = "";
            isNewInput = false;
        }
        currentInput += digit;
        expression += digit;
        DisplayText = expression;
    }

    [RelayCommand]
    public void Operator(string op) {
        if (double.TryParse(currentInput, out double value)) {
            previousValue = value;
            currentInput = "";
        }
        operation = op;
        expression += op;
        DisplayText = expression;
    }

    [RelayCommand]
    public void Equals() {
        if (!double.TryParse(currentInput, out double currentValue))
            return;

        double result = 0;
        switch (operation) {
            case "+":
                result = previousValue + currentValue;
                break;
            case "-":
                result = previousValue - currentValue;
                break;
            case "*":
                result = previousValue * currentValue;
                break;
            case "/":
                // Check for division by zero
                result = currentValue != 0 ? previousValue / currentValue : 0;
                break;
        }
        DisplayText = result.ToString();
        currentInput = result.ToString();
        expression = result.ToString();
        isNewInput = true;
    }

    [RelayCommand]
    public void Clear() {
        currentInput = "";
        previousValue = 0;
        operation = "";
        expression = "";
        DisplayText = "0";
    }

    [RelayCommand]
    public void Backspace() {
        if (!string.IsNullOrEmpty(currentInput)) {
            currentInput = currentInput.Substring(0, currentInput.Length - 1);
            expression = expression.Substring(0, expression.Length - 1);
            DisplayText = string.IsNullOrEmpty(expression) ? "0" : expression;
        }
    }
}