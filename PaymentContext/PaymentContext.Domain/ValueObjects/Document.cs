using Flunt.Validations;
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

            AddNotifications(new Contract()
                .Requires()
                .IsTrue(Validate(), "Document.Number", "Documento inválido")
            );
        }

        public string Number { get; private set; }
        public EDocumentType DocType { get; private set; }

        private bool Validate()
        {
            if (DocType == EDocumentType.CNPJ && Number.Length == 14)
                return true;
            if (DocType == EDocumentType.CPF && Number.Length == 11)
                return true;
            return false;
        }
    }
}