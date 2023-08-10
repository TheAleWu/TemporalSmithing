using System;
using Cairo;

namespace temporalsmithing.icon;

public class IconQuestion {

	public static void Drawquestion_svg(Context cr, int x, int y, float width, float height, double[] rgba) {
		Pattern pattern;
		var matrix = cr.Matrix;

		cr.Save();
		float w = 320;
		float h = 512;
		var scale = Math.Min(width / w, height / h);
		matrix.Translate(x + Math.Max(0, (width - w * scale) / 2), y + Math.Max(0, (height - h * scale) / 2));
		matrix.Scale(scale, scale);
		cr.Matrix = matrix;

		cr.Operator = Operator.Over;
		pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
		cr.SetSource(pattern);

		cr.NewPath();
		cr.MoveTo(204.300781, 32.011719);
		cr.LineTo(96, 32.011719);
		cr.CurveTo(43.058594, 32.011719, 0, 75.070313, 0, 128.011719);
		cr.CurveTo(0, 145.679688, 14.308594, 159.109375, 32, 159.109375);
		cr.CurveTo(49.691406, 159.109375, 64, 144.789063, 64, 128.011719);
		cr.CurveTo(64, 110.371094, 78.339844, 96.011719, 96, 96.011719);
		cr.LineTo(204.300781, 96.011719);
		cr.CurveTo(232.800781, 96.011719, 256, 119.199219, 256, 147.800781);
		cr.CurveTo(256, 167.519531, 245.03125, 185.269531, 225.5, 195.128906);
		cr.LineTo(127.800781, 252.398438);
		cr.CurveTo(117.101563, 258.199219, 112, 268.699219, 112, 280);
		cr.LineTo(112, 320);
		cr.CurveTo(112, 337.671875, 126.308594, 351.988281, 144, 351.988281);
		cr.CurveTo(161.691406, 351.988281, 176, 337.671875, 176, 320);
		cr.LineTo(176, 298.300781);
		cr.LineTo(256, 251.300781);
		cr.CurveTo(295.46875, 231.550781, 320, 191.878906, 320, 147.800781);
		cr.CurveTo(320, 83.949219, 268.101563, 32.011719, 204.300781, 32.011719);
		cr.ClosePath();
		cr.MoveTo(144, 400);
		cr.CurveTo(121.910156, 400, 104, 417.910156, 104, 440);
		cr.CurveTo(104, 462.089844, 121.910156, 479.101563, 144, 479.101563);
		cr.CurveTo(166.089844, 479.101563, 184, 461.199219, 184, 440);
		cr.CurveTo(184, 418.800781, 166.101563, 400, 144, 400);
		cr.ClosePath();
		cr.MoveTo(144, 400);
		cr.Tolerance = 0.1;
		cr.Antialias = Antialias.Default;
		cr.FillRule = FillRule.Winding;
		cr.FillPreserve();
		pattern.Dispose();

		cr.Restore();
	}

}