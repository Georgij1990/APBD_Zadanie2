﻿using LegacyApp.Interfaces;
using System;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using LegacyApp.Validators;

namespace LegacyApp
{
    public class UserService
    {
        private IClientRepository _clientRepository;
        private ICreditService _creditService;
        private UserValidator _userValidator;

        public UserService()
        {
            _clientRepository = new ClientRepository();
            _creditService = new UserCreditService();
            _userValidator = new UserValidator();
        }

        public UserService(IClientRepository clientRepository, ICreditService creditService, UserValidator userValidator)
        {
            _clientRepository = clientRepository;
            _creditService = creditService;
            _userValidator = userValidator;
        }

        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            // Logika biznesowa - walidacja
            _userValidator.ValidateUserName(firstName, lastName);

            // Logika biznesowa - walidacja
            _userValidator.ValidateEmail(email);

            // Logika biznesowa
            _userValidator.ValidateAge(dateOfBirth);

            // Infrastruktura
            var client = _clientRepository.GetById(clientId);

            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };

            // Logika biznesowa + Infrastruktura
            _userValidator.CheckType(user, client, _creditService);

            // Logika biznesowa
            _userValidator.ValidateLimits(user);

            //Infrastruktura
            UserDataAccess.AddUser(user);
            return true;
        }
    }
}
