---
layout: design-pattern
title:  "Dynamic tuple / DTO"
date:   2016-06-10 00:00:00 +0100
modifiedDate: 2016-06-10 00:00:00 +0100
categories:
   - architectural
   - c#
   - n-layer
   - strong-typing
   - dynamic-typing
   - duck-typing
authors: 
   - mfidemraizer
permalink: /:categories/:title
---

### Brief description

Relaxes *data-transfer objects'* strong-typing on strongly-typed languages to avoid implementing dozens of different types to cover each upper layers' data retrieval and persistence requirements.

It was first described in a [StackOverflow Q&A in 2011](http://stackoverflow.com/questions/4828024/dto-simplified-dynamic-tuples-how-lets-see-a-possible-solution).

### What does it solve?

In strongly-typed languages like C#, Java, C++ and many others, whenever you need to store *data*, you need a *data structure (i.e. lists, sets, dictionaries, linked lists, tuples...) or *custom types* (almost *classes*) to store it.

While strong typing is a good choice in almost every case, there's a problem when architecting and implementing large *client-server* solutions where two physical layers require bi-directional communication. For example, an HTML5/JavaScript application may need to query and store resources against a Web Service RESTful API. 

There's a common design pattern known as *data-transfer object* which defines that, instead of moving the data across boundaries using a *full object*, we should move *just the data required by both sides in the transaction*.

For example, let's say an user interface grid requires to show a customer list. A `Customer` entity may contain the following data:

- Id
- Name
- LastName
- Age
- IsActivated
- DateAdded
- DateModified

...and we need to show a list of top 15 customers in some user interface grid, and this grid will contain 3 columns: `Id`, `FullName` and `IsActivated`.

Actually transferring full `Customer` objects with all possible properties would we, at least, a waste of time and resources, because the user interface grid requires just 3 of 7 properties. For that reason, the *data-transfer object* patterns says that we should create another *class* to transfer *just what the user interface requires*:

	// Pseudo-code
	class CustomerBriefDto 
	{
		Id,
		FullName, // Concatenation of `Name` and `LastName`
		IsActivated
	}

This is afforable until your system is big enough to need to design more than 50 data-transfer objects. **Each new use case will require a new data-transfer object, and each change on the *domain object* will require a refactor on one or more data-transfer objects.**

### Solution

Sometimes an hyphothetical elegancy isn't enough (i.e. decoupling the domain from application layer data requirements implementing 100 data-transfer objects). The goal of programming and software development is producing good solutions in both terms of technical quality and productivity.

Some programming languages like C# on .NET have the ability to both support strong and dynamic typing:

	// Strongly-typed reference
	string text = "hello world";

	// Dynamically-typed reference
	dynamic text2 = "hello world";
	text2 = 1; // I can set an integer
	text2 = true; // ...also a boolean...

In addition, since the introduction of dynamic typing in .NET 4.0, the framework introduced *dynamic types* and there's one that's very interesting: [`ExpandoObject`](https://msdn.microsoft.com/en-us/library/system.dynamic.expandoobject(v=vs.110).aspx).

Also, one of most popular JSON serializers [JSON.NET](http://www.newtonsoft.com/json), can serialize y deserialize to `ExpandoObject`. That is, **any arbitrary JSON can be treated as just a [POCO](https://en.wikipedia.org/wiki/Plain_Old_CLR_Object)**.

As said before, when a project grows enough, it may happen that you need to implement not 10 but 200 *data-transfer objects* to support all use cases of querying and persisting data, and this situation might led to decrease your productivity and maintainibility. 

One possible solution is the *dynamic tuple / DTO* pattern proposed on this document. It's all about relaxing strong typing under this condition and go with the *[duck-typing](https://en.wikipedia.org/wiki/Duck_typing) approach*, where data-transfer objects are built as expando objects.

### Example

In newer versions of [ASP.NET Web API](http://www.asp.net/web-api), actions' parameter binding can be either setup to be done using custom types (i.e. *classes*) or *dynamic types*.

During configuration stage of ASP.NET Web API its serializer can be configured as follows:

	HttpConfiguration config = new HttpConfiguration();
	config.Formatters.JsonFormatter.SerializerSettings.Converters.Insert(0, new ExpandoObjectConverter());

And a controller may look as follows:

	public class CustomerController : ApiController 
	{
		public Task<IHttpActionResult> SaveCustomer(ExpandoObject dto)
		{

		}
	}

Instead of receiving a custom data-transfer object like `SaveCustomerDto` we just receive an `ExpandoObject`. 

For example, it's very easy to check if a property exists in the `ExpandoObject` because it explicitly implements `IDictionary<string, object>`:

	public class CustomerController : ApiController 
	{
		public Task<IHttpActionResult> SaveCustomer(ExpandoObject dto)
		{
			IDictionary<string, object> dtoAsDict = (IDictionary<string, object>)dto;

			if(dtoAsDict.ContainsKey("name")) 
			{

			}
		}
	}

And you may still access the whole data-transfer object with *dot syntax* typing it as `dynamic`:

	public class CustomerController : ApiController 
	{
		public Task<IHttpActionResult> SaveCustomer(dynamic dto)
		{
			string name = dto.name;
		}
	}