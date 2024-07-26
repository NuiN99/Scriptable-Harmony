using UnityEngine;

namespace NuiN.NExtensions
{
    public class InjectComponentAttribute : PropertyAttribute
    {
        public readonly SearchOrder searchOrder = SearchOrder.ChildrenFirst;
        public readonly InjectOptions injectOptions;

        public InjectComponentAttribute() { }
        
        public InjectComponentAttribute(SearchOrder searchOrder) => this.searchOrder = searchOrder;
        public InjectComponentAttribute(InjectOptions injectOptions) => this.injectOptions = injectOptions;
        
        public InjectComponentAttribute(InjectOptions injectOptions, SearchOrder searchOrder)
        {
            this.injectOptions = injectOptions;
            this.searchOrder = searchOrder;
        }
    }
    
    public enum SearchOrder
    {
        ChildrenFirst,
        ParentsFirst
    }

    public enum InjectOptions
    {
        None,
        AddComponent
    }
}