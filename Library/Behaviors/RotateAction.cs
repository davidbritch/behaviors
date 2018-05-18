using System.Threading.Tasks;
using Xamarin.Forms;

#pragma warning disable 4014

namespace Behaviors
{
	[Preserve(AllMembers = true)]
	public class RotateAction : AnimationBase, IAction
	{
		public static readonly BindableProperty FinalAngleProperty = BindableProperty.Create(nameof(FinalAngle), typeof(double), typeof(RotateAction), 0.0);
		public static readonly BindableProperty IsRelativeProperty = BindableProperty.Create(nameof(IsRelative), typeof(bool), typeof(RotateAction), false);
		public static readonly BindableProperty AxisProperty = BindableProperty.Create(nameof(Axis), typeof(RotationAxis), typeof(RotateAction), RotationAxis.Z);

		public double FinalAngle
		{
			get { return (double)GetValue(FinalAngleProperty); }
			set { SetValue(FinalAngleProperty, value); }
		}

		public bool IsRelative
		{
			get { return (bool)GetValue(IsRelativeProperty); }
			set { SetValue(IsRelativeProperty, value); }
		}

		public RotationAxis Axis
		{
			get { return (RotationAxis)GetValue(AxisProperty); }
			set { SetValue(AxisProperty, value); }
		}

		public async Task<bool> Execute(object sender, object parameter)
		{
			VisualElement element;
			if (TargetObject != null)
			{
				element = TargetObject as VisualElement;
			}
			else {
				element = sender as VisualElement;
			}

			if (element == null)
			{
				return false;
			}

			switch (Axis)
			{
				case RotationAxis.X:
					if (Await)
					{
						await element.RotateXTo(FinalAngle, (uint)Duration, GetEasingFunction());
					}
					else {
						element.RotateXTo(FinalAngle, (uint)Duration, GetEasingFunction());
					}
					break;
				case RotationAxis.Y:
					if (Await)
					{
						await element.RotateYTo(FinalAngle, (uint)Duration, GetEasingFunction());
					}
					else {
						element.RotateYTo(FinalAngle, (uint)Duration, GetEasingFunction());
					}
					break;
				case RotationAxis.Z:
					if (IsRelative)
					{
						if (Await)
						{
							await element.RelRotateTo(FinalAngle, (uint)Duration, GetEasingFunction());
						}
						else {
							element.RelRotateTo(FinalAngle, (uint)Duration, GetEasingFunction());
						}
					}
					else {
						if (Await)
						{
							await element.RotateTo(FinalAngle, (uint)Duration, GetEasingFunction());
						}
						else {
							element.RotateTo(FinalAngle, (uint)Duration, GetEasingFunction());
						}
					}
					break;
				default:
					break;
			}

			return true;
		}
	}
}

