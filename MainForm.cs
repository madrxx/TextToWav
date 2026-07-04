using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using TextToWav.Models;
using TextToWav.Services;
using TextToWav.Utilities;

namespace TextToWav
{
    public partial class MainForm : Form
    {
        private VoiceCatalog voiceCatalog;
        private SpeechExportService exportService;
        private SpeechPreviewService previewService;

        public MainForm()
        {
            InitializeComponent();

            if (IsInDesignMode())
            {
                return;
            }

            voiceCatalog = new VoiceCatalog();
            exportService = new SpeechExportService();
            previewService = new SpeechPreviewService(exportService);

            Icon = WindowsSoundIcon.GetIcon();

            LoadVoices();

            rateTrackBar.ValueChanged += RateTrackBar_ValueChanged;
            volumeTrackBar.ValueChanged += VolumeTrackBar_ValueChanged;
            pitchTrackBar.ValueChanged += PitchTrackBar_ValueChanged;

            previewButton.Click += PreviewButton_Click;
            stopButton.Click += StopButton_Click;
            saveButton.Click += SaveButton_Click;

            FormClosing += MainForm_FormClosing;
        }

        private bool IsInDesignMode()
        {
            return LicenseManager.UsageMode == LicenseUsageMode.Designtime;
        }

        private void LoadVoices()
        {
            voiceComboBox.Items.Clear();

            try
            {
                List<SpeechVoice> voices = voiceCatalog.GetVoices();

                foreach (SpeechVoice voice in voices)
                {
                    voiceComboBox.Items.Add(voice);
                }

                SelectFirstVoice();
                AdjustVoiceDropdownWidth();

                if (!voices.Any())
                {
                    MessageBox.Show(
                        "No compatible text-to-speech voices were found.",
                        "No voices",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not load voices:\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void SelectFirstVoice()
        {
            if (voiceComboBox.Items.Count > 0)
            {
                voiceComboBox.SelectedIndex = 0;
            }
        }

        private void AdjustVoiceDropdownWidth()
        {
            int width = voiceComboBox.Width;

            foreach (object item in voiceComboBox.Items)
            {
                string text = item.ToString();
                int itemWidth = TextRenderer.MeasureText(text, voiceComboBox.Font).Width + 30;

                if (itemWidth > width)
                {
                    width = itemWidth;
                }
            }

            voiceComboBox.DropDownWidth = width;
        }

        private SpeechVoice GetSelectedVoice()
        {
            return voiceComboBox.SelectedItem as SpeechVoice;
        }

        private SpeechSettings GetSpeechSettings()
        {
            return new SpeechSettings
            {
                Rate = rateTrackBar.Value,
                Volume = volumeTrackBar.Value,
                Pitch = pitchTrackBar.Value
            };
        }

        private void RateTrackBar_ValueChanged(object sender, EventArgs e)
        {
            rateLabel.Text = "Rate: " + rateTrackBar.Value;
        }

        private void VolumeTrackBar_ValueChanged(object sender, EventArgs e)
        {
            volumeLabel.Text = "Volume: " + volumeTrackBar.Value;
        }

        private void PitchTrackBar_ValueChanged(object sender, EventArgs e)
        {
            pitchLabel.Text = "Pitch: " + pitchTrackBar.Value;
        }

        private async void PreviewButton_Click(object sender, EventArgs e)
        {
            string text = inputTextBox.Text.Trim();

            if (!ValidateInput(text, out SpeechVoice selectedVoice))
            {
                return;
            }

            try
            {
                await previewService.PreviewAsync(text, selectedVoice, GetSpeechSettings());
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Failed to preview speech:\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (previewService != null)
                {
                    previewService.Stop();
                }
            }
            catch
            {
                // Ignore stop errors.
            }
        }

        private async void SaveButton_Click(object sender, EventArgs e)
        {
            string text = inputTextBox.Text.Trim();

            if (!ValidateInput(text, out SpeechVoice selectedVoice))
            {
                return;
            }

            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "WAV files (*.wav)|*.wav";
                dialog.Title = "Save speech as WAV";
                dialog.FileName = "speech.wav";

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    await exportService.SaveWavAsync(dialog.FileName, text, selectedVoice, GetSpeechSettings());

                    MessageBox.Show(
                        "WAV file saved successfully.",
                        "Done",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Failed to save WAV file:\n\n" + ex.Message,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
        }

        private bool ValidateInput(string text, out SpeechVoice selectedVoice)
        {
            selectedVoice = GetSelectedVoice();

            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show(
                    "Please enter some text first.",
                    "No text",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                return false;
            }

            if (selectedVoice == null)
            {
                MessageBox.Show(
                    "Please choose a voice first.",
                    "No voice",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                return false;
            }

            return true;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (previewService != null)
            {
                previewService.Dispose();
                previewService = null;
            }
        }
    }
}