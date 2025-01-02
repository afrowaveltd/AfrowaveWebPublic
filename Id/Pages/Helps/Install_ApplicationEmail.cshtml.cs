namespace Id.Pages.Helps
{
    public class Install_ApplicationEmailModel(IStringLocalizer<Install_ApplicationEmailModel> _t) : PageModel
    {
        private readonly IStringLocalizer<Install_ApplicationEmailModel> t = _t;
        public string Title { get; set; } = "";
        public List<string> Lines = new List<string>();

        public void OnGet()
        {
            Title = t["Application Email"];
            Lines.Add(t["The application email is the contact address for contacting administrators or the support of the application"]);
        }
    }
}