using System;

namespace Molodoy.Inspector.Extentions
{
    /// <summary>
    /// Attribute from method
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class InspectorButtonAttribute : Attribute
    {
        /// <summary>
        /// Button text
        /// </summary>
        public string name;

        /// <summary>
        /// Add Button to Inspector
        /// </summary>
        /// <param name="name">Button text</param>
        public InspectorButtonAttribute(string name)
        {
            this.name = name;
        }
    }
}