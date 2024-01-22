using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Apis2.Specifications;
using Talabat2.core.Entities;
using Talabat2.core.Repositories;
using Talabat2.Repository.Data;

namespace Talabat2.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;

        public GenericRepository(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
  
             return await _dbContext.Set<T>().ToListAsync();
            
        }

       

        public async Task<T?> GetByIdAsync(int id)
        {
            // return await  _context.Set<T>().Where(t=> t.Id == id).FirstOrDefaultAsync();

            if (typeof(T) == typeof(Product))                                       //مما تتكون الكوري
            {                                                               
                return await _dbContext.Prodects.                               // (1):- input Query(base or DbSet) 

                                                                               //(2) the specification (مكونات او مواصفات) This Query   
                    Where(p => p.Id == id)                                       //1: - where Condition
                    .Include(b => b.ProdectBrand) .Include(t => t.ProductType)  //2:- tow Includs
                    .FirstOrDefaultAsync() as T;                                //3....
            }                                                                   //4...
            return await _dbContext.Set<T>().FindAsync(id);
        }


        //========================== 


        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {
            return await  SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(),spec).ToListAsync();
        }



        public async Task<T> GetEntityWithSpecAsync(ISpecification<T> spec)
        {
            return await SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec).FirstOrDefaultAsync();
        }

        public async Task<int> GetCountWithSpacAsync(ISpecification<T> spec)
        {
            return await SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(),spec).CountAsync();
        }

        public async Task Add(T entity)
        {
             await _dbContext.Set<T>().AddAsync(entity);
        }

        public  void Update(T entity)
        {
             _dbContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }
    }
}
