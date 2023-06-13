namespace PedaloWebApp.Pages.Passengers
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using PedaloWebApp.Core.Domain.Entities;
    using PedaloWebApp.Core.Interfaces.Data;

    public class DeleteModel : PageModel
    {
        private readonly IDbContextFactory contextFactory;

        public DeleteModel(IDbContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        [BindProperty]
        public Passenger Passenger { get; set; }

        public IActionResult OnGet(Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            using var context = contextFactory.CreateReadOnlyContext();
            Passenger = context.Passengers.Find(id);

            if (Passenger == null)
            {
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnPost(Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            using var context = contextFactory.CreateContext();
            Passenger = context.Passengers.Find(id);

            if (Passenger == null)
            {
                return NotFound();
            }

            try
            {
                context.Passengers.Remove(Passenger);
                context.SaveChanges();
            }
            catch (Exception)
            {
                return RedirectToPage("/Error");
            }

            return RedirectToPage("./Index");
        }
    }
}
