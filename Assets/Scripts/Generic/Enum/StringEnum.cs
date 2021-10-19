namespace SmileProject.Generic
{
	public abstract class StringEnum<T> where T : StringEnum<T>
	{
		public string Value { get; private set; }

		public StringEnum(string value)
		{
			this.Value = value;
		}

		public override string ToString()
		{
			return Value;
		}
	}
}