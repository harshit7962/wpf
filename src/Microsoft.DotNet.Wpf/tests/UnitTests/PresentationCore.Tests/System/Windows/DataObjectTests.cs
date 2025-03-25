// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Windows;

[Collection("Sequential")]
[UISettings(MaxAttempts = 3)]
public class DataObjectTests
{
    [WpfFact]
    public void SetData_MultipleTypes_GetReturnsExptected()
    {
        int intData = 20;
        string stringData = "test string";

        DataObject data = new();

        data.SetData(intData);
        data.SetData(stringData);
        data.GetData(intData.GetType().FullName!).Should().Be(intData);
        data.GetData(stringData.GetType().FullName!).Should().Be(stringData);
    }

    [WpfFact]
    public void SetData_SameTypeTwice_OverwritesData()
    {
        string data1 = "data 1";
        string data2 = "data 2";

        DataObject data = new();

        data.SetData(data1);
        data.SetData(data2);
        data.GetData(data1.GetType().FullName!).Should().Be(data2);
    }

    [WpfFact]
    public void SetData_StringObject_Invoke_GetReturnsExpected()
    {
        string key = "key";
        string testData = "test data";

        DataObject data = new();

        data.SetData(key, testData);
        data.GetData(key).Should().Be(testData);
    }

    [WpfFact]
    public void SetData_StringObject_NullData_ThrowsArgumentNullException()
    {
        string key = "key";
        string? testData = null;

        DataObject data = new();

        Action act = () => data.SetData(key, testData);
        act.Should().Throw<ArgumentNullException>();
    }

    [WpfFact]
    public void SetData_StringObject_NullStringKey_ThrowsArgumentNullException()
    {
        string? key = null;
        string testData = "test data";

        DataObject data = new();

        Action act = () => data.SetData(key, testData);
        act.Should().Throw<ArgumentNullException>();
    }

    [WpfFact]
    public void SetData_StringObject_EmptyStringKey_ThrowsArgumentException()
    {
        string testData = "test data";

        DataObject data = new();
        
        Action act = () => data.SetData(string.Empty, testData);
        act.Should().Throw<ArgumentException>();
    }

    [WpfFact]
    public void SetData_StringObject_SameKeyTwice_OverwritesData()
    {
        string key = "key";
        string data1 = "data1";
        string data2 = "data2";

        DataObject data = new();

        data.SetData(key, data1);
        data.SetData(key, data2);

        data.GetData(key).Should().Be(data2);
    }

    [WpfFact]
    public void SetData_StringObject_DifferentKeys_DataIsStoredSeparately()
    {
        string key1 = "key1";
        string key2 = "key2";
        string data1 = "data1";
        string data2 = "data2";

        DataObject data = new();

        data.SetData(key1, data1);
        data.SetData(key2, data2);
        data.GetData(key1).Should().Be(data1);
        data.GetData(key2).Should().Be(data2);
    }

    [WpfFact]
    public void SetData_TypeObject_Invoke_GetReturnsExpected()
    {
        Type stringKey = typeof(string);
        Type intKey = typeof(int);
        Type boolKey = typeof(bool);
        string stringTestData = "string test data";
        string intTestData = "int test data";
        string boolTestData = "bool test data";

        DataObject data = new();

        data.SetData(stringKey, stringTestData);
        data.SetData(intKey, intTestData);
        data.SetData(boolKey, boolTestData);

        data.GetData(stringKey.FullName!).Should().Be(stringTestData);
        data.GetData(intKey.FullName!).Should().Be(intTestData);
        data.GetData(boolKey.FullName!).Should().Be(boolTestData);
    }

    [WpfFact]
    public void SetData_TypeObject_NullTypeKey_ThrowsArgumentNullException()
    {
        Type? key = null;
        string testData = "test data";

        DataObject data = new();

        Action act = () => data.SetData(key, testData);
        act.Should().Throw<ArgumentNullException>();
    }

    [WpfFact]
    public void SetData_TypeObject_NullData_ThrowsArgumentNullException()
    {
        Type key = typeof(string);

        DataObject data = new();

        Action act = () => data.SetData(key, null);
        act.Should().Throw<ArgumentNullException>();
    }

    [WpfFact]
    public void GetData_NonExistentKey_ReturnsNull()
    {
        Type key = typeof(string);

        DataObject data = new();

        data.GetData(key.FullName!).Should().BeNull();
    }

    [WpfFact]
    public void SetData_TypeObject_SameKeyTwice_OverwritesData()
    {
        Type key = typeof(string);
        string data1 = "data1";
        string data2 = "data2";

        DataObject data = new();

        data.SetData(key, data1);
        data.SetData(key, data2);

        data.GetData(key.FullName!).Should().Be(data2);
    }

    [WpfFact]
    public void SetData_TypeObject_DifferentKeys_DataIsStoredSeparately()
    {
        Type key1 = typeof(string);
        Type key2 = typeof(int);
        string data1 = "data1";
        string data2 = "data2";

        DataObject data = new();

        data.SetData(key1, data1);
        data.SetData(key2, data2);
        data.GetData(key1.FullName!).Should().Be(data1);
        data.GetData(key2.FullName!).Should().Be(data2);
    }
}
