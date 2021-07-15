﻿using System.Collections.Generic;
using HtcSharp.Logging.Appenders;

namespace HtcSharp.Logging.Config.Appenders {
    public class FileAppenderConfig : AppenderConfig {

        public override AppenderType Type { get; set; } = AppenderType.File;

        public string FileName { get; set; } = "log.log";

        public override IAppender GetAppender() {
            var formatter = Formatter?.GetFormatter();
            return new FileAppender(FileName, LogLevel, formatter);
        }
    }
}