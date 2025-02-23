namespace Id.Pages.Helps
{
    public class Install_ConfirmPasswordModel(IStringLocalizer<Install_ConfirmPasswordModel> _t) : PageModel
    {
        private readonly IStringLocalizer<Install_ConfirmPasswordModel> t = _t;

        public string Title { get; set; } = "";
        public List<string> Lines = new List<string>();

		public void OnGet()
        {
            Title = t["Confirm password"];
            Lines.Add(t["Please confirm the password you have set."]);
        }
    }
}