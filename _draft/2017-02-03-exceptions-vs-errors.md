---
layout: post
title: "Exceptions vs. Errors: Who's who and what are suposed to do"
excerpt: "There's always a controversial discussion on why and when to use exceptions or errors. Let's analyze who's who and what are suposed to do each other"
date: 2017-02-03 00:00:00 +0100
modifiedDate: 2017-02-03 00:00:00 +0100
categories: news
authors: 
   - mfidemraizer
---

There's always a controversial discussion on when and why to throw exceptions, and when *issues* during code execution should be notified as *errors* rather than *exceptions*.

First of all, we should talk about *who's who* on this game.

## What's an exception?

While there's also polemic around what can be considered an exception, I'm going to take my own risks to provide my two cents on this topic.

Whereas *exception* term itself is meaningful and you already understand that you're talking about *exceptional cases*, *exceptions* represent *cases of failure*.

Have your database server been down while your application layer was issuing a query? Probably this has happened many times in your programming life, and in most modern languages an *exception* was the sign of that something gone wrong.

Or, did you call a method and you got an *exception* which told you that an argument wasn't in the expected format?

Let me ask you a question: **what is the problem? The *exception* or *the use case?*** If I would ask this question myself, I would conclude that the problem is *the use case*. Ironically, some might argue that dealing exceptions is one of the most tedious tasks to deal with.

For me, the best way to define what's an exception is **they're the response to an *abnormal but expected usage or behavior of some given API***.

The main advantage of exceptions is that they're an extensible and customizable paradigm to short circuit a program and perform decisions based on them, to whether terminate the program or recover it. In the other hand, in most languages they're heavy to build them because they pick a bunch of data to make code debugging easier (they include a stack trace/callstack, a message, reasons...).

Moreover, most programming languages provide the `try/catch/finally` block in order to handle exceptions in the caller's scope or even a global exception handler to have a *last chance* to work them out all.

At the end of the day, *exceptions are thrown away*, and an improper handling can terminate the program unexpectedly.

## And what about *errors*.

In the other side, we find the *errors*. Either when they're a built-in or custom solution of some given programming language or development framework, *errors* are just a composite data where either the runtime environment or you the developer put relevant data when something gone wrong, right? ***So what's the difference between exceptions and errors?***

First of all, we should mention that legacy languages didn't provide *exceptions* and they just worked with *errors*. 

The main difference between errors and exceptions is that errors aren't *throwable and catchable* and usually they're *returned* in functions/methods or *fired* using events. 

Either way, if some programming language works with errors, they could potentially work for the same goal as exceptions, but in modern programming languages where they fully work with exceptions, ***errors* aren't conceptually part of the language, but an approach we may use in some scenarios** because we don't consider that some use cases are *exceptional cases*, but ***just expected cases on which we don't expect them to affect general program stability and reliability***.

For example, there's an ethernal discussion about how to express that a business rule has been broken. Do we throw an exception or use errors? 

Think about the following case: you're developing a banking software and functional analysts have told that current Law requirements disallow performing transactions of more than 2,000€ at once (they need to be chunked into many transactions, otherwise they must be notified to Treasury!). And customer tries to perform a transactions out of the permitted range. **So you throw an exception?** `throw new TransactionRuleViolation("Transactions over 2,000€ are currently forbidden")`? **I'm against this approach**. 

Ask yourself these questions:


> **Q: Is a forbidden transaction an *exceptional case*?**
> 
> **A:** *I don't think so. Basically, it's an attemp made by an user to perform a transaction and the program should notify that the transaction should be made for less than 2000€*.

> **Q: Does a given customer fully know the laws being applied to the transaction?**
>
> **A:** *It's unlikely to happen that a customer could be absolutely aware of limitations enforced by laws. The program is responsible of noticing the user that the whole transaction must be done fulfilling the laws.*

> **Q: Should you consider a broken business rule in the same group as an unexpected database server downtime?**
>
> **A:** *Isn't the question itself illogical?*

Hence *exceptions* should be avoided when we need to inform that there were one or many business broken rules, and *errors* should be used instead.

For example, let's think about another case like registering a new user in C#:

```c#
RegistrationResult result = userService.RegisterOne(name: "matias", password: "123");
```

`RegistrationResult` class would look as follows in C#:

```c#
public class RegistrationResult
{
	public RegistrationResult(IList<string> errors = null)
    {
        Errors = errors;
    }
    
    public bool Success => Errors?.Count == 0;
    public IList<string> Errors { get; }
}
```

...and our registration code would look like this:

```c#
public RegistrationResult RegisterOne(string name, string password)
{
	RegistrationResult result;
	User user = new User { Name = name, Password = password };
    
    if(UserRepository.Exists(user.Name))
    	result = new RegistrationResult(new List<string> { "An user with given name already exists" });
    else
   	{
    	UserRepository.Add(user);
        
        result = UserRegistrationResult();
    }
    
    return result;
}
```

