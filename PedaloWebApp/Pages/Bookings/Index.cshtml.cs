using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PedaloWebApp.Core.Domain.Entities;
using PedaloWebApp.Core.Interfaces.Data;

namespace PedaloWebApp.Pages.Bookings
{
    public class IndexModel : PageModel
    {
        private readonly IDbContextFactory contextFactory;

        public IndexModel(IDbContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public List<Booking> Bookings { get; set; }

        public IActionResult OnGet()
        {
            using var context = contextFactory.CreateReadOnlyContext();
            Bookings = context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Pedalo)
                .ToList();

            return Page();
        }
    }
}
