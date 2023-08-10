using HarmonyLib;
using temporalsmithing.behavior.collectible;
using temporalsmithing.behavior.entity;
using temporalsmithing.block;
using temporalsmithing.blockentity;
using temporalsmithing.content;
using temporalsmithing.harmony;
using temporalsmithing.icon;
using temporalsmithing.item.modifier;
using temporalsmithing.item.tool;
using temporalsmithing.patches;
using temporalsmithing.timer;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace temporalsmithing;

public class TemporalSmithing : ModSystem {

	public static readonly Harmony Harmony = new("de.alewu.temporalsmithing");
	public static TemporalSmithing Instance;
	public ICoreClientAPI ClientApi { get; private set; }
	public ICoreServerAPI ServerApi { get; private set; }

	public override void Start(ICoreAPI api) {
		Instance = this;

		api.RegisterItemClass("metal-file", typeof(ItemMetalFile));
		api.RegisterItemClass("temporal-infusion", typeof(ItemTemporalInfusion));
		api.RegisterItemClass("rune-of-ripping", typeof(RuneOfRipping));
		api.RegisterItemClass("rune-of-yield", typeof(RuneOfYield));
		api.RegisterItemClass("rune-of-hardening", typeof(RuneOfHardening));

		api.RegisterBlockClass("smithing-table", typeof(BlockSmithingTable));
		api.RegisterBlockEntityClass("smithing-table", typeof(BlockEntitySmithingTable));

		api.RegisterEntityBehaviorClass("playerdamagedentitylistener", typeof(PlayerDamagedEntityListener));
		api.RegisterEntityBehaviorClass("entityhurtlistener", typeof(EntityHurtListener));

		api.RegisterCollectibleBehaviorClass("modifiable", typeof(ModifiableBehavior));

		base.Start(api);
	}

	public override void StartClientSide(ICoreClientAPI api) {
		ClientApi = api;
		CreateCustomIcons(api);
		AdditionalModifierEvents.InitClient(api);

		#region Harmony Patches

		Harmony.Patch(typeof(GuiHandbookTextPage).GetMethod("Init", new[] { typeof(ICoreClientAPI) }),
			postfix: new HarmonyMethod(typeof(HarmonyHandbookReferenceInjection).GetMethod("Patch")));
		Harmony.Patch(
			typeof(CollectibleObject).GetMethod("OnAttackingWith",
				new[] { typeof(IWorldAccessor), typeof(Entity), typeof(Entity), typeof(ItemSlot) }),
			prefix: new HarmonyMethod(typeof(HarmonyCollectibleObjectOnAttackWith).GetMethod("Patch")));
		Harmony.Patch(
			typeof(CollectibleObject).GetMethod("DamageItem",
				new[] { typeof(IWorldAccessor), typeof(Entity), typeof(ItemSlot), typeof(int) }),
			prefix: new HarmonyMethod(typeof(HarmonyCollectibleObjectDamageItem).GetMethod("Patch")));

		#endregion
	}

	public override void StartServerSide(ICoreServerAPI api) {
		AdditionalModifierEvents.InitServer(api);
		ServerApi = api;

		api.Event.RegisterGameTickListener(TimerRegistry.DoTick, 100);
	}

	public override void AssetsLoaded(ICoreAPI api) {
		ApplyPatches(api);
	}

	private static void CreateCustomIcons(ICoreClientAPI api) {
		var customIcons = api.Gui.Icons.CustomIcons;

		#region Modifier Icons

		customIcons.Add("droplet", IconDroplet.Drawdroplet_svg);
		customIcons.Add("quickslash", IconQuickSlash.Drawquickslash_svg);
		customIcons.Add("question", IconQuestion.Drawquestion_svg);
		customIcons.Add("goldbar", IconGoldBar.Drawgoldbar_svg);

		#endregion

		#region System Icons

		customIcons.Add("warning", IconWarning.Drawwarning_svg);
		customIcons.Add("unlock", IconUnlock.Drawunlock_svg);
		customIcons.Add("hammer", IconHammer.Drawhammer_svg);
		customIcons.Add("shamrock", IconShamrock.Drawshamrock_svg);
		customIcons.Add("edgecrack", IconEdgeCrack.Drawedgecrack_svg);
		customIcons.Add("shatteredsword", IconShatteredSword.Drawshatteredsword_svg);
		customIcons.Add("openchest", IconOpenChest.Drawopenchest_svg);

		#endregion
	}

	private static void ApplyPatches(ICoreAPI api) {
		var engine = new PatchEngine(api);

		engine.ApplyPatch(new PatchAddEntityHurtListener(api));
		engine.ApplyPatch(new PatchAddPlayerDamagedEntityListener(api));
		engine.ApplyPatch(new PatchMakeSwordsSmithable());

		#region Logging

		if (engine.Applied > 0)
			api.Logger.Audit($"Successfully applied {engine.Applied} patch{(engine.Applied == 1 ? "" : "es")}");
		if (engine.NotFound > 0)
			api.Logger.Audit($"Did not apply {engine.NotFound} patch{(engine.NotFound == 1 ? "" : "es")} " +
							 "because the file being patched could not be found");
		if (engine.Error > 0)
			api.Logger.Audit($"Could not apply {engine.Error} patch{(engine.Error == 1 ? "" : "es")} " +
							 "as errors occurred");

		#endregion
	}

}
