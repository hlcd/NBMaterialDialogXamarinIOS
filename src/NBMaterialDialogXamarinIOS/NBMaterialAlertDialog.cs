using System;
using UIKit;

namespace NBMaterialDialogXamarinIOS
{
    public class NBMaterialAlertDialog : NBMaterialDialog
    {
        /**
           Displays an alert dialog with a simple text and buttons
           - parameter windowView: The window which the dialog is to be attached
           - parameter text: The main alert message
           - parameter okButtonTitle: The positive button if multiple buttons, or a dismiss action button if one button. Usually either OK or CANCEL (of only one button)
           - parameter action: The block you want to run when the user clicks any of the buttons. If no block is given, the standard dismiss action will be used
           - parameter cancelButtonTitle: The negative button when multiple buttons.
        */

        public static NBMaterialAlertDialog ShowAlertWithText(UIView windowView, string text, string okButtonTitle,
            Action<bool> action, string cancelButtonTitle)
        {
            return NBMaterialAlertDialog.ShowAlertWithTextAndTitle(windowView, text: text, title: null,
                dialogHeight: null,
                okButtonTitle: okButtonTitle, action: action, cancelButtonTitle: cancelButtonTitle);
        }
        
        /**
          Displays an alert dialog with a simple text, title and buttons.
          Remember to read Material guidelines on when to include a dialog title.
          - parameter windowView: The window which the dialog is to be attached
          - parameter text: The main alert message
          - parameter title: The title of the alert
          - parameter okButtonTitle: The positive button if multiple buttons, or a dismiss action button if one button. Usually either OK or CANCEL (of only one button)
          - parameter action: The block you want to run when the user clicks any of the buttons. If no block is given, the standard dismiss action will be used
          - parameter cancelButtonTitle: The negative button when multiple buttons.
        */

        public static NBMaterialAlertDialog ShowAlertWithTextAndTitle(UIView windowView,string text, string title, nfloat? dialogHeight, string okButtonTitle, Action<bool> action, string cancelButtonTitle)
        {
            var alertLabel = new UILabel();
            alertLabel.Lines = 0;
            alertLabel.Font = UIFontExtensions.RobotoRegularOfSize(14);
            alertLabel.TextColor = NBConfig.PrimaryTextDark;
            alertLabel.Text = text;
            alertLabel.SizeToFit();

            var dialog = new NBMaterialAlertDialog();
            dialog.ShowDialog(windowView, title: title, content: alertLabel,
                dialogHeight: dialogHeight ?? dialog.kMinimumHeight, okButtonTitle: okButtonTitle, action: action,
                cancelButtonTitle: cancelButtonTitle);
            return dialog;
        }
}
}
