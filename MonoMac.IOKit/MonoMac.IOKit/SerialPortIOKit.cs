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

			using (var serialPortIterator = IOKitFramework.FindModems ()) {
				if (serialPortIterator != null) {
					var modemService = serialPortIterator.Next ();

					while (modemService != null) {
						using (modemService) {
							var modemPath = IOKitFramework.GetModemPath (modemService);
							if (String.IsNullOrEmpty (modemPath)) {
								Trace.TraceError ("Could not get path for modem.\n");
							} else {
								using (var device = IOKitFramework.GetUSBDevice (modemService)) {
									if (device != null) {
										var vendorString = device.GetCFPropertyString (IOKitFramework.kUSBVendorString);
										var productString = device.GetCFPropertyString (IOKitFramework.kUSBProductString);
										var productID = device.GetCFPropertyInt (IOKitFramework.kUSBProductID);
										var vendorID = device.GetCFPropertyInt (IOKitFramework.kUSBVendorID);

										if ((productID > 0) && (vendorID > 0)) {
											var deviceDescriptor = new USBCommunicationDevice {
												ComName = modemPath,
												VendorString = vendorString,
												ProductString = productString,
												VendorID = vendorID,
												ProductID = productID,
												Caption = String.Format ("{0} {1}", vendorString, productString)
											};

											result.Add (deviceDescriptor);
										}
									}
								}
							}
						}

						modemService = serialPortIterator.Next ();
					}
				}
			}

			return result;
		}

	}
}
