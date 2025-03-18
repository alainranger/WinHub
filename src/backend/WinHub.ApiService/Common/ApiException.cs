namespace WinHub.ApiService.Common
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "<Pending>")]
	public class ApiException : Exception
	{
		public ApiException(string message) : base(message)
		{
		}

		public ApiException(string message, Exception innerException) : base(message, innerException)
		{
		}

		public ApiException()
		{
		}
	}
}
