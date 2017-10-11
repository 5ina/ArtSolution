using Abp.Application.Navigation;

namespace ArtSolution.Web.Areas.Admin.Models.Layout
{
    public class TopMenuViewModel
    {
        public UserMenu MainMenu { get; set; }

        public string ActiveMenuItemName { get; set; }
    }
}