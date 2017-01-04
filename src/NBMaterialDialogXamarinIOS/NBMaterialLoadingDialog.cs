using System;
using Foundation;
using UIKit;

namespace NBMaterialDialogXamarinIOS
{

    public class NBMaterialLoadingDialogSettings : NBDialogSettings
    {
        public string Text { get; set; }
    }

    public class NBMaterialLoadingDialog : NBMaterialDialog
    {
        public bool dismissOnBgTap = false;

        private UILabel _loadingLabel;

        protected nfloat kMinimumHeight => 72.0f;

        internal void SetLoadingLabel(UILabel label)
        {
            _loadingLabel = label;
        }

        /**
        Displays a loading dialog with a loading spinner, and a message
       
        - parameter message: The message displayed to the user while its loading
        - returns: The Loading Dialog
        */
        public static NBMaterialLoadingDialog Show(NBMaterialLoadingDialogSettings settings)
        {
            var containerView = new UIView();
            var circularLoadingActivity = new NBMaterialCircularActivityIndicator();
            var loadingLabel = new UILabel();

            //circularLoadingActivity.initialize()
            circularLoadingActivity.TranslatesAutoresizingMaskIntoConstraints = false;
            circularLoadingActivity.LineWidth = 3.5f;
            circularLoadingActivity.TintColor = NBConfig.AccentColor;

            containerView.AddSubview(circularLoadingActivity);

            loadingLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            loadingLabel.Font = UIFontExtensions.RobotoRegularOfSize(14);
            loadingLabel.TextColor = NBConfig.PrimaryTextDark;
            loadingLabel.Text = settings.Text;
            // TODO: Add support for multiple lines, probably need to fix the dynamic dialog height todo first
            loadingLabel.Lines = 0;

            containerView.AddSubview(loadingLabel);


            // Setup constraints
            NSMutableDictionary constraintViews = new NSMutableDictionary();
            constraintViews.SetValueForKey(circularLoadingActivity, new NSString("spinner"));
            constraintViews.SetValueForKey(loadingLabel, new NSString("label"));

            containerView.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|[spinner(==32)]-16-[label]|", NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
            containerView.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:[spinner(==32)]", NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
            // Center Y needs to be set manually, not through VFL
            containerView.AddConstraint(
                NSLayoutConstraint.Create(
                    circularLoadingActivity,
                    NSLayoutAttribute.CenterY,
                    NSLayoutRelation.Equal,
                    containerView,
                    NSLayoutAttribute.CenterY,
                    multiplier: 1,
                    constant: 0));
            containerView.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|[label]|",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));

            // Initialize dialog and display
            var dialog = new NBMaterialLoadingDialog();
            settings.Content = containerView;
            settings.DialogHeight = settings.DialogHeight ?? dialog.kMinimumHeight;
            dialog.SetLoadingLabel(loadingLabel);
            dialog.ShowDialog(settings);

            // Start spinner
            circularLoadingActivity.StartAnimating();

            return dialog;
        }

        public void UpdateDialogText(string text)
        {
            if(_loadingLabel == null)
                return;
            _loadingLabel.Text = text;
        }

        internal override void TappedBg()
        {
            if (dismissOnBgTap)
                base.TappedBg();
        }
    }
}