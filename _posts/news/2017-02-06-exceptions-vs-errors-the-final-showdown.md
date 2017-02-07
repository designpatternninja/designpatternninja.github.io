---
layout: post
title: "Exceptions vs. Errors. The final showdown."
excerpt: "A comprehensive analysis about when to use exceptions or errors. Comment out to provide your own thoughts!"
date: 2017-02-06 00:00:00 +0100
modifiedDate: 2017-02-06 00:00:00 +0100
categories: news
authors: 
   - mfidemraizer
---

There's always a controversy about when and why to throw exceptions, and when *issues* happening during code execution should be notified as *errors* rather than *exceptions*.

First of all, we should talk about *who's who* on this game.

## What's an exception?

While there's also polemic around what can be considered an exception, I'm going to take my own risks to provide my two cents on this topic.

Whereas *exception* term itself is meaningful and you already understand that we’re talking about exceptional cases, exceptions represent cases of failure.

Have your database server been down while your application layer was issuing a query? Probably this has happened many times in your programming life, and in most modern languages an *exception* was the sign of that something gone wrong.

Or, did you call a method and you got an *exception* which told you that an argument wasn't in the expected format?

Let me ask you a question: **what is the problem? The *exception* or *the use case?*** If I would ask this question myself, I would conclude that the problem is *the use case*. Ironically, some might argue that dealing exceptions is one of the most tedious tasks to deal with.

For me, the best way to define what's an exception is **they're the response to an *abnormal but expected usage or behavior of some given API***.

The main advantage of exceptions is that they're an extensible and customizable paradigm to short circuit a program and perform decisions based on them, to whether terminate the program or recover it. In the other hand, in most languages they're heavy to build them because they collect a bunch of data to make code debugging easier (they include a stack trace/callstack, a message, reasons...).

Moreover, most programming languages provide the `try/catch/finally` block in order to handle exceptions in the caller's scope or even a global exception handler to have a *last chance* to work them out all.

At the end of the day, *exceptions are thrown away*, and an improper handling can terminate the program unexpectedly. In fact, there've been always a discussion around this. Some exceptions are recoverable, others have already corrupted the program state and they should terminate the program notifying the user about what happened and what's next.

## And what about *errors*.

In the other side, we find the *errors*. Either when they're a built-in or custom solution of some given programming language or development framework, *errors* are just a composite data where either the runtime environment or you the developer put relevant data when something gone wrong, right? ***So what's the difference between exceptions and errors?***

First of all, we should mention that old programming languages didn't provide *exceptions* and they just worked with *errors*. 

The main difference between errors and exceptions is that errors aren't *throwable and catchable* and usually they're *returned* in functions/methods or *fired* using events. 

Either way, if some programming language works with errors, they could potentially work for the same goal as exceptions, but in modern programming languages where they fully work with exceptions, ***errors* aren't conceptually part of the language, but an approach we may use in some scenarios** because we don't consider that some use cases may be *exceptional cases*, but ***just expected cases on which we don't want them to affect general program stability and reliability***.

For example, there's an eternal discussion about how to express that a business rule has been broken. Do we throw an exception or return errors? 

Think about the following case: you're developing a banking software and functional analysts have told that current Law requirements disallow performing transactions of more than 2,000€ at once (they need to be chunked into many transactions, otherwise they must be notified to Treasury!). And customer tries to perform a transactions out of the permitted range. **So, would you throw an exception?** `throw new TransactionRuleViolation("Transactions over 2,000€ are currently forbidden")`? **I'm against this approach**. 

Ask yourself these questions:


> **Q: Is a forbidden transaction an *exceptional case*?**
> 
> **A:** *I don't think so. Basically, it's an attemp made by an user to perform a transaction and the program should notify that the transaction should be made for less than 2000€*.

> **Q: Does a given customer fully know the laws being applied on the transaction?**
>
> **A:** *It's unlikely to happen that a customer could be absolutely aware of limitations enforced by laws. The program is responsible of notifying the user that the whole transaction must be done fulfilling the laws.*

> **Q: Should you consider a broken business rule in the same group as an unexpected database server downtime?**
>
> **A:** *Isn't the question itself illogical?*

Therefore, *exceptions* should be avoided when we need to inform that there are one or many business broken rules, and *errors* should be used instead.

For example, let's think about another case like registering a new user in C#:

```c#
RegistrationResult result = userService.RegisterOne(name: "matias", password: "123");
```

`RegistrationResult` class would look as follows in C#:

```c#
public class RegistrationResult
{
	public RegistrationResult(IDictionary<string, string> errors = null)
    {
        Errors = errors;
    }
    
    public bool Success => Errors?.Count == 0;
    public IDictionary<string, string> Errors { get; }
}
```

...and our registration code would look like this:

```c#
public RegistrationResult RegisterOne(string name, string password)
{
	RegistrationResult result;
	User user = new User { Name = name, Password = password };
    
    if(UserRepository.Exists(user.Name))
    	result = new RegistrationResult
        (
        	new Dictionary<string, string> 
            { 
            	{ "Name", "An user with given name already exists" }
            )
        }
    }
    else
   	{
    	UserRepository.Add(user);
        
        result = new UserRegistrationResult();
    }
    
    return result;
}
```

Notice how an already taken *user name* doesn't need to be *handled* to avoid an *operation failure*, but the operation just produces a *failed resultation result* because `RegistrationResult.Success` would be `false`.

This is simpler than handling exceptions. See how would look POSTing an user on ASP.NET WebAPI / MVC Core:

```c#
public IHttpActionResult RegisterUser(RegisterUserDto dto)
{
    RegistrationResult result = UserService.RegisterUser(dto.Name, dto.Password);
    
    if(result.Success)
    {
    	return Ok();
    }
    else
    {
    	foreach(var error in result.Errors) 
        {
    		ModelState.Add(error.Key, error.Value);
        }
        
    	return BadRequest();
    }
}
```

Hypothetically, if `UserService.RegisterUser` would use *exceptions*, the whole POSTing would look like this:

```c#
public IHttpActionResult RegisterUser(RegisterUserDto dto)
{
	try
    {
    	UserService.RegisterUser(dto.Name, dto.Password);
        
        return Ok();
    }
    catch(UserTakenException)
    {
    	ModelState.Add("Name", dto.Name);
        
        return BadRequest();
    }
}
```

*Less code lines, right?* Yes, they're less lines. For me, the *exception-based flavor* smells as hell because **you get forced to *handle the exception in place*, otherwise the exception ends up producing an ugly and unclear HTTP/500 error** which is the core of the *bad smell*.

Let's summarize some reasons to use errors when there's no exceptional case:

- If you've already handled an exception at some point, unless you manually *rethrow* it, no one will be able to receive operation result details! 
- Leveraging *errors* you're not constraining *the caller* or the entire *call stack* to *handle an exception **or die*** nor you're missing the chance to do certain actions based on the errors across the call stack.
- Exceptions are too specific. While you could implement a single exception type called `ValidationException` which could come with many broken rules, you would defeat the purpose of exceptions, because you would be able to catch `ValidationException` instances but not a specific *broken rule*, hence you fall into implementing many `if` statements inside a `catch` block to handle each broken rule. Basically this is what you already do with errors.
- You save up resources. When you throw an exception, the runtime collects some information like current stack trace and other debugging information. Do you need this information just because an user name is too short?

## Summary

From my perspective, the summary can be splitted into the following two points:

- **Use exceptions** when you need to short circuit the flow which otherwise would leave the program into a wrong execution state leading to an unusable program.
- **Use errors** when an operation has evaluated that some input has no sense and the *erroneous state* isn't a threat against the program stability but its data integrity.

Note the slight difference between exceptions and errors: issues related to program stability versus those related to integrity. **This is the key point that makes the difference**.

Translated into the real world: **an atomic bomb explosion vs skipping the traffic light!** 

Would you believe a policeman fining you because you pressed the wrong button? Yeah, unless this policeman would be a *good* Terminator... ;)