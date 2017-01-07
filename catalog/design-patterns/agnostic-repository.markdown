---
layout: design-pattern
title: Agnostic repository
date: 2017-01-06 00:00:00 +0100
modifiedDate: 2017-01-06 00:00:00 +0100
categories:
    - language-agnostic
    - object-mapping
    - domain-driven-design
    - design-pattern
authors: 
   - mfidemraizer
featuredImage: /img/agnostic-repository/diagram.jpg
---

### Brief description

A [repository variant (see Repository design pattern)](/catalog/design-patterns/repository){:target="_blank"} completely agnostic to data-mapping technology.

### What does it solve?

Basically all modern object-oriented programming language doesn't support multi-inheritance. While this is a good constraint to avoid *design smells*, it imposes some limitation on some use cases.

For example, let's say we want to be able to code a [Repository design pattern)](/catalog/design-patterns/repository){:target="_blank"} implementation which could be both use a given *data mapper* and also be specialized in derived classes by overridding polymoprhic methods, but provide such specializations without coupling them to a particular *data mapper*.

See the following code to get convinced about it's not possible to specialize repositories this way and with simple inheritance:

{% highlight c# %}
// Sample code in C#
// This interface would define more methods, but 
// there's just one to simplify the reasoning.
public interface IRepository<T>
{
    void Add(T someObject);
}

// Probably you don't want to repeat yourself in terms of implement basic
// CRUD operations across your solution. Thus, you would end up creating
// a base class which would be associated with some data mapper of your choice.
// Change SomeDataMapper with who knows what OR/M, micro OR/M, NoSQL client
// or whatever ;)
public abstract class Repository<T> : IRepository<T>
{
    public Repository(SomeDataMapper dataMapper)
    {
        DataMapper = dataMapper;
    }

    private SomeDataMapper DataMapper { get; }

    public virtual void Add(T someObject)
    {
        DataMapper.InsertOne(someObject);
    }
}

// Imagine you want some specialization to share how repositories validate
// domain objects across your entire solution. You would end up creating
// a base class like the following one.
// Oops! You would be able to specialize this validatable repository
// unless some domain requires a repository which isn't tied to 
// Repository base class' data mapping technology...
public abstract class ValidatableRepository<T> : Repository<T>
{
    public Repository(IList<ISpecification<T>> specifications, SomeDataMapper dataMapper)
        : base(dataMapper)
    {
        Specifications = specifications;
    }

    private IList<ISpecification<T>> Specifications { get; }

    public override void Add(T SomeObject)
    {
        if(Specifications.All(spec => spec.IsSatisfiedBy(someObject)))
        {
            base.Add(someObject);
        }
    }
}

// You would define an interface to implement methods that go beyond
// basic CRUD from the repository interface.
public interface ICustomerRepository : IRepository<Customer>
{
    IList<Customer> GetTopCustomers(int maxResults = 10);
}

// Finally, you would both inherit validatable repository because you want
// to validate your domain objects using the approach implemented across your
// solution. But... Oops (again!). Your customer repository is still coupled
// with an specific data mapper technology, because it inherits the so-called
// validatable repository!
public class CustomerRepository : ValidatableRepository<Customer>, ICustomerRepository
{    	
    public CustomerRepository(IList<ISpecification<T>> specifications, SomeDataMapper dataMapper)
        : base(specifications, dataMapper)
    {
    }

    public IList<Customer> GetTopCustomers(int maxResults = 10)
    {
        return DataMapper.OrderByDescending(customer => customer.Sales.Count)
                    .Take(maxResults)
                    .ToList();
    }
}
{% endhighlight %}
    
Summary of issues using pure inheritance:

- Inheritance is the approach to share common rules and functionalities across all your repository implementations.
- Thus, you get stuck with a concrete data mapping technology.
    
    
### Solution

Main goal is to implement repositories which share a lot in common, but these common concerns shouldn't be coupled to a concrete data mapping technology.

#### There should be (at least) two repository implementations

![Agnostic Repository diagram](/img/agnostic-repository/diagram.jpg)

That's it! There should be two repository implementations:

1. One will be agnostic to data mapping technology. That is, it'll be the one that will implement concerns that will be absolutely common to all repositories across your solution.
2. Second will be the one coupled to a specific data mapping technology.

Both #1 and #2 will build an *agnostic repository*. Let's see a code sample in C# to learn how to achieve the whole goal with a pratical example (also check the comments
within the whole code sample):

{% highlight c# %}
// #1 We define a sample repository interface. A complete one
// would define methods for all basic CRUD operations.
public interface IRepository<T>
{
    void Add(T someObject);
}

// #2 We implement the whole agnostic repository. It's the one that
// will contain anything unrelated to data mapping technology.
// 
// Persistence will be handled by the believer repository, the one that
// will be coupled to a data mapping technology, but the agnostic repository
// will just rely on the injected repository to persist objects.
public abstract class AgnosticRepository<T> : IRepository<T>
{
    // #3 We receive both the believer repository and a collection of
    // specification to validate incoming object to be persisted somehow
    // using the believer repository!
    public AgnosticRepository(IRepository<T> believerRepository, IList<ISpecification<T>> specifications)
    {
        BelieverRepository = believerRepository;
        Specifications = specs;
    }

    private IRepository<T> BelieverRepository { get; }
    private IList<ISpecification<T>> Specifications { get; }

    public void Add(T someObject)
    {
        // #4 We validate the incoming object against all injected 
        // specifications. If the incoming object is satisfied by all
        // specifications, the object is persisted using the believer
        // repository!
        if(Specifications.All(spec => spec.IsSatisfiedBy(domainObject)))
        {
            BelieverRepository.Add(someObject);
        }
        else
        {
            //  NOTE: This is an oversimplification for the sake of this sample. 
            // Actually you wouldn't throw an exception, but you would return
            // unpassed specifications somehow.
            throw new InvalidOperationException("Given object could not fulfill its specifications!");
        }
    }
}

// #5 Here's the believer repository. In our sample, we'll implement a
// repository coupled to Entity Framework OR/M as the data mapping layer.
public abstract class EntityFrameworkRepository<T> : IRepository<T>
{
    public EntityFrameworkRepository(DbSet<T> dbSet)
    {
        DbSet = dbSet;
    }

    private DbSet<T> DbSet { get; }

    public void Add(T someObject)
    {
        // This is an oversimplification for the sake of this sample.
        // Here you could also implement everything that should be 
        // coupled to Entity Framework OR/M to add objects and perform
        // whatever check, transaction or any other operation that you should
        // be required here.
        DbSet.Add(someObject);
    }
}
{% endhighlight %}

Furthermore, you would use some *dependency injection*/*inversion of control* container framework to configure that any derived class of `AgnosticRepository<T>` should inject the `EntityFrameworkRepository<T>` implementation. That is, you would inject an agnostic repository as follows:

{% highlight c# %}
public sealed class CustomerService
{
    // We inject an implementation of customer repository interface.
    // The so-called implementation would be a derived class of 
    // AgnosticRepository<T>
    public CustomerService(ICustomerRepository customerRepository)
    {
    	CustomerRepository = customerRepository;
    }
    
    private ICustomerRepository CustomerRepository { get; }
    
    public Customer Create()
    {
    	Customer customer = new Customer();
        
    	CustomerRepository.Add(customer);
        
        return customer;
    }
}
{% endhighlight %}