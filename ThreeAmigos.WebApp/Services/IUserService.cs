using System;

namespace ThreeAmigos.WebApp.Services;

public interface IUserService{
    Task<String> GetUserDataAsync(string userEmail);

    Task UpdateUserDetails(string userId, string userName, string billingAddress, string phoneNumber);

    Task DeleteUserProfile(string userId);
}