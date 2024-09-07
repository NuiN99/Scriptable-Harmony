﻿using System;
using System.Linq;
using NuiN.NExtensions.Editor;
using UnityEditor.IMGUI.Controls;

namespace NuiN.NExtensions.Items
{
    internal sealed class ClassDropdownItem : AdvancedDropdownItem, IDropdownItem
    {
        private readonly Type type;

        /// <inheritdoc />
        public ClassDropdownItem(Type type)
            : base(type.Name)
        {
            this.type = type;
            enabled = type.GetConstructors().Any(x => x.GetParameters().Length == 0);
            icon = IconUtility.ScriptIcon;
        }

        /// <inheritdoc />
        ReferenceMode IDropdownItem.Mode => ReferenceMode.Raw;

        /// <inheritdoc />
        object IDropdownItem.GetValue()
        {
            return Activator.CreateInstance(type);
        }
    }
}
