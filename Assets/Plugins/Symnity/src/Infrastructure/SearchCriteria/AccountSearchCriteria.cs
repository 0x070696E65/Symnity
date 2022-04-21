#nullable enable
using System;
using System.Collections.Generic;
using System.Numerics;
using Symnity.Model.Accounts;
using Symnity.Model.Mosaics;
using Symnity.Model.Transactions;

namespace Symnity.Infrastructure.SearchCriteria
{
    /**
     * Defines the params used to search blocks. With this criteria, you can sort and filter
     * block queries using rest.
     */
    public class AccountSearchCriteria : SearchCriteria
    {
        /**
         * Account order by enum. (optional)
         */
        public AccountOrderBy OrderBy;
        /**
         * Account mosaic id. (optional)
         */
        public MosaicId? MosaicId;

        public AccountSearchCriteria(
            AccountOrderBy orderBy = AccountOrderBy.Id,
            MosaicId mosaicId = null,
            Order order = Order.Asc,
            int pageSize = 20,
            int pageNumber = 1,
            string? offset = null
            ): base(order, pageSize, pageNumber, offset)
        {
            OrderBy = orderBy;
            MosaicId = mosaicId;
        }
    }
}