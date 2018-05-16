using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

#pragma warning disable 1998

namespace Behaviors
{
	[Preserve(AllMembers = true)]
	public class InvokeMethodAction : BindableObject, IAction
	{
		Type _targetObjectType;
		MethodDescriptor _cachedMethodDescriptor;
		List<MethodDescriptor> _methodDescriptors = new List<MethodDescriptor>();

		public static readonly BindableProperty MethodNameProperty = BindableProperty.Create(nameof(MethodName), typeof(string), typeof(InvokeMethodAction), null, propertyChanged: OnMethodNameChanged);
		public static readonly BindableProperty TargetObjectProperty = BindableProperty.Create(nameof(TargetObject), typeof(object), typeof(InvokeMethodAction), null, propertyChanged: OnTargetObjectChanged);

		public string MethodName
		{
			get { return (string)GetValue(MethodNameProperty); }
			set { SetValue(MethodNameProperty, value); }
		}

		public object TargetObject
		{
			get { return (object)GetValue(TargetObjectProperty); }
			set { SetValue(TargetObjectProperty, value); }
		}

		public async Task<bool> Execute(object sender, object parameter)
		{
			object target;
			if (GetValue(TargetObjectProperty) != null)
			{
				target = TargetObject;
			}
			else {
				target = sender;
			}

			if (target == null || string.IsNullOrWhiteSpace(MethodName))
			{
				return false;
			}

			UpdateTargetType(target.GetType());

			MethodDescriptor methodDescriptor = FindBestMethod(parameter);
			if (methodDescriptor == null)
			{
				if (TargetObject != null)
				{
					throw new ArgumentException("Valid method not found.");
				}
				return false;
			}

			ParameterInfo[] parameters = methodDescriptor.Parameters;
			if (parameters.Length == 0)
			{
				methodDescriptor.MethodInfo.Invoke(target, parameters: null);
				return true;
			}
			else if (parameters.Length == 2)
			{
				methodDescriptor.MethodInfo.Invoke(target, new object[] { target, parameter });
				return true;
			}

			return false;
		}

		MethodDescriptor FindBestMethod(object parameter)
		{
			TypeInfo parameterTypeInfo = parameter == null ? null : parameter.GetType().GetTypeInfo();

			if (parameterTypeInfo == null)
			{
				return _cachedMethodDescriptor;
			}

			MethodDescriptor mostDerivedMethod = null;

			foreach (MethodDescriptor currentMethod in _methodDescriptors)
			{
				TypeInfo currentTypeInfo = currentMethod.SecondParameterTypeInfo;

				if (currentTypeInfo.IsAssignableFrom(parameterTypeInfo))
				{
					if (mostDerivedMethod == null || !currentTypeInfo.IsAssignableFrom(mostDerivedMethod.SecondParameterTypeInfo))
					{
						mostDerivedMethod = currentMethod;
					}
				}
			}

			return mostDerivedMethod ?? _cachedMethodDescriptor;
		}

		void UpdateTargetType(Type newTargetType)
		{
			if (newTargetType == _targetObjectType)
			{
				return;
			}

			_targetObjectType = newTargetType;
			UpdateMethodDescriptors();
		}

		void UpdateMethodDescriptors()
		{
			_methodDescriptors.Clear();
			_cachedMethodDescriptor = null;

			if (string.IsNullOrWhiteSpace(MethodName) || _targetObjectType == null)
			{
				return;
			}

			foreach (MethodInfo method in _targetObjectType.GetRuntimeMethods())
			{
				if (string.Equals(method.Name, MethodName, StringComparison.Ordinal) && method.ReturnType == typeof(void) && method.IsPublic)
				{
					var parameters = method.GetParameters();
					if (parameters.Length == 0)
					{
						_cachedMethodDescriptor = new MethodDescriptor(method, parameters);
					}
					else if (parameters.Length == 2 && parameters[0].ParameterType == typeof(object))
					{
						_methodDescriptors.Add(new MethodDescriptor(method, parameters));
					}
				}
			}

		}

		static void OnMethodNameChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var action = (InvokeMethodAction)bindable;
			var newType = newValue != null ? newValue.GetType() : null;
			action.UpdateTargetType(newType);
		}

		static void OnTargetObjectChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var action = (InvokeMethodAction)bindable;
			action.UpdateMethodDescriptors();
		}
	}
}

