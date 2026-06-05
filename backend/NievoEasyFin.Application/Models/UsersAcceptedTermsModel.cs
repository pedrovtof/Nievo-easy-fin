using NievoEasyFin.Application.Data.Entities;
using NievoEasyFin.Application.Data.Context.Database;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NievoEasyFin.Application.Models;

/// <summary>
/// Class model for userAcceptedTerms
/// </summary>
public class UsersAcceptedTermsModel : UsersAcceptedTermsEntity
{
    private readonly AuthOrigin _AuthMainNodeDatabase;

    private readonly AuthReplica? _AuthReplicaNodeDatabase;

    public UsersAcceptedTermsModel(AuthOrigin authMainNodeDatabase, AuthReplica authReplicaNodeDatabase)
    {
        _AuthMainNodeDatabase = authMainNodeDatabase;
        _AuthReplicaNodeDatabase = authReplicaNodeDatabase;
    }

    /// <summary>
    /// Method to create the data for column RequestDetails
    /// </summary>
    /// <param name="host">host from request header</param>
    /// <param name="UserAgent">User agent from request header</param>
    /// <returns>object</returns>
    public object MountRequestDetails(string host, string UserAgent)
        => new
        {
            Host = host,
            UserAgent = UserAgent
        };

    /// <summary>
    /// Create user acceptTerms entity from singup
    /// </summary>
    /// <param name="accepTermstId">ID of the terms accept</param>
    /// <param name="host">host of the request</param>
    /// <param name="userAgent">userAgent of the request</param>
    /// <param name="acceptTerms">accept terms of the request</param>
    /// <param name="userId">ID of the user</param>
    /// <returns></returns>
    public async Task<UsersAcceptedTermsEntity> CreateUsersAcceptedTermsEntityAsync(int accepTermstId, string host, string userAgent, bool acceptTerms, int userId)
    {
        if (AcceptId <= 1)
            return null;

        UsersAcceptedTermsEntity usersAcceptedTerms = new UsersAcceptedTermsEntity()
        {
            AcceptId = accepTermstId,
            UserId = userId,
            Accepted = acceptTerms,
            RequestDetails = JsonConvert.SerializeObject(MountRequestDetails(host, userAgent)),
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        await _AuthMainNodeDatabase.UsersAcceptedTerms.AddAsync(usersAcceptedTerms);
        await _AuthMainNodeDatabase.SaveChangesAsync();

        return usersAcceptedTerms;
    }

    /// <summary>
    /// Search for the UserId and AcceptId
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="acceptId"></param>
    /// <returns></returns>
    public async Task<UsersAcceptedTermsEntity> GetUsersAcceptedTermsEntityWithUserIdAndAcceptIdAsync(int userId, int acceptId)
        => await _AuthReplicaNodeDatabase.UsersAcceptedTerms.FirstOrDefaultAsync<UsersAcceptedTermsEntity>(x => x.UserId == userId && x.AcceptId == acceptId);
}
