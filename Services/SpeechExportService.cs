using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using TextToWav.Models;
using TextToWav.Utilities;

using SapiSpeechSynthesizer = System.Speech.Synthesis.SpeechSynthesizer;
using WinRtSpeechSynthesizer = Windows.Media.SpeechSynthesis.SpeechSynthesizer;
using WinRtVoiceInformation = Windows.Media.SpeechSynthesis.VoiceInformation;

namespace TextToWav.Services
{
    public class SpeechExportService
    {
        public async Task SaveWavAsync(string fileName, string text, SpeechVoice voice, SpeechSettings settings)
        {
            if (voice == null || voice.IsHeader)
            {
                throw new Exception("Please choose a voice first.");
            }

            if (voice.Type == SpeechVoiceType.Sapi)
            {
                SaveSapiWav(fileName, text, voice, settings);
                return;
            }

            if (voice.Type == SpeechVoiceType.WinRt)
            {
                byte[] wavBytes = await SynthesizeWinRtToWavBytesAsync(text, voice, settings);
                File.WriteAllBytes(fileName, wavBytes);
                return;
            }

            throw new Exception("Unsupported voice type.");
        }

        public async Task<byte[]> SynthesizeWinRtToWavBytesAsync(string text, SpeechVoice voice, SpeechSettings settings)
        {
            using (WinRtSpeechSynthesizer synth = new WinRtSpeechSynthesizer())
            {
                PrepareWinRtSynthesizer(synth, voice, settings);

                using (Windows.Media.SpeechSynthesis.SpeechSynthesisStream stream =
                    await synth.SynthesizeTextToStreamAsync(text))
                {
                    return await WinRtStreamHelper.ReadRandomAccessStreamAsync(stream);
                }
            }
        }

        private void SaveSapiWav(string fileName, string text, SpeechVoice voice, SpeechSettings settings)
        {
            using (SapiSpeechSynthesizer synth = new SapiSpeechSynthesizer())
            {
                PrepareSapiSynthesizer(synth, voice, settings);
                synth.SetOutputToWaveFile(fileName);
                synth.SpeakSsml(BuildSapiSsml(text, voice, settings));
                synth.SetOutputToNull();
            }
        }

        public void PrepareSapiSynthesizer(SapiSpeechSynthesizer synth, SpeechVoice voice, SpeechSettings settings)
        {
            if (voice == null || voice.Type != SpeechVoiceType.Sapi)
            {
                throw new Exception("Could not prepare the selected SAPI voice.");
            }

            synth.SelectVoice(voice.Id);
            synth.Rate = settings.Rate;
            synth.Volume = settings.Volume;
        }

        public string BuildSapiSsml(string text, SpeechVoice voice, SpeechSettings settings)
        {
            string escapedText = SecurityElement.Escape(text) ?? string.Empty;
            string language = string.IsNullOrWhiteSpace(voice.CultureName) ? "en-US" : voice.CultureName;

            StringBuilder ssml = new StringBuilder();
            ssml.Append("<?xml version=\"1.0\"?>");
            ssml.Append("<speak version=\"1.0\" xmlns=\"http://www.w3.org/2001/10/synthesis\" xml:lang=\"");
            ssml.Append(SecurityElement.Escape(language));
            ssml.Append("\">");

            if (settings.Pitch == 0)
            {
                ssml.Append(escapedText);
            }
            else
            {
                ssml.Append("<prosody pitch=\"");
                ssml.Append(ConvertPitchForSapiSsml(settings.Pitch));
                ssml.Append("\">");
                ssml.Append(escapedText);
                ssml.Append("</prosody>");
            }

            ssml.Append("</speak>");
            return ssml.ToString();
        }

        private string ConvertPitchForSapiSsml(int pitch)
        {
            if (pitch > 10)
            {
                pitch = 10;
            }

            if (pitch < -10)
            {
                pitch = -10;
            }

            if (pitch > 0)
            {
                return "+" + pitch.ToString(CultureInfo.InvariantCulture) + "st";
            }

            return pitch.ToString(CultureInfo.InvariantCulture) + "st";
        }

        public void PrepareWinRtSynthesizer(WinRtSpeechSynthesizer synth, SpeechVoice selectedVoice, SpeechSettings settings)
        {
            if (selectedVoice == null || selectedVoice.Type != SpeechVoiceType.WinRt)
            {
                throw new Exception("Could not prepare the selected Windows / OneCore voice.");
            }

            WinRtVoiceInformation voice = WinRtSpeechSynthesizer.AllVoices
                .FirstOrDefault(v => v.Id == selectedVoice.Id);

            if (voice == null)
            {
                throw new Exception("Could not find the selected Windows / OneCore voice.");
            }

            synth.Voice = voice;
            synth.Options.SpeakingRate = ConvertRateForWinRt(settings.Rate);
            synth.Options.AudioVolume = settings.Volume / 100.0;

            try
            {
                synth.Options.AudioPitch = ConvertPitchForWinRt(settings.Pitch);
            }
            catch
            {
                // Older Windows builds or individual voices may not support pitch adjustment.
            }
        }

        private double ConvertRateForWinRt(int sapiStyleRate)
        {
            if (sapiStyleRate < 0)
            {
                return 1.0 + (sapiStyleRate * 0.05);
            }

            return 1.0 + (sapiStyleRate * 0.10);
        }

        private double ConvertPitchForWinRt(int pitch)
        {
            if (pitch > 10)
            {
                pitch = 10;
            }

            if (pitch < -10)
            {
                pitch = -10;
            }

            return 1.0 + (pitch * 0.10);
        }
    }
}
