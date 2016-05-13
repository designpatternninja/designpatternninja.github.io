---
layout: design-pattern
title: Proud developer
date: 2016-05-08 00:00:00 +0100
modifiedDate: 2016-05-08 00:00:00 +0100
categories:
    - anti-pattern
    - behavioral
authors: 
   - mfidemraizer
permalink: /:categories/:title
---

### Brief description

It is a software development professional which cannot accept that work made by others is as valid as (or even better than) the one done by himself/herself.

### What causes it

Modern software development is built on top of *libraries* and *frameworks* which can be understood as *black boxes*, because one will blindly use them because *the docs* say *do what you want this way and it will always work in any scenario*. 

Above situation introduces some degree of risk, because if you don't own the source code, or you own it but you can't understand it (or you are out of time to learn how it works...), when you use a software library or framework which is faulty or throw exceptions, unless they their developers have provided good documentation or the errors come with good and useful messages and debugging information, you are almost done.

In the other hand, sometimes one may enter to a new job position, and he/she may realize that there is a lot of previous work made by current and past employees or collaborators in a given project, and it might take some time to get used with solutions made by others.

### The problem

The proud developer refuses to use work done either by external teams like open source projects or co-workers/collaborators mostly because some kind of mediocrity.

For example:

|  Argument  | Answer |
|  --------  | ------ |
| *I don't use that object-relation mapper because it's a big abstraction and it works slowly, ignoring that any abstraction layer is always a synonym of reducing overall system performance but it should increase productivity and maintainability, so I've developed my own object-relational mapper which is blazing fast!*. | **Yes, many object-relational mappers can be slow out-of-the-box. Abstractions can have some impact on overall performance but you use them because they solve a lot of issues and let you concentrate on your actual problem instead of writing a lot of insfrastructure code. Mostly, productivity is more important than premature optimizations. Most object-relational mappers can be tweaked to work better in your particular scenario...** |
| *In Web development, I don't use an UI framework because I want to interface with DOM directly: I don't like frameworks!* | **That sounds great! But as soon as you develop a full HTML5-based application you realize that DOM isn't the core problem when implementing intensive JavaScript-based applications...** |
| *It took the entire day to realize what was causing the issue.* | **What about asking your colleague about your issue? Or do you know StackOverflow.com ;)** |
| *I've been trying to solve the issue this way. Now it just works, and your solution and mine are fine.* | **The fact that both solution *work* doesn't mean that both solutions are good approaches to solve the issue** |
| *3 years ago, in some project, we had a lot of issues with this technology, I'll provide my own solution to avoid these issues* | **Who knows what happened 3 years ago. The world has advanced since *3 years ago* and we can't refuse to use a solution just because your old memories. And now we are other people than the one which participated in that other project.** |
| *Your code is crap, I've never used this approach and you want to change my mind* | **Life is full of changes. You need to go forward and keep learning new technologies, design patterns, paradigms and programming languages... The world can evolve with or without you!** |



