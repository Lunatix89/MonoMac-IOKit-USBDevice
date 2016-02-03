using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace MonoMac.IOKit {


	/// <summary>
	/// Native interop code to use the IOKit framework.
	/// </summary>
	public static class IOKitFramework {
		/// <summary>
		/// IOKit Framework Path
		/// </summary>
		public const string IOKitFrameworkPath = "IOKit.framework/IOKit";

		// Service Matching That is the 'IOProviderClass'.
		public const string kIOSerialBSDServiceValue = "IOSerialBSDClient";
		
		// Matching keys.
		public const string kIOSerialBSDTypeKey = "IOSerialBSDClientType";

		// Currently possible kIOSerialBSDTypeKey values. 
		public const string kIOSerialBSDAllTypes = "IOSerialStream";
		public const string kIOSerialBSDModemType = "IOModemSerialStream";
		public const string kIOSerialBSDRS232Type = "IORS232SerialStream";


		// Properties that resolve to a /dev device node to open for a particular service
		public const string kIOTTYDeviceKey = "IOTTYDevice";
		public const string kIOTTYBaseNameKey = "IOTTYBaseName";
		public const string kIOTTYSuffixKey = "IOTTYSuffix";
		public const string kIOCalloutDeviceKey = "IOCalloutDevice";
		public const string kIODialinDeviceKey = "IODialinDevice";

		//  USB device properties.
		public const string kUSBDeviceClass = "bDeviceClass";
		public const string kUSBDeviceSubClass = "bDeviceSubClass";
		public const string kUSBDeviceProtocol = "bDeviceProtocol";
		public const string kUSBDeviceMaxPacketSize = "bMaxPacketSize0";
		public const string kUSBCompatibilityMatch = "USBCompatibilityMatch";
		public const string kUSBVendorID = "idVendor";         // good name
		public const string kUSBVendorName = kUSBVendorID;       // bad name - keep for backward compatibility
		public const string kUSBProductID = "idProduct";         // good name
		public const string kUSBProductName = kUSBProductID;      // bad name - keep for backward compatibility
		public const string kUSBDeviceReleaseNumber = "bcdDevice";
		public const string kUSBSpecReleaseNumber = "bcdUSB";
		public const string kUSBManufacturerStringIndex = "iManufacturer";
		public const string kUSBProductStringIndex = "iProduct";
		public const string kUSBSerialNumberStringIndex = "iSerialNumber";
		public const string kUSBDeviceNumConfigs = "bNumConfigurations";
		public const string kUSBInterfaceNumber = "bInterfaceNumber";
		public const string kUSBAlternateSetting = "bAlternateSetting";
		public const string kUSBNumEndpoints = "bNumEndpoints";
		public const string kUSBInterfaceClass = "bInterfaceClass";
		public const string kUSBInterfaceSubClass = "bInterfaceSubClass";
		public const string kUSBInterfaceProtocol = "bInterfaceProtocol";
		public const string kUSBInterfaceStringIndex = "iInterface";
		public const string kUSBConfigurationValue = "bConfigurationValue";
		public const string kUSBProductString = "USB Product Name";
		public const string kUSBVendorString = "USB Vendor Name";
		public const string kUSBSerialNumberString = "USB Serial Number";
		public const string kUSB1284DeviceID = "1284 Device ID";

		// Registry plane names
		public const string kIOServicePlane = "IOService";
		public const string kIOPowerPlane = "IOPower";
		public const string kIODeviceTreePlane = "IODeviceTree";
		public const string kIOAudioPlane = "IOAudio";
		public const string kIOFireWirePlane = "IOFireWire";
		public const string kIOUSBPlane = "IOUSB";
		public const string kIOUSBDeviceClassName = "IOUSBDevice";

		// IOReturn* codes
		public const int IOKitCommonError = 0;
		public const int kIOReturnSuccess = KERN_SUCCESS;						// OK
		public const int kIOReturnError = IOKitCommonError + 0x2bc;				// general error 
		public const int kIOReturnNoMemory = IOKitCommonError + 0x2bd;			// can't allocate memory 
		public const int kIOReturnNoResources = IOKitCommonError + 0x2be;		// resource shortage 
		public const int kIOReturnIPCError = IOKitCommonError + 0x2bf;			// error during IPC 
		public const int kIOReturnNoDevice = IOKitCommonError + 0x2c0;			// no such device 
		public const int kIOReturnNotPrivileged = IOKitCommonError + 0x2c1;		// privilege violation 
		public const int kIOReturnBadArgument = IOKitCommonError + 0x2c2;		// invalid argument 
		public const int kIOReturnLockedRead = IOKitCommonError + 0x2c3;		// device read locked 
		public const int kIOReturnLockedWrite = IOKitCommonError + 0x2c4;		// device write locked 
		public const int kIOReturnExclusiveAccess = IOKitCommonError + 0x2c5;   // exclusive access and

		// KERN_* codes.
		public const int KERN_SUCCESS = 0;
		public const int KERN_FAILURE = 5;

		// IOKit master port (default port is NULL).
		public static readonly IntPtr kIOMasterPortDefault = IntPtr.Zero;
		// Default CF allocator (default allocator is NULL).
		public static readonly IntPtr kCFAllocatorDefault = IntPtr.Zero;
		// Options for IORegistryCreateIterator(), IORegistryEntryCreateIterator, IORegistryEntrySearchCFProperty()
		public const uint kIORegistryIterateRecursively = 0x00000001;
		public const uint kIORegistryIterateParents = 0x00000002;

		/// <summary>
		/// CF number types
		/// </summary>
		public enum CFNumberType {
			/* Fixed-width types */
			kCFNumberSInt8Type = 1,
			kCFNumberSInt16Type = 2,
			kCFNumberSInt32Type = 3,
			kCFNumberSInt64Type = 4,
			kCFNumberFloat32Type = 5,
			kCFNumberFloat64Type = 6,   /* 64-bit IEEE 754 */
										/* Basic C types */
			kCFNumberCharType = 7,
			kCFNumberShortType = 8,
			kCFNumberIntType = 9,
			kCFNumberLongType = 10,
			kCFNumberLongLongType = 11,
			kCFNumberFloatType = 12,
			kCFNumberDoubleType = 13,
			/* Other */
			kCFNumberCFIndexType = 14,
			//kCFNumberNSIntegerType CF_ENUM_AVAILABLE(10_5, 2_0) = 15,
			//kCFNumberCGFloatType CF_ENUM_AVAILABLE(10_5, 2_0) = 16,
			kCFNumberMaxType = 16
		};

		/// <summary>
		/// CF string encoding types.
		/// </summary>
		public enum CFStringEncoding {
			kCFStringEncodingMacRoman = 0,
			kCFStringEncodingWindowsLatin1 = 0x0500, /* ANSI codepage 1252 */
			kCFStringEncodingISOLatin1 = 0x0201, /* ISO 8859-1 */
			kCFStringEncodingNextStepLatin = 0x0B01, /* NextStep encoding*/
			kCFStringEncodingASCII = 0x0600, /* 0..127 (in creating CFString, values greater than 0x7F are treated as corresponding Unicode value) */
			kCFStringEncodingUnicode = 0x0100, /* kTextEncodingUnicodeDefault  + kTextEncodingDefaultFormat (aka kUnicode16BitFormat) */
			kCFStringEncodingUTF8 = 0x08000100, /* kTextEncodingUnicodeDefault + kUnicodeUTF8Format */
			kCFStringEncodingNonLossyASCII = 0x0BFF, /* 7bit Unicode variants used by Cocoa & Java */

			kCFStringEncodingUTF16 = 0x0100, /* kTextEncodingUnicodeDefault + kUnicodeUTF16Format (alias of kCFStringEncodingUnicode) */
			kCFStringEncodingUTF16BE = 0x10000100, /* kTextEncodingUnicodeDefault + kUnicodeUTF16BEFormat */
			kCFStringEncodingUTF16LE = 0x14000100, /* kTextEncodingUnicodeDefault + kUnicodeUTF16LEFormat */

			kCFStringEncodingUTF32 = 0x0c000100, /* kTextEncodingUnicodeDefault + kUnicodeUTF32Format */
			kCFStringEncodingUTF32BE = 0x18000100, /* kTextEncodingUnicodeDefault + kUnicodeUTF32BEFormat */
			kCFStringEncodingUTF32LE = 0x1c000100 /* kTextEncodingUnicodeDefault + kUnicodeUTF32LEFormat */
		};


		/// <summary>
		/// Returns an iterator to look up available communication devices.
		/// </summary>
		/// <returns>An instance of the type <see cref="IOIterator"/> on success; Otherwise, false.</returns>
		public static IOIterator FindModems() {
			var serialPortIterator = IntPtr.Zero;
			var kernResult = NativeMethods.FindModems(ref serialPortIterator);

			if (KERN_SUCCESS != kernResult) {
				if (SerialPortIOKit.Verbosity > TraceVerbosity.Silent) {
					Trace.TraceError("No modems were found.\n");
				}
				return null;
			}

			if (serialPortIterator == IntPtr.Zero) {
				return null;
			}

			return new IOIterator(serialPortIterator);
		}

		/// <summary>
		/// Gets the path of the given modem service.
		/// </summary>
		/// <param name="modemService">The modem service whose path will be looked up.</param>
		/// <returns>A string.</returns>
		public static string GetModemPath(IOObject modemService) {
			var modemPath = String.Empty;
			var kernResult = NativeMethods.GetModemPath(modemService.Handle, out modemPath);
			if (kernResult != KERN_SUCCESS) {
				return null;
			}

			return modemPath;
		}

		/// <summary>
		/// Gets the corresponding USB device of a modem service.
		/// </summary>
		/// <param name="modemService">A modem service.</param>
		/// <returns>An instance of a <see cref="CFObject"/> on success; Otherwise, false.</returns>
		public static IOObject GetUSBDevice(IOObject modemService) {
			var device = NativeMethods.GetUsbDevice(modemService.Handle);
			if (device != IntPtr.Zero) {
				return new IOObject(device);
			}

			return null;
		}

		/// <summary>
		/// Gets a dictionary value.
		/// </summary>
		/// <param name="dictionary">The dictionary to use.</param>
		/// <param name="key">The key to look up.</param>
		/// <returns>A string on sucess; Otherwise, null.</returns>
		public static string GetCFPropertyString(IOObject dictionary, string key) {
			var value = String.Empty;
			var kernResult = GetCFPropertyString(dictionary, key, out value);
			if (kernResult == KERN_SUCCESS) {
				return value;
			}

			return null;
		}

		/// <summary>
		/// Gets a dictionary value.
		/// </summary>
		/// <param name="dictionary">The dictionary to use.</param>
		/// <param name="key">The key to look up.</param>
		/// <param name="value">Receives the value.</param>
		/// <returns>KERN_SUCCESS on success.</returns>
		public static int GetCFPropertyString(IOObject dictionary, string key, out string value) {
			var maxValueSize = 4096;
			var kernResult = KERN_FAILURE;
			var bsdPathAsCFValue = NativeMethods.IORegistryEntrySearchCFProperty(dictionary.Handle, kIOServicePlane, NativeMethods.__CFStringMakeConstantString(key), kCFAllocatorDefault, kIORegistryIterateRecursively);
			// var bsdPathAsCFString = IORegistryEntryCreateCFProperty(modemService, __CFStringMakeConstantString(key), kCFAllocatorDefault, 0);

			if (bsdPathAsCFValue != IntPtr.Zero) {
				// Convert the value from a CFString to a C (NULL-terminated) string.
				try {
					unsafe {
						fixed (char* bsdValue = new char[maxValueSize]) {
							var result = NativeMethods.CFStringGetCString (bsdPathAsCFValue, bsdValue, maxValueSize, CFStringEncoding.kCFStringEncodingUTF8);

							if (result) {
								kernResult = KERN_SUCCESS;
								value = Marshal.PtrToStringAnsi ((IntPtr)bsdValue);
							} else {
								value = null;
							}
						}
					}
				} finally {
					NativeMethods.CFRelease (bsdPathAsCFValue);
				}
			} else {
				value = null;
			}
				
			return kernResult;
		}

		/// <summary>
		/// Gets a dictionary value.
		/// </summary>
		/// <param name="dictionary">The dictionary to use.</param>
		/// <param name="key">The key to look up.</param>
		/// <returns>A string on sucess; Otherwise, null.</returns>
		public static int GetCFPropertyInt(IOObject dictionary, string key) {
			var result = 0;
			var kernResult = GetCFPropertyInt(dictionary, key, out result);
			if (kernResult == KERN_SUCCESS) {
				return result;
			}

			return 0;
		}

		/// <summary>
		/// Gets a dictionary value.
		/// </summary>
		/// <param name="dictionary">The dictionary to use.</param>
		/// <param name="key">The key to look up.</param>
		/// <param name="value">Receives the value.</param>
		/// <returns>An integer.</returns>
		public static int GetCFPropertyInt(IOObject dictionary, string key, out int value) {
			var kernResult = KERN_FAILURE;
			var bsdPathAsCFValue = NativeMethods.IORegistryEntrySearchCFProperty(dictionary.Handle, kIOServicePlane, NativeMethods.__CFStringMakeConstantString(key), kCFAllocatorDefault, kIORegistryIterateRecursively);
			//var bsdPathAsCFString = IORegistryEntryCreateCFProperty(modemService, __CFStringMakeConstantString(key), kCFAllocatorDefault, 0);

			if (bsdPathAsCFValue != IntPtr.Zero) {
				// Convert the value from a CFNumber to an integer.
				try {
					var bsdValue = IntPtr.Zero;
					var result = NativeMethods.CFNumberGetValue(bsdPathAsCFValue, CFNumberType.kCFNumberIntType, out bsdValue);

					if (result) {
						kernResult = KERN_SUCCESS;
						value = bsdValue.ToInt32();
					} else {
						value = 0;
					}
				} finally {
					NativeMethods.CFRelease (bsdPathAsCFValue);
				}
			} else {
				value = 0;
			}

			return kernResult;
		}

		/// <summary>
		/// Releases a CFObjects handle.
		/// </summary>
		/// <param name="obj">The object to release.</param>
		internal static void CFRelease(CFObject obj) {
			NativeMethods.CFRelease(obj.Handle);
		}

		/// <summary>
		/// Releases a CFObjects handle.
		/// </summary>
		/// <param name="obj">The object to release.</param>
		internal static void IOObjectRelease(IOObject obj) {
			NativeMethods.IOObjectRelease(obj.Handle);
		}

		/// <summary>
		/// Releases a CFObjects handle.
		/// </summary>
		/// <param name="obj">The object to release.</param>
		internal static IOObject IOIteratorNext(IOObject obj) {
			var result = NativeMethods.IOIteratorNext(obj.Handle);
			if (result != IntPtr.Zero) {
				return new IOObject(result);
			}

			return null;
		}

		/// <summary>
		/// Native methods.
		/// </summary>
		private unsafe static class NativeMethods {

			[DllImport(IOKitFrameworkPath, CharSet = CharSet.Ansi)]
			public static extern IntPtr __CFStringMakeConstantString(string str);

			[DllImport(IOKitFrameworkPath, CharSet = CharSet.Ansi)]
			public static extern IntPtr IOServiceMatching(string name);

			[DllImport(IOKitFrameworkPath, CharSet = CharSet.Ansi)]
			public static extern void CFDictionarySetValue(IntPtr theDict, IntPtr key, IntPtr value);

			[DllImport(IOKitFrameworkPath, CharSet = CharSet.Ansi)]
			public static extern int IOServiceGetMatchingServices(IntPtr masterPort, IntPtr matching, out IntPtr iterator);

			[DllImport(IOKitFrameworkPath, CharSet = CharSet.Ansi)]
			public static extern int IOObjectRelease(IntPtr obj);

			[DllImport(IOKitFrameworkPath, CharSet = CharSet.Ansi)]
			public static extern IntPtr IOIteratorNext(IntPtr iterator);

			[DllImport(IOKitFrameworkPath, CharSet = CharSet.Ansi)]
			public static extern IntPtr IORegistryEntryCreateCFProperty(IntPtr entry, IntPtr key, IntPtr allocator, uint options);

			[DllImport(IOKitFrameworkPath, CharSet = CharSet.Ansi)]
			public static extern IntPtr IORegistryEntrySearchCFProperty(IntPtr entry, string plane, IntPtr key, IntPtr allocator, uint options);

			[DllImport(IOKitFrameworkPath, CharSet = CharSet.Ansi)]
			public static extern int CFRelease(IntPtr obj);

			[DllImport(IOKitFrameworkPath, CharSet = CharSet.Ansi)]
			public static extern bool CFStringGetCString(IntPtr theString, char* buffer, long bufferSize, CFStringEncoding encoding);

			[DllImport(IOKitFrameworkPath, CharSet = CharSet.Ansi)]
			public static extern bool CFNumberGetValue(IntPtr number, CFNumberType theType, out IntPtr valuePtr);

			[DllImport(IOKitFrameworkPath, CharSet = CharSet.Ansi)]
			public static extern int IORegistryEntryCreateIterator(IntPtr entry, string plane, uint options, out IntPtr iterator);

			[DllImport(IOKitFrameworkPath, CharSet = CharSet.Ansi)]
			public static extern int IORegistryEntryGetNameInPlane(IntPtr entry, string plane, char* name);

			[DllImport(IOKitFrameworkPath, CharSet = CharSet.Ansi)]
			public static extern bool IOObjectConformsTo(IntPtr obj, string className);

			public static int FindModems(ref IntPtr matchingServices) {
				IntPtr classesToMatch = IOServiceMatching(kIOSerialBSDServiceValue);
				if (classesToMatch == IntPtr.Zero) {
					if (SerialPortIOKit.Verbosity > TraceVerbosity.Silent) {
						Trace.TraceError("IOServiceMatching returned a NULL dictionary.");
					}

					return KERN_FAILURE;
				}

				// Each serial device object has a property with key
				// kIOSerialBSDTypeKey and a value that is one of kIOSerialBSDAllTypes,
				// kIOSerialBSDModemType, or kIOSerialBSDRS232Type. You can experiment with the
				// matching by changing the last parameter in the above call to CFDictionarySetValue.

				// As shipped, this sample is only interested in modems,
				// so add this property to the CFDictionary we're matching on.
				// This will find devices that advertise themselves as modems,
				// such as built-in and USB modems. However, this match won't find serial modems.

				// Look for devices that claim to be modems.
				CFDictionarySetValue(classesToMatch, __CFStringMakeConstantString(kIOSerialBSDTypeKey), __CFStringMakeConstantString(kIOSerialBSDModemType));

				// Get an iterator across all matching devices.
				var kernResult = IOServiceGetMatchingServices(kIOMasterPortDefault, classesToMatch, out matchingServices);
				if (KERN_SUCCESS != kernResult) {
					if (SerialPortIOKit.Verbosity > TraceVerbosity.Silent) {
						Trace.TraceError("IOServiceGetMatchingServices returned {0}\n", kernResult);
					}

					return kernResult;
				}

				return KERN_SUCCESS;
			}

			/// <summary>
			/// Given an iterator across a set of modems, return the BSD path to the first one with the callout device path matching MATCH_PATH if defined. 
			/// If MATCH_PATH is not defined, return the first device found.
			/// If no modems are found the path name is set to an empty string.
			/// </summary>
			/// <returns>.</returns>
			/// <param name="serialPortIterator">Serial port iterator.</param>
			/// <param name="path">Path.</param>
			public static int GetModemPath(IntPtr modemService, out string path) {
				//var modemService = IntPtr.Zero;
				var maxPathSize = 4096;
				var kernResult = KERN_FAILURE;

				path = String.Empty;

				var bsdPathAsCFString = IORegistryEntryCreateCFProperty(modemService, __CFStringMakeConstantString(kIOCalloutDeviceKey), kCFAllocatorDefault, 0);

				if (bsdPathAsCFString != IntPtr.Zero) {
					// Convert the path from a CFString to a C (NUL-terminated) string for use
					// with the POSIX open() call.
					try {
						unsafe {						
							fixed (char* bsdPath = new char[maxPathSize]) {
								var result = CFStringGetCString (bsdPathAsCFString, bsdPath, maxPathSize, CFStringEncoding.kCFStringEncodingUTF8);


								if (result) {
									var bsdPathStr = Marshal.PtrToStringAnsi ((IntPtr)bsdPath);

									if (SerialPortIOKit.Verbosity > TraceVerbosity.ErrorAndWarning) {
										Trace.TraceInformation ("Modem found with BSD path: {0}", bsdPathStr);
									}

									kernResult = KERN_SUCCESS;
									path = bsdPathStr;
								}
							}
						}
					} finally {
						CFRelease (bsdPathAsCFString);
					}
				}

				return kernResult;
			}


			public static IntPtr GetUsbDevice(IntPtr modemService) {
				var iterator = IntPtr.Zero;
				var device = IntPtr.Zero;

				if (modemService == IntPtr.Zero) {
					return IntPtr.Zero;
				}

				var status = IORegistryEntryCreateIterator(modemService, kIOServicePlane, kIORegistryIterateParents | kIORegistryIterateRecursively, out iterator);
				if (status == kIOReturnSuccess) {
					var currentService = IntPtr.Zero;
					while ((currentService = IOIteratorNext(iterator)) != IntPtr.Zero) {
						unsafe {
							fixed (char* serviceName = new char[4096]) {
								status = IORegistryEntryGetNameInPlane (currentService, kIOServicePlane, serviceName);
							}

							if ((status == kIOReturnSuccess) && (IOObjectConformsTo (currentService, kIOUSBDeviceClassName))) {
								device = currentService;
								break;
							} else {
								// Release the service object which is no longer needed
								IOObjectRelease (currentService);
							}
						}
					}

					// Release the iterator
					IOObjectRelease(iterator);
				}

				return device;
			}

		}


	}

}
