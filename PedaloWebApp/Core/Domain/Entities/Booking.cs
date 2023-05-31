namespace PedaloWebApp.Core.Domain.Entities
{
    using System;

    public class Booking
    {
        public Guid BookingId { get; set; } = Guid.NewGuid();
        public Guid CustomerId { get; set; }
        public Guid PedaloId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Pedalo Pedalo { get; set; }
        public Customer Customer { get; set; }

        // New property to calculate the total cost of the booking
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
