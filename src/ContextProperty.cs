using System.Collections.Generic;

namespace MtdKey.Storage
{
    public class ContextProperty
    {
        public DatabaseType DatabaseType { get; set; }
        public string ConnectionString { get; set; }
        public string MasterToken { get; set; }
        public List<string> AccessTokens { get; set; }

        public ContextProperty()
        {
            AccessTokens = new() { string.Empty };
            MasterToken = string.Empty;
        }

    }
}
