using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class AllEnum
    {
        public enum Target
        {
            _self,
            _blank
        }
    }

    public class DropdownList
    {
        public long ID { get; set; }
        public string Name { get; set; }
    }
}
