using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using UIKit;

namespace NBMaterialDialogXamarinIOS
{
    public class NBMaterialSnackbar : UIView
    {
        private Dictionary<NBLunchDuration, double> _durations = new Dictionary<NBLunchDuration, double>
        {
            {NBLunchDuration.Short, 1},
            {NBLunchDuration.Medium, 2 },
            {NBLunchDuration.Long, 3.5}
        };

        private nfloat kMinHeight = 48.0f;
        private nfloat kMaxHeight = 80.0f;
        private nfloat kHorizontalPadding = 24.0f;
        private nfloat kVerticalSinglePadding = 14.0f;
        private nfloat kVerticalMultiPadding = 24.0f;
        private UIFont kFontRoboto = UIFontExtensions.RobotoMediumOfSize(14);
        private UIColor kFontColor = NBConfig.PrimaryTextLight;
        private UIColor kDefaultBackground = UIColorExtensions.FromHex(hex: 0x323232, alpha: 1.0f);

        private NBLunchDuration lunchDuration;
        private bool hasRoundedCorners;
        private NSMutableDictionary constraintViews = new NSMutableDictionary();
        private NSMutableDictionary constraintMetrics = new NSMutableDictionary();

        private NSLayoutConstraint verticalConstraint;

        private nfloat currentHeight;
        private UILabel textLabel;

        public NBMaterialSnackbar()
        {
            Initialize();
        }

        public NBMaterialSnackbar(CGRect frame) : base(frame)
        {
            Initialize();
        }

        private void Initialize()
        {
            lunchDuration = NBLunchDuration.Medium;
            hasRoundedCorners = true;
            UserInteractionEnabled = false;
            BackgroundColor = kDefaultBackground;
        }

        private void Show()
        {
            Superview?.LayoutIfNeeded();
            verticalConstraint.Constant = 0;

            UIView.Animate(0.4, 0.2, UIViewAnimationOptions.TransitionNone, () => { textLabel.Alpha = 1.0f; }, (() =>
            {
            }));

            UIView.Animate(0.4, 0, UIViewAnimationOptions.CurveEaseOut, () => Superview?.LayoutIfNeeded(), Hide);
        }

        private void Hide()
        {
            verticalConstraint.Constant = currentHeight;
            UIView.Animate(0.4, _durations[lunchDuration], UIViewAnimationOptions.TransitionNone, () => Superview?.LayoutIfNeeded(), RemoveFromSuperview);
        }

        private static NBMaterialSnackbar CreateSingleWithTextAndDuration(UIView windowView, string text, NBLunchDuration duration)
        {

            NBMaterialSnackbar snack = new NBMaterialSnackbar();
            snack.lunchDuration = duration;
            snack.TranslatesAutoresizingMaskIntoConstraints = false;
            snack.currentHeight = snack.kMinHeight;

            snack.textLabel = new UILabel();
            snack.textLabel.BackgroundColor = UIColor.Clear;
            snack.textLabel.TextAlignment = UITextAlignment.Left;
            snack.textLabel.Font = snack.kFontRoboto;
            snack.textLabel.TextColor = snack.kFontColor;
            snack.textLabel.Lines = 1;
            snack.textLabel.Alpha = 0.0f;
            snack.textLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            snack.textLabel.Text = text;

            snack.AddSubview(snack.textLabel);

            windowView.AddSubview(snack);

            snack.constraintViews.SetValueForKey(snack.textLabel, new NSString("textLabel"));
            snack.constraintViews.SetValueForKey(snack, new NSString("snack"));

            snack.constraintMetrics.SetValueForKey(new NSNumber(snack.kVerticalSinglePadding), new NSString("vPad"));
            snack.constraintMetrics.SetValueForKey(new NSNumber(snack.kHorizontalPadding), new NSString("hPad"));
            snack.constraintMetrics.SetValueForKey(new NSNumber(snack.kMinHeight), new NSString("minHeight"));

            snack.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|-vPad-[textLabel]-vPad-|",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: snack.constraintMetrics,
                views: snack.constraintViews));
            snack.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-hPad-[textLabel]-hPad-|", NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: snack.constraintMetrics, views: snack.constraintViews));

            windowView.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:[snack(==minHeight)]", NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: snack.constraintMetrics, views: snack.constraintViews));
            windowView.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|[snack]|", NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: snack.constraintMetrics, views: snack.constraintViews));

            snack.verticalConstraint = NSLayoutConstraint.Create((NSObject)snack, (NSLayoutAttribute)NSLayoutAttribute.Bottom, (NSLayoutRelation)NSLayoutRelation.Equal, (NSObject)windowView, (NSLayoutAttribute)NSLayoutAttribute.Bottom, (nfloat)1.0, constant: snack.currentHeight);
            windowView.AddConstraint(snack.verticalConstraint);

            return snack;
        }

        // MARK: - Class functions
        // TODO: Include user actions
        // TODO: Include swipe to dismiss

        /**
        Displays a snackbar (new in material design) at the bottom of the screen
        
        - parameter text: The message to be displayed
        */
        public static void Show(string text, UIView windowView = null)
        {
            NBMaterialSnackbar.Show(text, NBLunchDuration.Medium, windowView);
        }

        /**
        Displays a snackbar (new in material design) at the bottom of the screen
        - parameter text: The message to be displayed
        - parameter duration: The duration of the snackbar
        */
        public static void Show(string text, NBLunchDuration duration, UIView windowView = null)
        {
            if (windowView == null)
            {
                windowView = UIApplication.SharedApplication.GetTopView();
            }
            NBMaterialSnackbar toast = NBMaterialSnackbar.CreateSingleWithTextAndDuration(windowView, text: text,
                duration: duration);
            toast.Show();
        }

    }
}