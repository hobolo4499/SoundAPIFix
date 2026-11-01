## SoundAPIFix (loaforcsSoundAPI REPO, SolEclips fork)
R.E.P.O. bindings for [SoundAPI](https://soundapi.loaforc.me), with one fix.

The original [loaforcsSoundAPI.REPO](https://github.com/loaforcsSoundAPI/loaforcsSoundAPI.REPO) by **loaforc** fixes an
oversight in the vanilla game that would cause SoundAPI to seemingly replace sounds with silence. Version 1.0.1 of it
throws a `NullReferenceException` every frame while a Slow Walker is in the level (walking around is enough), which floods the log. This fork fixes that.

### What changed
- `Patches/SoundPatch.cs`: the `PlayLoop` prefix no longer assumes the sound has an `AudioSource`. The game's own
  `PlayLoop` returns early for sounds without clips, and the Slow Walker's attack loop has neither clips nor a source.
- Packaging and build files for the SolEclips tooling.

### Install
Use it together with `loaforc-loaforcsSoundAPI`, like the original. It has the same plugin ID
(`me.loaforc.soundapi.repo`) and a higher version, so when sound packs also install the original 1.0.1, BepInEx loads
only this one.

### License
GPL-3.0, like the original (see `LICENSE`). Original code by loaforc; changes by SolEclips. Source:
https://github.com/hobolo4499/SoundAPIFix
