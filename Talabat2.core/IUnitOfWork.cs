using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat2.core.Entities;
using Talabat2.core.Repositories;

namespace Talabat2.core
{
    public  interface IUnitOfWork : IAsyncDisposable  
    {

        IGenericRepository<T> Repository<T>() where T : BaseEntity;

        //this save changes and get the number of rows was effected at DB
        Task<int> Complete();

    }
}
