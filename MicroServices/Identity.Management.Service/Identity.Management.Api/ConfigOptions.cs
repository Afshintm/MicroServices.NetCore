using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Management.Api
{
    public class ConfigOptions
    {
        public class ConnectionString
        {
            public string IdentityServerOprationsConnectionString { get; set; }
            public string ApplicationIdentityConnectionString { get; set; }
        }

        public ConnectionString ConnectionStrings { get; set; }
        public string MigrationAssembly { get; set; }
    }

}
