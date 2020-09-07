// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Security.Claims;

namespace HtcSharp.HttpModule.Http.Features.Authentication {
    // SourceTools-Start
    // Remote-File C:\ASP\src\Http\Http\src\Features\Authentication\HttpAuthenticationFeature.cs
    // Start-At-Remote-Line 7
    // SourceTools-End
    public class HttpAuthenticationFeature : IHttpAuthenticationFeature {
        public ClaimsPrincipal User { get; set; }
    }
}