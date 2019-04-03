﻿using System;

namespace EspionSpotify.Events
{
    /// <summary>
    /// Event gets triggered, when the Playin-state is changed (e.g Play --> Pause)
    /// </summary>
    public class PlayStateEventArgs
    {
        public Boolean Playing { get; set; }
    }
}
