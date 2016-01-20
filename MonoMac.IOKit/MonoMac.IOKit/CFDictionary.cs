using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kinematics.PowerBrainCommunication.IOKitFramework {

	/// <summary>
	/// Manages a native CFDictionary object.
	/// </summary>
	internal class CFDictionary : CFObject {

		/// <summary>
		/// Initializes a new instance of the see <see cref="CFObject"/>.
		/// </summary>
		public CFDictionary(IntPtr handle) :
				base(handle) {

		}

		/// <summary>
		/// Gets a properties value.
		/// </summary>
		/// <param name="keyName">The key name to look up.</param>
		/// <returns>The keys value on success; Otherwise, null.</returns>
		public string GetCFPropertyString(string keyName) {
			string value = null;
			IOKitInterop.GetCFPropertyString(this, keyName, out value);

			return value;
		}

		/// <summary>
		/// Gets a properties value.
		/// </summary>
		/// <param name="keyName">The key name to look up.</param>
		/// <returns>The keys value on success; Otherwise, null.</returns>
		public int GetCFPropertyInt(string keyName) {
			return IOKitInterop.GetCFPropertyInt(this, keyName);
		}
	}

}
