﻿namespace AkijCoreAPI.Services.PasswordHashers
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
    }
}