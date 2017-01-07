---
layout: post
title: Why and when to use abstract classes
excerpt: You've used many abstract classes, but maybe you don't find an use case to implement your own abstract classes.
date: 2017-01-01 00:00:00 +0100
modifiedDate: 2017-01-01 00:00:00 +0100
categories: news
authors: 
   - mfidemraizer
---

Practically all mainstream object-oriented programming languages are class-based, and *classes* are one of most basic constructs within the so-called programming paradigm to define custom types:

	// Pseudo-code
    class Person 
    {
        string name;
        int age;
    }
    
Also, when we implement libraries or frameworks, it might happen that third-party developers might need to integrate their code into your own system, but you don't want to share your source code. Also, you don't want to force them to implement code to integrate in yours in a specific way, but you just want to force it to implement certain properties and behaviors and you don't care how they're implemented unless they do what your system expect from an integration. Usually programming languages provide *interfaces* to define contracts to classes (and structures):
	
    // Pseudo-code
	interface IPerson
    {
    	sayMyName();
    }
    
    class Person implements IPerson
    {
        string name;
        int age;
        
        sayMyName()
        {
        	Output("My name is " + this.name + " and I'm " + this.age);
        }
    }
	
Anyway, there's a third contender called *abstract classes*, but what's its purpose after all?

## Abstract classes desmifitied!

First of all, abstract classes can't be instantiated like regular classes. Some languages like Visual Basic .NET don't call them *abstract classes* but there's a modifier `MustInherit`. 

Thus, it seems like we could define *abstract classes* like *classes that can't be instantiated and must be derived in some second class*, but I would find this a poor definition of them...

Abstract classes are a kind of classes that mix both concepts of *regular classes* and *interfaces*. That is, it's like a well-defined concept which still don't know how to behave at all. For example, my 1-year-old son is a kid which still requires some implementation details... he still can't code software...!), but he already knows how to say some words and he can walk! 

So let's define that all human can *potentially* walk, speak and think but, as far as I know, everyone can breath and cry (we'll define these common human features as an abstract class).

Therefore, we may conclude that:

- Breath and cry are common to all human. We do them since our moms have gave us birth, am I mistaken? 
- In the other hand, I shouldn't be mistaken if I define that we don't walk, speak or think when our moms have gave us birth, don't we?

See how I may reflect above facts implementing an abstract class:

	// Pseudo-code
	abstract class Human
    {
    	// We all birth with these skills...
    	breath()
        {
        	// Stuff to implement how to breath
        }
        
        cry() 
        {
        	// Stuff to implement how to cry :_(
        }
        
        // While these skills or behaviors can be achieved during our 
        // first 6 years of life, we define that all human may finally
        // walk, speak and think, but we don't know how just because we're
        // human. Oops! This is more lika a philosophical debate... ;) 
        // In summary, walk, speak and think are abstract skills/behaviors.
        abstract walk();
        abstract speak();
        abstract think();
    }


Now we could define a new class to represent what a one-year-old kid might be able to do, and that class would inherit `Human`. Note that it won't be an *abstract* class anymore, because `Human` is *the concept* and a one-year-old kid *is a possible human*:

	// Pseudo-code
	class OneYearKid : Human
    {
    	override walk()
        {
        	// Stuff to implement how to walk
        }
        
        // For now, a one year kid wouldn't speak or think, and usually
        // he/she would cry instead... So we call Human's cry() implementation
        // from the base class on both behaviors
        override speak() 
        {
            cry();
        }
        
        override think()
        {
            cry();
        }
    
    }

Yeah! Many kids can walk almost once they're 1 year old, but they can't still *speak* or *think*... See how I used `override` (hello *polymoprhism*) in order to implement how to walk on any human.

Moreover, a three-year kid should speak a lot of words and it would provide an enhanced implementation to `speak`:

	// Pseudo-code
	class ThreeYearKid : OneYearKid
    {
    	override speak()
        {
        	// Stuff to implement enhanced speaking skills
            if(isInVocuabulary)
            {
            	// Speak words found on a three-year-old kid possible
                // vocabulary
            }
            else
            {
            	// Still cry, a three-year-old kid would cry if can't express
                // something who doesn't know how to say it... :_(
            	cry();
            }
        }
        
        override think()
        {
        	if(isBasicThinking)
            {
            	// Think very basic things...
            }
            else
            {
            	// Cry! I don't understand what my parents are trying
                // to teach me... :_(
                cry();
            }
        }
    }

At the end of the day, it seems like an *abstract class* is an approach to define concepts that can potentially do a lot of stuff, but some of its behaviors are defined but not implemented, because implementations will be provided by more concrete concepts (i.e. *derived classes*).

## When to favor abstract classes over interfaces?

Actually there're few corner cases where you would use an *abstract class*, because most modern programming languages hopefully doesn't support *class multi-inheritance*, and using *abstract classes* to define abstract properties and behaviors just because you want a derived class to fulfill certain requirements isn't enough to use them.

In my opinion, **if you thought about an *abtract class* you should define some interface before**. 

Let's take *repository design pattern*  as example. You shouldn't start with an abstract class to implement such design pattern, you would define an interface to start with:

	// Pseudo-code
	interface IRepository
    {
    	void add(some);
        void update(some);
        void remove(some);
        object getById(id);
    }

Anyone who would be willing to need a repository to consume some data source within your system would need to implement that interface. Perhaps, any repository implementation might need to validate incoming objects when those are going to be *added* or *updated*, and you want to avoid repeating yourself. Thus, a possible solution to be sure that *all* repositories share a validation logic is creating a partial implementation in the form of an *abstract class*:

	// Pseudo-code
	abstract class Repository implements IRepository
    {
    	void add(some)
        {
        	bool isValid = false;
            
            // ...here we would implement some validation
            // logic. Once executed, it would set if object to add
            // to the repository is valid or not to "isValid" variable.
            
        	if(isValid)
            {
            	doAdd(some);
            }
            else
            {
            	throw new ValidationException();
            }
        }
        
        abstract void DoAdd(some);
    }
    
Let's explain above code snippet. We've implemented `add()` from `IRepository`interface as part of an abstract class called `Repository`:

- The whole `add()` implements some validation logic and, if some given object to be added is *valid*, a variable called `isValid` will be set with `true` boolean value. 
- If some given object *is valid*, we'll call an abstract method `doAdd()`. **It's abstract because we want that a concrete repository (i.e. a derived class)  must provide an implementation of what to do if the object to add *is valid***. That is, all concrete repositories (for example, `ProductRepository`, `CustomerRepository`, `InvoiceRepository`) would validate any object against some generic validation rules.

**But why you said that we should define an interface prior to an abstract class?** *Because you want to define an interface that describes what's required, and you provide a basic implementation in the form of an abstract class to generalize certain implementation details while you leave others to more specialized classes*. 