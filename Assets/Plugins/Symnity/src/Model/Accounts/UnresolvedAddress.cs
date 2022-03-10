namespace Symnity.Model.Accounts
{
    /**
    * Unresolved address is used when the referenced account can be accessed via an {@link Address} or
    * a {@link NamespaceId}
    */
    public interface UnresolvedAddress
    {
        /**
        * @param networkType the network type.
        * @return the encoded address or namespace id. Note that namespace id get the hex reversed and
        *     zero padded. See {@link SerializationUtils}
        */
        byte[] EncodeUnresolvedAddress();

        /** @return the plain representation of the address. */
        string Plain();
    }
}