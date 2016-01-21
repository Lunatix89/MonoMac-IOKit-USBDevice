using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MonoMac.IOKit {

	/// <summary>
	/// Serial port utilities using IOKit.
	/// </summary>
	public static class SerialPortIOKit {

		/// <summary>
		/// Returns a list of available communication devices.
		/// </summary>
		/// <returns>A list of <see cref="USBCommunicationDevice"/> instances.</returns>
		public static List<USBCommunicationDevice> GetUSBCommunicationDevices() {
			var result = new List<USBCommunicationDevice>();

			using (var serialPortIterator = IOKitFramework.FindModems()) {
				var modemService = serialPortIterator.Next();

				while (modemService != null) {
					using (modemService) {
						var modemPath = IOKitFramework.GetModemPath(modemService);
						if (String.IsNullOrEmpty(modemPath)) {
							Trace.TraceError("Could not get path for modem.\n");
						} else {
							//	var device = IOKitInterop.GetUSBDevice (modemService);
							//	using (var device = IOKitInterop.GetUSBDevice(modemService)) {
							//		if (device != null) {
							/*
							var vendorString = device.GetCFPropertyString(IOKitInterop.kUSBVendorString);
							var productString = device.GetCFPropertyString(IOKitInterop.kUSBProductString);
							var productID = device.GetCFPropertyInt(IOKitInterop.kUSBProductID);
							var vendorID = device.GetCFPropertyInt(IOKitInterop.kUSBVendorID);

							if ((productID > 0) && (vendorID > 0)) {
								var deviceDescriptor = new USBCommunicationsDevice {
									ComName = modemPath,
									VendorString = vendorString,
									ProductString = productString,
									VendorID = vendorID,
									ProductID = productID,
									Caption = String.Format("{0} {1}", vendorString, productString)
								};

								result.Add(deviceDescriptor);
							}
							*/
						}

						//}
						//	}
					}

					modemService = serialPortIterator.Next();
				}

			}

			return result;

			/*

			var serialPortIterator = IntPtr.Zero;


			var kernResult = IOKitInterop.FindModems(ref serialPortIterator);
			if (IOKitInterop.KERN_SUCCESS != kernResult) {
				Trace.TraceError("No modems were found.\n");
			}

			var result = new List<USBCommunicationsDevice> ();
			var modemService = IntPtr.Zero;

			while((modemService = IOKitInterop.IOIteratorNext (serialPortIterator)) != IntPtr.Zero) {
				var modemPath = String.Empty;
				kernResult = IOKitInterop.GetModemPath (modemService, out modemPath);
				if (IOKitInterop.KERN_SUCCESS != kernResult) {
					Trace.TraceError ("Could not get path for modem.\n");
					continue;
				} 

				try {
					var device = IOKitInterop.GetUsbDevice (modemService);
					if (device == IntPtr.Zero) {
						Trace.TraceError ("Unable to find matching USB device.\n");
						continue;
					}

					var vendorString = String.Empty;
					var productString = String.Empty;
					var productID = 0;
					var vendorID = 0;
					// var serialNumber = String.Empty;
				
					IOKitInterop.GetCFPropertyString (device, IOKitInterop.kUSBVendorString, out vendorString);
					IOKitInterop.GetCFPropertyString (device, IOKitInterop.kUSBProductString, out productString);
					IOKitInterop.GetCFPropertyInt (device, IOKitInterop.kUSBProductID, out productID);
					IOKitInterop.GetCFPropertyInt (device, IOKitInterop.kUSBVendorID, out vendorID);
					// IOKitInterop.GetCFPropertyString (device, IOKitInterop.kUSBSerialNumberString, out serialNumber);

					var deviceDescriptor = new USBCommunicationsDevice {
						ComName = modemPath,
						VendorString = vendorString,
						ProductString = productString,
						VendorID = vendorID,
						ProductID = productID,
						Caption = String.Format("{0} {1}", vendorString, productString)
					};

					result.Add(deviceDescriptor);

					IOKitInterop.IOObjectRelease (device);
				} finally {
					IOKitInterop.IOObjectRelease (modemService);
				}
			}

			// Release the iterator.
			IOKitInterop.IOObjectRelease(serialPortIterator);	
			
			return result;*/
		}

	}
}
