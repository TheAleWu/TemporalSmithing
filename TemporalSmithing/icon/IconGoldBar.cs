using System;
using Cairo;

namespace temporalsmithing.icon;

public class IconGoldBar {

	public static void Drawgoldbar_svg(Context cr, int x, int y, float width, float height, double[] rgba) {
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
    	cr.MoveTo(341.28125, 22.78125);
    	cr.LineTo(254, 123.75);
    	cr.LineTo(317.15625, 90.09375);
    	cr.LineTo(321.59375, 87.71875);
    	cr.LineTo(326.03125, 90.125);
    	cr.LineTo(344.34375, 100.0625);
    	cr.ClosePath();
    	cr.MoveTo(129.8125, 46.5625);
    	cr.LineTo(154.1875, 176.96875);
    	cr.LineTo(242.90625, 129.6875);
    	cr.ClosePath();
    	cr.MoveTo(321.53125, 108.96875);
    	cr.LineTo(83.34375, 235.90625);
    	cr.LineTo(192.15625, 293.5);
    	cr.LineTo(429.625, 167.75);
    	cr.ClosePath();
    	cr.MoveTo(364.28125, 153.65625);
    	cr.LineTo(389.875, 166.6875);
    	cr.LineTo(192.5625, 270.625);
    	cr.LineTo(125.6875, 234.9375);
    	cr.LineTo(151.03125, 221.53125);
    	cr.LineTo(192.5625, 243.722656);
    	cr.ClosePath();
    	cr.MoveTo(456.46875, 171.875);
    	cr.LineTo(465.5625, 195.90625);
    	cr.LineTo(491.78125, 173.25);
    	cr.ClosePath();
    	cr.MoveTo(440.6875, 183.03125);
    	cr.LineTo(199.46875, 310.78125);
    	cr.LineTo(193.6875, 394.15625);
    	cr.LineTo(465.75, 249.34375);
    	cr.ClosePath();
    	cr.MoveTo(106.125, 202.59375);
    	cr.LineTo(16.5625, 226.78125);
    	cr.LineTo(53.4375, 246.34375);
    	cr.LineTo(59.53125, 230.0625);
    	cr.LineTo(60.78125, 226.75);
    	cr.LineTo(63.90625, 225.09375);
    	cr.LineTo(106.128906, 202.59375);
    	cr.ClosePath();
    	cr.MoveTo(71.6875, 250.90625);
    	cr.LineTo(42.875, 327.90625);
    	cr.LineTo(174.71875, 398.0625);
    	cr.LineTo(180.875, 308.6875);
    	cr.ClosePath();
    	cr.MoveTo(407.03125, 301.78125);
    	cr.LineTo(232.78125, 394.53125);
    	cr.LineTo(287.6875, 497.972656);
    	cr.LineTo(325.5, 358.09375);
    	cr.LineTo(467.96875, 384.71875);
    	cr.LineTo(407.03125, 301.777344);
    	cr.ClosePath();
    	cr.MoveTo(106.3125, 382.847656);
    	cr.LineTo(86.875, 427.65625);
    	cr.LineTo(144, 402.875);
    	cr.LineTo(106.3125, 382.84375);
    	cr.ClosePath();
    	cr.MoveTo(106.3125, 382.847656);
    	cr.Tolerance = 0.1;
    	cr.Antialias = Antialias.Default;
    	cr.FillRule = FillRule.Winding;
    	cr.FillPreserve();
    	pattern.Dispose();
    
    	cr.Restore();
    }

}