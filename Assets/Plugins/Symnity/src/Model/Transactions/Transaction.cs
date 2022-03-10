using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text.RegularExpressions;
using Symbol.Builders;
using Org.BouncyCastle.Crypto.Digests;
using Symnity.Core.Crypto;
using Symnity.Core.Format;
using Symnity.Model.Accounts;
using Symnity.Model.Network;
using UnityEngine;

namespace Symnity.Model.Transactions
{
    [Serializable]
    public abstract class Transaction
    {
        /**
     * Transaction payload size
     */
        public int? _payloadSize;

        /**
     * Transaction header size
     *
     * Included fields are `size`, `verifiableEntityHeader_Reserved1`,
     * `signature`, `signerPublicKey` and `entityBody_Reserved1`.
     *
     * @var {number}
     */
        public static readonly int HeaderSize = 8 + 64 + 32 + 4;

        /**
     * Index of the transaction *type*
     *
     * Included fields are the transaction header, `version`
     * and `network`
     *
     * @var {number}
     */
        public static readonly int TypeIndex = HeaderSize + 2;

        /**
     * Index of the transaction *body*
     *
     * Included fields are the transaction header, `version`,
     * `network`, `type`, `maxFee` and `deadline`
     *
     * @var {number}
     */
        public static readonly int BodyIndex = HeaderSize + 1 + 1 + 2 + 8 + 8;

        /**
        * The transaction type.
        */
        public readonly TransactionType Type;

        /**
         * The network type.
         */
        public readonly NetworkType NetworkType;

        /**
         * The transaction version number.
         */
        public readonly byte Version;

        /**
         * The deadline to include the transaction.
         */
        public readonly Deadline Deadline;

        /**
         * A sender of a transaction must specify during the transaction definition a max_fee,
         * meaning the maximum fee the account allows to spend for this transaction.
         */
        public long MaxFee;

        /**
         * The transaction signature (missing if part of an aggregate transaction).
         */
        public readonly string Signature;

        /**
         * The account of the transaction creator.
         */
        public PublicAccount Signer;

        /**
         * Transactions meta data object contains additional information about the transaction.
         */
        public readonly TransactionInfo TransactionInfo;

        /**
         * @constructor
         * @param type
         * @param networkType
         * @param version
         * @param deadline
         * @param maxFee
         * @param signature
         * @param signer
         * @param transactionInfo
         */
        internal Transaction(TransactionType type, NetworkType networkType, byte version, Deadline deadLine,
            long maxFee,
            string signature = null, PublicAccount signer = null, TransactionInfo transactionInfo = null)
        {
            _payloadSize = null;
            Type = type;
            NetworkType = networkType;
            Version = version;
            Deadline = deadLine;
            MaxFee = maxFee;
            Signature = signature;
            Signer = signer;
            TransactionInfo = transactionInfo;
        }

        /**
         * Generate transaction hash hex
         *
         * @see https://github.com/nemtech/catapult-server/blob/main/src/catapult/model/EntityHasher.cpp#L32
         * @see https://github.com/nemtech/catapult-server/blob/main/src/catapult/model/EntityHasher.cpp#L35
         * @see https://github.com/nemtech/catapult-server/blob/main/sdk/src/extensions/TransactionExtensions.cpp#L46
         * @param {string} transactionPayload HexString Payload
         * @param {Array<number>} generationHashBuffer Network generation hash byte
         * @returns {string} Returns Transaction Payload hash
         */
        public static string CreateTransactionHash(string transactionPayload, byte[] generationHashBuffer)
        {
            // prepare
            var entityHash = new byte[32];
            var transactionBytes = new List<byte>(ConvertUtils.GetBytes(transactionPayload));
            // read transaction type
            var typeIdx = TypeIndex;
            var x = transactionBytes.GetRange(typeIdx, 2);
            x.Reverse();
            var entityType = 0;
            if (int.TryParse(ConvertUtils.ToHex(x.ToArray()), System.Globalization.NumberStyles.HexNumber, null,
                out var i))
            {
                entityType = i;
            }

            var isAggregateTransaction = (entityType == (int) TransactionType.AGGREGATE_BONDED ||
                                          entityType == (int) TransactionType.AGGREGATE_COMPLETE);

            // 1) add full signature
            var signature = transactionBytes.GetRange(8, 64);
            // 2) add public key to match sign/verify behavior (32 bytes)
            var publicKey = transactionBytes.GetRange(8 + 64, 32);

            // 3) add generationHash (32 bytes)
            var generationHash = new List<byte>(generationHashBuffer);

            // 4) add transaction data without header (EntityDataBuffer)
            // @link https://github.com/nemtech/catapult-server/blob/main/src/catapult/model/EntityHasher.cpp#L30
            var transactionBody = transactionBytes.GetRange(HeaderSize, transactionBytes.Count - HeaderSize);

            // in case of aggregate transactions, we hash only the merkle transaction hash.
            if (isAggregateTransaction) {
                transactionBody = transactionBytes.GetRange(HeaderSize, BodyIndex + 32 - HeaderSize);
            }

            // 5) concatenate binary hash parts
            // layout: `signature_R || signerPublicKey || generationHash || EntityDataBuffer`
            var entityHashBytes =
                new List<byte>(signature.Count + publicKey.Count + generationHash.Count + transactionBody.Count);
            entityHashBytes.AddRange(signature);
            entityHashBytes.AddRange(publicKey);
            entityHashBytes.AddRange(generationHash);
            entityHashBytes.AddRange(transactionBody);

            // 6) create SHA3 hash of transaction data
            // Note: Transaction hashing *always* uses SHA3
            var sha3256Hash = CryptoUtilities.CreateSha2356Hash(entityHashBytes.ToArray());
            return ConvertUtils.ToHex(sha3256Hash);
        }

        /**
        * @internal
        * Serialize and sign transaction creating a new SignedTransaction
        * @param account - The account to sign the transaction
        * @param generationHash - Network generation hash hex
        * @returns {SignedTransaction}
        */
        public SignedTransaction SignWith(Account account, string generationHash)
        {
            var generationHashBytes = ConvertUtils.GetBytes(generationHash);
            var byteBuffer = GenerateBytes();
            // 1. prepare the raw transaction to be signed
            var signingBytes = GetSigningBytes(new List<byte>(byteBuffer), new List<byte>(generationHashBytes));
            // 2. sign the raw transaction
            var signature = SignRawTransaction(account.GetPrivateKey(), signingBytes.ToArray());
            // 3. prepare the (signed) payload
            var payload = PreparePayload(byteBuffer, signature, account.GetPublicKey());
            return new SignedTransaction(
                payload,
                CreateTransactionHash(payload, generationHashBytes),
                account.GetPublicKey(),
                Type,
                NetworkType
            );
        }

        /**
         * Signs raw transaction with the given private key
         * @param {string} privateKey - Private key of the signer account
         * @param {Uint8Array} rawTransactionSigningBytes - Raw transaction siging bytes
         * @returns {Uint8Array} Signature byte array
         */
        public static byte[] SignRawTransaction(string privateKey, byte[] rawTransactionSigningBytes)
        {
            var keyPairEncoded = KeyPair.CreateKeyPairFromPrivateKeyString(privateKey);
            return KeyPair.Sign(keyPairEncoded, rawTransactionSigningBytes);
        }

        /**
         * Prepares and return signed payload
         * @param {Uint8Array} serializedTransaction Serialized transaction
         * @param {Uint8Array} signature Signature of the transaction
         * @param {string} publicKey Public key of the signing account
         * @returns {string} Payload (ready to be announced)
         */
        public static string PreparePayload(byte[] serializedTransaction, byte[] signature, string publicKey)
        {
            var signedTransactionBuffer = new List<byte>(serializedTransaction);
            var signatureTransactionBuffer = new List<byte>(signature);
            var resultList = signedTransactionBuffer.GetRange(0, 8);
            resultList.AddRange(signatureTransactionBuffer);
            resultList.AddRange(new List<byte>(ConvertUtils.GetBytes(publicKey)));
            resultList.AddRange(new List<byte> {0, 0, 0, 0});
            resultList.AddRange(signedTransactionBuffer.GetRange(64 + 32 + 4 + 8, signedTransactionBuffer.Count - 108));
            return ConvertUtils.ToHex(resultList.ToArray());
        }

        /**
        * Generate signing bytes
        * @param payloadBytes Payload buffer
        * @param generationHashBytes GenerationHash buffer
        * @return {number[]}
        */
        public List<byte> GetSigningBytes(List<byte> payloadBytes, List<byte> generationHashBytes)
        {
            var byteBufferWithoutHeader =
                payloadBytes.GetRange(4 + 64 + 32 + 8, payloadBytes.Count - (4 + 64 + 32 + 8));
            if (Type == TransactionType.AGGREGATE_BONDED || Type == TransactionType.AGGREGATE_COMPLETE)
            {
                generationHashBytes.AddRange(byteBufferWithoutHeader.GetRange(0, 52));
                return generationHashBytes;
            }

            {
                generationHashBytes.AddRange(byteBufferWithoutHeader);
                return generationHashBytes;
            }
        }
        
        /**
         * @override Transaction.size()
         * @description get the byte size of a transaction using the builder
         * @returns {number}
         * @memberof TransferTransaction
         */ 
        public virtual int GetSize() {
            return _payloadSize ?? CreateBuilder.GetSize();
        }

        /*
        * @internal
        * @returns {byte[]}
        */
        public virtual byte[] GenerateBytes()
        {
            return CreateBuilder.Serialize();
        }
        
        public TransactionBuilder CreateBuilder;
        
        /*
         * @description Serialize a transaction object
         * @returns {string}
         * @memberof Transaction
         */
        public virtual string Serialize()
        {
            return ConvertUtils.ToHex(GenerateBytes());
        }

        public virtual EmbeddedTransactionBuilder ToEmbeddedTransaction()
        {
            return new EmbeddedTransactionBuilder(
                new PublicKeyDto(ConvertUtils.GetBytes(Signer.PublicKey)),
                Version,
                (NetworkTypeDto)Enum.ToObject(typeof(NetworkTypeDto), (int)NetworkType),
                (TransactionTypeDto)Enum.ToObject(typeof(TransactionTypeDto), (int)Type)
            );
        }

        /**
     * @internal
     */
        public byte VersionToDTO()
        {
            return (byte) (((int) NetworkType << 8) + Version);
        }


        /**
         * @internal
         *
         * Converts the optional signer to a KeyDto that can be serialized.
         */
        protected virtual PublicKeyDto GetSignerAsBuilder()
        {
            return Signer != null ? Signer.ToBuilder() : new PublicKeyDto(new byte[32]);
        }

        /**
         * @internal
         *
         * Converts the optional signature to a SignatureDto that can be serialized.
         */
        protected virtual SignatureDto GetSignatureAsBuilder()
        {
            return new SignatureDto(Signature != null ? ConvertUtils.GetBytes(Signature) : new byte[64]);
        }
        
        /**
         * Set transaction maxFee using fee multiplier for **ONLY NONE AGGREGATE TRANSACTIONS**
         * @param feeMultiplier The fee multiplier
         * @returns {TransferTransaction}
         */
        public Transaction SetMaxFee(int feeMultiplier)
        {
            if (Type == TransactionType.AGGREGATE_BONDED && Type == TransactionType.AGGREGATE_COMPLETE) {
                throw new Exception("setMaxFee can only be used for none aggregate transactions.");
            }
            MaxFee = feeMultiplier * GetSize();
            return this;
        }

        /**
         * Convert an aggregate transaction to an inner transaction including transaction signer.
         * Signer is optional for `AggregateComplete` transaction `ONLY`.
         * If no signer provided, aggregate transaction signer will be delegated on signing
         * @param signer - Innre transaction signer.
         * @returns InnerTransaction
         */
        public Transaction ToAggregate(PublicAccount signer)
        {
            Signer = signer;
            return this;
        }
        
        /**
     * @internal
     *
     * Returns the signature from the serialized payload.
     */
        public static string GetSignatureFromPayload(string payload, bool isEmbedded)
        {
            var signature = payload.Substring(16, 144);
            return ResolveSignature(signature, isEmbedded);
        }

        /**
     * @internal
     *
     * Returns the signature hold in the transaction.
     */
        public static string ResolveSignature(string signature, bool isEmbedded)
        {
            return signature == null || isEmbedded || Regex.IsMatch(signature, "^[0]*$") ? null : signature;
        }
    }
}