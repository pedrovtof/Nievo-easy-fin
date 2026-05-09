using System.Reflection;
using System.Runtime.CompilerServices;
using Moq;
using StackExchange.Redis;
using NievoEasyfin.Application.Services.Cache;
using NievoEasyfin.Application.Data.Context.Cache;
using NievoEasyfin.Application.Models;

namespace NievoEasyfin.Tests.Mocks.Helpers;

public static class MockHelper
{
    public static T CreateUninitialized<T>() where T : class
    {
        return (T)RuntimeHelpers.GetUninitializedObject(typeof(T));
    }

    public static AuthDbCacheService CreateMockedCacheService(Mock<IDatabase> dbMock)
    {
        var service = CreateUninitialized<AuthDbCacheService>();

        // The property Conn is in AuthDbCacheContext
        var type = typeof(AuthDbCacheContext);

        // Set the property directly if possible, or its backing field
        var prop = type.GetProperty("Conn", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        if (prop != null && prop.CanWrite)
        {
            prop.SetValue(service, dbMock.Object);
        }
        else
        {
            var field = type.GetField("<Conn>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            field?.SetValue(service, dbMock.Object);
        }

        // Initialize private field 'rnd' in AuthDbCacheService
        var rndField = typeof(AuthDbCacheService).GetField("rnd", BindingFlags.Instance | BindingFlags.NonPublic);
        rndField?.SetValue(service, new Random());

        return service;
    }

    public static SmtpModel CreateMockedSmtpModel()
    {
        // For SmtpModel, we use a simple mock since Moq can mock it if we don't call base.
        // Or we use a subclass.
        return new Mock<SmtpModel>().Object;
    }
}
