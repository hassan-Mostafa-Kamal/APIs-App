using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat2.core;
using Talabat2.core.Entities;
using Talabat2.core.Repositories;
using Talabat2.Repository.Data;

namespace Talabat2.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _context;
        //private Dictionary<string, GenericRepository<BaseEntity>> _repositors;
        private Hashtable _repositors;

        public UnitOfWork(StoreContext context)
        {
            _context = context;
            _repositors = new Hashtable();
        }

        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            var typeOfT= typeof(T).Name;
            if (!_repositors.ContainsKey(typeOfT))
            {
                var repository = new GenericRepository<T>(_context);
                _repositors.Add(typeOfT,repository);
            }
            return _repositors[typeOfT] as IGenericRepository<T>;
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public  async ValueTask DisposeAsync()
        {
           await   _context.DisposeAsync();
        }

       
    }
}
