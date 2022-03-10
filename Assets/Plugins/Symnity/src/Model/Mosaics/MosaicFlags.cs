using System;
using System.Collections.Generic;
using Symbol.Builders;

namespace Symnity.Model.Mosaics
{
    [Serializable]
    /*
     * Mosaic flags model
     */
    public class MosaicFlags
    {
        /**
         * The creator can choose between a definition that allows a mosaic supply change at a later point or an immutable supply.
         * Allowed values for the property are "true" and "false". The default value is "false".
         */
        public readonly bool SupplyMutable;

        /**
         * The creator can choose if the mosaic definition should allow for transfers of the mosaic among accounts other than the creator.
         * If the property 'transferable' is set to "false", only transfer transactions
         * having the creator as sender or as recipient can transfer mosaics of that type.
         * If set to "true" the mosaics can be transferred to and from arbitrary accounts.
         * Allowed values for the property are thus "true" and "false". The default value is "true".
         */
        public readonly bool Transferable;

        /**
         * Not all the mosaics of a given network will be subject to mosaic restrictions. The feature will only affect
         * those to which the issuer adds the "restrictable" property explicitly at the moment of its creation. This
         * property appears disabled by default, as it is undesirable for autonomous tokens like the public network currency.
         */
        public readonly bool Restrictable;
        
        /**
         *  The creator can choose if he can revoke tokens after a transfer.
         */
        public readonly bool Revokable;

        /*
         * @param flags
         * @param divisibility
         * @param duration
         */
        public MosaicFlags(int flags)
        {
            SupplyMutable = (flags & (int)MosaicFlagsDto.SUPPLY_MUTABLE) != 0;
            Transferable = (flags & (int)MosaicFlagsDto.TRANSFERABLE) != 0;
            Restrictable = (flags & (int)MosaicFlagsDto.RESTRICTABLE) != 0;
            Revokable = (flags & (int)MosaicFlagsDto.REVOKABLE) != 0;
        }

        /**
             * Static constructor function with default parameters
             * @returns {MosaicFlags}
             * @param supplyMutable
             * @param transferable
             * @param restrictable
             */
        public static MosaicFlags Create(bool supplyMutable, bool transferable, bool restrictable = false, bool revokable = false)
        {
            return new MosaicFlags(ToFlag(supplyMutable, transferable, restrictable, revokable));
        }
        
        /**
         * It "adds up" individual flags into a bit wise number flag.
                    *
                    * @param supplyMutable - if the supply is mutable. First flag.
                    * @param transferable - if the balance can be transferred. Second flag.
                    * @param restrictable - if the transaction can be restricted. Third flag.
                    * @param revokable - if the balance can be revoked. Fourth flag.
                    * @private
                    */
        private static int ToFlag(
            bool supplyMutable,
            bool transferable,
            bool restrictable,
            bool revokable) {
            return (supplyMutable ? 1 : 0) + (transferable ? 2 : 0) + (restrictable ? 4 : 0) + (revokable ? 8 : 0);
        }

        /**
        * Get mosaic flag value in number
        * @returns {number}
        */
        public int GetValue()
        {
            return ToFlag(SupplyMutable,Transferable,Restrictable,Revokable);
        }
    }
}