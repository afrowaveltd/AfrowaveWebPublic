namespace Id.Models.ResultModels
{
	public class RegisterBrandResult
	{
		public bool BrandCreated { get; set; } = true;
		public int BrandId { get; set; } = 0;
		public bool LogoUploaded { get; set; } = true;
		public string ErrorMessage { get; set; } = string.Empty;
	}
}