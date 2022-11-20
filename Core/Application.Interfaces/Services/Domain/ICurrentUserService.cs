namespace Application.Interfaces.Services.Domain
{
    public interface ICurrentUserService
    {
        /// <summary>
        /// Returns the current user id
        /// </summary>
        int GetId();

        /// <summary>
        /// Returns the current user name
        /// </summary>
        string? GetName();

        /// <summary>
        /// Returns the current user email
        /// </summary>
        string? GetEmail();

        /// <summary>
        /// Returns the value of a specific claimType
        /// </summary>
        /// <param name="claimType"></param>
        string? GetClaim(string claimType);
    }
}
