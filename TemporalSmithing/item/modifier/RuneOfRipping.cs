namespace temporalsmithing.item.modifier;

public class RuneOfRipping : ModifierItem {

	public override object[] GetHandbookDescriptionArguments() {
		return new object[] { GetData<int>("duration") / 1000 };
	}

}