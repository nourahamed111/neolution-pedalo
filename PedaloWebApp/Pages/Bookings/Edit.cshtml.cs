using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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

        public IActionResult OnGet(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return BadRequest();
            }

            using var context = contextFactory.CreateReadOnlyContext();
            Booking = context.Bookings
                .Include(b => b.Customer) // Include the Customer navigation property
                .Where(b => b.BookingId == id)
                .Select(b => new BookingEditModel
                {
                    BookingId = b.BookingId,
                    CustomerId = b.CustomerId,
                    PedaloId = b.PedaloId,
                    StartDate = b.StartDate,
                    EndDate = b.EndDate,
                    CustomerFirstName = b.Customer.FirstName,
                    CustomerLastName = b.Customer.LastName
                })
                .FirstOrDefault();

            if (Booking == null)
            {
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            using var context = contextFactory.CreateContext();
            var bookingToUpdate = context.Bookings
                .FirstOrDefault(b => b.BookingId == Booking.BookingId);

            if (bookingToUpdate == null)
            {
                return NotFound();
            }

            try
            {
                // Update the properties of the booking
                bookingToUpdate.StartDate = Booking.StartDate;
                bookingToUpdate.EndDate = Booking.EndDate;

                // Find the customer based on the original ID
                var customer = context.Customers.FirstOrDefault(c =>
                    c.CustomerId == bookingToUpdate.CustomerId);

                if (customer == null)
                {
                    // Customer not found, handle the error accordingly
                    ModelState.AddModelError(string.Empty, "Unable to find a customer with the provided ID.");
                    return Page();
                }

                // Update the customer name
                customer.FirstName = Booking.CustomerFirstName;
                customer.LastName = Booking.CustomerLastName;

                context.SaveChanges();
            }
            catch (Exception)
            {
                return RedirectToPage("/Error");
            }

            return RedirectToPage("./Index");
        }
    }

    public class BookingEditModel
    {
        public Guid BookingId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid PedaloId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
    }
}
