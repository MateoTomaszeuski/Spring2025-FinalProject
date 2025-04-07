namespace Consilium.Maui.Controls;

public partial class CalculatorView : ContentView
{
    private string currentInput = "";
    private double previousValue = 0;
    private string operation = "";
    private bool isNewInput = false;

    public CalculatorView()
    {
        InitializeComponent();
    }

    void OnDigitClicked(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            if (isNewInput)
            {
                currentInput = "";
                isNewInput = false;
            }

            currentInput += button.Text;
            DisplayLabel.Text = currentInput;
        }
    }

    void OnOperatorClicked(object sender, EventArgs e)
    {
        if (double.TryParse(currentInput, out double value))
        {
            previousValue = value;
            currentInput = "";
        }

        if (sender is Button button)
        {
            operation = button.Text;
            isNewInput = true;
        }
    }

    void OnEqualsClicked(object sender, EventArgs e)
    {
        if (!double.TryParse(currentInput, out double currentValue))
            return;

        double result = 0;

        switch (operation)
        {
            case "+": result = previousValue + currentValue; break;
            case "-": result = previousValue - currentValue; break;
            case "*": result = previousValue * currentValue; break;
            case "/": result = currentValue != 0 ? previousValue / currentValue : 0; break;
        }

        DisplayLabel.Text = result.ToString();
        currentInput = result.ToString();
        isNewInput = true;
    }

    void OnClearClicked(object sender, EventArgs e)
    {
        currentInput = "";
        previousValue = 0;
        operation = "";
        DisplayLabel.Text = "0";
    }

    void OnBackspaceClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(currentInput))
        {
            currentInput = currentInput.Substring(0, currentInput.Length - 1);
            DisplayLabel.Text = string.IsNullOrEmpty(currentInput) ? "0" : currentInput;
        }
    }
}