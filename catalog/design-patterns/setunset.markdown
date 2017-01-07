---
layout: design-pattern
title:  "Set-unset value"
excerpt: Proper approach to transport object association changes across software boundaries.
date:   2016-10-21 00:00:00 +0100
modifiedDate: 2016-10-21 00:00:00 +0100
categories:
   - architectural
   - language-agnostic
   - design-pattern
authors: 
   - mfidemraizer
---

### Brief description

Defines that an object property can be setted or unsetted.

### What does it solve?

When working with *data-transfer objects (DTO)* and a *domain layer*, sometimes *DTOs* are mapped to *domain objects* and vice versa. 

Furthermore, when *DTOs* are filled with data that comes from another physical layer *(f.e. an HTTP request entity)*, and some DTO properties are *nullable* or may contain *default values* is very hard to disambiguate an use case where *null* or *default value* is the intended value or its just that the whole object property was not set at all.

Above case defines a problem: when a *DTO* is mapped to a *domain object*, and the whole *DTO* has some properties with *null* or *default values*, do these values should be set to the *domain object* or they should be discarded?

See the following example *DTO* expressed in JSON:

	{
	    "name": null,
	    "age": 0
	} 

And let's say there is a *domain object* in C# that looks as follows:

	public class Employee
	{
		public string Name { get; set; }
		public int Age { get; set; }
	}

Now we got an persistent object from the *repository* which has already a *Name* and *Age* set. If we map the whole *DTO* to that persistent object, the whole object will *unset* `Name` and `Age` properties, because the whole *DTO* has `"name": null` and `"age": 0`. **This can be harmful because it was not intentional: the *DTO* had property defaults just because it came without settings its values**.


### Solution

A possible solution to described problem is refactoring the *DTO* side, where any property that could be both setted with a value or unsetted with its default value should be defined wrapped with a type like the following one:


<img src="/img/setunsetvalue/setunsetvalue.jpg" style="display: block; margin: 0 auto; width: 131px">

For example, the *EmployeeDto* in JSON would look as follows if it would come with property values:

	{
		"name": { "value": "Matías" },
		"age": { "value": 31 }
	}

Otherwise, suppose that `"name"` should be *unsetted*. It would come as follows:

	{
		"name": { "unset": true },
		"age": { "value": 31 }
	}

...which would set `null` to `"name"` string once the *DTO* gets mapped to its corresponding domain object.

That is, thanks to the *set-unset value pattern*, it is possible to disambiguate when the intended value is *setting the default value* or *setting an actual value*.