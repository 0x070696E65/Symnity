using System;
using System.Runtime.InteropServices;
using Symnity.Http;
using Symnity.Model.Network;
using Symnity.Model.Transactions;
using UnityEngine;

namespace Symnity.UnityScript
{
    [Serializable]
    public class SssExtension : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern string getActivePublicKey();

        [DllImport("__Internal")]
        private static extern string getActiveAddress();

        [DllImport("__Internal")]
        private static extern int getActiveNetworkType();

        public string GetActivePublicKey()
        {
            return getActivePublicKey();
        }

        public string GetActiveAddress()
        {
            return getActiveAddress();
        }

        public NetworkType GetActiveNetworkType()
        {
            return getActiveNetworkType() switch
            {
                104 => NetworkType.MAIN_NET,
                152 => NetworkType.TEST_NET,
                _ => throw new Exception("not network type")
            };
        }

        public string node;
        public string callbackGameObjectName;
        public string callbackFunctionName;
        public string callBackPayload;
        public string transactionType;

        private SssExtension(string callbackGameObjectName, string callbackFunctionName, string callBackPayload,
            string transactionType)
        {
            this.callbackGameObjectName = callbackGameObjectName;
            this.callbackFunctionName = callbackFunctionName;
            this.callBackPayload = callBackPayload;
            this.transactionType = transactionType;
        }

        public void AnnounceTransaction(Transaction transaction, string node, SssMethodType methodType)
        {
            this.node = node;
            var method = methodType switch
            {
                SssMethodType.Announce => "Announce",
                _ => throw new ArgumentOutOfRangeException(nameof(methodType), methodType, null)
            };
            var excuse = new NativeExcuse();
            var callbackParameter = new SssExtension(
                callbackGameObjectName = gameObject.name,
                callbackFunctionName = method,
                callBackPayload = transaction.Serialize(),
                transactionType = GetCallbackTransactionType(transaction.Type)
            );
            var parameterJson = JsonUtility.ToJson(callbackParameter);
            excuse.Excuse("getSignedTransaction", parameterJson);
        }

        public async void Announce(string payload)
        {
            var result = await HttpUtilities.Announce(node, payload);
            Debug.Log(result);
        }
        
        private string GetCallbackTransactionType(TransactionType type)
        {
            var result = type switch
            {
                TransactionType.TRANSFER => TransactionType.TRANSFER.ToString(),
                TransactionType.RESERVED => TransactionType.RESERVED.ToString(),
                TransactionType.NAMESPACE_REGISTRATION => TransactionType.NAMESPACE_REGISTRATION.ToString(),
                TransactionType.ADDRESS_ALIAS => TransactionType.ADDRESS_ALIAS.ToString(),
                TransactionType.MOSAIC_ALIAS => TransactionType.MOSAIC_ALIAS.ToString(),
                TransactionType.MOSAIC_DEFINITION => TransactionType.MOSAIC_DEFINITION.ToString(),
                TransactionType.MOSAIC_SUPPLY_CHANGE => TransactionType.MOSAIC_SUPPLY_CHANGE.ToString(),
                TransactionType.MOSAIC_SUPPLY_REVOCATION => TransactionType.MOSAIC_SUPPLY_REVOCATION.ToString(),
                TransactionType.MULTISIG_ACCOUNT_MODIFICATION =>
                    TransactionType.MULTISIG_ACCOUNT_MODIFICATION.ToString(),
                TransactionType.AGGREGATE_COMPLETE => TransactionType.AGGREGATE_COMPLETE.ToString(),
                TransactionType.AGGREGATE_BONDED => TransactionType.AGGREGATE_BONDED.ToString(),
                TransactionType.HASH_LOCK => TransactionType.HASH_LOCK.ToString(),
                TransactionType.SECRET_LOCK => TransactionType.SECRET_LOCK.ToString(),
                TransactionType.SECRET_PROOF => TransactionType.SECRET_PROOF.ToString(),
                TransactionType.ACCOUNT_ADDRESS_RESTRICTION => TransactionType.ACCOUNT_ADDRESS_RESTRICTION.ToString(),
                TransactionType.ACCOUNT_MOSAIC_RESTRICTION => TransactionType.ACCOUNT_MOSAIC_RESTRICTION.ToString(),
                TransactionType.ACCOUNT_OPERATION_RESTRICTION =>
                    TransactionType.ACCOUNT_OPERATION_RESTRICTION.ToString(),
                TransactionType.ACCOUNT_KEY_LINK => TransactionType.ACCOUNT_KEY_LINK.ToString(),
                TransactionType.MOSAIC_ADDRESS_RESTRICTION => TransactionType.MOSAIC_ADDRESS_RESTRICTION.ToString(),
                TransactionType.MOSAIC_GLOBAL_RESTRICTION => TransactionType.MOSAIC_GLOBAL_RESTRICTION.ToString(),
                TransactionType.ACCOUNT_METADATA => TransactionType.ACCOUNT_METADATA.ToString(),
                TransactionType.MOSAIC_METADATA => TransactionType.MOSAIC_METADATA.ToString(),
                TransactionType.NAMESPACE_METADATA => TransactionType.NAMESPACE_METADATA.ToString(),
                TransactionType.VRF_KEY_LINK => TransactionType.VRF_KEY_LINK.ToString(),
                TransactionType.VOTING_KEY_LINK => TransactionType.VOTING_KEY_LINK.ToString(),
                TransactionType.NODE_KEY_LINK => TransactionType.NODE_KEY_LINK.ToString(),
                _ => throw new ArgumentOutOfRangeException()
            };
            return result;
        }
    }
    public enum SssMethodType
    {
        Announce
    }
}