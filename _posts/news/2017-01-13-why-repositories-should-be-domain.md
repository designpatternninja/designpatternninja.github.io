---
layout: post
title: Why repositories should be domain?
excerpt: Let's discuss why repositories are a domain concern.
date: 2017-01-13 00:00:00 +0100
modifiedDate: 2017-01-13 00:00:00 +0100
categories: news
authors: 
   - mfidemraizer
---

*Let's discuss why [repositories](/catalog/design-patterns/repository){:target="_blank"} are a domain concern*.

Nowadays [*domain-driven design*](https://en.wikipedia.org/wiki/Domain-driven_design){:target="_blank"} (from now, just *DDD*) is one of top trending programming paradigms since some years when any organizations think about implementing well tailored enterprise-grade solutions.

And then many of us ask ourselves a lot of questions because we're worried about architecting our solutions in the right way. Probably you already know that implementing a DDD-style architecture turns into deciding what's *domain*, *infrastructure*, *application*... Whenever you're going to design an interface or an implementation, you end up asking yourself: *where I put it?* (*actually experience is a good friend to go forward because finally you build a skillset and know-how meaning that you're not always asking yourself the same question all over again...*).

Inside DDD-ish solutions you need to implement repositories, because you want to keep your *domain* absolutely agnostic to *how* your domain objects are being persisted and also how they're retrieved from the underlying storage engine. Also, you don't want to repeat yourself, therefore centralizing querying and write operations in a single place (i.e. *the repository*) will enforce code reusability since many domain services will call repository's operations and they will focus their responsibility on implementing higher-level domain operations like calculations, decision making...

But... ***are repositories domain***?

## Why repositories aren't an infrastructure *concern*

There's a certain tendency on pointing out that repositories are an *infrastructure* concern. I could summarize the reasons I've found around the net about that statement:

1. Repositories are coupled with data mapping layer, or directly with a NoSQL, relational or file API, which all of them are infrastructure concerns.
2. Repository contracts (i.e. *interfaces* in most OOP languages) are infrastructure concerns.

Actually, both statements are right. Both #1 and #2 are founded in the fact that usually all repositories share some fundamental operations like *add*, *update*, *remove*, *get by id* and *list*. For example, in C# you would design an interface like this:

```c#
// T is a generic type parameter which will be a concrete domain object
// type once we implement this interface into some class.
public interface IRepository<T>
{
    void Add(T domainObject);
    void Update(T domainObject);
    void Remove(T domainObject)
    T GetById(T domainObject);
    IList<T> List(int skip = 0, int take = 0);
}
```
    
And, probably, if you want to share some basic behaviors across your entire project, you would implement an abstract class which would be derived by any concrete repository:

```c#
public abstract class Repository<T> : IRepository<T>
{
    public virtual void Add(T domainObject)
    {
        if(domainObject == null)
            throw new ArgumentNullException("domainObject");

		// Anything that should be done before actually adding
        // the domain object to the underlying store should be done here.
        
        // Once whatever stuff has been already done, we call DoAdd(...)
        // to let a concrete repository implementation persist given
        // domain object.
        DoAdd(domainObject),
    }

    protected abstract void DoAdd(T domainObject);

    // Rest of IRepository<T> method implementations either implemented
    // as abstract members or like Add(...), we could provide some basic
    // argument validation or a lot of other stuff. But we want to keep
    // this sample code as simple as possible.
}
```

Hence, because both contract and abstract implementation aren't specific to a *domain*, we would conclude that ***they should be an infrastructure concern*** (and so they're!).

My other conclusion is that **above described approach is an implementation detail, and it's not necessarily the repository pattern *per se*** even when I would really advocate to always define a repository pattern interface and a basic abstract class to avoid repeating yourself across your project when you need to implement things that should affect it entirely.

### Let's move on to the next and definitive argument

When architecting a DDD solution, there's a simple rule that says *define a repository to each aggregate root*, right? For example, if we've an *invoicing domain*, usually you define domain objects like these:

- Invoice
- InvoiceLine
- ...probably others.

Our aggregate root is *Invoice*, because an invoice can live *alone* while an invoice line lives inside an invoice. I shouldn't be mistaken on this.

So we define a repository interface to handle invoices:

```c#
// We also implement IRepository<T> to don't repeat ourselves... We really
// want to avoid defining GetById, Add, Remove... all over again...! Thus,
// we focus on providing domain-specific repository operations.
public interface IInvoiceRepository : IRepository<Invoice>
{
    // Retrieves all invoice lines from a given invoice by its identifier.
	IList<InvoiceLine> GetInvoiceLines(Guid invoiceId);
}
```

Stop here. Note how `IInvoiceRepository` is heavily tied to two specific domain inhabitants: `Invoice` and `InvoiceLine`. 

Are `Invoice` and `InvoiceLine` domain? Absolutely, yes they're domain objects of *Invoicing domain*. And what's handling the whole repository? It mediates between the domain and data mapping layer so *holds dependencies to both layers*... **No!** ***This couldn't be true.*** 

Modern software development has a heavily foundation on another paradigm/design pattern which you should already know: *dependency injection* and *inversion of control*. And also follow up SOLID principles (not every principle, but at least we should get inspired by them). So, aren't you coupling your code against interfaces instead of classes? If the answer is *yes*, I would ask myself: *does `IInvoiceRepository` has any dependency with the data mapping layer?* **No. But it's absolutely tied to the domain layer, because it handles *specific domain objects*!**

Conclusion: **repository is domain *per se***. And it's tightly coupled both to other domain concerns as long with the data mapping layer.