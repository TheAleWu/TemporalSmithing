namespace temporalsmithing.item.modifier;

public class RuneOfRipping : RuneItem {

	public override object[] GetHandbookDescriptionArguments() {
		return new object[] { GetData<int>("duration") / 1000 };
	}

}