using System;
using Cairo;

public class IconLockedFortress {

	public static void DrawlockedFortress_svg(Context cr, int x, int y, float width, float height, double[] rgba) {
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
		cr.MoveTo(102.78125, 20.625);
		cr.LineTo(102.78125, 113.5);
		cr.LineTo(149, 165.21875);
		cr.LineTo(149, 348.25);
		cr.LineTo(123.125, 348.25);
		cr.LineTo(99.53125, 489.15625);
		cr.LineTo(419.46875, 489.15625);
		cr.LineTo(395.875, 348.25);
		cr.LineTo(370.03125, 348.25);
		cr.LineTo(370.03125, 165.21875);
		cr.LineTo(416.25, 113.5);
		cr.LineTo(416.25, 20.625);
		cr.LineTo(362.3125, 20.625);
		cr.LineTo(362.3125, 64.59375);
		cr.LineTo(329.46875, 64.59375);
		cr.LineTo(329.46875, 20.625);
		cr.LineTo(275.53125, 20.625);
		cr.LineTo(275.53125, 64.59375);
		cr.LineTo(243.09375, 64.59375);
		cr.LineTo(243.09375, 20.625);
		cr.LineTo(189.15625, 20.625);
		cr.LineTo(189.15625, 64.59375);
		cr.LineTo(156.71875, 64.59375);
		cr.LineTo(156.71875, 20.625);
		cr.ClosePath();
		cr.MoveTo(263.8125, 157.71875);
		cr.CurveTo(293.984375, 157.71875, 318.4375, 182.175781, 318.4375, 212.34375);
		cr.CurveTo(318.4375, 233.183594, 306.789063, 251.289063, 289.625, 260.5);
		cr.LineTo(317.25, 400.75);
		cr.LineTo(210.375, 400.75);
		cr.LineTo(238, 260.5);
		cr.CurveTo(220.851563, 251.285156, 209.1875, 233.175781, 209.1875, 212.34375);
		cr.CurveTo(209.1875, 182.175781, 233.644531, 157.71875, 263.8125, 157.71875);
		cr.ClosePath();
		cr.MoveTo(263.8125, 157.71875);
		cr.Tolerance = 0.1;
		cr.Antialias = Antialias.Default;
		cr.FillRule = FillRule.Winding;
		cr.FillPreserve();
		pattern.Dispose();

		cr.Restore();
	}



}
