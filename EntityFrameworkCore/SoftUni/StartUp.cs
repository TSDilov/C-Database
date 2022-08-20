using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Z.EntityFramework.Plus;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var dbContex = new SoftUniContext();
            var townSeattle = dbContex.Towns
                .FirstOrDefault(t => t.Name == "Seattle");

            var addressesInSeattle = dbContex.Addresses
                .Include(a => a.Town)
                .Where(a => a.Town.Name == "Seattle")
                .ToList();

            foreach (var address in addressesInSeattle)
            {
                address.TownId = null;
            }

            var employeesWithAddressInSeattle = dbContex.Employees
                .Include(e => e.Address)
                .Where(e => e.Address.Town.Name == "Seattle")
                .ToList();

            foreach (var employee in employeesWithAddressInSeattle)
            {
                employee.AddressId = null;
            }

            dbContex.Towns.Remove(townSeattle);
            var countOfAddresses = addressesInSeattle.Count();
            dbContex.SaveChanges();

            Console.WriteLine($"{countOfAddresses} addresses in Seattle were deleted");
        }
    }
}
