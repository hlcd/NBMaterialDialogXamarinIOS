using System;
using System.Diagnostics;
using CoreFoundation;
using CoreGraphics;
using CoreText;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace NBMaterialDialogXamarinIOS
{
    public static class UIFontExtensions
    {
        private static readonly object LoadLock = new object();


        public static void LoadFont(string name, string format)
        {
            var bundle = NSBundle.FromClass(new Class(typeof(NBMaterialCircularActivityIndicator)));
            var fontURL = bundle.GetUrlForResource(name, format);
            var data = NSData.FromUrl(fontURL);

            var provider = new CGDataProvider(data);
            var font = CGFont.CreateFromProvider(provider);

            NSError error;
            if (!CTFontManager.RegisterGraphicsFont(font, out error))
            {
                throw new NSErrorException(error);
            }
        }

        public static UIFont RobotoMediumOfSize(nfloat fontSize)
        {

            var name = "Roboto-Medium";
            return LoadFont(fontSize, name);
        }

        public static UIFont RobotoRegularOfSize(nfloat fontSize)
        {
            var name = "Roboto-Regular";
            return LoadFont(fontSize, name);
        }

        private static UIFont LoadFont(nfloat fontSize, string name, string format = "ttf")
        {
            lock (LoadLock)
            {
                var font = UIFont.FromName(name, fontSize);
                if (font != null)
                    return font;
                LoadFont(name, format);
            }

            return UIFont.FromName(name, fontSize);
        }

        public static UIFont ArialFont(nfloat size) => UIFont.FromName("ArialMT", size);

        public static UIFont MontserratFontSemiBold(nfloat size)
        {
            var name = "Montserrat-SemiBold";
            return LoadFont(size, name, "otf");
        }
    }

}