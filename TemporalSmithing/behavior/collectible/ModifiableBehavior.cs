using System.Collections.Generic;
using System.Text;
using temporalsmithing.content.modifier;
using temporalsmithing.util;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;

namespace temporalsmithing.behavior.collectible;

public class ModifiableBehavior : CollectibleBehavior {

	private JsonObject props;

	public ModifiableBehavior(CollectibleObject collObj) : base(collObj) { }

	public List<string> PossibleModifiers { get; } = new();

	public override void Initialize(JsonObject properties) {
		props = properties;
		var possibleModifiersObj = props["possibleModifiers"].AsArray();
		foreach (var mod in possibleModifiersObj) {
			var str = mod.AsString();
			if (str is null) continue;
			PossibleModifiers.Add(str);
		}

		base.Initialize(properties);
	}

	public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world,
										 bool withDebugInfo) {
		var mods = Modifiers.Instance.ReadAppliedModifiers(inSlot.Itemstack);
		if (mods != null && mods.Count > 0) {
			var appliedModifiersStr = Lang.Get("temporalsmithing:applied-modifiers");
			dsc.AppendLine().AppendLine($"<font color=\"#a47e00\">{appliedModifiersStr}</font>");
			foreach (var entry in mods) {
				var iconColor = "#" + ColorToHex.Transform(entry.Modifier.GetIconColor());
				var icon = entry.Modifier.GetIconKey();
				var modName = Lang.Get(entry.Modifier.GetFullLangKey());

				dsc.AppendLine($"<font color=\"{iconColor}\"><icon name={icon}/></font> {modName}");
			}
		}

		base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
	}

	public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel,
											 EntitySelection entitySel, bool firstEvent,
											 ref EnumHandHandling handHandling, ref EnumHandling handling) {
		var slots = Modifiers.GetModifiableSlots(byEntity);
		var pair = new Pair<EnumHandHandling, EnumHandling>(handHandling, handling);

		Pair<EnumHandHandling, EnumHandling> OnHeldAttackStartInternal(
			ModifierEntry x, Pair<EnumHandHandling, EnumHandling> y) =>
			x.Modifier.OnInteractedWithStart(slot, byEntity, blockSel, entitySel, firstEvent, y.Left, y.Right, x);

		pair = Modifiers.Instance.PerformOnSlots(slots, pair, OnHeldAttackStartInternal);
		handHandling = pair.Left;
		handling = pair.Right;
	}

	public override bool OnHeldInteractStep(float secondsUsed, ItemSlot slot, EntityAgent byEntity,
											BlockSelection blockSel, EntitySelection entitySel,
											ref EnumHandling handling) {
		var slots = Modifiers.GetModifiableSlots(byEntity);
		var pair = new Pair<bool, EnumHandling>(true, handling);

		Pair<bool, EnumHandling> OnHeldAttackStartInternal(
			ModifierEntry x, Pair<bool, EnumHandling> y) =>
			x.Modifier.OnInteractedWithStep(y.Left, secondsUsed, slot, byEntity, blockSel, entitySel, y.Right, x);

		pair = Modifiers.Instance.PerformOnSlots(slots, pair, OnHeldAttackStartInternal);
		handling = pair.Right;
		return pair.Left;
	}

	public override void OnHeldInteractStop(float secondsUsed, ItemSlot slot, EntityAgent byEntity,
											BlockSelection blockSel,
											EntitySelection entitySel, ref EnumHandling handling) {
		var slots = Modifiers.GetModifiableSlots(byEntity);
		var result = handling;

		EnumHandling OnHeldAttackStopInternal(ModifierEntry x, EnumHandling y) =>
			x.Modifier.OnInteractedWithStop(secondsUsed, slot, byEntity, blockSel, entitySel, y, x);

		result = Modifiers.Instance.PerformOnSlots(slots, result, OnHeldAttackStopInternal);
		handling = result;
	}

}