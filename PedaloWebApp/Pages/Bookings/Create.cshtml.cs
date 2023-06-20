using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PedaloWebApp.Core.Domain.Entities;
using PedaloWebApp.Core.Interfaces.Data;

namespace PedaloWebApp.Pages.Bookings
{
    public class CreateModel : PageModel
    {
        private readonly IDbContextFactory contextFactory;

        public CreateModel(IDbContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        [BindProperty]
        public BookingCreateModel Booking { get; set; }
        public List<Pedalo> Pedalos { get; set; }
        public List<Customer> Customers { get; set; }
        public List<Passenger> Passengers { get; set; }

        public IActionResult OnGet()
        {
            using var context = contextFactory.CreateReadOnlyContext();
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

            var passengerNames = selectedPassengers
                .Select(p => $"{p.PassengerFirstName} {p.PassengerLastName}")
                .ToList();

            if (!passengerNames.Contains(Booking.CustomerFullName))
            {
                passengerNames.Add(Booking.CustomerFullName);
            }

            var startDateTime = Booking.Date.Date + Booking.StartTime;
            var endDateTime = Booking.Date.Date + Booking.EndTime;

            var newBooking = new Booking
            {
                BookingId = Guid.NewGuid(),
                PedaloId = pedalo.PedaloId,
                CustomerId = customer.CustomerId,
                StartDate = startDateTime,
                EndDate = endDateTime,
                PassengerNames = string.Join(",", passengerNames)
            };

            context.Bookings.Add(newBooking);
            context.SaveChanges();

            return RedirectToPage("./Index");
        }



    }

    public class BookingCreateModel
    {
        public string PedaloName { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string CustomerFullName { get; set; }
        public string CustomerFirstName
        {
            get
            {
                return CustomerFullName?.Split(' ')[0];
            }
        }

        public string CustomerLastName
        {
            get
            {
                return CustomerFullName?.Split(' ')[1];
            }
        }

        public List<Guid> SelectedPassengerIds { get; set; } = new List<Guid>();
    }


}
