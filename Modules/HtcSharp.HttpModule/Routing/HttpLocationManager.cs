﻿using System.Collections.Generic;
using System.Threading.Tasks;
using HtcSharp.HttpModule.Http.Abstractions;
using Newtonsoft.Json.Linq;

namespace HtcSharp.HttpModule.Routing {
    public class HttpLocationManager {
        internal readonly List<LocationConfig> Locations;
        internal readonly LocationConfig DefaultConfig;

        public HttpLocationManager(JToken defaultConfig, JObject locationConfigs) {
            DefaultConfig = new LocationConfig(null, defaultConfig as JArray, this, true);
            Locations = new List<LocationConfig>();
            if (locationConfigs == null) return;
            foreach (var (key, value) in locationConfigs) {
                Locations.Add(new LocationConfig(key, value as JArray, this));
            }
        }

        public async Task ProcessRequest(HttpContext context) {
            context.Request.RequestPath = context.Request.Path;
            //context.Request.RequestFilePath = context.Request.Path;
            foreach (var locationConfig in Locations) {
                if (!locationConfig.MatchLocation(context)) continue;
                await locationConfig.Execute(context);
                break;
            }

            if (!context.Response.HasStarted) await DefaultConfig.Execute(context);
        }
    }
}