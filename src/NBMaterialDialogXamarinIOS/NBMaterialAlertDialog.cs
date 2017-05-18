using System;
using UIKit;

namespace NBMaterialDialogXamarinIOS
{
    public class NBAlertDialogSettings : NBDialogSettings
    {
        public string Text { get; set; }

        public nfloat FontSize { get; set; } = 14;
    }

    public class NBMaterialAlertDialog : NBMaterialDialog
    {
        public static NBMaterialAlertDialog Show(NBAlertDialogSettings settings)
        {
            var alertLabel = new UILabel();
            alertLabel.Lines = 0;
            alertLabel.Font = UIFontExtensions.RobotoRegularOfSize(settings.FontSize);
            alertLabel.TextColor = NBConfig.PrimaryTextDark;
            alertLabel.Text = settings.Text;
            alertLabel.SizeToFit();

            var dialog = new NBMaterialAlertDialog();

            settings.Content = alertLabel;
            settings.DialogHeight = settings.DialogHeight ?? dialog.kMinimumHeight;

            dialog.ShowDialog(settings);
            return dialog;
        }
    }
}
