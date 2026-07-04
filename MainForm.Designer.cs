namespace TextToWav
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TextBox inputTextBox;
        private System.Windows.Forms.Button previewButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.ComboBox voiceComboBox;
        private System.Windows.Forms.Label voiceLabel;
        private System.Windows.Forms.TrackBar rateTrackBar;
        private System.Windows.Forms.Label rateLabel;
        private System.Windows.Forms.TrackBar volumeTrackBar;
        private System.Windows.Forms.Label volumeLabel;
        private System.Windows.Forms.TrackBar pitchTrackBar;
        private System.Windows.Forms.Label pitchLabel;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (previewService != null)
                {
                    previewService.Dispose();
                    previewService = null;
                }

                if (components != null)
                {
                    components.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.inputTextBox = new System.Windows.Forms.TextBox();
            this.previewButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.voiceComboBox = new System.Windows.Forms.ComboBox();
            this.voiceLabel = new System.Windows.Forms.Label();
            this.rateTrackBar = new System.Windows.Forms.TrackBar();
            this.rateLabel = new System.Windows.Forms.Label();
            this.volumeTrackBar = new System.Windows.Forms.TrackBar();
            this.volumeLabel = new System.Windows.Forms.Label();
            this.pitchTrackBar = new System.Windows.Forms.TrackBar();
            this.pitchLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.rateTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.volumeTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pitchTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // inputTextBox
            // 
            this.inputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inputTextBox.Location = new System.Drawing.Point(15, 15);
            this.inputTextBox.Multiline = true;
            this.inputTextBox.Name = "inputTextBox";
            this.inputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.inputTextBox.Size = new System.Drawing.Size(550, 200);
            this.inputTextBox.TabIndex = 0;
            this.inputTextBox.Text = "Type something here, then preview it or save it as a WAV file.";
            // 
            // previewButton
            // 
            this.previewButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.previewButton.Location = new System.Drawing.Point(15, 420);
            this.previewButton.Name = "previewButton";
            this.previewButton.Size = new System.Drawing.Size(175, 32);
            this.previewButton.TabIndex = 9;
            this.previewButton.Text = "Preview";
            this.previewButton.UseVisualStyleBackColor = true;
            // 
            // stopButton
            // 
            this.stopButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.stopButton.Location = new System.Drawing.Point(205, 420);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(175, 32);
            this.stopButton.TabIndex = 10;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            // 
            // saveButton
            // 
            this.saveButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.saveButton.Location = new System.Drawing.Point(395, 420);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(170, 32);
            this.saveButton.TabIndex = 11;
            this.saveButton.Text = "Save as WAV";
            this.saveButton.UseVisualStyleBackColor = true;
            // 
            // voiceComboBox
            // 
            this.voiceComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.voiceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.voiceComboBox.FormattingEnabled = true;
            this.voiceComboBox.Location = new System.Drawing.Point(107, 230);
            this.voiceComboBox.Name = "voiceComboBox";
            this.voiceComboBox.Size = new System.Drawing.Size(465, 21);
            this.voiceComboBox.TabIndex = 2;
            // 
            // voiceLabel
            // 
            this.voiceLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.voiceLabel.Location = new System.Drawing.Point(12, 228);
            this.voiceLabel.Name = "voiceLabel";
            this.voiceLabel.Size = new System.Drawing.Size(80, 23);
            this.voiceLabel.TabIndex = 1;
            this.voiceLabel.Text = "Voice:";
            this.voiceLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rateTrackBar
            // 
            this.rateTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rateTrackBar.Location = new System.Drawing.Point(107, 259);
            this.rateTrackBar.Minimum = -10;
            this.rateTrackBar.Name = "rateTrackBar";
            this.rateTrackBar.Size = new System.Drawing.Size(465, 45);
            this.rateTrackBar.TabIndex = 4;
            // 
            // rateLabel
            // 
            this.rateLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rateLabel.Location = new System.Drawing.Point(12, 259);
            this.rateLabel.Name = "rateLabel";
            this.rateLabel.Size = new System.Drawing.Size(120, 23);
            this.rateLabel.TabIndex = 3;
            this.rateLabel.Text = "Rate: 0";
            this.rateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // volumeTrackBar
            // 
            this.volumeTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.volumeTrackBar.Location = new System.Drawing.Point(107, 310);
            this.volumeTrackBar.Maximum = 100;
            this.volumeTrackBar.Name = "volumeTrackBar";
            this.volumeTrackBar.Size = new System.Drawing.Size(465, 45);
            this.volumeTrackBar.TabIndex = 6;
            this.volumeTrackBar.TickFrequency = 10;
            this.volumeTrackBar.Value = 100;
            // 
            // volumeLabel
            // 
            this.volumeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.volumeLabel.Location = new System.Drawing.Point(12, 310);
            this.volumeLabel.Name = "volumeLabel";
            this.volumeLabel.Size = new System.Drawing.Size(120, 23);
            this.volumeLabel.TabIndex = 5;
            this.volumeLabel.Text = "Volume: 100";
            this.volumeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pitchTrackBar
            // 
            this.pitchTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pitchTrackBar.Location = new System.Drawing.Point(107, 361);
            this.pitchTrackBar.Minimum = -10;
            this.pitchTrackBar.Name = "pitchTrackBar";
            this.pitchTrackBar.Size = new System.Drawing.Size(465, 45);
            this.pitchTrackBar.TabIndex = 8;
            // 
            // pitchLabel
            // 
            this.pitchLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pitchLabel.Location = new System.Drawing.Point(12, 361);
            this.pitchLabel.Name = "pitchLabel";
            this.pitchLabel.Size = new System.Drawing.Size(120, 23);
            this.pitchLabel.TabIndex = 7;
            this.pitchLabel.Text = "Pitch: 0";
            this.pitchLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 466);
            this.Controls.Add(this.rateTrackBar);
            this.Controls.Add(this.volumeTrackBar);
            this.Controls.Add(this.pitchTrackBar);
            this.Controls.Add(this.inputTextBox);
            this.Controls.Add(this.voiceLabel);
            this.Controls.Add(this.voiceComboBox);
            this.Controls.Add(this.rateLabel);
            this.Controls.Add(this.volumeLabel);
            this.Controls.Add(this.pitchLabel);
            this.Controls.Add(this.previewButton);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.saveButton);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Text to WAV";
            ((System.ComponentModel.ISupportInitialize)(this.rateTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.volumeTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pitchTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}