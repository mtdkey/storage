using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using MtdKey.Storage.Context.MSSQL;
using MtdKey.Storage.Context.MySQL;
using MtdKey.Storage.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MtdKey.Storage
{
    /// <summary>
    /// RequestProvider is responsible for all database queries. 
    /// </summary>
    public partial class RequestProvider : IDisposable
    {
        private DbContext context;
        private ContextProperty contextProperty;

        private void InitializeProperties(ContextProperty contextProperty)
        {
            this.contextProperty = contextProperty;

            if (contextProperty.DatabaseType.Equals(DatabaseType.MySQL))
            {
                context = MySQLContextFactory.CreateDbContext(contextProperty);
            }

            if (contextProperty.DatabaseType.Equals(DatabaseType.MSSQL))
            {
                context = MSSQLContextFactory.CreateDbContext(contextProperty);
            }
        }

        public RequestProvider(Action<ContextProperty> action)
        {
            ContextProperty contextProperty = new();
            action.Invoke(contextProperty);
            InitializeProperties(contextProperty);
        }

        public RequestProvider(ContextProperty contextProperty)
        {
            InitializeProperties(contextProperty);
        }

        public async Task BeginTransactionAsync()
        {
            await context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await context.Database.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await context.Database.RollbackTransactionAsync();
        }

        public async Task<IRequestResult> DeleteDatabaseAsync()
        {
            var requestResult = new RequestResult<bool>(true);

            bool exists = (context.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists();
            if (exists is not true) { return requestResult; }
            try
            {
                string databaseName = context.Database.GetDbConnection().Database;
                var script = SqlScript.DropDatabase(databaseName, contextProperty.DatabaseType);
                await context.Database.ExecuteSqlRawAsync(script);
            }
            catch (Exception exception)
            {
                requestResult.SetResultInfo(false, exception);
#if DEBUG
                throw;
#endif
            }

            return requestResult;
        }

        public async Task<IRequestResult> MigrationAsync()
        {
            var requestResult = new RequestResult<bool>(true);
            try
            {
                IEnumerable<string> pendingList = await context.Database.GetPendingMigrationsAsync();
                if (pendingList.Any() is true)
                    await context.Database.MigrateAsync();
            }
            catch (Exception exception)
            {
                requestResult.SetResultInfo(false, exception);
#if DEBUG
                throw;
#endif
            }

            if (!await DatabaseExistsAsync())
            {
                return new RequestResult<IRequestResult>(false, new Exception("Database does not exist!"));
            }

            return requestResult;
        }


        public async Task<bool> DatabaseExistsAsync()
        {
            return await context.Database.GetService<IRelationalDatabaseCreator>().ExistsAsync();
        }

        public void Dispose()
        {
            context.Dispose();
            GC.SuppressFinalize(this);
        }

    }
}
