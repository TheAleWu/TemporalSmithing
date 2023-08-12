using System.Linq;
using temporalsmithing.content.modifier;
using temporalsmithing.content.modifier.events;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace temporalsmithing.behavior.entity;

public class PlayerDamagedEntityListener : EntityBehavior {

	public PlayerDamagedEntityListener(Entity entity) : base(entity) { }

	public override string PropertyName() => "playerdamagedentitylistener";

	public override void OnEntityReceiveDamage(DamageSource damageSource, ref float damage) {
		if (damageSource.SourceEntity is not EntityPlayer || damageSource.Source is EnumDamageSource.Internal) return;
		var runesGrouped = RuneService.Instance.ReadRunesGrouped(damageSource.SourceEntity);
		foreach (var entry in runesGrouped.SelectMany(slot => slot.Value)) {
			var @event = new PlayerAttackedEntityEvent(entry.Value, entity, damageSource, damage);
			entry.Key.OnPlayerAttackedEntity(@event);
			damage = @event.Damage;
		}
	}

}
