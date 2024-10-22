namespace WinHub.ApiService.Contracts;

public class ContestResponse
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public DateTime StartDateTime { get; set; }
	public DateTime EndDateTime { get; set; }
	public DateTime ContestDateTime { get; set; }
}
