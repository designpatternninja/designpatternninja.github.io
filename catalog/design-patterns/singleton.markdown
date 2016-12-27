---
layout: design-pattern
title: Singleton
date: 2016-04-24 00:00:00 +0100
modifiedDate:   2016-04-24 00:00:00 +0100
categories:
       - creational
       - language-agnostic
       - design-pattern
authors: 
   - anonymous
   - mfidemraizer
---

### Brief description

Defines a globally-accessed object constrained to a single instance within an entire program and the whole class must not be publicly instantiable to avoid more than an instance within the so-called program.

### What does it solve?

In many solutions some data, used during the execution of some program, needs to happen once and they should be easily accessible from one or more software layers.

Good samples of above described scenario could be:

- *Settings parsed from disk*. In order to avoid accessing the disk many times, the program parses the settings once and they remain exposed from memory during the executing of a given program.
- *Heavy initialization*. Many frameworks, libraries and APIs require many resources to work properly. For example, some given framework may expose some components as objects. Initializing these objects can be time-consuming. 

These cases can end up in being too heavy to execute them all over again whenever some code depends on them since it can decrease overall performance of the system. 

Alternatively, there is other possible problem: sometimes some objects need to be built once and avoid further modifications within the execution cycle of some program. For example, some *immutable settings* where a system may require settings that should be configured *offline* in some configuration file, and they should be immutable during the execution of a given program. These settings may be read from disk once and loaded into memory, and they may be exposed as immutable data within the program.

### Solution 

Usually it is implemented in class-based object-oriented programming languages as regular, non-inheritable classes with a *private constructor* (or any other approach) to prevent that other code outside the class itself could instantiate the so-called class.

In the other hand, the *singleton class* may contain both static and instance members. A public static member (usually a *property* or *attribute*) exposes the *single instance* and *instance members* initialize the instance being created by the static code (see *Implementations* section to get further details on how to implement *singleton* in code). Since the whole property exposing the *single instance* is *static*, the single instance is globally-accessible from any software layer.

### Implementations

#### In C# #

In C#, static initializers are executed once within the execution of a program, and static members with initializers are initialized when one of them is first accessed. Also, static initializers are also *thread-safe*.

In the other hand, because static member initializers are executed once per program life-cycle and when they are accessed first time, a C# singleton can be considered lazy-loaded by default.

A simple singleton implementation may look as follows in C# 6 and above:

    public sealed class DatabaseSettings
    {
        private DatabaseSettings() {}

        public static DatabaseSettings Instance { get; } = new DatabaseSettings();

        public string ConnectionString { get; private set; }
        public string Url { get; private set; }
    }

A variation of above implementation may be required if the single instance should be configured some way, and static initializers would be replaced by a *static constructor*:

    public sealed class DatabaseSettings
    {
        private DatabaseSettings() {}

        static DatabaseSettings()
        {
            Instance = new DatabaseSettings();
            Instance.ConnectionString = "<some connection string>";
            Instance.LockSettings();
        }

        public static DatabaseSettings Instance { get; }

        public string ConnectionString { get; private set; }
        public string Url { get; private set; }
    }

#### In JavaScript

In JavaScript there is no actual approach to implement *singleton*, and depending on the ECMA-Script 262 standard, it may be implemented in different ways.

Alternatively, *singleton* can be partially implemented.

##### ECMA-Script 5

One approach with ECMA-Script 5 is using *globals*. A global variable holding [a *freezed* object](https://developer.mozilla.org/en/docs/Web/JavaScript/Reference/Global_Objects/Object/freeze) may act as a *singleton*:

    var global = window || GLOBAL;
    global.DatabaseSettings = Object.freeze({
        connectionString: "<some connection string>",
        timeout: 90
    });

##### ECMA-Script 2015 and above

With ECMA-Script 2015, JavaScript introduced syntactic sugar to support class-based object-oriented programming. It also provides syntactic sugar to simulate static members, but these are still declared as constructor function's members.

Actually, class-based syntax is a syntactic sugar over *prototypal inheritance* and, during run-time, ECMA-Script 2015 does not behave differently than ECMA-Script 5 and other earlier version of ECMA-Script 262 standard.

A possible singleton implementation would look as follows:

    class DatabaseSettings {
        get connectionString() {
            return this._connectionString;
        }
        set connectionString(value) {
            this._connectionStirng = value;
        }

        static get instance() {
            if(!this.hasOwnProperty("_instance")) {
                this._instance = new DatabaseSettings();
            }

            return this._instance;
        }
    }
There is no built-in approach to avoid creating more instances and, in the other hand, there is no way to prevent the singleton class from being derived in many other classes or extended using prototypal inheritance without syntactic sugar.