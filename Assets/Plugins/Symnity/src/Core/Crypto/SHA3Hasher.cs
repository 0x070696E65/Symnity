using System;
using Org.BouncyCastle.Crypto.Digests;

namespace Symnity.Core.Crypto
{
    public class SHA3Hasher
    {
        public Sha3Digest Hasher;
        /**
         * Calculates the hash of data.
         * @param {Uint8Array} dest The computed hash destination.
         * @param {Uint8Array} data The data to hash.
         * @param {number} length The hash length in bytes.
         */
        /*public static void Func(List<byte> dest, List<byte> data, int length){
            var hasher = SHA3Hasher.GetHasher(length);
            var hash = hasher.arrayBuffer(data);
            array.copy(dest, array.uint8View(hash));
        }*/
        public SHA3Hasher(Sha3Digest digest)
        {
            Hasher = digest;
        }

        /*
         * Creates a hasher object.
         * @param {number} length The hash length in bytes.
         * @returns {object} The hasher.
         */
        public static SHA3Hasher CreateHasher(int length = 64)
        {
            switch (length)
            {
                case 32:
                    return new SHA3Hasher(new Sha3Digest(256));
                case 64:
                    return new SHA3Hasher(new Sha3Digest(512));
            }
            throw new Exception("length isn't 32 or 64SHA3Hasher");
        }
        /*
        
            void Update(data){
                if (data is Uint8Array) {
                    hash.update(data);
                } else if ('string' === typeof data) {
                    hash.update(convert.hexToUint8(data));
                } else {
                    throw Error('unsupported data type');
                }
            }
            /*
            finalize: (result: any): void => {
                array.copy(result, array.uint8View(hash.arrayBuffer()));
            },*/
        };
    };

    /*
     * Get a hasher instance.
     * @param {numeric} length The hash length in bytes.
     * @returns {object} The hasher.
     */
    /*public static getHasher = (length = 64): any => {
        return {
            32: sha3_256,
            64: sha3_512,
        }[length];
    };*/
    