---
layout: design-pattern
title: "Semigroup"
excerpt: .
date: 2018-10-22 00:00:00 +0100
modifiedDate: 2018-10-22 00:00:00 +0100
categories:
  - functional-programming
  - language-agnostic
  - design-pattern
authors:
  - ???
---

### Brief description

Defines an associative binary operator over a set of zero or more elements which outputs a new set.

### What does it solve?

There're many data types which can be manipulated with a binary operation to semantically represent the same transformation. For example:

- Addition (add numbers, apples, pears, cars...)
- Merge (i.e. _merging collections, objects..._).

Usually those transformations are defined as individual functions. Let's see an example in JavaScript, and let's call that function `concat`:

```javascript
const numberConcat = (x, y) => x + y;
const objectConcat = (x, y) => ({ ...x, ...y });
const arrayConcat = (x, y) => [...x, ...y];

numberConcat(1, 2); // 3
objectConcat({ x: 1 }, { y: 2 }); // { x: 1, y 2 }
arrayConcat([1, 2], [3, 4]); // [1, 2, 3, 4]
```

But, what would happen if someone wants to define a very abstract function on which two values should be combined ignoring their types?

```javascript
const f = x => ys => concat(append(x)(ys));
```

The number of different implementations to `concat` may grow overtime as new requirements arise

### Solution

A possible solution to described problem is refactoring the _DTO_ side, where any property that could be both setted with a value or unsetted with its default value should be defined wrapped with a type like the following one:

<img src="/img/setunsetvalue/setunsetvalue.jpg" style="display: block; margin: 0 auto; width: 131px">

For example, the _EmployeeDto_ in JSON would look as follows if it would come with property values:

    {
    	"name": { "value": "Matías" },
    	"age": { "value": 31 }
    }

Otherwise, suppose that `"name"` should be _unsetted_. It would come as follows:

    {
    	"name": { "unset": true },
    	"age": { "value": 31 }
    }

...which would set `null` to `"name"` string once the _DTO_ gets mapped to its corresponding domain object.

That is, thanks to the _set-unset value pattern_, it is possible to disambiguate when the intended value is _setting the default value_ or _setting an actual value_.
