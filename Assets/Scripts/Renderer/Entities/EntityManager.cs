﻿using ROIO;
using ROIO.Models.FileTypes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EntityManager : MonoBehaviour {

    private Dictionary<uint, Entity> entityCache = new Dictionary<uint, Entity>();

    public Entity Spawn(EntitySpawnData data) {
        switch (data.objecttype) {
            case EntityType.PC:
                entityCache.TryGetValue(data.GID, out var pc);
                pc?.gameObject.SetActive(true);
                return pc ?? SpawnPC(data);
            case EntityType.NPC:
                entityCache.TryGetValue(data.AID, out var npc);
                npc?.gameObject.SetActive(true);
                return npc ?? SpawnNPC(data);
            case EntityType.MOB:
                entityCache.TryGetValue(data.AID, out var mob);
                mob?.gameObject.SetActive(true);
                return mob ?? SpawnMOB(data);
            default:
                return null;
        }
    }

    public void RemoveEntity(uint GID) {
        entityCache.TryGetValue(GID, out Entity entity);
        if (entity != null) {
            Destroy(entity.gameObject);
            entityCache.Remove(GID);
        }
    }

    public Entity GetEntity(uint GID) {
        var hasFound = entityCache.TryGetValue(GID, out var entity);
        if (hasFound) {
            return entity;
        } else if (Session.CurrentSession.Entity.GetEntityGID() == GID || Session.CurrentSession.AccountID == GID) {
            return Session.CurrentSession.Entity as Entity;
        } else {
            Debug.LogError($"No Entity found for given ID: {GID}");
            return null;
        }
    }

    //TODO this needs checking
    public void VanishEntity(uint GID, int type) {
        GetEntity(GID)?.Vanish(type);
    }

    public Entity SpawnItem(ItemSpawnInfo itemSpawnInfo) {

        Item item = DBManager.GetItemInfo(itemSpawnInfo.AID);
        string itemPath = DBManager.GetItemPath(itemSpawnInfo.AID, itemSpawnInfo.IsIdentified);

        ACT act = FileManager.Load(itemPath + ".act") as ACT;
        SPR spr = FileManager.Load(itemPath + ".spr") as SPR;

        var itemGO = new GameObject(item.identifiedDisplayName);
        itemGO.layer = LayerMask.NameToLayer("Items");
        itemGO.transform.localScale = Vector3.one;
        itemGO.transform.localPosition = itemSpawnInfo.Position;
        var entity = itemGO.AddComponent<Entity>();

        var body = new GameObject("Body");
        body.layer = LayerMask.NameToLayer("Items");
        body.transform.SetParent(itemGO.transform, false);
        body.transform.localPosition = new Vector3(0.5f, 0.4f, 0.5f);
        body.AddComponent<Billboard>();
        body.AddComponent<SortingGroup>();

        if (itemSpawnInfo.animate) {
            var animator = body.AddComponent<Animator>();
            animator.runtimeAnimatorController = Instantiate(Resources.Load("Animations/ItemDropAnimator")) as RuntimeAnimatorController;
        }

        var bodyViewer = body.AddComponent<EntityViewer>();

        entity.EntityViewer = bodyViewer;
        entity.Type = EntityType.ITEM;
        entity.ShadowSize = 0.5f;

        bodyViewer.ViewerType = ViewerType.BODY;
        bodyViewer.Entity = entity;
        bodyViewer.SpriteOffset = 0.5f;
        bodyViewer.HeadDirection = 0;
        bodyViewer.CurrentMotion = new EntityViewer.MotionRequest { Motion = SpriteMotion.Idle };

        entity.Init(spr, act);
        entity.AID = (uint)itemSpawnInfo.mapID;
        entityCache.Add(entity.AID, entity);
        entity.SetReady(true);

        return entity;
    }

    private Entity SpawnPC(EntitySpawnData data) {
        var player = new GameObject(data.name);
        player.layer = LayerMask.NameToLayer("Characters");
        player.transform.localScale = Vector3.one;

        var entity = player.AddComponent<Entity>();
        entity.Init(data, LayerMask.NameToLayer("Characters"));

        entityCache.Add(data.AID, entity);
        entity.SetReady(true);

        return entity;
    }

    private Entity SpawnNPC(EntitySpawnData data) {
        var npc = new GameObject(data.name);
        npc.layer = LayerMask.NameToLayer("NPC");
        npc.transform.localScale = Vector3.one;
        var entity = npc.AddComponent<Entity>();
        entity.Init(data, LayerMask.NameToLayer("NPC"));

        entityCache.Add(data.AID, entity);
        entity.SetReady(true);

        return entity;
    }

    private Entity SpawnMOB(EntitySpawnData data) {
        var mob = new GameObject(data.name);
        mob.layer = LayerMask.NameToLayer("Monsters");
        mob.transform.localScale = Vector3.one;
        var entity = mob.AddComponent<Entity>();
        entity.Init(data, LayerMask.NameToLayer("Monsters"));

        entityCache.Add(data.AID, entity);
        entity.SetReady(true);

        return entity;
    }

    public Entity SpawnPlayer(CharacterData data) {
        var player = new GameObject(data.Name);
        player.layer = LayerMask.NameToLayer("Characters");
        player.transform.localScale = Vector3.one;
        var entity = player.AddComponent<Entity>();
        entity.Init(data, LayerMask.NameToLayer("Characters"));

        var controller = player.AddComponent<EntityControl>();
        controller.Entity = entity;

        return entity;
    }
}
