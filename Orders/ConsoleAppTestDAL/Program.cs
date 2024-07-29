using System;
using System.Data.Common;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL;
using Entities.Models;
using Microsoft.Extensions.Configuration;

namespace ConsoleAppTestDAL
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //await CreateAsync().ConfigureAwait(false);
            await RetreiveAsync().ConfigureAwait(false);
            

        }

        static async Task CreateAsync()
        {
            // Crear una instancia de un nuevo cliente
            Customer customer = new Customer
            {
                FirstName = "Vladimir",
                LastName = "Cortés",
                City = "Bogotá",
                Country = "Colombia",
                Phone = "3144427602"
            };

            using (var repository = RepositoryFactory.CreateRepository())
            {
                try
                {
                    var createdCustomer = await repository.CreateAsync(customer);
                    Console.WriteLine($"Added Customer: {createdCustomer.LastName} {createdCustomer.FirstName}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
        static async Task RetreiveAsync()
        {
            using (var repository = RepositoryFactory.CreateRepository()) 
            {
                try
                {
                    Expression<Func<Customer, bool>> criteria = c => c.FirstName == "Vladimir" && c.LastName == "Cortés";
                    var customer = await repository.RetreiveAsync(criteria);

                    if (customer != null) 
                    {
                            Console.WriteLine($"Retriveid Customer:{customer.FirstName} \t {customer.LastName} \t City: {customer.City} \t Country: {customer.Country}");
                    }
                    else
                    {
                        Console.WriteLine("Customer not exist");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }
}
