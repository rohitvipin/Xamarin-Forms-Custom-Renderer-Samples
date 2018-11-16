using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using Samples.CustomRenderers;
using Samples.Droid.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using SearchView = Android.Support.V7.Widget.SearchView;

[assembly: ExportRenderer(typeof(SearchPage), typeof(SearchPageRenderer))]
namespace Samples.Droid.CustomRenderer
{
    public class SearchPageRenderer : PageRenderer
    {
        private SearchView _searchView;

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            if (e?.NewElement == null || e.OldElement != null)
            {
                return;
            }

            var inflater = LayoutInflater.From(MainActivity.Current);

            var toolbarView = inflater.Inflate(Resource.Layout.Toolbar, null);
            var toolbar = MainActivity.Current.FindViewById<Android.Support.V7.Widget.Toolbar>(toolbarView.Id);

            MainActivity.ToolBar = toolbar;

            MainActivity.ToolBar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);

            AddSearchToToolBar();
        }

        protected override void Dispose(bool disposing)
        {
            if (_searchView != null)
            {
                _searchView.QueryTextChange -= searchView_QueryTextChange;
                _searchView.QueryTextSubmit -= searchView_QueryTextSubmit;
            }
            MainActivity.ToolBar?.Menu?.RemoveItem(Resource.Menu.mainmenu);
            base.Dispose(disposing);
        }

        private void AddSearchToToolBar()
        {
            if (MainActivity.ToolBar == null || Element == null)
            {
                return;
            }

            MainActivity.ToolBar.Title = Element.Title;
            MainActivity.ToolBar.InflateMenu(Resource.Menu.mainmenu);

            _searchView = MainActivity.ToolBar.Menu?.FindItem(Resource.Id.action_search)?.ActionView?.JavaCast<SearchView>();

            if (_searchView == null)
            {
                return;
            }

            _searchView.QueryTextChange += searchView_QueryTextChange;
            _searchView.QueryTextSubmit += searchView_QueryTextSubmit;
            _searchView.QueryHint = (Element as SearchPage)?.SearchPlaceHolderText;
            _searchView.ImeOptions = (int)ImeAction.Search;
            _searchView.InputType = (int)InputTypes.TextVariationNormal;
            _searchView.MaxWidth = int.MaxValue;        //Hack to go full width - http://stackoverflow.com/questions/31456102/searchview-doesnt-expand-full-width
        }

        private void searchView_QueryTextSubmit(object sender, SearchView.QueryTextSubmitEventArgs e)
        {
            if (e == null)
            {
                return;
            }

            var searchPage = Element as SearchPage;
            if (searchPage == null)
            {
                return;
            }
            searchPage.SearchText = e.Query;
            searchPage.SearchCommand?.Execute(e.Query);
            e.Handled = true;
        }

        private void searchView_QueryTextChange(object sender, SearchView.QueryTextChangeEventArgs e)
        {
            var searchPage = Element as SearchPage;
            if (searchPage == null)
            {
                return;
            }
            searchPage.SearchText = e?.NewText;
        }
    }
}