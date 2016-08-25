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
        private UIView overlay;
        private UILabel titleLabel;
        private UIView containerView = new UIView();
        private UIView contentView = new UIView();
        private BFPaperButton okButton;
        private BFPaperButton cancelButton;
        private UITapGestureRecognizer tapGesture;
        private UIColor backgroundColor;
        private UIView windowView;
        private UIView tappableView = new UIView();

        private bool isStacked = false;

        private nfloat kBackgroundTransparency = 0.7f;
        private nfloat kPadding = 16.0f;
        private nfloat kWidthMargin = 40.0f;
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
        private Action<bool> userAction;
        private NSMutableDictionary constraintViews;
        private nfloat? _dialogHeight;

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
            HideDialog(-1);
        }

        /**
            Hides the dialog, sending a callback if provided when dialog was shown
            :params: buttonIndex The tag index of the button which was clicked
        */

        internal void HideDialog(int buttonIndex)
        {
            if (buttonIndex >= 0)
            {
                userAction?.Invoke(buttonIndex > 0);
            }

            tappableView.RemoveGestureRecognizer(tapGesture);

            foreach (var childView in View.Subviews)
            {
                childView.RemoveFromSuperview();
            }

            View.RemoveFromSuperview();
            strongSelf = null;
        }

        /**
           Displays a simple dialog using a title and a view with the content you need
           - parameter windowView: The window which the dialog is to be attached
           - parameter title: The dialog title
           - parameter content: A custom content view
        */

        public NBMaterialDialog ShowDialog(UIView windowView, string title, UIView content)
        {
            return ShowDialog(windowView, title: title, content: content, dialogHeight: null, okButtonTitle: null,
                action: null, cancelButtonTitle: null, stackedButtons: false);
        }

        /**
           Displays a simple dialog using a title and a view with the content you need
           - parameter windowView: The window which the dialog is to be attached
           - parameter title: The dialog title
           - parameter content: A custom content view
           - parameter dialogHeight: The height of the dialog
        */


        public NBMaterialDialog ShowDialog(UIView windowView, string title, UIView content, nfloat? dialogHeight)
        {
            return ShowDialog(windowView, title: title, content: content, dialogHeight: dialogHeight,
                okButtonTitle: null, action: null, cancelButtonTitle: null, stackedButtons: false);
        }

        /**
            Displays a simple dialog using a title and a view with the content you need
            - parameter windowView: The window which the dialog is to be attached
            - parameter title: The dialog title
            - parameter content: A custom content view
            - parameter dialogHeight: The height of the dialog
            - parameter okButtonTitle: The title of the last button (far-most right), normally OK, CLOSE or YES (positive response).
        */

        public NBMaterialDialog ShowDialog(UIView windowView, string title, UIView content, nfloat? dialogHeight,
            string okButtonTitle)
        {
            return ShowDialog(windowView, title: title, content: content, dialogHeight: dialogHeight,
                okButtonTitle: okButtonTitle, action: null, cancelButtonTitle: null, stackedButtons: false);
        }

        /**
          Displays a simple dialog using a title and a view with the content you need
          - parameter windowView: The window which the dialog is to be attached
          - parameter title: The dialog title
          - parameter content: A custom content view
          - parameter dialogHeight: The height of the dialog
          - parameter okButtonTitle: The title of the last button (far-most right), normally OK, CLOSE or YES (positive response).
          - parameter action: The action you wish to invoke when a button is clicked
        */

        public NBMaterialDialog ShowDialog(UIView windowView, string title, UIView content, nfloat? dialogHeight,
            string okButtonTitle, Action<bool> action)
        {
            return ShowDialog(windowView, title: title, content: content, dialogHeight: dialogHeight,
                okButtonTitle: okButtonTitle, action: action, cancelButtonTitle: null, stackedButtons: false);
        }

        /**
           Displays a simple dialog using a title and a view with the content you need
           - parameter windowView: The window which the dialog is to be attached
           - parameter title: The dialog title
           - parameter content: A custom content view
           - parameter dialogHeight: The height of the dialog
           - parameter okButtonTitle: The title of the last button (far-most right), normally OK, CLOSE or YES (positive response).
           - parameter action: The action you wish to invoke when a button is clicked
        */

        public NBMaterialDialog ShowDialog(UIView windowView, string title, UIView content, nfloat? dialogHeight,
            string okButtonTitle, Action<bool> action, string cancelButtonTitle)
        {

            return ShowDialog(windowView, title: title, content: content, dialogHeight: dialogHeight,
                okButtonTitle: okButtonTitle, action: action, cancelButtonTitle: cancelButtonTitle,
                stackedButtons: false);
        }


        /**
        Displays a simple dialog using a title and a view with the content you need
        - parameter windowView: The window which the dialog is to be attached
        - parameter title: The dialog title
        - parameter content: A custom content view
        - parameter dialogHeight: The height of the dialog
        - parameter okButtonTitle: The title of the last button (far-most right), normally OK, CLOSE or YES (positive response).
        - parameter action: The action you wish to invoke when a button is clicked
        - parameter cancelButtonTitle: The title of the first button (the left button), normally CANCEL or NO (negative response)
        - parameter stackedButtons: Defines if a stackd button view should be used
        */

        public NBMaterialDialog ShowDialog(UIView windowView, string title, UIView content, nfloat? dialogHeight,
            string okButtonTitle, Action<bool> action, string cancelButtonTitle, bool stackedButtons)
        {

            _dialogHeight = dialogHeight;
            isStacked = stackedButtons;

            nfloat totalButtonTitleLength = 0.0f;

            this.windowView = windowView;

            var windowSize = windowView.Bounds;

            windowView.AddSubview(View);
            View.Frame = windowView.Bounds;
            tappableView.Frame = View.Frame;
            tapGesture = new UITapGestureRecognizer(TappedBg);
            tappableView.AddGestureRecognizer(tapGesture);

            SetupContainerView();
            // Add content to contentView
            contentView = content;
            SetupContentView();


            if (title != null)
            {
                SetupTitleLabelWithTitle(title);
            }

            if (okButtonTitle != null)
            {
                UIStringAttributes attribs = new UIStringAttributes { Font = UIFontExtensions.RobotoMediumOfSize(14f) };
                totalButtonTitleLength += new NSString(okButtonTitle.ToUpper()).GetSizeUsingAttributes(attribs).Width + 8;
                if (cancelButtonTitle != null)
                {
                    totalButtonTitleLength +=
                        new NSString(cancelButtonTitle.ToUpper()).GetSizeUsingAttributes(attribs).Width + 8;
                }

                // Calculate if the combined button title lengths are longer than max allowed for this dialog, if so use stacked buttons.
                nfloat buttonTotalMaxLength = (windowSize.Width - (kWidthMargin * 2)) - 16 - 16 - 8;
                if (totalButtonTitleLength > buttonTotalMaxLength)
                {
                    isStacked = true;
                }
            }

            // Always display a close/ok button, but setting a title is optional.
            if (okButtonTitle != null)
            {
                if (okButton == null)
                {
                    okButton = new BFPaperButton();
                    okButton.Tag = 0;
                    okButton.TouchUpInside += (sender, args) => PressedAnyButton(sender as NSObject);
                }
                SetupButtonWithTitle(okButtonTitle, button: okButton, isStacked: isStacked);


            }

            if (cancelButtonTitle != null)
            {
                if (cancelButton == null)
                {
                    cancelButton = new BFPaperButton();
                    cancelButton.Tag = 1;
                    cancelButton.TouchUpInside += (sender, args) => PressedAnyButton(sender as NSObject);
                }
                SetupButtonWithTitle(cancelButtonTitle, button: cancelButton, isStacked: isStacked);


            }

            userAction = action;

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

        private void SetContainerSize()
        {
            if (windowView != null)
            {
                var windowSize = windowView.Bounds;
                containerView.Frame = new CGRect(kWidthMargin, (windowSize.Height - (_dialogHeight ?? kMinimumHeight))/2,
                    windowSize.Width - (kWidthMargin*2), Math.Min(kMaxHeight, (_dialogHeight ?? kMinimumHeight)));
                containerView.ClipsToBounds = true;
                //View.Frame = windowView.Bounds;
            }
        }

        // MARK: - User interaction
        /**
        Invoked when the user taps the background (anywhere except the dialog)
        */
        internal void TappedBg()
        {
            HideDialog(-1);
        }


        /**
        Invoked when a button is pressed
        - parameter sender: The button clicked
        */
        internal void PressedAnyButton(NSObject sender)
        {
            HideDialog((int)(sender as UIButton).Tag);
        }


        private void SetupViewConstraints()
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
                containerView.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:[content]-24-[okButton(==36)]-8-|",
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

        private void SetupContainerView()
        {
            containerView.BackgroundColor = backgroundColor;
            containerView.Layer.CornerRadius = 2.0f;
            containerView.Layer.MasksToBounds = true;
            containerView.Layer.BorderWidth = 0.5f;
            containerView.Layer.BorderColor = UIColorExtensions.FromHex(hex: 0xCCCCCC, alpha: 1.0f).CGColor;
            View.AddSubview(containerView);
        }

        private void SetupTitleLabelWithTitle(string title)
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

        private void SetupButtonWithTitle(string title, BFPaperButton button, bool isStacked)
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


        private void SetupContentView()
        {
            contentView.BackgroundColor = UIColor.Clear;
            contentView.TranslatesAutoresizingMaskIntoConstraints = false;
            containerView.AddSubview(contentView);
        }
    }
}