using System;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

#pragma warning disable 1998

namespace Behaviors
{
	[Preserve(AllMembers = true)]
	public sealed class SetPropertyAction : BindableObject, IAction
	{
		public static readonly BindableProperty PropertyNameProperty = BindableProperty.Create(nameof(PropertyName), typeof(string), typeof(SetPropertyAction), null);
		public static readonly BindableProperty TargetObjectProperty = BindableProperty.Create(nameof(TargetObject), typeof(object), typeof(SetPropertyAction), null);
		public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(object), typeof(SetPropertyAction), null);

		public string PropertyName
		{
			get { return (string)GetValue(PropertyNameProperty); }
			set { SetValue(PropertyNameProperty, value); }
		}

		public object TargetObject
		{
			get { return (object)GetValue(TargetObjectProperty); }
			set { SetValue(TargetObjectProperty, value); }
		}

		public object Value
		{
			get { return (object)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		public async Task<bool> Execute(object sender, object parameter)
		{
			object targetObject;
			if (TargetObject != null)
			{
				targetObject = TargetObject;
			}
			else {
				targetObject = sender;
			}

			if (targetObject == null || PropertyName == null)
			{
				return false;
			}

			UpdatePropertyValue(targetObject);
			return true;
		}

		void UpdatePropertyValue(object targetObject)
		{
			Type targetType = targetObject.GetType();
			PropertyInfo propertyInfo = targetType.GetRuntimeProperty(PropertyName);
			ValidateProperty(targetType.Name, propertyInfo);

			Exception innerException = null;
			try
			{
				object result = null;
				Type propertyType = propertyInfo.PropertyType;
				TypeInfo propertyTypeInfo = propertyType.GetTypeInfo();

				if (Value == null)
				{
					result = propertyTypeInfo.IsValueType ? Activator.CreateInstance(propertyType) : null;
				}
				else if (propertyTypeInfo.IsAssignableFrom(Value.GetType().GetTypeInfo()))
				{
					result = Value;
				}
				else {
					string valueAsString = Value.ToString();
					result = propertyTypeInfo.IsEnum ? Enum.Parse(propertyType, valueAsString, false) : TypeConverterHelper.Convert(valueAsString, propertyType.FullName);
				}
				propertyInfo.SetValue(targetObject, result, new object[0]);
			}
			catch (FormatException ex)
			{
				innerException = ex;
			}
			catch (ArgumentException ex)
			{
				innerException = ex;
			}

			if (innerException != null)
			{
				throw new ArgumentException("Cannot set value.", innerException);
			}
		}

		void ValidateProperty(string targetTypeName, PropertyInfo propertyInfo)
		{
			if (propertyInfo == null)
			{
				throw new ArgumentException("Cannot find property name.");
			}
			else if (!propertyInfo.CanWrite)
			{
				throw new ArgumentException("Property is read-only.");
			}
		}
	}
}

