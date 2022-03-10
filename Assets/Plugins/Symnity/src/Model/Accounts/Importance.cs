using System.Numerics;

namespace Symnity.Model.Accounts
{
    public class Importance
    {
        private BigInteger value;
        private BigInteger height;

        /**
        * Constructor.
        *
        * @param value Value
        * @param height Height
        */
        public Importance(BigInteger value, BigInteger height)
        {
            this.value = value;
            this.height = height;
        }
    }
}