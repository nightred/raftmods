using Harmony;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;


[ModTitle("SharkSpawnDelay")]
[ModDescription("increases the respawn of the shark to 10 min")]
[ModAuthor("nightred")]
[ModIconUrl("https://i.imgur.com/000.png")]
[ModWallpaperUrl("https://i.imgur.com/000.png")]
[ModVersion("0.0.1")]
[RaftVersion("Update 9 (3602784)")]
public class SharkSpawnDelay : Mod
{
    #region Variables
    public static SharkSpawnDelay instance;

    // Harmony
    public HarmonyInstance harmony;
    public readonly string harmonyID = "com.github.nightred.raftmods.sharkspawndelay";

    // Console stuff
    public static string modPrefix = "[SharkSpawn] ";
    #endregion

    #region Start/Update
    public void Start()
    {
        if (instance != null) { throw new Exception("SharkSpawnDelay singleton was already set"); }
        instance = this;

        harmony = HarmonyInstance.Create(harmonyID);
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }
    #endregion

    #region Events
    public void OnModUnload()
    {
        RConsole.Log(modPrefix + "unloaded!");
        harmony.UnpatchAll(harmonyID);
        Destroy(gameObject);
    }
    #endregion
}

#region Harmony Patches
[HarmonyPatch(typeof(Network_Host_Entities))]
[HarmonyPatch("CreateShark")]
[HarmonyPatch(new Type[] { typeof(float), typeof(Vector3), typeof(uint), typeof(uint), typeof(uint) })]
static class SharkRespawnPatch
{
    static void Prefix(ref float timeDelay)
    {
        timeDelay = 10 * 60;
    }
}
#endregion