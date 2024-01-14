using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ThreeAmigos.WebApp.Models;

namespace ThreeAmigos.WebApp.Pages;


    public class ProfileModel : PageModel
    {
        public UserProfileViewModel UserProfile { get; set; }

        public void OnGet()
        {
            Random r = new Random();
            int fundRange = 100;
            double rFund = r.NextDouble()* fundRange;

            UserProfile = new UserProfileViewModel()
            {
                Name = User.Identity.Name,
                EmailAddress = User.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                ProfileImage = User.Claims
                    .FirstOrDefault(c => c.Type == "picture")?.Value,
                Funds = rFund
            };

            

        }


    }
