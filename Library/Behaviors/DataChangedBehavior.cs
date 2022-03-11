using System;
using System.Globalization;
using Xamarin.Forms;

namespace Behaviors
{
	[Preserve(AllMembers = true)]
	public sealed class DataChangedBehavior : BehaviorPropertiesBase
	{
		public static readonly BindableProperty BindingProperty = BindableProperty.Create(nameof(Binding), typeof(object), typeof(DataChangedBehavior), null, propertyChanged: OnValueChanged);
		public static readonly BindableProperty ComparisonProperty = BindableProperty.Create("Comparison", typeof(ComparisonCondition), typeof(DataChangedBehavior), ComparisonCondition.Equal, propertyChanged: OnValueChanged);
		public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(object), typeof(DataChangedBehavior), null, propertyChanged: OnValueChanged);

		public object Binding
		{
			get { return (object)GetValue(BindingProperty); }
			set { SetValue(BindingProperty, value); }
		}

		public ComparisonCondition ComparisonCondition
		{
			get { return (ComparisonCondition)GetValue(ComparisonProperty); }
			set { SetValue(ComparisonProperty, value); }
		}

		public object Value
		{
			get { return (object)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		protected override void OnAttachedTo(VisualElement bindable)
		{
			base.OnAttachedTo(bindable);
		}

		protected override void OnDetachingFrom(VisualElement bindable)
		{
			base.OnDetachingFrom(bindable);
		}

		static bool Compare(object leftOperand, ComparisonCondition operatorType, object rightOperand)
		{
			if (leftOperand != null && rightOperand != null)
			{
				rightOperand = TypeConverterHelper.Convert(rightOperand.ToString(), leftOperand.GetType().FullName);
			}

			IComparable leftComparableOperand = leftOperand as IComparable;
			IComparable rightComparableOperand = rightOperand as IComparable;

			if ((leftComparableOperand != null) && (rightComparableOperand != null))
			{
				return EvaluateComparable(leftComparableOperand, operatorType, rightComparableOperand);
			}

			switch (operatorType)
			{
				case ComparisonCondition.Equal:
					return object.Equals(leftOperand, rightOperand);
				case ComparisonCondition.NotEqual:
					return !object.Equals(leftOperand, rightOperand);
				case ComparisonCondition.LessThan:
				case ComparisonCondition.LessThanOrEqual:
				case ComparisonCondition.GreaterThan:
				case ComparisonCondition.GreaterThanOrEqual:
					{
						if (leftComparableOperand == null && rightComparableOperand == null)
						{
							throw new ArgumentException("Invalid operands");
						}
						else if (leftComparableOperand == null)
						{
							throw new ArgumentException("Invalid left operand");
						}
						else {
							throw new ArgumentException("Invalid right operand");
						}
					}
			}

			return false;
		}

		static bool EvaluateComparable(IComparable leftOperand, ComparisonCondition operatorType, IComparable rightOperand)
		{
			object convertedOperand = null;
			try
			{
				convertedOperand = Convert.ChangeType(rightOperand, leftOperand.GetType(), CultureInfo.CurrentCulture);
			}
			catch (FormatException)
			{
			}
			catch (InvalidCastException)
			{
			}

			if (convertedOperand == null)
			{
				return operatorType == ComparisonCondition.NotEqual;
			}

			int comparison = leftOperand.CompareTo((IComparable)convertedOperand);
			switch (operatorType)
			{
				case ComparisonCondition.Equal:
					return comparison == 0;
				case ComparisonCondition.NotEqual:
					return comparison != 0;
				case ComparisonCondition.LessThan:
					return comparison < 0;
				case ComparisonCondition.LessThanOrEqual:
					return comparison <= 0;
				case ComparisonCondition.GreaterThan:
					return comparison > 0;
				case ComparisonCondition.GreaterThanOrEqual:
					return comparison >= 0;
			}

			return false;
		}

		static async void OnValueChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var behavior = (DataChangedBehavior)bindable;
			if (behavior.AssociatedObject == null)
			{
				return;
			}

			if (Compare(behavior.Binding, behavior.ComparisonCondition, behavior.Value))
			{
				foreach (BindableObject item in behavior.Actions)
				{
					item.BindingContext = behavior.BindingContext;
					IAction action = (IAction)item;
					await action.Execute(bindable, newValue);
				}
			}
		}
	}
}

