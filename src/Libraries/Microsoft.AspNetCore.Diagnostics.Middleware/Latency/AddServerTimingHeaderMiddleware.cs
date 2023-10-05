﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.Latency;
using Microsoft.Extensions.Primitives;
using Microsoft.Shared.Diagnostics;

namespace Microsoft.AspNetCore.Diagnostics.Latency;

/// <summary>
/// A middleware that populates Server-Timing header with response processing time.
/// </summary>
internal sealed class AddServerTimingHeaderMiddleware
{
    internal const string ServerTimingHeaderName = "Server-Timing";
    private readonly RequestDelegate _next;

    public AddServerTimingHeaderMiddleware(RequestDelegate next)
    {
        _next = Throw.IfNull(next);
    }

    /// <summary>
    /// Request handling method.
    /// </summary>
    /// <param name="context">The <see cref="HttpContext"/> for the current request.</param>
    /// <returns>A <see cref="Task"/> that represents the execution of this middleware.</returns>
    public Task InvokeAsync(HttpContext context)
    {
        context.Response.OnStarting(static ctx =>
        {
            var httpContext = (HttpContext)ctx;
            var latencyContext = httpContext.RequestServices.GetRequiredService<ILatencyContext>();

            if (latencyContext.TryGetCheckpoint(RequestCheckpointConstants.ElapsedTillHeaders, out var timestamp, out var timestampFrequency))
            {
                var elapsedMs = (long)(((double)timestamp / timestampFrequency) * 1000);

                if (httpContext.Response.Headers.TryGetValue(ServerTimingHeaderName, out var existing))
                {
                    httpContext.Response.Headers[ServerTimingHeaderName] = $"{existing}, reqlatency;dur={elapsedMs}";
                }
                else
                {
                    httpContext.Response.Headers.Append(ServerTimingHeaderName, $"reqlatency;dur={elapsedMs}");
                }
            }

            return Task.CompletedTask;
        }, context);

        return _next(context);
    }
}
