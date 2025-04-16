using System.Globalization;

namespace Consilium.Maui.Controls;

public partial class Calendar : ContentView {
    private DateTime _currentDate;

    public Calendar() {
        InitializeComponent();
        _currentDate = DateTime.Today;
        BuildCalendar(_currentDate);
    }

    private void BuildCalendar(DateTime targetDate) {
        CalendarGrid.Children.Clear();
        CalendarGrid.RowDefinitions.Clear();
        CalendarGrid.ColumnDefinitions.Clear();

        // Set the title
        MonthYearLabel.Text = targetDate.ToString("MMMM yyyy");

        // Define 7 columns for 7 days of the week
        for (int i = 0; i < 7; i++) {
            CalendarGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
        }

        // Day names, shifted based on first day of week
        DayOfWeek firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
        string[] dayNames = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;
        string[] shiftedDayNames = dayNames
            .Skip((int)firstDayOfWeek)
            .Concat(dayNames.Take((int)firstDayOfWeek))
            .ToArray();

        // Add weekday headers
        for (int i = 0; i < 7; i++) {
            var label = new Label {
                Text = shiftedDayNames[i],
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center
            };
            CalendarGrid.Children.Add(label);
            Grid.SetColumn(label, i);
            Grid.SetRow(label, 0);
        }

        // Add first row for day headers
        CalendarGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        // Calendar date generation
        DateTime firstOfMonth = new DateTime(targetDate.Year, targetDate.Month, 1);
        int daysInMonth = DateTime.DaysInMonth(targetDate.Year, targetDate.Month);

        // Calculate the correct column offset for the first day of the month
        int startDay = ((int)firstOfMonth.DayOfWeek - (int)firstDayOfWeek + 7) % 7;

        int currentRow = 1;
        int currentColumn = startDay;

        CalendarGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        for (int day = 1; day <= daysInMonth; day++) {
            if (currentColumn == 7) {
                currentColumn = 0;
                currentRow++;
                CalendarGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }

            var label = new Label {
                Text = day.ToString(),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = (targetDate.Year == DateTime.Today.Year &&
                                   targetDate.Month == DateTime.Today.Month &&
                                   day == DateTime.Today.Day)
                                    ? Color.FromArgb("#719984")
                                    : Colors.Transparent,
                Padding = 10
            };

            CalendarGrid.Children.Add(label);
            Grid.SetColumn(label, currentColumn);
            Grid.SetRow(label, currentRow);
            currentColumn++;
        }
    }

    private void OnPrevMonthClicked(object sender, EventArgs e) {
        _currentDate = _currentDate.AddMonths(-1);
        BuildCalendar(_currentDate);
    }

    private void OnNextMonthClicked(object sender, EventArgs e) {
        _currentDate = _currentDate.AddMonths(1);
        BuildCalendar(_currentDate);
    }
}