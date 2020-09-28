# Rich Domains

## Ubiquitous Language

WIP

## Rich Domains VS Anemic Domains

WIP

## Sub Domains

WIP

## Separation in Limited Contexts

WIP

## Organizing Solution

WIP

## Defining Entities

WIP

## Corrupted Code

WIP

## SOLID and Clean Code

WIP

## Primitive Obsession

WIP

## Value Objects

WIP

## Implementing Validations

WIP

## Design by Contract

WIP

## Testing Entities and Value Objects

WIP

## Commands

WIP

## Fail Fast Validations

WIP

## Testing Commands

WIP

## Repository Pattern

Repository pattern is a way to handle data sources (not databases). This pattern is a way to remove DB dependencies by abstracting the access to data (you don't care from where it gets the data).

The idea is to depend from the abstraction and not the implementation. In the Domain, you will have `interfaces` abstracting the implementation of the repository.

In this repository **(the GitHub repo)** you can find a small interface creation (not the full implementation):

    using  PaymentContext.Domain.Entities;
    namespace PaymentContext.Domain.Repositories
    {
        public  interface  IStudentRepository
        {
    	    bool  DocumentExists(string  document);

    	    bool  EmailExists(string  email);

    	    void  CreateSubscription(Student  student);
        }
    }

This way, you isolates your Domain from the data and will never be affected by it

## Handlers

Handlers are responsible for handling the process flow, for example:

-   see if CPF and email is registered
-   Generate the VOs and Entities
-   Add a payment
-   Add a subscription
-   Save in the Database
-   Send a "Welcome" email
-   Return the result

Sample implementation are the following files:

-   SubscriptionHandler.cs (Lacks the implementation of Credit Card handler, but it's equals to the Boleto and Paypal Handle)
-   CreateBoletoSubscriptionCommand.cs (A feel modifications, compare to CreateCreditCardSubscriptionCommand)
-   Name.cs (Just an override)
-   IHandler.cs (the handler abstraction as an interface)
-   ICommandResult.cs
-   CommandResult.cs

Here, you will find the Repository usage, but lacks the dependency injection (should be handled at the API, for example) that the Domain doesn't care about.

## Testing Handlers

It is quite simple, first you need to mock the StudentRepository returns, since you do not depend from the database. After that, you can write more test for the student, such as the existing email (that is already mocked, just use the same email when creating the Command)

Sample files:

-   SubscriptionHandlerTests.cs
-   FakeEmailService.cs
-   FakeStudentRepository.cs

## Queries

The Queries are used to separate the repository queries from it. In the repository, you should be using an ORM, such as Entity Framework.
In the repository, the code would be like this `_context.Student.Where(StudentQueries.GetStudentInfo(document));` and the query would be like this:

    public  static  Expression<Func<Student, bool>> GetStudentInfo(string  document)
    {
        return  x  => x.Document.Number == document;
    }

All of this is used when you are **NOT** writing manual SQL query in the code.

## Testing Queries

It's the same as the other tests, but here you have to use `AsQueryable()` from LINQ, to test the queries.

Sample file: StudentQueriesTests.cs
