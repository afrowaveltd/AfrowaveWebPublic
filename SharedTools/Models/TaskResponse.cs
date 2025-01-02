namespace SharedTools.Models
{
    public class TaskResponse
    {
        public bool Successful { get; set; } = true;
        public List<string>? Errors { get; set; }
    }
}