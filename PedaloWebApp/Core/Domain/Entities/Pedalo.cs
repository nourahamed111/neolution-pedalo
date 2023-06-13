namespace PedaloWebApp.Core.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Pedalo
    {
        public Guid PedaloId { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public PedaloColor Color { get; set; }
        public int Capacity { get; set; }
        public decimal HourlyRate { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
        public int AvailableCapacity
        {
            get { return Capacity - BookingCount; }
        }

        public int BookingCount
        {
            get
            {
                if (Bookings != null)
                {
                    return Bookings.Count;
                }
                return 0;
            }
        }
    }
   

    public enum PedaloColor
    {
        Red,
        Blue,
        Pink,
        Green,
        Yellow,
    }
}
