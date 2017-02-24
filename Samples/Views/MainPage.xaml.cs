using Xamarin.Forms;

namespace Samples.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void SearchPageSample_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new SearchPageSample());
        }
    }
}
