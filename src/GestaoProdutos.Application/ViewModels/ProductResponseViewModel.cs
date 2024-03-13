using System;
using System.Text.Json.Serialization;

namespace GestaoProdutos.Application.ViewModels
{
    public class ProductResponseViewModel
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; }
        [JsonPropertyName("date_created")]
        public DateTime DateCreated { get; set; }
        [JsonPropertyName("date_valid")]
        public DateTime DateValid { get; set; }
        [JsonPropertyName("supplier")]
        public SupplierResponseViewModel Supplier { get; set; }
    }
    public class SupplierResponseViewModel
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("cnpj")]
        public string CNPJ { get; set; }
    }
}
