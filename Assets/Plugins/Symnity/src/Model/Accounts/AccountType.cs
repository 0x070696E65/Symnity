namespace Symnity.Model.Accounts
{
    /**
     * 0 - Unlinked.
     * 1 - Main account that is linked to a remote harvester account.
     * 2 - Remote harvester account that is linked to a balance-holding account.
     * 3 - Remote harvester eligible account that is unlinked.
     */
    public enum AccountType
    {
        Unlinked = 0,
        Main = 1,
        Remote = 2,
        Remote_Unlinked = 3,
    }
}