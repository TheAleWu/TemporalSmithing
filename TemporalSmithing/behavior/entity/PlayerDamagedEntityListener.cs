using temporalsmithing.content.modifier;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace temporalsmithing.behavior.entity;

public class PlayerDamagedEntityListener : EntityBehavior {

	public PlayerDamagedEntityListener(Entity entity) : base(entity) { }

	public override string PropertyName() => "playerdamagedentitylistener";

	public override void OnEntityReceiveDamage(DamageSource damageSource, ref float damage) {
		if (damageSource.SourceEntity is not EntityPlayer || damageSource.Source is EnumDamageSource.Internal) return;
		var slots = Modifiers.GetModifiableSlots(damageSource.SourceEntity);
		var dmg = damage;

		float OnEntityReceiveDamageInternal(ModifierEntry x, float val) =>
			x.Modifier.OnAttackedWith(entity, damageSource, val, x);

		damage = Modifiers.Instance.PerformOnSlots(slots, dmg, OnEntityReceiveDamageInternal);
	}

}