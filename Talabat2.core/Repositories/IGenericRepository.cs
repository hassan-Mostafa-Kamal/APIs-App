using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Apis2.Specifications;
using Talabat2.core.Entities;

namespace Talabat2.core.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> GetAllAsync();

        Task<T> GetByIdAsync(int id);

        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec);

        Task<T> GetEntityWithSpecAsync(ISpecification<T> spec);

        Task<int> GetCountWithSpacAsync(ISpecification<T> spec); //this mathod will have anouther specification class


        Task Add(T entity);

        void Update(T entity);
        void Delete(T entity);


    }
}
