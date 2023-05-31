using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PedaloWebApp.Core.Domain.Entities;
using PedaloWebApp.Core.Interfaces.Data;
using System.Linq;
using System;

namespace PedaloWebApp.Pages.Bookings
{
    public class DeleteModel : PageModel
    {
        private readonly IDbContextFactory contextFactory;

        public DeleteModel(IDbContextFactory contextFactory) =>
            this.contextFactory = contextFactory;

        [BindProperty]
        public Booking Booking { get; set; }

        public IActionResult OnGet(Guid id) // Update parameter type to Guid
        {
            using var context = this.contextFactory.CreateReadOnlyContext();
            this.Booking = context.Bookings
                .Include(x => x.Customer)
                .FirstOrDefault(x => x.BookingId == id);

            if (this.Booking == null)
            {
                return this.NotFound();
            }

            return this.Page();
        }

        public IActionResult OnPost()
        {
            using var context = this.contextFactory.CreateContext();
            var booking = context.Bookings.Find(this.Booking.BookingId);

            if (booking != null)
            {
                context.Bookings.Remove(booking);
                context.SaveChanges();
            }

            return this.RedirectToPage("./Index");
        }
    }
}
