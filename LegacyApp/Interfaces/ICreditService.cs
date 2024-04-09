using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyApp.Interfaces
{
    public interface ICreditService
    {
        int GetCreditLimit(string lastName, DateTime birthDate);
    }
}
