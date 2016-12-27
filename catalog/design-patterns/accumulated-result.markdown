---
layout: design-pattern
title:  "Accumulated result"
date:   2016-03-12 17:47:10 +0100
modifiedDate: 2016-03-12 17:47:10 +0100
categories:
   - architectural
   - language-agnostic
   - n-layer
   - design-pattern
authors: 
   - mfidemraizer
---

### Brief description

Represents a multi-purpose, orthogonal entity which transports both results of a called operation and also useful information for the callers like a status, status description and details about the actual result of the whole operation.

### What does it solve?
In multi-layered architectures like MVC, DDD and others, either if they are n-tier or not, top software layers need to receive a *result*. Usually, the whole *result* is just the object resulting of calling an operation against the system.

While just providing *the object* sometimes is enough, this is not true when there is a need to implement a proper way of handling unsuccessful requests.

Consider the following domain-driven design sample flow, where there is a point - *the RESTful API endpoint* - that transmits data over the wire and the so-called data is received by a client which may or may not work with the same platform as the backend:

![Layering sample](/img/accumulated_result/layering_sample.png)

Any layer within the above sample excluding the *repository* (since it is the lowest layer in the stack) may need information about how previous operation in the past layer has ended (*did it finish fine?*).

Above paragraph introduces the following issues:

- Does the operation finished successfully or it is faulted?
- If it is faulted, what happened? 
- If it was a faulted, why it happened?

#### This pattern is not a replacament for exceptions

Exceptions are not covered by this pattern. An operation is not considered as *unsuccessful* if an exception is thrown within its call stack. This pattern covers non-exceptional cases like unsuccessful model validation. That is, **do not stop throwing exceptions replacing them with accumulated results, because exceptions usually are a sign of possible *bugs* or *unexpected behaviors* like database connection loss, configuration errors, architecture flaws...**

### Solution

Since layers on top of others may need information about how what happened in a bottom layer, bottom layers need to give details to the next layer in addition to the *object or value that may return a regular operation*. 

That is, an operation will need to notify the next layer the following details:

* Was it successful or unsuccessful?
* If it was unsuccessful...
 * ...what was the reason?
 * ...what was affected by the fault?

Also, most successful operations will return...
* No object at all. They will just provide a successful or unsuccessful status. This case will be called **basic result**.
* A single object. Some operations will require to return an object by its unique identifier or based on some criteria. This case will be called **single-object result**.
* Zero or more objects. Some operations will list zero, one or more objects. This case will be called **multiple-object result**.

Thus, all operations within a system may return three kinds of results:
- Basic result.
- Single-object result.
- Multiple-object result.

All three possible results share some data in common:
- They provide that the operation was successful or not.
- They provide a message describing what went fine or what went wrong.
- they provide a dictionary or affected resources, where the keys are resource names and values are detailed reasons to failed resources.

See the following diagram for a basic result class:

![Basic result](/img/accumulated_result/basic_result.png)

And single-object and multiple-object results may be designed as the following diagram:

![Basic result](/img/accumulated_result/single_multiple_result.png)

The three types of possible results will be the return type of any operation in any layer (excepting infrastructure/ortogonal layers), and this will allow any other layer on top of the others to accumulate the required info to take the necessary actions to notify what and why went wrong if an operation call is faulted.

The point of this pattern is a result instance is shared accross of all layers, from the bottom to top of the software layering stack, and each layer can alter the state of the whole result. For example, an *user repository* may try to add an user but it does not meet the domain specification for user domain object, and the `IRepository.Add` operation returns an unsuccessful result which notify the caller that the operation went wrong because the *user name cannot be null or empty*. Layers on top of repository may want to add more affected resources, an expanded description to the issue or leave it *as is* to transmit it over the wire. This is the reason to call this pattern ***accumulated*** *result*.



### Implementations
***In C#***

	public class BasicResult
    {
    	public bool IsSuccessful { get; set; }
    	public string Description { get; set; }
        public Dictionary<string, string> AffectedResources { get; } = new Dictionary<string, string>();
    }
    
    public sealed class SingleObjectResult<TObject> : BasicResult
    {
    	public TObject { get; set; }
    }
    
    public sealed class MultipleObjectResult<TCollection, TObject> : BasicResult
    	where TCollection : ICollection<TObject>
    {
    	public TCollection Objects { get; set; }
    }
    
    public interface IRepository<TDomainObject>
    {
    	public SingleObjectResult GetById(Guid id);
        public MultipleObjectResult<IList<TDomainObject>, TDomainObject> List(int take = 10, int skip = 0);
    	public BasicResult Add(TDomainObject someObject);
    }
    
    public sealed class User 
    {
    	public Guid Id  { get; set; }
        public string Name { get; set; }
    }
    
    public sealed UserRepository : IRepository<User>
    {
    	public SingleObjectResult<User> GetById(Guid id)
        {
        	return new SingleObjectResult<User> 
            { 
            	IsSuccessful = true,
            	Object = new User { Name = "Dummy user" } 
            };
        }
        
        public MultipleObjectResult<IList<User>, User> List(int take = 10, int skip = 0)
        {
        	return new MultipleObjectResult<IList<User>, User>
            {
            	IsSuccessful = true,
            	Objects = new List<User> { new User { Name = "Dummy user" } }
            };
        }
        
        public BasicResult Add(User someObject) 
        {
            // Sample unsuccessful try to add an user which does not 
            // meet some domain specification
        	return new BasicResult 
            { 
            	IsSuccessful = false,
                Description = "Given user does not satisfy current specification",
                AffectedResources = new Dictionary<string, string>
                {
                	{ "User.Name", "Given name cannot be null or empty" }
                }
            }
        }
    }