#nullable enable
using System;
using System.Collections.Generic;
using System.Numerics;
using Symnity.Model.Accounts;
using Symnity.Model.Mosaics;
using Symnity.Model.Transactions;

namespace Symnity.Infrastructure.SearchCriteria
{
    public class NodeSearchCriteria
    {
        public enum Order
        {
            Natural,
            Random
        }
        
        public int Limit;
        public bool Ssl;
        public Order NOrder;
        
        public NodeSearchCriteria(
            int limit = 0,
            bool ssl = true,
            Order order = Order.Natural
        )
        {
            Limit = limit;
            Ssl = ssl;
            NOrder = order;
        }
    }
}