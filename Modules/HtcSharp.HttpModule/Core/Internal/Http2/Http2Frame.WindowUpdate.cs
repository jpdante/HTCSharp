// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace HtcSharp.HttpModule.Core.Internal.Http2 {
    // SourceTools-Start
    // Remote-File C:\ASP\src\Servers\Kestrel\Core\src\Internal\Http2\Http2Frame.WindowUpdate.cs
    // Start-At-Remote-Line 5
    // SourceTools-End
    /* https://tools.ietf.org/html/rfc7540#section-6.9
        +-+-------------------------------------------------------------+
        |R|              Window Size Increment (31)                     |
        +-+-------------------------------------------------------------+
    */
    internal partial class Http2Frame {
        public int WindowUpdateSizeIncrement { get; set; }

        public void PrepareWindowUpdate(int streamId, int sizeIncrement) {
            PayloadLength = 4;
            Type = Http2FrameType.WINDOW_UPDATE;
            Flags = 0;
            StreamId = streamId;
            WindowUpdateSizeIncrement = sizeIncrement;
        }
    }
}