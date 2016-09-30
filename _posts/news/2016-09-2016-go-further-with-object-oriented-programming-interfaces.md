---
layout: post
title: Go further with interfaces in object-oriented programming
date: 2016-09-30 00:00:00 +0100
modifiedDate: 2016-09-30 00:00:00 +0100
categories: news
authors: 
   - mfidemraizer
permalink: /:categories/:title
---

Undoubtedly if you've already worked with any strongly-typed object-oriented programming language, you should also know that there's a good friend called *interface* and maybe you've implemented some one but you've never found an use case to define one yourself. Perhaps you've both implemented and defined *interfaces*, **but it's worth the effort digging into this topic and discuss some points.**

## Interfaces are...

Usually people define interfaces *as contracts*. Let's see the following code in C# (but it's still applicable to any programming language supporting interfaces!):

	public class EmployeeContract
	{
		public string Content { get; set; }

		public void Sign()
		{
			// Some code to sign this contract...
		}
	}

Someone would say: if any kind of contract should be signed, you could define an interface as follows:

	public interface IContract
	{
		void Sign();
	}

...and our `EmployeeContract` would implement the whole interface:

	public class EmployeeContract : IContract // <--- Implements IContract...
	{
		public string Content { get; set; }

		public void Sign()
		{
			// Some code to sign this contract...
		}
	}

So it seems that we could now implement a class that would be able to store any contract implementing `IContract` and sign them in batch:

	public class ContractManager 
	{
		// See how ContractManager owns a list of objects implementing IContract.
		// It doesn't matter what contracts are stored, but we're absolutely sure that
		// they'll be contracts in the form of IContract!
		public IList<IContract> Contracts { get; } = new List<IContract>();

		public void SignAll()
		{
			foreach(var contract in Contracts) 
			{
				contract.Sign();
			}
		}
	}

Above sample is a common usage of interfaces: an API needs that a given object or set of objects has full guarantees that they will own a member like a property, method, event or whatever, and *interfaces* are the best friend to force objects to fulfill a contract. That is, it seems correct to compare interfaces with **contracts**.


## Interfaces can be used to amplify types!

Apart of considering interfaces as *contracts*, there's another use case that I would call as *type amplifiers*.

Let's take the following situation: I want to describe a car. So I would say that a car...

- ...has doors.
- ...has wheels.
- ...has an engine.
- ...and so on.

If you look further at *car* components you'll find that most of them are common to other vehicles: a motor bike has also wheels and engine, *hasn't it*? ***Oh yeah, I would define a base class to all vehicles and...*** **NOOOOO!!** You wouldn't do that because there're vehicles that work without an engine! What about a bicycle??

So what? *Interfaces!*:

	public interface IHasEngine
	{
		IEngine Engine { get; }
	}

	public interface IHasWheels
	{
		// We want to define an object with wheels, but 
		// we don't know how many wheels will own it!
		// Therefore, we define a list of wheels
		IList<IWheel> Wheels { get; }
	}

And our *car* could look as follows:

	public class Car : IHasEngine, IHasWheels
	{
		public Car(IEngine engine, IList<IWheel> wheels)
		{
			// Dependency injection!
			Engine = engine;
			Wheels = wheels;
		}

		public IEngine Engine { get; }
		public IList<IWheel> Wheels { get; }
	}

The main advantage of going this way is we can ask our objects if they own an engine or wheels, while we won't know if that object is a `Car`, `MotorBike` or even a `Bicycle`:

	// Note that I type the car as just an object
	object obj = new Car
	(
		new EngineImpl(),
		// Doesn't a car has 4 wheels? ;)
		new List<IWheel>() { new WheelImpl(), new WheelImpl(), new WheelImpl(), new WheelImpl() }
	);

Now we could ask que `obj` if it has an engine in a very elegant way:

	if(obj is IHasEngine)
	{
		// I've an engine, but I don't care what kind of vehicle you're right now...!
	}

	if(obj is IHasWheels)
	{
		// Ohhh you've wheels!
	}

Some will call this phenomenom as [*interface segregation principle*](https://en.wikipedia.org/wiki/Interface_segregation_principle).

Ok, I admit that above samples are just micro-contracts, but I didn't want you to focus on that fact that I've defined interfaces to fulfill contracts of how need to be a car engine or its wheels, but I want to focus you on the fact that you're providing more semantics to your software architecture, and you can ask information about your objects **using regular typing language tools** instead of who knows what design smell.

**The best part of using interfaces this way is that you can define detailed contracts of how your objects need to be to work with some given API without mandatorily implement all your interfaces in a class called `Car`, but you leave developers the freedom of implementing their architecture in their own way unless they expose that architecture fulfilling some interfaces to interoperate with some other framework or API**:

	// Why not? A mechanical duck! 
	public class Duck : IHasEngine, IHasWheels
	...

Who cares where developers implement your interfaces: you just want an implementation of your interfaces with an expected behavior. Well, I would say that it would be strange that `IHasEngine` would be implemented by a class called `Employee`, but this is developers' fault, not yours. If an `Employee` works like any vechicle with an engine, then, your API will work with that type of employees... It's absurd, but, at the end of the day, **our conclusion will be that interfaces open the door to develop more behavioral-oriented software architectures where there's a more clear separation of concerns, because APIs are extremely decoupled from implementation details!**
