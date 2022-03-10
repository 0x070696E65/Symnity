using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Symnity.Core.Crypto;

namespace Symnity.Core.Format
{
    public class KeyGenerator
    {
        public static BigInteger GenerateUInt64Key(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            var hashBytes = CryptoUtilities.CreateSha2356Hash(bytes);
            var list = new List<byte>(hashBytes).GetRange(0, 8);
            list[7] = (byte)(list[7] | 0x80);
            return new BigInteger(list.ToArray());
        }
    }
}

