using hookup_backend.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Linq;
using System;
using System.Data.Common;
using hookup_backend_unit_tests.RepoTests;

namespace hookup_backend_unit_tests.SqliteInMemoryTests
{
        public class SqliteInMemoryUserControllerTest : UserRepoTests, IDisposable
        {
            private readonly DbConnection _connection;

            public SqliteInMemoryUserControllerTest()
                : base(
                    new DbContextOptionsBuilder<HookupContext>()
                        .UseSqlite(CreateInMemoryDatabase())
                        .Options)
            {
                _connection = RelationalOptionsExtension.Extract(ContextOptions).Connection;
            }

            private static DbConnection CreateInMemoryDatabase()
            {
                var connection = new SqliteConnection("Filename=:memory:");

                connection.Open();

                return connection;
            }

            public void Dispose() => _connection.Dispose();
        }
    
}