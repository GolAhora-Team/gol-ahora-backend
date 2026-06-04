using System.Threading.Tasks;

namespace Aplication.Interfaces.IRecibo
{
    public interface IReciboService
    {
        Task<byte[]> GenerarPdfRecibo(int reservaId);
    }
}
