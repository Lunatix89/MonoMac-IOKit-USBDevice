using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MonoMac.IOKit {

	/// <summary>
	/// Verbosity enumeration.
	/// </summary>
	public enum TraceVerbosity {

		/// <summary>
		/// No messages will be sent to trace listeners.
		/// </summary>
		Silent,

		/// <summary>
		/// Only error messages will be sent to trace listeners.
		/// </summary>
		Error,


		/// <summary>
		/// Only error and warning messages will be sent to trace listeners.
		/// </summary>
		ErrorAndWarning,

		/// <summary>
		/// All messages will be sent to trace listeners.
		/// </summary>
		All

	}

	/// <summary>
	/// Serial port utilities using IOKit.
	/// </summary>
	public static class SerialPortIOKit {
#if DEBUG
		private static TraceVerbosity verbosity = TraceVerbosity.All;
#else
		private static TraceVerbosity verbosity = TraceVerbosity.ErrorAndWarning;
#endif
		/// <summary>
		/// Gets or sets the trace verbosity.
		/// </summary>
		public static TraceVerbosity Verbosity {
			get { return verbosity; }
			set { verbosity = value; }
		}

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
								if (verbosity > TraceVerbosity.Silent) {
									Trace.TraceError("Could not get path for modem.\n");
								}
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
