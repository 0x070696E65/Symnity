using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using Cysharp.Threading.Tasks;
using Symnity.Core.Format;
using Symnity.Http.Model;
using Symnity.Infrastructure.SearchCriteria;
using Symnity.Model.Accounts;
using Symnity.Model.Metadatas;
using Symnity.Model.Mosaics;

namespace Symnity.Infrastructure
{
    public class MultisigRepository
    {
        private string Node;
        public MultisigRepository(string node)
        {
            Node = node;
        }

        public async UniTask<MultisigAccountInfo> GetMultisigAccountInfo(Address address)
        {
            var info = await ApiMultisig.GetMultisigAccountInfomation(Node, address.Plain());
            return new MultisigAccountInfo(
                info.multisig.version,
                Address.CreateFromEncoded(info.multisig.accountAddress),
                info.multisig.minApproval,
                info.multisig.minRemoval,
                new List<Address>(info.multisig.cosignatoryAddresses.Select(Address.CreateFromEncoded)), 
                new List<Address>(info.multisig.multisigAddresses.Select(Address.CreateFromEncoded))
            );
        }
    }
}