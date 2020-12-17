using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataService.Common
{
    public class SqlConfigurationList
    {
        public List<SqlConfiguration> SqlConfigurations { get; set; }

        public string GetValueByKey(string key)
        {
            return SqlConfigurations?.FirstOrDefault(config => string.Equals(key, config.DbName))?.ConnectionString;
        }
    }
}
