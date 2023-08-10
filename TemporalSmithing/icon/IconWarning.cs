using System;
using Cairo;

namespace temporalsmithing.icon;

public class IconWarning {

	public static void Drawwarning_svg(Context cr, int x, int y, float width, float height, double[] rgba) {
		Pattern pattern;
		var matrix = cr.Matrix;

		cr.Save();
		float w = 512;
		float h = 512;
		var scale = Math.Min(width / w, height / h);
		matrix.Translate(x + Math.Max(0, (width - w * scale) / 2), y + Math.Max(0, (height - h * scale) / 2));
		matrix.Scale(scale, scale);
		cr.Matrix = matrix;

		cr.Operator = Operator.Over;
		pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
		cr.SetSource(pattern);

		cr.NewPath();
		cr.MoveTo(506.300781, 417);
		cr.LineTo(293, 53);
		cr.CurveTo(276.671875, 25, 235.460938, 25, 219.019531, 53);
		cr.LineTo(5.820313, 417);
		cr.CurveTo(-10.589844, 444.898438, 9.847656, 480, 42.738281, 480);
		cr.LineTo(469.339844, 480);
		cr.CurveTo(502.101563, 480, 522.601563, 445, 506.300781, 417);
		cr.ClosePath();
		cr.MoveTo(232, 168);
		cr.CurveTo(232, 154.75, 242.75, 144, 256, 144);
		cr.CurveTo(269.25, 144, 280, 154.800781, 280, 168);
		cr.LineTo(280, 296);
		cr.CurveTo(280, 309.25, 269.25, 320, 256.898438, 320);
		cr.CurveTo(244.550781, 320, 232, 309.300781, 232, 296);
		cr.ClosePath();
		cr.MoveTo(256, 416);
		cr.CurveTo(238.640625, 416, 224.558594, 401.921875, 224.558594, 384.558594);
		cr.CurveTo(224.558594, 367.199219, 238.628906, 353.121094, 256, 353.121094);
		cr.CurveTo(273.371094, 353.121094, 287.441406, 367.199219, 287.441406, 384.558594);
		cr.CurveTo(287.398438, 401.898438, 273.398438, 416, 256, 416);
		cr.ClosePath();
		cr.MoveTo(256, 416);
		cr.Tolerance = 0.1;
		cr.Antialias = Antialias.Default;
		cr.FillRule = FillRule.Winding;
		cr.FillPreserve();
		pattern.Dispose();

		cr.Restore();
	}

}