using System;
using System.Reflection;
using Xamarin.Forms;

namespace Behaviors
{
	[Preserve(AllMembers = true)]
	public sealed class EventHandlerBehavior : BehaviorPropertiesBase
	{
		Delegate _eventHandler;
		object _resolvedSource;

		public static readonly BindableProperty EventNameProperty = BindableProperty.Create(nameof(EventName), typeof(string), typeof(EventHandlerBehavior), propertyChanged: OnEventNameChanged);
		public static readonly BindableProperty SourceObjectProperty = BindableProperty.Create(nameof(SourceObject), typeof(object), typeof(EventHandlerBehavior), null, propertyChanged: OnSourceObjectChanged);

		public string EventName
		{
			get { return (string)GetValue(EventNameProperty); }
			set { SetValue(EventNameProperty, value); }
		}

        public object SourceObject
		{
			get { return GetValue(SourceObjectProperty); }
			set { SetValue(SourceObjectProperty, value); }
		}

		protected override void OnAttachedTo(VisualElement bindable)
		{
			base.OnAttachedTo(bindable);
			SetResolvedSource(ComputeResolvedSource());
		}

		protected override void OnDetachingFrom(VisualElement bindable)
		{
			base.OnDetachingFrom(bindable);
			SetResolvedSource(null);
		}

        void SetResolvedSource(object newSource)
		{
			if (AssociatedObject == null || _resolvedSource == newSource)
			{
				return;
			}
			if (_resolvedSource != null)
			{
				DeregisterEvent(EventName);
			}
			_resolvedSource = newSource;
			if (_resolvedSource != null)
			{
				RegisterEvent(EventName);
			}
		}

        object ComputeResolvedSource()
		{
			if (SourceObject != null)
			{
				return SourceObject;
			}
			return AssociatedObject;
		}

		void RegisterEvent(string eventName)
		{
			if (string.IsNullOrWhiteSpace(eventName))
			{
				return;
			}

			Type sourceObjectType = _resolvedSource.GetType();
			EventInfo eventInfo = sourceObjectType.GetRuntimeEvent(eventName);
			if (eventInfo == null)
			{
				return;
			}
			MethodInfo methodInfo = typeof(EventHandlerBehavior).GetTypeInfo().GetDeclaredMethod("OnEvent");
			_eventHandler = methodInfo.CreateDelegate(eventInfo.EventHandlerType, this);
			eventInfo.AddEventHandler(_resolvedSource, _eventHandler);
		}

		void DeregisterEvent(string eventName)
		{
			if (string.IsNullOrWhiteSpace(eventName))
			{
				return;
			}

			if (_eventHandler == null)
			{
				return;
			}

			EventInfo eventInfo = _resolvedSource.GetType().GetRuntimeEvent(eventName);
			if (eventInfo == null)
			{
				throw new ArgumentException(string.Format("EventHandlerBehavior: Can't de-register the '{0}' event.", EventName));
			}
			eventInfo.RemoveEventHandler(_resolvedSource, _eventHandler);
			_eventHandler = null;
		}

		async void OnEvent(object sender, object eventArgs)
		{
			foreach (BindableObject bindable in Actions)
			{
				bindable.BindingContext = BindingContext;
				var action = (IAction)bindable;
				await action.Execute(sender, eventArgs);
			}
		}

		static void OnEventNameChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var behavior = (EventHandlerBehavior)bindable;
			if (behavior.AssociatedObject == null || behavior._resolvedSource == null)
			{
				return;
			}

			string oldEventName = (string)oldValue;
			string newEventName = (string)newValue;

			behavior.DeregisterEvent(oldEventName);
			behavior.RegisterEvent(newEventName);
		}

        static void OnSourceObjectChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var behavior = (EventHandlerBehavior)bindable;
			behavior.SetResolvedSource(behavior.ComputeResolvedSource());
		}
	}
}

