# MTMT VFX Core

MTMT VFX Core is a From The Depths mod plugin that replaces and extends multiple in-game weapon visual effects through Harmony patches, pooled custom prefabs, and a dedicated options UI tab.

The project is built as a reusable VFX foundation so other MTMT VFX modules can share common loading, pooling, dispatching, and configuration behavior.

## What It Does

- Patches in-game effect emitters (APS, railgun, CRAM, explosions, laser pulse, PAC, trails, and more) and routes rendering to custom logic.
- Uses pooled GameObjects to reduce allocation churn and control maximum active effect counts.
- Adds an in-game settings screen under the options UI for toggles and pool sizing.
- Supports degraded-mode awareness so expensive VFX can be skipped when performance mode is active.
- Loads plugin metadata (`plugin.json`) and checks workshop version information for update notices.

## Current Feature Coverage

- APS muzzle flash override and railgun muzzle effect replacement.
- CRAM muzzle effect replacement.
- Explosion visual replacement with size banding/scaling.
- Laser pulse replacement.
- PAC beam replacement.
- Optional continuous laser object pipeline (scaffolded and configurable).
- APS projectile trail suppression/cloning path.
- CRAM and plasma trail suppression patches.
- Flamer/plasma toggle patch paths exist (with partial behavior and TODO areas).

## Runtime Architecture

### 1) Plugin Bootstrap

- `CorePlugin` is the entrypoint (`GamePlugin_PostLoad`).
- On load it:
	- Runs metadata/version initialization.
	- Applies all Harmony patches via `PatchAll()`.

### 2) Asset and Registry Layer

- `AssetLoader` resolves prefabs from configured asset bundles.
- `AssetRegistry` stores registered assets as `AssetContainer` entries with priority and source tracking.
- `VFXRegistry` is a simpler legacy-style map-based registry still present in the project.

### 3) Pooling Layer

- `VFXPool` maintains reserve/rendered queues and supports:
	- fixed-size behavior based on settings,
	- adaptive growth when dynamic pooling is enabled,
	- return-to-pool behavior through kill components.
- `VFXMappedPool<T>` provides key-to-instance mapping for scenarios needing stable instance pairing.

### 4) Dispatch and Lifecycle

- `MainThreadDispatcher` queues patch-triggered actions and executes them in `FixedUpdate` on Unity's main thread.
- `Loader` patch initializes `VFXManager` after `AutoBattle.Start`.

### 5) Effect Orchestration

- `VFXManager` lazy-initializes pools by effect enum family:
	- `MuzzleFlash`
	- `RailgunName`
	- `Explosion`
	- `BeamName`
	- trail support object(s)
- Provides object creation helpers (`Create`, `InstantiateCopy`) used by patches.

## Settings and Configuration

There are two configuration surfaces:

- `config.json`: plugin-level defaults shipped with releases (DEPRECATED, replaced with `SettingsConfig`)
- `SettingsConfig` profile module: in-game persistent options

### High-Level Toggles

- Debug mode (`DEBUG_MODE`)
- Dynamic pooling (`ADAPTIVE`)
- Ignore degraded mode (`E_IN_DEGRADED`)

### Effect Toggles

- APS muzzle (`E_MUZZLE`)
- APS railgun (`E_RAILGUN`)
- Explosions (`E_EXPL`)
- Continuous laser (`E_CONTINUOUS`)
- Pulse laser (`E_PULSE`)
- PAC (`E_PAC`)
- Plasma (`E_PLASMA`)
- Flamer (`E_FLAMER`)

### Pool Size Controls

- `COUNT_MUZZLE`
- `COUNT_RAILGUN`
- `COUNT_EXPL`
- `COUNT_PULSE`
- `COUNT_PAC`
- `COUNT_PLASMA`
- `COUNT_FLAMER`

The options UI is injected through `UIPatch` into the game's options menu as an `MTMT VFX` tab.

## Project Layout

Top-level folders:

- `Source/` - C# source, solution, and project files.
- `Asset Bundles/` - bundled VFX assets used by the mod.
- `Assets/` - asset-bundle build artifacts/metadata.
- `releases/` - packaged release snapshots.

Important source folders:

- `Source/Core/` - plugin bootstrap, manager, pooling, registries, utilities, enums.
- `Source/Effects/` - Harmony patches for weapon/effect systems.
- `Source/Projectiles/` - projectile trail and projectile-related patching.
- `Source/Internal/` - pooled object behavior and return/fade helper components.
- `Source/UI/` - settings data model, UI screen, and UI patch hook.

## Build Requirements

- .NET target: `netstandard2.1`
- Harmony: `0Harmony.dll`
- From The Depths managed assemblies referenced from your local game install.
- Windows + Visual Studio/MSBuild workflow expected by current project settings.

The project currently uses explicit `HintPath` references in the `.csproj` to local FtD managed DLLs. You will need to adjust those paths for your machine if your Steam library location differs.

## Build and Local Deploy

1. Open `Source/MTMTVFX.sln`.
2. Ensure all referenced FtD DLL paths resolve in `Source/MTMT_VFXCore.csproj`.
3. Build the solution in `Debug`.
4. The post-build step copies outputs to the mod root and mirrors the mod folder into:

`%USERPROFILE%\Documents\From The Depths\Mods\MTMTVFXCore`

The post-build script also excludes development-only directories (`.git`, `releases`, `bin`, `obj`, `Dll`) during sync.

## Packaging and Release

- Release snapshots are stored in `releases/MTMT VFX Core vX.Y.Z-*`.
- `plugin.json` defines public mod metadata (name, version, game version, dependencies, workshop ID, and binary filenames).
- `header.header` and `config.json` are included for mod packaging/runtime behavior.

## Known Issues

- Some code paths are scaffolded/TODO (for example helper methods intended to be patched with custom behavior)
- Plasma/flamer functionality includes partial implementations and may require additional polish
- Pool resizing can cause performance spikes when changed during runtime, as indicated in the UI
- Many effects are 1 frame behind due to the reliance on `MainThreadDispatcher`

## License

See `LICENSE`.
