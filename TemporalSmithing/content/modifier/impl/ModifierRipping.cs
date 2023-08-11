﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using temporalsmithing.item.modifier;
using temporalsmithing.timer;
using temporalsmithing.util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace temporalsmithing.content.modifier.impl;

public class ModifierRipping : Modifier {

	private const int MaxBloodParticleSpawners = 3;
	private readonly Dictionary<long, int> bloodParticleSpawner = new();
	private readonly Dictionary<long, Timer> timers = new();

	private readonly Dictionary<string, int> entityBloodColors = new Dictionary<string, int>()
	   .Append("drifter-", Color.FromArgb(155, 94, 0).ToArgb())
	   .Append("locust-", Color.FromArgb(41, 29, 10).ToArgb())
	   .Append("bell-", Color.FromArgb(47, 16, 82).ToArgb())
	   .Append("strawdummy", 0);

	public ModifierRipping() {
		OnModificationFinish += (api, modified, modifier) => ApplyOnItem(api, modified, modifier);
		OnModifierRemoved += RemoveFromItem;
	}

	public override string GetKey() {
		return "rune-of-ripping";
	}

	public override string GetIconKey() {
		return "droplet";
	}

	public override Color GetIconColor() {
		return Color.FromArgb(240, 30, 30);
	}

	public override int GetRequiredHitsToApply() {
		return 25;
	}

	public override int GetRequiredHitsToRemove() {
		return 20;
	}

	public override float GetItemRetrievalChanceOnRemoval() {
		return 0.75f;
	}

	public override object[] GetDescriptionArguments(ItemStack stack) {
		var seconds = 0;
		if (stack.Item is RuneItem mi) {
			seconds = mi.GetData<int>("duration") / 1000;
		}

		return new object[] { seconds };
	}

	public override float OnAttackedWith(Entity entity, DamageSource damageSource, float damage,
										 ModifierEntry currentEntry) {
		ProccingTimer timer = null;
		if (timers.ContainsKey(entity.EntityId)) {
			timer = timers[entity.EntityId] as ProccingTimer;
			if (timer is null || !timer.IsAlive()) {
				timers.Remove(entity.EntityId);
				timer = null;
			}
		}

		var bleedSecs = (currentEntry.SourceItem.Item as RuneItem)?.GetData<int>("duration");
		if (bleedSecs is null) return base.OnAttackedWith(entity, damageSource, damage, currentEntry);
		var duration = GetNerfedBloodDuration(TimeSpan.FromMilliseconds(bleedSecs.Value), timer);

		if (timer is null) {
			timer = TimerRegistry.StartProccingTimer(duration, TimeSpan.FromSeconds(1),
				new object[] { entity, damageSource.SourceEntity });
			timer.LifeCondition = x => {
				var damaged = x.Data.AccessSafe<Entity>(0);
				return damaged is not null && damaged.State != EnumEntityState.Despawned;
			};
			timer.OnInvocation += BleedDamage;
			timer.OnFinish += _ => bloodParticleSpawner.Remove(entity.EntityId);
			timers.Add(entity.EntityId, timer);
		} else {
			timer.Extend(duration);
		}

		if (bloodParticleSpawner.ContainsKey(entity.EntityId) &&
			bloodParticleSpawner[entity.EntityId] >= MaxBloodParticleSpawners)
			return base.OnAttackedWith(entity, damageSource, damage, currentEntry);

		TemporalSmithing.Instance.ClientApi.Event.RegisterAsyncParticleSpawner((dt, manager) =>
			SpawnBloodParticles(dt, manager, timer));
		bloodParticleSpawner.TryGetValue(entity.EntityId, out var count);
		count++;
		bloodParticleSpawner[entity.EntityId] = count;

		return base.OnAttackedWith(entity, damageSource, damage, currentEntry);
	}

	private static void BleedDamage(object[] data) {
		var entity = data.AccessSafe<Entity>(0);
		var attacker = data.AccessSafe<Entity>(1);
		if (attacker is null) return;
		var damageSource = new DamageSource {
			Source = EnumDamageSource.Internal,
			Type = EnumDamageType.PiercingAttack,
			SourcePos = attacker.Pos.XYZ,
			KnockbackStrength = 0f,
			SourceEntity = attacker
		};
		if (entity is null) return;
		entity.ReceiveDamage(damageSource, 0.5f);
		entity.SetActivityRunning("invulnerable", 0);
	}

	private static TimeSpan GetNerfedBloodDuration(TimeSpan defaultSpan, Timer timer) {
		if (timer is null || timer.GetPerformedExtensions() < 0) return defaultSpan;
		return TimeSpan.FromMilliseconds(defaultSpan.TotalMilliseconds *
										 Math.Pow(Math.E, -0.2 * timer.GetPerformedExtensions()));
	}

	private bool SpawnBloodParticles(float dt, IAsyncParticleManager manager, DefaultTimer timer) {
		var entity = timer.Data.AccessSafe<Entity>(0);

		if (entity is null) return false;
		var bloodColor = entityBloodColors.GetOrDefault(x => entity.Code.Path.StartsWith(x.Key),
			Color.FromArgb(180, 0, 0).ToArgb()).FirstOrDefault();
		if (bloodColor == 0) return false;
		manager.Spawn(new SimpleParticleProperties {
			MinPos = entity.Pos.XYZ.Clone().Add(0, entity.LocalEyePos.Y / 2, 0),
			MinVelocity = new Vec3f(-1, -1, -1),
			AddVelocity = new Vec3f(2, 2, 2),
			Color = bloodColor,
			MinQuantity = 1,
			AddQuantity = 3,
			LifeLength = 0.2f,
			addLifeLength = 5,
			GravityEffect = 0.8f,
			MinSize = 0.2f,
			MaxSize = 0.5f,
			SizeEvolve = EvolvingNatFloat.create(EnumTransformFunction.LINEARNULLIFY, -0.2f),
			ParticleModel = EnumParticleModel.Cube,
			WithTerrainCollision = true,
			Bounciness = 0
		});

		return timer.IsAlive();
	}

}