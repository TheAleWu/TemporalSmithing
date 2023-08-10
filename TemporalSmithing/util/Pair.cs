namespace temporalsmithing.util;

public class Pair<TX, TY> {

	public TX Left { get; set; }

	public TY Right { get; set; }

	public Pair(TX left, TY right) {
		Left = left;
		Right = right;
	}

}