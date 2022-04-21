using System.Collections.Generic;

namespace Symnity.Infrastructure
{
    /**
     * It represents a page of results after a repository search call.
     *
     * @param <T> then model type.
     */
    public class Page<T>
    {
        public readonly List<T> Data;
        public readonly int PageNumber;
        public readonly int PageSize;

        /**
         * Constructor.
         *
         * @param data the page data
         * @param pageNumber the current page number starting from 1.
         * @param pageSize the page size.
         */
        public Page(List<T> data, int pageNumber, int pageSize)
        {
            Data = data;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}