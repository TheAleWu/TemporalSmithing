using System;
using Cairo;

namespace temporalsmithing.icon;

public class IconDroplet {

	public static void Drawdroplet_svg(Context cr, int x, int y, float width, float height, double[] rgba) {
		Pattern pattern;
		var matrix = cr.Matrix;

		cr.Save();
		float w = 384;
		float h = 512;
		var scale = Math.Min(width / w, height / h);
		matrix.Translate(x + Math.Max(0, (width - w * scale) / 2), y + Math.Max(0, (height - h * scale) / 2));
		matrix.Scale(scale, scale);
		cr.Matrix = matrix;

		cr.Operator = Operator.Over;
		pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
		cr.SetSource(pattern);

		cr.NewPath();
		cr.MoveTo(16, 319.101563);
		cr.CurveTo(16, 245.898438, 118.300781, 89.429688, 166.898438, 19.300781);
		cr.CurveTo(179.199219, 1.585938, 204.800781, 1.585938, 217.101563, 19.300781);
		cr.CurveTo(265.699219, 89.429688, 368, 245.898438, 368, 319.101563);
		cr.CurveTo(368, 417.199219, 289.199219, 496, 192, 496);
		cr.CurveTo(94.800781, 496, 16, 417.199219, 16, 319.101563);
		cr.ClosePath();
		cr.MoveTo(112, 319.101563);
		cr.CurveTo(112, 311.199219, 104.800781, 303.101563, 96, 303.101563);
		cr.CurveTo(87.160156, 303.101563, 80, 311.199219, 80, 319.101563);
		cr.CurveTo(80, 381.898438, 130.101563, 432, 192, 432);
		cr.CurveTo(200.800781, 432, 208, 424.800781, 208, 416);
		cr.CurveTo(208, 407.199219, 200.800781, 400, 192, 400);
		cr.CurveTo(147.800781, 400, 112, 364.199219, 112, 319.101563);
		cr.ClosePath();
		cr.MoveTo(112, 319.101563);
		cr.Tolerance = 0.1;
		cr.Antialias = Antialias.Default;
		cr.FillRule = FillRule.Winding;
		cr.FillPreserve();
		pattern.Dispose();

		cr.Restore();
	}

}