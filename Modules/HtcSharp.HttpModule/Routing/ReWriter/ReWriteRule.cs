﻿using System;
using System.Text.RegularExpressions;
using HtcSharp.HttpModule.Http.Abstractions;

namespace HtcSharp.HttpModule.Routing.ReWriter {
    public class ReWriteRule {
        private readonly byte _ruleType;
        private readonly Regex _pattern;
        private readonly string _rewriteData;
        private readonly string _flag;
        private readonly int _statusCode;

        public ReWriteRule(string rule) {
            var ruleParts = rule.Split(" ");
            if (ruleParts[0].Equals("rewrite", StringComparison.CurrentCultureIgnoreCase)) {
                _ruleType = 1;
                _pattern = new Regex(ruleParts[1]);
                _rewriteData = ruleParts[2];
                if (ruleParts.Length == 4) _flag = ruleParts[3];
            } else if (ruleParts[0].Equals("return", StringComparison.CurrentCultureIgnoreCase)) {
                _ruleType = 2;
                _statusCode = int.Parse(ruleParts[1]);
                _rewriteData = ruleParts[2];
                _flag = string.Empty;
            }
        }

        public byte MatchRule(string request, HttpContext context, out string newRequest) {
            if (_ruleType == 1) {
                var requestParts = request.Split("/");
                if (_pattern.IsMatch(request)) {
                    byte response = 0;
                    newRequest = _rewriteData;
                    newRequest = newRequest.Replace("$scheme", context.Request.Scheme);
                    newRequest = newRequest.Replace("$host", context.Request.Host.ToString());
                    foreach (Match match in _pattern.Matches(request)) {
                        newRequest = newRequest.Replace($"${match.Name}", match.Value);
                    }

                    /*var rewriteQuery = _rewriteData.Split("?");
                    request = rewriteQuery[0].Replace("$scheme", context.Request.Scheme);
                    if (rewriteQuery.Length == 1) {
                        request = _rewriteData;
                        Logger.Info($"{request}");
                        for (var p = 1; p < requestParts.Length; p++) {
                            request = request.Replace($"${p}", requestParts[p]);
                        }
                        Logger.Info($"{request}");
                    } else if (rewriteQuery.Length == 2) {
                        var queryParts = rewriteQuery[1].Split("&");
                        foreach (var query in queryParts) {
                            var queryData = query.Split("=");
                            var key = queryData[0];
                            var value = queryData[1];
                            for (var p = 1; p < requestParts.Length; p++) {
                                value = value.Replace($"${p}", requestParts[p]);
                            }
                            context.Request.Query.Add(key, value);
                        }
                    }*/
                    if (_flag.Equals("last", StringComparison.CurrentCultureIgnoreCase)) response = 0;
                    if (_flag.Equals("break", StringComparison.CurrentCultureIgnoreCase)) response = 1;
                    if (_flag.Equals("redirect", StringComparison.CurrentCultureIgnoreCase)) response = 2;
                    if (_flag.Equals("permanent", StringComparison.CurrentCultureIgnoreCase)) response = 3;
                    return response;
                }
            } else if (_ruleType == 2) {
                newRequest = request;
                return 0;
            }

            newRequest = request;
            return 0;
        }
    }
}