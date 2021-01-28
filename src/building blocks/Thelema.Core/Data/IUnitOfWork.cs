using System.Threading.Tasks;

namespace Thelema.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}