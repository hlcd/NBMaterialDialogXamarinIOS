using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using UIKit;

namespace NBMaterialDialogXamarinIOS.Sample
{
    public class MainViewController : UIViewController
    {
        private bool IsPad { get; } = UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad;

        private const float IPhoneSmallFontSize = 12f;
        private const float IPadSmallFontSize = 18f;
        private const float IPhoneNormalFontSize = 14f;
        private const float IPadNormalFontSize = 20f;

        protected nfloat SmallFontSize
            =>
            new nfloat(UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad
                ? IPadSmallFontSize
                : IPhoneSmallFontSize);

        public static nfloat NormalFontSize
            =>
            new nfloat(UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad
                ? IPadNormalFontSize
                : IPhoneNormalFontSize);

        public MainViewController()
        {
            View.BackgroundColor = UIColor.White;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            var loadingIndicatoLabel = new UILabel(new CGRect(32, 56, 195, 21));
            loadingIndicatoLabel.Text = "Simple loading indicator:";
            loadingIndicatoLabel.MinimumFontSize = 17;
            loadingIndicatoLabel.SizeToFit();

            View.AddSubview(loadingIndicatoLabel);

            var loadingIndicatorView =
                new NBMaterialCircularActivityIndicator(new CGRect(x: 247, y: 46, width: 48, height: 48));
            loadingIndicatorView.SetAnimating(true);
            View.AddSubview(loadingIndicatorView);

            var action =
                new Action<bool>(
                    b =>
                    {
                        string button = b ? "DISAGREE" : "AGREE";
                        NBMaterialToast.Show($"{button} was clicked", NBLunchDuration.Long);
                    });

            var alertDialogButton = new UIButton(new CGRect(32, 139, 230, 30));
            alertDialogButton.SetTitleColor(UIColor.Blue, UIControlState.Normal);
            alertDialogButton.SetTitle("Show Alert Dialog with title", UIControlState.Normal);
            alertDialogButton.TouchUpInside +=
                async (sender, args) =>
                {
                    var buttons = new List<ButtonOptionItem>();
                    buttons.Add(new ButtonOptionItem("Usuń z kolekcji", "Usuwa pozycję z bieżącej kolekcji, jej pliki pozostają zachowane."));
                    //buttons.Add(new ButtonOptionItem("Usuń dane", "Usuwa pliki z urządzenia. Pozycja w formie niepobranej pozostaje w przypisanych jej kolekcjach."));
                    //buttons.Add(new ButtonOptionItem("Przenieś do kosza", "Usuwa pozycję ze wszystkich kolekcji użytkownika i przenosi ją do kosza, kasując powiązane pliki z urządzenia."));

                    var content = CreateButtonsDescriptionView(buttons);
                    var settings = new NBAlertDialogSettings
                    {
                        Content = content,
                        DialogHeight = content.Frame.Height,
                        DialogWidth = content.Frame.Width,
                        CancelButtonTitle = "Anuluj",
                        HideDialogOnTapOnOverlay = true,
                        RequestedYPosition = 0
                        
                    };




                    //               var settings = new NBAlertDialogSettings();
                    //               //settings.OkButtonTitle = "Ok";
                    //               settings.CancelButtonTitle = "Anuluj";
                    //               settings.Content = CreatePositionChooseView();//CreateUpdateToLectorDialog();//CreateAudiobooksWelcomeDialog();
                    //               settings.DialogHeight = 280;
                    //               settings.DialogWidth = 200;
                    //settings.HideDialogOnTapOnOverlay = true;
                    settings.CancelAction = () =>
                    {
                        NBMaterialToast.Show($"dialog was cancelled", NBLunchDuration.Long);
                    };
                    settings.ButtonAction = (b) =>
                    {
                        NBMaterialToast.Show($"dialog has result", NBLunchDuration.Long);
                    };
                    var dialog = new NBMaterialAlertDialog();
					dialog.ShowDialog(settings);

					//await Task.Delay(TimeSpan.FromSeconds(5));

					//dialog.HideDialog();
                };
            View.AddSubview(alertDialogButton);

            var progressDialogButton = new UIButton(new CGRect(32, 177, 230, 30));
            progressDialogButton.SetTitleColor(UIColor.Blue, UIControlState.Normal);
            progressDialogButton.SetTitle("Show loading (3 seconds)", UIControlState.Normal);
            progressDialogButton.TouchUpInside +=
                (sender, args) =>
                {
                    var dialogSetting = new NBMaterialLoadingDialogSettings();
                    dialogSetting.Text =
                        "Show loading (3 seconds) (0%)";
                    //dialogSetting.DialogHeight = 200;
                    var loadingDialog = NBMaterialLoadingDialog.Show(dialogSetting);
                    NSTimer.CreateScheduledTimer(1, (_) =>
                    {
                        loadingDialog.UpdateDialogText("Show loading (3 seconds) (33%)");
                    });
                    NSTimer.CreateScheduledTimer(2, (_) =>
                    {
                        loadingDialog.UpdateDialogText("Show loading (3 seconds) (66%)");
                    });

                    NSTimer.CreateScheduledTimer(3, (_) =>
                    {
                        loadingDialog.HideDialog();
                    });
                };
            View.AddSubview(progressDialogButton);


            var showToastButton = new UIButton(new CGRect(32, 215, 195, 30));
            showToastButton.SetTitleColor(UIColor.Blue, UIControlState.Normal);
            showToastButton.SetTitle("Display a simple Toast", UIControlState.Normal);
            showToastButton.TouchUpInside +=
                (sender, args) =>
                        NBMaterialToast.Show("Super awesome toast message, cheers!", NBLunchDuration.Long);

            View.AddSubview(showToastButton);

            var showSnackbarButton = new UIButton(new CGRect(32, 253, 230, 30));
            showSnackbarButton.SetTitleColor(UIColor.Blue, UIControlState.Normal);
            showSnackbarButton.SetTitle("Display a simple Snackbar", UIControlState.Normal);
            showSnackbarButton.TouchUpInside +=
                (sender, args) =>
                        NBMaterialSnackbar.Show("Super awesome toast message, cheers!", NBLunchDuration.Long);

            View.AddSubview(showSnackbarButton);

            var pagedDialogButton = new UIButton(new CGRect(32, 291, 230, 30));
            pagedDialogButton.SetTitleColor(UIColor.Blue, UIControlState.Normal);
            pagedDialogButton.SetTitle("Show Paged Dialog", UIControlState.Normal);
            pagedDialogButton.TouchUpInside +=
                (sender, args) =>
                {
                    var pages = new List<NBPageDialogItem>()
                    {
                        //new NBPageDialogItem(CreateWelcomaPageOneView(),240),
                        new NBPageDialogItem(CreateWelcomaPageTwoView(), 300),
                        //new NBPageDialogItem(CreateAudiobooksWelcomeDialog(),300)

                    };
                    var settings = new NBPagedDialogSettings();
                    //settings.OkButtonTitle = "OK";
                    settings.HideDialogOnTapOnOverlay = true;
                    settings.Pages = pages;


                    var dialog = new NBPagedDialog();
                    dialog.ShowPagedDialog(settings);
                };
            View.AddSubview(pagedDialogButton);
        }

        public UIView CreateAudiobooksWelcomeDialog()
        {
            var topView = UIApplication.SharedApplication.GetTopView();
            var dialogWidth = topView.Frame.Width - 80;
            var dialogHeight = 280;

            UIView view = new UIView(new CGRect(0, 0, dialogWidth, dialogHeight));
            view.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
            view.TranslatesAutoresizingMaskIntoConstraints = true;
            view.BackgroundColor = UIColor.Clear;

            var alertLabel = new UILabel();
            alertLabel.Lines = 0;
            alertLabel.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
            alertLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            alertLabel.Font = UIFontExtensions.RobotoRegularOfSize(NormalFontSize);

            alertLabel.TextColor = NBConfig.PrimaryTextDark;
            alertLabel.Text = "Witaj w Legimi";
            alertLabel.SizeToFit();

            view.AddSubview(alertLabel);

            var alertLabel2 = new UILabel();
            // new UILabel(new CGRect(0, alertLabel.Frame.Height + 8,view.Frame.Width - 40, alertLabel.Frame.Height * 2));
            alertLabel2.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
            alertLabel2.TranslatesAutoresizingMaskIntoConstraints = false;
            alertLabel2.Lines = 2;
            alertLabel2.Font = UIFontExtensions.RobotoRegularOfSize(dialogWidth <= 240 ? NormalFontSize - 1 : NormalFontSize);
            alertLabel2.TextColor = NBConfig.PrimaryTextDark;
            alertLabel2.Text = "Twojej mobilnej bibliotece, liczącej tysiące tomów";
            alertLabel2.SizeToFit();
            view.AddSubview(alertLabel2);

            var alertLabel3 = new UILabel();
            //new UILabel(new CGRect(0, alertLabel2.Frame.Y + alertLabel2.Frame.Height + 8, view.Frame.Width - 40, 130)));
            alertLabel3.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
            alertLabel3.TranslatesAutoresizingMaskIntoConstraints = false;
            alertLabel3.Lines = 18;
            alertLabel3.Font = UIFontExtensions.RobotoRegularOfSize(dialogWidth <= 240 ? SmallFontSize - 1 : SmallFontSize);
            alertLabel3.TextColor = NBConfig.PrimaryTextDark;
            alertLabel3.Text = "Pamiętaj, że zamawiając abonament bez limitu, część książek nie tylko przeczytasz, ale również odsłuchasz dzięki wersji z lektorem lub syntezatorem mowy (sukcesywnie zwiększamy ich liczbę). W każdej chwili możesz oczywiście przełączyć się między trybem czytania i słuchania.Pamiętaj, że zamawiając abonament bez limitu, część książek nie tylko przeczytasz, ale również odsłuchasz dzięki wersji z lektorem lub syntezatorem mowy (sukcesywnie zwiększamy ich liczbę). W każdej chwili możesz oczywiście przełączyć się między trybem czytania i słuchania.";
            alertLabel3.SizeToFit();
            view.AddSubview(alertLabel3);

            var constraintViews = new NSMutableDictionary();
            constraintViews.SetValueForKey(alertLabel, new NSString("alertLabel"));
            constraintViews.SetValueForKey(alertLabel2, new NSString("alertLabel2"));
            constraintViews.SetValueForKey(alertLabel3, new NSString("alertLabel3"));
            constraintViews.SetValueForKey(view, new NSString("view"));
            view.AddConstraints(NSLayoutConstraint.FromVisualFormat(
                "V:|-8-[alertLabel]-8-[alertLabel2]-8-[alertLabel3]",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
            view.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-8-[alertLabel]-8-|",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
            view.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-8-[alertLabel2]-8-|",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
            view.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-8-[alertLabel3]-8-|",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));

            return view;
        }

        private UIView CreateAudiobooksHowToDialog()
        {
            var topView = UIApplication.SharedApplication.GetTopView();
            var dialogWidth = topView.Frame.Width - 80;
            var dialogHeight = 240;
            UIView view = new UIView(new CGRect(0, 0, dialogWidth, dialogHeight));
            view.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;
            view.TranslatesAutoresizingMaskIntoConstraints = true;
            view.BackgroundColor = UIColor.Clear;

            var alertLabel = new UILabel();
            alertLabel.Lines = 0;
            alertLabel.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
            alertLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            alertLabel.TextColor = NBConfig.PrimaryTextDark;
            alertLabel.AttributedText = ParseBoldText("Ta książka posiada również <b>wersję audio.</b> Możesz więc w każdym momencie przełączyć się na tryb słuchania. Zrobisz to w tym miejscu:", SmallFontSize);

            view.AddSubview(alertLabel);

            var imageView = new UIImageView(UIImage.FromBundle("pasek"));
            imageView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
            imageView.TranslatesAutoresizingMaskIntoConstraints = false;
            imageView.Frame = new CGRect(0, 0, view.Frame.Width - 20, 60);
            imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
            view.AddSubview(imageView);

            var alertLabel2 = new UILabel();
            alertLabel2.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
            alertLabel2.TranslatesAutoresizingMaskIntoConstraints = false;
            alertLabel2.Lines = 0;
            alertLabel2.Font = UIFontExtensions.RobotoRegularOfSize(SmallFontSize);
            alertLabel2.TextColor = NBConfig.PrimaryTextDark;
            alertLabel2.Text = "Gdy tylko zechcesz jednym dotknięciem ekranu wrócisz do czytania. Teraz nic nie przeszkodzi Ci w lekturze.";
            //alertLabel2.SizeToFit();
            view.AddSubview(alertLabel2);

            var constraintViews = new NSMutableDictionary();
            constraintViews.SetValueForKey(alertLabel, new NSString("alertLabel"));
            constraintViews.SetValueForKey(imageView, new NSString("imageView"));
            constraintViews.SetValueForKey(alertLabel2, new NSString("alertLabel2"));
            constraintViews.SetValueForKey(view, new NSString("view"));
            view.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|-8-[alertLabel]-2-[imageView]-2-[alertLabel2]",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
            view.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-8-[alertLabel]-8-|",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
            view.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-8-[alertLabel2]-8-|",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));


            var imageWidth = UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad ? 300f : dialogWidth <= 240f ? 180f : 200f;
            var imageHeight = UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad ? 90f : 60f;
            imageView.AddConstraint(NSLayoutConstraint.Create(imageView, NSLayoutAttribute.Width, NSLayoutRelation.Equal,
                null, NSLayoutAttribute.NoAttribute, 1f, imageWidth));
            imageView.AddConstraint(NSLayoutConstraint.Create(imageView, NSLayoutAttribute.Height,
                NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1f, imageHeight));
            view.AddConstraint(NSLayoutConstraint.Create(imageView, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal,
                view, NSLayoutAttribute.CenterX, 1f, 0f));

            return view;
        }


        private UIView CreateWelcomaPageOneView()
        {
            var topView = UIApplication.SharedApplication.GetTopView();
            var dialogWidth = topView.Frame.Width - 80;
            var dialogHeight = 240;

            UIView view = new UIView(new CGRect(0, 0, dialogWidth, dialogHeight));
            view.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
            view.TranslatesAutoresizingMaskIntoConstraints = true;
            view.BackgroundColor = UIColor.Clear;

            var alertLabel = new UILabel();
            alertLabel.Lines = 0;
            alertLabel.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
            alertLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            alertLabel.Font = UIFontExtensions.RobotoMediumOfSize(NormalFontSize);

            alertLabel.TextColor = NBConfig.PrimaryTextDark;
            alertLabel.Text = "Witaj w Legimi!";
            alertLabel.SizeToFit();

            view.AddSubview(alertLabel);

            var alertLabel2 = new UILabel();
            //new UILabel(new CGRect(0, alertLabel2.Frame.Y + alertLabel2.Frame.Height + 8, view.Frame.Width - 40, 130)));
            alertLabel2.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
            alertLabel2.TranslatesAutoresizingMaskIntoConstraints = false;
            alertLabel2.Lines = 10;          
            alertLabel2.TextColor = NBConfig.PrimaryTextDark;
            alertLabel2.AttributedText = ParseBoldText("Oddajemy do Twojej dyspozycji ponad <b>20 tysięcy</b> tytułów. Możesz bezpłatnie wypróbować, jak wygodne jest czytanie i słuchanie książek z Legimi. Aktywując dostęp <b>bez limitu +</b> będziesz wygodnie zmieniać format z tekstowego na dźwiękowy, by nigdy nie rozstawać się z książką, która właśnie Cię pochłonęła.", dialogWidth <= 240 ? SmallFontSize - 1 : NormalFontSize);
            alertLabel2.SizeToFit();
            view.AddSubview(alertLabel2);

            var constraintViews = new NSMutableDictionary();
            constraintViews.SetValueForKey(alertLabel, new NSString("alertLabel"));
            constraintViews.SetValueForKey(alertLabel2, new NSString("alertLabel2"));
            constraintViews.SetValueForKey(view, new NSString("view"));
            view.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|-8-[alertLabel]-8-[alertLabel2]",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
            view.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|[alertLabel]|",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
            view.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|[alertLabel2]|",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));

            return view;
        }


        private UIView CreateWelcomaPageTwoView()
        {
            var topView = UIApplication.SharedApplication.GetTopView();
            var dialogWidth = topView.Frame.Width - 80;
            var dialogHeight = 300;

            UIView view = new UIView(new CGRect(0, 0, dialogWidth, dialogHeight));
            view.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
            view.TranslatesAutoresizingMaskIntoConstraints = true;
            view.BackgroundColor = UIColor.Blue;

            var alertLabel = new UILabel();
            alertLabel.Lines = 0;
            alertLabel.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
            alertLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            alertLabel.Font = UIFontExtensions.RobotoRegularOfSize(NormalFontSize);
            alertLabel.TextColor = NBConfig.PrimaryTextDark;
            alertLabel.Text = "Pamiętaj że w Legimi spotkasz następujące formaty książek:";
            alertLabel.SizeToFit();

            view.AddSubview(alertLabel);

            var formatView = new UIView(new CGRect(0, 0, dialogWidth, 40));
            formatView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
            formatView.TranslatesAutoresizingMaskIntoConstraints = false;


            var ebooksView = CreateFormatItemView(dialogWidth, "<b>ebook:</b> format tesktowy + dźwiękowy (syntezator mowy)", UIImage.FromBundle("ebook"));
            var audiobooksView = CreateFormatItemView(dialogWidth, "<b>audiobooki:</b> format dźwiękowy (czytane przez lektora)", UIImage.FromBundle("audiobook"));
            var synchroobookBookView = CreateFormatItemView(dialogWidth, "<b>synchrobooki:</b> format tesktowy + dźwiękowy (lektor)", UIImage.FromBundle("synchrobook"));

            formatView.AddSubview(ebooksView);
            formatView.AddSubview(audiobooksView);
            formatView.AddSubview(synchroobookBookView);

            var formatsConstraints = new NSMutableDictionary();
            formatsConstraints.SetValueForKey(ebooksView, new NSString("ebooksView"));
            formatsConstraints.SetValueForKey(audiobooksView, new NSString("audiobooksView"));
            formatsConstraints.SetValueForKey(synchroobookBookView, new NSString("synchroobookBookView"));
            formatsConstraints.SetValueForKey(formatView, new NSString("formatView"));
            formatView.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|-4-[ebooksView]-4-[audiobooksView]-4-[synchroobookBookView]-4-|",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: formatsConstraints));
            formatView.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|[ebooksView]|",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: formatsConstraints));
            formatView.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|[audiobooksView]|",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: formatsConstraints));
            formatView.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|[synchroobookBookView]|",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: formatsConstraints));

            view.AddSubview(formatView);

            var alertLabel2 = new UILabel();
            //new UILabel(new CGRect(0, alertLabel2.Frame.Y + alertLabel2.Frame.Height + 8, view.Frame.Width - 40, 130)));
            alertLabel2.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
            alertLabel2.TranslatesAutoresizingMaskIntoConstraints = false;
            alertLabel2.Lines = 8;
            alertLabel2.Font = UIFontExtensions.RobotoRegularOfSize(dialogWidth <= 240 ? SmallFontSize - 1 : SmallFontSize);
            alertLabel2.TextColor = NBConfig.PrimaryTextDark;
            alertLabel2.Text = "Użyj filtrów wyszukiwania, by mieć pewność, że dana książka jest dostępna w interesującym Cię formacie. Osoby zamawiające usługę Legimi poprzez operatorów komórkowych mogą korzystać tylko z formatu tekstowego Użyj filtrów wyszukiwania, by mieć pewność, że dana książka jest dostępna w interesującym Cię formacie. Osoby zamawiające usługę Legimi poprzez operatorów komórkowych mogą korzystać tylko z formatu tekstowego.";
            alertLabel2.SizeToFit();
            view.AddSubview(alertLabel2);


            var constraintViews = new NSMutableDictionary();
            constraintViews.SetValueForKey(alertLabel, new NSString("alertLabel"));
            constraintViews.SetValueForKey(alertLabel2, new NSString("alertLabel2"));
            constraintViews.SetValueForKey(formatView, new NSString("formatView"));
            constraintViews.SetValueForKey(view, new NSString("view"));
            view.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|[alertLabel][formatView][alertLabel2]",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
            view.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|[alertLabel]|",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
            view.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|[formatView]|",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
            view.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|[alertLabel2]|",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));

            return view;
        }

        private UIView CreateFormatItemView(nfloat dialogWidth, string description, UIImage image)
        {
            var formatContainerView = new UIView(new CGRect(0, 0, dialogWidth, 40));
            formatContainerView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;
            formatContainerView.TranslatesAutoresizingMaskIntoConstraints = false;

            var formatLabel = new UILabel();
            formatLabel.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
            formatLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            formatLabel.Lines = 2;
            formatLabel.TextColor = NBConfig.PrimaryTextDark;
            formatLabel.AttributedText = ParseBoldText(description, dialogWidth <= 240 ? SmallFontSize-1 : NormalFontSize);
            formatLabel.SizeToFit();
            formatContainerView.AddSubview(formatLabel);

            var formatIcon = new UIImageView(image);
            formatLabel.AutoresizingMask = UIViewAutoresizing.None;
            formatIcon.TranslatesAutoresizingMaskIntoConstraints = true;
            formatIcon.SizeToFit();
            formatContainerView.ContentMode = UIViewContentMode.ScaleAspectFit;
            formatContainerView.AddSubview(formatIcon);

            var formatConstraints = new NSMutableDictionary();
            formatConstraints.SetValueForKey(formatLabel, new NSString("formatLabel"));
            formatConstraints.SetValueForKey(formatIcon, new NSString("formatIcon"));
            formatConstraints.SetValueForKey(formatContainerView, new NSString("formatContainerView"));
            formatContainerView.AddConstraints(NSLayoutConstraint.FromVisualFormat(
                $"H:|-8-[formatIcon]-4-[formatLabel]-4-|",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: formatConstraints));
            formatContainerView.AddConstraints(NSLayoutConstraint.FromVisualFormat($"V:|-4-[formatIcon]-4-|",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: formatConstraints));
            formatContainerView.AddConstraints(NSLayoutConstraint.FromVisualFormat($"V:|-4-[formatLabel]",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: formatConstraints));
            return formatContainerView;
        }

        private NSAttributedString ParseBoldText(string text, nfloat fontSize)
        {
            var pattern = "(<b>(?:(?!<b>).)*</b>)";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            string[] substrings = regex.Split(text);

            var dictionary = new Dictionary<NSRange, UIStringAttributes>();
            var font = UIFontExtensions.RobotoRegularOfSize(fontSize);
            var sb = new StringBuilder();
            var boldFont = UIFontExtensions.RobotoRegularOfSize(fontSize);
            var len = 0;
            foreach (var item in substrings)
            {
                string res = item;
                if (regex.Match(item).Success)
                {
                    res = res.Replace("<b>", "");
                    res = res.Replace("</b>", "");
                    dictionary.Add(new NSRange(len, res.Length), new UIStringAttributes { Font = boldFont });
                }
                else
                {
                    dictionary.Add(new NSRange(len, res.Length), new UIStringAttributes { Font = font });
                }

                len += res.Length;
                sb.Append(res);
            }

            var attributedText = new NSMutableAttributedString(sb.ToString());
            foreach (var attributese in dictionary)
            {
                attributedText.AddAttributes(attributese.Value, attributese.Key);
            }

            return attributedText;
        }

        private UIView CreateUpdateToLectorDialog()
        {
            var topView = UIApplication.SharedApplication.GetTopView();
            var dialogWidth = topView.Frame.Width - 80;
            var dialogHeight = UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad ? 420: 260;
            UIView view = new UIView(new CGRect(0, 0, dialogWidth, dialogHeight));
            view.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;
            view.TranslatesAutoresizingMaskIntoConstraints = true;
            view.BackgroundColor = UIColor.Clear;

            var alertLabel = new UILabel();
            alertLabel.Lines = 0;
            alertLabel.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
            alertLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            alertLabel.Font = UIFontExtensions.RobotoRegularOfSize(NormalFontSize);
            alertLabel.TextColor = NBConfig.PrimaryTextDark;
            alertLabel.Text =
                "Jeśli w przyszłości zechcesz zaktualizować plik do wersji z lektorem, przejdź do zakładki  „Aktualizacja” i pobierz nową wersję.";

            view.AddSubview(alertLabel);

            var imageView = new UIImageView(UIImage.FromBundle("updateToLector"));
            imageView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
            imageView.TranslatesAutoresizingMaskIntoConstraints = false;
            imageView.Frame = new CGRect(0, 0, view.Frame.Width - 20, 100);
            imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
            view.AddSubview(imageView);


            var constraintViews = new NSMutableDictionary();
            constraintViews.SetValueForKey(alertLabel, new NSString("alertLabel"));
            constraintViews.SetValueForKey(imageView, new NSString("imageView"));
            constraintViews.SetValueForKey(view, new NSString("view"));
            view.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|-8-[alertLabel]-2-[imageView]",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
            view.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-8-[alertLabel]-8-|",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));


            var imageWidth = UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad ? 400 : dialogWidth <= 240f ? 160f : 200f;
            var imageHeight = imageWidth * 0.7f;
            imageView.AddConstraint(NSLayoutConstraint.Create(imageView, NSLayoutAttribute.Width, NSLayoutRelation.Equal,
                null, NSLayoutAttribute.NoAttribute, 1f, imageWidth));
            imageView.AddConstraint(NSLayoutConstraint.Create(imageView, NSLayoutAttribute.Height,
                NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1f, imageHeight));
            view.AddConstraint(NSLayoutConstraint.Create(imageView, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal,
                view, NSLayoutAttribute.CenterX, 1f, 0f));

            return view;
        }

		public UIView CreatePositionChooseView()
		{
			var topView = UIApplication.SharedApplication.GetTopView();
		    var dialogWidth = 200;//topView.Frame.Width - 80;
			var dialogHeight = 280;

			UIView view = new UIView(new CGRect(0, 0, dialogWidth, dialogHeight));
			view.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
			view.TranslatesAutoresizingMaskIntoConstraints = true;
			view.BackgroundColor = UIColor.Clear;

			//var contentLabel = new UILabel(new CGRect(0, 0, dialogWidth - 20, 20));
			//contentLabel.Lines = 0;
			//contentLabel.LineBreakMode = UILineBreakMode.WordWrap;
			////contentLabel.PreferredMaxLayoutWidth = dialogWidth;
			//contentLabel.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
			//contentLabel.TranslatesAutoresizingMaskIntoConstraints = false;
			//contentLabel.TextColor = NBConfig.PrimaryTextDark;
			//contentLabel.Font = UIFontExtensions.RobotoRegularOfSize(NormalFontSize);
			//contentLabel.Text = "Od którego miejsca chcesz kontynuować czytanie?";
			//contentLabel.SizeToFit();
			//view.AddSubview(contentLabel);

			var scroll = new UIScrollView(new CGRect(0, 0, dialogWidth, dialogHeight))
			{
				ShowsHorizontalScrollIndicator = false,
				ShowsVerticalScrollIndicator = true,
				BackgroundColor = UIColor.Red
			};
			scroll.Bounces = false;
			scroll.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
			scroll.TranslatesAutoresizingMaskIntoConstraints = false;


			int top = 0;
			int contentHeight = 0;
			int contentWidth = (int)scroll.Frame.Width;
			var itemViewWidth = dialogWidth - 40;
			var itemViewHeight = 10;

			var internalView = new UIView();
			internalView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
			internalView.TranslatesAutoresizingMaskIntoConstraints = false;
			internalView.BackgroundColor = UIColor.Green;

			var itemViews = new List<UIView>();

			for (int i = 0; i < 3; i++)
			{



				var itemView = new UIView();
				itemView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
				itemView.TranslatesAutoresizingMaskIntoConstraints = false;
				var itemTop = 0;

				var alertLabel = new UILabel();
				alertLabel.Lines = 0;
				alertLabel.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
				alertLabel.TranslatesAutoresizingMaskIntoConstraints = false;
				alertLabel.LineBreakMode = UILineBreakMode.WordWrap;
				alertLabel.TextColor = NBConfig.PrimaryTextDark;
				alertLabel.Font = UIFontExtensions.RobotoRegularOfSize(NormalFontSize);
				alertLabel.Text = "Od miejsca, w którym zakonczono na tym urządzeniu ";
				itemView.AddSubview(alertLabel);


				var chapterLabel = new UILabel();
				chapterLabel.Lines = 0;
				chapterLabel.LineBreakMode = UILineBreakMode.WordWrap;
				chapterLabel.TextColor = NBConfig.PrimaryTextDark;
				chapterLabel.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
				chapterLabel.TranslatesAutoresizingMaskIntoConstraints = false;
				chapterLabel.Font = UIFontExtensions.RobotoMediumOfSize(SmallFontSize);
				chapterLabel.Text = "Rozdział 12";
				itemView.AddSubview(chapterLabel);


				var pageLabel = new UILabel();
				pageLabel.Lines = 0;
				pageLabel.LineBreakMode = UILineBreakMode.WordWrap;
				pageLabel.TextColor = NBConfig.PrimaryTextDark;
				pageLabel.Font = UIFontExtensions.RobotoRegularOfSize(SmallFontSize);
				pageLabel.Text = "s. 220";
				pageLabel.TextColor = NBConfig.PrimaryTextDark;
				pageLabel.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
				pageLabel.TranslatesAutoresizingMaskIntoConstraints = false;
				itemView.AddSubview(pageLabel);

				var itemConstraints = new NSMutableDictionary();
				itemConstraints.SetValueForKey(itemView, new NSString("itemView"));
				itemConstraints.SetValueForKey(alertLabel, new NSString("alertLabel"));
				itemConstraints.SetValueForKey(chapterLabel, new NSString("chapterLabel"));
				itemConstraints.SetValueForKey(pageLabel, new NSString("pageLabel"));
				itemView.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|[alertLabel]-4-[chapterLabel]-4-[pageLabel]|",
				NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: itemConstraints));
				itemView.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|[alertLabel]|",
					NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: itemConstraints));
				itemView.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|[chapterLabel]|",
					NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: itemConstraints));
				itemView.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|[pageLabel]|",
					NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: itemConstraints));

				//itemView.AddConstraint(NSLayoutConstraint.Create(alertLabel, NSLayoutAttribute.Width, NSLayoutRelation.Equal,
				//													 itemView, NSLayoutAttribute.Width, 1f, 0f));
				//itemView.AddConstraint(NSLayoutConstraint.Create(chapterLabel, NSLayoutAttribute.Width, NSLayoutRelation.Equal,
				//													 itemView, NSLayoutAttribute.Width, 1f, 0f));
				//itemView.AddConstraint(NSLayoutConstraint.Create(pageLabel, NSLayoutAttribute.Width, NSLayoutRelation.Equal,
				//													 itemView, NSLayoutAttribute.Width, 1f, 0f));

				//internalView.AddConstraint(NSLayoutConstraint.Create(itemView, NSLayoutAttribute.Width, NSLayoutRelation.Equal,
				//													 internalView, NSLayoutAttribute.Width, 1f, 0f));
				itemViews.Add(itemView);
			}

			var internalconstraintViews = new NSMutableDictionary(); 
            internalconstraintViews.SetValueForKey(internalView, new NSString("internalView"));
            
			int x = 1;
			string horizontalFormat = "V:|";
			foreach (var itemView in itemViews)
			{
				var itemId = $"itemView{x}";
				internalView.AddSubview(itemView);
				internalconstraintViews.SetValueForKey(itemView, new NSString(itemId));   
				internalView.AddConstraints(NSLayoutConstraint.FromVisualFormat($"H:|[{itemId}]|",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: internalconstraintViews));
				horizontalFormat += $"[{itemId}]-4-";
				x++;
			}
			horizontalFormat += "|";

			internalView.AddConstraints(NSLayoutConstraint.FromVisualFormat(horizontalFormat,
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: internalconstraintViews));
		
			scroll.AddConstraint(NSLayoutConstraint.Create(internalView, NSLayoutAttribute.Width, NSLayoutRelation.Equal,
			                                                     scroll, NSLayoutAttribute.Width, 1f, 0f));
			

			scroll.AddSubview(internalView);

			var scrollconstraintViews = new NSMutableDictionary();     
			scrollconstraintViews.SetValueForKey(scroll, new NSString("scroll"));     
            scrollconstraintViews.SetValueForKey(internalView, new NSString("internalView"));
            scroll.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|[internalView]|",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: scrollconstraintViews));
            scroll.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|[internalView]|",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: scrollconstraintViews));

			view.AddSubview(scroll);
			var constraintViews = new NSMutableDictionary();
			//constraintViews.SetValueForKey(contentLabel, new NSString("contentLabel"));     
			constraintViews.SetValueForKey(scroll, new NSString("scroll"));     
            constraintViews.SetValueForKey(view, new NSString("view"));
            view.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|[scroll]|",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
            //view.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-4-[contentLabel]-4-|",
            //    NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
			 view.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|[scroll]|",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
			
//			view.AddConstraint(NSLayoutConstraint.Create(scroll, NSLayoutAttribute.Width, NSLayoutRelation.Equal,
//			                                                     null, NSLayoutAttribute.Width, 1f, 100f));


            return view;

        }

        public UIView CreateButtonsDescriptionView(IEnumerable<ButtonOptionItem> items, string checkboxText = null)
        {
            var buttons = items.ToList();
            var topView = UIApplication.SharedApplication.GetTopView();
            var dialogWidth = IsPad ? 600 : topView.Frame.Width - 80;
            var dialogHeight = buttons.Count() * 140;
            //if (IsPad)
            //    DialogWidth = (float)dialogWidth;

            var view = new UIView(new CGRect(0, 0, dialogWidth, dialogHeight))
            {
                AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth,
                TranslatesAutoresizingMaskIntoConstraints = true,
                BackgroundColor = UIColor.Clear
            };

            var checkbox = new MaterialCheckBox
            {
                Text = checkboxText,
                AutoresizingMask = UIViewAutoresizing.FlexibleWidth,
                TranslatesAutoresizingMaskIntoConstraints = false
            };
            checkbox.SetVisibility(() => string.IsNullOrEmpty(checkboxText) == false);
            view.AddSubview(checkbox);

            var scroll = new UIScrollView(new CGRect(0, 0, dialogWidth, dialogHeight))
            {
                ShowsHorizontalScrollIndicator = false,
                ShowsVerticalScrollIndicator = true,
                Bounces = false,
                AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth,
                TranslatesAutoresizingMaskIntoConstraints = false,
                BackgroundColor = UIColor.Red
            };

            var internalView = new UIView
            {
                AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth,
                TranslatesAutoresizingMaskIntoConstraints = false,
            };

            var itemViews = new List<UIView>();
            int j = 0;
            foreach (var item in buttons)
            {
                var index = j;
                var itemView = new UIView
                {
                    AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth,
                    TranslatesAutoresizingMaskIntoConstraints = false,
                    BackgroundColor = UIColor.Green
                };

                var message = new UILabel
                {
                    Lines = 0,
                    AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth,
                    TranslatesAutoresizingMaskIntoConstraints = false,
                    Font = UIFontExtensions.RobotoMediumOfSize(NormalFontSize),
                    TextColor = NBConfig.PrimaryTextDark,
                    Text = item.Description,
                    TextAlignment = UITextAlignment.Center
                };

                itemView.AddSubview(message);

                var actionButton = new UIButton
                {
                    AutoresizingMask = UIViewAutoresizing.FlexibleWidth,
                    TranslatesAutoresizingMaskIntoConstraints = false,
                    BackgroundColor = NBConfig.AccentColor,
                };
                actionButton.SetTitleColor(UIColor.White, UIControlState.Normal);
                actionButton.Layer.CornerRadius = 5;
                actionButton.ClipsToBounds = true;
                actionButton.SetTitle(item.ButtonCaption, UIControlState.Normal);
                actionButton.TitleLabel.Font = UIFontExtensions.RobotoMediumOfSize(NormalFontSize);
                actionButton.TouchUpInside += (sender, e) =>
                {
                    //PublishResult(item, index);
                };
                itemView.AddSubview(actionButton);

                var itemConstraints = new NSMutableDictionary();
                itemConstraints.SetValueForKey(itemView, new NSString("itemView"));
                itemConstraints.SetValueForKey(message, new NSString("message"));
                itemConstraints.SetValueForKey(actionButton, new NSString("actionButton"));
                itemView.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|[actionButton]-[message]-|",
                    NSLayoutFormatOptions.DirectionLeadingToTrailing, null, itemConstraints));
                itemView.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|[message]|",
                    NSLayoutFormatOptions.DirectionLeadingToTrailing, null, itemConstraints));
                itemView.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|[actionButton]|",
                    NSLayoutFormatOptions.DirectionLeadingToTrailing, null, itemConstraints));
                //itemView.AddConstraint(NSLayoutConstraint.Create(message, NSLayoutAttribute., NSLayoutRelation.Equal,
                //                                                        itemView, NSLayoutAttribute.CenterY, 1f, 0f));
                itemViews.Add(itemView);
                j++;
            }

            var internalconstraintViews = new NSMutableDictionary();
            internalconstraintViews.SetValueForKey(internalView, new NSString("internalView"));

            var i = 1;
            var verticalFormat = new StringBuilder("V:|");
            foreach (var itemView in itemViews)
            {
                var itemId = $"itemView{i}";
                internalView.AddSubview(itemView);
                internalconstraintViews.SetValueForKey(itemView, new NSString(itemId));
                internalView.AddConstraints(NSLayoutConstraint.FromVisualFormat($"H:|[{itemId}]|",
                    NSLayoutFormatOptions.DirectionLeadingToTrailing, null, internalconstraintViews));

                if (i < itemViews.Count)
                {
                    var separator = new UIView(new CGRect(0, 0, 0, 10))
                    {
                        AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight,
                        TranslatesAutoresizingMaskIntoConstraints = false,
                        BackgroundColor = UIColorExtensions.FromHex(0xE0E0E0)
                    };
                    internalView.AddSubview(separator);
                    var separatorId = $"separator{i}";
                    internalconstraintViews.SetValueForKey(separator, new NSString(separatorId));
                    internalView.AddConstraints(NSLayoutConstraint.FromVisualFormat($"H:|[{separatorId}]-4-|",
                        NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: internalconstraintViews));

                    internalView.AddConstraint(NSLayoutConstraint.Create(separator, NSLayoutAttribute.Height,
                        NSLayoutRelation.Equal, null, NSLayoutAttribute.Height, 1f, 1f));

                    verticalFormat.Append($"[{itemId}]-2-[{separatorId}]-4-");
                }
                else
                {
                    verticalFormat.Append($"[{itemId}]-2-");
                }

                i++;
            }
            verticalFormat.Append("|");

            internalView.AddConstraints(NSLayoutConstraint.FromVisualFormat(verticalFormat.ToString(),
                NSLayoutFormatOptions.DirectionLeadingToTrailing, null, internalconstraintViews));

            scroll.AddConstraint(NSLayoutConstraint.Create(internalView, NSLayoutAttribute.Width, NSLayoutRelation.Equal,
                scroll, NSLayoutAttribute.Width, 1f, 0f));

            scroll.AddSubview(internalView);

            var scrollconstraintViews = new NSMutableDictionary();
            scrollconstraintViews.SetValueForKey(scroll, new NSString("scroll"));
            scrollconstraintViews.SetValueForKey(internalView, new NSString("internalView"));
            scroll.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|[internalView]|",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, null, scrollconstraintViews));
            scroll.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|[internalView]|",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, null, scrollconstraintViews));


            view.AddSubview(scroll);

            var constraintViews = new NSMutableDictionary();
            constraintViews.SetValueForKey(checkbox, new NSString("Checkbox"));
            constraintViews.SetValueForKey(scroll, new NSString("Scroll"));
            constraintViews.SetValueForKey(view, new NSString("view"));
            view.AddConstraints(NSLayoutConstraint.FromVisualFormat(
                "V:|[Scroll]|",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
            view.AddConstraints(NSLayoutConstraint.FromVisualFormat(
                "H:|[Scroll]|",
                NSLayoutFormatOptions.DirectionLeadingToTrailing, metrics: null, views: constraintViews));
            view.AddConstraint(NSLayoutConstraint.Create(checkbox, NSLayoutAttribute.CenterX,
                                                         NSLayoutRelation.Equal, view, NSLayoutAttribute.CenterX, 1f, 1f));

            return view;
        }

        //public IObservable<DialogListResult<T>> ShowListDialog<T>(IEnumerable<T> listItems, string cancelOption = null, string title = null, bool cancelable = false) where T : DialogListItem
        //{
        //    var items = listItems.ToList();
        //    var dialogHeight = 70 * items.Count;
        //    if (dialogHeight > 220)
        //    {
        //        dialogHeight = 220;
        //    }

        //    var subject = new Subject<DialogListResult<T>>();
        //    var settings = new NBAlertDialogSettings
        //    {
        //        Title = title,
        //        DialogHeight = dialogHeight + 40,
        //        HideDialogOnTapOnOverlay = cancelable
        //    };

        //    var topView = UIApplication.SharedApplication.GetTopView();
        //    var dialogWidth = topView.Frame.Width - 80;


        //    var scroll = new UIScrollView(new CGRect(0, 0, dialogWidth, dialogHeight))
        //    {
        //        ShowsHorizontalScrollIndicator = false,
        //        ShowsVerticalScrollIndicator = true
        //    };


        //    int top = 0;
        //    int contentHeight = 0;
        //    int contentWidth = 0;
        //    int i = 0;
        //    foreach (var listItem in items)
        //    {
        //        var itemViewHeight = 10;
        //        var itemView = new DialogListItemView(new CGRect(0, top, dialogWidth, itemViewHeight));
        //        var item = listItem;

        //        var tapGesture = new UITapGestureRecognizer(() =>
        //        {
        //            subject.OnNext(new DialogListResult<T>(DialogResult.Positive, item));
        //            subject.OnCompleted();
        //        });

        //        itemView.AddGestureRecognizer(tapGesture);

        //        var iosListItem = item as IOSDialogListItem;
        //        var left = 0;
        //        if (iosListItem?.IconResource != null)
        //        {
        //            var itemImage = new UIImageView();
        //            itemImage.Image = UIImage.FromBundle(iosListItem.IconResource);
        //            itemImage.SizeToFit();
        //            itemView.AddSubview(itemImage);
        //            itemViewHeight = (int)itemImage.Frame.Height;
        //            left = (int)itemImage.Frame.Width + 12;
        //        }

        //        var alertLabel = new UILabel();
        //        alertLabel.Lines = 0;
        //        alertLabel.Font = UIFontExtensions.RobotoRegularOfSize(IOSDialogViewProvider.NormalFontSize);
        //        alertLabel.TextColor = NBConfig.PrimaryTextDark;
        //        alertLabel.Text = listItem.Content;
        //        alertLabel.SizeToFit();
        //        alertLabel.Frame = new CGRect(left, 0, dialogWidth - left, alertLabel.Frame.Height);

        //        itemViewHeight = (int)Math.Max(itemViewHeight, alertLabel.Frame.Height);

        //        itemView.Frame = new CGRect(0, top, dialogWidth, itemViewHeight);
        //        if (itemViewHeight > alertLabel.Frame.Height)
        //            alertLabel.CenterVerticalInParent(itemView.Frame);
        //        itemView.AddSubview(alertLabel);

        //        scroll.AddSubview(itemView);
        //        if (i == items.Count - 1)
        //        {
        //            contentHeight = top + (int)itemView.Frame.Height;
        //            contentWidth = (int)alertLabel.Frame.Width;
        //        }

        //        top += itemViewHeight + 8;
        //        i++;
        //    }
        //    scroll.ContentSize = new CGSize(contentWidth, contentHeight);
        //    settings.Content = scroll;

        //    var dialog = new NBMaterialAlertDialog();
        //    dialog.ShowDialog(settings);

        //    return subject.AsObservable()
        //        .Select(res =>
        //        {
        //            dialog.HideDialog();
        //            return res;
        //        });
        //}
    }
}