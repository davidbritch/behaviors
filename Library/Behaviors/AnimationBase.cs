using Xamarin.Forms;

namespace Behaviors
{
	[Preserve(AllMembers = true)]
	public class AnimationBase : BindableObject
	{
		public static readonly BindableProperty DurationProperty = BindableProperty.Create(nameof(Duration), typeof(int), typeof(AnimationBase), 250);
		public static readonly BindableProperty EasingFunctionProperty = BindableProperty.Create(nameof(EasingFunction), typeof(EasingFunction), typeof(AnimationBase), EasingFunction.Linear);
		public static readonly BindableProperty TargetObjectProperty = BindableProperty.Create(nameof(TargetObject), typeof(object), typeof(AnimationBase), null);
		public static readonly BindableProperty AwaitProperty = BindableProperty.Create(nameof(Await), typeof(bool), typeof(AnimationBase), false);

		public int Duration
		{
			get { return (int)GetValue(DurationProperty); }
			set { SetValue(DurationProperty, value); }
		}

		public EasingFunction EasingFunction
		{
			get { return (EasingFunction)GetValue(EasingFunctionProperty); }
			set { SetValue(EasingFunctionProperty, value); }
		}

		public object TargetObject
		{
			get { return (object)GetValue(TargetObjectProperty); }
			set { SetValue(TargetObjectProperty, value); }
		}

		public bool Await
		{
			get { return (bool)GetValue(AwaitProperty); }
			set { SetValue(AwaitProperty, value); }
		}

		protected Easing GetEasingFunction()
		{
			switch (EasingFunction)
			{
				case EasingFunction.BounceIn:
					return Easing.BounceIn;
				case EasingFunction.BounceOut:
					return Easing.BounceOut;
				case EasingFunction.CubicIn:
					return Easing.CubicIn;
				case EasingFunction.CubicOut:
					return Easing.CubicOut;
				case EasingFunction.CubicInOut:
					return Easing.CubicInOut;
				case EasingFunction.Linear:
					return Easing.Linear;
				case EasingFunction.SinIn:
					return Easing.SinIn;
				case EasingFunction.SinOut:
					return Easing.SinOut;
				case EasingFunction.SinInOut:
					return Easing.SinInOut;
				case EasingFunction.SpringIn:
					return Easing.SpringIn;
				case EasingFunction.SpringOut:
					return Easing.SpringOut;
				default:
					return Easing.Linear;
			}
		}
	}
}

