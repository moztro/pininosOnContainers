using Containers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.Data.Context;

namespace MyFirstCoreWebApplication.Data
{
    public class DbInitializer
    {
        public static void Initialize()
        {
        }

        public static async Task Initialize(ApplicationContext context)
        {
            context.Database.EnsureCreated();            

            // Look for any users.
            if (context.Persons.Any())
            {
                return; // DB has been seeded
            }

            await CreateDefaultPerson(context);
        }

        private static async Task CreateDefaultPerson(ApplicationContext context)
        {
            var person = new Person
            {
                FirstName = "My first data",
                LastName = "My Lastname data",
                Age = 30
            };

           context.Persons.Add(person);
           await context.SaveChangesAsync();
        }
    }
}
