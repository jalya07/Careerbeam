namespace Careerbeam.Core.Entities;

public class Category
{
    public int Id { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<CategoryTranslation> Translations { get; set; } = new List<CategoryTranslation>();
}