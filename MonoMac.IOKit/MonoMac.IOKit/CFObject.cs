using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoMac.IOKit {

	/// <summary>
	/// Manages native CFObjects references.
	/// </summary>
	public class CFObject : IDisposable {
		private IntPtr handle;
		private bool disposedValue = false; // To detect redundant calls

		/// <summary>
		/// Gets the native handle.
		/// </summary>
		public IntPtr Handle {
			get { return handle; }
		}

		/// <summary>
		/// Initializes a new instance of the see <see cref="CFObject"/>.
		/// </summary>
		public CFObject(IntPtr handle) {
			this.handle = handle;
		}

		/// <summary>
		/// Finalizes this instance.
		/// </summary>
		~CFObject() {
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
					IOKitInterop.CFRelease(this);
					handle = IntPtr.Zero;
				}

				disposedValue = true;
			}
		}

	}

}
