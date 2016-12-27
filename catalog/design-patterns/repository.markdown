---
layout: design-pattern
title: Repository
date: 2016-05-16 00:00:00 +0100
modifiedDate: 2016-05-16 00:00:00 +0100
categories:
    - language-agnostic
    - object-mapping
    - domain-driven-design
    - design-pattern
authors: 
   - eevans
   - mfidemraizer
---

### Brief description

Defines an object-oriented-friendly collection-like interface which persists and retrieves domain objects, and abstracts the domain from details on how to handle *domain object to data* and *data to domain object* translation.

### What does it solve?

In order to implement a *clear separation of concerns*, the *domain* shouldn't be responsible of implementing queries or persistence logic, but it should just implement simple or complex operations to get the state of some domain object and its associations, or operate over one or more domain objects to alter them. 

Otherwise, usually arises a *big issue*: if domain would be the responsible of implementing querying and persistence, and many operations within the whole domain and others would need to *repeat* that code, wouldn't be a high risk of breaking the domain itself? That is, each part of a given domain would be able to arbitrarily decide *how* to work with the persistence layer.

Another issue is that, *how could domain rules be enforced by default to avoid breaking the whole domain form a central place?*.

### Solution

A clear separation of concerns on domain implementations introduces the requirement of having a code which can be identified as *still domain* but also as a boundary between the domain and *the data persistence layer*. This is what *repository* actually does.

### Implementation details 

In strongly-typed object-oriented programming languages, *repository* may be defined as an interface with a generic set of operations that may happen in any domain:

	// Pseudo-code
	interface IRepository
	{
		void AddOrUpdate(DomainObject);
		void Remove(DomainObject);
		DomainObject GetById(DomainObjectId);
	}

And using the [*interface segregation principle*](https://en.wikipedia.org/wiki/Interface_segregation_principle), if more operations are required (i.e. *to implement more specific queries or persistence operations*), more specific interfaces can be defined:

    // Pseudo-code
    // This interface extends IRepository with more requirements
	interface ICustomerRepository : IRepository
	{
		// A query to get top customers with pagination
		IList GetTopCustomers(startFrom, count);

		// Remove a customer by its alphanumeric code
		void RemoveByCode(code)
	}

In dynamically-typed languages like JavaScript which don't include *interface* semantics (they work using *duck-typing*), repositories can be implemented as either classes, prototypes or object literals:

	class Repository {
		addOrUpdate(domainObject) {}		
		remove(domainObject) {}
		getById(domainObject) {}
	}

	// ...or...
	var Repository = function() {};
	Repository.prototype = {
		addOrUpdate: domainObject => {}		
		remove: domainObject => {}
		getById: domainObject => {}
	}

	// ...or...
	var Repository = {
		addOrUpdate: domainObject => {}		
		remove: domainObject => {}
		getById: domainObject => {}
	};

Implementation details of a given repository interface may vary depending on which is the underlying data storage. For example, the data storage may be SQL or *No*SQL:

- On *SQL* (i.e. *relational*) storage, a data mapper layer which is usually an *object-relational mapper* (OR/M) and repository may use it to persist and query data.
- On *NoSQL* storage, there could or couldn't be a data mapper at all, since some are very object-oriented friendly or provide an easy path to serialize objects to a string or binary representation of them.

Whichever is the underlying data storage paradigm, it's important that queries implemented as methods of a given repository should start and end in their declaring repository. That is, a given layer should perform operations agains some repository and repository shouldn't let an upper layer specialize a query that can be represented by a repository operation. In other words, a *GetAll()* operation is discouraged:

	// Pseudo-code
	interface ICustomerRepository
	{
		// Returns all stored customers...
		IList GetAll();
	}

	// Retrieving all customers...
	IList allCustomers = customerRepository.GetAll();
	// ...and now we could apply filters on the returned list:
	IList customersStartingWithALetter = allCustomers.Where(customer => customer.Name.StartsWith("A"));

It is absolutely discouraged because:

- It may retrieve all objects to memory.
- Some simple or complex filters and other kind of operations may be sent as commands to the underlying data storage, and a given data mapper or data access strategy may retrieve the filtered data already. Using a `GetAll()` operation can't be implemented this way.
- A `GetAll()` derives the responsibility of querying persisted domain objects to a layer different than the repository. The role of a repository is to mediate between the domain and a data mapper or other data access strategies. If the domain can add behavior to a query started by a given repository, then the repository itself is useless.