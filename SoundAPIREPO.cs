using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace loaforcsSoundAPI.REPO;

// Same GUID as loaforc's original on purpose: when both are installed (sound packs depend on the original),
// BepInEx loads only the newest version of a GUID, so this fixed one replaces it.
[BepInPlugin("me.loaforc.soundapi.repo", "loaforcsSoundAPI.REPO", "1.0.2")]
public class SoundAPIREPO : BaseUnityPlugin {
	internal const string PluginGuid = "me.loaforc.soundapi.repo";

	public static SoundAPIREPO Instance { get; private set; }
	internal new static ManualLogSource Logger { get; private set; }

	private void Awake() {
		Logger = BepInEx.Logging.Logger.CreateLogSource(PluginGuid);
		Instance = this;

		Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);

		Logger.LogInfo($"{PluginGuid} has loaded :3 (SolEclips fix {Info.Metadata.Version})");
	}
}
