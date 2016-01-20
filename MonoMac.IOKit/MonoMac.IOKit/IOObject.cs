using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kinematics.PowerBrainCommunication.IOKitFramework {

	/// <summary>
	/// Manages native IOObject references.
	/// </summary>
	internal class IOObject : IDisposable {
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
					IOKitInterop.IOObjectRelease(this);
					handle = IntPtr.Zero;
				}

				disposedValue = true;
			}
		}

	}
}
