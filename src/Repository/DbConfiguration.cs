namespace Repository
{
    public class DbConfiguration
    {
        public string? ConnectionString { get; init; }
        public string? DefaultSchema { get; init; }
        public string? Collation { get; init; }
        public string? PostgresAdminDbName { get; init; } = "postgres";
    }
}