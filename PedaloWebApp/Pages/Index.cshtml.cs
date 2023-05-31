using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PedaloWebApp.Core.Domain.Entities;
using PedaloWebApp.Core.Interfaces.Data;

namespace PedaloWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IDbContextFactory dbContextFactory;

        public List<Customer> Customers { get; set; }
        public List<Pedalo> Pedaloes { get; set; }

        public IndexModel(IDbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public void OnGet()
        {
            using var context = dbContextFactory.CreateReadOnlyContext();
            Customers = context.Customers.ToList();
            Pedaloes = context.Pedaloes.Include(p => p.Bookings).ToList();
        }
    }
}
