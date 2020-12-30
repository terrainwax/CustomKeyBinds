﻿using CustomKeyBinds.Tools;
using HarmonyLib;
using UnityEngine;

namespace CustomKeyBinds.Patches
{
    internal static class KeyboardJoystickPatches
    {
        [HarmonyPatch(typeof(KeyboardJoystick), nameof(KeyboardJoystick.Update))]
        public static class PatchMainMenuManagerUpdate
        {
            public static bool Prefix(KeyboardJoystick __instance)
            {
                if (!PlayerControl.LocalPlayer) return false;
                var del = Vector2.zero;
                if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(ConfigManager.keyBinds[KeyAction.Right]))
                    del.x += 1f;
                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(ConfigManager.keyBinds[KeyAction.Left]))
                    del.x -= 1f;
                if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(ConfigManager.keyBinds[KeyAction.Forward]))
                    del.y += 1f;
                if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(ConfigManager.keyBinds[KeyAction.Backward]))
                    del.y -= 1f;
                del.Normalize();
                __instance.del = del;

                HandleHud();
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    if (Minigame.Instance)
                        Minigame.Instance.Close();
                    else if (DestroyableSingleton<HudManager>.InstanceExists && MapBehaviour.Instance &&
                             MapBehaviour.Instance.IsOpen)
                        MapBehaviour.Instance.Close();
                    else if (CustomPlayerMenu.Instance) CustomPlayerMenu.Instance.Close(true);
                }

                return false;
            }

            private static void HandleHud()
            {
                if (!DestroyableSingleton<HudManager>.InstanceExists) return;
                if (Input.GetKeyDown(ConfigManager.keyBinds[KeyAction.Report]))
                    DestroyableSingleton<HudManager>.Instance.ReportButton.DoClick();
                if (Input.GetKeyDown(ConfigManager.keyBinds[KeyAction.Use]))
                    DestroyableSingleton<HudManager>.Instance.UseButton.DoClick();
                if (Input.GetKeyDown(ConfigManager.keyBinds[KeyAction.Map]))
                    DestroyableSingleton<HudManager>.Instance.OpenMap();
                if (Input.GetKeyDown(ConfigManager.keyBinds[KeyAction.Tasks])) Utils.ToggleTab();
                if (PlayerControl.LocalPlayer.Data != null && PlayerControl.LocalPlayer.Data.IsImpostor &&
                    Input.GetKeyDown(ConfigManager.keyBinds[KeyAction.Kill]))
                    DestroyableSingleton<HudManager>.Instance.KillButton.PerformKill();
            }
        }
    }
}