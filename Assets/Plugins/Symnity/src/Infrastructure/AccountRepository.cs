using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Cysharp.Threading.Tasks;
using Symnity.Http.Model;
using Symnity.Infrastructure.SearchCriteria;
using Symnity.Model.Accounts;
using Symnity.Model.Mosaics;

namespace Symnity.Infrastructure
{
    public class AccountRepository
    {
        private string Node;
        public AccountRepository(string node)
        {
            Node = node;
        }

        public async UniTask<AccountInfo> GetAccountInfo(Address address)
        {
            var info = await ApiAccount.GetAccountInformation(Node, address.Plain());
            return GetAccountInformation(info);
        }

        private static AccountInfo GetAccountInformation(ApiAccount.AccountDatum info)
        {
            var mosaics = new List<Mosaic>();
            info.account.mosaics.ForEach(mosaic =>
            {
                var m = new Mosaic(new MosaicId(mosaic.id), long.Parse(mosaic.amount));
                mosaics.Add(m);
            });
            var accountType = info.account.accountType switch
            {
                0 => AccountType.Unlinked,
                1 => AccountType.Main,
                2 => AccountType.Remote,
                3 => AccountType.Remote_Unlinked,
                _ => throw new Exception("")
            };
            var supplementalPublicKeys = new SupplementalPublicKeys();
            if (info.account.supplementalPublicKeys != null)
            {
                supplementalPublicKeys.linked = new AccountLinkPublicKey(info.account.supplementalPublicKeys.linked.publicKey);
                supplementalPublicKeys.node = new AccountLinkPublicKey(info.account.supplementalPublicKeys.node.publicKey); 
                supplementalPublicKeys.vrf = new AccountLinkPublicKey(info.account.supplementalPublicKeys.vrf.publicKey);
                if (info.account.supplementalPublicKeys.voting.Count != 0)
                {
                    for (var i = 0; i < info.account.supplementalPublicKeys.voting.Count; i++)
                    {
                        supplementalPublicKeys.voting[i].publicKey =
                            info.account.supplementalPublicKeys.voting[i].keys[0];
                        supplementalPublicKeys.voting[i].startEpoch =
                            int.Parse(info.account.supplementalPublicKeys.voting[i].keys[1]);
                        supplementalPublicKeys.voting[i].endEpoch =
                            int.Parse(info.account.supplementalPublicKeys.voting[i].keys[2]);
                    }   
                }
            }

            var activityBuckets = new List<ActivityBucket>();
            if (info.account.activityBuckets.Count != 0)
            {
                info.account.activityBuckets.ForEach(activityBucket =>
                {
                    var ab = new ActivityBucket(
                        BigInteger.Parse(activityBucket.startHeight),
                        BigInteger.Parse(activityBucket.totalFeesPaid),
                        activityBucket.beneficiaryCount,
                        BigInteger.Parse(activityBucket.rawScore)
                    );
                    activityBuckets.Add(ab);
                });
            }
            var accountInfo = new AccountInfo(
                info.id,
                info.account.version,
                Address.CreateFromEncoded(info.account.address),
                BigInteger.Parse(info.account.addressHeight),
                info.account.publicKey,
                BigInteger.Parse(info.account.publicKeyHeight),
                BigInteger.Parse(info.account.importance),
                BigInteger.Parse(info.account.importanceHeight),
                mosaics,
                accountType,
                supplementalPublicKeys,
                activityBuckets
            );
            return accountInfo;
        }
        
        public async UniTask<Page<AccountInfo>> Search(AccountSearchCriteria searchCriteria)
        {
            var result = await ApiAccount.SearchAccounts(Node, searchCriteria);
            var list = result.data.Select(GetAccountInformation).ToList();
            return new Page<AccountInfo>(
                list,
                result.pagination.pageNumber,
                result.pagination.pageSize
            );
        }
    }
}