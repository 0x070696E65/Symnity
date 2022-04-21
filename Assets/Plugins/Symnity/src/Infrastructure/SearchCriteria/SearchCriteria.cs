using JetBrains.Annotations;

namespace Symnity.Infrastructure.SearchCriteria
{
    public class SearchCriteria
    {
        /**
        * Sort responses in ascending or descending order based on the collection property set on the
        * param ''order''. If the request does not specify ''order'', REST returns the collection
        * ordered by id. (optional)
        */
        public Order Order;

        /**
        * Number of entities to return for each request. (optional)
        */
        public int PageSize;

        /**
         * Filter by page number. (optional)
         */
        public int PageNumber;

        /**
         * Entry id at which to start pagination. If the ordering parameter is set to -id, the elements
         * returned precede the identifier. Otherwise, newer elements with respect to the id are
         * returned. (optional)
         */
        [CanBeNull] public string Offset;

        public SearchCriteria(Order order = Order.Asc, int pageSize = 20, int pageNumber = 1, string offset = null)
        {
            Order = order;
            PageSize = pageSize;
            PageNumber = pageNumber;
            Offset = offset;
        }
    }
}