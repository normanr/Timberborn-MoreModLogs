using System;
using UnityEngine;
using HarmonyLib;
using Timberborn.BaseComponentSystem;
using Timberborn.SelectionSystem;

namespace Mods.MoreModLogs {

    [HarmonyPatch(typeof(SelectableObjectRetriever))]
    [HarmonyPatch(nameof(SelectableObjectRetriever.GetSelectableObject))]
    [HarmonyPatch([typeof(BaseComponent)])]
    static class SelectableObjectPatch {

        static void Finalizer(BaseComponent target, Exception __exception) {
            if (__exception == null) return;
            var obj = target.GameObject;
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
