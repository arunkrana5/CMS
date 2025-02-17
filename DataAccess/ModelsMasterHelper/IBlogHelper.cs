using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ModelsMasterHelper
{
    public interface IBlogHelper
    {
        List<BlogType.Admin.List> GetBlogTypeList(GetResponse modal);
        BlogType.Admin.Add GetBlogType(GetResponse modal);
        PostResponse fnSetBlogType(BlogType.Admin.Add model);

        List<BlogTags.Admin.List> GetBlogTagsList(GetResponse modal);
        BlogTags.Admin.Add GetBlogTags(GetResponse modal);
        PostResponse fnSetBlogTags(BlogTags.Admin.Add model);


        List<BlogCategory.Admin.List> GetBlogCategoryList(GetResponse modal);
        BlogCategory.Admin.Add GetBlogCategory(GetResponse modal);
        PostResponse fnSetBlogCategory(BlogCategory.Admin.Add model);
        List<Blog.Admin.List> GetBlogList(GetResponse modal);
        Blog.Admin.Add GetBlog(GetResponse modal);
        PostResponse fnSetBlog(Blog.Admin.Add model);
    }
}
