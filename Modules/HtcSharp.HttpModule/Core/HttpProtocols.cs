// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace HtcSharp.HttpModule.Core {
    // SourceTools-Start
    // Remote-File C:\ASP\src\Servers\Kestrel\Core\src\HttpProtocols.cs
    // Start-At-Remote-Line 7
    // SourceTools-End
    [Flags]
    public enum HttpProtocols {
        None = 0x0,
        Http1 = 0x1,
        Http2 = 0x2,
        Http1AndHttp2 = Http1 | Http2,
    }
}