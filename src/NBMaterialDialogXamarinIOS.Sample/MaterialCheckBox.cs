﻿using CoreGraphics;
using Foundation;
using UIKit;
using static NBMaterialDialogXamarinIOS.Sample.MainViewController;

namespace NBMaterialDialogXamarinIOS.Sample
{
    public sealed class MaterialCheckBox : UIView
    {
        private bool ShouldSetupConstraints { get; set; } = true;

        private UILabel Label { get; set; }

        private UISwitch Checkbox { get; set; }

        public MaterialCheckBox()
        {
            Label = new UILabel
            {
                Lines = 0,
                AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth,
                TranslatesAutoresizingMaskIntoConstraints = false,
                Font = UIFontExtensions.RobotoMediumOfSize(NormalFontSize),
                TextColor = NBConfig.PrimaryTextDark,
                UserInteractionEnabled = true
            };

            AddSubview(Label);

            // Replaced BEMCheckBox with UISwitch after migrating to .Net 6.0
            Checkbox = new UISwitch(new CGRect(0, 0, 25, 25))
            {
                OnTintColor = NBConfig.AccentColor,
            };
            AddSubview(Checkbox);

            var tapGesture = new UITapGestureRecognizer(() => Checkbox.On = !Checkbox.On);
            Label.AddGestureRecognizer(tapGesture);
        }

        public override void UpdateConstraints()
        {
            if (ShouldSetupConstraints)
            {
                var constraintViews = new NSMutableDictionary();
                constraintViews.SetValueForKey(Checkbox, new NSString("Checkbox"));
                constraintViews.SetValueForKey(Label, new NSString("Label"));
                constraintViews.SetValueForKey(this, new NSString("view"));

                AddConstraint(NSLayoutConstraint.Create(Label, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal,
                    this, NSLayoutAttribute.CenterY, 1f, 0f));
                var centerYcheckbox = NSLayoutConstraint.Create(Checkbox, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal,
                                                                this, NSLayoutAttribute.CenterY, 1f, 0f);
                centerYcheckbox.Priority = 250;
                AddConstraint(centerYcheckbox);
                //AddConstraints(NSLayoutConstraint.FromVisualFormat(
                //    "V:|-[Label]-|",
                //    NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
                //           AddConstraints(NSLayoutConstraint.FromVisualFormat(
                //               "V:|-[Checkbox]",
                //NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
                AddConstraints(NSLayoutConstraint.FromVisualFormat(
                    "H:|-[Checkbox]-[Label]-|",
                    NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));

                ShouldSetupConstraints = false;
            }
            base.UpdateConstraints();
        }

        public string Text
        {
            get { return Label.Text; }
            set { Label.Text = value; }
        }

        public bool IsChecked => Checkbox.On;
    }
}