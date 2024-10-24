# StreamKit

StreamKit is a Twitch-integrated mod for RimWorld that enhances gameplay by
connecting Twitch events with in-game actions. The mod offers a plugin-friendly
architecture, and optional web server, and efficient two-way communication
between the game and external services.

# Features

- **Twitch Integration**: Supports Twitch event subscriptions, including
  chat messages, follows, subscriptions, and more.
- **Two-Way Communication**: The mod communicates with an optional web sever for
  remote control and live game data monitoring.
- **Plugin System**: Third-party developers can extend the mod with plugins and
  use the built-in settings api for persistent configuration storage.

# Installation

## Steam Workshop

You can install the mod from the [Steam Workshop](https://docs.sirrandoo.com/streamkit):
    1. Subscribe to the mod on the Steam Workshop page.
    2. Launch RimWorld and enable the mod through the mod manager.

## Manual Installation

1. Download the latest release from the [GitHub Releases](https://github.com/sirrandoo/streamkit/releases).
2. Extract the contents to your RimWorld `Mods` directory: <br/>
   `C:\Program Files (x86)\Steam\steamapps\common\RimWorld\Mods`
3. Enable the mod from the mod manager in-game

# Usage

1. Navigate to the mod's settings menu in RimWorld.
2. Enter your Twitch credentials and configure event triggers.
3. Customize game responses to Twitch chat events.

# Plugin Development

StreamKit supports third-party plugins, which can register with the mod and
store persistent settings through the settings api.

## Example Plugin Code

```csharp
public class ExamplePlugin : IKitPlugin 
{
    public void Initialize(ISettingsStore settings) 
    {
        var config = settings.GetOrCreate("example_plugin", new ExampleSettings());
        config.MaxValue = 100;
    }
}
```

# Architecture Overview

- **Mod Core**: Handles all game-side events and connects to the Twitch API.
- **Web Server** (optional): Offers a browser-based interface for controlling
  the mod and viewing real-time game data.
- **Plugin System**: Extensible with external plugins for custom functionality.
- **Settings API**: Allows plugins to store and retrieve configuration settings.

# License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) 
file for details

# Credits

- **FontAwesome Icons**: This mod uses **FontAwesome SVG icons**, converted to
  **PNG format** to ensure compatibility with Unity's IMGUI system.
  [FontAwesome Website](https://fontawesome.com)

# Contributing

Contributions are welcome. Please follow these steps:

1. Fork the repository.
2. Create a branch for your feature or bug fix.
3. Submit a pull request with a detailed explanation of your changes.
