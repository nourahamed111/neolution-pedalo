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

        public IActionResult OnGet()
        {
            using var context = contextFactory.CreateReadOnlyContext();
            Pedalos = context.Pedaloes.ToList();
            Customers = context.Customers.ToList();

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            using var context = contextFactory.CreateContext();

            // Find the selected Pedalo based on the Pedalo name
            var pedalo = context.Pedaloes.FirstOrDefault(p =>
                p.Name == Booking.PedaloName);

            if (pedalo == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid Pedalo selection.");
                return Page();
            }

            // Find the selected Customer based on the Customer name
            var customer = context.Customers.FirstOrDefault(c =>
                c.FirstName == Booking.CustomerFirstName && c.LastName == Booking.CustomerLastName);

            if (customer == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid Customer selection.");
                return Page();
            }

            // Create a new booking instance
            var newBooking = new Booking
            {
                BookingId = Guid.NewGuid(),
                PedaloId = pedalo.PedaloId,
                CustomerId = customer.CustomerId,
                StartDate = Booking.StartDate,
                EndDate = Booking.EndDate
            };

            // Save the new booking
            context.Bookings.Add(newBooking);
            context.SaveChanges();

            return RedirectToPage("./Index");
        }
    }

    public class BookingCreateModel
    {
        public string PedaloName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
    }
}
