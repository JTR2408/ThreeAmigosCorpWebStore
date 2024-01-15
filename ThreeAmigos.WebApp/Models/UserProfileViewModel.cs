using System;
using System.Security.Cryptography.X509Certificates;

namespace ThreeAmigos.WebApp.Models;

public class UserProfileViewModel
{
    public string EmailAddress { get; set; }

    public string Name { get; set; }

    public string ProfileImage { get; set; }

    public double Funds {get; set;}

    public string phoneNumber {get; set;}

    public string billingAddress {get;set;}

}