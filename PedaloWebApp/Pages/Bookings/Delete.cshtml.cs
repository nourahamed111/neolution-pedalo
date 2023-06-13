using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PedaloWebApp.Core.Domain.Entities;
using PedaloWebApp.Core.Interfaces.Data;

namespace PedaloWebApp.Pages.Bookings
{
    public class DeleteModel : PageModel
    {
        private readonly IDbContextFactory contextFactory;

        public DeleteModel(IDbContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        [BindProperty]
        public Booking Booking { get; set; }

        public IActionResult OnGet(Guid id)
        {
            using var context = this.contextFactory.CreateReadOnlyContext();
            this.Booking = context.Bookings
                .Include(x => x.Customer)
                .Include(x => x.Pedalo)
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
            this.Booking = context.Bookings.Find(this.Booking.BookingId);

            if (this.Booking == null)
            {
                return this.NotFound();
            }

            context.Bookings.Remove(this.Booking);
            context.SaveChanges();

            return this.RedirectToPage("./Index");
        }
    }
}
