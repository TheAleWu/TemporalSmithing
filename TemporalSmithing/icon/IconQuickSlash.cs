using System;
using Cairo;

namespace temporalsmithing.icon;

public class IconQuickSlash {

	public static void Drawquickslash_svg(Context cr, int x, int y, float width, float height, double[] rgba) {
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
		cr.MoveTo(270.878906, 444.542969);
		cr.CurveTo(576.855469, 496.617188, 318.441406, 29.007813, 23.097656, 25.679688);
		cr.CurveTo(447.570313, -7.507813, 696.863281, 640.746094, 270.878906, 444.539063);
		cr.ClosePath();
		cr.MoveTo(270.878906, 444.542969);
		cr.Tolerance = 0.1;
		cr.Antialias = Antialias.Default;
		cr.FillRule = FillRule.Winding;
		cr.FillPreserve();
		pattern.Dispose();

		cr.Restore();
	}



}