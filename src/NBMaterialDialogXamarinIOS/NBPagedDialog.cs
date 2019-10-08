using System;
using System.Collections.Generic;
using System.Linq;
using BFPaperButtonXamarinIOS;
using Foundation;
using UIKit;

namespace NBMaterialDialogXamarinIOS
{
    public class NBPageDialogItem
    {
        public NBPageDialogItem(UIView view, nfloat dialogHeight)
        {
            View = view;
            DialogHeight = dialogHeight;
        }

        public UIView View { get; }

        public nfloat DialogHeight { get; }
    }

    public class NBPagedDialogSettings : NBDialogSettings
    {
        public IEnumerable<NBPageDialogItem> Pages { get; set; }
    }

    public class NBPagedDialog : NBMaterialDialog
    {
        private int _currentPage = -1;
        private List<NBPageDialogItem> _pages = new List<NBPageDialogItem>();

        internal override void PressedAnyButton(NSObject sender)
        {
            if (_pages.Count == 0 || _currentPage == _pages.Count - 1)
            {
                HideDialog((int) (sender as UIButton).Tag, false);
            }
            else
            {
                ShowNextPage();
            }
        }

        private void ShowNextPage()
        {
            _currentPage++;
            _dialogHeight = _pages[_currentPage].DialogHeight;
            containerView.RemoveFromSuperview();
            constraintViews.Clear();
            constraintViews = null;
            contentView.RemoveFromSuperview();

            SetupContainerView();
            // Add content to contentView

            contentView = _pages[_currentPage].View;
            SetupContentView();

            SetupViewConstraints();
            //// To get dynamic width to work we need to comment this out and uncomment the stuff in setupViewConstraints. But its currently not working..
            SetContainerSize();
        }

        public NBMaterialDialog ShowPagedDialog(NBPagedDialogSettings settings)
        {
            _currentPage = 0;
            _pages = settings.Pages.ToList();

            return ShowDialog(settings);
        }

        public override NBMaterialDialog ShowDialog(NBDialogSettings settings)
        {
            _hideDialogOnTapOnOverlay = settings.HideDialogOnTapOnOverlay;
            _dialogHeight = _pages[_currentPage].DialogHeight;
            isStacked = settings.StackedButtons;

            nfloat totalButtonTitleLength = 0.0f;

            windowView = settings.WindowView;

            var windowSize = windowView.Bounds;

            windowView.AddSubview(View);
            View.Frame = windowView.Bounds;
            tappableView.Frame = View.Frame;
            tapGesture = new UITapGestureRecognizer(TappedBg);
            tappableView.AddGestureRecognizer(tapGesture);

            SetupContainerView();
            // Add content to contentView
            contentView = _pages[_currentPage].View;
            SetupContentView();


            if (settings.Title != null)
            {
                SetupTitleLabelWithTitle(settings.Title);
            }

            if (settings.OkButtonTitle != null)
            {
                UIStringAttributes attribs = new UIStringAttributes { Font = UIFontExtensions.MontserratFontSemiBold(14f) };
                totalButtonTitleLength += new NSString(settings.OkButtonTitle.ToUpper()).GetSizeUsingAttributes(attribs).Width + 8;
                if (settings.CancelButtonTitle != null)
                {
                    totalButtonTitleLength +=
                        new NSString(settings.CancelButtonTitle.ToUpper()).GetSizeUsingAttributes(attribs).Width + 8;
                }

                // Calculate if the combined button title lengths are longer than max allowed for this dialog, if so use stacked buttons.
                nfloat buttonTotalMaxLength = (windowSize.Width - (kWidthMargin * 2)) - 16 - 16 - 8;
                if (totalButtonTitleLength > buttonTotalMaxLength)
                {
                    isStacked = true;
                }
            }

            // Always display a close/ok button, but setting a title is optional.
            if (settings.OkButtonTitle != null)
            {
                if (okButton == null)
                {
                    okButton = new BFPaperButton();
                    okButton.Tag = 0;
                    okButton.TouchUpInside += (sender, args) => PressedAnyButton(sender as NSObject);
                }
                SetupButtonWithTitle(settings.OkButtonTitle, button: okButton, isStacked: isStacked);
            }

            if (settings.CancelButtonTitle != null)
            {
                if (cancelButton == null)
                {
                    cancelButton = new BFPaperButton();
                    cancelButton.Tag = 1;
                    cancelButton.TouchUpInside += (sender, args) => PressedAnyButton(sender as NSObject);
                }
                SetupButtonWithTitle(settings.CancelButtonTitle, button: cancelButton, isStacked: isStacked);


            }

            userAction = settings.ButtonAction;

            SetupViewConstraints();

            //// To get dynamic width to work we need to comment this out and uncomment the stuff in setupViewConstraints. But its currently not working..
            SetContainerSize();
            return this;
        }
    }
}