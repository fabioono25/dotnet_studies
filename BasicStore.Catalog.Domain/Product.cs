﻿using BasicStore.Core.DomainObjects;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;

namespace BasicStore.Catalog.Domain
{
    // aggregation root
    public class Product : Entity, IAggregateRoot
    {
        public string Name { get; set; }
        public string Description { get; private set; }
        public bool Active { get; private set; }
        public decimal Value { get; private set; }
        public DateTime CreationDate { get; private set; }
        public string Image { get; private set; }
        public int QuantityStock { get; private set; }

        // adaptation for EF (CategoryId)
        public Guid CategoryId { get; private set; }

        public Category Category { get; private set; }

        public Product(string name, string description, bool active, decimal value, Guid categoryId, DateTime creationDate, string image)
        {
            CategoryId = categoryId;
            Name = name;
            Description = description;
            Active = active;
            Value = value;
            CreationDate = creationDate;
            Image = image;

            Validate();
        }

        // Ad-Hoc setters
        public void Activate() => Active = true;

        public void Inactivate() => Active = false;

        public void UpdateCategory(Category category)
        {
            Category = category;
            CategoryId = category.Id;
        }

        public void UpdateDescription(string description)
        {
            AssertionConcern.ValidateIfEmpty(Name, "Description required");
            Description = description;
        }

        public void DecreaseStock(int quantity)
        {
            if (quantity < 0) quantity *= -1;
            // if (!PossuiEstoque(quantity)) throw new DomainException("Insufficient stock");
            QuantityStock -= quantity;
        }

        public void RestoreStock(int quantity)
        {
            QuantityStock += quantity;
        }

        public bool HasStock(int quantity)
        {
            return QuantityStock >= quantity;
        }

        public void Validate()
        {
            AssertionConcern.ValidateIfEmpty(Name, "Name required");
            AssertionConcern.ValidateIfEmpty(Description, "Description required");
            AssertionConcern.ValidateIfEqual(CategoryId, Guid.Empty, "CategoryId required");
            AssertionConcern.ValidateIfLessThan(Value, 1, "Value must be more than 0");
            AssertionConcern.ValidateIfEmpty(Image, "Image required");
        }
    }

    // serves product
    public class Category : Entity
    {
        public string Name { get; private set; }
        public int Code { get; private set; }

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
}
