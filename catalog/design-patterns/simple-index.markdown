---
layout: design-pattern
title: Simple index
excerpt: An approach to index data using collections.
date: 2017-01-08 00:00:00 +0100
modifiedDate: 2017-01-08 00:00:00 +0100
categories:
    - design-pattern
    - language-agnostic
    - data
    - data-structures
    - optimization
    - object-oriented-programming
authors: 
   - mfidemraizer
---

### Brief description

An approach to index data using collections.

### What does it solve?

Perhaps a *hidden* but yet powerful usage of a combinations of collection is underrated in many software solutions, where search operations are performed by iterating collection items one by one, which might hurt performance in some cases.

For example, in JavaScript and ECMA-Script 5.x standard onwards, `Array`'s prototype includes functions like `filter` or `find` to filter out items within an array:

{% highlight javascript %}
// An array contaning object with a name property
var array1 = [{ id: 1, name: "Matías" }, { id: 2, name: "Dani" }, { id: 3, name: "John" }];

// A filtered array of items contaning N letter (case insensitive)
var itemsContaningN = array1.filter(item => item.name.toLowerCase().includes("n"));
{% endhighlight %}

Also, imagine that you would want to get an ordered array of persons ordered by their age:

{% highlight javascript %}
// We would create an array contaning objects having an age property
var array1 = [{ id: 1, name: "Matías", age: 31 }, { id: 2, name: "Dani", age: 1 }, { id: 3, name: "John", age: 80 }];

// So we would use Array.prototype.sort and a compare function to order
// the whole array by age (ascending)
var byAgeArray = array1.sort((previous, next) => {
	return previous.age - next.age;
});
{% endhighlight %}

Above approaches are fine when we work with tiny collections, but once you need to work with collections of 1000, 10,000 or more items, these would turn into a performance bottleneck.

## Solution

In many languages like JavaScript where *functional programming* style have been introduced to simplify a lot of kinds of iterations, it sounds *cool* to use them everywhere, but as have been stated on *What does it solve?*, *cool things* aren't always a good friend of optimal performance.

An approach to avoid such performance bottlenecks could be a *simple indexing* infrastructure. Let's use JavaScript as a sample, but it would also work in many other languages like C#, Java, Scala, and even NoSQL database technologies like Redis (read the comments to understand the approach):

{% highlight javascript %}
// Defines an object called index which contains a
var index = {
  persons: new Map(),
  // We use Set instead of an array, because we want to be sure
  // that person identifiers must be unique within these
  // collections. In JavaScript and many other languages,
  // sets are hashed.
  personsContaningN: new Set(),
  personsOlderThan30: new Set(),

  // addPerson() adds a person to the index
  addPerson: function(person) {
    // Adds the whole person to the Map (dictionary)
    // Map's keys will be persons' identifiers, while the values
    // will be the persons themselves
    this.persons.set(person.id, person);

    // Indexes persons containing the "n" letter (case insensitive)
    // Matched persons are indexed in a set collection which will contain
    // the identifiers only
    if (person.name.toLowerCase().includes("n")) {
      this.personsContaningN.add(person.id);
    }

    // Indexes persons older than 30.
    // Matched persons are indexed in a set collection which will contain
    // the identifiers only
    if (person.age > 30) {
      this.personsOlderThan30.add(person.id);
    }
  },

  // Gets full persons' objects by giving one or more identifiers.
  // While it seems to be like a bad approach because the more identifiers
  // you give the more time will take the operation to end, it's advisable
  // that you do pagination on given identifiers before calling this function.
  // At the end of the day, getPersons() will do 5, 10 or 15 iterations to get
  // each full person object, and each retrieval is O(1) because we're accessing
  // a Map which usually stores data in an internal hash table.
  getPersons: function(ids) {
    var result = [];

    for (let id of ids) {
      result.push(this.persons.get(id));
    }

    return result;
  }
};

// Fill the persons Map (dictionary) with some persons
index.addPerson({
  id: 1,
  name: "Matías",
  age: 31
});
index.addPerson({
  id: 2,
  name: "Dani",
  age: 1
});
index.addPerson({
  id: 3,
  name: "John",
  age: 80
});

// Now if we want the persons whom name contains "n" we would
// retrieve them as follows.
// First of all, we get all index.personsContaningN identifiers using
// Set.prototype.values() function, and we give them as parameter to
// index.getPersons() function which will access each index.persons Map
// (dictionary) to get the full object of each given identifier!
var personsContaningN = index.getPersons(index.personsContaningN.values());

// Or if we want to retrieve persons older than 30:
var personsOlderThan30 = index.getPersons(index.personsOlderThan30.values());
{% endhighlight %}

Also, taking the whole `index` object, just imagine that we own a person `id` and
we want to know if the whole person is older than 30. We wouldn't need to get
the person from `index.persons` and check if `person.age > 30`, but it would be
as easy as (read the comments to get further information):

{% highlight javascript %}
var personId = 3;

// In JavaScript, Set collection defines a function called
// has() which is known as contains() in many other programming languages
// to check if a given value is within the collection. Since sets are hashed,
// usually finding if a value is within the whole set is an operation with 
// O(1) time complexity. That is, it won't iterate the set to check if it contains
// the whole item!
if(index.personsOlderThan30.has(personId)) {
	// Do stuff here
}
{% endhighlight %}

### Highlights of this approach

- It's applicable to most modern programming languages and even NoSQL databases like Redis.
- Objects or data is retrieved very fast even with large collections because they're not iterated anymore to search or extract objects or data.
- Some boolean checks become easier and more readable.
- As a *con*, some indexing logic should be implemented and objects or data should be indexed carefully.







    
   