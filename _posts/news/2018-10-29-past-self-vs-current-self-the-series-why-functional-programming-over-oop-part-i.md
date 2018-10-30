---
layout: post
title: 'PAST SELF vs. CURRENT SELF - THE SERIES: WHY "FUNCTIONAL PROGRAMMING" OVER "OOP" (PART I)'
excerpt: "I start a series of posts on which I'll discuss with my past about different topics about software programming using dialectics"
date: 2018-10-29 00:00:00 +0100
modifiedDate: 2018-10-30 00:00:00 +0100
categories: news
authors: 
   - mfidemraizer
---

**PAST SELF**: How's going? What have you been learning since we met last time?


**CURRENT SELF**: Yup! Fine! I believe you know I'm someone who loves to learn and put in doubt everything. More or less a year ago I thought about questioning some of my knowledge and skill set around programming. Concretely, the *object-oriented programming* (OOP).


**PAST SELF**: Really? Is it that just because you want to diverge? Do you declare yourself an OOP rebel?


**CURRENT SELF**: No way. I wonder if everyone has felt implementing too much ceremony to do simple things.


**PAST SELF**: May be. Anyway, we could be agreed about classes as archetypes in the software field, or even objects, form a reasoning which is considered to mimic how our mind actually works.


**CURRENT SELF**: Indeed. Let's say a car has a body, doors that can be opened, a *five-speed gearbox*. Or the *wheel* to control the *direction*. Isn't this an exercise of abstraction? It seems like we could agree about that, but, what if we have been influenced by decades of a specific mindset?


**PAST SELF**: Enlighten me.


**CURRENT SELF**: Let's go! I'm going to try it. Think about a chair. Do you see it in your mind?


**PAST SELF**: Right.


**CURRENT SELF**: Describe the first thing that you've identified about the chair.


**PAST SELF**: Perhaps it's obvious: it's a tool on which you rest your ass!


**CURRENT SELF**: Indeed, you've discovered nothing amazingly original!


See. Probably some people may put their eyes in aesthetics. However, it seems that you've directly thought about the functionality of some given chair: it's a place to sit down. Is it its shape, color or height really relevant at glance?


**PAST SELF**: This is asking something which drives to a very subjective answer: you take for granted that almost anyone would provide a similar point of view. Where are you going to?


**CURRENT SELF**: I disagree. When you go to the supermarket to buy dish detergent, you do it because you're looking for something with a concrete utility: you may choose a certain product because of its aesthetics or price, but at the end of the day that product works to wash the dishes.

When you organize your shopping, what do you ask yourself?

Let me give you some examples: I need cookies because I want snack something in the afternoon, or I need garbage bags because I'd like to hygienically store what I don't need anymore and throw it away later. Did I mention anything which couldn't be considered functionality?


**PAST SELF**: Not for now.


**CURRENT SELF**: Alright. Now I'm going to ask you another thing: once you know what's needed, do you accept whichever thing? For example, you need a screwdriver because you've identified that you need to unscrew. But, does any screwdriver work for you? The answer is no.

So, the next question is what kind of screwdriver you really need: a star screwdriver, a plain screwdriver? Also, you need the screw diameter and its height.


**PAST SELF**: Indeed. Functionality is taken for granted (i.e. unscrew), and we focus on details based on what's the purpose of the whole screwdriver.

So what?


**CURRENT SELF**: I guess that you've already noted a clear separation of tool's functionality and its concrete details. There're star screwdrivers, plain screwdrivers...


**PAST SELF**: Yes, but again: so what?


**CURRENT SELF**: How could you still be saying "so what?" Our starting point was the OOP is the nearest mindset to how human mind actually works. This paradigm drives us to think in concepts first, later we go for concreteness, and finally we identify actions (usually called "methods").

See, I really believe I've raised you another point of view to tackle the same problems.


**PAST SELF**: Who cares about the order? Aren't we still talking about the same thing?


**CURRENT SELF**: No, we're talking about a substantial difference: OOP works around objects which perform actions and communicate with other objects. Nevertheless, I'm posing you that we're segregating functionality from concrete attributes (i.e. data) that may be mandatory to let something work: unscrewing star screws requires screws owning a star screw.

Now let's express the same problem from OOP standpoint: the star screwdriver requires a star screw.


**PAST SELF**: I doesn't make sense to me. What's going on?


**CURRENT SELF**: There's an essential paradigmatic difference.


In opposite to how I express the object-oriented analysis, I don't talk about the screwdriver. Do you get it?

Instead, when I develop the object-oriented analysis, I identify an archetype (the screwdriver), a concrete concept (the star screwdriver), and what's meant to do (unscrew star screws).


**PAST SELF**: You're right. Go forward.


**CURRENT SELF**: Then, without recurring to labels, we see two different paradigms which try to address the same problem (unscrew star screws).

When we talk about a functionality (unscrew) and then an input (a star screw), and an output which would be the fail to unscrew the screw or how many times you had to turn the screwdriver, firstly we've thought about functions, then the data.

In the opposite side, we've talked about a concept (the star screwdriver) which has a functionality (unscrew star screws), the input would be a star screw, and probably it may not produce an output: success will be quiet, and failure may throw an exception.


**PAST SELF**: I see! This is interesting. But, anyway, I've seen this mindset as part of OOP languages since a lot years. After all, what's new with this?


**CURRENT SELF**: Basically, you're right pointing out that there's nothing new with that. It's just you didn't realize that when you put functions first, then you think about the data, we're talking about functional programming (FP).

FP isn't a newcomer. In fact, it's older than OOP.

Note that most programming languages are hybridizations of what we call imperative programming (the meta-paradigm on which OOP is based on), and FP.


**PAST SELF**: It sounds wonderful: we get the best of both worlds.


**CURRENT SELF**: Actually, I would say that FP in OOP simplifies or compacts mechanisms and practices: you get more with few lines. But it's still imperative programming and OOP. 

FP represents a paradigm that can work independently. When a language is *functional only*, we say that it's a purely-functional programming language.


**PAST SELF**: Really? Would you care to elaborate more about this? It's interesting.


**CURRENT SELF**: Maybe another day. We've talked too much for now and it's too late. We may continue the discussion further another day. Is it next Monday fine for you?


**PAST SELF**: It sounds perfect for me! See you on next Monday.