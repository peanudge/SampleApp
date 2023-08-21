namespace Domain.Models;

public class Category
{
    public long CategoryId { get; set; }
    public string Name { get; set; } = null!;
    public long? ParentCategoryId { get; set; }
    public Category ParentCategory { get; set; } = null!;
    public List<Category> SubCategories { get; set; } = null!;
}
