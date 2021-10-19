using System.Collections.Generic;

namespace SmileProject.Generic
{
	public abstract class StringEnum<T> where T : StringEnum<T>
	{
		private static List<StringEnum<T>> enums = new List<StringEnum<T>>();
		public string Value { get; private set; }

		public StringEnum(string value)
		{
			this.Value = value;
			enums.Add(this);
		}

		public override string ToString()
		{
			return Value;
		}

		public static IEnumerable<StringEnum<T>> GetAll()
		{
			return enums;
		}
	}
}