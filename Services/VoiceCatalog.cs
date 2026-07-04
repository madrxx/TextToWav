using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using TextToWav.Models;

using SapiSpeechSynthesizer = System.Speech.Synthesis.SpeechSynthesizer;
using WinRtSpeechSynthesizer = Windows.Media.SpeechSynthesis.SpeechSynthesizer;
using WinRtVoiceInformation = Windows.Media.SpeechSynthesis.VoiceInformation;

namespace TextToWav.Services
{
    public class VoiceCatalog
    {
        public bool WinRtAvailable { get; private set; }

        public List<SpeechVoice> GetVoices()
        {
            List<SpeechVoice> voices = new List<SpeechVoice>();

            AddSapiVoices(voices);
            AddWinRtVoicesIfAvailable(voices);

            return voices;
        }

        private void AddSapiVoices(List<SpeechVoice> voices)
        {
            using (SapiSpeechSynthesizer synth = new SapiSpeechSynthesizer())
            {
                foreach (InstalledVoice voice in synth.GetInstalledVoices())
                {
                    if (voice.Enabled)
                    {
                        voices.Add(new SpeechVoice
                        {
                            DisplayName = "[SAPI] " + voice.VoiceInfo.Name,
                            Id = voice.VoiceInfo.Name,
                            CultureName = voice.VoiceInfo.Culture == null ? "en-US" : voice.VoiceInfo.Culture.Name,
                            Type = SpeechVoiceType.Sapi
                        });
                    }
                }
            }
        }

        private void AddWinRtVoicesIfAvailable(List<SpeechVoice> voices)
        {
            WinRtAvailable = false;

            try
            {
                WinRtVoiceInformation[] winRtVoices = WinRtSpeechSynthesizer.AllVoices.ToArray();

                foreach (WinRtVoiceInformation voice in winRtVoices)
                {
                    voices.Add(new SpeechVoice
                    {
                        DisplayName = "[Windows] " + voice.DisplayName + " (" + voice.Language + ")",
                        Id = voice.Id,
                        CultureName = voice.Language,
                        Type = SpeechVoiceType.WinRt
                    });
                }

                WinRtAvailable = winRtVoices.Length > 0;
            }
            catch
            {
                // Windows / OneCore speech is optional. If unavailable, the app remains SAPI-only.
                WinRtAvailable = false;
            }
        }
    }
}
