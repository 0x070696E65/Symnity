using System.Numerics;

namespace Symnity.Model.Mosaics
{
    /** This interface is used when a NamespaceId can be provided as an alias of a Mosaic Id. */
    public interface UnresolvedMosaicId
    {
        /**
        * Gets the MosaicId/NamespaceId as a long number. It may be negative is it's overflowed.
        *
        * @return Long id.
        */
        long GetIdAsLong();

        /**
        * Gets the MosaicId/NamespaceId as an hex string.
        *
        * @return the hex string.
        */
        string GetIdAsHex();

        /**
        * Gets the MosaicId/NamespaceId as a {@link BigInteger}.
        *
        * @return Long id.
        */
        BigInteger GetId();
    }
}