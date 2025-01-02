namespace Id.Pages.Helps
{
    public class Install_ApplicationNameModel : PageModel
    {
        private readonly IStringLocalizer<Install_ApplicationNameModel> t;

        public Install_ApplicationNameModel(IStringLocalizer<Install_ApplicationNameModel> _t)
        {
            t = _t;
        }

        public string Title { get; set; } = "";
        public List<string> Lines = new List<string>();

        public void OnGet()
        {
            Title = t["Application name"];

            Lines.Add(t["The application name is the name of the application that will be displayed in the browser"]);
            Lines.Add(t["The application name is also used in the email templates"]);
            Lines.Add(t["The application name should be unique and descriptive"]);
            Lines.Add(t["The application name must be at least 3 characters long"]);
            Lines.Add(t["The application name must be shorter than 32 characters"]);
        }
    }
}