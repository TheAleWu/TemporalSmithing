using System;
using Cairo;

namespace temporalsmithing.icon;

public class IconHammer {

	public static void Drawhammer_svg(Context cr, int x, int y, float width, float height, double[] rgba) {
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
		cr.MoveTo(132.8125, 27.625);
		cr.LineTo(162.40625, 161.75);
		cr.CurveTo(166.953125, 165.339844, 172.085938, 165.9375, 180.46875, 163.71875);
		cr.LineTo(185.65625, 162.34375);
		cr.LineTo(189.46875, 166.15625);
		cr.LineTo(273.6875, 250.34375);
		cr.LineTo(277.464844, 254.125);
		cr.LineTo(276.121094, 259.3125);
		cr.CurveTo(273.703125, 268.675781, 274.640625, 274.359375, 279.496094, 279.21875);
		cr.LineTo(293.027344, 292.75);
		cr.LineTo(390.5625, 195.1875);
		cr.LineTo(377.027344, 181.65625);
		cr.CurveTo(373.492188, 178.117188, 367.640625, 177.242188, 357.375, 180.21875);
		cr.LineTo(352.0625, 181.746094);
		cr.LineTo(348.15625, 177.84375);
		cr.LineTo(261.96875, 91.652344);
		cr.LineTo(260.34375, 88.375);
		cr.LineTo(259.246094, 85.058594);
		cr.LineTo(259.246094, 64.375);
		cr.CurveTo(259.234375, 64.355469, 259.230469, 64.332031, 259.21875, 64.3125);
		cr.LineTo(132.808594, 27.625);
		cr.ClosePath();
		cr.MoveTo(310.59375, 85.5);
		cr.LineTo(296.4375, 99.6875);
		cr.LineTo(340.21875, 143.46875);
		cr.LineTo(354.402344, 129.3125);
		cr.ClosePath();
		cr.MoveTo(430.84375, 181.34375);
		cr.LineTo(270.875, 341.3125);
		cr.LineTo(307.9375, 378.4375);
		cr.LineTo(467.9375, 218.4375);
		cr.ClosePath();
		cr.MoveTo(196.5, 199.59375);
		cr.LineTo(21.5, 374.59375);
		cr.LineTo(21.5, 462.1875);
		cr.LineTo(240.3125, 243.40625);
		cr.ClosePath();
		cr.MoveTo(196.5, 199.59375);
		cr.Tolerance = 0.1;
		cr.Antialias = Antialias.Default;
		cr.FillRule = FillRule.Winding;
		cr.FillPreserve();
		pattern.Dispose();

		cr.Restore();
	}

}