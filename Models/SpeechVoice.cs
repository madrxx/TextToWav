namespace TextToWav.Models
{
    public class SpeechVoice
    {
        public string DisplayName { get; set; }
        public string Id { get; set; }
        public string CultureName { get; set; }
        public SpeechVoiceType Type { get; set; }

        public bool IsHeader
        {
            get { return Type == SpeechVoiceType.Header; }
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
