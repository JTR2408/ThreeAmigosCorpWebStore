﻿using System.Security.Claims;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ThreeAmigos.WebApp.Models;
using ThreeAmigos.WebApp.Services;

namespace ThreeAmigos.WebApp.Controller;

public class AccountController : ControllerBase{

    private readonly IUserService _userService;

    public AccountController(IUserService userService){
        _userService = userService;
    }

    public async Task Login(string returnUrl = "/"){
        var authenticationProperties = new
            LoginAuthenticationPropertiesBuilder()
                .WithRedirectUri(returnUrl)
                .Build();

        await HttpContext.ChallengeAsync(
            Auth0Constants.AuthenticationScheme, authenticationProperties);
    }

    [Authorize]
    public async Task Logout(){
        var authenticationProperties = new
            LogoutAuthenticationPropertiesBuilder()
                .WithRedirectUri("/Index")
                .Build();

        await HttpContext.SignOutAsync(
            Auth0Constants.AuthenticationScheme, authenticationProperties);

        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);
    }

    [Authorize]
    public async Task<IActionResult> UpdateProfile(string Name, string BillingAddress, string PhoneNumber)
    {
        var userEmail = User.Identity.Name;
        if (userEmail == null){
            return BadRequest("Email is null");
        }

        var userData = await _userService.GetUserDataAsync(userEmail);
        dynamic userObject = JsonConvert.DeserializeObject<List<dynamic>>(userData)[0];

        if(BillingAddress == null){
            try{
                BillingAddress = userObject.user_metadata.billing_address;
            }
            catch(Exception e){
                BillingAddress = null;
            }
        }
        if(PhoneNumber == null){
            try{
                PhoneNumber = userObject.user_metadata.contact_number;
            }
            catch (Exception e){
                PhoneNumber = null;
            }
        }
        if(Name == null){
            Name = userObject.nickname;
        } 

        // Access the 'user_id' property
        string userId = userObject.user_id;
        string nickname = Name;
        string billingAddress = BillingAddress;
        string phoneNumber = PhoneNumber;

        await _userService.UpdateUserDetails(userId, nickname, billingAddress, phoneNumber);

        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    public async Task<IActionResult> DeleteProfile(){
        var userEmail = User.Identity.Name;
        if (userEmail == null){
            return BadRequest("Email is null");
        }

        var userData = await _userService.GetUserDataAsync(userEmail);
        dynamic userObject = JsonConvert.DeserializeObject<List<dynamic>>(userData)[0];
        string userId = userObject.user_id;
        _userService.DeleteUserProfile(userId);
        await Logout();

        return RedirectToAction("Index", "Home");
    }

}