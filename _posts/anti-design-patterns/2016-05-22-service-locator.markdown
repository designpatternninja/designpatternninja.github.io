---
layout: design-pattern
title: Service locator
date: 2016-05-22 00:00:00 +0100
modifiedDate: 2016-05-22 00:00:00 +0100
categories:
    - anti-pattern
    - software-architecture
    - dependency-injection
    - inversion-of-control
authors: 
   - anonymous
   - mfidemraizer
permalink: /:categories/:title
---

### Brief description

A *service locator* is a *singleton* which provides global access to *inversion of control container* to get implementations by calling a method that will return an implementation to a given interface.

### The problem

*Inversion of control* and *dependency injection* design patterns aren't tied to be implemented by a given *framework* or *library* but they're more a concept or paradigm which defines how code dependencies should be built to develop maintainable, testable and elegant code.

In other words, both concepts can be implemented manually. For example, in C# there could be an interface like the following one:

	public interface IWeapon 
    {
         void Attack();
    }

...and a class called *Solider* which may inject weapons on itself to own weapons to fight against an enemy:

	public class Solider
	{
        public Solider(IList<IWeapon> weapons)
        {
			Weapons = weapon;
        }

        private IList<Weapon> Weapons { get; }
	}

This would be the *right way* of injecting dependencies. As shown in the sample above, how `IList<IWeapon>` gets injected into `Solider` isn't tied to the whole class itself, but it'll be defined by the code which may instantiate `Solider`:

	// A sample IWeapon implementation:
	public class Riffle : IWeapon
	{
		public void Attack()
		{
			Console.WriteLine("BANG! BANG! BANG!");
		}
	}

	// Somewhere Solider should be instantiated and we'll provide a Riffle:
	IWeapon weapon = new Riffle();
	IList<IWeapon> weaponList = new List<IWeapon> { weapon };

	// Manual injection...
	Soldier soldier = new Soldier(weaponList);

Usually, when an *inversion of control container framework* is used, the whole dependency injection of *multiple implementations of `IWeapon`* would be done automatically, since the entire dependency tree is initialized, instantiated and receive injections based on a given configuration.

This is the desired approach, because interface implementations and classes receiving injections remain decoupled of how dependency injection happens and even *who* does the whole injection (it can be manual or automatic injection). And it is the right approach, because a code base has near to zero dependency on what inversion of control container or dependency injection framework is being used.

In the opposite side, there's the *service locator anti-pattern*. For example, let's take the same `Solider` and `IList<IWeapon>` sample using a possible *service locator* approach:


	public class Solider
	{
        public Solider()
        {
			// Solider wouldn't be possible to be successfully instantiated
			// if the ServiceLocator isn't correctly configured and started!!! 
			Weapons = ServiceLocator.Instance.GetServices<IWeapon>();
        }

        private IList<Weapon> Weapons { get; }
	}

...or also:

	// OK, Solider isn't coupled with how IWeapon implementations get injected, but
	// the code that's instantiating it is cupled to the ServiceLocator...
	Soldier soldier = new Soldier(ServiceLocator.Instance.GetServices<IWeapon>());


At the end of the day, the *service locator anti-pattern* has some clear drawbacks:

- It couples a project to an inversion of control container and dependency injection framework or library and that dependency is spread a lot of code lines. A change to other container may be a big refactor.
- It prevents *unit testing*, because there's no way to test code without configuring a dependency injection framework or library, because a given object dependencies can't be faked/mocked manually (or using some mocking framework...).