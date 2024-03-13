using GestaoProdutos.Domain.Exceptions;
using System;
using Xunit;

namespace GestaoProdutos.Domain.Tests
{
    public class ProductTests
    {
        [Fact(DisplayName = "Deve retornar uma exceção, date_created é após date_valid")]
        [Trait("Domain", "ProductAggregate")]
        public void Should_Fail_DateCreateIsAfterValid()
        {
            //Arrange
            var dateCreated = DateTime.Now.AddDays(5);
            var dateValid = DateTime.Now.AddDays(3);
            Supplier supplier = new("Nike", "64722539000137");

            //Act and Assert
            var exception = Assert.Throws<DomainException>(() => new Product("Bola", dateCreated, dateValid, supplier));
            Assert.Equal("Data de criação nao pode ser maior ou igual que a data de validade", exception.Message);
        }

        [Fact(DisplayName = "Deve retornar uma exceção, date_created é igual date_valid")]
        [Trait("Domain", "ProductAggregate")]
        public void Should_Fail_DateCreateIsEqualToDateValid()
        {
            //Arrange
            var date = DateTime.Now.AddDays(1);
            Supplier supplier = new("Nike", "64722539000137");

            //Act and Assert
            var exception = Assert.Throws<DomainException>(() => new Product("Bola", date, date, supplier));
            Assert.Equal("Data de criação nao pode ser maior ou igual que a data de validade", exception.Message);
        }

        [Fact(DisplayName = "Deve retornar uma exceção, cnpj do forncedor é invalido")]
        [Trait("Domain", "ProductAggregate")]
        public void Should_Fail_SupplierCnpjIsInvalid()
        {
            //Act and Assert
            var exception = Assert.Throws<DomainException>(() => new Supplier("Nike", "ddd"));
            Assert.Equal("CNPJ do fornecedor é invalido", exception.Message);
        }

        [Theory(DisplayName = "Fornecedor criado com sucesso cnpj valido")]
        [Trait("Domain", "ProductAggregate")]
        [InlineData("41.916.248/0001-84")]
        [InlineData("24.089.514/0001-23")]
        [InlineData("25.117.928/0001-81")]
        public void Should_Success_SupplierValid(string document)
        {
            //Arrange & Act
            Supplier supplier = new("Nike", document);

            //Assert 
            Assert.NotNull(supplier);
        }

        [Fact(DisplayName = "Deve criar um produto com fornecedor e inativa-lo")]
        [Trait("Domain", "ProductAggregate")]
        public void Should_Create_ProductAndInactive()
        {
            //Arrange
            var dateCreated = DateTime.Now.AddDays(3);
            var dateValid = DateTime.Now.AddDays(5);
            Supplier supplier = new("Nike", "64722539000137");

            //Act
            var product = new Product("Bola futebol", dateCreated, dateValid, supplier);
            product.Inactive();

            //Assert
            Assert.False(product.IsActive);
            Assert.NotNull(product.Supplier);
            Assert.NotEmpty(product.Description);
            Assert.Equal(product.DateCreated, dateCreated);
            Assert.Equal(product.DateValid, dateValid);
        }
    }
}
