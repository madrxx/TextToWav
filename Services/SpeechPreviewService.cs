using System;
using System.IO;
using System.Media;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using TextToWav.Models;

using SapiSpeechSynthesizer = System.Speech.Synthesis.SpeechSynthesizer;

namespace TextToWav.Services
{
    public class SpeechPreviewService : IDisposable
    {
        private readonly SpeechExportService exportService;
        private SapiSpeechSynthesizer previewSynth;
        private SoundPlayer previewPlayer;
        private MemoryStream previewSoundStream;
        private int previewVersion;
        private bool disposed;

        public SpeechPreviewService(SpeechExportService exportService)
        {
            this.exportService = exportService;
            previewSynth = new SapiSpeechSynthesizer();
        }

        public async Task PreviewAsync(string text, SpeechVoice voice, SpeechSettings settings)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("SpeechPreviewService");
            }

            StopCurrentPreview();
            int thisPreviewVersion = previewVersion;

            if (voice.Type == SpeechVoiceType.Sapi)
            {
                ResetPreviewSynthesizer();
                previewSynth.SetOutputToDefaultAudioDevice();
                exportService.PrepareSapiSynthesizer(previewSynth, voice, settings);
                previewSynth.SpeakSsmlAsync(exportService.BuildSapiSsml(text, voice, settings));
                return;
            }

            if (voice.Type == SpeechVoiceType.WinRt)
            {
                byte[] wavBytes = await exportService.SynthesizeWinRtToWavBytesAsync(text, voice, settings);

                if (thisPreviewVersion != previewVersion)
                {
                    return;
                }

                previewSoundStream = new MemoryStream(wavBytes);
                previewPlayer = new SoundPlayer(previewSoundStream);
                previewPlayer.Play();
                return;
            }

            throw new Exception("Unsupported voice type.");
        }

        public void Stop()
        {
            StopCurrentPreview();
            ResetPreviewSynthesizer();
        }

        private void ResetPreviewSynthesizer()
        {
            if (previewSynth != null)
            {
                try
                {
                    previewSynth.SpeakAsyncCancelAll();
                }
                catch
                {
                    // Ignore cancellation errors.
                }

                previewSynth.Dispose();
            }

            previewSynth = new SapiSpeechSynthesizer();
        }

        private void StopCurrentPreview()
        {
            previewVersion++;

            try
            {
                if (previewSynth != null)
                {
                    previewSynth.SpeakAsyncCancelAll();
                }
            }
            catch
            {
                // Ignore SAPI stop errors.
            }

            try
            {
                if (previewPlayer != null)
                {
                    previewPlayer.Stop();
                    previewPlayer.Dispose();
                    previewPlayer = null;
                }
            }
            catch
            {
                // Ignore player stop errors.
            }

            try
            {
                if (previewSoundStream != null)
                {
                    previewSoundStream.Dispose();
                    previewSoundStream = null;
                }
            }
            catch
            {
                // Ignore stream disposal errors.
            }
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            StopCurrentPreview();

            if (previewSynth != null)
            {
                try
                {
                    previewSynth.SpeakAsyncCancelAll();
                }
                catch
                {
                    // Ignore shutdown errors.
                }

                previewSynth.Dispose();
                previewSynth = null;
            }

            disposed = true;
        }
    }
}
