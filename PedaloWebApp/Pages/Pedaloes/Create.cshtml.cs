using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PedaloWebApp.Core.Domain.Entities;
using PedaloWebApp.Core.Interfaces.Data;

namespace PedaloWebApp.Pages.Pedalos
{
    public class CreateModel : PageModel
    {
        private readonly IDbContextFactory contextFactory;

        public CreateModel(IDbContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        [BindProperty]
        public Pedalo Pedalo { get; set; }

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
            this.Pedalo.PedaloId = Guid.NewGuid();

            try
            {
                context.Pedaloes.Add(this.Pedalo);
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
