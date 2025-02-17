using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
     public  class IndexModal
    {
        public List<Banner.Web> BannerList { get; set; }
      
        public List<CMSContent> CMSContent { get; set; }

        public List<Product.Web> ProductList { get; set; }
        public List<Client.Web> ClientsList { get; set; }
        
    }
}
