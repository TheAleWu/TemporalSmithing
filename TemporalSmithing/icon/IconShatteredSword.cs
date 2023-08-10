using System;
using Cairo;

namespace temporalsmithing.icon;

public class IconShatteredSword {

	public static void Drawshatteredsword_svg(Context cr, int x, int y, float width, float height, double[] rgba) {
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
		cr.MoveTo(496.222656, 16.816406);
		cr.LineTo(447.746094, 61.023438);
		cr.LineTo(496.222656, 65.597656);
		cr.ClosePath();
		cr.MoveTo(454.671875, 18.039063);
		cr.LineTo(385.761719, 29.316406);
		cr.LineTo(378.722656, 94.863281);
		cr.LineTo(454.671875, 18.035156);
		cr.ClosePath();
		cr.MoveTo(352.261719, 77.90625);
		cr.LineTo(264.371094, 128.207031);
		cr.LineTo(279.820313, 169.234375);
		cr.LineTo(362.480469, 152.363281);
		cr.ClosePath();
		cr.MoveTo(416.601563, 102.929688);
		cr.LineTo(383.632813, 170.371094);
		cr.LineTo(408.476563, 241.054688);
		cr.LineTo(462.789063, 106.867188);
		cr.LineTo(416.597656, 102.929688);
		cr.ClosePath();
		cr.MoveTo(145.238281, 175.738281);
		cr.CurveTo(135.21875, 330.738281, 124.523438, 178.902344, 114.449219, 184.792969);
		cr.CurveTo(118.589844, 193.65625, 117.828125, 203.503906, 111.671875, 209.667969);
		cr.CurveTo(105.515625, 215.832031, 95.679688, 216.59375, 86.824219, 212.445313);
		cr.CurveTo(73.902344, 234.613281, 74.945313, 259.785156, 89.3125, 274.175781);
		cr.LineTo(119.152344, 244.308594);
		cr.CurveTo(177.804688, 287.175781, 228.519531, 337.246094, 269.722656, 395.546875);
		cr.LineTo(233.53125, 431.777344);
		cr.LineTo(240.140625, 425.171875);
		cr.CurveTo(254.425781, 439.472656, 279.332031, 440.59375, 301.371094, 427.90625);
		cr.CurveTo(296.492188, 418.707031, 297.003906, 408.164063, 303.476563, 401.683594);
		cr.CurveTo(309.949219, 395.207031, 320.476563, 394.691406, 329.664063, 399.574219);
		cr.CurveTo(342.324219, 377.511719, 341.203125, 352.574219, 326.929688, 338.273438);
		cr.LineTo(298.683594, 366.554688);
		cr.CurveTo(241.648438, 324.144531, 190.773438, 273.886719, 147.257813, 216.167969);
		cr.LineTo(182.710938, 180.675781);
		cr.LineTo(176.101563, 187.277344);
		cr.CurveTo(168.269531, 179.433594, 157.238281, 175.550781, 145.238281, 175.734375);
		cr.ClosePath();
		cr.MoveTo(253.6875, 180.25);
		cr.CurveTo(236.078125, 197.660156, 214.015625, 219.460938, 191.46875, 241.625);
		cr.CurveTo(200.339844, 251.941406, 209.488281, 261.960938, 218.875, 271.71875);
		cr.LineTo(277.9375, 212.59375);
		cr.LineTo(253.6875, 180.246094);
		cr.ClosePath();
		cr.MoveTo(331.96875, 212.9375);
		cr.LineTo(297, 219.96875);
		cr.LineTo(232, 285.03125);
		cr.CurveTo(243.328125, 296.214844, 254.988281, 307.011719, 267, 317.40625);
		cr.CurveTo(289.019531, 294.273438, 310.765625, 270.996094, 331.96875, 248);
		cr.ClosePath();
		cr.MoveTo(173.46875, 313.25);
		cr.CurveTo(142.324219, 354.109375, 105.59375, 387.574219, 64.566406, 415.125);
		cr.CurveTo(68.695313, 428.832031, 80.960938, 441.5, 95.1875, 445.6875);
		cr.CurveTo(124.65625, 404.429688, 156.300781, 365.785156, 196.160156, 335.71875);
		cr.CurveTo(188.789063, 328.054688, 181.246094, 320.5625, 173.472656, 313.25);
		cr.ClosePath();
		cr.MoveTo(44.5, 410.28125);
		cr.LineTo(23.65625, 431.125);
		cr.CurveTo(30.789063, 455.773438, 51.5, 476.035156, 77.375, 484.5625);
		cr.LineTo(96.65625, 465.25);
		cr.CurveTo(69.707031, 460.96875, 48.34375, 438.339844, 44.78125, 412.3125);
		cr.ClosePath();
		cr.MoveTo(44.5, 410.28125);
		cr.Tolerance = 0.1;
		cr.Antialias = Antialias.Default;
		cr.FillRule = FillRule.Winding;
		cr.FillPreserve();
		pattern.Dispose();

		cr.Restore();
	}

}