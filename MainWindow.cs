using System;
using System.Net.Http;
using System.Xml.Linq;

using UIBuddy;

using Gtk;

public partial class MainWindow: Gtk.Window
{
	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();

		CellRendererText textRenderer = new CellRendererText ();
		TreeViewColumn column = new TreeViewColumn ();
		column.Title = "Hierarchy";
		column.PackStart (textRenderer, true);
		column.SetCellDataFunc (textRenderer, (TreeViewColumn c, CellRenderer r, TreeModel m, TreeIter i) => {
			WidgetItem item = (WidgetItem) m.GetValue (i, 1);
			CellRendererText tr = (CellRendererText) r;
			tr.Text = item.Name ?? item.FullType;
			tr.Foreground = item.Visible ? "black" : "grey";
		});

		widgetTreeview.AppendColumn (column);
		widgetTreeview.Selection.Changed += (object sender, EventArgs e) => {
			TreeIter iter;

			if (widgetTreeview.Selection.GetSelected (out iter)) {
				WidgetItem item = (WidgetItem) widgetTreeview.Model.GetValue (iter, 1);
				DisplayItem (item);
			}
		};
		UpdateWindowHierarchy ();
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void onRefreshClicked (object sender, EventArgs e)
	{
		UpdateWindowHierarchy ();
	}

	void UpdateWindowHierarchy ()
	{
		XDocument doc = null;
		try {
			doc = XDocument.Load ("http://localhost:12698/AutoTest/");
		} catch {
			MessageDialog dialog = new MessageDialog (this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Error connecting to instance of MonoDevelop");
			dialog.Run ();
			dialog.Destroy ();
		}

		if (doc == null) {
			return;
		}

		TreeStore model = new WindowHierarchyModel (doc);

		widgetTreeview.Model = model;
	}

	void DisplayItem (WidgetItem item)
	{
		detailsTextView.Buffer.Text = item.ToString ();
	}
}
