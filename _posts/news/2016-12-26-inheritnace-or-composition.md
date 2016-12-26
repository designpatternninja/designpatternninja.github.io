---
layout: post
title: Inheritance or composition?
date: 2016-12-26 00:00:00 +0100
modifiedDate: 2016-12-26 00:00:00 +0100
categories: news
authors: 
   - mfidemraizer
permalink: /:categories/:title
---

While many experienced developers with deep knowledge and skills about object-oriented programming will say *this article is for dummies*, but while you might feel that it's a too basic question, I've found too many professionals that won't choose the right path when defining object graphs.

## So... when...?

First of all, I would introduce that object-oriented programming was conceived to describe real-world facts into a programming language.

In procedural languages, rather than using good identifiers, we weren't able to group certain data fields and behaviors:

    function car_engine_start() {

    }
    
Yeah, you know that it's about *cars* because it function identifier begins with `car_` and it's something with *car engines* because the word *engine* happens there. And you know that you're going to start a *car engine* because that identifier ends with *start*.

In object-oriented programming, you would express this as follows:

	// It's an invented syntax. Take it as pseudo-code ;)
    class Engine 
    {
		function start() 
        {
        
        }
    }

    class Car 
    {
    	constructor()
        {
        	engine = new Engine();
        }
        
        Engine engine;
    }


## Where are you going with this? This is even more basic than when to use inheritance or composition...

My intention is to express that object-oriented programming is about matching what's in your mind and code it accordingly!

## Key point: *to have* vs. *to be* verbs

Let's decribe some real-world concepts or objects:

- A car **is a** machine.
- A cat **is an** animal.
- My hand **has** fingers.
- My home **has** chairs.
- An employee **is** a person.

So now let's return to the object-oriented programming world... Let me know if you would find the following code sample correct:
    
    class Car
    {
    
    }

	class Machine
    {
    	Car car;
    }
    
Something is wrong here! A machine has a car, but don't we said that a **car *IS* a machine?????**. How can a machine contain a car? Ok, wait, I know you thought about a scrapping car machine... But you know that we weren't talking about scrapping cars. We're defining that **a car is a machine**.

And what about the following sample:

    class Chair 
    {

    }

    class Home : Chair
    {
    }
    
*Ahrg!* This is a crazy idea...! Because you want chairs in your home, your home inherits the definition of what is a chair... NO. This is completely wrong. Next code sample is right:

    class Chair 
    {

    }

    class Home
    {
        CollectionOfChairs chairs;
    }

Sounds good: my home has a collection of chairs. Actually, my home has many chairs, right? 

## TL;DR

In summary, in object-oriented programming:

- You choose inheritance when you describe a fact, concept or object relationship with the verb ***to be***.
- You choose composition/association when you describe a fact, concept or object relationship using the verb **to have** (also **to belong**, and any that might sound like some being property of other and vice versa).

## Some might argue that you should prefer composition over inheritance...

Since object-oriented programming inception it has been alive a long discussion about turning inheritance into a *corner case*.

Perhaps because inheritance can make code refactoring process more tedious. For example, it might happen that a *super class* might be the base class of many inheritance trees, and a small change in that *super class* might end up in breaking a lot of code when you also use polymorphism intensively.

Anyway, from my point of view, I would say that we should be cautious when designing our object graphs and practice a good separation of concerns in order to avoid too large inheritance trees and super classes holding too much reponsibility.

I wouldn't convert the exception into the rule...