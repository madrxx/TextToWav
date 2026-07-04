using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TextToWav.Models;
using TextToWav.Services;
using TextToWav.Utilities;

namespace TextToWav
{
    public class MainForm : Form
    {
        private TextBox inputTextBox;
        private Button previewButton;
        private Button stopButton;
        private Button saveButton;
        private ComboBox voiceComboBox;
        private Label voiceLabel;
        private TrackBar rateTrackBar;
        private Label rateLabel;
        private TrackBar volumeTrackBar;
        private Label volumeLabel;
        private TrackBar pitchTrackBar;
        private Label pitchLabel;

        private readonly VoiceCatalog voiceCatalog;
        private readonly SpeechExportService exportService;
        private readonly SpeechPreviewService previewService;

        public MainForm()
        {
            voiceCatalog = new VoiceCatalog();
            exportService = new SpeechExportService();
            previewService = new SpeechPreviewService(exportService);

            Text = "Text to WAV";
            Width = 600;
            Height = 505;
            StartPosition = FormStartPosition.CenterScreen;
            Icon = WindowsSoundIcon.GetIcon();

            inputTextBox = new TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Left = 15,
                Top = 15,
                Width = 550,
                Height = 200,
                Text = "Type something here, then preview it or save it as a WAV file."
            };

            voiceLabel = new Label
            {
                Left = 15,
                Top = 235,
                Width = 80,
                Text = "Voice:"
            };

            voiceComboBox = new ComboBox
            {
                Left = 100,
                Top = 230,
                Width = 465,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            rateLabel = new Label
            {
                Left = 15,
                Top = 275,
                Width = 120,
                Text = "Rate: 0"
            };

            rateTrackBar = new TrackBar
            {
                Left = 100,
                Top = 265,
                Width = 465,
                Minimum = -10,
                Maximum = 10,
                Value = 0,
                TickFrequency = 1
            };

            volumeLabel = new Label
            {
                Left = 15,
                Top = 320,
                Width = 120,
                Text = "Volume: 100"
            };

            volumeTrackBar = new TrackBar
            {
                Left = 100,
                Top = 310,
                Width = 465,
                Minimum = 0,
                Maximum = 100,
                Value = 100,
                TickFrequency = 10
            };

            pitchLabel = new Label
            {
                Left = 15,
                Top = 365,
                Width = 120,
                Text = "Pitch: 0"
            };

            pitchTrackBar = new TrackBar
            {
                Left = 100,
                Top = 355,
                Width = 465,
                Minimum = -10,
                Maximum = 10,
                Value = 0,
                TickFrequency = 1
            };

            previewButton = new Button
            {
                Left = 15,
                Top = 420,
                Width = 175,
                Height = 32,
                Text = "Preview"
            };

            stopButton = new Button
            {
                Left = 205,
                Top = 420,
                Width = 175,
                Height = 32,
                Text = "Stop"
            };

            saveButton = new Button
            {
                Left = 395,
                Top = 420,
                Width = 170,
                Height = 32,
                Text = "Save as WAV"
            };

            Controls.Add(inputTextBox);
            Controls.Add(voiceLabel);
            Controls.Add(voiceComboBox);
            Controls.Add(rateLabel);
            Controls.Add(rateTrackBar);
            Controls.Add(volumeLabel);
            Controls.Add(volumeTrackBar);
            Controls.Add(pitchLabel);
            Controls.Add(pitchTrackBar);
            Controls.Add(previewButton);
            Controls.Add(stopButton);
            Controls.Add(saveButton);

            LoadVoices();

            rateTrackBar.ValueChanged += (s, e) =>
            {
                rateLabel.Text = "Rate: " + rateTrackBar.Value;
            };

            volumeTrackBar.ValueChanged += (s, e) =>
            {
                volumeLabel.Text = "Volume: " + volumeTrackBar.Value;
            };

            pitchTrackBar.ValueChanged += (s, e) =>
            {
                pitchLabel.Text = "Pitch: " + pitchTrackBar.Value;
            };

            previewButton.Click += PreviewButton_Click;
            stopButton.Click += StopButton_Click;
            saveButton.Click += SaveButton_Click;

            FormClosing += MainForm_FormClosing;
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
                previewService.Stop();
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
            previewService.Dispose();
        }
    }
}
