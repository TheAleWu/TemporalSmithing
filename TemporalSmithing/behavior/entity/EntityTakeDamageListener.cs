using System.Linq;
using temporalsmithing.content.modifier;
using temporalsmithing.content.modifier.events;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace temporalsmithing.behavior.entity;

public class EntityTakeDamageListener : EntityBehavior {

	public EntityTakeDamageListener(Entity entity) : base(entity) { }

	public override string PropertyName() => "entityhurtlistener";

	public override void OnEntityReceiveDamage(DamageSource damageSource, ref float damage) {
		var runesGrouped = RuneService.Instance.ReadRunesGrouped(entity);
		foreach (var entry in runesGrouped.SelectMany(slot => slot.Value)) {
			var @event = new EntityTakeDamageEvent(entry.Value, entity, damageSource, damage);
			entry.Key.OnEntityTakeDamage(@event);
			damage = @event.Damage;
		}
	}

}
