// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace HtcSharp.HttpModule.Core.Internal.Http2 {
    // SourceTools-Start
    // Remote-File C:\ASP\src\Servers\Kestrel\Core\src\Internal\Http2\Http2Frame.Priority.cs
    // Start-At-Remote-Line 5
    // SourceTools-End
    /* https://tools.ietf.org/html/rfc7540#section-6.3
        +-+-------------------------------------------------------------+
        |E|                  Stream Dependency (31)                     |
        +-+-------------+-----------------------------------------------+
        |   Weight (8)  |
        +-+-------------+
    */
    internal partial class Http2Frame {
        public int PriorityStreamDependency { get; set; }

        public bool PriorityIsExclusive { get; set; }

        public byte PriorityWeight { get; set; }

        public void PreparePriority(int streamId, int streamDependency, bool exclusive, byte weight) {
            PayloadLength = 5;
            Type = Http2FrameType.PRIORITY;
            StreamId = streamId;
            PriorityStreamDependency = streamDependency;
            PriorityIsExclusive = exclusive;
            PriorityWeight = weight;
        }
    }
}