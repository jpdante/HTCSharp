// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using HtcSharp.HttpModule.Connections.Abstractions.Exceptions;
using HtcSharp.HttpModule.Core.Features;
using HtcSharp.HttpModule.Core.Internal.Http;
using HtcSharp.HttpModule.Http.Features;

namespace HtcSharp.HttpModule.Core.Internal.Http2 {
    // SourceTools-Start
    // Remote-File C:\ASP\src\Servers\Kestrel\Core\src\Internal\Http2\Http2Stream.FeatureCollection.cs
    // Start-At-Remote-Line 13
    // SourceTools-End
    internal partial class Http2Stream : IHttp2StreamIdFeature,
        IHttpMinRequestBodyDataRateFeature,
        IHttpResetFeature,
        IHttpResponseTrailersFeature {
        private IHeaderDictionary _userTrailers;

        IHeaderDictionary IHttpResponseTrailersFeature.Trailers {
            get {
                if (ResponseTrailers == null) {
                    ResponseTrailers = new HttpResponseTrailers();
                    if (HasResponseCompleted) {
                        ResponseTrailers.SetReadOnly();
                    }
                }

                return _userTrailers ?? ResponseTrailers;
            }
            set { _userTrailers = value; }
        }

        int IHttp2StreamIdFeature.StreamId => _context.StreamId;

        MinDataRate IHttpMinRequestBodyDataRateFeature.MinDataRate {
            get => throw new NotSupportedException(CoreStrings.Http2MinDataRateNotSupported);
            set {
                if (value != null) {
                    throw new NotSupportedException(CoreStrings.Http2MinDataRateNotSupported);
                }

                MinRequestBodyDataRate = value;
            }
        }

        void IHttpResetFeature.Reset(int errorCode) {
            var abortReason = new ConnectionAbortedException(CoreStrings.FormatHttp2StreamResetByApplication((Http2ErrorCode)errorCode));
            ResetAndAbort(abortReason, (Http2ErrorCode)errorCode);
        }
    }
}