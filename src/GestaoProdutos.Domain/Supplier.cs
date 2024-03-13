using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoProdutos.Domain
{
    [Table("suppliers", Schema = "public")]
    public class Supplier
    {
        [Key]
        [Column("id")]
        public long Id { get; private set; }
        [Column("description")]
        public string Description { get; private set; }
        [Column("cnpj")]
        public CnpjVO Cnpj { get; private set; }

        //[JsonIgnore]
        //public List<Product> Products { get; private set; }

        public Supplier()
        {

        }

        public void ChangeDescription(string description)
        {
            Description = description;
        }

        public Supplier(string description, string cnpj)
        {
            Description = description;
            Cnpj = new CnpjVO(cnpj);
        }
    }
}
