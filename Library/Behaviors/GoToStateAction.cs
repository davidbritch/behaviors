using System;
using System.Threading.Tasks;
using Xamarin.Forms;

#pragma warning disable 1998

namespace Behaviors
{
	[Preserve(AllMembers = true)]
	public sealed class GoToStateAction : BindableObject, IAction
    {
		public static readonly BindableProperty StateNameProperty = BindableProperty.Create(nameof(StateNameProperty), typeof(string), typeof(GoToStateAction), null);
		public static readonly BindableProperty TargetObjectProperty = BindableProperty.Create(nameof(TargetObject), typeof(VisualElement), typeof(GoToStateAction), null);
        
        public string StateName
		{
			get { return (string)GetValue(StateNameProperty); }
			set { SetValue(StateNameProperty, value); }
		}

		public object TargetObject
        {
            get { return (VisualElement)GetValue(TargetObjectProperty); }
            set { SetValue(TargetObjectProperty, value); }
        }

        public async Task<bool> Execute(object sender, object parameter)
		{
		    if (string.IsNullOrWhiteSpace(StateName))
			{
				return false;
			}

			if (TargetObject != null)
			{
				VisualElement element = TargetObject as VisualElement;
				if (element == null)
				{
					return false;
				}

				return GoToState(element, StateName);
			}
			return false;
		}

		bool GoToState(VisualElement visualElement, string stateName)
        {
            if (visualElement == null)
            {
                throw new ArgumentNullException(nameof(visualElement));
            }
            if (string.IsNullOrWhiteSpace(stateName))
            {
                throw new ArgumentNullException(nameof(stateName));
            }

            return VisualStateManager.GoToState(visualElement, stateName);
        }
	}
}
