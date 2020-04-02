using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Behaviors
{
	[Preserve(AllMembers = true)]
	[ContentProperty("Actions")]
	public class BehaviorPropertiesBase : BehaviorBase<VisualElement>
    {
		public static readonly BindableProperty ActionsProperty = BindableProperty.Create(nameof(Actions), typeof(ObservableCollection<IAction>), typeof(BehaviorPropertiesBase), null);

		public ObservableCollection<IAction> Actions
        {
            get
            {
                return (ObservableCollection<IAction>)GetValue(ActionsProperty);
            }
        }

        public BehaviorPropertiesBase()
		{         
			SetValue(ActionsProperty, new ObservableCollection<IAction>());
		}
    }
}
