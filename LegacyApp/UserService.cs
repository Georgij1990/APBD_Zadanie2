using LegacyApp.Interfaces;
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
            if (!_userValidator.ValidateUserName(firstName, lastName)) return false;

            // Logika biznesowa - walidacja
            if (!_userValidator.ValidateEmail(email)) return false;

            // Logika biznesowa
            if(!_userValidator.ValidateAge(dateOfBirth)) return false;

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
            if (!_userValidator.ValidateLimits(user)) return false;

            //Infrastruktura
            UserDataAccess.AddUser(user);
            return true;
        }
    }
}
