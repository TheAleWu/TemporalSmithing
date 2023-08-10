using System.Drawing;

namespace temporalsmithing.util;

public class ColorToHex {

	private ColorToHex() {
		// No instantiation
	}

	public static string Transform(Color color) {
		return color.R.ToString("X2") +
			   color.G.ToString("X2") +
			   color.B.ToString("X2");
	}

}