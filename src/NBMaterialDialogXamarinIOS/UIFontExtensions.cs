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


        public static void LoadFont(string name)
        {
            var bundle = NSBundle.FromClass(new Class(typeof(NBMaterialCircularActivityIndicator)));
            var fontURL = bundle.GetUrlForResource(name, "ttf");
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

        private static UIFont LoadFont(nfloat fontSize, string name)
        {
            lock (LoadLock)
            {
                try
                {
                    var font = UIFont.FromName(name, fontSize);
                    if (font != null)
                        return font;
                    LoadFont(name);
                }
                catch (Exception)
                {
                    Debug.WriteLine("Unable to load font");
                }
            }

            return UIFont.FromName(name, fontSize);
        }
    }

}