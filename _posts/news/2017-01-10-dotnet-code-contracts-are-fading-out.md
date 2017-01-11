---
layout: post
title: .NET Code Contracts are fading out
excerpt: Code Contracts are the design by contract approach to .NET. But it's fading out.
date: 2017-01-10 00:00:00 +0100
modifiedDate: 2017-01-10 00:00:00 +0100
categories: news
authors: 
   - mfidemraizer
---

[*Code Contracts*](https://msdn.microsoft.com/en-us/library/dd264808%28v=vs.110%29.aspx){:target="_blank"} has been a Microsoft Research project as an approach to [*design by contract*](https://en.wikipedia.org/wiki/Design_by_contract){:target="_blank"} in .NET Framework.

## Stop... What's *design by contract*?

*Design by contract* is a programming paradigm where code is filled with pre-condition, post-conditions, assertions and assumptions which define *how the code is meant to work*.

For example, have you ever met your wonderful friend called `NullReferenceException`? You get these exceptions when you try to access a member of a null reference to a given object:

```c#
public void DoStuff(string text)
{
	string replacedText = text.Replace("a", "b");
}

// Calling DoStuff(...) this way will throw NullReferenceException
DoStuff(null);
```

Would you use an `if` statement to prevent the whole `NullReferenceException`?

```c#
public void DoStuff(string text)
{
	if(string.IsNullOrEmpty(text)) 
    {
    	return;
    }
    
	string replacedText = text.Replace("a", "b");
}
```

**NO!!** You shouldn't go this route. If your operation expects a `string` and it can't be null, it shouldn't accept a `null` `string`. Design by contract in action:

```c#
public void DoStuff(string text)
{
    // A pre-condition
	Contract.Requires(!string.IsNullOrEmpty(text));
    
	string replacedText = text.Replace("a", "b");
}
```

What about an operation that mustn't return a `null` reference?

```c#
public string DoStuff(string text)
{
    // A post-condition
    Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>());
    
    return null;
}
```

Both pre and post-conditions can be checked during compile-time and run-time. Compile-time checking is very powerful, because it can give hints or errors to let you know that you're building a call stack that can end up in accessing a `null` reference or many other possible wrong usages of your code.

Although there're are many other features in *Code Contracts*, I won't cover them on this article, because we're talking about the fall of *Code Contracts*! But, why *Code Contracts* are also going nowhere [(you might be also interested on why .NET Framework also goes nowhere...)](/news/2017/01/05/dotnet-code-contracts-are-fading-out){:target="_blank"}?

## Why *Code Contracts* are fading out?

Perhaps *Code Contracts* might be a strange monster inside .NET jungle, because the whole feature was introduced in .NET Framework as part of the `System.Diagnostics.Contracts` namespace, but they do nothing *per se*: *they need a post-compilation step*.

Either pre or post-conditions aren't compiled in a regular C# project build: they need a [Visual Studio extension](https://marketplace.visualstudio.com/items?itemName=RiSEResearchinSoftwareEngineering.CodeContractsforNET){:target="_blank"}. 

Strangely, the whole extension has never become a pre-installed package during a Visual Studio installation: it has been remained as a reserach project, but `System.Diagnostics.Contracts` namespace is part of *Base Class Library* within .NET Framework.

The problem with *Code Contracts* is that they need to be reworked to catch up with every new Visual Studio and C# version. For a long time, it was a closed-source project, but since more than a year it turned into a community-driven project hosted in a [GitHub repository owned by Microsoft](https://github.com/Microsoft/CodeContracts){:target="_blank"} in an effort to expand its life.

Sadly, community hasn't been involved in the project nor maintainers could continue putting their effort because they're focused on other projects.

The result is .NET Core support isn't still in the *Code Contracts* roadmap and it seems like there's no intention on going forward on this. Just [review this issue](https://github.com/Microsoft/CodeContracts/issues/409){:target="_blank"} on GitHub to get convinced about the whole conclusion...

Also, you mightn't know that a long time ago there was another research project called *Spec#* which was a superset of C# and it included design by contract as part of its own semantics ([taken from Wikipedia](https://en.wikipedia.org/wiki/Spec_Sharp){:target="_blank"} ):

```c#
static int Main(string![] args)
    requires args.Length > 0;
    ensures return == 0;
{
    foreach(string arg in args)
    {
        Console.WriteLine(arg);
    }
    return 0;
}
```

...and it was also abandoned.

## So sad.

It's very sad that Microsoft hasn't put more resources on *Code Contracts*  to evolve and enhance it, and compatibilize it with .NET Core. 

What's worse, most modern .NET Framework codebase has a lot of *code contracts* defined in it, but it seems that even other .NET-related products like Entity Framework Core has abandoned *code contracts* in favor of the wonders of *Resharper* ([see this source code file as a sample of the whole fact](https://github.com/aspnet/EntityFramework/blob/dev/src/Microsoft.EntityFrameworkCore/ChangeTracking/ChangeTracker.cs){:target=_blank}).

Now, **we *design by contract* followers are orphan**, because excepting some paid solutions like PostSharp (which in turn doesn't provide the same powerness of *Code Contracts*) or Resharper (which also provides some kind of contracts but they're a compile-time only feature), we're stuck with **no choice**. 


Well, yes, we still have a choice: throw them away **and compromise code quality and get back to write more tests again**.


