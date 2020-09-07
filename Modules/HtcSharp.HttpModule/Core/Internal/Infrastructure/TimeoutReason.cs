﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace HtcSharp.HttpModule.Core.Internal.Infrastructure {
    // SourceTools-Start
    // Remote-File C:\ASP\src\Servers\Kestrel\Core\src\Internal\Infrastructure\TimeoutReason.cs
    // Start-At-Remote-Line 5
    // SourceTools-End
    internal enum TimeoutReason {
        None,
        KeepAlive,
        RequestHeaders,
        ReadDataRate,
        WriteDataRate,
        RequestBodyDrain,
        TimeoutFeature,
    }
}