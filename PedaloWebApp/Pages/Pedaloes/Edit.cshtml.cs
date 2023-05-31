using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PedaloWebApp.Core.Domain.Entities;
using PedaloWebApp.Core.Interfaces.Data;

namespace PedaloWebApp.Pages.Pedalos
{
    public class EditModel : PageModel
    {
        private readonly IDbContextFactory contextFactory;

        public EditModel(IDbContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        [BindProperty]
        public PedaloEditModel Pedalo { get; set; }

        public IActionResult OnGet(Guid? id)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            using var context = this.contextFactory.CreateReadOnlyContext();
            var pedalo = context.Pedaloes.FirstOrDefault(p => p.PedaloId == id);

            if (pedalo == null)
            {
                return this.NotFound();
            }

            this.Pedalo = new PedaloEditModel
            {
                PedaloId = pedalo.PedaloId,
                Name = pedalo.Name,
                Color = pedalo.Color,
                Capacity = pedalo.Capacity,
                HourlyRate = pedalo.HourlyRate
            };

            return this.Page();
        }

        public IActionResult OnPost()
        {
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            using var context = this.contextFactory.CreateContext();
            var pedalo = context.Pedaloes.FirstOrDefault(p => p.PedaloId == this.Pedalo.PedaloId);
            if (pedalo == null)
            {
                return this.NotFound();
            }

            try
            {
                pedalo.Name = this.Pedalo.Name;
                pedalo.Color = this.Pedalo.Color;
                pedalo.Capacity = this.Pedalo.Capacity;
                pedalo.HourlyRate = this.Pedalo.HourlyRate;

                context.SaveChanges();
            }
            catch (Exception)
            {
                return this.RedirectToPage("/Error");
            }

            return this.RedirectToPage("./Index");
        }
    }

    public class PedaloEditModel
    {
        public Guid PedaloId { get; set; }
        public string Name { get; set; }
        public PedaloColor Color { get; set; }
        public int Capacity { get; set; }
        public decimal HourlyRate { get; set; }
    }
}

