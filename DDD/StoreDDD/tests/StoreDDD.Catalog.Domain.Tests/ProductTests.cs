using BasicStore.Catalog.Domain;
using BasicStore.Core.DomainObjects;

namespace StoreDDD.Catalog.Domain.Tests
{
    public class ProductTests
    {
        [Fact]
        public void Product_Validate_ShouldReturnExceptions()
        {
            // Arrange & Act & Assert

            var ex = Assert.Throws<DomainException>(() =>
                new Product(string.Empty, "Descricao", false, 100, Guid.NewGuid(), DateTime.Now, "Imagem", new Dimensions(1, 1, 1))
            );

            Assert.Equal("O campo Nome do produto n�o pode estar vazio", ex.Message);

            ex = Assert.Throws<DomainException>(() =>
                new Product("Nome", string.Empty, false, 100, Guid.NewGuid(), DateTime.Now, "Imagem", new Dimensions(1, 1, 1))
            );

            Assert.Equal("O campo Descricao do produto n�o pode estar vazio", ex.Message);

            ex = Assert.Throws<DomainException>(() =>
                new Product("Nome", "Descricao", false, 0, Guid.NewGuid(), DateTime.Now, "Imagem", new Dimensions(1, 1, 1))
            );

            Assert.Equal("O campo Valor do produto n�o pode se menor igual a 0", ex.Message);

            ex = Assert.Throws<DomainException>(() =>
                new Product("Nome", "Descricao", false, 100, Guid.Empty, DateTime.Now, "Imagem", new Dimensions(1, 1, 1))
            );

            Assert.Equal("O campo CategoriaId do produto n�o pode estar vazio", ex.Message);

            ex = Assert.Throws<DomainException>(() =>
                new Product("Nome", "Descricao", false, 100, Guid.NewGuid(), DateTime.Now, string.Empty, new Dimensions(1, 1, 1))
            );

            Assert.Equal("O campo Imagem do produto n�o pode estar vazio", ex.Message);

            ex = Assert.Throws<DomainException>(() =>
                new Product("Nome", "Descricao", false, 100, Guid.NewGuid(), DateTime.Now, "Imagem", new Dimensions(0, 1, 1))
            );

            Assert.Equal("O campo Altura n�o pode ser menor ou igual a 0", ex.Message);
        }
    }
}
