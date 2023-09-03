using System.Threading.Tasks;
using BasicStore.Core.DomainObjects.DTO;

namespace BasicStore.Pagamentos.Business
{
    public interface IPagamentoService
    {
        Task<Transacao> RealizarPagamentoPedido(PagamentoPedido pagamentoPedido);
    }
}