namespace PedaloWebApp.Pages.Passengers
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using PedaloWebApp.Core.Interfaces.Data;

    public class EditModel : PageModel
    {
        private readonly IDbContextFactory contextFactory;

        public EditModel(IDbContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        [BindProperty]
        public PassengerEditModel Passenger { get; set; }

        public IActionResult OnGet(Guid? id)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            using var context = this.contextFactory.CreateReadOnlyContext();
            this.Passenger = context.Passengers
                .Where(m => m.PassengerId == id)
                .Select(x => new PassengerEditModel
                {
                    PassengerId = x.PassengerId,
                    PassengerFirstName = x.PassengerFirstName,
                    PassengerLastName = x.PassengerLastName,
                 
                })
                .FirstOrDefault();

            if (this.Passenger == null)
            {
                return this.NotFound();
            }

            return this.Page();
        }

        public IActionResult OnPost()
        {
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            using var context = this.contextFactory.CreateContext();
            var Passenger = context.Passengers.FirstOrDefault(x => x.PassengerId == this.Passenger.PassengerId);
            if (Passenger == null)
            {
                return this.NotFound();
            }

            try
            {
                Passenger.PassengerFirstName = this.Passenger.PassengerFirstName;
                Passenger.PassengerLastName = this.Passenger.PassengerLastName;

                context.SaveChanges();
            }
            catch (Exception)
            {
                return this.RedirectToPage("/Error");
            }

            return this.RedirectToPage("./Index");
        }
    }

    public class PassengerEditModel
    {
        public Guid PassengerId { get; set; }
        public string PassengerFirstName { get; set; }
        public string PassengerLastName { get; set; }
    }
}
