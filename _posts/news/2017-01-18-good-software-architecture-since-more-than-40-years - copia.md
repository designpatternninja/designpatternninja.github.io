---
layout: post
title: Good software architecture, since more than 40 years
excerpt: "Software development started more than 40 years ago. Does our legacy should be thrown away to reinvent the wheel every year, everyday?"
date: 2017-01-18 00:00:00 +0100
modifiedDate: 2017-01-18 00:00:00 +0100
categories: news
authors: 
   - mfidemraizer
---

Let's start the discussion with a diagram and check the signature at the bottom of it (press on the diagram to see its full version):

[![Diagram](/img/news/good-software-architecture-since-more-than-40-years/old-diagram-small.jpg)](/img/news/good-software-architecture-since-more-than-40-years/old-diagram.jpg){:target="_blank"}

Did you notice that there's a signature from **1970**? 

When I was a teenager, I had some low moments but I had the chance (and good luck) to live with **Ferdinando Marzoni** for some years. He's unknown to the masses, but **he was a programmer from the 70s to 90s**, and he experienced the early ages of software and computing. **Some of his clients were IBM and Pirelli**.

![Ferdinando Marzoni](/img/news/good-software-architecture-since-more-than-40-years/ferdinando-marzoni.jpg)

A while before he died, he inherited me many functional and technical analysis from projects for oil market, public administration and stock management. The diagram included on this article is part of one of these analysis composed by more than 500 pages each one!

## In 1970, there was no *domain-driven design*, *test-driven development*, *MVC* or many other concepts, paradigms, practices...

Software programmers from the 70s were very different than ours. Computers were just evolving and their computational power and memory were very very limited, and software engineering was just a dream. Moreover, professionals had to emerge learning from technical papers (sorry, *înternet was even out of their imagination), and it wasn't easy to work with hexadecimal codes, or later code everything with machine language (i.e. *assembler*). Other projects were already implemented using Basic, C, Fortran, Cobol and relational databases.

As software programming was considered a task done by extraterrestrials, projects were well-paid for the effort that required to end up with a good solution. Also, finding programmers was an extreme hard task!

Ferdinando Marzoni explained me that old days' projects were funded with over 1 million dollars and clients weren't never obstructive during the development process: they provided the resources and relied on professionals. Projects were always planned to be executed during 1-2 years, and it was always an initial intensive task to document all concerns exposed by the client. Anyway, Ferdinando did partial demonstrations of the products being developed to validate them against the client.

While the key point on 70s-80s projects was memory/processor efficiency rather than a comprehensive and ultra-productive software architecture, he pointed me that even with procedural programming he and his teams implemented well-structured code. In fact, when he was too old to continue as programmer and it took place the transition from MS-DOS to Windows and graphical environments, as programming languages were more and more higher-level, he thought that programmers didn't care about producing *good software* but they started to be more focused on the result: *it just works, what's the issue with this, eh!*.

## Computing science legacy is ignored very often

Let me point out that based on my experience and own thoughts, I feel that both companies and very often professionals ignore that there's still room to innovate or invent new paradigms and programming styles, **but we can't ignore our legacy**.

When computers became a mainstream product and software development kits were cheaper or absolutely free of charge, people started to think that programming was *easy*: if you own a computer, you can do it yourself.

Many individuals have started businesses creating their own applications and it turns out that something that *looked good* was enough to sell it out. Everyone could (and still can...) run successful business with software plagued by bugs and very bad design decisions... But the selling point was (and is) software programming services became cheap in few years: from Ferdinando Marzoni's multimillion projects to pay for a complete custom CRM about $20k (or even less!).

## Some new but outdated technical discussions

Did you find yourself discussing with other coworkers or technical managers things like doing relational database design right? Have you ever had to discuss why others shouldn't create relations between objects but pointing out that composition is the answer?

Probably I would be able to toss dozens of possible discussions that happen too often when you work together with other team members, and most of them can be summarized into a very simple sentence: ***please stop reinventing the square wheel once more!***.

While many technologies like *relational database management systems* (RDBMS), programming languages and other related things have evolved since Ferdinando Marzoni's old days, we should recall to ***good old software architecture, since more than 40 years*** to solve many problems and **get absolutely focused on problems that have still room to improve their known solutions**.

Possibly, the worst attitude I've found on technical and/or business managers, and also in some coworkers, is that **a bunch of ignorance forces us to justify what has already worked for more than 40 years** and they try to convince us that it's *our approach* instead of *one of most accepted approaches by the entire software development community*. Take the following invented chat as a sample of what I've said on this paragraph:

> **[Matías]** > We should embrance a good separationf of concern, layering and we all know that despite of *DDD* not being perfect, we really need a service and repository layer, don't we?
> 
> **[Manager/Coworker]** > Too much layering, it should be another approach to make things simpler.
> 
> **[Matias]** > Yeah, actually computing science is still in its infancy, but our goal on this project shouldn't be starting a deep R&D about how to improve current state of software development. *DDD* just works, with some *cons*, of course.
>
> **[Manager/Coworker]** Usually we could implement the same requirement in less time... well, why don't you start your project your way and later we see if it's a good choice or not?
>
> **[Matías]** Right... BTW we're going to implement a large project with a lot of concerns to solve... There's a small room here to get other choices, and those would end up in a custom solution that will mimic *DDD* with no formal way of knowing what paradigm and patterns we've really used in our project, and this has high impact on teamwork and future new coworkers...
>
> **[Manager/Coworker]** Well, let's do your way... but I'm not convinced that we're in the right track.

*So it's better to implement something the **chaotic way** because at least I'm aware of my wrong but known approach*. **Background: I've been eating spaguettis since my birth, don't try to change my mind and introduce vegetables into my diet!** ;)


**Note that I'm not against innovation and creativity**. I find myself very often going beyond *the standard solution*, but I've always thought that **software development is *rational* and founded on the same pillars of scientific knowledge**. Thus, you must go further on everything, but you need to try to **don't re-invent the wheel just for the sake of reinventing it**. If you don't know a better approach, use what we, everyone has already found useful in the last 40 years.