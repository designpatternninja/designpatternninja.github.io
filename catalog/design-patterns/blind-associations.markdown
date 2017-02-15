---
layout: design-pattern
title: Blind associations
excerpt: A class which exposes its associations using interfaces in a way that weakly-typed scenarios don't need the type of the owner.
date: 2017-02-14 00:00:00 +0100
modifiedDate: 2017-02-14 00:00:00 +0100
categories:
    - design-pattern
    - architectural
    - .net
    - c#
    - object-graph
    - composition
    - aggregation
authors: 
   - mfidemraizer
---

### Brief description
<sup>**This pattern is specific to C# and other .NET CLI languages**</sup>

A class which exposes its associations using interfaces in a way that weakly-typed scenarios don't need the type of the owner.

### What does it solve?

Usually aggregates are exposed as *public* class members:

```c#
// A is the aggregate root
public class A 
{
	public B B { get; set; } = new B();
    public C C { get; set; } = new B();
}

public class B {}
public class C {}
```

In a normal usage, there should be no hassle in terms of accessing `A.B` and `A.C`, but sometimes the requirements go beyond accessing associations using the most simple approach:

```c#
// Simple approach
A a = new A();
B b = a.B;
C c = a.C;
```

While designing a given API its classes might be part of another layer where there's no direct access to the aggregate root class of some object within a graph.

Probably the whole problem can be solved defining a *parallel interface graph*:

```c#
public interface IWhatever
{
	B B { get; }
}

public class A : IWhatever
{
	public B B { get; set; } = new B();
    public C C { get; set; } = new B();
}
```

Thus, a given instance of `A` typed as `object` can be checked to determine if it implements `IWhatever` to get the association with `B`:

```c#
public void DoStuff(object some)
{
	IWhatever whatever = some as IWhatever;
    
    if(whatever != null)
    {
    	B b = whatever.B;
    }
}

DoStuff(new A());
```

In a simple case like the above some might argue that it would be fine to use this approach, but the problem goes bigger once the object graph is too deep as eveything ends up in a *parallel interface graph* to the formal object graph defined by classes meaning more maintenance whenever the classes within the graph change their implementation:

```
public interface IA
{
	B B { get; }
}

public interface IB
{
	C C { get; }
}

public interface IC
{
	D D { get; }
}

public class A  : IA
{
	public string Text { get; set; }
	public B B { get; set; }
}

public class B : IB
{
	public int Number { get; set; }
	public C C { get; set; }
}

public class C : IC
{
	public bool Flag { get; set; }
	publilc D { get; set; }
}

public class D
{
	public string Description { get; set; }
}
```

As noted in the last code snippet, the more deeper is the object graph, the more interfaces should be defined to let that some code could extract aggregates from the mentioned object graph.

### Solution

C# and .NET own an alternate way of implementing interfaces called [*explicit interface implementation*](https://msdn.microsoft.com/en-us/library/ms173157.aspx){:target="_blank"}. That is, instead of implementing an interface member by just providing the same member signature, explicit implementations look like the following code:

```c#
public interface IWhatever
{
	string Text { get; }
}

public class SomeImpl : IWhatever
{
	// Explicit interface implementation
	string IWhatever.Text => "hello world";
}
```

Also, in .NET, since the introduction of *generics*, a given interface can be implemented more than once on the same class or struct. Both *explicit implementations* and the ability to implement an interface many times with generics, gives a set of powerful tools to generalize and simplify the problem of *too many interfaces* and *parallel interface graph*.

*Blind associations* pattern is about defining a single interface like the following one:

```c#
public interface IHasAssociation<T>
{
	T Association { get; }
}
```

And an object graph could implement the whole interface at any level:

```c#
public class A  : IHasAssociation<B>
{
	public B B { get; set; }
    
    B IHasAssociation<B>.Association => B;
}

public class B : IHasAssociation<C>, IHasAssociation<D>
{
	public C C { get; set; }
    public D D { get; set; }
    
    C IHasAssociation<C>.Association => C;
    D IHasAssociation<D>.Association => D;

}

public class D : IHasAssociation<E>
{
	public E E { get; set; }
    
    E IHasAssociation<E>.Association => E;
}

public class E
{
}
```

Now let's initialize the object graph:

```c#
A a = new A();
a.B = new B();
a.B.C = new C();
a.B.D = new D();
a.B.D.E = new E();
```

Now a class accepting object's type as `object` would be able to gain a generalized access to the object graph using interface implementations' discovery:

```c#
public void DoStuff(object root)
{
	IHasAssociation<B> hasB = root as IHasAssocitation<B>;
    
    if(hasB != null)
    {
    	IHasAssociation<C> hasC = hasB as IHasAssociation<C>;
        IHasAssociation<D> hasD = hasB as IHasAssociation<D>;
        
        if(hasD != null)
        {
        	IHasAssociation<E> hasE = hasD as IHasAssociation<E>;
            
            // At this point, we would have destructured the full
            // object graph into variables!
        }
    }
}

DoStuff(a);
```

A more powerful interface of *blind associations* would be also [covariant](https://msdn.microsoft.com/en-us/library/dd799517(v=vs.110).aspx){:target="_blank"}:

```c#
public interface IHasAssociation<out T>
{
	T Association { get; }
}
```

See the following object graph:

```
public class SuperClass
{

}

public class DerivedClass : SuperClass
{

}

public class A : IHasAssociation<DerivedClass>
{
	public DerivedClass Derived { get; set; }
    
    DerivedClass IHasAssociation<DerivedClass>.Association => Derived;
}
```

Since `IHasAssociation<out T>` `T` generic type parameter is covariant, there's a chance to even get the association as follows:

```c#
A a = new A();
a.Derived = new DerivedClass();

IHasAssociation<SuperClass> hasSuperClass = a as IHasAssociation<SuperClass>;

if(hasSuperClass != null)
{
	// Effectively, it has a reference of type SuperClass, which is in turn
    // a derived class of SuperClass! Covariance is very powerful!
}
```

## *Has-many associations*

*Blind associations* can also work with *has-many* associations. The interface looks as follows:

```c#
public interface IHasManyAssociation<out TEnumerable, out T>
	where TEnumerable : IEnumerable<T>
{
	TEnumerable Association { get; }
}
```

And it would be implemented this way:

```c#
public class A : IHasManyAssociations<IList<B>, B>
{
	public IList<B> ManyB { get; set; }
    
    IList<B> IHasManyAssociations<IList<B>, B>.Association => ManyB;
}

public class B
{

}
```

Finally, *the list of associated instances of `B`* would be retrieved as the following code snippert shows:

```c#
A a = new A();
a.ManyB = new List<B>() { new B(), new B() };

// TEnumerable is covariant, thus, since IList<T>
// implements IEnumerable<T>, the has-many association can
// be got as IEnumerable<T>!
IHasManyAssociation<IEnumerable<B>, B> hasManyB = a as IHasManyAssociation<IEnumerable<B>, B>;

if(hasManyB != null)
{
	// Do stuff with associated instances of B!
    IEnumerable<B> manyB = hasManyB.Association;
}
```