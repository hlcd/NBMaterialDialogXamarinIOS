using System;
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

            var action =
                new Action<bool>(
                    b =>
                    {
                        string button = b ? "DISAGREE" : "AGREE";
                        NBMaterialToast.ShowWithText(View, $"{button} was clicked",NBLunchDuration.Long);
                    });

            var alertDialogButton = new UIButton(new CGRect(32, 139, 230, 30));
            alertDialogButton.SetTitleColor(UIColor.Blue, UIControlState.Normal);
            alertDialogButton.SetTitle("Show Alert Dialog with title", UIControlState.Normal);
            alertDialogButton.TouchUpInside +=
                (sender, args) =>
                {
                    var settings = new NBAlertDialogSettings
                    {
                        Text = "Simple alert dialog",
                        Title = "Catchy Title",
                        DialogHeight = 160,
                        OkButtonTitle = "AGREE",
                        CancelButtonTitle = "DISAGREE",
                        ButtonAction = action
                    };
                    NBMaterialAlertDialog.ShowAlertWithTextAndTitle(settings);
                };
            View.AddSubview(alertDialogButton);

            var progressDialogButton = new UIButton(new CGRect(32, 177, 230, 30));
            progressDialogButton.SetTitleColor(UIColor.Blue, UIControlState.Normal);
            progressDialogButton.SetTitle("Show loading (3 seconds)", UIControlState.Normal);
            progressDialogButton.TouchUpInside +=
                (sender, args) =>
                {
                    var loadingDialog = NBMaterialLoadingDialog.ShowLoadingDialogWithText("Loading something..");
                    NSTimer.CreateScheduledTimer(3, (_) =>
                    {
                        loadingDialog.HideDialog();
                    });
                };
            View.AddSubview(progressDialogButton);


            var showToastButton = new UIButton(new CGRect(32,215, 195, 30));
            showToastButton.SetTitleColor(UIColor.Blue, UIControlState.Normal);
            showToastButton.SetTitle("Display a simple Toast",UIControlState.Normal);
            showToastButton.TouchUpInside +=
                (sender, args) =>
                        NBMaterialToast.ShowWithText(View, "Super awesome toast message, cheers!", NBLunchDuration.Long);

            View.AddSubview(showToastButton);

            var showSnackbarButton = new UIButton(new CGRect(32, 253, 230, 30));
            showSnackbarButton.SetTitleColor(UIColor.Blue, UIControlState.Normal);
            showSnackbarButton.SetTitle("Display a simple Snackbar", UIControlState.Normal);
            showSnackbarButton.TouchUpInside +=
                (sender, args) =>
                        NBMaterialSnackbar.ShowWithText(UIApplication.SharedApplication.KeyWindow.RootViewController.View, "Super awesome toast message, cheers!", NBLunchDuration.Long);

            View.AddSubview(showSnackbarButton);
        }
    }
}