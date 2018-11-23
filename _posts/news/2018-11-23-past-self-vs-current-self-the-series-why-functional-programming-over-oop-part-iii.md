---
layout: post
title: "PAST SELF vs. CURRENT SELF - THE SERIES: WHY FUNCTIONAL PROGRAMMING OVER OOP (PART III)"
excerpt: "The talk continues discussing the wonders of immutability, and some other concepts"
date: 2018-11-23 00:00:00 +0100
modifiedDate: 2018-11-23 00:00:00 +0100
categories: news
authors: 
   - mfidemraizer
---


## Continuation of [Part II](/news/2018/11/04/past-self-vs-current-self-the-series-why-functional-programming-over-oop-part-ii)

**PAST SELF**: Ooops! It's Friday, two weeks later after our last talk: what's going on?

**CURRENT SELF**: Yup! Sorry, we couldn't meet before. I've been busy these days...

**PAST SELF**: No problem, man. It's just I'm very intrigued. Do you remember that you asked me to think about questions based on our previous talks? 

**CURRENT SELF**: Sure. Do you have ones? Go, go, go!

**PAST SELF**: Let's do it. We've been discussing about the wonders of *functional programming*. But, anyway, I can visualize that I wouldn't be using *dots* to access *methods* anymore like I've been doing in OOP. 

Thus, if we're working with functions, I feel we've done a step backwards in software programming evolution.

Actually, OOP came as a successor paradigm to procedural programming, which consisted in both functions and procedures. 

So, what's up with *functional programming*? What's innovative about it?

**CURRENT SELF**: Oh! That question... Anyone coming from imperative and OOP world has made this question at some point in their learning process. 

First of all, we need to distinguish what's a *function* from *procedure*, and what's the meaning of *function* itself in functional programming.

**PAST SELF**: Functions return values, while procedures don't. I'm smart, aren't?

**CURRENT SELF**: Right, right... So, yes: this is the difference in terms of imperative paradigm. 

In the opposite side, since *functional programming* is based on a mathematical concept called *lambda calculus*, *functions* are defined as side-effect free abstraction which maps a set of inputs and produces an output. Also, *functions* in functional programming, given the same inputs, these produce the same output.


**PAST SELF**: I already implemented *functions* then in OOP!

**CURRENT SELF**: Of course. Imperative programming has also swallowed these boring concepts from mathematics.

In fact, these functions that obey the mathematical definition of *function* are *pure functions*.

**PAST SELF** : Please stop introducing more concepts! So... Let's keep on topic. What makes a *pure method* in OOP different from a *pure function* in FP?

**CURRENT SELF**: Let's see. First of all, OOP's is attached to an object, which is a mutable structure. It seems like designing methods this way should mean that you're not coding OOP anymore. 

Theoretically, an instance method which obeys the mathematical definition of *function* is also a *function*. But, what does a function do in OOP?

**PAST SELF**: I'm just theorizing about it.

**CURRENT SELF**: Understood. So, in procedural programming, both procedures and functions are just a way to box sentences to evolve reusable code. In fact, OOP is a paradigm on top of procedural. Object method's goal is reusability with a more savvy approach, but at its core, we're talking about the same definitions.

**PAST SELF**: Thus, what's makes the difference?

**CURRENT SELF**: The little big difference is FP considers *functions* as *values*. Just think about *integers* or *strings*: you're very used to work with their associated methods to perform additions and concatenations, respectively.

Functions in FP are a value like strings, integers and booleans. They may be transformed, combined, composed, *curried*... You even provide them as inputs to other functions!

**PAST SELF**: «*Curried*»? What's this?

**CURRENT SELF**: Keep us on topic! Did you get the idea? Procedural and functional programming have different meanings. Note that FP has no concept of *procedure*. All functions receive inputs and return an output. 

**PAST SELF**: This sounds disturbing. For example, I'd like to implement a *command line interface*, and when I stream text to the *standard output*, most libraries have no return value. 

**CURRENT SELF**: This is because imperative programming has no problems with side-effects. When you stream text to the standard output, there's no return value or, at least, a possible *exception* if something is wrong. Ironically, the *main output* is a side-effect. Isn't this absurd?

**PAST SELF**: Why?

**CURRENT SELF**: When you eat a nice beefsteak in your parent's home... What do them expect from you?

Let's say you eat it... How do your parents know you liked it? They're going to feel bad if you give no feedback to them!

**PAST SELF**: Yeah. So?

**CURRENT SELF**: Functional programming makes things clearer: if you eat that beefsteek, you're going to say *thank you daddy and mommy! it's wonderful*, therefore they will give you another feedback: *if you liked it, we'll buy more next week! Come to our home again!*, then you'll answer *Of course!*, and the successive feedbacks will produce new conversations for the rest of your lifes. 

Communication shouldn't be a side-effect, should be? Everything in FP is a pipeline of glued functions to produce an  expected output.

**PAST SELF**: Hold. Let's summarize our talk. FP isn't procedural programming because the former has a different meaning of *function*: functions are *values*. 

**CURRENT SELF**: That's it. And FP has functions to work with functions. And functions can be inputs and outputs of themselves. 

**PAST SELF**: Ok. It seems like my favorite language is functional then, because I've been passing functions to functions since years.

**CURRENT SELF**: Indeed. This is called *hybridization*: imperative, OOP languages are influenced by FP. Most modern language are hybrid ones. I believe we talked about this before.

**PAST SELF**: So, I'm already a FP developer!

**CURRENT SELF**: I hope so! You're just starting to understand what's FP. Perhaps you might consider yourself a *master* once you've understood what's a *monad*.

**PAST SELF**: *Mo...nad*!!!? 

**CURRENT SELF**: Oh, and do you imagine yourself coding with expressions instead of sentences? Most people has grown up understanding programming as a series of sentences with conditionals and loops, and FP avoids them like a plague!

**PAST SELF**: I got it. But you've made me feel really bad: FP seems to be full of strange words and practices from maths...

**CURRENT SELF**: It seems that we need to stop here and continue our discussion later, right?

**PAST SELF**: I think so. Will you call me to meet again?

**CURRENT SELF**: Yep! See you soon!

