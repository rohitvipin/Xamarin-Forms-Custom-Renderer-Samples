using Samples.CustomRenderers;
using Samples.ViewModels;

namespace Samples.Views
{
    public partial class SearchPageSample : SearchPage
    {
        public SearchPageSample()
        {
            BindingContext = new SearchPageSampleViewModel();
            InitializeComponent();
        }
    }
}
