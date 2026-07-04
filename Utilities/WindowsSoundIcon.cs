using System;
using System.Drawing;
using System.IO;

namespace TextToWav.Utilities
{
    public static class WindowsSoundIcon
    {
        public static Icon GetIcon()
        {
            string systemRoot = Environment.GetFolderPath(Environment.SpecialFolder.Windows);

            string[] candidates =
            {
                Path.Combine(systemRoot, "System32", "SndVol.exe"),
                Path.Combine(systemRoot, "System32", "mmsys.cpl"),
                Path.Combine(systemRoot, "System32", "AudioSrv.dll")
            };

            foreach (string candidate in candidates)
            {
                try
                {
                    if (File.Exists(candidate))
                    {
                        Icon icon = Icon.ExtractAssociatedIcon(candidate);

                        if (icon != null)
                        {
                            return (Icon)icon.Clone();
                        }
                    }
                }
                catch
                {
                    // Try the next candidate.
                }
            }

            return SystemIcons.Information;
        }
    }
}
