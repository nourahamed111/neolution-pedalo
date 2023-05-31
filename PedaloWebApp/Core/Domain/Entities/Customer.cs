namespace PedaloWebApp.Core.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Customer
    {
        public Guid CustomerId { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthdayDate { get; set; }

        // Navigation property to access the bookings made by the customer
        public virtual ICollection<Booking> Bookings { get; set; }

        // New property to store the total revenue
        public decimal TotalRevenue
        {
            get
            {
                if (Bookings != null)
                {
                    return Bookings.Sum(booking => booking.TotalCost);
                }
                return 0;
            }
        }
    }
}
