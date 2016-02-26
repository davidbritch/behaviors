using System;
using System.Reflection;
using Xamarin.Forms;

namespace Behaviors
{
	[ContentProperty ("Actions")]
	public sealed class EventHandlerBehavior : BehaviorBase<View>
	{
		Delegate eventHandler;

		public static readonly BindableProperty EventNameProperty = BindableProperty.Create ("EventName", typeof(string), typeof(EventHandlerBehavior), null, propertyChanged: OnEventNameChanged);
		public static readonly BindableProperty ActionsProperty = BindableProperty.Create ("Actions", typeof(ActionCollection), typeof(EventHandlerBehavior), null);

		public string EventName {
			get { return (string)GetValue (EventNameProperty); }
			set { SetValue (EventNameProperty, value); }
		}

		public ActionCollection Actions {
			get {
				var actionCollection = (ActionCollection)GetValue (ActionsProperty);
				if (actionCollection == null) {
					actionCollection = new ActionCollection ();
					SetValue (ActionsProperty, actionCollection);
				}
				return actionCollection;
			}
		}

		protected override void OnAttachedTo (View bindable)
		{
			base.OnAttachedTo (bindable);
			RegisterEvent (EventName);
		}

		protected override void OnDetachingFrom (View bindable)
		{
			DeregisterEvent (EventName);
			base.OnDetachingFrom (bindable);
		}

		void RegisterEvent (string name)
		{
			if (string.IsNullOrWhiteSpace (name)) {
				return;
			}

			EventInfo eventInfo = AssociatedObject.GetType ().GetRuntimeEvent (EventName);
			if (eventInfo == null) {
				throw new ArgumentException (string.Format ("EventHandlerBehavior: Can't register the '{0}' event.", EventName));
			}
			MethodInfo methodInfo = typeof(EventHandlerBehavior).GetTypeInfo ().GetDeclaredMethod ("OnEvent");
			eventHandler = methodInfo.CreateDelegate (eventInfo.EventHandlerType, this);
			eventInfo.AddEventHandler (AssociatedObject, eventHandler);
		}

		void DeregisterEvent (string name)
		{
			if (string.IsNullOrWhiteSpace (name)) {
				return;
			}

			if (eventHandler == null) {
				return;
			}
			EventInfo eventInfo = AssociatedObject.GetType ().GetRuntimeEvent (EventName);
			if (eventInfo == null) {
				throw new ArgumentException (string.Format ("EventHandlerBehavior: Can't de-register the '{0}' event.", EventName));
			}
			eventInfo.RemoveEventHandler (AssociatedObject, eventHandler);
			eventHandler = null;
		}

		void OnEvent (object sender, object eventArgs)
		{
			foreach (BindableObject bindable in Actions) {
				bindable.BindingContext = BindingContext;
				var action = (IAction)bindable;
				action.Execute (sender, eventArgs);
			}
		}

		static void OnEventNameChanged (BindableObject bindable, object oldValue, object newValue)
		{
			var behavior = (EventHandlerBehavior)bindable;
			if (behavior.AssociatedObject == null) {
				return;
			}

			string oldEventName = (string)oldValue;
			string newEventName = (string)newValue;

			behavior.DeregisterEvent (oldEventName);
			behavior.RegisterEvent (newEventName);
		}
	}
}

