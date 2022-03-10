using System;
using Symnity.Core.Format;

namespace Symnity.Model.Lock
{
    public enum LockHashAlgorithm
    {
        Op_Sha3_256 = 0,
        Op_Hash_160 = 1,
        Op_Hash_256 = 2,
    }

    public static class LockHashAlgorithmExtensions
    {
        public static bool LockHashAlgorithmLengthValidator(this LockHashAlgorithm self, string input)
        {
            if (ConvertUtils.IsHexString(input))
            {
                switch (self)
                {
                    case LockHashAlgorithm.Op_Sha3_256:
                    case LockHashAlgorithm.Op_Hash_256:
                        return input.Length == 64;
                    case LockHashAlgorithm.Op_Hash_160:
                        return input.Length == 40 || input.Length == 64;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(self), self, null);
                }
            }

            return false;
        }
    }
}