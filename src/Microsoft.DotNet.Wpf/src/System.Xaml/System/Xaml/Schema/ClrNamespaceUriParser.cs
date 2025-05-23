// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.Xaml.MS.Impl;

namespace System.Xaml.Schema
{
    internal static class ClrNamespaceUriParser
    {
        public static string GetUri(string clrNs, string assemblyName)
        {
            return $"{KnownStrings.UriClrNamespace}:{clrNs};{KnownStrings.UriAssembly}={assemblyName}";
        }

        public static bool TryParseUri(string uriInput, out string clrNs, out string assemblyName)
        {
            clrNs = null;
            assemblyName = null;

            // xmlns:foo="clr-namespace:System.Windows;assembly=myassemblyname"
            // xmlns:bar="clr-namespace:MyAppsNs"
            // xmlns:spam="clr-namespace:MyAppsNs;assembly="

            int colonIdx = KS.IndexOf(uriInput, ':');
            if (colonIdx == -1)
            {
                return false;
            }

            ReadOnlySpan<char> keyword = uriInput.AsSpan(0, colonIdx);
            if (!KS.Eq(keyword, KnownStrings.UriClrNamespace))
            {
                return false;
            }

            int clrNsStartIdx = colonIdx + 1;
            int semicolonIdx = KS.IndexOf(uriInput, ';');
            if (semicolonIdx == -1)
            {
                clrNs = uriInput.Substring(clrNsStartIdx);
                assemblyName = null;
                return true;
            }
            else
            {
                int clrnsLength = semicolonIdx - clrNsStartIdx;
                clrNs = uriInput.Substring(clrNsStartIdx, clrnsLength);
            }

            int assemblyKeywordStartIdx = semicolonIdx+1;
            int equalIdx = KS.IndexOf(uriInput, '=');
            if (equalIdx == -1)
            {
                return false;
            }

            keyword = uriInput.AsSpan(assemblyKeywordStartIdx, equalIdx - assemblyKeywordStartIdx);
            if (!KS.Eq(keyword, KnownStrings.UriAssembly))
            {
                return false;
            }

            assemblyName = uriInput.Substring(equalIdx + 1);
            return true;
        }
    }
}
