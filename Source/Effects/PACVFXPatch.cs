using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace MTMTVFX.Effects
{
    [HarmonyPatch(typeof(ParticleCannonEffect), "RenderAndRun")]
    public class PACVFXPatch
    {
        private static bool Prefix(ParticleCannonEffect __instance)
        {
            if (!Core.Util.E_PAC) return true;

            Color col = new Color(1f, 0.5f, 0.5f, 2f) * __instance.m_BaseColor;
            Gradient gradient = new Gradient();
            GradientColorKey[] colorKeys = { new GradientColorKey(col, 0f) };

            GameObject[] children = __instance.gameObject.GetChildren();
            ParticleSystem[] psList = __instance.gameObject.GetComponentsInChildren<ParticleSystem>();

            foreach (ParticleSystem ps in psList)
            {
                if (ps.name == "shockwave2") continue;

                ParticleSystem.ColorOverLifetimeModule psColor = ps.colorOverLifetime;
                GradientAlphaKey[] alphaKeys = psColor.color.gradient.alphaKeys;
                gradient.SetKeys(colorKeys, alphaKeys);

                psColor.color = gradient;
            }

            foreach (GameObject child in children)
            {
                if (child.name == "shockwave2")
                {
                    child.SetActive(false);
                }

                if (child.name == "SecondaryEffect")
                {
                    child.SetActive(false);
                }

                if (child.name == "Light")
                {
                    Light light = child.GetComponent<Light>();
                    Color lc = __instance.m_BaseColor;
                    lc.a = light.color.a;
                    light.color = lc;
                }
            }
            return false;
        }
    }

    //[HarmonyPatch(typeof(ParticleCannon), "MainThreadClientAndServerFire")]
    public class PACVFXPatch2
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            if (!Core.Util.E_PAC)
            {
                foreach (var instruction in instructions)
                    yield return instruction;

                yield break;
            }

            foreach (var ins in instructions)
            {
                // Just before storing effectScale, the value is on the stack
                if (ins.opcode == OpCodes.Stfld &&
                    ins.operand is System.Reflection.FieldInfo fi &&
                    fi.Name == "effectScale")
                {
                    // Replace computed value with 0f
                    yield return new CodeInstruction(OpCodes.Pop);        // pop computed value
                    yield return new CodeInstruction(OpCodes.Ldc_R4, 0f); // push 0
                }

                yield return ins;
            }


        }
    }
}
