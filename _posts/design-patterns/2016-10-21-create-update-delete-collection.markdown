---
layout: design-pattern
title:  "Create-Update-Delete collection"
date:   2016-10-30 00:00:00 +0100
modifiedDate: 2016-10-30 00:00:00 +0100
categories:
   - architectural
   - language-agnostic
   - design-pattern
authors: 
   - mfidemraizer
permalink: /:categories/:title
---

### Brief description

Defines an object to transport modifications to a target collection.

### What does it solve?

When working in a multi-layered architecture, an application layer may need to modify the model (or, in DDD, a *domain model* or a *domain object*). Some *models/domain objects* may contain *collection* associations:

	// Sample Customer class in C#
	public class Customer
	{
		public IList<string> EmailAddresses { get; set; }
	}

Usually the application layer modifies the model/domain using *data-transfer objects* (DTOs):

	public class CustomerDto 
	{
		// Sample Customer DTO
		public IList<string> EmailAddresses { get; set; }
	}

And a big problem arises: a simple collection-to-collection mapping from the DTO to the model wouldn't be enough, because otherwise it would mean that the *DTO* should transport the entire source collection to avoid losing data.

For example, let's say that an already persisted `Customer` has email addresses `["matias@domain-a.com", "john@ateam.com", "whatever@whichever.com"]`. When the whole `Customer` is sent to the application layer, it comes with the all `EmailAddresses`, and they're bound to the *user interface*. The *user* removes an email and saves the `Customer`. How does the model/domain know what email address was removed?

-  A valid approach would be computing an intersection of DTO's `EmailAddresses` and target model/domain object `EmailAddresses` counterpart. 
-  Another valid approach would be just mapping the DTO's `EmailAddresses` collection to the model/domain object one..

Both approaches have a big drawback: client must send the entire `EmailAddresses` collection as part of `Customer` DTO, while it would be nice that the DTO itself could express *just drop this email address* instead of re-sending the rest of email addresses that they are already persisted.



### Solution

A valid approach to solve the problem described before could be a conventional DTO that splits a single collection into *three collections*:

- A collection of items to be **created**.
- A collection of items to be **updated**.
- A collection of items to be **deleted**.

<img src="/img/createupdatedeletecollection/createupdatedeletecollection.jpg" style="display: block; margin: 0 auto; width: 256px">

That is, the DTO can express which items should be *created*, *updated* or *deleted* and the model/domain will map proposed changes to persist requested changes.