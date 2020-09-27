using System;
using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Domain.Entities
{
    public class PaypalPayment : Payment
    {
        public PaypalPayment(string transactionCode, DateTime paidDate, DateTime expiredDate, decimal total, decimal totalPaid, string payer, Document document, Address address, Email email)
         : base(paidDate, expiredDate, total, totalPaid, payer, document, address, email)
        {
            TransactionCode = transactionCode;
        }
        public string TransactionCode { get; private set; }
    }
}