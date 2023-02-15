using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Bootstrap.Components.Misc.CalendarComponent;

namespace TopUI.Blazor.Bootstrap.Components;

public partial class CalendarView<TValue>
{
    private CultureInfo _culture = default!;
    DateTime? _viewDate;

    [Parameter] public CultureInfo? Culture { get; set; }
    [Parameter] public EventCallback<DateTime?> SelectedDateChanged { get; set; }
    [Parameter] public DateTime? MaxDate { get; set; }
    [Parameter] public DateTime? MinDate { get; set; }
    [Parameter] public Func<DateTime, string>? DateClass { get; set; }

    [Inject] private IStringLocalizer<Misc.CalendarComponent.Resources.CalendarView> Localizer { get; set; } = default!;

    private DateTime? InternalDate
    {
        get
        {
            if (Value is DateOnly dateOnly)
                return dateOnly.ToDateTime(TimeOnly.MinValue);
            else if (Value is DateTime dateTime)
                return dateTime.Date;

            return null;
        }
    }

    private DateTime ViewDate
    {
        get
        {
            if (_viewDate == null)
            {
                if (Value is DateTime dateTime)
                    _viewDate = GetMonthStart(dateTime);
                else if (Value is DateOnly dateOnly)
                    _viewDate = GetMonthStart(dateOnly);
                else
                    _viewDate = GetMonthStart(DateTime.Now);
            }

            return _viewDate.Value;
        }
    }

    private Calendar Calendar => _culture.Calendar;

    private int DaysToBeginningOfWeek
    {
        get
        {
            return ((int)ViewDate.DayOfWeek - (int)_culture.DateTimeFormat.FirstDayOfWeek + 7) % 7;
        }
    }

    protected override void OnParametersSet()
    {
        _viewDate = null;
        _culture = Culture ?? CultureInfo.CurrentCulture;

        base.OnParametersSet();
    }

    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            yield return c;

        yield return "tui-calendar";
    }


    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        if (EditorDataType == typeof(DateTime) || EditorDataType == typeof(DateOnly))
        {
            if (BindConverter.TryConvertTo(value, CultureInfo.InvariantCulture, out result))
            {
                validationErrorMessage = null;
                return true;
            }
        }

        result = default;
        validationErrorMessage = $"Data type '{EditorDataType.Name}' not supported.";
        return false;
    }

    private DateTime GetMonthStart(DateTime date)
    {
        return Calendar.ToDateTime(Calendar.GetYear(date), Calendar.GetMonth(date), 1, 0, 0, 0, 0);
    }

    private DateTime GetMonthStart(DateOnly date)
    {
        return Calendar.ToDateTime(Calendar.GetYear(date.ToDateTime(TimeOnly.MinValue)), Calendar.GetMonth(date.ToDateTime(TimeOnly.MinValue)), 1, 0, 0, 0, 0);
    }

    private void SelectDate(DateTime date)
    {
        if (BindConverter.TryConvertTo<TValue>(date.ToString(), null, out var result))
            CurrentValue = result;

        // force view date to be re-calculated
        _viewDate = null;
    }

    private void OnGotoMonth(object? selectedMonth)
    {
        if (int.TryParse(selectedMonth?.ToString(), out int month))
        {
            var y = Calendar.GetYear(ViewDate);
            var d = Calendar.GetDayOfMonth(ViewDate);

            _viewDate = GetMonthStart(Calendar.ToDateTime(y, month, d, 0, 0, 0, 0));
        }
    }

    private void OnGotoYear(object? selectedYear)
    {
        if (int.TryParse(selectedYear?.ToString(), out int year))
        {
            var m = Calendar.GetMonth(ViewDate);

            _viewDate = GetMonthStart(Calendar.ToDateTime(year, m, 1, 0, 0, 0, 0));
        }
    }

    private void OnPrev()
    {
        if (_viewDate == null)
            return;

        var date = Calendar.AddMonths(_viewDate.Value, -1);

        _viewDate = GetMonthStart(date);
    }

    private void OnNext()
    {
        if (_viewDate == null)
            return;

        var date = Calendar.AddMonths(_viewDate.Value, 1);

        _viewDate = GetMonthStart(date);
    }
}
