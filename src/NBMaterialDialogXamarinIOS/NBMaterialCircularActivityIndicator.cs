using System;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;

namespace NBMaterialDialogXamarinIOS
{
    public class NBMaterialCircularActivityIndicator : UIView
    {
        private CAShapeLayer _progressLayer;
        private bool _isAnimating;
        private bool _hidesWhenStopped;
        private CAMediaTimingFunction _timingFunction;
        private NSObject _didBecomeActiveNotificationToken;

        public CAShapeLayer ProgressLayer
        {
            get
            {
                if (_progressLayer == null)
                {
                    _progressLayer = new CAShapeLayer();
                    _progressLayer.StrokeColor = TintColor.CGColor;
                    _progressLayer.FillColor = null;
                    _progressLayer.LineWidth = 3.0f;
                }

                return _progressLayer;
            }
        }

        public bool IsAnimating => _isAnimating;

        /**
            Defines the thickness of the indicator. Change this to make the circular indicator larger
        */
        public nfloat LineWidth
        {
            get { return ProgressLayer.LineWidth; }
            set
            {
                ProgressLayer.LineWidth = value;
                UpdatePath();
            }
        }

        /**
            Defines if the indicator should be hidden when its stopped (usually yes).
        */
        public bool HidesWhenStopped
        {
            get { return _hidesWhenStopped; }
            set
            {
                _hidesWhenStopped = value;
                Hidden = !IsAnimating && _hidesWhenStopped;
            }
        }

        public UIColor IndicatorColor
        {
            get { return TintColor; }
            set { TintColor = value; }
        }

        private const string KNBCircleStrokeAnimationKey = "nbmaterialcircularactivityindicator.stroke";
        private const string KNBCircleRotationAnimationKey = "nbmaterialcircularactivityindicator.rotation";

        public NBMaterialCircularActivityIndicator(CGRect rect) : base(rect)
        {
            Initialize();
        }

        public NBMaterialCircularActivityIndicator(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        private void Initialize()
        {
            TintColor = NBConfig.AccentColor;

            _timingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseInEaseOut);

            Layer.AddSublayer(ProgressLayer);

            _didBecomeActiveNotificationToken = NSNotificationCenter.DefaultCenter.AddObserver(UIApplication.DidBecomeActiveNotification, _ => ResetAnimations());
        }

        protected override void Dispose(bool disposing)
        {
            _didBecomeActiveNotificationToken?.Dispose();
            _didBecomeActiveNotificationToken = null;
            base.Dispose(disposing);

        }


        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            ProgressLayer.Frame = new CGRect(x: 0, y: 0, width: Bounds.Width, height: Bounds.Height);
            UpdatePath();
        }

        public override void TintColorDidChange()
        {
            base.TintColorDidChange();

            ProgressLayer.StrokeColor = TintColor.CGColor;
        }


        /**
            If for some reason you need to reset, call this
        */
        public void ResetAnimations()
        {
            if (IsAnimating)
            {
                StopAnimating();
                StartAnimating();
            }
        }

        /**
        Start or stop animating the indicator
  
        - parameter animate: BOOL
        */
        public void SetAnimating(bool animate)
        {
            if (animate)
                StartAnimating();
            else
            {
                StopAnimating();
            }
        }

        private void StartAnimating()
        {
            if (IsAnimating)
                return;

            CABasicAnimation animation = new CABasicAnimation();
            animation.KeyPath = "transform.rotation";
            animation.Duration = 4.0;
            animation.From = new NSNumber(0.0f);
            animation.To = new NSNumber(2 * Math.PI);
            animation.RepeatCount = float.MaxValue;
            animation.TimingFunction = _timingFunction;
            ProgressLayer.AddAnimation(animation, KNBCircleRotationAnimationKey);

            CABasicAnimation headAnimation = new CABasicAnimation();
            headAnimation.KeyPath = "strokeStart";
            headAnimation.Duration = 1.0;
            headAnimation.From = new NSNumber(0.0);
            headAnimation.To = new NSNumber(0.25);
            headAnimation.TimingFunction = _timingFunction;

            CABasicAnimation tailAnimation = new CABasicAnimation();
            tailAnimation.KeyPath = "strokeEnd";
            tailAnimation.Duration = 1.0;
            tailAnimation.From = new NSNumber(0.0);
            tailAnimation.To = new NSNumber(1.0);
            tailAnimation.TimingFunction = _timingFunction;

            CABasicAnimation endHeadAnimation = new CABasicAnimation();
            endHeadAnimation.KeyPath = "strokeStart";
            endHeadAnimation.BeginTime = 1.0;
            endHeadAnimation.Duration = 0.5;
            endHeadAnimation.From = new NSNumber(0.25);
            endHeadAnimation.To = new NSNumber(1.0);
            endHeadAnimation.TimingFunction = _timingFunction;

            CABasicAnimation endTailAnimation = new CABasicAnimation();
            endTailAnimation.KeyPath = "strokeEnd";
            endTailAnimation.BeginTime = 1.0;
            endTailAnimation.Duration = 0.5;
            endTailAnimation.From = new NSNumber(1.0);
            endTailAnimation.To = new NSNumber(1.0);
            endTailAnimation.TimingFunction = _timingFunction;

            CAAnimationGroup animations = new CAAnimationGroup();
            animations.Duration = 1.5;
            animations.Animations = new CAAnimation[] { headAnimation, tailAnimation, endHeadAnimation, endTailAnimation };
            animations.RepeatCount = float.MaxValue;
            ProgressLayer.AddAnimation(animations, KNBCircleStrokeAnimationKey);

            _isAnimating = true;

            if (HidesWhenStopped)
            {
                Hidden = false;
            }
        }

        private void StopAnimating()
        {
            if (!IsAnimating)
                return;

            ProgressLayer.RemoveAnimation(KNBCircleRotationAnimationKey);
            ProgressLayer.RemoveAnimation(KNBCircleStrokeAnimationKey);
            _isAnimating = false;

            if (HidesWhenStopped)
            {
                Hidden = true;
            }
        }

        private void UpdatePath()
        {
            var center = new CGPoint(x: Bounds.GetMidX(), y: Bounds.GetMidY());
            var radius = new nfloat(Math.Min(Bounds.Width / 2, Bounds.Height / 2) - ProgressLayer.LineWidth / 2);
            var startAngle = new nfloat(0.0);
            var endAngle = new nfloat(2.0 * Math.PI);
            UIBezierPath path = UIBezierPath.FromArc(center, radius, startAngle, endAngle, true);

            ProgressLayer.Path = path.CGPath;
            ProgressLayer.StrokeStart = new nfloat(0.0);
            ProgressLayer.StrokeEnd = new nfloat(0.0);
        }
    }

}