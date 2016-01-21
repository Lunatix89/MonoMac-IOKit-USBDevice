
using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.IOKit;
using System.Timers;

namespace SerialPortMonitor
{
	
	public partial class MainWindowController : MonoMac.AppKit.NSWindowController {
		private readonly Timer enumerationTimer = new Timer(1000);
		private NSArrayController communicationDevices;

		/// <summary>
		/// Gets the window.
		/// </summary>
		public new MainWindow Window {
			get { return (MainWindow)base.Window; }
		}

		[Export("USBCommunicationDevices")]
		public NSArrayController USBCommunicationDevices {
			get { return communicationDevices; }
			set {
				WillChangeValue ("USBCommunicationDevices");
				communicationDevices = value;
				DidChangeValue ("USBCommunicationDevices");
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SerialPortMonitor.MainWindowController"/> class.
		/// </summary>
		/// <param name="handle">Handle.</param>
		public MainWindowController (IntPtr handle) 
			: base (handle)
		{
			Initialize ();
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="SerialPortMonitor.MainWindowController"/> class.
		/// </summary>
		/// <param name="coder">Coder.</param>
		[Export ("initWithCoder:")]
		public MainWindowController (NSCoder coder) 
			: base (coder)
		{
			Initialize ();
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="SerialPortMonitor.MainWindowController"/> class.
		/// </summary>
		public MainWindowController () 
			: base ("MainWindow")
		{
			Initialize ();
		}
		
		// Shared initialization code
		private void Initialize () {
			enumerationTimer.Elapsed += (sender, e) => BeginInvokeOnMainThread(new NSAction(EnumerateUSBDevices));
			enumerationTimer.Start ();
		}

		public override void WindowDidLoad () {
			base.WindowDidLoad ();

			

			/*
			DeviceTableView.AddColumn (new NSTableColumn {
				HeaderCell = new NSCell("No."),
				Editable = false,
			});
			DeviceTableView.AddColumn (new NSTableColumn {
				HeaderCell = new NSCell("Com Name."),
				Editable = false,
			});
			DeviceTableView.AddColumn (new NSTableColumn {
				HeaderCell = new NSCell("Vendor."),
				Editable = false,
			});
			DeviceTableView.AddColumn (new NSTableColumn {
				HeaderCell = new NSCell("Vendor ID."),
				Editable = false,
			});
			DeviceTableView.AddColumn (new NSTableColumn {
				HeaderCell = new NSCell("Product."),
				Editable = false,
			});
			DeviceTableView.AddColumn (new NSTableColumn {
				HeaderCell = new NSCell("Product ID."),
				Editable = false,
			});
			*/

		}

		private List<NSMutableDictionary> deviceList = new List<NSMutableDictionary> ();

		private void EnumerateUSBDevices() { 
			var availableDevices = SerialPortIOKit.GetUSBCommunicationDevices ();
			var keys = new string[] { "ComName", "Vendor", "VendorID", "Product", "ProductID" };


			foreach (var item in availableDevices) {
				if (!deviceList.Any(k => k["ComName"].ToString().Equals(item.ComName))) {
					var objects = new object[] { 
						new NSString(item.ComName),
						new NSString(item.VendorString ?? "?"), 
						new NSString("0x" + item.VendorID.ToString("X")), 
						new NSString(item.ProductString ?? "?"), 
						new NSString("0x" + item.ProductID.ToString("X"))
					};
					var dictionary = NSMutableDictionary.FromObjectsAndKeys(objects, keys);
					deviceList.Add (dictionary);

					tableContent.AddObject (dictionary);
				}
			}

			var deletedValue = false;
			do {
				deletedValue = false;

				foreach (var item in deviceList) {
					if (!availableDevices.Any(k => k.ComName.Equals(item["ComName"].ToString()))) {
						deviceList.Remove (item);
						tableContent.RemoveObject(item);
						deletedValue = true;
						break;
					}
				}
			} while (deletedValue);
		}

	}
}

