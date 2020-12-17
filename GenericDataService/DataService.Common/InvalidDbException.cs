using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Common
{
    public class InvalidDbException : Exception
    {
        public string DbName { get; set; }

        public InvalidDbException(string dbName) : base($"Invalid Database : {dbName}")
        {
            DbName = dbName;
        }
    }
}
