using System.Threading.Tasks;
using Xamarin.Forms;

#pragma warning disable 4014

namespace Behaviors
{
	[Preserve(AllMembers = true)]
	public class ScaleAction : AnimationBase, IAction
	{
		public static readonly BindableProperty FinalScaleProperty = BindableProperty.Create(nameof(FinalScale), typeof(double), typeof(ScaleAction), 1.0);
		public static readonly BindableProperty IsRelativeProperty = BindableProperty.Create(nameof(IsRelative), typeof(bool), typeof(ScaleAction), false);

		public double FinalScale
		{
			get { return (double)GetValue(FinalScaleProperty); }
			set { SetValue(FinalScaleProperty, value); }
		}

		public bool IsRelative
		{
			get { return (bool)GetValue(IsRelativeProperty); }
			set { SetValue(IsRelativeProperty, value); }
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

			if (IsRelative)
			{
				if (Await)
				{
					await element.RelScaleTo(FinalScale, (uint)Duration, GetEasingFunction());
				}
				else {
					element.RelScaleTo(FinalScale, (uint)Duration, GetEasingFunction());
				}
			}
			else {
				if (Await)
				{
					await element.ScaleTo(FinalScale, (uint)Duration, GetEasingFunction());
				}
				else {
					element.ScaleTo(FinalScale, (uint)Duration, GetEasingFunction());
				}
			}

			return true;
		}
	}
}

