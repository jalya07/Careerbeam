namespace Careerbeam.Core.Entities;

public class CategoryTranslation
{
    public int Id { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public string LanguageCode { get; set; } = string.Empty; // "en", "az" или "ru"
    public string Name { get; set; } = string.Empty;
}