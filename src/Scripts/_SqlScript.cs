using MtdKey.Storage.Context;
using System.IO;
using System.Reflection;


namespace MtdKey.Storage.Scripts
{
    internal static class SqlScript
    {
        private static string GetScript(string scriptName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = typeof(SqlScript).Namespace + $".{scriptName}";
            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            using StreamReader reader = new(stream);
            string sqlScript = reader.ReadToEnd();
            return sqlScript;
        }

        public static string DropDatabase(string databaseName, DatabaseType databaseType)
        {
            var fileName = databaseType.Equals(DatabaseType.MSSQL) ? "MSSQL_DropDatabase.sql" : "MySQL_DropDatabase.sql";
           var script = GetScript(fileName);
           return  script.Replace("DATABASENAME", databaseName);
        }

        public static string SearchText(string text,  DatabaseType databaseType)
        {
            var fileName = databaseType.Equals(DatabaseType.MSSQL) ? "MSSQL_SearchText.sql" : "MySQL_SearchText.sql";
            var safeText = text.Replace("'","''");
            var script = GetScript(fileName);
            return script.Replace("{searchtext}", safeText);
        }

        public static string StackMaxIds(DatabaseType databaseType)
        {
            var fileName = databaseType.Equals(DatabaseType.MSSQL) ? "MSSQL_StackMaxIds.sql" : "MySQL_StackMaxIds.sql";            
            var script = GetScript(fileName);
            return script;
        }

    }
}
