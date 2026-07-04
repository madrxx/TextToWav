using System;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace TextToWav.Utilities
{
    public static class WinRtStreamHelper
    {
        public static async Task<byte[]> ReadRandomAccessStreamAsync(IRandomAccessStream stream)
        {
            if (stream.Size > int.MaxValue)
            {
                throw new Exception("The generated audio is too large to read into memory.");
            }

            byte[] bytes = new byte[(int)stream.Size];

            using (DataReader reader = new DataReader(stream.GetInputStreamAt(0)))
            {
                await reader.LoadAsync((uint)stream.Size);
                reader.ReadBytes(bytes);
            }

            return bytes;
        }
    }
}
