# VRising PVP plugin

* Based on [BepInEx](https://github.com/BepInEx/BepInEx)
* Uses [Wetstone](https://github.com/molenzwiebel/Wetstone) for things like hot reloading and hooks (chat commands delegate).

## Current functionalities

### Commands

- `.tp` - teleports you to the arena location
- `.hp` - refills your hp bar
- `.blood <type> <quality>` - sets your blood to type <type> and quality <quality>
  - allowed types are `scholar`, `warrior`, `worker`, `creature`, `brute`
  - quality ranges from 1 to 100
- `.weapons` - adds a kit of t8 weapons

### Respawn in same position

When you die, you respawn in the same exact position, so duelling has no wait time.
