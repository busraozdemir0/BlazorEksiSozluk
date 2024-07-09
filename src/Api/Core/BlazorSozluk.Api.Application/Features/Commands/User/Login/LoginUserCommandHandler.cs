﻿using AutoMapper;
using BlazorSozluk.Api.Application.Interfaces.Repositories;
using BlazorSozluk.Common.Infrastructure;
using BlazorSozluk.Common.Infrastructure.Exceptions;
using BlazorSozluk.Common.Models.Queries;
using BlazorSozluk.Common.Models.RequestModels;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Api.Application.Features.Commands.User.Login
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserViewModel>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public LoginUserCommandHandler(IUserRepository userRepository, IMapper mapper, IConfiguration configuration)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        public async Task<LoginUserViewModel> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var dbUser = await userRepository.GetSingleAsync(i => i.EmailAddress == request.EmailAddress);

            // db'de user gercekten var mi bos mu diye bakiliyor
            if (dbUser == null)
                throw new DatabaseValidationException("Kullanıcı bulunamadı!"); // Kendi yazdigimiz custom exception

            // Sifre kontrolu
            var pass = PasswordEncryptor.Encrpt(request.Password); // Disardan gelen sifreyi hashliyoruz ve db'deki sifre ile ayni olup olmadigini kontrol ediyoruz.
            if (dbUser.Password != pass)
                throw new DatabaseValidationException("Yanlış şifre!");

            // Email onaylanmamissa yine hata firlatilacak
            if (!dbUser.EmailConfirmed)
                throw new DatabaseValidationException("Bu Email adresi henüz onaylanmamış!");

            var result = mapper.Map<LoginUserViewModel>(dbUser);

            // Bu claim'ler JWT icerisinde yer alacak
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, dbUser.Id.ToString()),
                new Claim(ClaimTypes.Email, dbUser.EmailAddress),
                new Claim(ClaimTypes.Name, dbUser.UserName),
                new Claim(ClaimTypes.GivenName, dbUser.FirstName),
                new Claim(ClaimTypes.Surname, dbUser.LastName),
            };

            result.Token = GenerateToken(claims);

            return result;
        }

        private string GenerateToken(Claim[] claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AuthConfig:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddDays(10);

            var token = new JwtSecurityToken(claims: claims,
                                            expires: expiry,
                                            signingCredentials: creds,
                                            notBefore: DateTime.Now);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
