using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GestaoProdutos.Application.Dtos
{
    public class ProductRequestViewModel
    {
        [Required]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [Required]
        [JsonPropertyName("date_created")]
        public DateTime DateCreated { get; set; }
        [Required]
        [JsonPropertyName("date_valid")]
        public DateTime DateValid { get; set; }
        [Required]
        [JsonPropertyName("supplier")]
        public SupplierRequestViewModel Supplier { get; set; }
    }

    public class SupplierRequestViewModel
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [Required]
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [Required]
        [JsonPropertyName("cnpj")]
        public string CNPJ { get; set; }
    }
}