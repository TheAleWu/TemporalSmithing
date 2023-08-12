using System.Linq;
using temporalsmithing.content.modifier;
using temporalsmithing.content.modifier.events;
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

	private static void OnEntityDeath(Entity entity, DamageSource damageSource) {
		if (damageSource.SourceEntity is not EntityPlayer || damageSource.Source is EnumDamageSource.Internal) return;
		var runesGrouped = RuneService.Instance.ReadRunesGrouped(damageSource.SourceEntity);
		foreach (var entry in runesGrouped.SelectMany(slot => slot.Value)) {
			var @event = new PlayerKilledEntityEvent(entry.Value, entity, damageSource);
			entry.Key.OnKillEntityWith(@event);
		}
	}

	private static void OnBreakBlock(IServerPlayer byPlayer, BlockSelection blockSel, ref float dropQuantityMultiplier,
									 ref EnumHandling handling) {
		var runesGrouped = RuneService.Instance.ReadRunesGrouped(byPlayer.Entity);
		foreach (var entry in runesGrouped.SelectMany(slot => slot.Value)) {
			var @event = new BlockBreakEvent(entry.Value, byPlayer, blockSel, dropQuantityMultiplier, handling);
			entry.Key.OnBreakBlockWith(@event);

			dropQuantityMultiplier = @event.DropQuantityMultiplier;
			handling = @event.Handling;
		}
	}

}
