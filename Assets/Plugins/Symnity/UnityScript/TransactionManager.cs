using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Symnity.Http;
using Symnity.Model.Accounts;
using Symnity.Model.Messages;
using Symnity.Model.Mosaics;
using Symnity.Model.Network;
using Symnity.Model.Transactions;
using UnityEngine;

namespace Symnity.UnityScript
{
    public class TransactionManager : MonoBehaviour
    {
        public static async UniTask<SignedTransaction> CreateSignedTransferTransaction(
            string node,
            NetworkType networkType,
            string receiverAddress,
            string signerPrivateKey,
            string mosaicID,
            long sendMosaicQuantity,
            string message,
            int maxFee
        )
        {
            var epocAdjustment = await HttpUtiles.GetEpochAdjustment(node);
            var deadline = Deadline.Create(epocAdjustment);
            var address = Address.CreateFromRawAddress(receiverAddress);
            var generationHash = await HttpUtiles.GetGenerationHash(node);

            var tx = TransferTransaction.Create(
                deadline,
                address,
                new List<Mosaic>() {new Mosaic(new MosaicId(mosaicID), sendMosaicQuantity)},
                PlainMessage.Create(message),
                networkType,
                maxFee
            );

            var signerAccount = Account.CreateFromPrivateKey(signerPrivateKey, networkType);
            return signerAccount.Sign(tx, generationHash);
        }
    }
}