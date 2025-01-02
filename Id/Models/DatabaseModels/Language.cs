using System.ComponentModel.DataAnnotations;

namespace Id.Models.DatabaseModels
{
    public class Language
    {
        [Key]
        public int Id { get; set; }

        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Native { get; set; } = string.Empty;
        public int Rtl { get; set; } = 0;
    }
}