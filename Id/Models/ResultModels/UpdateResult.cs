namespace Id.Models.ResultModels
{
	public class UpdateResult
	{
		public bool Success { get; set; } = true;
		public List<string> Errors { get; set; } = [];
		public Dictionary<string, string> UpdatedValues { get; set; } = [];
	}
}