﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.Latency;
using Microsoft.Extensions.Diagnostics.Latency.Internal;
using Microsoft.Shared.Diagnostics;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extensions to add console latency data exporter.
/// </summary>
public static class LatencyConsoleExtensions
{
    /// <summary>
    /// Add latency data exporter for the console.
    /// </summary>
    /// <param name="services">Dependency injection container.</param>
    /// <returns>The value of <paramref name="services" />.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="services"/> is <see langword="null"/>.</exception>
    public static IServiceCollection AddConsoleLatencyDataExporter(this IServiceCollection services)
    {
        _ = Throw.IfNull(services);

        _ = services.AddOptions<LatencyConsoleOptions>();
        services.TryAddSingleton<ILatencyDataExporter, LatencyConsoleExporter>();

        return services;
    }

    /// <summary>
    /// Add latency data exporter for the console.
    /// </summary>
    /// <param name="services">Dependency injection container.</param>
    /// <param name="configure"><see cref="LatencyConsoleOptions"/> configuration delegate.</param>
    /// <returns>The value of <paramref name="services" />.</returns>
    /// <exception cref="ArgumentNullException">Either <paramref name="services"/> or <paramref name="configure"/> is <see langword="null"/>.</exception>
    public static IServiceCollection AddConsoleLatencyDataExporter(this IServiceCollection services, Action<LatencyConsoleOptions> configure)
    {
        _ = Throw.IfNull(services);
        _ = Throw.IfNull(configure);

        _ = services.Configure(configure);

        return services.AddConsoleLatencyDataExporter();
    }

    /// <summary>
    /// Add latency data exporter for the console.
    /// </summary>
    /// <param name="services">Dependency injection container.</param>
    /// <param name="section">Configuration of <see cref="LatencyConsoleOptions"/>.</param>
    /// <returns>The value of <paramref name="services" />.</returns>
    /// <exception cref="ArgumentNullException">Either <paramref name="services"/> or <paramref name="section"/> is <see langword="null"/>.</exception>
    public static IServiceCollection AddConsoleLatencyDataExporter(this IServiceCollection services, IConfigurationSection section)
    {
        _ = Throw.IfNull(services);
        _ = Throw.IfNull(section);

        _ = services.Configure<LatencyConsoleOptions>(section);

        return services.AddConsoleLatencyDataExporter();
    }
}
