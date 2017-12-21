using System;

namespace UIKit
{
    // ReSharper disable once InconsistentNaming
    public static class UIViewExtensions
    {
        public static void Show(this UIView view)
        {
            view.Hidden = false;
        }

        public static void Hide(this UIView view)
        {
            view.Hidden = true;
        }

        public static bool IsVisible(this UIView view)
        {
            return view.Hidden == false;
        }

        public static void SetVisibility(this UIView view, Func<bool> predicate)
        {
            if (predicate())
            {
                view.Show();
                return;
            }

            view.Hide();
        }
    }
}