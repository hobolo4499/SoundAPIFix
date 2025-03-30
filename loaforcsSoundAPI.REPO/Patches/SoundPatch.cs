using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace loaforcsSoundAPI.REPO.Patches;

[HarmonyPatch(typeof(Sound))]
static class SoundPatch {
	[HarmonyTranspiler, HarmonyPatch(nameof(Sound.PlayLoop))]
	static IEnumerable<CodeInstruction> FixLoopingSounds(IEnumerable<CodeInstruction> instructions) {
		return new CodeMatcher(instructions)
			.MatchForward(true,
				new CodeMatch(OpCodes.Ldarg_0),
				new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(Sound), nameof(Sound.Source))),
				new CodeMatch(OpCodes.Ldarg_0),
				new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(Sound), nameof(Sound.LoopClip))),
				new CodeMatch(OpCodes.Callvirt, AccessTools.PropertySetter(typeof(AudioSource), nameof(AudioSource.clip)))
			)
			.Insert(
				new CodeInstruction(OpCodes.Ldarg_0),
				new CodeInstruction(OpCodes.Ldc_I4_1),
				new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(Sound), nameof(Sound.AudioInfoFetched)))
			)
			.InstructionEnumeration();
	}

	[HarmonyPrefix, HarmonyPatch(nameof(Sound.PlayLoop))]
	static void ResetAudioInfoFetched(Sound __instance) {
		if(!__instance.Source.clip) __instance.AudioInfoFetched = false;
	}
}