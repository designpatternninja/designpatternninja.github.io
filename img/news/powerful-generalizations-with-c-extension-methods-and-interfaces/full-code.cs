// (c) 2017 - Matías Fidemraizer / http://designpattern.ninja
// Distributed under Creative Commons' Attribution-NonCommercial-ShareAlike 4.0 International license
// https://creativecommons.org/licenses/by-nc-sa/4.0/legalcode

using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatternNinja.Demonstration
{
    public interface ICanGetObjectsById<TId, TObject>
    {
        TObject GetById(TId id);
    }
    public interface IUniquelyIdentifiable<out TId>
    {
        TId Id { get; }
    }


    public class DomainObject : IUniquelyIdentifiable<Guid>, IEquatable<DomainObject>
    {
        public Guid Id { get; set; }

        public virtual bool Equals(DomainObject other)
        {
            if (other == null) return false;

            return ReferenceEquals(this, other) || Id == other.Id;
        }

        public override bool Equals(object other) => Equals(other as Tag);
        public override int GetHashCode() => Id.GetHashCode();
    }

    public interface ITag : IUniquelyIdentifiable<Guid>
    {
        string Name { get; }
    }

    public interface ITaggable
    {
        ISet<ITag> Tags { get; }
    }

    public class Tag : DomainObject, ITag
    {
        public string Name { get; set; }
    }

    public class Contact : DomainObject, ITaggable
    {
        public ISet<Tag> Tags { get; set; } = new HashSet<Tag>();
        ISet<ITag> ITaggable.Tags => new HashSet<ITag>(Tags.Cast<ITag>());
    }

    public interface IContactRepository
    {
        Contact GetById(Guid id);
    }

    public interface IContactService : ICanGetObjectsById<Guid, Contact>
    {
    }

    public class DefaultContactService : IContactService
    {
        public DefaultContactService(IContactRepository repository)
        {
            Repository = repository;
        }

        private IContactRepository Repository { get; }

        public Contact GetById(Guid id) => Repository.GetById(id);

    }

    public static class TaggingExtensions
    {
        public static void AddTags<TId, TObject>(this ICanGetObjectsById<TId, TObject> objectByIdGetter, TId id, IEnumerable<ITag> tags)
            where TObject : ITaggable, IUniquelyIdentifiable<TId>
        {
            TObject someObject = objectByIdGetter.GetById(id);

            foreach (ITag tag in tags)
            {
                someObject.Tags.Add(tag);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            IContactService contactService = new DefaultContactService(null);
            Guid contactId = Guid.Parse("383488f0-4ad6-48e9-aeb6-b975292332f1");

            contactService.AddTags
            (
                contactId,
                new[] { new Tag { Name = "a" }, new Tag { Name = "b" } }
            );
        }
    }
}