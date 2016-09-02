using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
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
            var loadingIndicatoLabel = new UILabel(new CGRect(32, 56, 195, 21));
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
                        NBMaterialToast.Show($"{button} was clicked", NBLunchDuration.Long);
                    });

            var alertDialogButton = new UIButton(new CGRect(32, 139, 230, 30));
            alertDialogButton.SetTitleColor(UIColor.Blue, UIControlState.Normal);
            alertDialogButton.SetTitle("Show Alert Dialog with title", UIControlState.Normal);
            alertDialogButton.TouchUpInside +=
                (sender, args) =>
                {

                    //var settings = new NBAlertDialogSettings
                    //{
                    //    Text = "Simple alert dialog",
                    //    Title = "Catchy Title",
                    //    DialogHeight = 160,
                    //    OkButtonTitle = "AGREE",
                    //    CancelButtonTitle = "DISAGREE",
                    //    ButtonAction = action
                    //};
                    //NBMaterialAlertDialog.Show(settings);

                    UIView view = new UIView(new CGRect(0, 0, View.Frame.Width - 80, 240));
                    view.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;
                    view.TranslatesAutoresizingMaskIntoConstraints = true;
                    view.BackgroundColor = UIColor.Clear;

                    var settings = new NBDialogSettings();
                    settings.OkButtonTitle = "OK";


                    var alertLabel = new UILabel();
                    alertLabel.Lines = 0;
                    alertLabel.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
                    alertLabel.TranslatesAutoresizingMaskIntoConstraints = false;
                    //alertLabel.Font =
                    //    UIFontExtensions.RobotoRegularOfSize(UIDevice.CurrentDevice.UserInterfaceIdiom ==
                    //                                         UIUserInterfaceIdiom.Pad
                    //        ? 18
                    //        : 12);
                    alertLabel.TextColor = NBConfig.PrimaryTextDark;
                    //alertLabel.Text =
                    //    "Ta książka posiada również <b>wersję audio.</b> Możesz więc w każdym momencie przełączyć się na tryb słuchania. Zrobisz to w tym miejscu:";
                    alertLabel.AttributedText =
                        AttributedString(
                            "Ta książka posiada również <b>wersję audio.</b> Możesz więc w każdym momencie przełączyć się na tryb słuchania. Zrobisz to w tym miejscu:");


                    //alertLabel.SizeToFit();

                    view.AddSubview(alertLabel);

                    var imageView = new UIImageView(UIImage.FromBundle("pasek"));
                    imageView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
                    imageView.TranslatesAutoresizingMaskIntoConstraints = false;
                    imageView.Frame = new CGRect(0, 0, view.Frame.Width - 20, 60);
                    imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
                    view.AddSubview(imageView);

                    var alertLabel2 = new UILabel();// new UILabel(new CGRect(0, alertLabel.Frame.Height + 8,view.Frame.Width - 40, alertLabel.Frame.Height * 2));
                    alertLabel2.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
                    alertLabel2.TranslatesAutoresizingMaskIntoConstraints = false;
                    alertLabel2.Lines = 0;
                    alertLabel2.Font = UIFontExtensions.RobotoRegularOfSize(UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad ? 18 : 12);
                    alertLabel2.TextColor = NBConfig.PrimaryTextDark;
                    alertLabel2.Text = "Gdy tylko zechcesz jednym dotknięciem ekranu wrócisz do czytania. Teraz nic nie przeszkodzi Ci w lekturze.";
                    //alertLabel2.SizeToFit();
                    view.AddSubview(alertLabel2);

                    var constraintViews = new NSMutableDictionary();
                    constraintViews.SetValueForKey(alertLabel, new NSString("alertLabel"));
                    constraintViews.SetValueForKey(imageView, new NSString("imageView"));
                    constraintViews.SetValueForKey(alertLabel2, new NSString("alertLabel2"));
                    constraintViews.SetValueForKey(view, new NSString("view"));
                    view.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|-8-[alertLabel]-2-[imageView]-2-[alertLabel2]", NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
                    view.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-8-[alertLabel]-8-|", NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews)); view.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-8-[alertLabel]-8-|", NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews)); view.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-8-[alertLabel]-8-|", NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
                    view.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-8-[alertLabel2]-8-|", NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews)); view.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-8-[alertLabel]-8-|", NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews)); view.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-8-[alertLabel]-8-|", NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
                    //view.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-8-[imageView]-8-|", NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews)); view.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-8-[alertLabel]-8-|", NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews)); view.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-8-[alertLabel]-8-|", NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));

                    var imageWidth = UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad ? 300f : 200f;
                    var imageHeight = UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad ? 90f : 60f;
                    imageView.AddConstraint(NSLayoutConstraint.Create(imageView, NSLayoutAttribute.Width, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1f, imageWidth));
                    imageView.AddConstraint(NSLayoutConstraint.Create(imageView, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1f, imageHeight));
                    view.AddConstraint(NSLayoutConstraint.Create(imageView, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, view,NSLayoutAttribute.CenterX,1f,0f));

                    //view.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-8-[alertLabel2]-8-|", NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
                    //view.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-8-[alertLabel3]-8-|", NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
                    // view.AddConstraint(NSLayoutConstraint.Create(
                    //alertLabel3,
                    //NSLayoutAttribute.Width,
                    //NSLayoutRelation.Equal,
                    //view,
                    //NSLayoutAttribute.Width,
                    //multiplier: 1,
                    //constant: 20));

                    //view.SizeToFit();

                    var dialog = new NBMaterialAlertDialog();


                    settings.Content = view;
                    settings.DialogHeight = 280;

                    dialog.ShowDialog(settings);
                };
            View.AddSubview(alertDialogButton);

            var progressDialogButton = new UIButton(new CGRect(32, 177, 230, 30));
            progressDialogButton.SetTitleColor(UIColor.Blue, UIControlState.Normal);
            progressDialogButton.SetTitle("Show loading (3 seconds)", UIControlState.Normal);
            progressDialogButton.TouchUpInside +=
                (sender, args) =>
                {
                    var dialogSetting = new NBMaterialLoadingDialogSettings();
                    dialogSetting.Text =
                        "Trwa aktualizacja aplikacji. Może to potrwać do kilku minut, ale gwarantujemy, że warto poczekać, by skorzystać z nowych możliwości dostępu do książek z Legimi.";
                    dialogSetting.DialogHeight = 200;
                    var loadingDialog = NBMaterialLoadingDialog.Show(dialogSetting);
                    NSTimer.CreateScheduledTimer(3, (_) =>
                    {
                        loadingDialog.HideDialog();
                    });
                };
            View.AddSubview(progressDialogButton);


            var showToastButton = new UIButton(new CGRect(32, 215, 195, 30));
            showToastButton.SetTitleColor(UIColor.Blue, UIControlState.Normal);
            showToastButton.SetTitle("Display a simple Toast", UIControlState.Normal);
            showToastButton.TouchUpInside +=
                (sender, args) =>
                        NBMaterialToast.Show("Super awesome toast message, cheers!", NBLunchDuration.Long);

            View.AddSubview(showToastButton);

            var showSnackbarButton = new UIButton(new CGRect(32, 253, 230, 30));
            showSnackbarButton.SetTitleColor(UIColor.Blue, UIControlState.Normal);
            showSnackbarButton.SetTitle("Display a simple Snackbar", UIControlState.Normal);
            showSnackbarButton.TouchUpInside +=
                (sender, args) =>
                        NBMaterialSnackbar.Show("Super awesome toast message, cheers!", NBLunchDuration.Long);

            View.AddSubview(showSnackbarButton);
        }

        private NSAttributedString AttributedString(string text)
        {
            var pattern = "(<b>(?:(?!<b>).)*</b>)";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            string[] substrings = regex.Split(text);

            var fontSize = UIDevice.CurrentDevice.UserInterfaceIdiom ==
                           UIUserInterfaceIdiom.Pad
                ? 18
                : 12;
            var dictionary = new Dictionary<NSRange, UIStringAttributes>();
            var font = UIFontExtensions.RobotoRegularOfSize(fontSize);
            var sb = new StringBuilder();
            var boldFont = UIFont.BoldSystemFontOfSize(fontSize);
            var len = 0;
            foreach (var item in substrings)
            {
                string res = item;
                if (regex.Match(item).Success)
                {
                    res = res.Replace("<b>", "");
                    res = res.Replace("</b>", "");
                    dictionary.Add(new NSRange(len, res.Length), new UIStringAttributes { Font = boldFont });
                }
                else
                {
                    dictionary.Add(new NSRange(len, res.Length), new UIStringAttributes { Font = font });
                }

                len += res.Length;
                sb.Append(res);
            }


            var attributedText = new NSMutableAttributedString(sb.ToString());
            foreach (var attributese in dictionary)
            {
                attributedText.AddAttributes(attributese.Value, attributese.Key);
            }

            return attributedText;
        }

        private static NSAttributedString GetAttributedStringFromHtml(string html)
        {
            NSError error = null;
            var htmlString = new NSAttributedString(html, new NSAttributedStringDocumentAttributes
            {
                DocumentType = NSDocumentType.HTML,
                StringEncoding = NSStringEncoding.UTF8,
            },
                ref error);
            //htmlString.Get
            return htmlString;
        }
    }
}