using System;
using System.Collections.Generic;

namespace PedaloWebApp.Core.Domain.Entities
{
    public class Booking
    {
        public Guid BookingId { get; set; } = Guid.NewGuid();
        public Guid CustomerId { get; set; }
        public Guid PedaloId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PassengerNames { get; set; } // New column for storing passenger names
        public Pedalo Pedalo { get; set; }
        public List<Passenger> Passengers { get; set; }
        public Customer Customer { get; set; }
        public bool IsFull
        {
            get { return Pedalo.AvailableCapacity <= 0; }
        }
        public decimal TotalCost
        {
            get
            {
                if (EndDate.HasValue)
                {
                    TimeSpan duration = EndDate.Value - StartDate;
                    decimal totalHours = (decimal)duration.TotalHours;
                    return totalHours * Pedalo.HourlyRate;
                }
                return 0;
            }
        }
    }
}
