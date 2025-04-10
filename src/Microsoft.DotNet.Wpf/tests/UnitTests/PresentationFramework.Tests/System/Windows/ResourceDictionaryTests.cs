// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Windows.Media;

namespace System.Windows;
public class ResourceDictionaryTests
{
    private readonly ResourceDictionary _rd;

    private string SampleDictionaryPath => "/PresentationFramework.Tests;component/System/Windows/ResourceDictionary.xaml";

    private readonly Application _sampleApplication;

    public ResourceDictionaryTests()
    {
        _sampleApplication = new();

        _rd = new ResourceDictionary
        {
            Source = new Uri(SampleDictionaryPath, UriKind.Relative)
        };

        _sampleApplication.Resources.MergedDictionaries.Add(_rd);
    }

    [Fact]
    public void ResourceDictionary_ShouldContainRedColor()
    {
        Color expectedColor = Color.FromArgb(255, 255, 0, 0);
        Color redColor = (Color)_rd["RedColor"];

        redColor.Should().Be(expectedColor);
    }
}
