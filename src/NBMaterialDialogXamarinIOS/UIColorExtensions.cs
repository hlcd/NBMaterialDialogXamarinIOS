using UIKit;

namespace NBMaterialDialogXamarinIOS
{
    public static class UIColorExtensions
    {
        public static UIColor FromHex(int hex, float alpha = 1.0f)
        {
            var red = ((hex & 0xFF0000) >> 16) / 255.0f;
            var green = ((hex & 0xFF00) >> 8) / 255.0f;
            var blue = (hex & 0xFF) / 255.0f;
            return new UIColor(red, green, blue, alpha);
        }

    }
}
