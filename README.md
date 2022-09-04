
# Soul Hackers 2 Utils

A small BepInEx mod for Soul Hackers 2.

## Included Patches

- *Run In Background* - stops the game from pausing when the game window loses focus
- *Spawn Companions* - party members follow you while in a dungeon
- *Global Costumes* - applies the selected outfit to party members while in a dungeon
- *Run Key* - run while holding R2 or LeftShift (rebindable via config)

## Installation

## Installing BepInEx

1. [Download][1] the latest BepInEx IL2CPP x64 build and extract it to the game folder.

2. In the `BepInEx` directory, create a `config` directory.

3. In the `config` directory, create a `BepInEx.cfg` file.

4. Add the following to `BepInEx.cfg`, then save the file.

   ```ini
   [Logging]

   UnityLogListening = false
   ```

5. Boot the game once. BepInEx will run its first boot process - let it finish before closing the game (it might take a while).

## Installing the Plugin

1. Put `SoulHackers2Utils.dll` in `BepInEx/plugins`.

2. Boot the game once to generate a config file.

3. Edit `BepInEx/config/SoulHackers2Utils.cfg` to match your preferences.

4. Boot the game again to play with the patches you've enabled.

[1]: https://builds.bepinex.dev/projects/bepinex_be
