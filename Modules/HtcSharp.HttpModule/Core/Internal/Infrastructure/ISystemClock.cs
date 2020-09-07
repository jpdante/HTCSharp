// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace HtcSharp.HttpModule.Core.Internal.Infrastructure {
    // SourceTools-Start
    // Remote-File C:\ASP\src\Servers\Kestrel\Core\src\Internal\Infrastructure\ISystemClock.cs
    // Start-At-Remote-Line 7
    // SourceTools-End
    /// <summary>
    /// Abstracts the system clock to facilitate testing.
    /// </summary>
    internal interface ISystemClock {
        /// <summary>
        /// Retrieves the current UTC system time.
        /// </summary>
        DateTimeOffset UtcNow { get; }

        /// <summary>
        /// Retrieves ticks for the current UTC system time.
        /// </summary>
        long UtcNowTicks { get; }

        /// <summary>
        /// Retrieves the current UTC system time.
        /// This is only safe to use from code called by the <see cref="Heartbeat"/>.
        /// </summary>
        DateTimeOffset UtcNowUnsynchronized { get; }
    }
}