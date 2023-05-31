namespace PedaloWebApp.Pages.Customers
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
        public Customer Customer { get; set; }

        public IActionResult OnGet(Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            using var context = contextFactory.CreateReadOnlyContext();
            Customer = context.Customers.Find(id);

            if (Customer == null)
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
            Customer = context.Customers.Find(id);

            if (Customer == null)
            {
                return NotFound();
            }

            try
            {
                context.Customers.Remove(Customer);
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
