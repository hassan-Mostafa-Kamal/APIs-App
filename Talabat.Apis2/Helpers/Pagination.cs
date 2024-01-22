using Talabat.Apis2.Dtos;

namespace Talabat.Apis2.Helpers
{
    public class Pagination<T>   // for stander Response to any EndPoint GetAll With pagination
    {
       

        public int PageSize { get; set; }
        public int PageIndex { get; set; }

        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }

   
     

        public Pagination(IReadOnlyList<T> data, int pageIndex, int pageSize ,int count)
        {
           
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
            Data = data;

        }
    }
}
