using temporalsmithing.content.modifier;
using temporalsmithing.util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Server;

namespace temporalsmithing.content;

public class AdditionalModifierEvents {

	private const int EntityHit = 0;
	private const int EntityRightClick = 1;
	private static bool initClient;
	private static bool initServer;

	private AdditionalModifierEvents() { }

	public static void InitClient(ICoreClientAPI api) {
		if (initClient) return;
		initClient = true;

		api.Event.OnEntityDeath += OnEntityDeath;
	}

	public static void InitServer(ICoreServerAPI api) {
		if (initServer) return;
		initServer = true;

		var eventApi = api.Event;
		eventApi.BreakBlock += OnBreakBlock;
		eventApi.OnEntityDeath += OnEntityDeath;
	}

	private static void OnEntityDeath(Entity entity, DamageSource damagesource) {
		var slots = Modifiers.GetModifiableSlots(damagesource?.SourceEntity);

		Modifiers.Instance.PerformOnSlots(slots, x => x.Modifier.OnKillEntityWith(entity, damagesource, x));
	}

	private static void OnBreakBlock(IServerPlayer byplayer, BlockSelection blocksel, ref float dropquantitymultiplier,
									 ref EnumHandling handling) {
		var slots = Modifiers.GetModifiableSlots(byplayer.Entity);

		var pair = new Pair<float, EnumHandling>(dropquantitymultiplier, handling);

		Pair<float, EnumHandling> OnEntityReceiveDamageInternal(ModifierEntry x, Pair<float, EnumHandling> y) =>
			x.Modifier.OnBreakBlockWith(byplayer, blocksel, y.Left, y.Right, x);

		pair = Modifiers.Instance.PerformOnSlots(slots, pair, OnEntityReceiveDamageInternal);
		dropquantitymultiplier = pair.Left;
		handling = pair.Right;
	}

}