namespace NuiN.NExtensions.Items
{
    internal interface IDropdownItem
    {
        internal ReferenceMode Mode { get; }
        object GetValue();
    }
}
