# MIDI Monkey

<div align="center">
  <img src="https://github.com/Psystec/MIDI-Monkey/blob/master/Images/Monkey.png" alt="MIDI Monkey Logo" width="200"/>
  <h3>Play Any MIDI File or Device in Any Game or Application</h3>
  <p>A lightweight, customizable MIDI playback tool for gamers and musicians</p>
</div>

![GitHub Release](https://img.shields.io/github/v/release/Psystec/MIDI-Monkey?include_prereleases&style=for-the-badge)
![.NET Version](https://img.shields.io/badge/.NET-8.0-5C2D91?style=for-the-badge&logo=.net)
[![Discord](https://img.shields.io/badge/Discord-Join%20Us-5865F2?style=for-the-badge&logo=discord&logoColor=white)](https://discord.gg/s3vqJRCGht)

> **Note:** This application was previously known as "Once Human MIDI Maestro" but has been expanded to work with any game or application using custom MIDI keymaps.
> 
## 🎹 Overview

MIDI Monkey is a powerful, user-friendly application that allows you to play MIDI files or a MIDI instrument in any game or application by translating MIDI notes into keyboard keypresses. Whether you want to play piano in your favorite MMORPG, create music in a sandbox game, or automate key sequences in any application, MIDI Monkey makes it simple and intuitive.

## ✨ Key Features

- **Universal Compatibility**: Works with any game or application through customizable key mappings
- **Live MIDI Input**: Connect your MIDI keyboard or device for real-time playing
- **MIDI File Playback**: Load and play any standard MIDI file
- **Custom Key Mappings**: Create and manage profiles for different games and applications
- **Visual Note Display**: See which notes are being played in real-time
- **Playback Controls**: Adjust tempo, add modifier delays, and loop songs
- **Global Hotkeys**: Control playback with F5/F6 without leaving your game
- **Minimal Resource Usage**: Lightweight design with low CPU and memory footprint

## 📷 Screenshots

<div align="center">
  <img src="https://github.com/Psystec/MIDI-Monkey/blob/master/Images/MIDIMonkey_Example.png" alt="MIDI Monkey Main Interface" width="700"/>
</div>

## 🚀 Getting Started

### Requirements

- Windows OS
- .NET 8.0 Runtime ([Download](https://dotnet.microsoft.com/en-us/download/dotnet/8.0))
- Any game or application that accepts keyboard input

### Installation

1. Download the latest release from the [Releases page](https://github.com/YourUsername/MIDI-Monkey/releases)
2. Launch MIDI Monkey.

## 📝 How to Use

### Basic Setup

1. **Select a MIDI KeyMap Profile**:
   - Choose from the built-in profiles or create your own
   - Each profile maps MIDI notes to specific keyboard keys

2. **Load MIDI Files**:
   - Click "Select MIDI Folder" to load your MIDI files
   - Select a MIDI file from the list

3. **Start Playback**:
   - Make sure your game/application is running
   - Click "Play Song" or press F5 to start playback
   - Press F6 to stop playback at any time

### Using a MIDI Keyboard

1. Connect your MIDI keyboard or device to your computer
2. Select it from the "MIDI Device" dropdown
3. Click "Use MIDI Device"
4. Play notes on your MIDI keyboard - they will be automatically translated to keypresses

### Creating Custom KeyMaps

1. Navigate to the 'MIDIKeyMaps' folder in your installation directory
2. Create a new JSON file with the following structure:

```json
{
  "ApplicationName": "YOUR_GAME_PROCESS_NAME",
  "KeyMappings": {
    "60": [ "", "VK_Q" ],
    "61": [ "", "VK_2" ],
    "62": [ "", "VK_W" ],
    "63": [ "", "VK_3" ],
    "64": [ "", "VK_E" ]
  }
}
```

3. The numbers represent MIDI note values (60 = middle C)
4. The strings represent keyboard keys to press
5. You can add modifiers by including them as the first key:

```json
"48": [ "LCONTROL", "VK_Q" ]
```

### Advanced Settings

- **Tempo**: Adjust playback speed (faster or slower)
- **Modifier Delay**: Add a delay between pressing modifier keys and the main key
- **Repeat Song**: Enable to loop the song continuously
- **Always on Top**: Keep MIDI Monkey visible over your game

## 📋 Supported Games & Applications

MIDI Monkey works with any game or application that accepts keyboard input, including:

- Once Human
- Final Fantasy XIV
- Rust
- Starfield
- World of Warcraft
- Black Desert Online
- Minecraft
- Any application that can be controlled with keyboard shortcuts

## 🔧 Troubleshooting

**Game not recognizing key presses?**
- Make sure you've selected the correct KeyMap profile
- Verify the process name in your KeyMap matches your game exactly
- Try running MIDI Monkey as administrator

**MIDI device not working?**
- Ensure your MIDI device is properly connected and recognized by Windows
- Check if any other application is using the MIDI device
- Restart MIDI Monkey after connecting your device

## 🤝 Contributing

## 💖 Support the Project

If you find MIDI Monkey useful, consider supporting its development:

- [Donate via PayPal](https://paypal.me/PsystecZA)
- Star the repository
- Share with friends and fellow gamers

## 📜 License

**This repository intentionally omits a supplementary license.** Exclusive copyright is held by the contributors.
The source code hosted in this repository may not be copied, distributed, or modified without risk of take-downs, shake-downs, or litigation.
For more information regarding the conditions of use where repositories omit a supplemental license; see [GitHub Terms of Service](https://docs.github.com/en/github/site-policy/github-terms-of-service#d-user-generated-content), or [the summary of 'No License' conditions](https://choosealicense.com/no-permission/).
(Sorry for this, need to stop the people selling my code.)

---

<div align="center">
  <p>Made with ❤️ by Psystec</p>
  <p>Copyright © 2025</p>
</div>
