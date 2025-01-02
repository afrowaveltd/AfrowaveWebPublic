namespace SharedTools.Models
{
    public class LibretranslateLanguage
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public List<string> Targets { get; set; } = [];
    }
}