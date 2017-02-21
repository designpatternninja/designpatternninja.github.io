---
layout: post
title: "Object contracts instead of interfaces in JavaScript"
excerpt: "In dynamically-typed language like JavaScript you shouldn't expect interfaces. So what?"
date: 2017-02-19 00:00:00 +0100
modifiedDate: 2017-02-19 00:00:00 +0100
categories: news
authors: 
   - mfidemraizer
---

Probably one of the worst JavaScript anit-patterns is **trying to emulate a class-based and strongly-typed language**. Right, now JavaScript as of ECMA-Script 2015 and above standards support class syntax but it's still a syntactic sugar over prototypal inheritance. That is, classes are just a readability improvement and they're not introduced to provide strong typìng in JavaScript.

Do you already know what's [*duck typing*](https://en.wikipedia.org/wiki/Duck_typing)? It's another way of refering to *dynamic typing* and it's based on the concept of *if something looks like a duck and quacks like a duck, it's a duck.*

So, if JavaScript code is always developed using *duck typing*, how you fill the gap of *interfaces*? I'm talking about those partial types on which we define a contract of what must implement a given type. For example, something like this in C#:

```c#
public interface ISerializable
{
	string Serialize(object someObject);
}
```

If a *class implements `ISerializable`* you know that it has a method called `Serializable` with the return type and parameters defined in the whole interface.

Clearly, JavaScript has no concept of *object contract* but we often need an object as argument, and usually we end up with some code that looks as follows:

```javascript
function doStuff(args) {
	if(typeof args != "object") {
    	throw Error("Arguments must be an object");
    }
    
    if(args.hasOwnProperty("arg1") && args.hasOwnProperty("arg2")) {
    	console.log("Caller has passed all required arguments!");
        
        // Do stuff with given arguments here
    }
}
```

Doesn't this checks look like *interfaces*? And this is a very hyphotetical case. Real world coding implies a lot of checks like these to make possible bad uses of some function *meaningful* instead of just receiving an error like '*Something is undefined*'.

## So I scratched my head

There should be a solution to both don't fall into trying to simulate interfaces code smell and still be able to keep things simpler to avoid too many explicit checks.

EMCA-Script 2015 standard introduced a very interesting language feature called *destructuring*. One of its flavors let us extract certain properties from a given object into local variables:

```javascript
var obj1 = { x: 1, y: "hello world", z: true };
var {x, z} = obj1;
```

This is awesome, because we can use destructuring to build a new object which just contains a subset of properties. Doesn't this look like upcasting an object to an interface type?

```javascript
var obj1 = { x: 1, y: "hello world", z: true };
var {x, z} = obj1;
// Bingo!
var obj2 = { x, z };
```

But this has a big problem: what happens if you *destructure* an object which doesn't contain the required property? See the comment bellow at the bottom of the code snippet:

```javascript
var obj1 = {};
var {x, z} = obj1;
// Oops, x and z are undefined! Destructuring unexistent 
// properties won't throw an error...
```

I believe that we failed to replace interfaces by a built-in JavaScript language feature... We need something that let us define a contract that must be fulfilled by a given object, but **we don't want to simulate interfaces**.

What about a function to which we give an already created object and a dummy/sample one with the properties that must be fulfilled by the actual object? These should be the requirements to fulfill a given contract:

- All sample properties must be matched in the given object.
- None of matched properties in the given object could be `undefined`.
- Property types from both sides must match.

Therefore I got to implement this function (see the comments in the following code snippet to understand how it works):

```javascript
// Checkes whether if a contract is fulfilled by a given object
// -------------------------------------------------------------------------
// This function takes the following parameters:
// source => The object from which the properties must be extracted to.
// contract => A sample object to define which properties must fulfill the
// 		  source object. Each property value should be its expected type name.
Object.assertContract = (source, contract) => {
  // If either the source object or the contract one aren't objects
  // return false directly!
  if (typeof source != "object" || typeof contract != "object") {
    return false;
  }

  // Iterates all property names defined in the sample object 
  for (let propertyName of Object.getOwnPropertyNames(contract)) {
    // The whole property name must exist in the source object's prototype
    // chain or the matched property must be of the same type as the contract's one
    if (!(propertyName in source) || typeof source[propertyName] != contract[propertyName]) {
      // If the whole property defined in the contract is fulfilled by
      // no property in the source object, we shourtcircuit the loop and
      // we return false directly
      return false;
    }
  }
  
  // If we arrived here is because the source object fulfilled the given
  // contract!!
  return true;
};
```

`Object.assertContract` could be used as follows:

```javascript
var obj1 = {
	x: 11,
    name: "matias",
    quack: () => console.log("quack quack!!")
};

// We get an object owning both x and doStuff properties!
var assertionResult = Object.assertContract(obj1, {
	name: "string", 
    quack: "function"
});

if(assertionResult) {
	// Contract assertion was successful!
    obj1.quack();
} else {
	// Contract assertion was unsucessful :(
}
```

Actually, I believe that `Object.assertContract` is a good way of filling the gap of interfaces in JavaScript while it's not out the dynamically-typing game, because it's not trying to simulate strong typing but just *object contracts*. Note that `Object.assertContract` won't assist you in forcing an object property to fulfill a contract. That is, first-level property checks are enough. Otherwise we would end up with a true interface emulation which would defeat the purpose of *duck typing*.

At the end of the day, it's just a shortcut to avoid a lot of manual checks and it increases readability: the code self-documents what you expect from a given object to consider it a *duck*!