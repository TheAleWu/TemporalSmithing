using System.Collections.Generic;
using System.Text;
using temporalsmithing.content.modifier;
using temporalsmithing.content.modifier.events;
using temporalsmithing.util;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;

namespace temporalsmithing.behavior.collectible;

public class RuneApplicableBehavior : CollectibleBehavior {

	private JsonObject props;

	public RuneApplicableBehavior(CollectibleObject collObj) : base(collObj) { }

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
		var mods = RuneService.Instance.ReadRunes(inSlot.Itemstack);
		if (mods != null && mods.Count > 0) {
			var appliedModifiersStr = Lang.Get("temporalsmithing:applied-modifiers");
			dsc.AppendLine().AppendLine($"<font color=\"#a47e00\">{appliedModifiersStr}</font>");
			foreach (var entry in mods) {
				var iconColor = "#" + ColorToHex.Transform(entry.Rune.GetIconColor());
				var icon = entry.Rune.GetIconKey();
				var modName = Lang.Get(entry.Rune.GetFullLangKey());

				dsc.AppendLine($"<font color=\"{iconColor}\"><icon name={icon}/></font> {modName}");
			}
		}

		base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
	}

	public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel,
											 EntitySelection entitySel, bool firstEvent,
											 ref EnumHandHandling handHandling, ref EnumHandling handling) {
		var runesGrouped = RuneService.Instance.ReadRunesGrouped(slot);
		foreach (var entry in runesGrouped) {
			var @event = new InteractionWithStartEvent(entry.Value, slot, byEntity, blockSel, entitySel, firstEvent, handHandling, handling);
			entry.Key.OnInteractedWithStart(@event);
			handHandling = @event.HandHandling;
			handling = @event.Handling;
		}
	}

	public override bool OnHeldInteractStep(float secondsUsed, ItemSlot slot, EntityAgent byEntity,
											BlockSelection blockSel, EntitySelection entitySel,
											ref EnumHandling handling) {
		var runesGrouped = RuneService.Instance.ReadRunesGrouped(slot);
		var cancelled = false;
		foreach (var entry in runesGrouped) {
			var @event = new InteractionWithStepEvent(entry.Value, slot, byEntity, blockSel, entitySel, handling);
			entry.Key.OnInteractedWithStep(@event);
			handling = @event.Handling;
			cancelled |= @event.Cancelled;
		}
		return !cancelled;
	}

	public override void OnHeldInteractStop(float secondsUsed, ItemSlot slot, EntityAgent byEntity,
											BlockSelection blockSel,
											EntitySelection entitySel, ref EnumHandling handling) {
		var runesGrouped = RuneService.Instance.ReadRunesGrouped(slot);
		foreach (var entry in runesGrouped) {
			var @event = new InteractionWithStopEvent(entry.Value, slot, byEntity, blockSel, entitySel, handling);
			entry.Key.OnInteractedWithStop(@event);
			handling = @event.Handling;
		}
	}

}
