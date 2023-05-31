using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PedaloWebApp.Core.Domain.Entities;
using PedaloWebApp.Core.Interfaces.Data;

namespace PedaloWebApp.Pages.Pedaloes
{
    public class DeleteModel : PageModel
    {
        private readonly IDbContextFactory contextFactory;

        public DeleteModel(IDbContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        [BindProperty]
        public Pedalo Pedalo { get; set; }

        public IActionResult OnGet(Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            using var context = contextFactory.CreateReadOnlyContext();
            Pedalo = context.Pedaloes.Find(id);

            if (Pedalo == null)
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
            Pedalo = context.Pedaloes.Find(id);

            if (Pedalo == null)
            {
                return NotFound();
            }

            try
            {
                context.Pedaloes.Remove(Pedalo);
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
