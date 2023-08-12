using temporalsmithing.content.modifier;
using temporalsmithing.util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Server;

namespace temporalsmithing.content;

public class EventExtensions {

	private const int EntityHit = 0;
	private const int EntityRightClick = 1;
	private static bool initClient;
	private static bool initServer;

	private EventExtensions() { }

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
		var slots = RunePowers.GetModifiableSlots(damagesource?.SourceEntity);

		RunePowers.Instance.PerformOnSlots(slots, x => x.RunePower.OnKillEntityWith(entity, damagesource, x));
	}

	private static void OnBreakBlock(IServerPlayer byplayer, BlockSelection blocksel, ref float dropquantitymultiplier,
									 ref EnumHandling handling) {
		var slots = RunePowers.GetModifiableSlots(byplayer.Entity);

		var pair = new Pair<float, EnumHandling>(dropquantitymultiplier, handling);

		Pair<float, EnumHandling> OnEntityReceiveDamageInternal(RunePowerEntry x, Pair<float, EnumHandling> y) =>
			x.RunePower.OnBreakBlockWith(byplayer, blocksel, y.Left, y.Right, x);

		pair = RunePowers.Instance.PerformOnSlots(slots, pair, OnEntityReceiveDamageInternal);
		dropquantitymultiplier = pair.Left;
		handling = pair.Right;
	}

}
