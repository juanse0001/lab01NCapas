using BLL.Exceptions;
using DAL;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Suppliers
    {
        public async Task<Supplier> CreateAsync(Supplier supplier)
        {
            Supplier supplierResult = null;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                //Buscar si el nombre del provedor existe
                Supplier supplierSearch = await repository.RetreiveAsync<Supplier>(s => s.ContactName == supplier.ContactName);
                if (supplierSearch == null)
                {
                    //No existe podemos crearlo
                    supplierResult = await repository.CreateAsync(supplier);
                }
                else
                {
                    //Podriamos lanzar una Excepcion 
                    //Para notificar que el cliente ya existe.
                    //Podriamos Crear incluso una cap de exepciones
                    //Perzonalizada y consumirlas desde otras capas
                    SupplierExceptions.ThrowSupplierAlreadyExistsException(supplierSearch.ContactTitle, supplierSearch.ContactName);
                }
                return supplierResult!;
            }
        }

        public async Task<Supplier> RetrieveByIDAsync(int id)
        {
            Supplier result = null;

            using (var repository = RepositoryFactory.CreateRepository())
            {
                Supplier supplier = await repository.RetreiveAsync<Supplier>(s => s.Id == id);

                // Check if customer was found
                if (supplier == null)
                {
                    // Throw a CustomerNotFoundException (assuming you have this class)
                    SupplierExceptions.ThrowInvalidCustomerIdException(id);
                }
                return supplier!;
            }
        }
        public async Task<List<Supplier>> RetreiveAllAsync()
        {
            List<Supplier> Result = null;

            using (var r = RepositoryFactory.CreateRepository())
            {
                // Define el criterio de filtro para obtener todos los clientes.
                Expression<Func<Supplier, bool>> allSupplierCriteria = x => true;
                Result = await r.FilterAsync<Supplier>(allSupplierCriteria);
            }

            return Result;
        }
        public async Task<bool> UpdateAsync(Supplier supplier)
        {
            bool Result = false;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                // Validar que el nombre del cliente no exista
                Supplier supplierSearch = await repository.RetreiveAsync<Supplier>
                    (s => s.ContactName == supplier.ContactName && s.Id != supplier.Id);
                if (supplierSearch == null)
                {
                    // No existe
                    Result = await repository.UpdateAsync(supplier);
                }
                else
                {
                    // Podemos implementar alguna lógica para
                    // indicar que no se pudo modificar
                    SupplierExceptions.ThrowSupplierAlreadyExistsException(supplierSearch.ContactName, supplierSearch.ContactTitle);
                }
            }
            return Result;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            bool Result = false;
            // Buscar un cliente para ver si tiene Orders (Ordenes de Compra)
            var customer = await RetrieveByIDAsync(id);
            if (customer != null)
            {
                // Eliminar el cliente
                using (var repository = RepositoryFactory.CreateRepository())
                {
                    Result = await repository.DeleteAsync(customer);
                }
            }
            else
            {
                // Podemos implementar alguna lógica
                // para indicar que el producto no existe
                CustomerExceptions.ThrowInvalidCustomerIdException(id);
            }
            return Result;
        }
    }
}

