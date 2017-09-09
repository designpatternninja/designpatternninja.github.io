---
layout: post
title: "Brainfucking C#: Functional piping using extension methods"
excerpt: "Do you want to embrance functional programming using C#? How to achieve function piping?"
date: 2017-09-09 00:00:00 +0100
modifiedDate: 2017-09-09 00:00:00 +0100
categories: news
authors: 
   - mfidemraizer
---

The rise of functional programming is irrefutable: the wonders of its programming style and rules are good friends of *concurrency* and aside of this it can produce quality code with less effort. But this is another discussion. 

Probably you've come up to this article because you're either interested on learning more about what's functional programming or you're already applying but you're struggling to code in a more declarative way.

## Let's brainfuck a little before going into the solution

Let's take the following code snippet in C# 7.0 using *local functions*:

```c#
// Takes an integer, adds 2 and returns it:
int sum2(int x) => x + 2;

// Takes an integer and it does the power of 2:
int pow2(int x) => x * x;
```

Now, if we want to call `sum2` and use its result on `pow2` we would do this:

```c#
var sumResult = sum2(4);
var powResult = pow2(sumResult);
````

*Right?* Ok, what would happen if we want to take the result of `sum2(4)` into `power2` directly? **We would use *delegates***:

```c#
// Takes an integer, adds 2 and returns it:
// This is a refactor of sum2 local function. Now it takes a second parameter on which we can 
// give a delegate which takes an integer as parameter and returns integer too.
//
// The whole function does the addition and gives the result to the continuationPredicate delegate call.
int sum2(int x, Func<int, int> continuationPredicate) => continuationPredicate(x + 2);
```

And since `pow2` has the same signature as the `sum2` second parameter `Func<int, int>` we may chain them together as follows:

```c#
// That's it! We give pow2 as argument, and C# compiler is smart enough to infer that pow2 matches
// the delegate signature, thus, it converts it to the delegate behind the scenes!
var result = sum2(4, pow2);
```

Imagine that `pow2` could also allow *chaining* using a delegate the same way as `sum2` does it:

```c#
int pow2(int x, Func<int, int> continuationPredicate);
```

...and we introduce a new local function:

```c#
int divideBy3(int x) => x / 3;
```

Our method chain would look as follows:

```c#
var result = sum2(4, x => pow2(x, divideBy3));
```

Let's add even another local function to chain:

```c#
int writeToConsole(int x) => Console.WriteLine(x);
```

Thus, the chain would look like the following snippet:

```c#
var result = sum2(4, x => pow2(x, r => divideBy3(r, writeToConsole)));
```

*Isn't this **awful***? Just imagine a chain of 10 functions...

Other languages like F# would express the same chaining with a more declarative and concise way:

```f#
sum2 4 |> pow2 |> divideBy3 |> writeToConsole
```

In functional languages you can *pipe* functions. The above code means that *the result of calling `sum2` is the input parameter of the next function*, and so on. Isn't this more expressive and even *simpler* than *function inside function inside function*...?

## Ok, but how do I pipe functions in C#?

As of today, there's no *function piping* operator in C#. The *short answer* is *you can't pipe functions*. 

Anyway, let's continue our brainfuck. You know about *extension methods*, don't you? From now, I'll start a series of steps, to build a *puzzle* so we'll be able to simulate *function piping* in C#. Be confident about me and hold on until we get to the solution.

First of all, let's define some pseudo-code. Say we've four methods:

- `sum2`: you give a number and it adds `2` and returns the result of the addition.
- `pow2`: you give a number and it squares it, and returns the result.
- `divideBy3`: you give a number and it divides it by 3, and returns the result.
- `writeToConsole`: it receives an object and writes it to the console output.

We want to be able to chain them all starting from a given number: `sum2(3) -> pow2 -> divideBy3` -> `writeToConsole`. That is, *the result of `sum2` will be the input of `pow2`, the result of `pow2` will be the input of `divideBy3`, and the result of `divideBy3` will be the input of `writeToConsole`*.

In addition, we'll try to avoid the execution of every single function in the chain unless the last one has been called. *Why?* Because when you pipe functions you're expressing a full flow and you should be interested on the input at the beggining of the chain and its output.

Do you already know *extension methods*? They're a good tool to chain methods. For example, have you ever seen something like this in your code?

```c#
list.Where(i => i > 10).Select(i => new { Number = i });
```

OK! But you should consider the following sample extension methods:

```c#
public static class Extensions
{
	public static int Sum2(this int num) => num + 2;
    public static int DivideBy3(this int num) => num / 3;
}
```

Those would be chained as follows:

```c#
Extensions.Sum2(3).DivideBy3();
```

Right! Anyway, there's a *little* problem here. Each method in the chain are executed inmediately. There's nothing wrong here, but what would happen is you might need to compose the chain across many functions and it needs to be executed only at certain point or it's discarded? Our current approach won't work well on this scenario... BTW, did you think there's no easy workaround for this? *Wrong!* There's one.

#### Problem preface

Check the following code snippet:

```c#
Func<int> operation1 = () => 1 + 1;
Func<Func<int>, int> operation2 = previousOperation =>  previousOperation() * 2;

var result = operation2(operation1);
```

Hold, let's explain that code:

1. `operation1` is a delegate which takes no arguments and does the addition `1 + 1` and returns the result.
2. `operation2` is a delegate which takes *one argument* which is a delegate that must return an `int` value, and overall the delegate must also return an `int` value.
3. Finally, `operation1` is called giving `operation2` as argument, and it's called and return `4`.

Did you notice that I can chain `operation1` with `operation2` but not the opposite? 

```c#
// I can do this
operation2(operation1);

//...but you can't do the opposite:
operation1(operation2);
```

`operation1` doesn't declare a `Func<int>` parameter to receive another delegate as argument. Let's add it:

```c#
Func<Func<int>, int> operation1 = previousOperation =>  previousOperation()  1 + 1;
Func<Func<int>, int> operation2 = previousOperation =>  previousOperation() * 2;

var result = operation1(operation2);
```

So this is effectively equivalent to function piping, but what would happen if we introduce a third delegate?

```c#
Func<Func<int>, int> operation1 = previousOperation =>  previousOperation()  1 + 1;
Func<Func<int>, int> operation2 = previousOperation =>  previousOperation() * 2;
Func<Func<int>, int> operation3 = previousOperation =>  previousOperation() / 5;

// How do I give the third operation to operation 3?
var result = operation1(operation2);
```

You've already noticed that we can't give `operation3` to `operation2` as we would need to invoke `operation2` to be able to provide it:

```c#
var result = operation1(() => operation2(operation3));
```

As you add more functions that may be piped with others, your code can eventually become a *callback hell*:

```c#
// Whoaaa!! Too many () => )))))))))) ...
var result = operation1(() => operation2(() => operation3(() => operation4(() => operation5(operation6(() => operation7(() => operation8(() => operation9(operation10))))))));
```

Wouldn't be wonderful to see the same code as follows?

```c#
var result = operation1 |> operation2 |> operation3 |> operation4 |> operation5 |> operation6 |> operationN
```

Since C# has no *function piping operation*, what if we could pipe them all using *dot syntax*?

```c#
var result = operation1().operation2().operation3().operation4().operation5().operation6().operationN();
```

...and what if we would want to accumulate those functions and call them all at certain point (sometimes called *deferred execution*)?

*Yes, we can do it!* Using *delegates* and *extension methods*.

### Take 1: Deferred function piping in C# using *extension methods*

Let's think about a basic *function piping* case:

1. Take a number and sum `2`.
2. Take the result of previous function and multiply by `3`.
3. Take the result of previous function and write it to the *console output* (i.e. `Console.WriteLine(...)`).

These *functions* would translate into *regular class methods* as follows:

```c#
public class Ops
{
    public int Sum2(int x) => x + 2;
    public int MultiplyBy3(int x) => x * 3;
    public void WriteToConsole(object input) => Console.WriteLine(input);
}
```

We would need to turn them all into *extesion methods* in order to build the `Sum2(4).MultiplyBy3().WriteToConsole()` method chain, right?

```c#
// Extension method classes must be static, the whole methods must be also static and the extended type
// should be the first argument starting with "this"
public static class Ops
{
    public static int Sum2(this int x) => x + 2;
    public static int MultiplyBy3(this int x) => x * 3;
    public static void WriteToConsole(this object input) => Console.WriteLine(input);
}
```

But, again, we want to defer the execution of a given method chain execution until the last method is called (for example: `WriteToConsole`).

How may we call a function that doesn't run its code? See how on this code snippet:

```c#
Func<int, Func<int>> sum2 = x => () => x + 2;
```

It's a delegate where its return value is a `Func<int>`:

- `x => ...` is the delegate which should return a `Func<int>`.
- `() => x + 2` is the `Func<int>`. It doesn't take any input parameter and returns an `int`. 

<sup>* For your information, this is known as [*function currying*](https://stackoverflow.com/questions/36314/what-is-currying).</sup>

So it turns out that if we call the whole `sum2` delegate, you won't do the sum but receive a delegate which takes no parameters and once called will return the result:

```c#
Func<int> calcSum2 = sum2();
int result = calcSum2();
```

So if you got the hint, we should refactor our `Ops` class, don't we?

```c#
public static class Ops
{
    public static Func<int> Sum2(this int x) => () => x + 2;
    public static Func<int> MultiplyBy3(this int x) => () => x * 3;
    public static Action WriteToConsole(this object input) => () => Console.WriteLine(input);
}
```

...but now we won't be able to chain methods since we're providing extensions to `int` and `object`. No problem, we need a second step here: extend on `Func<int>` and `Func<object>`:

```c#
public static class Ops
{
    public static Func<int> Sum2(this Func<int> x) => () => x + 2;
    public static Func<int> MultiplyBy3(this Func<int> x) => () => x * 3;
    public static Action WriteToConsole(this Func<object> input) => () => Console.WriteLine(input);
}
```

Oops! But now those extension points are *delegates*. We can't do `x + 2` anymore: we just need to call the delegate instead:

```c#
// Note that now "x" and "input" parameters are called inside the curryied function
public static class Ops
{
    public static Func<int> Sum2(this Func<int> x) => () => x() + 2;
    public static Func<int> MultiplyBy3(this Func<int> x) => () => x() * 3;
    public static Action WriteToConsole(this Func<object> input) => () => Console.WriteLine(input());
}
```

Yeah! Now we may chain them all!

```c#
Ops.Sum2(() => 5).MultiplyBy3().WriteToConsole();
```

Remember that the whole chain doesn't get executed until the last returned delegate gets invoked:

```c#
var chain = Ops.Sum2(() => 5).MultiplyBy3().WriteToConsole();
chain();

// or just (note the double call at the end of the chain):
Ops.Sum2(() => 5).MultiplyBy3().WriteToConsole()();
```

Did you note that I had to give a delegate to `Sum2` instead of a literal `int`? Wouldn't be wonderful if we could give `5` instead of `() => 5`? This is possible, but it adds some odd code. Let's refactor `Ops` again:

```c#
public static class Ops
{
    public static Func<int> Sum2(this Func<int> previous, int? x = null) => 
    	() => (int)(previous?.Invoke() ?? x) + 2;
    public static Func<int> MultiplyBy3(this Func<int> previous, int x = null) => 
    	() => (int)(previous?.Invoke() ?? x) * 3;
    public static Action WriteToConsole(this Func<object> previous, object input = null) => 
    	() => Console.WriteLine(previous?.Invoke() ?? input);
}
```

*Previous refactoring summary:*

**1) Every function has introduced a second optional and nullable input parameter . *Why?* Because we want to either call those functions as standalone ones or as part of a *chain*:**

```c#
// MultiplyBy3 acts like a standalone function as we don't pass a delegate to just
// give an integer literal
Ops.MultiplyBy3(4).Sum2().WriteToConsole();
```

...or:

```c#
// Now MultiplyBy3 is part of a function chain aka piping
Ops.Sum2(20).MultiplyBy3().WriteToConsole();
```

**2) You should've noted that every function body returns a parameterless `Func<T>` on which we find this pattern: `previous?.Invoke() ?? x`. Do you already know the [*null conditional operator*](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-conditional-operators)? We check if `previous` (i.e. *the previous delegate in the chain*) isn't `null` and then we invoke it. Otherwise, `previous?.Invoke()` expression will be `null`, and we use the [*null-coalescing operator*](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-conditional-operator) to evaluate the right part of the `??`. That is, we either get the result of calling `previous` or the optional input parameter value.**

**3) The value of `previous?.Invoke() ?? x` is surrounded using parenthesis `()` and it becomes the left part of an addition or subtraction. Also, we need to cast the whole expression to `int` since the right operand of these addition and subtraction are non-nullable integer literals.**

Ok, are we able to even drop the first function class qualification in the chain? *Yes, we can!* We can take advantage of `using static` statement](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/using-static):

```c#
using static Ops;

public class Program
{
	// Remember that you need to call () twice to run the chain!
	public static void Main(string args[]) 
    	=> Sum2(20).MultiplyBy3().WriteToConsole()();
}
```

## Why deferred execution is interesting?

Yep! I know, you should've already noted that there's some question on the air: if extension methods themselves are a good approach to create chains, why do you really need to implement *deferred execution*?

The fact that you're creating a chain but it's not executed *per se* means that you may sparse a chain across many boundaries. Let's take the `Ops` extension methods to illustrate this:

```c#
public static class ServiceOps
{
	public static Func<int> SomeCalculation(Func<int> previous, int? x, Func<int, Func<int>> multiply)
    	=> () => multiply(previous?.Invoke() ?? yourAge).Sum2();
}
```

Check how I could add `.Sum2()` to the result of `multiply` delegate call, and return the complete calculation expression as a `Func<int>` again to make it chainable by extension methods of that delegate type!

The whole extension method would be called as follows:

```c#
using static Ops;
using static ServiceBusOps;

public class Program
{
	public static void Main(string[] args)
    	=> SomeCalculation(4, x => () => MultiplyBy3(x)).WriteToConsole()();
}
```

Did you catch the idea? You can continue the chain inside another function passing it as a delagete, and return it to be continued. 

This can be extremely powerful, since you can create a chain passing in it through many layers and decide to entirely call it or not. This can save resources.

Also, there's another usage: you can store complex chains as a single delegate without ever calling it at all:

```c#
using static Ops;
using static ServiceBusOps;

public class Program
{
	public static void Main(string[] args)
	{
    	var complexCalc = SomeCalculation(4, x => () => MultiplyBy3(x)).Sum2();
    }
}
```

## Last words

Well, today we've been brainfucking about C#. The whole *function piping* can be applied to more complex scenarios, but you got the hint about how you can simulate function piping using extension methods and delegates to defer chain's execution.

There's yet another approach which involves method extensions and **it can also defer function chains' execution...** *But this will be our next C# brainfucking article in the series!*


