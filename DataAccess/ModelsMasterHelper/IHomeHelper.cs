using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ModelsMasterHelper
{
    public interface IHomeHelper
    {
        List<WebMenu.List> GetMenu_Web(GetMenuResponse modal);
        List<CMSContent> GetCMSContent(GetWebResponse modal);
        List<Banner.Web> GetBanner_WebList(GetBannerResponse modal);
        List<Testimonials.List> GetTestimonialsList(GetResponse modal);
        List<ExternalLinks.List> GetExternalLinksList(GetResponse modal);
        List<FAQ.List> GetFAQList(GetResponse modal);
        List<MetaTags.Web> GetMetaTags_WebList(GetResponse modal);
        string GetMenuURLType(string MenuURL);
        List<Blog.Web> GetBlog_WebList(GetWebResponse modal);
        Blog.Details GetBlogDetails_Web(GetWebResponse modal);
        List<BlogRelatedInformation> BlogRelatedInformationList(GetResponse modal);
        IndexModal GetIndexDetails();
        List<Blog.Web> GetBlogOthers_WebList(GetWebResponse modal);

        List<Product.Web> GetProduct_WebList(string CategoryURL, string ProductURL);
        List<Category.Web> GetCategory_WebList();
        List<Gallery.Web> GetGalleryList_Web(int IsShowHome = 0);

    }
}
