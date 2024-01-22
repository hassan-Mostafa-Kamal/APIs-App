using System.Linq.Expressions;
using Talabat2.core.Entities;

namespace Talabat.Apis2.Specifications
{
    public class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get ; set ; }

        public List<Expression<Func<T, object>>> Includs { get ; set ; }
        

        public BaseSpecification()
        {
            Includs = new List<Expression<Func<T, object>>> ();
        }

        public BaseSpecification(Expression<Func<T, bool>> criteriaExpression)
        {

            Includs = new List<Expression<Func<T, object>>>();
            Criteria = criteriaExpression;

        }

        //==============================================

        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDescending { get; set; }
      
        public void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        public void AddOrerByDesc(Expression<Func<T, object>> orderByDescExpression)
        {
            OrderByDescending = orderByDescExpression;
        }


        //=============================================================

        public int Take { get ; set; }
        public int Skip { get; set; }
        public bool IsPaginationEnabled { get ; set ; }

        public void ApplyPagination(int skip,int take)
        {
            IsPaginationEnabled = true;
         
            Skip = skip;
            Take = take;
        }

    }
}
