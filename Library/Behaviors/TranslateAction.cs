using System.Threading.Tasks;
using Xamarin.Forms;

#pragma warning disable 4014

namespace Behaviors
{
	[Preserve(AllMembers = true)]
	public class TranslateAction : AnimationBase, IAction
	{
		public static readonly BindableProperty XProperty = BindableProperty.Create(nameof(X), typeof(double), typeof(TranslateAction), 1.0);
		public static readonly BindableProperty YProperty = BindableProperty.Create(nameof(Y), typeof(double), typeof(TranslateAction), 1.0);

		public double X
		{
			get { return (double)GetValue(XProperty); }
			set { SetValue(XProperty, value); }
		}

		public double Y
		{
			get { return (double)GetValue(YProperty); }
			set { SetValue(YProperty, value); }
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

			if (Await)
			{
				await element.TranslateTo(X, Y, (uint)Duration, GetEasingFunction());
			}
			else {
				element.TranslateTo(X, Y, (uint)Duration, GetEasingFunction());
			}

			return true;
		}
	}
}

