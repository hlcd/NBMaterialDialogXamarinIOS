﻿using System;
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

        private const float IPhoneSmallFontSize = 12f;
        private const float IPadSmallFontSize = 18f;
        private const float IPhoneNormalFontSize = 14f;
        private const float IPadNormalFontSize = 20f;

        protected nfloat SmallFontSize
            =>
            new nfloat(UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad
                ? IPadSmallFontSize
                : IPhoneSmallFontSize);

        protected nfloat NormalFontSize
            =>
            new nfloat(UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad
                ? IPadNormalFontSize
                : IPhoneNormalFontSize);

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
                    var settings = new NBAlertDialogSettings();
                    settings.OkButtonTitle = "OK";
                    settings.Content = CreateAudiobooksHowToDialog();//CreateAudiobooksWelcomeDialog();
                    settings.DialogHeight = 260;


                    var dialog = new NBMaterialAlertDialog();
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
                        "Show loading (3 seconds) (0%)";
                    //dialogSetting.DialogHeight = 200;
                    var loadingDialog = NBMaterialLoadingDialog.Show(dialogSetting);
                    NSTimer.CreateScheduledTimer(1, (_) =>
                    {
                        loadingDialog.UpdateDialogText("Show loading (3 seconds) (33%)");
                    });
                    NSTimer.CreateScheduledTimer(2, (_) =>
                    {
                        loadingDialog.UpdateDialogText("Show loading (3 seconds) (66%)");
                    });

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

        public UIView CreateAudiobooksWelcomeDialog()
        {
            var topView = UIApplication.SharedApplication.GetTopView();
            var dialogWidth = topView.Frame.Width - 80;
            var dialogHeight = 220;

            UIView view = new UIView(new CGRect(0, 0, dialogWidth, dialogHeight));
            view.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
            view.TranslatesAutoresizingMaskIntoConstraints = true;
            view.BackgroundColor = UIColor.Clear;

            var alertLabel = new UILabel();
            alertLabel.Lines = 0;
            alertLabel.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
            alertLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            alertLabel.Font = UIFontExtensions.RobotoRegularOfSize(NormalFontSize);

            alertLabel.TextColor = NBConfig.PrimaryTextDark;
            alertLabel.Text = "Witaj w Legimi";
            alertLabel.SizeToFit();

            view.AddSubview(alertLabel);

            var alertLabel2 = new UILabel();
            // new UILabel(new CGRect(0, alertLabel.Frame.Height + 8,view.Frame.Width - 40, alertLabel.Frame.Height * 2));
            alertLabel2.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
            alertLabel2.TranslatesAutoresizingMaskIntoConstraints = false;
            alertLabel2.Lines = 2;
            alertLabel2.Font = UIFontExtensions.RobotoRegularOfSize(dialogWidth <= 240 ? NormalFontSize-1 : NormalFontSize);
            alertLabel2.TextColor = NBConfig.PrimaryTextDark;
            alertLabel2.Text = "Twojej mobilnej bibliotece, liczącej tysiące tomów";
            alertLabel2.SizeToFit();
            view.AddSubview(alertLabel2);

            var alertLabel3 = new UILabel();
            //new UILabel(new CGRect(0, alertLabel2.Frame.Y + alertLabel2.Frame.Height + 8, view.Frame.Width - 40, 130)));
            alertLabel3.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
            alertLabel3.TranslatesAutoresizingMaskIntoConstraints = false;
            alertLabel3.Lines = 9;
            alertLabel3.Font = UIFontExtensions.RobotoRegularOfSize(dialogWidth <= 240 ? SmallFontSize -1 : SmallFontSize);
            alertLabel3.TextColor = NBConfig.PrimaryTextDark;
            alertLabel3.Text = "Pamiętaj, że zamawiając abonament bez limitu, część książek nie tylko przeczytasz, ale również odsłuchasz dzięki wersji z lektorem lub syntezatorem mowy (sukcesywnie zwiększamy ich liczbę). W każdej chwili możesz oczywiście przełączyć się między trybem czytania i słuchania.";
            alertLabel3.SizeToFit();
            view.AddSubview(alertLabel3);

            var constraintViews = new NSMutableDictionary();
            constraintViews.SetValueForKey(alertLabel, new NSString("alertLabel"));
            constraintViews.SetValueForKey(alertLabel2, new NSString("alertLabel2"));
            constraintViews.SetValueForKey(alertLabel3, new NSString("alertLabel3"));
            constraintViews.SetValueForKey(view, new NSString("view"));
            view.AddConstraints(NSLayoutConstraint.FromVisualFormat(
                "V:|-8-[alertLabel]-8-[alertLabel2]-8-[alertLabel3]",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
            view.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-8-[alertLabel]-8-|",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
            view.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-8-[alertLabel2]-8-|",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
            view.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-8-[alertLabel3]-8-|",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));

            return view;
        }

        private UIView CreateAudiobooksHowToDialog()
        {
            var topView = UIApplication.SharedApplication.GetTopView();
            var dialogWidth = topView.Frame.Width - 80;
            var dialogHeight = 240;
            UIView view = new UIView(new CGRect(0, 0, dialogWidth, dialogHeight));
            view.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;
            view.TranslatesAutoresizingMaskIntoConstraints = true;
            view.BackgroundColor = UIColor.Clear;

            var alertLabel = new UILabel();
            alertLabel.Lines = 0;
            alertLabel.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
            alertLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            alertLabel.TextColor = NBConfig.PrimaryTextDark;
            alertLabel.AttributedText = ParseBoldText("Ta książka posiada również <b>wersję audio.</b> Możesz więc w każdym momencie przełączyć się na tryb słuchania. Zrobisz to w tym miejscu:");

            view.AddSubview(alertLabel);

            var imageView = new UIImageView(UIImage.FromBundle("pasek"));
            imageView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
            imageView.TranslatesAutoresizingMaskIntoConstraints = false;
            imageView.Frame = new CGRect(0, 0, view.Frame.Width - 20, 60);
            imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
            view.AddSubview(imageView);

            var alertLabel2 = new UILabel();
            alertLabel2.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
            alertLabel2.TranslatesAutoresizingMaskIntoConstraints = false;
            alertLabel2.Lines = 0;
            alertLabel2.Font = UIFontExtensions.RobotoRegularOfSize(SmallFontSize);
            alertLabel2.TextColor = NBConfig.PrimaryTextDark;
            alertLabel2.Text = "Gdy tylko zechcesz jednym dotknięciem ekranu wrócisz do czytania. Teraz nic nie przeszkodzi Ci w lekturze.";
            //alertLabel2.SizeToFit();
            view.AddSubview(alertLabel2);

            var constraintViews = new NSMutableDictionary();
            constraintViews.SetValueForKey(alertLabel, new NSString("alertLabel"));
            constraintViews.SetValueForKey(imageView, new NSString("imageView"));
            constraintViews.SetValueForKey(alertLabel2, new NSString("alertLabel2"));
            constraintViews.SetValueForKey(view, new NSString("view"));
            view.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|-8-[alertLabel]-2-[imageView]-2-[alertLabel2]",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
            view.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-8-[alertLabel]-8-|",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
            view.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-8-[alertLabel2]-8-|",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));


            var imageWidth = UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad ? 300f : dialogWidth <= 240f ? 180f :200f;
            var imageHeight = UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad ? 90f : 60f;
            imageView.AddConstraint(NSLayoutConstraint.Create(imageView, NSLayoutAttribute.Width, NSLayoutRelation.Equal,
                null, NSLayoutAttribute.NoAttribute, 1f, imageWidth));
            imageView.AddConstraint(NSLayoutConstraint.Create(imageView, NSLayoutAttribute.Height,
                NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1f, imageHeight));
            view.AddConstraint(NSLayoutConstraint.Create(imageView, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal,
                view, NSLayoutAttribute.CenterX, 1f, 0f));

            return view;
        }


        private NSAttributedString ParseBoldText(string text)
        {
            var pattern = "(<b>(?:(?!<b>).)*</b>)";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            string[] substrings = regex.Split(text);

            var fontSize = SmallFontSize;
            var dictionary = new Dictionary<NSRange, UIStringAttributes>();
            var font = UIFontExtensions.RobotoRegularOfSize(fontSize);
            var sb = new StringBuilder();
            var boldFont = UIFontExtensions.RobotoRegularOfSize(fontSize);
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
    }
}