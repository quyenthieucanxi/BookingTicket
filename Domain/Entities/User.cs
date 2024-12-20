using Domain.DomainEvents;
using Domain.Primitives;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public  sealed class User :  IdentityUser<Guid>, IAggregateRoot
{
   private User ()
   {
      
   }
   private User(UserId id,string email ,string userName, Name name)
   {
      Id = id.Value;
      Name = name;
      UserName = userName;
      Email = email;
   }

   public Name Name { get; private set; }
   
   // Navigation Property
   public ICollection<Booking> Bookings { get; set; }

   public static User Create(UserId id, string email,string userName, Name name)
   {
      var user = new User(id, email,userName, name);
      user.RaiseDomainEvent(new UserRegisterDomainEvent(id));
      return user;
   }
   private readonly List<IDomainEvent> _domainEvents = new();

   public IReadOnlyCollection<IDomainEvent> DomainEvents  => _domainEvents;
   public void RaiseDomainEvent(IDomainEvent domainEvent)
   {
      _domainEvents.Add(domainEvent);
   }

   public void ClearDomainEvents()
   {
      _domainEvents.Clear();
   }
}  