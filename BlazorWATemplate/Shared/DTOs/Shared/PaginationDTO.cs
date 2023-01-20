namespace BlazorWATemplate.Shared.DTOs.Shared
{
    public class PaginationDTO
    {
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 10;
        public string SearchQuery { get; set; } = "";
    }
}
