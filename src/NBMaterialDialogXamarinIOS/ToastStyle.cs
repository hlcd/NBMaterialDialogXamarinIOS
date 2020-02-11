using UIKit;

namespace NBMaterialDialogXamarinIOS
{
    public class ToastStyle
    {
        public static readonly ToastStyle Default = new ToastStyle(UIColorExtensions.FromHex(hex: 0x323232, alpha: 1.0f),
            NBConfig.PrimaryTextLight);

        public ToastStyle(UIColor backgroundColor, UIColor fontColor)
        {
            BackgroundColor = backgroundColor;
            FontColor = fontColor;
        }

        public UIColor BackgroundColor { get; }

        public UIColor FontColor { get; }
    }
}