using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class ConfigSetting
    {
        public int ConfigID { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string ConfigKey { get; set; }

        public string ConfigValue { get; set; }
        public string Remarks { get; set; }
        public string Help { get; set; }
    }
}
