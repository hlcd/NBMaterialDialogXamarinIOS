using UIKit;

namespace UIKit
{
    public static class UIApplicationExtensions
    {
        public static UIView GetTopView(this UIApplication app)
        {
            var rootViewController = app.KeyWindow.RootViewController;
            if (rootViewController.NavigationController != null)
            {
                return rootViewController.NavigationController?.TopViewController.View;
            }
            return rootViewController.View;
        }
    }
}