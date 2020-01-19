﻿using System;
using System.Collections.Generic;
using System.Text;
using HtcSharp.HttpModule.Core.Http.Features;

namespace HtcSharp.HttpModule.Core.Http.Http2 {
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
            set {
                _userTrailers = value;
            }
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
