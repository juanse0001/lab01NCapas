using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Exceptions
{
    public class SupplierExceptions : Exception
    {
        // You can add more static methods here to throw other supplier-related exceptions

        public SupplierExceptions()
        {
            throw new SupplierExceptions($"No Supplier found in the database.");
        }

        private SupplierExceptions(string message)
            : base(message)
        {
            throw new Exception(message);
        }

        public static void ThrowSupplierAlreadyExistsException(string ContactTitle, string ContactName)
        {
            throw new SupplierExceptions($"A Suppleir with the name already exists: {ContactTitle} {ContactName}.");
        }

        public static void ThrowInvalidCustomerIdException(int id)
        {
            throw new SupplierExceptions($"Invalid Supplier ID: {id}");
        }
    }
}
