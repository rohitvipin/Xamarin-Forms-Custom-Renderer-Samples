using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Samples.CustomRenderers;

[assembly: ExportRenderer(typeof(SearchPage), typeof(Samples.iOS.CustomRenderer.SearchPageRenderer))]
namespace Samples.iOS.CustomRenderer
{
    public class SearchPageRenderer : PageRenderer
    {
        private UIButton _searchButton;
        private UIButton _cancelButton;
        private UIBarButtonItem _searchbarButtonItem;
        private UIView _defaultTitleView;

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            _defaultTitleView = NavigationItem?.TitleView;
            CreateSearchButton();
            CreateSearchToolbar();
            CreateCancelButton();
            DisplayRightBarButton(_searchButton);
        }

        private void CreateSearchButton()
        {
            if (NavigationController?.NavigationBar == null)
            {
                return;
            }
            var height = NavigationController.NavigationBar.Frame.Height;
            var searchButtonView = new UIView(new CGRect(0, 0, 25, height));
            _searchButton = new UIButton(UIButtonType.System)
            {
                BackgroundColor = UIColor.Clear,
                Frame = searchButtonView.Frame,
                AutosizesSubviews = true,
                AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleMargins
            };
            _searchButton.SetTitle("\uD83D\uDD0D", UIControlState.Normal);
            _searchButton.TouchUpInside -= SearchButton_TouchUpInside;
            _searchButton.TouchUpInside += SearchButton_TouchUpInside;
        }

        private void CreateSearchToolbar()
        {
            var element = Element as SearchPage;

            if (element == null || NavigationController?.NavigationBar == null)
            {
                return;
            }

            var width = NavigationController.NavigationBar.Frame.Width;
            var height = NavigationController.NavigationBar.Frame.Height;
            var searchBar = new UIStackView(new CGRect(0, 0, width * 0.75, height))
            {
                Alignment = UIStackViewAlignment.Center,
                Axis = UILayoutConstraintAxis.Horizontal,
                Spacing = 3
            };

            var searchTextField = new UITextField
            {
                BackgroundColor = UIColor.White,
                AttributedPlaceholder = new NSAttributedString(element.SearchPlaceHolderText, foregroundColor: UIColor.Gray),
                Placeholder = element.SearchPlaceHolderText,
            };
            searchTextField.SizeToFit();

            // Delete button
            var textDeleteButton = new UIButton(new CGRect(0, 0, searchTextField.Frame.Size.Height + 5, searchTextField.Frame.Height)) { BackgroundColor = UIColor.Clear };
            textDeleteButton.SetTitleColor(UIColor.FromRGB(146, 146, 146), UIControlState.Normal);
            textDeleteButton.SetTitle("\u24CD", UIControlState.Normal);

            textDeleteButton.TouchUpInside += (sender, e) =>
            {
                searchTextField.Text = string.Empty;
                searchTextField.ResignFirstResponder();
            };

            searchTextField.RightView = textDeleteButton;
            searchTextField.RightViewMode = UITextFieldViewMode.Always;

            // Border
            searchTextField.BorderStyle = UITextBorderStyle.RoundedRect;
            searchTextField.Layer.BorderColor = UIColor.FromRGB(239, 239, 239).CGColor;
            searchTextField.Layer.BorderWidth = 1;
            searchTextField.Layer.CornerRadius = 5;
            searchTextField.EditingChanged += (sender, e) => element.SetValue(SearchPage.SearchTextProperty, searchTextField.Text);
			searchTextField.KeyboardType = UIKeyboardType.Default;
			searchTextField.EditingDidEndOnExit += (sender, e) => element.SearchCommand?.Execute(searchTextField.Text);
            searchBar.AddArrangedSubview(searchTextField);

            _searchbarButtonItem = new UIBarButtonItem(searchBar);
        }

        private void CreateCancelButton()
        {
            var element = Element as SearchPage;
            if (element == null)
            {
                return;
            }
            if (NavigationController?.NavigationBar != null)
            {
                var searchButtonView = new UIView(new CGRect(0, 0, 60, NavigationController.NavigationBar.Frame.Height));
                _cancelButton = new UIButton(UIButtonType.System)
                {
                    BackgroundColor = UIColor.Clear,
                    Frame = searchButtonView.Frame,
                    AutosizesSubviews = true,
                    AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleMargins,
                    HorizontalAlignment = UIControlContentHorizontalAlignment.Right
                };
            }
            _cancelButton.SetTitle("Cancel", UIControlState.Normal);
            _cancelButton.TouchUpInside -= CancelButton_TouchUpInside;
            _cancelButton.TouchUpInside += CancelButton_TouchUpInside;
        }

        private void DisplayRightBarButton(UIView button)
        {
            if (ParentViewController?.NavigationItem != null)
            {
                ParentViewController.NavigationItem.RightBarButtonItem = new UIBarButtonItem(button);
            }
        }

        private void ClearRightToolbarButton()
        {
            if (ParentViewController?.NavigationItem != null)
            {
                ParentViewController.NavigationItem.RightBarButtonItem = null;
            }
        }

        private void SearchButton_TouchUpInside(object sender, System.EventArgs e) => ShowSearchToolbar();

        private void CancelButton_TouchUpInside(object sender, System.EventArgs e)
        {
            ClearRightToolbarButton();
            HideSearchToolbar();
            DisplayRightBarButton(_searchButton);
        }

        private void ShowSearchToolbar()
        {
            ClearRightToolbarButton();
            DisplayRightBarButton(_cancelButton);

            NavigationItem.SetLeftBarButtonItem(_searchbarButtonItem, true);

            NavigationItem.TitleView = new UIView();

            if (ParentViewController?.NavigationItem == null)
            {
                return;
            }
            ParentViewController.NavigationItem.LeftBarButtonItem = NavigationItem.LeftBarButtonItem;
            ParentViewController.NavigationItem.TitleView = NavigationItem.TitleView;
        }

        private void HideSearchToolbar()
        {
            NavigationItem.TitleView = _defaultTitleView;
            if (ParentViewController?.NavigationItem == null)
            {
                return;
            }
            ParentViewController.NavigationItem.LeftBarButtonItem = null;
            ParentViewController.NavigationItem.TitleView = NavigationItem.TitleView;
        }
    }
}
