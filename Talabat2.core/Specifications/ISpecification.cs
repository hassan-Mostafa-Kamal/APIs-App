using System.Linq.Expressions;
using Talabat2.core.Entities;

namespace Talabat.Apis2.Specifications
{
    public interface ISpecification<T> where T : BaseEntity
    {

        public Expression<Func<T,bool>> Criteria { get; set; }

        public List<Expression<Func<T,object>>> Includs { get; set; }

        public Expression<Func<T,object>> OrderBy { get; set; }

        public Expression<Func<T,object>> OrderByDescending { get; set; }


        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPaginationEnabled { get; set; }


    }
}
