using System;
using UIKit;

namespace NBMaterialDialogXamarinIOS
{
    public class NBDialogSettings
    {


        private UIView _windowView;

        public UIView WindowView
        {
            get { return _windowView ?? GetTopView(); }
            set { _windowView = value; }
        }

        public string Title { get; set; }

        public UIView Content { get; set; }

        public nfloat? DialogHeight { get; set; }

        public string OkButtonTitle { get; set; }

        public string CancelButtonTitle { get; set; }

        public Action<bool> ButtonAction { get; set; }

        public bool StackedButtons { get; set; }

        public bool HideDialogOnTapOnOverlay { get; set; }

        private UIView GetTopView()
        {
            var rootViewController = UIApplication.SharedApplication.KeyWindow.RootViewController;
            if (rootViewController.NavigationController != null)
            {
                return rootViewController.NavigationController?.TopViewController.View;
            }
            return rootViewController.View;
        }
    }
}