namespace Id.Pages.Helps
{
    public class Install_ApplicationDescriptionModel(IStringLocalizer<Install_ApplicationDescriptionModel> _t) : PageModel
    {
        private readonly IStringLocalizer<Install_ApplicationDescriptionModel> t = _t;

        public string Title { get; set; } = string.Empty;
        public List<string> Lines { get; set; } = new();

        public void OnGet()
        {
            Title = t["Application Description"];
            Lines.Add(t["The application description is a short text that describes the purpose of application."]);
            Lines.Add(t["This description will be displayed in the application list."]);
            Lines.Add(t["The description is not required, but it is recommended to fill it in."]);
        }
    }
}