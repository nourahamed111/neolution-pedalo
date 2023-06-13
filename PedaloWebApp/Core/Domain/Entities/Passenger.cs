using System;

namespace PedaloWebApp.Core.Domain.Entities
{
    public class Passenger
    {
        public Guid PassengerId { get; set; } = Guid.NewGuid();
        public string PassengerFirstName { get; set; }
        public string PassengerLastName { get; set; }
    }
}
