using System;
using Cairo;

namespace temporalsmithing.icon;

public class IconOpenChest {

	public static void Drawopenchest_svg(Context cr, int x, int y, float width, float height, double[] rgba) {
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
		cr.MoveTo(457.03125, 213.035156);
		cr.LineTo(416.515625, 100.238281);
		cr.CurveTo(425, 77.230469, 433.269531, 68.074219, 437.527344, 64.632813);
		cr.CurveTo(440.6875, 62.070313, 443.449219, 61.097656, 445.710938, 61.730469);
		cr.CurveTo(449.847656, 62.898438, 454.488281, 69.429688, 457.988281, 79.183594);
		cr.CurveTo(469.257813, 110.53125, 468.367188, 166.277344, 457.03125, 213.035156);
		cr.ClosePath();
		cr.MoveTo(132.742188, 195.136719);
		cr.LineTo(445.546875, 229.976563);
		cr.LineTo(401.726563, 107.878906);
		cr.LineTo(145.558594, 79.339844);
		cr.CurveTo(148.152344, 115.441406, 143.644531, 159.253906, 132.742188, 195.136719);
		cr.ClosePath();
		cr.MoveTo(128.980469, 77.5);
		cr.LineTo(83.921875, 72.480469);
		cr.LineTo(120.949219, 175.601563);
		cr.CurveTo(128.722656, 143.542969, 131.574219, 107.246094, 128.980469, 77.5);
		cr.ClosePath();
		cr.MoveTo(101.460938, 27.191406);
		cr.CurveTo(97.667969, 30.289063, 90.691406, 37.855469, 83.210938, 55.757813);
		cr.LineTo(402.230469, 91.300781);
		cr.CurveTo(407.5625, 77.605469, 413.601563, 66.597656, 420.109375, 58.804688);
		cr.LineTo(108.796875, 24.128906);
		cr.CurveTo(106.222656, 23.839844, 103.382813, 25.640625, 101.457031, 27.191406);
		cr.ClosePath();
		cr.MoveTo(382.089844, 310.527344);
		cr.LineTo(382.699219, 479.878906);
		cr.LineTo(449.050781, 426.25);
		cr.LineTo(448.441406, 256.898438);
		cr.ClosePath();
		cr.MoveTo(366.164063, 487.898438);
		cr.LineTo(46.621094, 452.304688);
		cr.LineTo(46, 278.394531);
		cr.LineTo(365.554688, 313.988281);
		cr.ClosePath();
		cr.MoveTo(216.726563, 337.648438);
		cr.CurveTo(220.167969, 335.824219, 222.199219, 332.128906, 221.894531, 328.242188);
		cr.CurveTo(221.644531, 321.875, 216.53125, 316.152344, 210.464844, 315.386719);
		cr.LineTo(210.339844, 315.386719);
		cr.CurveTo(204.199219, 314.691406, 199.433594, 319.3125, 199.691406, 325.722656);
		cr.CurveTo(199.929688, 329.925781, 202.046875, 333.796875, 205.460938, 336.265625);
		cr.LineTo(203.289063, 361.335938);
		cr.LineTo(220.859375, 363.34375);
		cr.ClosePath();
		cr.MoveTo(436.007813, 245.664063);
		cr.LineTo(128.546875, 211.414063);
		cr.LineTo(128.546875, 270.953125);
		cr.LineTo(371.257813, 297.992188);
		cr.ClosePath();
		cr.MoveTo(58.308594, 263.128906);
		cr.LineTo(112.648438, 269.1875);
		cr.LineTo(112.648438, 219.207031);
		cr.ClosePath();
		cr.MoveTo(58.308594, 263.128906);
		cr.Tolerance = 0.1;
		cr.Antialias = Antialias.Default;
		cr.FillRule = FillRule.Winding;
		cr.FillPreserve();
		pattern.Dispose();

		cr.Restore();
	}

}