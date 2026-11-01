using System.Text;
using Dapper;
using Microsoft.EntityFrameworkCore;
using NievoEasyFin.Application.Data.Context.Database;
using NievoEasyFin.Application.Data.Entities;
using NievoEasyFin.Application.Data.Views;

namespace NievoEasyFin.Application.Models
{
    public class SchedulerTypeModel : SchedulerTypeEntity
    {
        private readonly CoreOrigin _CoreMainNodeDatabase;

        private readonly CoreReplica _CoreReplicaNodeDatabase;

        public SchedulerTypeModel(CoreOrigin coreMainNodeDatabase, CoreReplica coreReplicaNodeDatabase)
        {
            _CoreMainNodeDatabase = coreMainNodeDatabase;
            _CoreReplicaNodeDatabase = coreReplicaNodeDatabase;
        }

        /// <summary>
        /// Method to search for scheduler types
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<(List<SchedulerTypeView>, int)> GetSchedulerTypeAsync(int page, int pageSize)
        {
            List<SchedulerTypeView> schedulerType = new();
            StringBuilder query = new();
            DynamicParameters parameters = new();

            query.Append("""
                select
                    st.id as Id,
                    st.name as Name, 
                    st.active as Active,
                    st.description as Description,
                    st.cron_tab as CronTab,
                    count(*) over() as Records
                from payment.scheduler_type st 
                where st.active is true
                order by st.id
                limit @limit
                offset @offset
            """);

            parameters.Add("limit", pageSize);
            parameters.Add("offset", (page - 1) * pageSize);

            var connection = _CoreReplicaNodeDatabase.Database.GetDbConnection();

            schedulerType.AddRange(
                await connection.QueryAsync<SchedulerTypeView>(query.ToString(), parameters)
            );

            int records = schedulerType.Any() ? schedulerType.FirstOrDefault().Records : 0;

            return (schedulerType, records);
        }
    }
}