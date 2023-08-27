using BasicStore.Core.DomainObjects;

namespace BasicStore.Catalog.Domain;



// serves product
public class Category : Entity
{
    public string Name { get; private set; }
    public int Code { get; private set; }

    // because of EF
    public ICollection<Product> Products{ get; set; }

    // because of EF
    protected Category()
    {
        
    }

    public Category(string name, int code)
    {
        Name = name;
        Code = code;

        Validate();
    }

    public override string ToString()
    {
        return $"{Name} - {Code}";
    }

    public void Validate()
    {
        AssertionConcern.ValidateIfEmpty(Name, "Name required");
        AssertionConcern.ValidateIfEqual(Code, 0, "Code must not be 0");
    }
}