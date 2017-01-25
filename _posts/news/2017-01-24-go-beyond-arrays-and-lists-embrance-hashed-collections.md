---
layout: post
title: Go beyond arrays and lists. Embrance hashed collections
excerpt: "Most developers feel confortable using arrays or list collections. Unleash the power of hashed collections!"
date: 2017-01-24 00:00:00 +0100
modifiedDate: 2017-01-24 00:00:00 +0100
categories: news
authors: 
   - mfidemraizer
---

Whatever programming language you may use, if it's more or less modern (say from 20 years onwards...) provide data structures and one of them are *collections*.

While an *array* isn't a collection *per se* in some languages like JavaScript they're collections with the semantics of a *list*: *a collection where item ordering is based on insertion order and items can be accessed by a numeric index*.

Perhaps you're already familiar with other collections but you're not using them too much: *sets* and *maps/dictionaries*.

## *Sets*

Possibly one of most forgotten collections, *sets* are collections of unique but unordered items, and you can't get a concrete item by index like you would do when working with *arrays* or *lists* (you know: `list[0]`).

JavaScript has introduced *sets* since ECMA-Script 2015 standard and it has made available a built-in object called `Set`. It works as follows:

```javascript
var set = new Set();
set.add(1);
set.add(2);
set.add(3);
```
    
In C#, there's an `ISet<T>` implementation called `HashSet<T>`, and it works like in JavaScript:

```c#
var set = new HashSet<int>();
set.Add(1);
set.Add(2);
set.Add(3);
```
    
BTW, this looks like a *list*, right? For now, yes, it does.

So what's the point of using *sets*. Well, first of all, did you find yourself doing checks like this?

```javascript
var arr = [1,2];
if(!arr.includes(3)) {
    arr.push(3);
}
```

Argh! You need to check if some items is already in a given collection prior to adding it. Now see the same goal using *sets*:

```javascript
var set = new Set([1, 2]);
set.add(3);
```

Wait! But you didn't check if `3` was in the set already, so? Easy: *sets* are collections of unique values and `Set` implementation is smart enough to just add elements that aren't present in the whole collection. In JavaScript and C#, the function/method to add elements has a *boolean* return value! If it couldn't be added, it'll return `false`. As easy as this. **Therefore, it's 100% safe to add elements to a set without checking if it's already there**. Doesn't this keep things simpler?

JavaScript has an important drawback: it can't define custom equalities between objects and *sets* are limited to be unique with object references and primitive values (integers, booleans, strings...), but .NET and C# are more mature on this matter:

```c#
public class Person
{
	public Person(string name) 
    {
    	Name = name;
    }
    
	public string Name { get; }
    
    public override bool Equals(object other) 
    {
    	if(other == null) return false;
        
        Person otherPerson = other as Person;
        
        return otherPerson != null && otherPerson.Name == Name;
    }
    
    public overide int GetHashCode() => Name.GetHashCode();
}
```

As you noted in the code snippet, in C# we can override `Object.Equals` and `Object.GetHashCode` to customize what means *equals* for a `Person` (in our case, they're equal if both own the same name).

For example, check the follow sample code:

```c#
HashSet<Person> personSet = new HashSet<Person>();
personSet.Add(new Person("Matías"));
personSet.Add(new Person("Matías"));
```

Would both persons be added to the *set*? The answer is ***no***, because both persons are the same person as of how `Object.Equals` has been overridden on `Person` class! No need to check for duplicates! Keep it simple, stupid (again)!

Now let's consider that you've two lists (instead of *sets*) like these:

```c#
List<string> list1 = new List<string> { "a", "b", "c" , "d" };
List<string> list2 = new List<string> { "a", "d" };
```

And I ask you that I want the items which are present in both lists. AFAIK, you would try to use LINQ as follows:

```c#
var itemsInBothLists = list1.Where(item => list2.Contains(item));
```

OK, but don't you know that this will end up in a lot of iterations? There will be one for each item in `list1` and for each `list1` iteration there will be *N* on `list2`. Now imagine that `list1` has 10K items and `list2` 30k items. I feel that your wonderful code would be a nice code smell...

Again, *sets* are the solution on this case, using **intersections**:

```c#
HashSet<string> set1 = new HashSet<string> { "a", "b", "c" , "d" };
HashSet<string> set2 = new HashSet<string> { "a", "d" };

var coincidences = set1.Intersect(set2);
```

Brilliant!

Also, I can get the items that aren't in `set2`:

```c#
var notInSet1 = set2.except(set1);
```

Awesome!

Do you imagine how this simple approach can be very powerful? You can intersect 3 sets to perform complex queries! For example, I want users who are online, which speak English and also in Europe. You would build these three sets with user identifiers:

```c#
HashSet<int> onlineUsers = new HashSet<int>() { 1, 2, 3, 4, 5 };
HashSet<int> englishUsers = new HashSet<int>() { 2, 4, 5 };
HashSet<int> europeanUsers = new HashSet<int>() { 1, 2, 5 };

// result = [2, 5]
var result = onlineUsers.Intersect(englishUsers).Intersect(europeanUsers);
```

Sadly, in JavaScript there're no built-in intersections and other common *set* operations, but they can be easily implemented:

```javascript
if(!("intersect" in Set.prototype)) 
	Set.prototype.intersect = function(otherSet) {
    	var result = new Set();
        
        for(let element in this.values) {
        	if(otherSet.has(element)) {
            	result.add(element);
            }
        }
    };
    
if(!("except" in Set.prototype)) 
	Set.prototype.except = function(otherSet) {
    	var result = new Set();
        
        for(let element in this.values) {
        	if(!otherSet.has(element)) {
            	result.add(element);
            }
        }
    };
```

So... Really you just own some user identifiers, but you want to load full users' data to show it on some UI... You should already know these friends: *dictionaries* and *maps*.

Prior to filling any set, you would use a *map* or *dictionary* as an in-memory key-value database, where keys will be the user identifiers and values the user objects:

```javascript
var map = new Map();
map.set(1, { name: "Matías", age: 31 });
map.set(2, { name: "John", age 45 });
map.set(3, { name: "Laura", age: 12 });
map.set(4, { name: "Bob", age: 27 });
map.set(5, { name: "Justin", age: 58 });
```

Once we've built a dictionary of users, and we've already got a result of given *intersection*, retrieving full users' data is as easy as:

```javascript
var users = [];

for(let id of intersectionResult) {
    users.push(map.get(id));
}
```

## Understanding what means *hashed* collections

At this point you would be convinced about the power of *hashed collections* but, anyway, *why are they called **hashed** collections*?

It turns that when you need to search for some occurences inside a regular collection, if matching element is the last element, **you'll need to iterate the entire collection to get it!**. This can be a big performance bottleneck if we talk about really large collections.

So, how can we solve this? **We do using hash functions**. A hash function is a function to which we provide a value and it outputs **a number**.

For example, a hash function called `F` to which we give `hello world` may output a number like `384`. And guest what's this number?

```javascript
// We initialize an array of 500 indexes
var array = [];
array.length = 500;

// We hash "hello world" and provide that it should produce
// a number between 0 and 499. Imagine that the number will be 345
var slot = hashFunction("hello world", 0, array.length - 1);

// We store "hello world" on the number that the hash function
// has produced!
array[slot] = "hello world";
```

The whole `hashFunction` must produce the same number for the same input string. That is, when we want to check if "hello world" is within the array, we do this:

```javascript
var slot = hashFunction("hello world", 0, array.length - 1);

if(typeof array[slot] != "undefined") {
	// So "hello world" is present in our wonderful array!
}
```
Do you already found how this improves performance? **We can check that a given value is within the whole array at a constant time, either if the value is at the begining, middle or end of the array since we know exactly where's stored!**. We don't need to iterate the entire array anymore!

The bad news is that there's no perfect *hash function*: it might happen that two or more values could be computed into the same *slot number*, and this situation is known as **hash collision**. The better is the hash function, the lesser is the chance to repeat a slot. BTW, as I've already say, AFAIK, there's no perfect hash function...

Thus, what it happens then? Well, a simple solution is to store an array on each main array index:

```javascript
var array = [];
array.length = 500;

for(let i = 0; i < array.length; i++) {
	array[i] = [];
}
```

...and our storage algorythm would be modified as follows:

```javascript
var array = [];
array.length = 500;

var slot = hashFunction("hello world", 0, array.length - 1);

array[slot].push("hello world");
```

Instead of adding `"hello world"` to the slot itself, we push it into the array stored in the slot. Now our code to verify if `"hello world"` is already stored in our array would look as follows:

```javascript
var slot = hashFunction("hello world", 0, array.length - 1);

// We check if "hello world" is in the array stored in the slot
// computed by the hash function
if(array[slot].some(element => element == "hello world")) {
	
}
```

You might say that now our algorythm is not as performant as the first one, **because once we access a slot, we need to iterate the entire inner array**. BTW, the main array may have 100K slots, **but each slot may have 5 indexes!**. That is, you potentially saved up thousands of iterations at the risk of doing less than 10 iterations on a given slot to get to your element. Hereby, we consider that element access is *nearly constant in time*.

## Do you see now that you need to go beyond arrays and lists?

Whenever you need to work with large datasets, you need to think about hashed collections.

Dictionaries or maps are also hashed collections: their *keys* are hashed, and this is the reason for which accessing dictionaries/maps' keys is also blazing-fast!

Stop using arrays or lists to later make use of fancy and fluent collection querying APIs:

```javascript
// Imagine that this array has 200 persons...
var array = [{ name: "Matías" }, { name: "John" }];

// Suboptimal!
var person = array.find(person => person.name == "Matías");
```

and replace them with:

```javascript
var map = new Map();
map.set("Matías", { name: "Matías", age: 31 });
map.set("Matías", { name: "John", age: 74 });

// BLAZING-FAST!!!!!!!!!!!! Either way, if the dictionary has 2 
// or 2 million elements, a given person will be retrieved at the same
// speed!
var person = map.get("Matías");
```

## Further reading

If you are interested on this topic, I would double check these links:

- [Hash function on Wikipedia](https://en.wikipedia.org/wiki/Hash_function){:target="_blank"}
- [Hash table on Wikipedia](https://en.wikipedia.org/wiki/Hash_table){:target="_blank"}

Also, I would take a look at [*Redis*](http://redis.io), an in-memory key-value store which implements values as data structures. Leveraging *Redis* is a good challenge to get used with the advantages of using hashed collections!


