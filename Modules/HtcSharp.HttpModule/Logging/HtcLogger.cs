﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace HtcSharp.HttpModule.Logging {
    // SourceTools-Start
    // Ignore-Copyright
    // SourceTools-End
    public class HtcLogger : ILogger {
        private readonly HtcSharp.Core.Logging.Abstractions.ILogger _logger;

        public HtcLogger(HtcSharp.Core.Logging.Abstractions.ILogger logger) {
            _logger = logger;
        }

        internal IExternalScopeProvider ScopeProvider { get; set; }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) {
            var level = (int) logLevel;
            _logger.Log((HtcSharp.Core.Logging.Abstractions.LogLevel) level, $"[{eventId}] {formatter(state, exception)}", exception);
        }

        public bool IsEnabled(LogLevel logLevel) {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state) => ScopeProvider?.Push(state) ?? null;
    }
}