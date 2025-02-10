namespace Id.Models.ResultModels
{
	public class CheckInputResult
	{
		public bool Success { get; set; } = true;
		public List<string> Errors { get; set; } = [];
	}
}