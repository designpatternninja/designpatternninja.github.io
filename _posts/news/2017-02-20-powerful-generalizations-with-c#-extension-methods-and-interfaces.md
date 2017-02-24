---
layout: post
title: "Powerful generalizations using C# extension methods, generics and interfaces"
excerpt: "Tired of not finding a compelling use case for C# extensions methods, generics and interfaces? Check this article as soon as possible!"
date: 2017-02-20 00:00:00 +0100
modifiedDate: 2017-02-20 00:00:00 +0100
categories: news
authors: 
   - mfidemraizer
---

Say you're a developer working on an average project and you get used to some programming approaches and when C# 2.0 introduced generics you said '*oh nice! generic collections!'* but since 2004 you've found no compelling reason to use *generics* in your own custom types.

Also, later on, C# 3.0 introduced *extension methods* and you realized how powerful was (and still is) LINQ, didn't you? But since 2007, you're still consuming existing extension methods defined in both the .NET Framework *base class library* or many satellite frameworks but, again, you're either just being a consumer of them, or you implement extension methods as simple utility methods...

Finally, you regularly implement interfaces and in few cases you define your own ones.

So if you're reading to this article is because you want to know how to take more advantage of these three mentioned C# language features, right?

## Let's introduce an actual use case

Imagine you're implementing a project where you can *tag* certain entities. Of course... Now you've StackOverflow.com in your mind but ok, it's a good sample of my hypothetical use case.

Moreover, you're going to define and implement a service layer. Think that we're developing a CRM where the central entity is `Customer` and each one has many contacts (i.e. many instances of a `Contact` entity):

```c#
// For now we don't care about what properties
// may define each entity...
public class Customer
{
}

public class Contact
{
}
```

Oops, I forgot to design a third entity, didn't I? 

```c#
public class Tag
{
	public string Name { get; set; }
    public string Description { get; set; }
}
```

Do these entities need to be uniquely identifiable? Probably yes, because you're going to get them someway and you'll need an `Id`. For that reason, I'm going to design an interface called `IUniquelyIdentifiable` (don't ask me why yet, continue reading the rest of this article please!):

```c# 
public interface IUniquelyIdentifiable<out TId>
{
	TId Id { get; }
}
```

...and we'll design a base class called `DomainObject` which will implement `IUniquelyIdentifiable`:

```c#
public abstract class DomainObject : IUniquelyIdentifiable<Guid>
{
	public Guid Id { get; set; }
}
```

Now our three entities will inherit `DomainObject`:

```c#
public class Customer : DomainObject
{
}

public class Contact : DomainObject
{
}

public class Tag : DomainObject
{
	public string Name { get; set; }
}
```

If I'm not mistaken, I said that both `Customer` and `Contact` entities are *taggable*, thus I expect them to have a *has many* association with `Tag`:

```c#
public class Customer : DomainObject
{
	public ISet<Tag> Tags { get; set; } = new HashSet<Tag>();
}

public class Contact : DomainObject
{
	public ISet<Tag> Tags { get; set; } = new HashSet<Tag>();
}
```

Note that I used `ISet<T>` collection, because I don't care about the *tag ordering*, but I care about being sure that `Tags` collections contains *unique tags*. Therefore, I'll need to override `Object.GetHashCode()`, `Object.Equals(object)` and implement `IEquatable<T>` on `DomainObject` base class in order to let any entity be equatable or stored in a *hashed collection*:

```c#
public class DomainObject : IUniquelyIdentifiable<Guid>, IEquatable<T>
{
	public Guid Id { get; set; }
    
	public virtual bool Equals(Tag other)
    {
    	if(other == null) return false;
        
        return ReferenceEquals(this, other) || Id == other.Id;
    }
    
    public virtual override bool Equals(object other) => Equals(other as Tag);
    public virtual override int GetHashCode() => Id.GetHashCode();
}
```

The next step could be defining two *service* interfaces: `IContactService` and `ICustomerService`. We'll omit *CRUD*-style methods and we'll get focused on defining a method to add tags to a given `Customer` or `Contact`:

```c#
public interface IContactService
{
	void AddTags(Guid contactId, IEnumerable<Tag> tagsToAdd);
}

public interface ICustomerService
{
	void AddTags(Guid customerId, IEnumerable<Tag> tagsToAdd);
}
```

So we implement them:

```c#
public class DefaultContactService : IContactService
{
	public DefaultContactService(IContactRepository repository)
    {
    	Repository = repository;
    }
    
    private IContactRepository Repository { get; }
    
	public void AddTags(Guid contactId, IEnumerable<Tag> tagsToAdd)
    {
    	Contact contact = Repository.GetById(contactId);
        
        foreach(Tag tag in tagsToAdd)
        {
        	contact.Tags.Add(tag);
        }
    }
}

public class DefaultCustomerService : ICustomerService
{
	public DefaultCustomerService(ICustomerRepository repository)
    {
    	Repository = repository;
    }
    
    private ICustomerRepository Repository { get; }
    
	public void AddTags(Guid customerId, IEnumerable<Tag> tagsToAdd)
    {
    	Customer customer = Repository.GetById(customerId);
        
        foreach(Tag tag in tagsToAdd)
        {
        	contact.Tags.Add(tag);
        }
    }
}
```

Hold... did you see that I'm repeating myself? **Do you remember the *DRY* (i.e. Don't Repeat Yourself) principle?** Just try to envision how can this service layer may grow when dozens of entities might need to be also tagged! And it's the same method all over again just changing the *taggable entity*...

## The party starts now!

Our goal is to implement the `AddTags` once and support any *taggable entity* and call it from any *service*. Did I say *taggable entity*? This sounds like I need an *interface*.  

Wait... what if I want to implement that generalized `AddTags` as part of an infrastructure framework shared by many projects? Would you couple your CRM and Forum using the same `Tag` entity? Another question: do all projects need just a tag *identifier* and *name*? What if some project requires a *tag description*? *Hmmmm...* This also sounds like we need a *tag interface*!

```c#
public interface ITag
: IUniquelyIdentifiable<Guid>
{
	string Name { get; }
}

public interface ITaggable<TTag>
	   where TTag : ITag
{
	ISet<TTag> Tags { get; }
}
```

First of all, we're going to implement `ITag` on `Tag` entity:

```c#
// Tag already implements the required 
// members to fulfill ITag's contract
public class Tag : DomainObject, ITag
{
	public string Name { get; set; }
}
```

Furthermore, we still need to implement `ITaggable<TTag>` interface on both `Contact` and `Customer`:

```c#
public class Customer : DomainObject, ITaggable<Tag>
{
	public ISet<Tag> Tags { get; set; } = new HashSet<Tag>();
}

public class Contact : DomainObject, ITaggable<Tag>
{
	public ISet<Tag> Tags { get; set; } = new HashSet<Tag>();
}
```

OK, now I want you to see the non-generalized `AddTags` method body:

```c#
public void AddTags(Guid contactId, IEnumerable<Tag> tagsToAdd)
{
    Contact contact = Repository.GetById(contactId);

    foreach(Tag tag in tagsToAdd)
    {
        contact.Tags.Add(tag);
    }
}
```

If you look at it you should identify the following facts to build a good generalization for this method:

1. The tags being passed as arguments should be of type `ITag`.
2. You don't want a specific *taggable entity* but just a *taggable entity*.
3. You don't want to get a *taggable entity by its identifier* against a specific repository.

Our generalization will be tailored as an *extension method* but we still have some some issue to solve before we can go forward: if we want to generalize `AddTags`, since it'll require an implementation of a given repository, we could define a repository interface that any repository should fulfill, but we would be forcing the caller of the future extension method to give a repository instance as argument and we want to only keep two parameters as the non-generalized version of the so-called method.

In the other hand, do you find that a method like `AddTags` should be a *repository method*? I believe that it goes outside the scope and responsibilities of a repository, because *it adds tags to an aggregate root* hence it sounds more like a service layer operation. This conclusion introduces a new requirement: **services should expose a method to get entities by their unique identifier**, but we want to put in practice the *interface segregation principle* and a *good separation of concerns*, thus any service which may want to take advantage of our `AddTags` generalization **must** implement a method to get entities by their *identifier*. So, we need a new interface:

```c#
public interface ICanGetObjectsById<TId, TObject>
{
	TObject GetById(TId id);
}
```

So we add it to both `IContactService` and `ICustomerService` interfaces, and we're going to drop `AddTags` methods:

```c#
public interface IContactService : ICanGetObjectsById<Guid, Contact>
{
}

public interface ICustomerService : ICanGetObjectsById<Guid, Customer>
{
}
```

...and we implement the new method on both service interface implementations:

```c#
public class DefaultContactService : IContactService
{
	public DefaultContactService(IContactRepository repository)
    {
    	Repository = repository;
    }
    
    private IContactRepository Repository { get; }
    
    public Contact GetById(Guid id) => Repository.GetById(id);
}

public class DefaultCustomerService : ICustomerService
{
	public DefaultCustomerService(ICustomerRepository repository)
    {
    	Repository = repository;
    }
    
    private ICustomerRepository Repository { get; }
    
    public Customer GetById(Guid id) => Repository.GetById(id);
}
```

Finally we got all the pieces to make the magic!

```c#
public static class TaggingExtensions
{
	// This method can add tags to any object that can get taggable objects
    // by their identifier, and tag can be any object implementing ITag.
	public static void AddTags<TId, TObject, TTag>(this ICanGetObjectsById<TId, TObject> objectByIdGetter, TId id, IEnumerable<TTag> tags)
            where TObject : ITaggable<TTag>, IUniquelyIdentifiable<TId>
            where TTag : ITag
    {
    	TObject someObject = objectByIdGetter.GetById(id);
        
        foreach(TTag tag in tags)
        {
        	someObject.Tags.Add(tag);
        }
    }
}
```

The whole extension method would be used as follows:

```c#
IContactService contactService = new ContactService(new ContactRepository());
contactService.AddTags
(
	contactId, 
    new [] { new Tag { Name = "a" }, new Tag { Name = "b" } }
);
```

Maybe you feel that we did an excercise of over-engineering, but think that a few interfaces and small effort brings us the chance to implement how tags are added to all entities within many enterprise projects. Do you want to do something before and after adding tags to some entity? You change it once and it's done for all!

Perhaps you've already found that there might be some issue with this approach. The *extension method to rule them all* is nice unless you need to customize how to add tags for a particular *taggable entity*. No worries on this: you can perfectly add an interface method `AddTags` to the service that requires the whole customization and C# compiler will call it instead of the generalized `AddTags` extension method!

I hope you enjoyed this coding adventure! **Feel free to comment out this article if you don't fully undestood the approach or you've any doubt about the topic.**

[Download the full source code](/img/news/powerful-generalizations-with-c-extension-methods-and-interfaces/full-code.cs) that compiles if you want to play with it. **Don't expect to execute anything with that sample code, it's just for the sake of demonstrating that we've achieved our goal of adding the whole extension method based on what we've learnt on this article**.
