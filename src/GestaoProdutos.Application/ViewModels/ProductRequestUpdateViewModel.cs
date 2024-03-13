using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GestaoProdutos.Application.ViewModels
{
    public class ProductRequestUpdateViewModel
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
    }
}
