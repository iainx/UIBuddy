using System;
using System.Xml.Linq;
using Gtk;

namespace UIBuddy
{
	public class WindowHierarchyModel : TreeStore
	{
		public WindowHierarchyModel (XDocument document) : base (typeof (string), typeof (WidgetItem))
		{
			XElement results = document.Element ("results");
			foreach (var result in results.Elements ()) {
				Console.WriteLine ("Toplevel: " + result.ToString ());
				AddResultsToTree (result, null); 	
			}
		}

		WidgetItem ItemFromXml (XElement result)
		{
			WidgetItem item = new WidgetItem () {
				Name = (string) result.Attribute ("name"),
				WidgetType = (string) result.Attribute ("type"),
				FullType = (string) result.Attribute ("fulltype"),
				Visible = Convert.ToBoolean ((string) result.Attribute ("visible")),
				Sensitive = Convert.ToBoolean ((string) result.Attribute ("sensitive")),
				Allocation = (string) result.Attribute ("allocation")
			};
			return item;
		}

		void AddResultsToTree (XElement result, TreeIter? currentIter)
		{
			WidgetItem item = ItemFromXml (result);

			TreeIter parentIter;
			string name = String.IsNullOrEmpty (item.Name) ? item.FullType : item.Name;
			if (currentIter.HasValue) {
				parentIter = this.AppendValues ((TreeIter) currentIter, name, item);
			} else {
				parentIter = this.AppendValues (name, item);
			}

			var childResults = result.Elements ();
			if (childResults != null) {
				foreach (var childResult in childResults) {
					AddResultsToTree (childResult, parentIter);
				}
			}
		}
	}

}

