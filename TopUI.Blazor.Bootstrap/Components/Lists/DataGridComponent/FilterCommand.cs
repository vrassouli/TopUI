using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components;

public class FilterCommand
{ 
    public List<FieldFilter>? Filters { get; set; }
}

public abstract class FieldFilter
{
    public string Field { get; set; }
    public FilterOperator Operator { get; set; } = FilterOperator.EqualsTo;
    public LogincalOperator Logical { get; set; } = LogincalOperator.And;
    public abstract object? Value { get; }
    public FieldFilter (string field)
    {
        Field = field;
    }
}

public class FieldFilter<T> : FieldFilter
{
    public FieldFilter(string field) : base(field)
    {
    }

    public override object? Value { get => InternalValue; }
    internal T InternalValue { get; set; } = default!;
}

public enum FilterOperator
{
    [Display(Name = "Contains")]
    Contains,

    [Display(Name = "Starts with")]
    StartsWith,

    [Display(Name = "Ends with")]
    EndsWith,

    [Display(Name = "Equals to")]
    EqualsTo,

    [Display(Name = "Greater than")]
    GreaterThan,

    [Display(Name = "Greater than or equal")]
    GreaterThanOrEqual,

    [Display(Name = "Less than")]
    LessThan,

    [Display(Name = "Less than or equal")]
    LessThanOrEqual,
}

public enum LogincalOperator
{
    And,
    Or
}