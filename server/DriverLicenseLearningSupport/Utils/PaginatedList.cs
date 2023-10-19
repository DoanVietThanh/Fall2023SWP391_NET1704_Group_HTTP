using System.Collections;

namespace DriverLicenseLearningSupport.Utils
{
    public class PaginatedList<T> : List<T>
    {
        public int TotalPage { get; set; }
        public int PageIndex { get; set; }

        public PaginatedList(List<T> items, int totalPage, int pageIndex)
        {
            // generate total page by total items
            TotalPage = totalPage;
            // set page index
            PageIndex = pageIndex;
            // add paging items
            AddRange(items);
        }

        public static PaginatedList<T> CreateByIEnumerable(IEnumerable<T> source, int pageIndex ,int pageSize) 
        {
            var totalPage = (int)Math.Ceiling(source.Count() / (double)pageSize);

            // invalid pageIndex
            if (pageIndex > totalPage) pageIndex = 1;

            // get items by pageIndex, pageSize
            var sourceItems = source.Skip((pageIndex - 1) * pageSize) // offset
                                    .Take(pageSize) // get page size items
                                    .ToList();

            // paging
            return new PaginatedList<T>(sourceItems, totalPage, pageIndex);
        }
    }
}
