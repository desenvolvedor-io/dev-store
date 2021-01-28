using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NSE.Pagamentos.API.Models;
using NSE.Pagamentos.Facade;
using NSE.Pagamentos.NerdsPag;

namespace NSE.Pagamentos.CardAntiCorruption
{
    public class PagamentoCartaoCreditoFacade : IPagamentoFacade
    {
        private readonly PagamentoConfig _pagamentoConfig;

        public PagamentoCartaoCreditoFacade(IOptions<PagamentoConfig> pagamentoConfig)
        {
            _pagamentoConfig = pagamentoConfig.Value;
        }

        public async Task<Transacao> AutorizarPagamento(Pagamento pagamento)
        {
            var nerdsPagSvc = new NerdsPagService(_pagamentoConfig.DefaultApiKey,
                _pagamentoConfig.DefaultEncryptionKey);

            var cardHashGen = new CardHash(nerdsPagSvc)
            {
                CardNumber = pagamento.CartaoCredito.NumeroCartao,
                CardHolderName = pagamento.CartaoCredito.NomeCartao,
                CardExpirationDate = pagamento.CartaoCredito.MesAnoVencimento,
                CardCvv = pagamento.CartaoCredito.CVV
            };
            var cardHash = cardHashGen.Generate();

            var transacao = new Transaction(nerdsPagSvc)
            {
                CardHash = cardHash,
                CardNumber = pagamento.CartaoCredito.NumeroCartao,
                CardHolderName = pagamento.CartaoCredito.NomeCartao,
                CardExpirationDate = pagamento.CartaoCredito.MesAnoVencimento,
                CardCvv = pagamento.CartaoCredito.CVV,
                PaymentMethod = PaymentMethod.CreditCard,
                Amount = pagamento.Valor
            };

            return ParaTransacao(await transacao.AuthorizeCardTransaction());
        }

        public async Task<Transacao> CapturarPagamento(Transacao transacao)
        {
            var nerdsPagSvc = new NerdsPagService(_pagamentoConfig.DefaultApiKey,
                _pagamentoConfig.DefaultEncryptionKey);

            var transaction = ParaTransaction(transacao, nerdsPagSvc);

            return ParaTransacao(await transaction.CaptureCardTransaction());
        }

        public async Task<Transacao> CancelarAutorizacao(Transacao transacao)
        {
            var nerdsPagSvc = new NerdsPagService(_pagamentoConfig.DefaultApiKey,
                _pagamentoConfig.DefaultEncryptionKey);

            var transaction = ParaTransaction(transacao, nerdsPagSvc);

            return ParaTransacao(await transaction.CancelAuthorization());
        }

        public static Transacao ParaTransacao(Transaction transaction)
        {
            return new Transacao
            {
                Id = Guid.NewGuid(),
                Status = (StatusTransacao) transaction.Status,
                ValorTotal = transaction.Amount,
                BandeiraCartao = transaction.CardBrand,
                CodigoAutorizacao = transaction.AuthorizationCode,
                CustoTransacao = transaction.Cost,
                DataTransacao = transaction.TransactionDate,
                NSU = transaction.Nsu,
                TID = transaction.Tid
            };
        }

        public static Transaction ParaTransaction(Transacao transacao, NerdsPagService nerdsPagService)
        {
            return new Transaction(nerdsPagService)
            {
                Status = (TransactionStatus) transacao.Status,
                Amount = transacao.ValorTotal,
                CardBrand = transacao.BandeiraCartao,
                AuthorizationCode = transacao.CodigoAutorizacao,
                Cost = transacao.CustoTransacao,
                Nsu = transacao.NSU,
                Tid = transacao.TID
            };
        }
    }
}