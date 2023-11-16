using System.Collections;
using UnityEngine;
using HarmonyLib;
using System;

namespace HearthsNeighbor
{
    /*[HarmonyPatch]*/
    public class CheckCollision
    {
        /*[HarmonyPrefix]
        [HarmonyPatch(typeof(Collider), nameof(Collider.enabled), MethodType.Setter)]
        public static void ColliderChecker_Postfix(Collider __instance, bool value)
        {
            if (__instance.gameObject.name == "DevWyrm")
            {
                HearthsNeighbor.LogMessage($"Collider was changed to {value}. This was called by {Environment.StackTrace}");
            }
        }*/
    }
}