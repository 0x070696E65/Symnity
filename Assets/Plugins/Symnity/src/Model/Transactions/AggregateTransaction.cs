using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Symbol.Builders;
using Symnity.Core.Crypto;
using Symnity.Core.Format;
using Symnity.Model.Accounts;
using Symnity.Model.Network;

namespace Symnity.Model.Transactions
{
    public class AggregateTransaction : Transaction
    {
        /**
         * The array of innerTransactions included in the aggregate transaction.
         */
        public readonly List<Transaction> InnerTransactions;

        /**
         * The array of transaction cosigners signatures.
         */
        public readonly List<AggregateTransactionCosignature> Cosignatures;
        
        /**
         * @param networkType
         * @param type
         * @param version
         * @param deadline
         * @param maxFee
         * @param innerTransactions
         * @param cosignatures
         * @param signature
         * @param signer
         * @param transactionInfo
         */
        public AggregateTransaction(
            NetworkType networkType,
            TransactionType type,
            byte version,
            Deadline deadline,
            long maxFee,
            List<Transaction> innerTransactions,
            List<AggregateTransactionCosignature> cosignatures,
            string signature = null,
            PublicAccount signer = null,
            TransactionInfo transactionInfo = null
        )
            : base(type, networkType, version, deadline, maxFee, signature, signer, transactionInfo)
        {
            InnerTransactions = innerTransactions;
            Cosignatures = cosignatures;
        }

        /**
         * Create an aggregate complete transaction object
         * @param deadline - The deadline to include the transaction.
         * @param innerTransactions - The array of inner innerTransactions.
         * @param networkType - The network type.
         * @param cosignatures
         * @param maxFee - (Optional) Max fee defined by the sender
         * @param signature - (Optional) Transaction signature
         * @param signer - (Optional) Signer public account
         * @returns {AggregateTransaction}
         */
        public static AggregateTransaction CreateComplete(
            Deadline deadline,
            List<Transaction> innerTransactions,
            NetworkType networkType,
            List<AggregateTransactionCosignature> cosignatures,
            long maxFee = 0,
            string signature = null,
            PublicAccount signer = null
        )
        {
            return new AggregateTransaction(
                networkType,
                TransactionType.AGGREGATE_COMPLETE,
                TransactionVersion.AGGREGATE_COMPLETE,
                deadline,
                maxFee,
                innerTransactions,
                cosignatures,
                signature,
                signer
            );
        }
        
        /*
         * Create a transaction object from payload
         * @param {string} payload Binary payload
         * @returns {AggregateTransaction}
         */
    //public static AggregateTransaction CreateFromPayload(string payload) {
        /*
         * Get transaction type from the payload hex
         * As buffer uses separate builder class for Complete and bonded
         */
        /*var builder = AggregateCompleteTransactionBuilder.LoadFromBinary(new BinaryReader(new MemoryStream(ConvertUtils.GetBytes(payload))));
        var type = (TransactionType)Enum.ToObject(typeof(TransactionType), builder.type);

        var innerTransactions = builder.GetTransactions();
        var networkType = (NetworkType)Enum.ToObject(typeof(NetworkType), builder.GetNetwork());
        var signerPublicKey = ConvertUtils.ToHex(builder.GetSignerPublicKey().GetKey());
        var signature = GetSignatureFromPayload(payload, false);
        var consignatures = builder.GetCosignatures().Select((cosig) => {
            return new AggregateTransactionCosignature(
                ConvertUtils.ToHex(cosig.signature.GetSignature()),
                PublicAccount.CreateFromPublicKey(ConvertUtils.ToHex(cosig.signerPublicKey.GetKey()), networkType),
                cosig.version
            );
        });

        return type == TransactionType.AGGREGATE_COMPLETE
            ? AggregateTransaction.CreateComplete(
                  Deadline.CreateFromDTO(builder.deadline.GetTimestamp()),
                  innerTransactions.map((transactionRaw) => {
                      return CreateTransactionFromPayload(Convert.uint8ToHex(transactionRaw.serialize()), true) as InnerTransaction;
                  }),
                  networkType,
                  consignatures,
                  new UInt64(builder.fee.amount),
                  signature,
                  signerPublicKey.match(`^[0]+$`) ? undefined : PublicAccount.createFromPublicKey(signerPublicKey, networkType),
              )
            : AggregateTransaction.createBonded(
                  Deadline.createFromDTO(builder.deadline.timestamp),
                  innerTransactions.map((transactionRaw) => {
                      return CreateTransactionFromPayload(Convert.uint8ToHex(transactionRaw.serialize()), true) as InnerTransaction;
                  }),
                  networkType,
                  consignatures,
                  new UInt64(builder.fee.amount),
                  signature,
                  signerPublicKey.match(`^[0]+$`) ? undefined : PublicAccount.createFromPublicKey(signerPublicKey, networkType),
              );
        }*/
        
        /**
         * @override Transaction.size()
         * @description get the byte size of a transaction using the builder
         * @returns {number}
         * @memberof TransferTransaction
         */ 
        public override int GetSize() {
            return _payloadSize ?? CreateBuilder().GetSize();
        }
        
        /**
         * @description Serialize a transaction object
         * @returns {string}
         * @memberof Transaction
         */
        public override string Serialize()
        {
            return ConvertUtils.ToHex(GenerateBytes());
        }
        
        /*
        * @internal
        * @returns {byte[]}
        */
        public override byte[] GenerateBytes()
        {
            return CreateBuilder().Serialize();
        }
        
        /*
         * @internal
         * @returns {TransactionBuilder}
         */
        public new AggregateCompleteTransactionBuilder CreateBuilder()
        {
            var embeddedTransactions = 
                new List<Transaction>(InnerTransactions).Select(transaction => transaction.ToEmbeddedTransaction());
            var coSignatureBuilders = 
                new List<AggregateTransactionCosignature>(Cosignatures).Select((coSignature) => {
                var signerBytes = ConvertUtils.GetBytes(coSignature.Signer.PublicKey);
                var signatureBytes = ConvertUtils.GetBytes(coSignature.Signature);
                return new CosignatureBuilder(coSignature.Version, new PublicKeyDto(signerBytes), new SignatureDto(signatureBytes));
            });
            
            //if(Type == TransactionType.AGGREGATE_COMPLETE) {
                return new AggregateCompleteTransactionBuilder(
                    GetSignatureAsBuilder(),
                    GetSignerAsBuilder(),
                    VersionToDTO(),
                    (NetworkTypeDto)Enum.ToObject(typeof(NetworkTypeDto), (byte)NetworkType),
                    (TransactionTypeDto)Enum.ToObject(typeof(TransactionTypeDto), (short) TransactionType.AGGREGATE_COMPLETE),
                    new AmountDto(MaxFee),
                    new TimestampDto(Deadline.AdjustedValue),
                    new Hash256Dto(CalculateInnerTransactionHash()),
                    embeddedTransactions.ToList(),
                    coSignatureBuilders.ToList()
                );
            //}
            /*else
            {
                return new AggregateBondedTransactionBuilder(
                    GetSignatureAsBuilder(),
                    GetSignerAsBuilder(),
                    VersionToDTO(),
                    NetworkTypeDto.PUBLIC_TEST.RawValueOf((byte) NetworkType),
                    EntityTypeDto.RESERVED.RawValueOf((short) TransactionType.AGGREGATE_COMPLETE),
                    new AmountDto(MaxFee),
                    new TimestampDto(Deadline.AdjustedValue),
                    new Hash256Dto(CalculateInnerTransactionHash()),
                    embeddedTransactions,
                    cosignatureBuilders
                );
            }*/
        }
        
        /*
         * @internal
         * Generate inner transaction root hash (merkle tree)
         * @returns {byte[]}
         */
        private byte[] CalculateInnerTransactionHash() {
            
            var builder = new MerkleHashBuilder(32);
            foreach (var transaction in InnerTransactions)
            {
                var entityHash = new byte[32];
                
                // Note: Transaction hashing *always* uses SHA3
                var hasher = SHA3Hasher.CreateHasher(32);
                // for each embedded transaction hash their body
                
                var serializedByte = new List<byte>(transaction.ToEmbeddedTransaction().Serialize()); 
                
                var padding = GeneratorUtils.GetPadding(serializedByte.Count, 8);
                var listPadding = new List<byte>(padding);
                for (var i = 0; i < padding; i++) listPadding.Add(0);
                serializedByte.AddRange(listPadding);

                hasher.Hasher.BlockUpdate(serializedByte.ToArray(), 0, serializedByte.Count);
                hasher.Hasher.DoFinal(entityHash, 0);

                // update merkle tree (add transaction hash)
                builder.Update(new List<byte>(entityHash));
            }
            
            // calculate root hash with all transactions
            return builder.GetRootHash().ToArray();
        }
        
        /**
         * @internal
         * Sign transaction with cosignatories creating a new SignedTransaction
         * @param initiatorAccount - Initiator account
         * @param cosignatories - The array of accounts that will cosign the transaction
         * @param generationHash - Network generation hash hex
         * @returns {SignedTransaction}
         */
        public SignedTransaction SignTransactionWithCosignatories(
            Account initiatorAccount,
            List<Account> cosignatories,
            string generationHash
        ) {
            var signedTransaction = SignWith(initiatorAccount, generationHash);
            var transactionHashBytes = ConvertUtils.GetBytes(signedTransaction.Hash);
            var signedPayload = signedTransaction.Payload;
            cosignatories.ForEach((cosigner) => {
                var keyPairEncoded = KeyPair.CreateKeyPairFromPrivateKeyString(cosigner.GetPrivateKey());
                var signature = KeyPair.Sign(keyPairEncoded, transactionHashBytes);
                signedPayload += "0000000000000000" + cosigner.GetPublicKey() + ConvertUtils.ToHex(signature);
            });

            // Calculate new size
            var size = "00000000" + (signedPayload.Length / 2).ToString("X");
            var formatedSize = size.Substring(size.Length - 8);
            var littleEndianSize =
                formatedSize.Substring(6, 2) + formatedSize.Substring(4, 2) + formatedSize.Substring(2, 2) + formatedSize.Substring(0, 2);
            signedPayload = littleEndianSize + signedPayload.Substring(8, signedPayload.Length - 8);
            return new SignedTransaction(signedPayload, signedTransaction.Hash, initiatorAccount.GetPublicKey(), Type, NetworkType);
        }
        
        /**
         * @internal
         * Sign transaction with cosignatories collected from cosigned transactions and creating a new SignedTransaction
         * For off chain Aggregated Complete Transaction co-signing.
         * @param initiatorAccount - Initiator account
         * @param {CosignatureSignedTransaction[]} cosignatureSignedTransactions - Array of cosigned transaction
         * @param generationHash - Network generation hash hex
         * @return {SignedTransaction}
         */
        public SignedTransaction SignTransactionGivenSignatures(
            Account initiatorAccount,
            CosignatureSignedTransaction[] cosignatureSignedTransactions,
            string generationHash
        ) {
            var signedTransaction = SignWith(initiatorAccount, generationHash);
            var signedPayload = signedTransaction.Payload;
            var listCosignatureSignedTransactions = new List<CosignatureSignedTransaction>(cosignatureSignedTransactions);
            listCosignatureSignedTransactions.ForEach((cosignedTransaction) => {
                signedPayload += cosignedTransaction.Version.ToString("D16") + cosignedTransaction.SignerPublicKey + cosignedTransaction.Signature;
            });
            
            // Calculate new size
            var size = "00000000" + (signedPayload.Length / 2).ToString("X");
            var formatedSize = size.Substring(size.Length - 8);
            var littleEndianSize =
            formatedSize.Substring(6, 2) + formatedSize.Substring(4, 2) + formatedSize.Substring(2, 2) + formatedSize.Substring(0, 2);
            signedPayload = littleEndianSize + signedPayload.Substring(8, signedPayload.Length - 8);
            return new SignedTransaction(signedPayload, signedTransaction.Hash, initiatorAccount.GetPublicKey(), Type, NetworkType);
        }
        
        /**
         * Set transaction maxFee using fee multiplier for **ONLY AGGREGATE TRANSACTIONS**
         * @param feeMultiplier The fee multiplier
         * @param requiredCosignatures Required number of cosignatures
         * @returns {AggregateTransaction}
         */
        public AggregateTransaction SetMaxFeeForAggregate(int feeMultiplier, int requiredCosignatures) {
            if (Type != TransactionType.AGGREGATE_BONDED && Type != TransactionType.AGGREGATE_COMPLETE) {
                throw new Exception("setMaxFeeForAggregate can only be used for aggregate transactions.");
            }
            // Check if current cosignature count is greater than requiredCosignatures.
            var calculatedCosignatures = requiredCosignatures > Cosignatures.Count ? requiredCosignatures : Cosignatures.Count;
            // version + public key + signature
            var sizePerCosignature = 8 + 32 + 64;
            // Remove current cosignature length and use the calculated one.
            var calculatedSize = GetSize() - Cosignatures.Count * sizePerCosignature + calculatedCosignatures * sizePerCosignature;
            MaxFee = calculatedSize * feeMultiplier;
            return this;
        }
    }
}