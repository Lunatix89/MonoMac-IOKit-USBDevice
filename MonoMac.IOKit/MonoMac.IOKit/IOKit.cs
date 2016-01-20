/*
using System;
using System.Runtime.InteropServices;
using MonoMac.Foundation;

namespace Kinematics.PowerBrainCommunication
{

	internal sealed class IOKit
	{
		internal const int KERN_SUCCESS = 0;
		internal static readonly IntPtr MACH_PORT_NULL = IntPtr.Zero;
		internal const int kIOReturnNoDevice =  -536870208;

		private const string IOKitFrameworkPath = "IOKit.framework/IOKit";

		internal enum ASCommands : byte {Awake, Sleep}

		[DllImport (IOKitFrameworkPath, CharSet = CharSet.Ansi)]
		internal static extern IntPtr IORegistryEntryFromPath (IntPtr masterPort, string path);

		internal static IOKitObject IORegistryEntryFromPath (string path) {
			IntPtr r = IORegistryEntryFromPath (IntPtr.Zero, path);
			if (r == MACH_PORT_NULL) {
				throw new IOKitException (-1);
			}

			return r;
		}

		[DllImport (IOKitFrameworkPath)]
		internal static extern void IOObjectRelease (int @object);


		[DllImport (IOKitFrameworkPath)]
		internal static extern int IOMasterPort(IntPtr bootstrapPort, out IntPtr masterPort);

		internal static IntPtr IOMasterPort() {
			IntPtr masterPort;
			CheckResult(IOMasterPort(MACH_PORT_NULL, out masterPort));

			return masterPort;
		}

		internal static void CheckResult(int errcode) {
			if (errcode != KERN_SUCCESS) {
				throw new IOKitException (errcode);
			}
		}

		[DllImport (IOKitFrameworkPath, CharSet = CharSet.Ansi)]
		internal static extern IntPtr IOServiceMatching(string name);

		[DllImport (IOKitFrameworkPath)]
		static extern IntPtr IORegistryEntryCreateCFProperty(IntPtr entry, IntPtr key, IntPtr allocator, uint options);

		[DllImport (IOKitFrameworkPath, CharSet = CharSet.Ansi)]
		public static extern int CFDictionarySetValue (IntPtr @a, string b, string c);


		public static T GetCFProperty<T> (IntPtr iterator, string propertyName) where T : NSObject {
			NSString key = (NSString)propertyName;
			IntPtr result = IORegistryEntryCreateCFProperty (iterator, key.Handle, IntPtr.Zero, 0);
			if (result != IntPtr.Zero) {
				// var nsObject = new NSObject (result);

				return (T)Activator.CreateInstance (typeof(T), new object[] { result });
			}

			return default(T);
		}

		public static int GetCFPropertyAsInt (IntPtr iterator, string propertyName) {
			NSString key = (NSString)propertyName;
			IntPtr result = IORegistryEntryCreateCFProperty (iterator, key.Handle, IntPtr.Zero, 0);
			if (result != IntPtr.Zero) {
				return result.ToInt32();
			}

			return 0;
		}

		[DllImport(IOKitFrameworkPath)]
		internal static extern int IOServiceGetMatchingServices (
			IntPtr masterPort, IntPtr matchingDictionary, out IntPtr iterator);

		internal static IOKitObject IOServiceGetMatchingServices(IntPtr masterPort, IntPtr matchingDictionary) {
			IntPtr iterator;
			CheckResult(IOServiceGetMatchingServices(masterPort, matchingDictionary, out iterator));

			return iterator;
		}

		[DllImport(IOKitFrameworkPath)]
		internal static extern IntPtr IOIteratorNext (IntPtr iterator);

		internal static IOKitObject IOIteratorNext(IOKitObject iterator) {
			IntPtr service = IOIteratorNext ((IntPtr) iterator);

			if (service.ToInt32() == IOKit.kIOReturnNoDevice) {
				throw new IOKitException (IOKit.kIOReturnNoDevice);
			}

			return service;
		}

		[DllImport(IOKitFrameworkPath)]
		internal static extern void IOObjectRelease(IntPtr @object);

		[DllImport(IOKitFrameworkPath)]
		internal static extern int IOServiceOpen(IntPtr service, IntPtr owningTask, UInt32 type, out IntPtr connect);

		internal static IOKitObject IOServiceOpen(IOKitObject service) {
			IntPtr connect;
			CheckResult (IOServiceOpen (service, mach_task_self (), 0, out connect));

			return connect;
		}

		[DllImport(IOKitFrameworkPath)]
		internal static extern IntPtr mach_task_self();

		[DllImport(IOKitFrameworkPath)]
		internal static extern int IOConnectCallStructMethod (
			IntPtr connection, UInt32 selector, sbyte[] inputStruct,
			uint inputStructCnt, sbyte[] outputStruct, ref uint outputStructCnt);

		static sbyte[] IOConnectCallStructMethodIn = null;

		internal static sbyte[] IOConnectCallStructMethod(IOKitObject connect) {
			uint osize = 40, isize = 40;

			sbyte[] @out = new sbyte[osize];
			if (IOConnectCallStructMethodIn == null) {
				IOConnectCallStructMethodIn = new sbyte[isize];

				for (int i = 0; i<isize; i++) {
					IOConnectCallStructMethodIn [i] = 1;
				}
			}

			IOKit.CheckResult (IOKit.IOConnectCallStructMethod (
				connect, 5, IOConnectCallStructMethodIn, osize, @out, ref isize
			));

			return @out;
		}
	}

	internal class IOKitObject : IDisposable {
		IntPtr value;

		public IOKitObject(IntPtr val) {
			value = val;
		}

		public void Dispose() {
			IOKit.IOObjectRelease (value);
		}

		public static implicit operator IntPtr(IOKitObject obj) {
			return obj.value;
		}

		public static implicit operator IOKitObject(IntPtr val) {
			return new IOKitObject (val);
		}

	}

	internal class IOKitException: Exception {
		public IOKitException(int errcode) : base("Error code: 0x" + errcode.ToString("X")) {
		}
	}

}

*/