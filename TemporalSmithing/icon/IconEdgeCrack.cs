using System;
using Cairo;

namespace temporalsmithing.icon; 

public class IconEdgeCrack {

	public static void Drawedgecrack_svg(Context cr, int x, int y, float width, float height, double[] rgba) {
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
		cr.MoveTo(117.9375, 50.5);
		cr.LineTo(120.46875, 150.40625);
		cr.LineTo(225.78125, 209.375);
		cr.ClosePath();
		cr.MoveTo(378.84375, 73.09375);
		cr.LineTo(281.40625, 109.0625);
		cr.LineTo(302.34375, 280);
		cr.ClosePath();
		cr.MoveTo(494.0625, 151.84375);
		cr.LineTo(360.152344, 211.78125);
		cr.LineTo(427.71875, 331.53125);
		cr.LineTo(433.371094, 341.59375);
		cr.LineTo(422.371094, 345.0625);
		cr.LineTo(340.308594, 370.84375);
		cr.LineTo(397.746094, 420.09375);
		cr.LineTo(421.496094, 440.46875);
		cr.LineTo(390.46875, 436.46875);
		cr.LineTo(136.246094, 403.65625);
		cr.LineTo(100.839844, 399.09375);
		cr.LineTo(133.933594, 385.71875);
		cr.LineTo(261.121094, 334.371094);
		cr.LineTo(173.5, 295.03125);
		cr.LineTo(19.75, 363.90625);
		cr.LineTo(19.75, 494.5625);
		cr.LineTo(494.0625, 494.5625);
		cr.ClosePath();
		cr.MoveTo(136.28125, 202.3125);
		cr.LineTo(39.75, 224.96875);
		cr.LineTo(286.59375, 297.3125);
		cr.ClosePath();
		cr.MoveTo(136.28125, 202.3125);
		cr.Tolerance = 0.1;
		cr.Antialias = Antialias.Default;
		cr.FillRule = FillRule.Winding;
		cr.FillPreserve();
		pattern.Dispose();

		cr.Restore();
	}



}