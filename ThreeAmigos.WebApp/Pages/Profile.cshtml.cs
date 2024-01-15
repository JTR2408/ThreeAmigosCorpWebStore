using System.Security.Claims;
using Auth0.AuthenticationApi.Models;
using Auth0.ManagementApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ThreeAmigos.WebApp.Models;
using ThreeAmigos.WebApp.Services;

namespace ThreeAmigos.WebApp.Pages;


    public class ProfileModel : PageModel
    {

        public UserProfileViewModel UserProfile { get; set; }
        private readonly IUserService _userService;

        public ProfileModel(IUserService userService){
            _userService = userService;
        }

        public async Task OnGetAsync()
        {

        var userData = await _userService.GetUserDataAsync(User.Identity.Name);
        dynamic userInfo = JsonConvert.DeserializeObject<List<dynamic>>(userData)[0];

        string userName = userInfo.nickname;
        string profilePicture = userInfo.picture;

        string billingAddress = userInfo?.user_metadata?.billing_address;
        string phoneNumber = userInfo?.user_metadata?.number;


            Random r = new Random();
            int fundRange = 100;
            double rFund = r.NextDouble()* fundRange;
            double roundFund = (double)Math.Round(rFund,2);

            UserProfile = new UserProfileViewModel()
            {
                Name = userName,
                EmailAddress = User.Identity.Name,
                ProfileImage = profilePicture,
                billingAddress = billingAddress,
                phoneNumber = phoneNumber,
                Funds = roundFund
            };
        }

    }
