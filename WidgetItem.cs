using System;

namespace UIBuddy
{
	public class WidgetItem
	{
		public string WidgetType { get; set; }
		public string FullType { get; set; }
		public string Name { get; set; }
		public bool Visible { get; set; }
		public bool Sensitive { get; set; }
		public string Allocation { get; set; }

		public WidgetItem ()
		{
		}

		public override string ToString ()
		{
			return string.Format ("WidgetType = {0}\nFullType = {1}\nName = {2}\nVisible = {3}\nSensitive = {4}\nAllocation = {5}", WidgetType, FullType, Name, Visible, Sensitive, Allocation);
		}
	}
}

