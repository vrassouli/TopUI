using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Bootstrap.Components.Utilities;
using TopUI.Blazor.Core;
using TopUI.Blazor.Core.Abstractions;
using TopUI.Blazor.Core.Extensions;

namespace TopUI.Blazor.Bootstrap.Components;

public partial class ComboBox<TValue> : BootstrapFormControlComponent<TValue>, IChildrenContainerComponent<ComboBoxItem<TValue>>, ISelectionContainerComponent
{
    private readonly bool _isMultipleSelect;

    [Parameter] public SelectionMode Selection { get; set; } = SelectionMode.Single;
    [Parameter] public int SelectedIndex { get; set; }
    [Parameter] public EventCallback<int> SelectedIndexChanged { get; set; }
    [Parameter] public List<int> SelectedIndices { get; set; } = new();
    [Parameter] public EventCallback<IEnumerable<int>> SelectedIndicesChanged { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public FormControlSize Size { get; set; } = FormControlSize.Default;
    [Parameter] public bool AutoComplete { get; set; }
    [Parameter] public string? InputText { get; set; }
    [Parameter] public EventCallback<string?> InputTextChanged { get; set; }

    public List<ComboBoxItem<TValue>> Children { get; } = new();

    public void AddChild(ComboBoxItem<TValue> child)
    {
        Children.Add(child);
    }

    public void RemoveChild(ComboBoxItem<TValue> child)
    {
        Children.Remove(child);
    }

    //------------------

    /// <summary>
    /// Constructs an instance of <see cref="InputSelect{TValue}"/>.
    /// </summary>
    public ComboBox()
    {
        _isMultipleSelect = typeof(TValue).IsArray;
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (_isMultipleSelect)
            throw new NotImplementedException("Multiple selects are not supported yet!");

        if (!AutoComplete)
        {
            builder.OpenElement(0, "select");
            builder.AddMultipleAttributes(1, AdditionalAttributes);
            builder.AddAttribute(3, "multiple", _isMultipleSelect);

            if (_isMultipleSelect)
            {
                builder.AddAttribute(4, "value", BindConverter.FormatValue(CurrentValue)?.ToString());
                builder.AddAttribute(5, "onchange", EventCallback.Factory.CreateBinder<string?[]?>(this, SetCurrentValueAsStringArray, default));
            }
            else
            {
                builder.AddAttribute(6, "value", CurrentValueAsString);
                builder.AddAttribute(7, "onchange", EventCallback.Factory.CreateBinder<string?>(this, __value => CurrentValueAsString = __value, default));
            }

            builder.OpenComponent<CascadingValue<ComboBox<TValue>>>(8);
            builder.AddAttribute(10, nameof(CascadingValue<ComboBox<TValue>>.Value), this);
            builder.AddAttribute(11, nameof(CascadingValue<ComboBox<TValue>>.IsFixed), true);
            builder.AddAttribute(12, nameof(CascadingValue<ComboBox<TValue>>.ChildContent), ChildContent);
            builder.CloseComponent();

            builder.CloseElement();
        }
        else
        {
            builder.OpenElement(13, "input");
            builder.AddAttribute(14, "value", InputText);
            builder.AddAttribute(15, "oninput", EventCallback.Factory.Create(this, OnAutoCompleteInput));
            builder.AddAttribute(16, "onchange", EventCallback.Factory.Create(this, OnAutoCompleteChange));
            builder.AddMultipleAttributes(17, AdditionalAttributes);
            builder.AddAttribute(18, "list", $"{Id}_datalist");
            builder.CloseElement();

            builder.OpenElement(19, "datalist");
            builder.AddAttribute(20, "id", $"{Id}_datalist");

            builder.OpenComponent<CascadingValue<ComboBox<TValue>>>(21);
            builder.AddAttribute(22, nameof(CascadingValue<ComboBox<TValue>>.Value), this);
            builder.AddAttribute(23, nameof(CascadingValue<ComboBox<TValue>>.IsFixed), true);
            builder.AddAttribute(24, nameof(CascadingValue<ComboBox<TValue>>.ChildContent), ChildContent);
            builder.CloseComponent();

            builder.CloseElement();

        }
    }

    /// <inheritdoc />
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
        => this.TryParseSelectableValueFromString(value, out result, out validationErrorMessage);

    /// <inheritdoc />
    protected override string? FormatValueAsString(TValue? value)
    {
        // We special-case bool values because BindConverter reserves bool conversion for conditional attributes.
        if (typeof(TValue) == typeof(bool))
        {
            return (bool)(object)value! ? "true" : "false";
        }
        else if (typeof(TValue) == typeof(bool?))
        {
            return value is not null && (bool)(object)value ? "true" : "false";
        }

        return base.FormatValueAsString(value);
    }

    private void SetCurrentValueAsStringArray(string?[]? value)
    {
        CurrentValue = BindConverter.TryConvertTo<TValue>(value, CultureInfo.CurrentCulture, out var result)
            ? result
            : default;
    }

    //---------------------

    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            yield return c;

        if (AutoComplete)
            yield return "form-control";
        else
            yield return "form-select";

        if (Size != FormControlSize.Default)
            yield return $"form-select-{Size}".ToLower();
    }

    protected override async void OnCurrentValueChanged()
    {
        var item = Children.FirstOrDefault(x => EqualityComparer<TValue>.Default.Equals(x.Value, CurrentValue));

        if (item != null)
            await OnItemSelected(item);
    }

    private void OnAutoCompleteChange(ChangeEventArgs args)
    {
        var value = args.Value?.ToString();

        var childItem = Children.FirstOrDefault(x => Equals(x.Value, value));
        if (childItem != null)
            CurrentValueAsString = ValueToString(childItem.Value);
        else
            CurrentValueAsString = default;
    }

    private void OnAutoCompleteInput(ChangeEventArgs args)
    {
        SetInputText(args.Value?.ToString());
    }

    private void SetInputText(string? text)
    {
        InputText = text;
        InvokeAsync(async () => { await InputTextChanged.InvokeAsync(text); });
    }

    internal string? ValueToString(TValue? value)
    {
        return FormatValueAsString(value);
    }

    internal virtual async Task<int> OnItemSelected(ComboBoxItem<TValue> item)
        => await SelectableChildContainerHelper.OnItemSelected(this, item);

    internal bool IsSelected(ComboBoxItem<TValue> item)
        => SelectableChildContainerHelper.IsSelected(this, item);
}
