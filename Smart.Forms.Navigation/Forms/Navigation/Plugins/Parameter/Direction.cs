﻿namespace Smart.Forms.Navigation.Plugins.Parameter
{
    using System;

    /// <summary>
    ///
    /// </summary>
    [Flags]
    public enum Direction
    {
        Import = 0x00000001,
        Export = 0x00000002,
        Both = Import | Export,
    }
}
