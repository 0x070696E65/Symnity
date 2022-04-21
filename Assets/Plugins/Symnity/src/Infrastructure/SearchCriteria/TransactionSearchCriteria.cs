using System;
using System.Collections.Generic;
using System.Numerics;
using JetBrains.Annotations;
using Symnity.Model.Accounts;
using Symnity.Model.Mosaics;
using Symnity.Model.Transactions;

namespace Symnity.Infrastructure.SearchCriteria
{
    /**
    * Defines the params used to search transactions. With this criteria, you can sort and filter
    * transactions queries using rest.
    */
    public class TransactionSearchCriteria : SearchCriteria
    {
        /**
        * The group of transaction (confirmed, unconfirmed or partial). Required.
        */
        public TransactionGroup Group;

        /**
        * Filter by address involved in the transaction.
        *
        * An account's address is consider to be involved in the transaction when the account is the
        * sender, recipient, or it is required to cosign the transaction.
        *
        * This filter cannot be combined with ''recipientAddress'' and ''signerPublicKey'' query
        * params.  (optional)
        */
        [CanBeNull] public  Address Address;

        /**
        * Address of an account receiving the transaction. (optional)
        */
        [CanBeNull] public Address RecipientAddress;

        /**
        * Public key of the account signing the entity. (optional)
        */
        [CanBeNull] public string SignerPublicKey;

        /**
        * Filter by block height. (optional, default to null)
        */
        public BigInteger? Height;

        /**
        * Filter by transaction type. To filter by multiple transaction type.  (optional, default to
        * new empty array)
        */
        [CanBeNull] public  List<TransactionType> Type;

        /**
        * When true, the endpoint also returns all the embedded aggregate transactions. When
        * false, only top-level transactions used to calculate the block transactionsHash are
        * returned.  (optional, default to false)
        */
        public bool Embedded;

        /**
        * Only blocks with height greater or equal than this one are returned.
        */
        public BigInteger? FromHeight;

        /**
        * Only blocks with height smaller or equal than this one are returned.
        */
        public BigInteger? ToHeight;

        /**
         * Filters transactions involving a specific `mosaicId` hex.
         */
        [CanBeNull] public MosaicId TransferMosaicId;

        /**
         * Requires providing the `transferMosaicId` filter.
         * Only transfer transactions with a transfer amount of the provided mosaic id, greater or equal than this amount are returned.
         */
        public BigInteger? FromTransferAmount;

        /**
         * Requires providing the `transferMosaicId` filter. Only transfer transactions with a transfer amount of the provided mosaic id, lesser or equal than this amount are returned.
         */
        public BigInteger? ToTransferAmount;

        public TransactionSearchCriteria(
            TransactionGroup group,
            Address address = null,
            Address recipientAddress = null,
            string signerPublicKey = null,
            BigInteger? height = null,
            List<TransactionType> type = null,
            bool embedded = false,
            BigInteger? fromHeight = null,
            BigInteger? toHeight = null,
            MosaicId transferMosaicId = null,
            BigInteger? fromTransferAmount = null,
            BigInteger? toTransferAmount = null,
            Order order = Order.Asc,
            int pageSize = 20,
            int pageNumber = 1,
            string offset = null
            ): base(order, pageSize, pageNumber, offset)
        {
            Group = group;
            Address = address;
            RecipientAddress = recipientAddress;
            SignerPublicKey = signerPublicKey;
            Height = height;
            Type = type;
            Embedded = embedded;
            FromHeight = fromHeight;
            ToHeight = toHeight;
            TransferMosaicId = transferMosaicId;
            FromTransferAmount = fromTransferAmount;
            ToTransferAmount = toTransferAmount;
        }
    }
}