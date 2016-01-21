// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoMac.Foundation;
using System.CodeDom.Compiler;

namespace SerialPortMonitor
{
	[Register ("MainWindowController")]
	partial class MainWindowController
	{
		[Outlet]
		MonoMac.AppKit.NSScrollView DeviceTable { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTableHeaderView DeviceTableHeaderView { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTableView DeviceTableView { get; set; }

		[Outlet]
		MonoMac.AppKit.NSArrayController tableContent { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (DeviceTable != null) {
				DeviceTable.Dispose ();
				DeviceTable = null;
			}

			if (DeviceTableHeaderView != null) {
				DeviceTableHeaderView.Dispose ();
				DeviceTableHeaderView = null;
			}

			if (DeviceTableView != null) {
				DeviceTableView.Dispose ();
				DeviceTableView = null;
			}

			if (tableContent != null) {
				tableContent.Dispose ();
				tableContent = null;
			}
		}
	}

	[Register ("MainWindow")]
	partial class MainWindow
	{
		
		void ReleaseDesignerOutlets ()
		{
		}
	}
}
