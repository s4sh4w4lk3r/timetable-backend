using Throw;

namespace Repository
{
    public class DbConfiguration
    {
        public string? ConnectionString { get; init; }
        public string? DatabaseEngineName { get; init; }
        public DatabaseEngine DatabaseEngine 
        { 
            get 
            {
                DatabaseEngineName.ThrowIfNull().IfWhiteSpace();

                return DatabaseEngineName.ToLower() switch
                {
                    "mysql" => DatabaseEngine.MySql,
                    "postgresql" => DatabaseEngine.PostgreSql,
                    "postgres" => DatabaseEngine.PostgreSql,
                    _ => throw new InvalidOperationException($"Тип базы данных {DatabaseEngineName} не поддерживается."),
                };
            }
        }
        public string? DefaultSchema { get; init; }
        public string? Collation { get; init; }
        public string? CharSet { get; init; }
        public string? PostgresAdminDbName { get; init; } = "postgres";
    }
    public enum DatabaseEngine { MySql, PostgreSql }
}
