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

	// The vanilla PlayLoop returns before touching Source when the sound has no clips, and some loops (the Slow
	// Walker's standaloneLoopSound) have neither clips nor a Source. This prefix runs before that check, so it
	// must not assume a Source either (it threw a NullReferenceException every frame).
	[HarmonyPrefix, HarmonyPatch(nameof(Sound.PlayLoop))]
	static void ResetAudioInfoFetched(Sound __instance) {
		if(__instance.Source && !__instance.Source.clip) __instance.AudioInfoFetched = false;
	}
}