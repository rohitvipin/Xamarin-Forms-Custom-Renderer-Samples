using System.Net.Http;
using System.Windows.Input;
using Xamarin.Forms;

namespace Samples.ViewModels
{
	public class SearchPageSampleViewModel : BaseViewModel
	{
		public SearchPageSampleViewModel()
		{
			SearchCommand = new Command(async (object obj) =>
			{
				using (var client = new HttpClient())
				{
					SearchResult = await client.GetStringAsync("https://www.googleapis.com/customsearch/v1?key=AIzaSyD44XPaSG0I-jqOSXCWlQCOJtQ4WiN-c4o&cx=017576662512468239146:omuauf_lfve&q=" + obj);
				}
			});
		}

		public ICommand SearchCommand { get; }
		string searchResult;

		public string SearchResult
		{
			get
			{
				return searchResult;
			}

			set
			{
				searchResult = value;
				OnPropertyChanged();
			}
		}
	}
}

