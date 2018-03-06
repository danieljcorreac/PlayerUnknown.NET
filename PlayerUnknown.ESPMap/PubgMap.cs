namespace PlayerUnknown.ESPMap
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;

    public static class PubgMap
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="PubgMap"/> is initialized.
        /// </summary>
        public static bool Initialized
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the maps image.
        /// </summary>
        public static Dictionary<string, Bitmap> Maps
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public static void Initialize()
        {
            if (PubgMap.Initialized)
            {
                return;
            }

            PubgMap.Initialized = true;
            PubgMap.Maps        = new Dictionary<string, Bitmap>();

            PubgMap.GetImages();
        }

        /// <summary>
        /// Configures this instance.
        /// </summary>
        private static void GetImages()
        {
            var Images = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\Resources\\Maps\\", "*.bmp");

            foreach (var ImagePath in Images)
            {
                var MapName = Path.GetFileNameWithoutExtension(ImagePath);

                if (PubgMap.Maps.ContainsKey(MapName) == false)
                {
                    PubgMap.Maps.Add(MapName, new Bitmap(ImagePath));
                }
            }
        }

        /// <summary>
        /// Gets the map.
        /// </summary>
        /// <param name="MapName">Name of the map.</param>
        public static Bitmap GetMap(string MapName)
        {
            if (PubgMap.Maps.ContainsKey(MapName))
            {
                return PubgMap.Maps[MapName];
            }

            return null;
        }
    }
}
