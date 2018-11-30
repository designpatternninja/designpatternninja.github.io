---
layout: post
title: "PAST SELF vs. CURRENT SELF - THE SERIES: WHY FUNCTIONAL PROGRAMMING OVER OOP (PART II)"
excerpt: "The discussion continues. After introducing the basics of functional thinking, let's go deeper on the topic!"
date: 2018-11-04 00:00:00 +0100
modifiedDate: 2018-11-04 00:00:00 +0100
categories: news
authors: 
   - mfidemraizer
---

## Continuation of [Part I](/news/2018/10/29/past-self-vs-current-self-the-series-why-functional-programming-over-oop-part-i)

**CURRENT SELF**: Hey! You came back to continue the discussion, right?

**PAST SELF**: Why not? I'm really interested about the topic. Let's start where we left it on last Monday.

Do you remember that you mentioned the fact that there're *purely-functional programming languages*? What do you mean with that?

**CURRENT SELF**: Let me introduce the topic while I avoid a formal definition. I'd like to get you to the conclusion of why a purely-functional language may be benefical.

**PAST SELF**: Of course, go forward.

**CURRENT SELF**: Our previous discussion was about how there's yet room to accept that another mindset (i.e. *functional one*) could be also considered as *how our mind works* in opposite to OOP, the predominant mindset in programming field.

**PAST SELF**: Indeed.

**CURRENT SELF**: We got to the conclusion that functions are abstractions of actions, where these require input *data* , and data has to fit an expected shape, so that function produces an output of same or other shape.

Try to visualize the following case: there's a lab which *analyzes water healthiness*. There would be a function called `analyzeWaterHealthiness` which would input a *water sample*, and it would output a *report*.

**PAST SELF**: That is, when we analyze water's healthiness, we've lost a part of water reserves.

**CURRENT SELF**: Good catch! Exactly. Why? Because we need to use chemical substances to analyze the whole water. Otherwise, we would poison people to know if that people would be poisoned by the water that needs to keep healthy conditions to be consumed by us. 

**PAST SELF**: That's obvious.

**CURRENT SELF**: In OOP's mindset we would have thought about a water tank owning an action called *analyzeHealthiness* receiving a *water sample* too. But there's a problem here. 

In fact, since OOP is about mutability and object methods may alter object's state, it would mean that there's no guarantee of not altering own water sample's healthiness to check its own healthiness by design. Wouldn't you mix some chemical components with the whole water sample to get the report?

**PAST SELF**: Sounds logical. What's the alternative? We wouldn't be able to analyze some water sample without previously using certain substances on it. 

**CURRENT SELF**: Absolutely. BTW, in FP mindset, you wouldn't do that. Functional languages tend to offer reference and data immutability by design. So, how can we analyze a water sample without mess up it?

**PAST SELF**: Actually, I can't imagine what I would do in this case. And you?

**CURRENT SELF**: Have someone be able to produce water in some lab?

**PAST SELF**: Not yet.

**CURRENT SELF**: What if we could do that? How would you solve the issue in order to avoid wasting the existing water?

**PAST SELF**: Sorry, I don't follow you right now. Since this isn't possible, we would need to do some research on how to use those chemical substances to get the healthiness report, and eliminate them from the water. We can't do the impossible.

**CURRENT SELF**: Be flexible, *like water*...

**PAST SELF**: Oh, *Bruce Lee has come alive again*...  

**CURRENT SELF**: No, you need to be *Bruce Lee* to understand this! Let's see. If we would be able to *create water*, we could solve our issue and also one of core conflicts of the next World War. 

If we've that power, nothing prevents us from *cloning* a water sample and leave it out again in the tank, while we mess up the cloned sample to get the healthiness report. Now, do you get it?

**PAST SELF**: Wait... Are you telling me that functional programming has this property? So, you never mutate the inputs, don't you?

**CURRENT SELF**: That's it, man! You got it!

**PAST SELF**: But, doesn't this mean wasting a lot of memory overtime? If every input is a copy of some previous output...

**CURRENT SELF**: *Your lack of faith is disturbing...*.

**PAST SELF**: I was *Bruce Lee* already. Now I'm *Admiral Motti* from Star Wars too...

**CURRENT SELF**: Whatever. FP languages implement memory optimizations to avoid that problem. So, for example, when the runtime creates a dictionary, and you add a new pair, the runtime produces a new instance of the whole dictioanry which has a reference to the previous one. 

**PAST SELF**: You said that *you add a new pair*. This is mutation. Where's that immutability?

**CURRENT SELF**: The verb *to add* should be understood as *I create a new dictionary on which I reference the previous one, and it has a new pair*. Is it clear now? The dictionary data structure is memory optimized, and it's incremental and it's still immutable. You'll never mutate the previous dictionary: you get a new dictionary wrapping the previous one.

**PAST SELF**: I got it.

**CURRENT SELF**: This is applicable to many other data structures. Immutability is a very important feature in functional languages. It avoids a lot of problems, and turns others into simpler ones.

For example, parallel programming or threading are a breeze with immutability. You don't need to synchronize resources anymore. You get less noise, less complexity, and at the end of the day less issues in the long run. This means, increased maintainability. 

**PAST SELF**: Nice! 

**CURRENT SELF**: Now let's take things further. Not all functional languages provide immutability by default and/or by design...

**PAST SELF**: Hold: it's too late again. I've to do some stuff at home! It has been a very interesting talk, but we need to leave it for now until next Monday.

**CURRENT SELF**: Oh, OK. Anyway, I hope you'll think about what we've talked these last two conversations, and you ask me some questions, right? 

**PAST SELF**: Absolutely. I've some already, but I need to go. Thank you and see you soon!

**CURRENT SELF**: See you! Goodbye!

## [Next part: III](/news/2018/11/22/past-self-vs-current-self-the-series-why-functional-programming-over-oop-part-iii)
