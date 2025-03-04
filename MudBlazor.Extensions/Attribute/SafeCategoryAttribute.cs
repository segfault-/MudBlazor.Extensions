﻿namespace MudBlazor.Extensions.Attribute;

/// <summary>
/// Don't know why i need this but however why MudBlazor think its a good idea to throw an exception if the category is not valid
/// This is something I don't understand on a string name based param. So I created this attribute to prevent this exception.
/// </summary>
public class SafeCategoryAttribute : CategoryAttribute
{
    /// <summary>
    /// Constructor
    /// </summary>
    public SafeCategoryAttribute(string name) : base(GetValidCategory(name)) { }

    private static string GetValidCategory(string name)
    {
        try
        {
            // Try to create a CategoryAttribute with the provided name.
            // If it doesn't throw an exception, the name is valid.
            var categoryAttribute = new CategoryAttribute(name);

            // If we've made it this far without an exception, return the name as-is.
            return name;
        }
        catch (ArgumentException)
        {
            // If an ArgumentException was thrown, the name is not valid.
            // Return "Misc" as a safe default.
            return "Misc";
        }
    }
}