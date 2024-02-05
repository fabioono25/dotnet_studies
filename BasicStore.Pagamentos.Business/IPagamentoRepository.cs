using BasicStore.Core.Data;

namespace BasicStore.Pagamentos.Business
{
    public interface IPagamentoRepository : IRepository<Pagamento>
    {
        void Adicionar(Pagamento pagamento);

        void AdicionarTransacao(Transacao transacao);
    }
}