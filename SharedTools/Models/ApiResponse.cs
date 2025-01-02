namespace SharedTools.Models
{
    public class ApiResponse<T>
    {
        public bool Successful { get; set; } = true;
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}