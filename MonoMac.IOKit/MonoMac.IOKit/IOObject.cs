using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoMac.IOKit {

	/// <summary>
	/// Manages native IOObject references.
	/// </summary>
	public class IOObject : IDisposable {
		protected IntPtr handle;
		protected bool disposedValue = false;


		/// <summary>
		/// Gets the native handle.
		/// </summary>
		public IntPtr Handle {
			get { return handle; }
		}

		/// <summary>
		/// Initializes a new instance of the see <see cref="IOObject"/>.
		/// </summary>
		public IOObject(IntPtr handle) {
			this.handle = handle;
		}

		/// <summary>
		/// Finalizes this instance.
		/// </summary>
		~IOObject() {
			Dispose(false);
		}

		/// <summary>
		/// Frees managed and unmanaged resources.
		/// </summary>
		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Frees managed and unmanaged resources.
		/// </summary>
		/// <param name="disposing">True if called from user space; Otherwise, false.</param>
		protected virtual void Dispose(bool disposing) {
			if (!disposedValue) {
				if (disposing) {
					// dispose managed state (managed objects).
				}

				if (handle != IntPtr.Zero) {
					IOKitFramework.IOObjectRelease(this);
					handle = IntPtr.Zero;
				}

				disposedValue = true;
			}
		}

		/// <summary>
		/// Gets a properties value.
		/// </summary>
		/// <param name="keyName">The key name to look up.</param>
		/// <returns>The keys value on success; Otherwise, null.</returns>
		public string GetCFPropertyString(string keyName) {
			string value = null;
			IOKitFramework.GetCFPropertyString(this, keyName, out value);

			return value;
		}

		/// <summary>
		/// Gets a properties value.
		/// </summary>
		/// <param name="keyName">The key name to look up.</param>
		/// <returns>The keys value on success; Otherwise, null.</returns>
		public int GetCFPropertyInt(string keyName) {
			return IOKitFramework.GetCFPropertyInt(this, keyName);
		}

	}
}
