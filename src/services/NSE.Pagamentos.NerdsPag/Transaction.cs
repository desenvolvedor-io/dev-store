using System;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Pagamentos.NerdsPag
{
   public class Transaction
    {
        public Transaction(NerdsPagService nerdsPagService)
        {
            NerdsPagService = nerdsPagService;
        }

        protected Transaction(){}

        private readonly NerdsPagService NerdsPagService;

        protected string Endpoint { get; set; }

        public int SubscriptionId { get; set; }

        public TransactionStatus Status { get; set; }

        public int AuthorizationAmount { get; set; }

        public int PaidAmount { get; set; }

        public int RefundedAmount { get; set; }

        public string CardHash { get; set; }

        public string CardNumber { get; set; }

        public string CardExpirationDate { get; set; }

        public string StatusReason { get; set; }

        public string AcquirerResponseCode { get; set; }

        public string AcquirerName { get; set; }

        public string AuthorizationCode { get; set; }

        public string SoftDescriptor { get; set; }

        public string RefuseReason { get; set; }

        public string Tid { get; set; }

        public string Nsu { get; set; }

        public decimal Amount { get; set; }

        public int? Installments { get; set; }

        public decimal Cost { get; set; }

        public string CardHolderName { get; set; }

        public string CardCvv { get; set; }

        public string CardLastDigits { get; set; }

        public string CardFirstDigits { get; set; }

        public string CardBrand { get; set; }

        public string CardEmvResponse { get; set; }

        public string PostbackUrl { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public float? AntifraudScore { get; set; }

        public string BilletUrl { get; set; }

        public string BilletInstructions { get; set; }

        public DateTime? BilletExpirationDate { get; set; }

        public string BilletBarcode { get; set; }

        public string Referer { get; set; }

        public string IP { get; set; }

        public bool? ShouldCapture { get; set; }

        public bool? Async { get; set; }

        public string LocalTime { get; set; }

        public DateTime TransactionDate { get; set; }

        public Task<Transaction> AuthorizeCardTransaction()
        {
            var success = new Random().Next(2) == 0;
            Transaction transaction;

            if (success)
            {
                transaction = new Transaction
                {
                    AuthorizationCode = GetGenericCode(),
                    CardBrand = "MasterCard",
                    TransactionDate = DateTime.Now,
                    Cost = Amount * (decimal)0.03,
                    Amount = Amount,
                    Status = TransactionStatus.Authorized,
                    Tid = GetGenericCode(),
                    Nsu = GetGenericCode()
                };

                return Task.FromResult(transaction);
            }

            transaction = new Transaction
            {
                AuthorizationCode = "",
                CardBrand = "",
                TransactionDate = DateTime.Now,
                Cost = 0,
                Amount = 0,
                Status = TransactionStatus.Refused,
                Tid = "",
                Nsu = ""
            };

            return Task.FromResult(transaction);
        }

        public Task<Transaction> CaptureCardTransaction()
        {
            var transaction = new Transaction
                {
                    AuthorizationCode = GetGenericCode(),
                    CardBrand = CardBrand,
                    TransactionDate = DateTime.Now,
                    Cost = 0,
                    Amount = Amount,
                    Status = TransactionStatus.Paid,
                    Tid = Tid,
                    Nsu = Nsu
                };

            return Task.FromResult(transaction);
        }

        public Task<Transaction> CancelAuthorization()
        {
            var transaction = new Transaction
            {
                AuthorizationCode = "",
                CardBrand = CardBrand,
                TransactionDate = DateTime.Now,
                Cost = 0,
                Amount = Amount,
                Status = TransactionStatus.Cancelled,
                Tid = Tid,
                Nsu = Nsu
            };

            return Task.FromResult(transaction);
        }

        private string GetGenericCode()
        {
            return new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }
    }
}
