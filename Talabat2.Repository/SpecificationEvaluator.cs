using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Apis2.Specifications;
using Talabat2.core.Entities;

namespace Talabat2.Repository
{
    public static class SpecificationEvaluator<T> where T : BaseEntity
    {

        public static  IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
        {
            var query = inputQuery;
               // If we have Criteria
            if (spec.Criteria != null)
            {
                query = inputQuery.Where(spec.Criteria);
            }



            // If we have Sorting
            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }

            if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }


            // If we have Pagination
            if (spec.IsPaginationEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            
            }



            // If we have Includs
            query = spec.Includs.Aggregate(query,(currentQuery,includeExpression)=> currentQuery.Include(includeExpression));


            return query;

        }

    }
}
