using System.ComponentModel.DataAnnotations;

namespace WinHub.Blazor.DataAnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
internal sealed class DateGreaterThanAttribute : ValidationAttribute
{
	public string ComparisonProperty { get; }

	public DateGreaterThanAttribute(string comparisonProperty)
	{
		if (string.IsNullOrEmpty(comparisonProperty))
			throw new ArgumentException("Comparison property name cannot be null or empty.", nameof(comparisonProperty));

		ComparisonProperty = comparisonProperty;
	}

	protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
	{
		var currentValue = (DateTime?)value;

		var comparisonProperty = validationContext.ObjectType.GetProperty(ComparisonProperty);
		if (comparisonProperty == null)
			return new ValidationResult($"Unknown property: {ComparisonProperty}");

		var comparisonValue = (DateTime?)comparisonProperty.GetValue(validationContext.ObjectInstance);

		if (currentValue.HasValue && comparisonValue.HasValue && currentValue <= comparisonValue)
		{
			return new ValidationResult(ErrorMessage ?? $"The date must be greater than {ComparisonProperty}.");
		}

		return ValidationResult.Success!;
	}
}
