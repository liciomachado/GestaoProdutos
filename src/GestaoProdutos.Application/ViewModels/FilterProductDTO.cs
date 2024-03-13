using System;

namespace GestaoProdutos.Application.ViewModels
{
    public class FilterProductDTO
    {
        public string? Description { get; set; }
        public DateTime? StartDateCreated { get; set; }
        public DateTime? FinishDateCreated { get; set; }

        public DateTime? StartDateValid { get; set; }
        public DateTime? FinishDateValid { get; set; }
        public int Size { get; set; }
        public int Page { get; set; }

        public FilterProductDTO(string? description, DateTime? startDateCreated, DateTime? finishDateCreated,
            DateTime? startDateValid, DateTime? finishDateValid, int size, int page)
        {
            Description = description;
            StartDateCreated = startDateCreated;
            FinishDateCreated = finishDateCreated;
            StartDateValid = startDateValid;
            FinishDateValid = finishDateValid;
            Size = size;
            Page = page;
        }
    }
}
