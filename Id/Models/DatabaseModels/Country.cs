using System.ComponentModel.DataAnnotations;

namespace Id.Models.DatabaseModels
{
    public class Country
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Dial_code { get; set; } = string.Empty;
        public string Emoji { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;

        public List<UserAddress> Addresses { get; set; } = new();
    }
}