using System;
using UnityEngine;
using HarmonyLib;
using Timberborn.SelectionSystem;

namespace Mods.MoreModLogs {

    [HarmonyPatch(typeof(SelectableObjectRetriever))]
    [HarmonyPatch(nameof(SelectableObjectRetriever.GetSelectableObject))]
    [HarmonyPatch([typeof(GameObject)])]
    static class SelectableObjectPatch {

        static void Finalizer(GameObject gameObject, Exception __exception) {
            if (__exception == null) return;
            var obj = gameObject;
            var path = obj.name;
            while (obj?.transform?.parent?.gameObject != null)
            {
                obj = obj?.transform?.parent?.gameObject;
                path = obj.name + "." + path;
            }
            Debug.LogError(DateTime.Now.ToString("HH:mm:ss ") + "SelectableObject component not found on object with path " + path);
        }
    }
}
