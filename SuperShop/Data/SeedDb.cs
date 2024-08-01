﻿using Microsoft.AspNetCore.Identity;
using SuperShop.Data.Entities;
using SuperShop.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;       
        private Random _random;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
            
            _random = new Random();
        }

        public IUserHelper UserManager { get; }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            var user = await _userHelper.GetUserByEmailAsync("diogosdl25@hotmail.com");

            if (user == null)
            {
                user = new User
                {
                    FirstName = "Diogo",
                    LastName = "Sanlez",
                    Email = "diogosdl25@hotmail.com",
                    UserName  = "diogosdl25@hotmail.com",
                    PhoneNumber = "1234567890",
                };

                var result = await _userHelper.AddUserAsync(user, "123456");

                if (result != IdentityResult.Success) 
                {
                    throw new InvalidOperationException("Couldn't create the user in seeder");                    
                }
            }

            if(!_context.Products.Any()) 
            {
                AddProduct("Iphone X", user);
                AddProduct("Iphone XV", user);
                AddProduct("Ipad Mini", user);
                AddProduct("SmartWatch", user);
                await _context.SaveChangesAsync();
            }

        }

        private void AddProduct(string name, User user)
        {
            _context.Products.Add(new Product
            {
                Name  = name,
                Price = _random.Next(1000),
                IsAvailable = true,
                Stock = _random.Next(100),
                User = user
            });
        }
    }
}
