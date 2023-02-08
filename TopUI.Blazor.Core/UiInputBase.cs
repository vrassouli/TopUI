﻿using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Core;

public abstract class UiInputBase<TValue> : UiComponent, IDisposable
{
    private readonly EventHandler<ValidationStateChangedEventArgs> _validationStateChangedHandler;
    private bool _hasInitializedParameters;
    private bool _previousParsingAttemptFailed;
    private ValidationMessageStore? _parsingValidationMessages;
    private Type? _nullableUnderlyingType;

    [CascadingParameter] private EditContext? CascadedEditContext { get; set; }

    /// <summary>
    /// Gets or sets the value of the input. This should be used with two-way binding.
    /// </summary>
    /// <example>
    /// @bind-Value="model.PropertyName"
    /// </example>
    [Parameter]
    public TValue? Value { get; set; }

    /// <summary>
    /// Gets or sets a callback that updates the bound value.
    /// </summary>
    [Parameter] public EventCallback<TValue> ValueChanged { get; set; }

    /// <summary>
    /// Gets or sets an expression that identifies the bound value.
    /// </summary>
    [Parameter] public Expression<Func<TValue>>? ValueExpression { get; set; }

    /// <summary>
    /// Gets or sets the display name for this field.
    /// <para>This value is used when generating error messages when the input value fails to parse correctly.</para>
    /// </summary>
    [Parameter] public string? DisplayName { get; set; }

    /// <summary>
    /// Gets the associated <see cref="EditForm.EditContext"/>.
    /// This property is uninitialized if the input does not have a parent <see cref="EditForm"/>.
    /// </summary>
    protected EditContext EditContext { get; set; } = default!;

    /// <summary>
    /// Gets the <see cref="FieldIdentifier"/> for the bound value.
    /// This property is uninitialized if the input does not have data bound value/>.
    /// </summary>
    protected internal FieldIdentifier FieldIdentifier { get; set; }

    protected Type EditorDataType => _nullableUnderlyingType ?? typeof(TValue);
    protected bool IsNumeric
    {
        get
        {
            if (EditorDataType == typeof(int) ||
                EditorDataType == typeof(long) ||
                EditorDataType == typeof(float) ||
                EditorDataType == typeof(double) ||
                EditorDataType == typeof(decimal))
                return true;

            return false;
        }
    }

    /// <summary>
    /// Gets or sets the current value of the input.
    /// </summary>
    protected TValue? CurrentValue
    {
        get => Value;
        set
        {
            var hasChanged = !EqualityComparer<TValue>.Default.Equals(value, Value);
            if (hasChanged)
            {
                OnCurrentValueChanging(value);

                Value = value;
                _ = ValueChanged.InvokeAsync(Value);

                if (!string.IsNullOrEmpty(FieldIdentifier.FieldName))
                    EditContext?.NotifyFieldChanged(FieldIdentifier);

                OnCurrentValueChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets the current value of the input, represented as a string.
    /// </summary>
    protected string? CurrentValueAsString
    {
        get => FormatValueAsString(CurrentValue);
        set
        {
            _parsingValidationMessages?.Clear();

            bool parsingFailed;

            if (_nullableUnderlyingType != null && string.IsNullOrEmpty(value))
            {
                // Assume if it's a nullable type, null/empty inputs should correspond to default(T)
                // Then all subclasses get nullable support almost automatically (they just have to
                // not reject Nullable<T> based on the type itself).
                parsingFailed = false;
                CurrentValue = default!;
            }
            else if (TryParseValueFromString(value, out var parsedValue, out var validationErrorMessage))
            {
                parsingFailed = false;
                CurrentValue = parsedValue!;
            }
            else
            {
                parsingFailed = true;

                // EditContext may be null if the input is not a child component of EditForm.
                if (EditContext is not null)
                {
                    _parsingValidationMessages ??= new ValidationMessageStore(EditContext);
                    _parsingValidationMessages.Add(FieldIdentifier, validationErrorMessage);

                    // Since we're not writing to CurrentValue, we'll need to notify about modification from here
                    EditContext.NotifyFieldChanged(FieldIdentifier);
                }
            }

            // We can skip the validation notification if we were previously valid and still are
            if (parsingFailed || _previousParsingAttemptFailed)
            {
                EditContext?.NotifyValidationStateChanged();
                _previousParsingAttemptFailed = parsingFailed;
            }
        }
    }

    /// <summary>
    /// Constructs an instance of <see cref="InputBase{TValue}"/>.
    /// </summary>
    protected UiInputBase()
    {
        _validationStateChangedHandler = OnValidateStateChanged;
    }

    /// <summary>
    /// Formats the value as a string. Derived classes can override this to determine the formating used for <see cref="CurrentValueAsString"/>.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <returns>A string representation of the value.</returns>
    protected virtual string? FormatValueAsString(TValue? value) => value?.ToString();

    protected virtual void OnCurrentValueChanged()
    {
    }

    protected virtual void OnCurrentValueChanging(TValue? value)
    {
    }

    /// <summary>
    /// Parses a string to create an instance of <typeparamref name="TValue"/>. Derived classes can override this to change how
    /// <see cref="CurrentValueAsString"/> interprets incoming values.
    /// </summary>
    /// <param name="value">The string value to be parsed.</param>
    /// <param name="result">An instance of <typeparamref name="TValue"/>.</param>
    /// <param name="validationErrorMessage">If the value could not be parsed, provides a validation error message.</param>
    /// <returns>True if the value could be parsed; otherwise false.</returns>
    protected abstract bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage);

    ///// <summary>
    ///// Gets a CSS class string that combines the <c>class</c> attribute and and a string indicating
    ///// the status of the field being edited (a combination of "modified", "valid", and "invalid").
    ///// Derived components should typically use this value for the primary HTML element's 'class' attribute.
    ///// </summary>
    //protected string CssClass
    //{
    //    get
    //    {
    //        var fieldClass = EditContext?.FieldCssClass(FieldIdentifier);
    //        return AttributeUtilities.CombineClassNames(AdditionalAttributes, fieldClass) ?? string.Empty;
    //    }
    //}

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        parameters.SetParameterProperties(this);

        if (!_hasInitializedParameters)
        {
            // This is the first run
            // Could put this logic in OnInit, but its nice to avoid forcing people who override OnInit to call base.OnInit()

            //if (ValueExpression == null)
            //{
            //    throw new InvalidOperationException($"{GetType()} requires a value for the 'ValueExpression' " +
            //        $"parameter. Normally this is provided automatically when using 'bind-Value'.");
            //}

            if (ValueExpression != null)
                FieldIdentifier = FieldIdentifier.Create(ValueExpression);

            if (CascadedEditContext != null)
            {
                EditContext = CascadedEditContext;
                EditContext.OnValidationStateChanged += _validationStateChangedHandler;
            }

            _nullableUnderlyingType = Nullable.GetUnderlyingType(typeof(TValue));
            _hasInitializedParameters = true;
        }
        else if (CascadedEditContext != EditContext)
        {
            // Not the first run

            // We don't support changing EditContext because it's messy to be clearing up state and event
            // handlers for the previous one, and there's no strong use case. If a strong use case
            // emerges, we can consider changing this.
            throw new InvalidOperationException($"{GetType()} does not support changing the {nameof(EditContext)} dynamically.");
        }

        UpdateAdditionalValidationAttributes();

        // For derived components, retain the usual lifecycle with OnInit/OnParametersSet/etc.
        return base.SetParametersAsync(ParameterView.Empty);
    }

    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            yield return c;

        if (!string.IsNullOrEmpty(FieldIdentifier.FieldName))
        {
            var fieldClass = EditContext?.FieldCssClass(FieldIdentifier);
            if (!string.IsNullOrEmpty(fieldClass))
                yield return fieldClass;
        }
    }

    private void OnValidateStateChanged(object? sender, ValidationStateChangedEventArgs eventArgs)
    {
        UpdateAdditionalValidationAttributes();

        StateHasChanged();
    }

    private void UpdateAdditionalValidationAttributes()
    {
        if (EditContext is null)
        {
            return;
        }

        var hasAriaInvalidAttribute = AdditionalAttributes.ContainsKey("aria-invalid");
        if (EditContext.GetValidationMessages(FieldIdentifier).Any())
        {
            if (hasAriaInvalidAttribute)
            {
                // Do not overwrite the attribute value
                return;
            }

            AddAttribute("aria-invalid", "true");

            //if (ConvertToDictionary(AdditionalAttributes, out var additionalAttributes))
            //{
            //    AdditionalAttributes = additionalAttributes;
            //}

            //// To make the `Input` components accessible by default
            //// we will automatically render the `aria-invalid` attribute when the validation fails
            //// value must be "true" see https://www.w3.org/TR/wai-aria-1.1/#aria-invalid
            //additionalAttributes["aria-invalid"] = "true";
        }
        else if (hasAriaInvalidAttribute)
        {
            // No validation errors. Need to remove `aria-invalid` if it was rendered already
            RemoveAttribute("aria-invalid", "true");

            //if (AdditionalAttributes!.Count == 1)
            //{
            //    // Only aria-invalid argument is present which we don't need any more
            //    AdditionalAttributes = null;
            //}
            //else
            //{
            //    if (ConvertToDictionary(AdditionalAttributes, out var additionalAttributes))
            //    {
            //        AdditionalAttributes = additionalAttributes;
            //    }

            //    additionalAttributes.Remove("aria-invalid");
            //}
        }
    }

    ///// <summary>
    ///// Returns a dictionary with the same values as the specified <paramref name="source"/>.
    ///// </summary>
    ///// <returns>true, if a new dictionary with copied values was created. false - otherwise.</returns>
    //private static bool ConvertToDictionary(IReadOnlyDictionary<string, object>? source, out Dictionary<string, object> result)
    //{
    //    var newDictionaryCreated = true;
    //    if (source == null)
    //    {
    //        result = new Dictionary<string, object>();
    //    }
    //    else if (source is Dictionary<string, object> currentDictionary)
    //    {
    //        result = currentDictionary;
    //        newDictionaryCreated = false;
    //    }
    //    else
    //    {
    //        result = new Dictionary<string, object>();
    //        foreach (var item in source)
    //        {
    //            result.Add(item.Key, item.Value);
    //        }
    //    }

    //    return newDictionaryCreated;
    //}

    /// <inheritdoc/>
    protected virtual void Dispose(bool disposing)
    {
    }

    void IDisposable.Dispose()
    {
        // When initialization in the SetParametersAsync method fails, the EditContext property can remain equal to null
        if (EditContext is not null)
        {
            EditContext.OnValidationStateChanged -= _validationStateChangedHandler;
        }

        Dispose(disposing: true);
    }
}
