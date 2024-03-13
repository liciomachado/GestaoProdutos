using FluentAssertions;
using GestaoProdutos.API.Middlewares;
using GestaoProdutos.API.Tests.Abstractions;
using GestaoProdutos.Application.Dtos;
using GestaoProdutos.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace GestaoProdutos.API.Tests
{
    public class ProductControllerFunctionalTest : BaseFunctionalTests
    {
        private readonly FunctionalTestWebAppFactory _testsFixture;

        private readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public ProductControllerFunctionalTest(FunctionalTestWebAppFactory testsFixture) : base(testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact(DisplayName = "Deve retornar NotFound para id não existente")]
        [Trait("Functional", "ProductController")]
        public async Task Should_Return_NotFound_WhenIdNotFound()
        {
            //Arrange
            var id = 50L;
            //Act
            var response = await HttpClient.GetAsync($"api/product/{id}");
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        }

        [Fact(DisplayName = "Deve retornar um Produto com id valido")]
        [Trait("Functional", "ProductController")]
        public async Task Should_Return_Product()
        {
            //Arrange
            ProductResponseViewModel productResponse = await CreateProduct();

            //Act 
            var response = await HttpClient.GetAsync($"api/product/{productResponse.Id}");
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var stringJson = await response.Content.ReadAsStringAsync();
            var productResponseView = JsonSerializer.Deserialize<ProductResponseViewModel>(stringJson, jsonOptions);
            productResponseView.Should().NotBeNull();
            productResponseView.Description.Should().NotBeNullOrEmpty();
        }

        [Fact(DisplayName = "Deve Atualizar um Produto com id valido")]
        [Trait("Functional", "ProductController")]
        public async Task Should_Return_ProductUpdated()
        {
            //Arrange
            ProductResponseViewModel productResponse = await CreateProduct();
            ProductRequestUpdateViewModel request = new()
            {
                Description = "Bola futebol",
                DateCreated = DateTime.Now.AddDays(-10),
                DateValid = DateTime.Now.AddDays(10),
            };
            //Act 
            var response = await HttpClient.PutAsJsonAsync($"api/product/{productResponse.Id}", request);
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var stringJson = await response.Content.ReadAsStringAsync();
            var productResponseView = JsonSerializer.Deserialize<ProductResponseViewModel>(stringJson, jsonOptions);
            productResponseView.Should().NotBeNull();
            productResponseView.Description.Should().Be(request.Description);
            productResponseView.DateCreated.Should().Be(request.DateCreated);
            productResponseView.DateValid.Should().Be(request.DateValid);
        }

        [Fact(DisplayName = "Deve desativar o produto com base no produto valido")]
        [Trait("Functional", "ProductController")]
        public async Task Should_Return_NoContentProductInactive()
        {
            //Arrange
            ProductResponseViewModel productResponse = await CreateProduct();

            //Act 
            var response = await HttpClient.DeleteAsync($"api/product/{productResponse.Id}");
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact(DisplayName = "Erro ao desativar produto, id invalido")]
        [Trait("Functional", "ProductController")]
        public async Task Should_ReturnError_InvalidIdToDisableProduct()
        {
            //Arrange
            var id = 50L;
            //Act 
            var response = await HttpClient.DeleteAsync($"api/product/{id}");
            //Assert
            var stringJson = await response.Content.ReadAsStringAsync();
            var dtoResponse = JsonSerializer.Deserialize<ErrorRequest>(stringJson);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            dtoResponse.Should().NotBeNull();
            dtoResponse!.Success.Should().BeFalse();
            dtoResponse!.Message.First().Should().Be("O id do produto não é valido");
        }

        [Fact(DisplayName = "Erro ao atualizar produto, id invalido")]
        [Trait("Functional", "ProductController")]
        public async Task Should_ReturnError_InvalidIdToupdateProduct()
        {
            //Arrange
            var id = 50L;
            ProductRequestUpdateViewModel request = new()
            {
                Description = "Bola futebol",
                DateCreated = DateTime.Now.AddDays(-10),
                DateValid = DateTime.Now.AddDays(10),
            };
            //Act 
            var response = await HttpClient.PutAsJsonAsync($"api/product/{id}", request);
            //Assert
            var stringJson = await response.Content.ReadAsStringAsync();
            var dtoResponse = JsonSerializer.Deserialize<ErrorRequest>(stringJson);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            dtoResponse.Should().NotBeNull();
            dtoResponse!.Success.Should().BeFalse();
            dtoResponse!.Message.First().Should().Be("O id do produto não é valido");
        }

        [Fact(DisplayName = "Erro ao cadastrar produto, cnpj invalido")]
        [Trait("Functional", "ProductController")]
        public async Task Should_ReturnError_InvalidCNPJSupplier()
        {
            //Arrange
            ProductRequestViewModel viewModel = new()
            {
                Description = "bola",
                DateCreated = DateTime.Now.AddDays(-5),
                DateValid = DateTime.Now.AddDays(5),
                Supplier = new SupplierRequestViewModel
                {
                    Description = "Kibola",
                    CNPJ = "ddd"
                }
            };

            //Act 
            var response = await HttpClient.PostAsJsonAsync($"api/product", viewModel);
            //Assert
            var stringJson = await response.Content.ReadAsStringAsync();
            var dtoResponse = JsonSerializer.Deserialize<ErrorRequest>(stringJson);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            dtoResponse.Should().NotBeNull();
            dtoResponse!.Success.Should().BeFalse();
            dtoResponse!.Message.First().Should().Be("CNPJ do fornecedor é invalido");
        }

        [Fact(DisplayName = "Erro ao cadastrar produto, payload invalido")]
        [Trait("Functional", "ProductController")]
        public async Task Should_ReturnError_InvalidPayload()
        {
            //Arrange
            ProductRequestViewModel viewModel = new()
            {
                Description = "",
                Supplier = new SupplierRequestViewModel
                {
                    Description = "Kibola",
                    CNPJ = "ddd"
                }
            };

            //Act 
            var response = await HttpClient.PostAsJsonAsync($"api/product", viewModel);
            //Assert
            var stringJson = await response.Content.ReadAsStringAsync();
            var dtoResponse = JsonSerializer.Deserialize<ErrorRequest>(stringJson, jsonOptions);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            dtoResponse.Should().NotBeNull();
            dtoResponse!.Success.Should().BeFalse();
            dtoResponse!.Message.Any().Should().BeTrue();
        }

        [Fact(DisplayName = "Deve filtrar por descricao")]
        [Trait("Functional", "ProductController")]
        public async Task Should_Return_ListFilteredByDescription()
        {
            //Arrange
            await CreateProduct("camiseta futebol");
            await CreateProduct("camiseta volei");
            await CreateProduct("camiseta handebol");
            string valueFilter = "camiseta";
            var filter = $"description={valueFilter}&size=10&page=1";

            //Act 
            var response = await HttpClient.GetAsync($"api/product?{filter}");

            //Assert
            var stringJson = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var productResponseView = JsonSerializer.Deserialize<List<ProductResponseViewModel>>(stringJson, jsonOptions);
            productResponseView.Should().NotBeNull();
            productResponseView.Count.Should().Be(3);
        }

        [Fact(DisplayName = "Deve filtrar por dateCreated")]
        [Trait("Functional", "ProductController")]
        public async Task Should_Return_ListFilteredByDateCreated()
        {
            //Arrange
            await CreateProduct("bola futebol");
            await CreateProduct("bola volei");
            await CreateProduct("bola handebol");
            var filter = @$"startDateValid={DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd")}
                        &finishDateValid={DateTime.Now.AddDays(+6).ToString("yyyy-MM-dd")}
                        &size=10&page=1";
            //Act 
            var response = await HttpClient.GetAsync($"api/product?{filter}");

            //Assert
            var stringJson = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var productResponseView = JsonSerializer.Deserialize<List<ProductResponseViewModel>>(stringJson, jsonOptions);
            productResponseView.Should().NotBeNull();
            productResponseView.Count.Should().BeGreaterThanOrEqualTo(3);
        }

        private async Task<ProductResponseViewModel> CreateProduct(string description = "bola")
        {
            //Arrange 1 
            ProductRequestViewModel viewModel = new()
            {
                Description = description,
                DateCreated = DateTime.Now.AddDays(-5),
                DateValid = DateTime.Now.AddDays(5),
                Supplier = new SupplierRequestViewModel
                {
                    Description = "Kibola",
                    CNPJ = "41.916.248/0001-84"
                }
            };
            //Act 1 
            var response = await HttpClient.PostAsJsonAsync($"api/product", viewModel);
            var stringJson = await response.Content.ReadAsStringAsync();
            var productResponseView = JsonSerializer.Deserialize<ProductResponseViewModel>(stringJson, jsonOptions);
            return productResponseView;
        }
    }
}
