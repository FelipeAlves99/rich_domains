using System;
using Flunt.Notifications;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Repositories;
using PaymentContext.Domain.Services;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Shared.Commands;
using PaymentContext.Shared.Handlers;

namespace PaymentContext.Domain.Handlers
{
    public class SubscriptionHandler : Notifiable, IHandler<CreateBoletoSubscriptionCommand>, IHandler<CreatePaypalSubscriptionCommand>
    {
        private readonly IStudentRepository _repository;
        private readonly IEmailService _emailService;

        public SubscriptionHandler(IStudentRepository repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        public ICommandResult Handle(CreateBoletoSubscriptionCommand command)
        {
            //Fail fast validation
            command.Validate();
            if (command.Invalid)
            {
                AddNotifications(command);
                return new CommandResult(false, "Não foi possível realizar seu cadastro");
            }

            // Verify if Document already exists
            if (_repository.DocumentExists(command.Document))
                AddNotification("Document", "Este CPF já está em uso");

            // Verify if Email already exists
            if (_repository.EmailExists(command.Document))
                AddNotification("Email", "Este email já está em uso");

            //Generate VOs
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var email = new Email(command.Email);
            var address = new Address(command.Street, command.Number, command.Neighborhood, command.City, command.State, command.Country, command.ZipCode);

            //Generate Entities
            var student = new Student(name, document, email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new BoletoPayment
                (command.BarCode, command.BoletoNumber, command.PaidDate, command.ExpiredDate, command.Total, command.TotalPaid, command.Payer,
                 new Document(command.PayerDocument, command.PayerDocumentType), address, email);

            //Relationships
            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            //Group Validations
            AddNotifications(name, document, email, address, student, subscription, payment);

            //Save info
            _repository.CreateSubscription(student);

            //Send email
            _emailService.Send(student.Name.ToString(), student.Email.Address, "Bem vindo ao balta.io", "Sua assinatura foi criada");

            return new CommandResult(true, "Assinatura realizada com sucesso.");
        }

        public ICommandResult Handle(CreatePaypalSubscriptionCommand command)
        {
            // Verify if Document already exists
            if (_repository.DocumentExists(command.Document))
                AddNotification("Document", "Este CPF já está em uso");

            // Verify if Email already exists
            if (_repository.EmailExists(command.Document))
                AddNotification("Email", "Este email já está em uso");

            //Generate VOs
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var email = new Email(command.Email);
            var address = new Address(command.Street, command.Number, command.Neighborhood, command.City, command.State, command.Country, command.ZipCode);

            //Generate Entities
            var student = new Student(name, document, email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new PaypalPayment
                (command.TransactionCode, command.PaidDate, command.ExpiredDate, command.Total, command.TotalPaid, command.Payer,
                 new Document(command.PayerDocument, command.PayerDocumentType), address, email);

            //Relationships
            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            //Group Validations
            AddNotifications(name, document, email, address, student, subscription, payment);

            //Save info
            _repository.CreateSubscription(student);

            //Send email
            _emailService.Send(student.Name.ToString(), student.Email.Address, "Bem vindo ao balta.io", "Sua assinatura foi criada");

            return new CommandResult(true, "Assinatura realizada com sucesso.");
        }
    }
}