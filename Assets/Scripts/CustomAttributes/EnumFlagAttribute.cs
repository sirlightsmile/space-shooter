using System;
using UnityEngine;

public class EnumFlagAttribute : PropertyAttribute
{
	public enum FlagLayout
	{
		Dropdown,
		List
	}

	public FlagLayout _layout = FlagLayout.List;

	public EnumFlagAttribute() { }

	public EnumFlagAttribute(FlagLayout layout)
	{
		_layout = layout;
	}
}