﻿using System;
using UnityEngine;

namespace NuiN.NExtensions.Editor
{
    public struct GUIEnabledScope : IDisposable
    {
        public readonly bool PreviouslyEnabled;

        public GUIEnabledScope(bool enabled)
        {
            PreviouslyEnabled = GUI.enabled;
            GUI.enabled = enabled;
        }

        public void Dispose()
        {
            GUI.enabled = PreviouslyEnabled;
        }
    }
}