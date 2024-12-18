﻿using AuthXml.DTO;

namespace AuthXml.Service
    {
    public interface IAuthService
        {
        Task<string> LoginAsync(UserDto userDto);
        Task<(string Token, UserDetailsDto UserDetails)> LoginWithJwtAsync(UserDto userDto);
        }
    }