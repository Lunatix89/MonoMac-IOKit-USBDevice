using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kinematics.PowerBrainCommunication.IOKitFramework {

	/// <summary>
	/// Manages IOIterator handles.
	/// </summary>
	internal class IOIterator : IOObject {

		/// <summary>
		/// Initializes a new instance of the see <see cref="IOObject"/>.
		/// </summary>
		public IOIterator(IntPtr handle) :
			base(handle) {

		}

		/// <summary>
		/// Iterates one step forward.
		/// </summary>
		/// <returns>A <see cref="IOObject"/> instance on success; Null if the iterator has reached it's end.</returns>
		public IOObject Next() {
			if (disposedValue) {
				throw new ObjectDisposedException("handle");
			}

			if (handle == IntPtr.Zero) {
				throw new InvalidOperationException("Can not iterate over uninitialized objects.");
			}

			return IOKitInterop.IOIteratorNext(this);
		}

	}

}
