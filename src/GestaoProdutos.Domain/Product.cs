using GestaoProdutos.Domain.Core;
using GestaoProdutos.Domain.Exceptions;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoProdutos.Domain
{
    [Table("products", Schema = "public")]
    public class Product : IAggregateRoot
    {
        [Key()]
        [Column("id")]
        public long Id { get; private set; }
        [Column("description")]
        public string Description { get; private set; }
        [Column("is_active")]
        public bool IsActive { get; private set; }
        [Column("date_created")]
        public DateTime DateCreated { get; private set; }
        [Column("date_valid")]
        public DateTime DateValid { get; private set; }

        [Column("fk_supplier")]
        public virtual long FkSupplier { get; private set; }
        [ForeignKey(nameof(FkSupplier))]
        public Supplier Supplier { get; private set; }

        public Product() { }

        public Product(string description, DateTime dateCreated, DateTime dateValid, Supplier supplier)
        {
            IsDateValid(dateCreated, dateValid);

            Description = description;
            IsActive = true;
            DateCreated = dateCreated;
            DateValid = dateValid;
            Supplier = supplier;
        }

        private static void IsDateValid(DateTime dateCreated, DateTime dateValid)
        {
            bool isDateCreatedValid = dateCreated < dateValid;
            if (!isDateCreatedValid) throw new DomainException("Data de criação nao pode ser maior ou igual que a data de validade");
        }

        public void Inactive() => IsActive = false;

        public void UpdateValues(string description, DateTime dateCreated, DateTime dateValid)
        {
            if (!string.IsNullOrEmpty(description)) Description = description;
            IsDateValid(dateCreated, dateValid);
            DateCreated = dateCreated;
            DateValid = dateValid;
        }
    }
}
