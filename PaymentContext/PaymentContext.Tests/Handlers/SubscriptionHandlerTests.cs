using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Handlers;
using PaymentContext.Tests.Mocks;

namespace PaymentContext.Tests.Handlers
{
    [TestClass]
    public class SubscriptionHandlerTests
    {
        [TestMethod]
        public void ShouldReturnErrorWhenDocumentExists()
        {
            var handler = new SubscriptionHandler(new FakeStudentRepository(), new FakeEmailService());
            var command = new CreateBoletoSubscriptionCommand();
            command.FirstName = "Felipe";
            command.LastName = "Alves";
            command.Document = "12345678912";
            command.Email = "falves99.dev@gmail.com";
            command.BarCode = "123456789";
            command.BoletoNumber = "123456789";
            command.PaymentNumber = "123456789";
            command.PaidDate = DateTime.Now;
            command.ExpiredDate = DateTime.Now.AddMonths(1);
            command.Total = 60;
            command.TotalPaid = 60;
            command.Payer = "Myself";
            command.PayerDocument = "12345678911";
            command.PayerDocumentType = EDocumentType.CPF;
            command.PayerEmail = "myself@myself.com";
            command.Street = "fala";
            command.Number = "2020";
            command.Neighborhood = "dev";
            command.City = "as";
            command.State = "sd";
            command.Country = "fd";
            command.ZipCode = "123456";

            handler.Handle(command);
            Assert.AreEqual(false, handler.Valid);
        }
    }
}