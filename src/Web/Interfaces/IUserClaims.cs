namespace Web.Interfaces
{
    public interface IUserClaims
    {
        string GetClaim(string claimtypes);

        bool IsManager();

        bool IsFrontDesk();
    }
}
