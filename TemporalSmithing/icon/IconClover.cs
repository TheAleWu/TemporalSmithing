using System;
using Cairo;

public class IconClover {

	public static void Drawclover_svg(Context cr, int x, int y, float width, float height, double[] rgba) {
		Pattern pattern;
		Matrix matrix = cr.Matrix;

		cr.Save();
		float w = 512;
		float h = 512;
		float scale = Math.Min(width / w, height / h);
		matrix.Translate(x + Math.Max(0, (width - w * scale) / 2), y + Math.Max(0, (height - h * scale) / 2));
		matrix.Scale(scale, scale);
		cr.Matrix = matrix;

		cr.Operator = Operator.Over;
		pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
		cr.SetSource(pattern);

		cr.NewPath();
		cr.MoveTo(105.1875, 26.425781);
		cr.CurveTo(66.871094, 32.425781, 43.054688, 52.761719, 73.816406, 99.421875);
		cr.CurveTo(6.191406, 97.785156, -9.359375, 188.75, 62.289063, 218.390625);
		cr.CurveTo(117.128906, 241.074219, 144.296875, 212.378906, 205.273438, 233.398438);
		cr.CurveTo(141.261719, 225.519531, 86.132813, 255.09375, 51.460938, 299.996094);
		cr.CurveTo(12.953125, 349.867188, 27.355469, 420.144531, 91.871094, 400.328125);
		cr.CurveTo(82.847656, 467.449219, 154.257813, 500.46875, 195.777344, 447.496094);
		cr.CurveTo(231.570313, 401.835938, 211.289063, 343.738281, 237.621094, 264.609375);
		cr.CurveTo(244.71875, 326.316406, 232.003906, 373.371094, 249.191406, 431.089844);
		cr.CurveTo(270.585938, 502.917969, 352.269531, 506.695313, 367.730469, 437.429688);
		cr.CurveTo(423.570313, 488.738281, 463.550781, 428.019531, 443.484375, 360.785156);
		cr.CurveTo(425.492188, 300.5, 352.722656, 262.585938, 275.835938, 242.082031);
		cr.CurveTo(348.640625, 239.050781, 384.679688, 286.59375, 443.613281, 272.167969);
		cr.CurveTo(516.058594, 254.433594, 507.980469, 163.742188, 432.789063, 143.246094);
		cr.CurveTo(478.558594, 88.328125, 413.34375, 33.199219, 345.609375, 50.128906);
		cr.CurveTo(288.496094, 64.402344, 254.761719, 122.105469, 243.683594, 193.457031);
		cr.CurveTo(238.132813, 127.839844, 214.859375, 73.320313, 162.652344, 43.140625);
		cr.CurveTo(143.011719, 31.789063, 122.601563, 26.402344, 105.1875, 26.429688);
		cr.ClosePath();
		cr.MoveTo(105.1875, 26.425781);
		cr.Tolerance = 0.1;
		cr.Antialias = Antialias.Default;
		cr.FillRule = FillRule.Winding;
		cr.FillPreserve();
		pattern.Dispose();

		cr.Restore();
	}

}
