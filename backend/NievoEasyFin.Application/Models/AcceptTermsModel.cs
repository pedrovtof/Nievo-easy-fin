using NievoEasyFin.Application.Data.Entities;
using NievoEasyFin.Application.Data.Context.Database;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using Dapper;
using System.Text;

namespace NievoEasyFin.Application.Models;

/// <summary>
/// Class model for AcceptTermsmodel
/// </summary>
public class AcceptTermsModel : AcceptTermsEntity
{
    private readonly AuthOrigin _AuthMainNodeDatabase;

    private readonly AuthReplica? _AuthReplicaNodeDatabase;

    private readonly string CODE_SINGUP_TERMS = DotNetEnv.Env.GetString("CODE_SINGUP_TERMS");

    public AcceptTermsModel(AuthOrigin authMainNodeDatabase, AuthReplica authReplicaNodeDatabase)
    {
        _AuthMainNodeDatabase = authMainNodeDatabase;
        _AuthReplicaNodeDatabase = authReplicaNodeDatabase;
    }

    /// <summary>
    /// Getter code singup const
    /// </summary>
    /// <returns>code_singup_terms</returns>
    public string GetCodeSingupTerms() => CODE_SINGUP_TERMS;

    /// <summary>
    /// Not implemented
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<AcceptTermsEntity> CreateAcceptTermsEntityAsync()
        => throw new NotImplementedException();


    /// <summary>
    /// Method to get term code (the last one active)
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public async Task<AcceptTermsEntity> GetAcceptTermsWithCodeAsync(string code)
    {
        var query = new StringBuilder();
        var parameters = new DynamicParameters();

        query.Append("""
            SELECT id, code, "name", description, "version", "content", created_at, updated_at, active
            FROM journey.accept_terms
            """);

        query.Append(" WHERE 1=1 AND code = @code ");

        parameters.Add("code", code);
        parameters.Add("active", true);

        query.Append(" ORDER BY version DESC ");
        query.Append(" LIMIT 1 ;");

        var connection = _AuthReplicaNodeDatabase.Database.GetDbConnection();

        var acceptTerms = await connection.QueryFirstAsync<AcceptTermsEntity>(
            query.ToString(),
            parameters
        );

        return acceptTerms;
    }
}
