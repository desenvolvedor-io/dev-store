using System.Threading.Tasks;

namespace NSE.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}