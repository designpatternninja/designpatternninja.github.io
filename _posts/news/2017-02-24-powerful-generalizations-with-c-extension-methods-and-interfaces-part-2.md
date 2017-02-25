---
layout: post
title: "Powerful generalizations using C# extension methods, generics and interfaces - Part 2"
excerpt: "Did you get excited about the previous post? Let's take the generalization to the next level!"
date: 2017-02-24 00:00:00 +0100
modifiedDate: 2017-02-24 00:00:00 +0100
categories: news
authors: 
   - mfidemraizer
---

Did you get excited about [previous article]({% post_url /news/2017-02-20-powerful-generalizations-with-c-extension-methods-and-interfaces %}) about how to use extension methods, generics and interfaces to implement powerful generalizations? Let's go further to the next level of generalization!

## Our new goal

In the previous article we could implement an extension method to *add tags* to some *taggable entity*, right? But... did you ask yourself if we could implement an extension method to any *has-many* aggregation?

For example:

- A `Company` has many `Employee`.
- An `Order` has many `OrderLine`.
- A `SocialNetwork` has many `Users`.

Isn't our goal to avoid repeating ourselves? With the given approach in the previous article we would end up with an interface and an extension method for each *has-many* aggregation... This isn't keeping things simple...

That is, our goal is to implement an extension method to handle all *has-many* aggregations whichever *aggregate root* that holds *many aggregates*.

## The party starts again!

First of all, we need a new interface called `IHasMany` and it'll be declared as follows:

```c#
public interface IHasMany<out TAggregate, out TCollection>
    where TCollection : IEnumerable<TAggregate>
{
    TCollection Aggregates { get; }
}
```

OK. Now we've defined that some aggregate root may contain *many aggregates of some other type* which are held by a collection that must implement `IEnumerable<T>`.

Next step: design two aggregates and implement the whole interface:

```c#
public class Company : IHasMany<Employee, ISet<Employee>>
{
	public Guid Id { get; set; }
	public ISet<Employee> Employees { get; set; } = new HashSet<Employee>();
    
    // When we access Aggregates we'll get the Employees set.
    ISet<Employee> IHasMany<Employee, ISet<Employee>>.Aggregates => Employees;
}

// Hote that we need to implement IEquatable<T>
// because otherwise the Company.Employees set collection wouldn't
// know how to determine what makes an employee unique
public class Employee : IEquatable<Employee>
{
	public Guid Id { get; set; }
    
	public override int GetHashCode() => Id.GetHashCode();
    public override bool Equals(object other) => Equals(other as Employee);
    public bool Equals(Employee other) 
    {
    	if(other == null) return false;
        
        return ReferenceEquals(this, other) || Id == other.Id;
    }
}
```

Why `IHasMany` is implemented that way?

```c#
ISet<Employee> IHasMany<Employee, ISet<Employee>>.Aggregates => Employees; 
```

This is an [*explicit interface implementation*](https://msdn.microsoft.com/en-us/library/ms173157.aspx){:target="_blank"}. Basically it has two positive effects:

1. With explicit interface implementations you may implement the same interface many times on the same class if its generic arguments vary. Think about it like *method overloading*.
2. An explicitly implemented interface member isn't accessible unless an object implementing its interface is downcasted to that interface. I used an explicit interface implementation because of this: I don't want that `Aggregates` property could be publicly accessible from a `Company` object. *Why?* Because we implement that interface to meet our goal. We don't want to pollute the usage of the `Company` class.

Moreover, now we would implement a *company serivce*:

```c#
public interface ICompanyService
{
	// Methods here. I don't add them because it doesn't matter 
    // to meet our goal.
}

public sealed CompanyService : ICompanyService
{
}
```

Do you remember the `ICanGetObjectById` from the previous article? It looked as follows:

```c#
public interface ICanGetObjectsById<TId, TObject>
{
	TObject GetById(TId id);
}
```

That interface should be implemented by a *company service*:

```
public interface ICompanyService : ICanGetOBjectById<Guid, Company>
{
	// Methods here. I don't add them because it doesn't matter 
    // to meet our goal.
}

public sealed class CompanyService : ICompanyService
{
	public CompanyService(ICompanyRepository companyRepository)
    {
    	CompanyRepository = companyRepository;
    }
    
    private ICompanyRepository CompanyRepository { get; }
    
	public Task<Company> GetByIdAsync(Guid id)
    {
    	return CompanyRepository.GetByIdAsync(id);
    }
}
```

And finally we can implement our wonderful extension method:

```c#
public static class AggregationExtensions
{
    public async static Task<int> AddAggregatesToAsync<TId, TAggregateRoot, TAggregate>(this ICanGetEntityById<TId, TAggregateRoot> canGetById, TId aggregateRootId, IEnumerable<TAggregate> aggregates)
        where TId : IEquatable<TId>
        where TAggregateRoot : IHasMany<TAggregate, ISet<TAggregate>>, IUniquelyIdentifiable<TId>
    {
        TAggregateRoot aggregateRoot = await canGetById.GetByIdAsync(aggregateRootId);

        int addCount = 0;

        foreach (TAggregate aggregate in aggregates)
        {
            if (aggregateRoot.Aggregates.Add(aggregate))
                addCount++;
        }

        return addCount;
    }
}
```

This is very awesome! That is, we're able to add aggregates to any aggregate root by providing the *aggregate root's identifier* and what *aggregates to add*:

```c#
Guid companyId = Guid.Parse("<some existing company GUID>");
ICompanyService companyService = new CompanyService();
await companyService.AddAggregatesToAsync
(
    companyId,
    new []
    {
    	new Employee(),
        new Employee(),
        new Employee()
    }
)
```

Finally, just think how many code lines you could save up with this approach! Because it shouldn't be only applied to *adding aggregates* but also to *removing ones*. And even more use cases...! Anything related to manipulate aggregates within an aggregate root, either write or read operations that may work the same way across the entire solution.

Don't repeat yourself and go for it!