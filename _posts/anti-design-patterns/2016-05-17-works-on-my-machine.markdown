---
layout: design-pattern
title: Works on my machine
date: 2016-05-17 00:00:00 +0100
modifiedDate: 2016-05-17 00:00:00 +0100
categories:
    - anti-pattern
    - behavioral
authors: 
   - anonymous
   - mfidemraizer
permalink: /:categories/:title
---

### Brief description

A software developer argues that something that has already implemented works just because it worked on his development machine, even when someone has told that it doesn't work on a development server and/or production server, or in some co-worker machine. 

### What causes it

Many software developers have used to work *alone*. That is, they've not got how to work as part of a development team. They've always considered that something that works on their machine should work anywhere.

Usually this situation happens when a *manager* or any *co-worker* tells to some developer that some part of some application or service doesn't work, and the author of the code answers *it works on my machine!*...

## The problem

When something works on author's machine but neither does in other colleague's one, or in some server, means that a given source code misses some part of the whole code, some software component, library or framework is present on developer's machine where the code already works but they're missing in the rest of machines.

This is a symptom of maybe:

- ...the source code isn't reviewed ofter to check that it's correctly pushed to the version control system (for example, GIT, Mercurial, SVN...).
- ...not using *continuous integration* and not implementing *automated tests* to verify that the source code compiles and can be executed.
- ...just some developer's mediocricity, because that guy or girl doesn't test himself/herself the code in the right way to be sure that it'll work under practically any condition.
- ...a combination of all above situations...