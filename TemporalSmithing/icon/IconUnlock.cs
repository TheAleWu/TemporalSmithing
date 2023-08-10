using System;
using Cairo;

namespace temporalsmithing.icon;

public class IconUnlock {

	public static void Drawunlock_svg(Context cr, int x, int y, float width, float height, double[] rgba) {
		Pattern pattern;
		var matrix = cr.Matrix;

		cr.Save();
		float w = 448;
		float h = 512;
		var scale = Math.Min(width / w, height / h);
		matrix.Translate(x + Math.Max(0, (width - w * scale) / 2), y + Math.Max(0, (height - h * scale) / 2));
		matrix.Scale(scale, scale);
		cr.Matrix = matrix;

		cr.Operator = Operator.Over;
		pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
		cr.SetSource(pattern);

		cr.NewPath();
		cr.MoveTo(224, 64);
		cr.CurveTo(179.800781, 64, 144, 99.820313, 144, 144);
		cr.LineTo(144, 192);
		cr.LineTo(384, 192);
		cr.CurveTo(419.300781, 192, 448, 220.699219, 448, 256);
		cr.LineTo(448, 448);
		cr.CurveTo(448, 483.300781, 419.300781, 512, 384, 512);
		cr.LineTo(64, 512);
		cr.CurveTo(28.648438, 512, 0, 483.300781, 0, 448);
		cr.LineTo(0, 256);
		cr.CurveTo(0, 220.699219, 28.648438, 192, 64, 192);
		cr.LineTo(80, 192);
		cr.LineTo(80, 144);
		cr.CurveTo(80, 64.46875, 144.5, 0, 224, 0);
		cr.CurveTo(281.5, 0, 331, 33.691406, 354.101563, 82.269531);
		cr.CurveTo(361.699219, 98.230469, 354.898438, 117.300781, 338.101563, 124.898438);
		cr.CurveTo(322.101563, 132.5, 303.898438, 125.699219, 296.300781, 109.699219);
		cr.CurveTo(283.398438, 82.628906, 255.898438, 64, 224, 64);
		cr.ClosePath();
		cr.MoveTo(256, 384);
		cr.CurveTo(273.699219, 384, 288, 369.699219, 288, 352);
		cr.CurveTo(288, 334.300781, 273.699219, 320, 256, 320);
		cr.LineTo(192, 320);
		cr.CurveTo(174.300781, 320, 160, 334.300781, 160, 352);
		cr.CurveTo(160, 369.699219, 174.300781, 384, 192, 384);
		cr.ClosePath();
		cr.MoveTo(256, 384);
		cr.Tolerance = 0.1;
		cr.Antialias = Antialias.Default;
		cr.FillRule = FillRule.Winding;
		cr.FillPreserve();
		pattern.Dispose();

		cr.Restore();
	}

}