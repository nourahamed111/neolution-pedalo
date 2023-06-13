using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PedaloWebApp.Core.Domain.Entities;
using PedaloWebApp.Core.Interfaces.Data;

namespace PedaloWebApp.Pages.Passengers
{
    public class CreateModel : PageModel
    {
        private readonly IDbContextFactory contextFactory;

        public CreateModel(IDbContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        [BindProperty]
        public Passenger Passenger { get; set; }

        public IActionResult OnGet()
        {
            return this.Page();
        }

        public IActionResult OnPost()
        {
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            using var context = this.contextFactory.CreateContext();
            this.Passenger.PassengerId = Guid.NewGuid();

            try
            {
                context.Passengers.Add(this.Passenger);
                context.SaveChanges();
            }
            catch (Exception)
            {
                return this.RedirectToPage("/Error");
            }

            return this.RedirectToPage("./Index");
        }
    }
}
