using PaymentContext.Domain.Enums;
using PaymentContext.Shared.ValueObjects;

namespace PaymentContext.Domain.ValueObjects
{
    public class Document : ValueObject
    {
        public Document(string number, EDocumentType type)
        {
            Number = number;
            DocType = type;
        }

        public string Number { get; private set; }
        public EDocumentType DocType { get; private set; }
    }
}