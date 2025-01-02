namespace Id.Pages.Helps
{
    public class Install_ConfirmPasswordModel : PageModel
    {
        private readonly IStringLocalizer<Install_ConfirmPasswordModel> t;

        public string Title { get; set; } = "";
        public List<string> Lines = new List<string>();

        public Install_ConfirmPasswordModel(IStringLocalizer<Install_ConfirmPasswordModel> _t)
        {
            t = _t;
        }

        public void OnGet()
        {
            Title = t["Confirm password"];
            Lines.Add(t["Please confirm the password you have set."]);
        }
    }
}