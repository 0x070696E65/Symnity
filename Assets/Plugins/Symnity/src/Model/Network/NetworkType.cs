using System;

namespace Symnity.Model.Network
{
    /**
     * Static class containing network type constants.
     */
    [Serializable]
    public enum NetworkType
    {
        /**
         * Main net network
         * @type {number}
         */
        MAIN_NET = 104,
        /**
         * Test net network
         * @type {number}
         */
        TEST_NET = 152,
    }
}