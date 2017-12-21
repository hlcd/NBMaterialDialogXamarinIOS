using System;
using BFPaperButtonXamarinIOS;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace NBMaterialDialogXamarinIOS
{
    /**
        Simple material dialog class
    */

    public class NBMaterialDialog : UIViewController
    {
        // MARK: - Class variables
        private UIView _overlay;
        private UILabel titleLabel;
        protected UIView containerView = new UIView();
        protected UIView contentView = new UIView();
        protected BFPaperButton okButton;
        protected BFPaperButton cancelButton;
        protected UITapGestureRecognizer tapGesture;
        private UIColor backgroundColor;
        protected UIView windowView;
        protected UIView tappableView = new UIView();

        protected bool isStacked = false;

        private nfloat kBackgroundTransparency = 0.7f;
        private nfloat kPadding = 16.0f;
        protected nfloat kWidthMargin = 40.0f;
        private nfloat kHeightMargin = 24.0f;
        protected nfloat kMinimumHeight => 120.0f;


        private nfloat? _kMaxHeight;

        public nfloat kMaxHeight
        {
            get
            {
                if (_kMaxHeight == null)
                {
                    var window = UIScreen.MainScreen.Bounds;
                    _kMaxHeight = window.Height - kHeightMargin - kHeightMargin;
                }
                return _kMaxHeight.Value;
            }
        }

        private NBMaterialDialog strongSelf;
        protected Action<bool> userAction;
        protected NSMutableDictionary constraintViews;
        protected nfloat? _dialogHeight;
        protected nfloat? _dialogWidth;
        protected bool _hideDialogOnTapOnOverlay;
        private Action _cancelAction;

        public NBMaterialDialog()
        {
            Initialize(UIColor.White);
        }

        public NBMaterialDialog(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
            Initialize(UIColor.White);
        }

        private void Initialize(UIColor color)
        {
            View.Frame = UIScreen.MainScreen.Bounds;
            View.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
            View.BackgroundColor = new UIColor(red: 0, green: 0, blue: 0, alpha: kBackgroundTransparency);
            tappableView.BackgroundColor = UIColor.Clear;
            tappableView.Frame = View.Frame;
            View.AddSubview(tappableView);
            backgroundColor = color;
            SetupContainerView();
            //View.AddSubview(containerView);

            //Retaining itself strongly so can exist without strong refrence
            strongSelf = this;
        }

        // Note: - Dialog Lifecycle

        /**
            Hides the dialog
        */

        public void HideDialog()
        {
            HideDialog(-1, false);
        }

        /**
            Hides the dialog, sending a callback if provided when dialog was shown
            :params: buttonIndex The tag index of the button which was clicked
        */

        internal virtual void HideDialog(int buttonIndex, bool userCancelled)
        {
            if (buttonIndex >= 0)
            {
                userAction?.Invoke(buttonIndex > 0);
            }

			if (buttonIndex < 0 && _hideDialogOnTapOnOverlay && userCancelled)
            {
                _cancelAction?.Invoke();
            }

            tappableView.RemoveGestureRecognizer(tapGesture);

            foreach (var childView in View.Subviews)
            {
                childView.RemoveFromSuperview();
            }

            View.RemoveFromSuperview();
            strongSelf = null;
        }

        public virtual NBMaterialDialog ShowDialog(NBDialogSettings settings)
        {
            _hideDialogOnTapOnOverlay = settings.HideDialogOnTapOnOverlay;
            _cancelAction = settings.CancelAction;
            _dialogHeight = settings.DialogHeight;
            _dialogWidth = settings.DialogWidth;
            isStacked = settings.StackedButtons;

            nfloat totalButtonTitleLength = 0.0f;

            windowView = settings.WindowView;

            var windowSize = windowView.Bounds;

            windowView.AddSubview(View);
            View.Frame = windowView.Bounds;
            tappableView.Frame = View.Frame;
            tapGesture = new UITapGestureRecognizer(TappedBg);
            tappableView.AddGestureRecognizer(tapGesture);

            SetupContainerView();
            // Add content to contentView
            contentView = settings.Content;
            SetupContentView();


            if (settings.Title != null)
            {
                SetupTitleLabelWithTitle(settings.Title);
            }

            if (settings.OkButtonTitle != null)
            {
                UIStringAttributes attribs = new UIStringAttributes { Font = UIFontExtensions.RobotoMediumOfSize(14f) };
                totalButtonTitleLength += new NSString(settings.OkButtonTitle.ToUpper()).GetSizeUsingAttributes(attribs).Width + 8;
                if (settings.CancelButtonTitle != null)
                {
                    totalButtonTitleLength +=
                        new NSString(settings.CancelButtonTitle.ToUpper()).GetSizeUsingAttributes(attribs).Width + 8;
                }

                // Calculate if the combined button title lengths are longer than max allowed for this dialog, if so use stacked buttons.
                nfloat buttonTotalMaxLength = (windowSize.Width - (kWidthMargin * 2)) - 16 - 16 - 8;
                if (totalButtonTitleLength > buttonTotalMaxLength)
                {
                    isStacked = true;
                }
            }

            // Always display a close/ok button, but setting a title is optional.
            if (settings.OkButtonTitle != null)
            {
                if (okButton == null)
                {
                    okButton = new BFPaperButton();
                    okButton.Tag = 0;
                    okButton.TouchUpInside += (sender, args) => PressedAnyButton(sender as NSObject);
                }
                SetupButtonWithTitle(settings.OkButtonTitle, button: okButton, isStacked: isStacked);


            }

            if (settings.CancelButtonTitle != null)
            {
                if (cancelButton == null)
                {
                    cancelButton = new BFPaperButton();
                    cancelButton.Tag = 1;
                    cancelButton.TouchUpInside += (sender, args) => PressedAnyButton(sender as NSObject);
                }
                SetupButtonWithTitle(settings.CancelButtonTitle, button: cancelButton, isStacked: isStacked);


            }

            userAction = settings.ButtonAction;

            SetupViewConstraints();

            //// To get dynamic width to work we need to comment this out and uncomment the stuff in setupViewConstraints. But its currently not working..
            SetContainerSize();
            return this;
        }

        // Note: - View lifecycle
        public override void ViewWillLayoutSubviews()
        {
            base.ViewWillLayoutSubviews();
            SetContainerSize();
        }

        protected void SetContainerSize()
        {
            if (windowView != null)
            {
                var windowSize = windowView.Bounds;
                var height = _dialogHeight ?? kMinimumHeight;
                var defaultWidth = windowSize.Width - (kWidthMargin * 2);
                var width = _dialogWidth ?? defaultWidth;
                var leftMargin = width == defaultWidth ? kWidthMargin : (windowSize.Width - width) / 2;
                containerView.Frame = new CGRect(leftMargin, (windowSize.Height - height)/2,
                    width, Math.Min(kMaxHeight, height));
                containerView.ClipsToBounds = true;  
            //    contentView.Frame = new CGRect(0,0,containerView.Frame.Width-48f, contentView.Frame.Height);
                //View.Frame = windowView.Bounds;
            }
        }

        // MARK: - User interaction
        /**
        Invoked when the user taps the background (anywhere except the dialog)
        */
        internal virtual void TappedBg()
        {
            if (_hideDialogOnTapOnOverlay)
                HideDialog(-1, true);
        }


        /**
        Invoked when a button is pressed
        - parameter sender: The button clicked
        */
        internal virtual void PressedAnyButton(NSObject sender)
        {
            HideDialog((int)(sender as UIButton).Tag, false);
        }


        protected void SetupViewConstraints()
        {
            if (constraintViews == null)
            {
                constraintViews = new NSMutableDictionary();
                constraintViews.SetValueForKey(contentView, new NSString("content"));
                constraintViews.SetValueForKey(containerView, new NSString("containerView"));
                constraintViews.SetValueForKey(windowView, new NSString("window"));
            }
            containerView.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-24-[content]-24-|",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, null, constraintViews));
            if (titleLabel != null)
            {
                constraintViews["title"] = titleLabel;
                containerView.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|-24-[title]-24-[content]", NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
                containerView.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-24-[title]-24-|", NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
            }
            else
            {
                containerView.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|-24-[content]", NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
            }

            if (okButton != null || cancelButton != null)
            {
                if (isStacked)
                {
                    SetupStackedButtonsConstraints();
                }
                else
                {
                    SetupButtonConstraints();
                }
            }
            else
            {
                containerView.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:[content]-24-|",
                    NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
            }
            // TODO: Fix constraints for the containerView so we can remove the dialogheight var
            //
            //        let margins = ["kWidthMargin": kWidthMargin, "kMinimumHeight": kMinimumHeight, "kMaxHeight": kMaxHeight]
            //        view.addConstraints(NSLayoutConstraint.constraintsWithVisualFormat("H:|-(>=kWidthMargin)-[containerView(>=80@1000)]-(>=kWidthMargin)-|", options: NSLayoutFormatOptions(0), metrics: margins, views: constraintViews))
            //        view.addConstraints(NSLayoutConstraint.constraintsWithVisualFormat("V:|-(>=kWidthMargin)-[containerView(>=48@1000)]-(>=kWidthMargin)-|", options: NSLayoutFormatOptions(0), metrics: margins, views: constraintViews))
            //        view.addConstraint(NSLayoutConstraint(item: containerView, attribute: NSLayoutAttribute.CenterX, relatedBy: NSLayoutRelation.Equal, toItem: view, attribute: NSLayoutAttribute.CenterX, multiplier: 1, constant: 0))
            //        view.addConstraint(NSLayoutConstraint(item: containerView, attribute: NSLayoutAttribute.CenterY, relatedBy: NSLayoutRelation.Equal, toItem: view, attribute: NSLayoutAttribute.CenterY, multiplier: 1, constant: 0))

        }

        private void SetupButtonConstraints()
        {
            if (okButton != null)
            {
                constraintViews.SetValueForKey(okButton, new NSString("okButton"));
                containerView.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:[content]-[okButton(==36)]-8-|",
                    NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));

                // The cancel button is only shown when the ok button is visible
                if (cancelButton != null)
                {
                    constraintViews.SetValueForKey(cancelButton, new NSString("cancelButton"));
                    containerView.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:[cancelButton(==36)]-8-|",
                        NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
                    containerView.AddConstraints(
                        NSLayoutConstraint.FromVisualFormat("H:[cancelButton(>=64)]-8-[okButton(>=64)]-8-|",
                            NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
                }
                else
                {
                    containerView.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:[okButton(>=64)]-8-|",
                        NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
                }
            }
            else if (cancelButton != null)
            {
                constraintViews.SetValueForKey(cancelButton, new NSString("cancelButton"));
                containerView.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:[content]-[cancelButton(==36)]-8-|",
                    NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
                containerView.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:[cancelButton(>=64)]-8-|",
                    NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
            }
        }

        private void SetupStackedButtonsConstraints()
        {
            constraintViews["okButton"] = okButton;
            constraintViews["cancelButton"] = cancelButton;

            containerView.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:[content]-24-[okButton(==48)]-[cancelButton(==48)]-8-|", NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));

            containerView.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-[okButton]-16-|", NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
            containerView.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-[cancelButton]-16-|", NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
        }

        //   Note: Private view helpers / initializers

        protected void SetupContainerView()
        {
            containerView.BackgroundColor = backgroundColor;
            containerView.Layer.CornerRadius = 2.0f;
            containerView.Layer.MasksToBounds = true;
            containerView.Layer.BorderWidth = 0.5f;
            containerView.Layer.BorderColor = UIColorExtensions.FromHex(hex: 0xCCCCCC, alpha: 1.0f).CGColor;
            View.AddSubview(containerView);
        }

        protected void SetupTitleLabelWithTitle(string title)
        {
            titleLabel = new UILabel();
            if (title != null)
            {
                titleLabel.TranslatesAutoresizingMaskIntoConstraints = false;
                titleLabel.Font = UIFontExtensions.RobotoMediumOfSize(20);
                titleLabel.TextColor = UIColor.FromWhiteAlpha(white: 0.13f, alpha: 1.0f);
                titleLabel.Lines = 0;
                titleLabel.Text = title;
                containerView.AddSubview(titleLabel);
            }
        }

        protected void SetupButtonWithTitle(string title, BFPaperButton button, bool isStacked)
        {

            button.TranslatesAutoresizingMaskIntoConstraints = false;
            button.SetTitle(title.ToUpper(), UIControlState.Normal);
            button.SetTitleColor(NBConfig.AccentColor, UIControlState.Normal);
            button.IsRaised = false;
            if (button.TitleLabel != null)
                button.TitleLabel.Font = UIFontExtensions.RobotoMediumOfSize(14);
            if (isStacked)
            {
                //button.. = UIControlContentHorizontalAlignment.Right;
                button.ContentEdgeInsets = new UIEdgeInsets(0, 0, 0, 20);
            }
            else
            {
                button.ContentEdgeInsets = new UIEdgeInsets(0, 8, 0, 8);
            }

            containerView.AddSubview(button);
        }


        protected void SetupContentView()
        {
            contentView.BackgroundColor = UIColor.Clear;
            contentView.TranslatesAutoresizingMaskIntoConstraints = false;
            containerView.AddSubview(contentView);
        }
    }
}