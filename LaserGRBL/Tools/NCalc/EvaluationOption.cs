using System;

namespace NCalc
{
    // Summary:
    //     Provides enumerated values to use to set evaluation options.
    [Flags]
    public enum EvaluateOptions
    {
        // Summary:
        //     Specifies that no options are set.
        None = 1,
        //
        // Summary:
        //     Specifies case-insensitive matching.
        IgnoreCase = 2,
        //
        // Summary:
        //     No-cache mode. Ingores any pre-compiled expression in the cache.
        NoCache = 4,
        //
        // Summary:
        //     Treats parameters as arrays and result a set of results.
        IterateParameters = 8,
        //
        // Summary:
        //     When using Round(), if a number is halfway between two others, it is rounded toward the nearest number that is away from zero. 
        RoundAwayFromZero = 16
    }
}
