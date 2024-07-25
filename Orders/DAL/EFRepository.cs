using DAL.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;


namespace DAL
{
    public interface EFRepository : IRepository
        {
        ApplicationDbContext _context;

        public EFRepository(ApplicationDbContext context)
        {
            this._context = context;

        }

        private bool disposedValue;

        public async Task<TEntity> IRepository.CreateAsync<TEntity>(TEntity toCreate)
        {
            TEntity result = default(TEntity);
            try
            {
                await _context.Set<TEntity>().AddAsync(toCreate);
                await _context.SaveChangesAsync();
                result = toCreate;
            }
            catch (Exception)
            {
                throw;

            }
        }
        public Task<bool> deleteAsync<TEntity>(TEntity toDelete) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<List<TEntity>> FirlterAsync<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> retreiveAsync<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public Task<bool> updateAsync<TEntity>(TEntity todelete) where TEntity : class
        {
            throw new NotImplementedException();
        }
    }
}
