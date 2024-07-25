using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace DAL
{
    public interface IRepository
    {
        Task<TEntity> CreateAsync<TEntity>(TEntity toCreate) where TEntity : class;

        Task<bool> deleteAsync<TEntity>(TEntity toDelete) where TEntity : class;

        Task<bool> updateAsync<TEntity>(TEntity todelete) where TEntity : class;

        Task<TEntity> retreiveAsync<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class;

        Task<List<TEntity>> FirlterAsync<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class;
    }
}
