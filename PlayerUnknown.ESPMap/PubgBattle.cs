namespace PlayerUnknown.ESPMap
{
    using System;
    using System.Drawing;

    public class PubgBattle
    {
        /// <summary>
        /// Gets or sets the battle start time.
        /// </summary>
        public DateTime StartTime
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the battle end time.
        /// </summary>
        public DateTime EndTime
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the elapsed time.
        /// </summary>
        public TimeSpan ElapsedTime
        {
            get
            {
                if (this.Ended)
                {
                    return this.EndTime.Subtract(this.StartTime);
                }

                return DateTime.UtcNow.Subtract(this.StartTime);
            }
        }

        /// <summary>
        /// Gets or sets the name of the map.
        /// </summary>
        public string MapName
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="PubgBattle"/> has started.
        /// </summary>
        public bool Started
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="PubgBattle"/> has ended.
        /// </summary>
        public bool Ended
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the playing map.
        /// </summary>
        public Bitmap PlayingMap
        {
            get
            {
                return PubgMap.GetMap(this.MapName);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PubgBattle"/> class.
        /// </summary>
        public PubgBattle()
        {
            // PubgBattle.
        }

        /// <summary>
        /// Begins the game.
        /// </summary>
        /// <param name="MapName">Name of the map.</param>
        public void BeginGame(string MapName)
        {
            Logging.Warning(this.GetType(), "Starting a game on " + MapName + ".");

            this.MapName   = MapName;
            this.Started   = true;
            this.Ended     = false;
            this.StartTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Stops the game.
        /// </summary>
        public void StopGame()
        {
            Logging.Warning(this.GetType(), "Ending a game on " + MapName + ".");

            this.Started   = false;
            this.Ended     = true;
            this.EndTime   = DateTime.UtcNow;
        }
    }
}
