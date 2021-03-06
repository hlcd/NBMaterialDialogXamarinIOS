﻿using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using UIKit;

namespace NBMaterialDialogXamarinIOS
{
    public enum NBLunchDuration
    {
        Short = 1,
        Medium = 2,
        Long = 3
    }

    public class NBMaterialToast : UIView
    {
        private nfloat kHorizontalMargin = new nfloat(16.0);
        private nfloat kVerticalBottomMargin = new nfloat(30.0);

        private nfloat kMinHeight = new nfloat(48.0);
        private nfloat kHorizontalPadding = new nfloat(24.0);
        private nfloat kVerticalPadding = new nfloat(16.0);
        private UIFont kFontRoboto = UIFontExtensions.ArialFont(14);

        private NBLunchDuration lunchDuration;
        private bool hasRoundedCorners;
        private NSMutableDictionary constraintViews = new NSMutableDictionary();
        private NSMutableDictionary constraintMetrics = new NSMutableDictionary();

        private Dictionary<NBLunchDuration, double> _durations = new Dictionary<NBLunchDuration, double>
        {
            {NBLunchDuration.Short, 1},
            {NBLunchDuration.Medium, 2 },
            {NBLunchDuration.Long, 3.5}
        };

        public NBMaterialToast()
        {
            Initialize();
        }

        public NBMaterialToast(CGRect frame) : base(frame)
        {
            Initialize();
        }

        private void Initialize()
        {
            lunchDuration = NBLunchDuration.Medium;
            hasRoundedCorners = true;
            UserInteractionEnabled = false;
            BackgroundColor = ToastStyle.Default.BackgroundColor;
        }

        private void Show()
        {
            UIView.Animate(0.2, () => Alpha = 1.0f, Hide);
        }

        private void Hide()
        {
            UIView.Animate(0.8, _durations[lunchDuration], UIViewAnimationOptions.TransitionNone, () => Alpha = 0.0f, RemoveFromSuperview);
        }

        private static NBMaterialToast CreateWithTextAndConstraints(UIView windowView, string text, NBLunchDuration duration, ToastStyle style)
        {
            if (style == null)
                throw new ArgumentNullException(nameof(style));

            var toast = new NBMaterialToast
            {
                lunchDuration = duration,
                Alpha = 0.0f,
                TranslatesAutoresizingMaskIntoConstraints = false,
                BackgroundColor = style.BackgroundColor
            };

            var textLabel = new UILabel
            {
                BackgroundColor = UIColor.Clear,
                TextAlignment = UITextAlignment.Left,
                Font = toast.kFontRoboto,
                TextColor = style.FontColor,
                Lines = 0,
                TranslatesAutoresizingMaskIntoConstraints = false,
                AccessibilityIdentifier = "NbMaterialToastLabel",
                Text = text
            };

            if (toast.hasRoundedCorners)
            {
                toast.Layer.MasksToBounds = true;
                toast.Layer.CornerRadius = 15.0f;
            }

            toast.AddSubview(textLabel);

            windowView.AddSubview(toast);

            toast.constraintViews.SetValueForKey(textLabel, new NSString("textLabel"));
            toast.constraintViews.SetValueForKey(toast, new NSString("toast"));

            var verticalBottomMargin = toast.kVerticalBottomMargin;
            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                var insets = UIApplication.SharedApplication.Windows[0].SafeAreaInsets;
                if (insets.Bottom > 0)
                {
                    verticalBottomMargin += insets.Bottom;
                }
            }

            toast.constraintMetrics.SetValueForKey(new NSNumber(toast.kVerticalPadding), new NSString("vPad"));
            toast.constraintMetrics.SetValueForKey(new NSNumber(toast.kHorizontalPadding), new NSString("hPad"));
            toast.constraintMetrics.SetValueForKey(new NSNumber(toast.kMinHeight), new NSString("minHeight"));
            toast.constraintMetrics.SetValueForKey(new NSNumber(verticalBottomMargin),new NSString("vMargin"));
            toast.constraintMetrics.SetValueForKey(new NSNumber(toast.kHorizontalMargin), new NSString("hMargin"));

            toast.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|-vPad-[textLabel]-vPad-|", NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: toast.constraintMetrics, views: toast.constraintViews));
            toast.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-hPad-[textLabel]-hPad-|", NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: toast.constraintMetrics, views: toast.constraintViews));
            toast.SetContentHuggingPriority(750, UILayoutConstraintAxis.Vertical);
            toast.SetContentHuggingPriority(750, UILayoutConstraintAxis.Horizontal);
            toast.SetContentCompressionResistancePriority(750, UILayoutConstraintAxis.Horizontal);

            windowView.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:[toast(>=minHeight)]-vMargin-|",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: toast.constraintMetrics,
                views: toast.constraintViews));
            windowView.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-(>=hMargin)-[toast]-(>=hMargin)-|", NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: toast.constraintMetrics, views: toast.constraintViews));

            windowView.AddConstraint(NSLayoutConstraint.Create(toast, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal,
                windowView, NSLayoutAttribute.CenterX, multiplier: 1.0f, constant: 0));

            return toast;

        }

        /**
            Displays a classic toast message with a user defined text, shown for a standard period of time
            - parameter windowView: The window which the toast is to be attached
            - parameter text: The message to be displayed
        */
        public static void Show(string text, UIView windowView = null)
        {
            Show(text, NBLunchDuration.Medium, ToastStyle.Default);
        }

        /**
            Displays a classic toast message with a user defined text and duration
            - parameter windowView: The window which the toast is to be attached
            - parameter text: The message to be displayed
            - parameter duration: The duration of the toast
        */
        public static void Show(string text, NBLunchDuration duration, ToastStyle style, UIView windowView = null)
        {
            if (style == null)
                throw new ArgumentNullException(nameof(style));

            if (windowView == null)
            {
                windowView = UIApplication.SharedApplication.GetTopView();
            }

            var toast = CreateWithTextAndConstraints(windowView, text, duration, style);
            toast.Show();
        }

    }
}