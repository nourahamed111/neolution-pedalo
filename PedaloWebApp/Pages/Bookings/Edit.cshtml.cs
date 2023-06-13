using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PedaloWebApp.Core.Domain.Entities;
using PedaloWebApp.Core.Interfaces.Data;

namespace PedaloWebApp.Pages.Bookings
{
    public class EditModel : PageModel
    {
        private readonly IDbContextFactory contextFactory;

        public EditModel(IDbContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        [BindProperty]
        public BookingEditModel Booking { get; set; }
        public List<Pedalo> Pedalos { get; set; }
        public List<Customer> Customers { get; set; }
        public List<Passenger> Passengers { get; set; }

        public IActionResult OnGet(Guid id)
        {
            using var context = contextFactory.CreateReadOnlyContext();
            var bookingEntity = context.Bookings
                .Include(b => b.Pedalo)
                .Include(b => b.Customer)
                .Include(b => b.Passengers)
                .FirstOrDefault(b => b.BookingId == id);

            if (bookingEntity == null)
            {
                return NotFound();
            }

            Booking = new BookingEditModel
            {
                BookingId = bookingEntity.BookingId,
                PedaloName = bookingEntity.Pedalo.Name,
                Date = bookingEntity.StartDate.Date,
                StartTime = bookingEntity.StartDate.TimeOfDay,
                EndTime = bookingEntity.EndDate?.TimeOfDay,
                CustomerFullName = $"{bookingEntity.Customer.FirstName} {bookingEntity.Customer.LastName}",
                AvailablePassengerIds = context.Passengers.Select(p => p.PassengerId).ToList(),
                SelectedPassengerIds = bookingEntity.Passengers.Select(p => p.PassengerId).ToList()
            };

            Pedalos = context.Pedaloes.ToList();
            Customers = context.Customers.ToList();
            Passengers = context.Passengers.ToList();

            return Page();
        }


        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            using var context = contextFactory.CreateContext();

            var pedalo = context.Pedaloes.FirstOrDefault(p =>
                p.Name == Booking.PedaloName);

            if (pedalo == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid Pedalo selection.");
                return Page();
            }

            if (pedalo.AvailableCapacity <= 0)
            {
                ModelState.AddModelError(string.Empty, "The selected Pedalo is already at maximum capacity.");
                return Page();
            }

            var customer = context.Customers.FirstOrDefault(c =>
                c.FirstName == Booking.CustomerFirstName && c.LastName == Booking.CustomerLastName);

            if (customer == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid Customer selection.");
                return Page();
            }

            var selectedPassengers = Booking.SelectedPassengerIds
                .Select(id => context.Passengers.FirstOrDefault(p => p.PassengerId == id))
                .Where(p => p != null)
                .ToList();

            if (selectedPassengers.Count > pedalo.AvailableCapacity)
            {
                ModelState.AddModelError(string.Empty, "Cannot add more passengers than the Pedalo's available capacity.");
                return Page();
            }

            var passengerNames = selectedPassengers
                .Select(p => $"{p.PassengerFirstName} {p.PassengerLastName}")
                .ToList();
            if (passengerNames.Count == 0)
            {
                passengerNames.Add($"{customer.FirstName} {customer.LastName}");
            }
            var updatedBooking = context.Bookings.FirstOrDefault(b => b.BookingId == Booking.BookingId);

            if (updatedBooking == null)
            {
                return NotFound();
            }

            var startDateTime = Booking.Date.Date + Booking.StartTime;
            var endDateTime = Booking.Date.Date + Booking.EndTime.GetValueOrDefault();

            updatedBooking.PedaloId = pedalo.PedaloId;
            updatedBooking.CustomerId = customer.CustomerId;
            updatedBooking.StartDate = startDateTime;
            updatedBooking.EndDate = endDateTime;
            updatedBooking.PassengerNames = string.Join(",", passengerNames);

            context.SaveChanges();

            return RedirectToPage("./Index");
        }
    }

    public class BookingEditModel
    {
        public Guid BookingId { get; set; }
        public string PedaloName { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string CustomerFullName { get; set; }

        public string CustomerFirstName
        {
            get
            {
                return CustomerFullName?.Split(' ')[0];
            }
            set
            {
                // Set the first name based on the provided value
                string[] names = CustomerFullName?.Split(' ');
                if (names != null && names.Length > 0)
                {
                    names[0] = value;
                    CustomerFullName = string.Join(" ", names);
                }
            }
        }

        public string CustomerLastName
        {
            get
            {
                return CustomerFullName?.Split(' ')[1];
            }
            set
            {
                // Set the last name based on the provided value
                string[] names = CustomerFullName?.Split(' ');
                if (names != null && names.Length > 1)
                {
                    names[1] = value;
                    CustomerFullName = string.Join(" ", names);
                }
            }
        }

        public List<Guid> AvailablePassengerIds { get; set; } = new List<Guid>();
        public List<Guid> SelectedPassengerIds { get; set; } = new List<Guid>();
    }
}
