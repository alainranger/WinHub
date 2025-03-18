namespace WinHub.Shared.Common;

/// <summary>
/// Initializes a new instance of the <see cref="ApiError"/> class.
/// </summary>
/// <param name="code">The error code.</param>
/// <param name="message">The error message.</param>
public sealed class ApiError(string code, string message) : IEquatable<ApiError>
{

	/// <summary>
	/// Gets the error code.
	/// </summary>
	public string Code { get; } = code;

	/// <summary>
	/// Gets the error message.
	/// </summary>
	public string Message { get; } = message;

	public static implicit operator string(ApiError error) => error?.Code ?? string.Empty;

	public static bool operator ==(ApiError a, ApiError b)
	{
		if (a is null && b is null)
		{
			return true;
		}

		if (a is null || b is null)
		{
			return false;
		}

		return a.Equals(b);
	}

	public static bool operator !=(ApiError a, ApiError b) => !(a == b);

	/// <inheritdoc />
	public bool Equals(ApiError? other) => other is not null && GetAtomicValues().SequenceEqual(other.GetAtomicValues());

	/// <inheritdoc />
	public override bool Equals(object? obj)
	{
		if (obj == null)
		{
			return false;
		}

		if (GetType() != obj.GetType())
		{
			return false;
		}

		if (obj is not ApiError valueObject)
		{
			return false;
		}

		return GetAtomicValues().SequenceEqual(valueObject.GetAtomicValues());
	}

	/// <inheritdoc />
	public override int GetHashCode()
	{
		HashCode hashCode = default;

		foreach (var obj in GetAtomicValues())
		{
			hashCode.Add(obj);
		}
		return hashCode.ToHashCode();
	}

	public override string ToString() => Code ?? string.Empty;

	/// <summary>
	/// Gets the atomic values of the value object.
	/// </summary>
	/// <returns>The collection of objects representing the value object values.</returns>
	private IEnumerable<object> GetAtomicValues()
	{
		yield return Code;
		yield return Message;
	}

	/// <summary>
	/// Gets the empty error instance.
	/// </summary>
	internal static ApiError None => new(string.Empty, string.Empty);
}
