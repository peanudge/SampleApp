namespace Domain.DTO
{
    public class CategoryDTO
    {
        public long CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<CategoryDTO> SubCategories { get; set; } = null!;
    }
}
