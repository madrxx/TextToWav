# TextToWav

A small Windows WinForms application that previews typed text and saves it as a WAV file.

## Features

- Type text into a textbox
- Preview speech using SAPI/OneCore voices
- Adjust speech rate, volume, and pitch
- Save the result as a `.wav` file

## Requirements

- Windows
- .NET Framework 4.7.2 or newer installed
- Visual Studio 2022/2026, or MSBuild, to build from source
- At least one installed text-to-speech voice

## Voice support

This app supports two Windows voice systems:

- Classic SAPI voices through `System.Speech.Synthesis`
- Windows / OneCore voices through `Windows.Media.SpeechSynthesis`, when available

Windows / OneCore support is optional. If the newer speech API is unavailable on a system, the app falls back to SAPI voices only.

Voices are prefixed in the dropdown with `[SAPI]` or `[Windows]` depending on which engine the voice comes from.

Pitch is applied differently depending on the engine:

- SAPI voices use generated SSML with a numerical semitone pitch value, such as `+3st` or `-3st`.
- Windows / OneCore voices use the native `AudioPitch` option.

Some voices may interpret pitch differently or ignore it.

## Project structure

```text
Program.cs                        Application startup
MainForm.cs                       WinForms UI and event handling
Models/SpeechVoice.cs             Voice model used by the UI and services
Models/SpeechVoiceType.cs         SAPI / Windows / header enum
Models/SpeechSettings.cs          Rate, volume, and pitch settings
Services/VoiceCatalog.cs          Voice discovery and optional OneCore fallback
Services/SpeechPreviewService.cs  Preview playback and cancellation
Services/SpeechExportService.cs   WAV export for SAPI and Windows voices
Utilities/WinRtStreamHelper.cs    WinRT stream-to-byte-array helper
Utilities/WindowsSoundIcon.cs     Finds a Windows audio-related icon for the form
```

## Building in Visual Studio

1. Open `TextToWav.slnx` in Visual Studio.
2. Restore NuGet packages if prompted.
3. Select `Release` configuration.
4. Build the project.
5. The executable will be created in `bin\Release`.
