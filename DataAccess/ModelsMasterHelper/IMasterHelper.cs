using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ModelsMasterHelper
{
    public interface IMasterHelper
    {
        List<WebMenu.Admin.ParentChildCollection> GetMenu_ParentChildCollectionList(GetResponse modal);
        List<WebMenu.Admin.MenuURLType> GetMenuURLTypeList(GetResponse modal);
        PostResponse fnSetWebiteMenu(WebMenu.Admin.Add model);
      
        List<WebMenu.Admin.List> GetMaster_WebsiteMenuList(GetMenuResponse modal);
        WebMenu.Admin.Add GetMaster_WebsiteMenu(GetMenuResponse modal);
      
        PostResponse fnSetCMS(CMS.Add model);
        PostResponse fnSetCMSSection(CMSSection.Add model);

        List<CMS.List> GetCMSList(GetResponse modal);
        CMS.Add GetCMS(GetResponse modal);
        List<CMSSection.List> GetCMSSectionList(GetResponse modal);
        CMSSection.Add GetCMSSection(GetResponse modal);
        List<DocumentType.List> GetDocumentTypeList(GetResponse modal);
        DocumentType.Add GetDocumentType(GetResponse modal);
        PostResponse fnSetDocumentType(DocumentType.Add model);
        List<Banner.Admin.List> GetBannerList(GetResponse modal);
        Banner.Admin.Add GetBanner(GetResponse modal);
        PostResponse fnSetBanner(Banner.Admin.Add model);
        List<Testimonials.Admin.List> GetTestimonialsList(GetResponse modal);
        Testimonials.Admin.Add GetTestimonials(GetResponse modal);
        PostResponse fnSetTestimonials(Testimonials.Admin.Add model);

        List<ExternalLinks.Admin.List> GetExternalLinksList(GetResponse modal);
        ExternalLinks.Admin.Add GetExternalLinks(GetResponse modal);
        PostResponse fnSetExternalLinks(ExternalLinks.Admin.Add model);
        List<FAQ.Admin.List> GetFAQList(GetResponse modal);
        FAQ.Admin.Add GetFAQ(GetResponse modal);
        PostResponse fnSetFAQ(FAQ.Admin.Add model);

        List<GalleryGroup.Admin.List> GetGalleryGroupList(GetResponse modal);
        GalleryGroup.Admin.Add GetGalleryGroup(GetResponse modal);
        PostResponse fnSetGalleryGroup(GalleryGroup.Admin.Add model);

        List<Gallery.Admin.List> GetGalleryList(GetResponse modal);
        Gallery.Admin.Add GetGallery(GetResponse modal);
        PostResponse fnSetGallery(Gallery.Admin.Add model);
        List<Query.List> GetQueryList(GetResponse modal);
        List<MetaTags.Admin.List> GetMetaTagsList(GetResponse modal);
        MetaTags.Admin.Add GetMetaTags(GetResponse modal);
        PostResponse fnSetMetaTags(MetaTags.Admin.Add model);

        List<Client.Admin.List> GetClientList(GetResponse modal);
        Client.Admin.Add GetClient(GetResponse modal);
        PostResponse fnSetClient(Client.Admin.Add model);
        List<MasterAll.Admin.List> GetMasterAllList(GetMasterResponse modal);
        MasterAll.Admin.Add GetMasterAll(GetMasterResponse modal);
        PostResponse fnSetMasterAll(MasterAll.Admin.Add model);


    }
}
