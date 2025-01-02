namespace Id.Pages.Helps
{
    public class Install_ApplicationWebsiteModel(IStringLocalizer<Install_ApplicationWebsiteModel> _t) : PageModel
    {
        private readonly IStringLocalizer<Install_ApplicationWebsiteModel> t = _t;
        public string Title { get; set; } = "";
        public List<string> Lines = [];

        public void OnGet()
        {
            Title = t["Application Website"];
            Lines.Add(t["The application website is the URL of the website where the application is hosted"]);
        }
    }
}