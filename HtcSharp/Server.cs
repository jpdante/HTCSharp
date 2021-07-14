﻿using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HtcSharp.Core;
using HtcSharp.Core.Module;
using HtcSharp.Core.Plugin;
using HtcSharp.Internal;
using HtcSharp.Logging;
using HtcSharp.Logging.Appenders;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace HtcSharp {
    public class Server : DaemonApplication {

        private readonly ILogger Logger = LoggerManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

        private ArgsReader ArgsReader;
        private Config Config;
        private ModuleManager ModuleManager;
        private PluginManager PluginManager;

        protected override async Task OnLoad() {
            var multiAppender = new MultiAppender();
            multiAppender.AddAppender(new ConsoleAppender(LogLevel.All));
            multiAppender.AddAppender(new RollingFileAppender(new RollingFileAppender.RollingFileConfig(), LogLevel.All));
            LoggerManager.Init(multiAppender);

            Logger.LogInfo("Loading...");

            ArgsReader = new ArgsReader(Args);
            await LoadConfig();

            ModuleManager = new ModuleManager();
            await ModuleManager.LoadModules(Path.GetFullPath(Config.ModulesPath));

            PluginManager = new PluginManager();
            await PluginManager.LoadPlugins(Path.GetFullPath(Config.ModulesPath));
        }

        protected override async Task OnStart() {
            Logger.LogInfo("Starting...");
            await ModuleManager.InitModules();
            await PluginManager.InitPlugins();
        }

        protected override async Task OnExit() {
            await PluginManager.UnloadPlugins();
            await ModuleManager.UnloadModules();
            Logger.LogInfo("Exiting...");
        }

        private Task LoadConfig() {
            string configPath = ArgsReader.GetOrDefault("config", "./config.yml");
            configPath = Path.GetFullPath(configPath);
            if (File.Exists(configPath)) {
                using var fileStream = new FileStream(configPath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
                using var streamReader = new StreamReader(fileStream, Encoding.UTF8);
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(new CamelCaseNamingConvention())
                    .Build();
                Config = deserializer.Deserialize<Config>(streamReader);
            } else {
                using var fileStream = new FileStream(configPath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
                using var streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
                Config = new Config();
                var serializer = new SerializerBuilder()
                    .WithNamingConvention(new PascalCaseNamingConvention())
                    .Build();
                serializer.Serialize(streamWriter, Config);
            }
            return Task.CompletedTask;
        }
    }
}