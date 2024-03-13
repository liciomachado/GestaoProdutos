using System.Threading.Tasks;

namespace GestaoProdutos.Domain.Core
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}