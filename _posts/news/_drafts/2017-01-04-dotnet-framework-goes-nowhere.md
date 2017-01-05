---
layout: post
title: .NET Framework goes nowhere
date: 2017-01-04 00:00:00 +0100
modifiedDate: 2017-01-04 00:00:00 +0100
categories: news
authors: 
   - mfidemraizer
---

Windows Communication Foundation, Windows Workflow Foundation, Windows Presentation Foundation... Do you remember them? Of course! They were the cutting-edge of .NET technologies... in 2006 in .NET 3.0 days. These technologies are still with us in .NET Framework 4.6 series (the latest ones).

![.NET 3.x stack](/img/news/{{ page.title | slugify }}/dotnet-framework-3-stack.png)

A lot of things have happened since .NET 3.0 days. Basically Microsoft entered the cloud business and HTML5 has been growing to current state of Web and multi-platform development (hello [Electron](http://electron.atom.io/)). RESTful/JSON APIs have taken the space of old-school SOAP/XML services and, in the cloud era, SQL databases have lost momentum thanks to NoSQL databases like Mongo, Couch, Redis, Kafka and many others (hey, I'm not concluding that SQL databases are useless, I'm just analyzing that there're many use cases where they're out of the game - *possibly* - forever)

**TOO MUCH ADVANCEMENTS!**

Also, in the server/backend space, NodeJS has turned into a BIG player. It's 100% open source and multi-plataform.

## .NET Core to the rescue!

.NET was conceived with same goals as Java in terms of being possible to develop once and deploy to multiple platforms and devices. Excluding Mono, .NET since its inception couldn't fulfill such hope or, at least, not in terms of officially supporting other operating systems than ones with the Windows logo and devices designed for them.

Also, current state of software development demands *open sourcism*, because most developers don't love technologies which can't be evolved in community anymore: everyone wants to be able to contribute to enhance their favorite tool or programming language. It's a matter of getting things faster: millions of developers can do more than a closed development team constrained by finantial limitations.

Microsoft introduced less than a year ago the first RTM version of .NET Core, a redefined, open source .NET available to major platforms/operating systems: Windows, Linux and Mac.

#### Goodbye, .NET 3.0 era... Oops, also 1.0, 2.0, 3.5, 4.5, 4.6 era!

Windows Communication Foundation, Windows Presentation Foundation, Windows Forms, ASP.NET Web Forms, .NET Remoting, Code Access Security... All of them are out of the game in .NET Core!

![.NET Foundation](/img/news/{{ page.title | slugify }}/net-foundation-logo.png)

.NET Core is a subset of what .NET Framework was before, and it's focused on technologies to support cloud development: ASP.NET Core MVC (a merge of ASP.NET MVC and ASP.NET WebAPI into a single development model) and Entity Framework Core, as long with most of non-tech-specific members of .NET Framework Base Class Library. Also, there's a deployment approach called *self-contained deployment* which requires *no framework* anymore: **Common Language Runtime is embedded as part of your applications and distributed as any other .NET library with the wonders of NuGet package manager!**.

Oh, and I forgot to say that .NET Core builds are available for Windows, Linux and Mac. And there's a multi-platform Visual Studio called *Visual Studio Code*.

## The obvious question: how may .NET Framework compete against .NET Core?

Actually, my opinion is that **.NET Framework won't be able to compete against .NET Core**. Why I would choose to be still on the .NET Framework route?

- I've a large code base which relies on either legacy or .NET Framework-only APIs like Windows Communication Foundation, Windows Presentation Foundation, Windows Forms, ASP.NET Web Forms, .NET Remoting, Windows Workflow Foundation and some others...
- I really love some tech/API like Windows Presentation Foundation and I found no choice outside .NET world (or I want to stay in that world), and I understand that I'm going to stick with a tech that won't evolve since it's in maintanance state.
- I still don't rely on current state of .NET Core: I would want to await until it gets more mature and feature rich (i.e. `await dotNetCore.EvolveMoreAsync()` :D).

Other than one of above choices, do you really think there's a third, or even fourth reason to continue relying on .NET Framework?

Closed-source, mastodonic, Windows-only development platforms are gone. Even Linux-only ones are gone too. It's the time for freedom: develop and deploy anywhere from anywhere.

Ok, you're right, probably you're still waiting to see **why an open-sourced, non-mastodonic, non-Windows-only development plataform is better than the opposite (i.e. .NET Framework from 1.x to 4.x)**:

1. Open source is a selling point. Both for end-users and your team colleagues.
2. Being able to check the source code of your framework or language of choice is a big chance to both *learn from magic* and overcome problems efficiently, because you can understand why you don't get what you want just checking the source code if published documentation isn't enough.
3. Cross-platform/multi-platform support is also a selling point. The more operating systems and devices are supported by a development stack, the more colleagues you'll find to both assist or get assisted by them. There's a big chance to build a great community of very collaborative members!
4. Good intentions are easier to spread around the world. Why you wouldn't want to work with .NET if you can choose the IDE, operating system, machine... Everyone knows the wonders of .NET and C#, what could you argue against them? *Don't you like strongly-typed languages? This is a good reason, indeed!*
5. Also, like 4th point above, teamwork is more enjoyable, because each team member can choose where to develop the same code as you. Your company could just provide the machine and everyone is invited to decide which operating system to work with!

## My opinion

Actually both front-end and back-end development has changed a lot since the inception of .NET Framework. Remember that there were some smartphones/PDA using Windows CE/Mobile, and we were in the PC era. Also, internet was still evolving and spreading around the world, and the Web was still about personal home pages and some massive e-commerce sites (Amazon, eBay...).

In the mean time, many open source technologies evolved and became very popular. Do you remember when ASP.NET AJAX was declared obsolete and Microsoft recommended us to use jQuery/AJAX? From these times, the Web has also evolved exponentially from the Web browser war to the JavaScript framework pyscosis!

Also, don't miss mobile development! A world where Microsoft hasn't been able to catch up with other tech giants like Google and Apple. 

Desktop is still an important platform, but smartphones are *the wave* right now, and many businesses that were used to work with laptops and desktop computers are now working with mobile/tablet line-of-business applications (I know there're still many areas where more traditional computing solutions are the best choice).

.NET Framework was conceived as a multi-purpose plataform, but anyway it was the defacto standard to develop Windows-only desktop GUI applications. Both Windows Forms and Windows Presentation Foundation have matured over the years, but they've also lost *momentum* because they're Windows-only GUI frameworks. Today there's still no choice to develop multi-platform GUI apps under the Microsoft umbrella excepting [Xamarin](https://www.xamarin.com/) (which was acquired by Microsoft during 2016). In terms of server-oriented technologies, .NET Framework has offered many APIs like ASP.NET Web Forms, Windows Communication Foundation or Windows Workflow Foundation, but they're all technologies from the past because they're built on top of what would be considered old-school approaches (too much configuration and abstraction).

In the opposite side, .NET Core is a slimmed down concept of .NET Framework, where a lot of legacy has been thrown away in favor of a more realistic focus on what can still be interesting to offer by a set of server and cloud-oriented technologies, whith a wider point of view, because .NET Core is a multi-platform, multi-device software development platform, both for developers and end-users.

While seems that Microsoft has put a great effort on showing that .NET Framework isn't dead thanks to .NET Standard










