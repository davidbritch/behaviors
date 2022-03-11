namespace Behaviors.Sample
{
	public sealed class Person
	{
		public string Name { get; private set; }
		public int Age { get; private set; }
		public int? AgeParameter { get; private set; }

		public Person (string name, int age, int? ageParam = null)
		{
			Name = name;
			Age = age;
			AgeParameter = ageParam;
		}

		public override string ToString ()
		{
			return Name;
		}
	}
}

