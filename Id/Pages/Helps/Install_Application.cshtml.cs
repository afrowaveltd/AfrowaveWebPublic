namespace Id.Pages.Helps
{
    public class Install_ApplicationModel : PageModel
    {
        private readonly IStringLocalizer<Install_ApplicationModel> t;

        public string Title { get; set; } = "";
        public List<string> Lines = new List<string>();

        public Install_ApplicationModel(IStringLocalizer<Install_ApplicationModel> _t)
        {
            t = _t;
        }

        public void OnGet()
        {
            Title = t["Create Authenticator Application"];

            Lines.Add(t["To create an authenticator application, you need to fill the form properly and click on the 'Next' button"]);
            Lines.Add(t["For the security reason, use the dedicated email address for the administrator account"]);
            Lines.Add(t["The administrator account is the first account that will be created"]);
        }
    }
}