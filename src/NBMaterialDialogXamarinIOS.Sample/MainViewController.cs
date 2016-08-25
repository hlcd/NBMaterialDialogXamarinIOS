using CoreGraphics;
using Foundation;
using UIKit;

namespace NBMaterialDialogXamarinIOS.Sample
{
    public class MainViewController : UIViewController
    {

        public MainViewController()
        {
            View.BackgroundColor = UIColor.White;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            var loadingIndicatoLabel = new UILabel(new CGRect(32,56,195,21));
            loadingIndicatoLabel.Text = "Simple loading indicator:";
            loadingIndicatoLabel.MinimumFontSize = 17;
            loadingIndicatoLabel.SizeToFit();

            View.AddSubview(loadingIndicatoLabel);

            var loadingIndicatorView =
                new NBMaterialCircularActivityIndicator(new CGRect(x: 247, y: 46, width: 48, height: 48));
            loadingIndicatorView.SetAnimating(true);
            View.AddSubview(loadingIndicatorView);

            var showToastButton = new UIButton(new CGRect(32,253, 195, 30));
            showToastButton.SetTitleColor(UIColor.Blue, UIControlState.Normal);
            showToastButton.SetTitle("Display a simple Toast",UIControlState.Normal);
            showToastButton.TouchUpInside +=
                (sender, args) =>
                        NBMaterialToast.ShowWithText(View, "Super awesome toast message, cheers!", NBLunchDuration.Long);

            View.AddSubview(showToastButton);
        }
    }
}