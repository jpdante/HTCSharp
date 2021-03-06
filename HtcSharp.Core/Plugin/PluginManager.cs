﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
using System.Threading.Tasks;
using HtcSharp.Abstractions;
using HtcSharp.Logging;

namespace HtcSharp.Core.Plugin {
    public class PluginManager {

        private readonly ILogger Logger = LoggerManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        private readonly List<IPlugin> _plugins;
        private readonly Dictionary<IPlugin, BasePlugin> _pluginsDictionary;

        public IReadOnlyList<IPlugin> Plugins => _plugins;

        public PluginManager() {
            _plugins = new List<IPlugin>();
            _pluginsDictionary = new Dictionary<IPlugin, BasePlugin>();
        }

        public async Task LoadPlugins(string path) {
            if (!Directory.Exists(path)) return;
            foreach (string assemblyPath in GetFiles(path, "*.plugin.dll", SearchOption.AllDirectories)) {
                try {
                    await LoadPlugin(assemblyPath);
                } catch (Exception ex) {
                    Logger.LogError($"Failed to load plugin '{assemblyPath}'.", ex);
                }
            }
        }

        public async Task LoadPlugin(string assemblyPath) {
            var assemblyLoadContext = new AssemblyLoadContext("ModuleContext", true);
            var assembly = assemblyLoadContext.LoadFromAssemblyPath(assemblyPath);
            foreach (var pluginType in assembly.GetTypes().Where(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsAbstract)) {
                var plugin = Activator.CreateInstance(pluginType) as IPlugin;
                if (plugin == null) continue;
                // TODO: Check if is compatible
                _plugins.Add(plugin);
                _pluginsDictionary.Add(plugin, new BasePlugin(plugin, assemblyLoadContext));
                await plugin.Load();
                Logger.LogInfo($"Loaded plugin {plugin.Name} {plugin.Version}.");
            }
        }

        public async Task InitPlugins() {
            foreach (var plugin in _plugins.ToArray()) {
                try {
                    await InitPlugin(plugin);
                    Logger.LogInfo($"Enabled plugin {plugin.Name} {plugin.Version}.");
                } catch (Exception ex) {
                    Logger.LogError($"Failed to enable plugin {plugin.Name} {plugin.Version}.", ex);
                }
            }
        }

        public async Task InitPlugin(IPlugin plugin) {
            await plugin.Enable();
        }

        public async Task UnloadPlugins() {
            foreach (var plugin in _plugins.ToArray()) {
                try {
                    await UnloadPlugin(plugin);
                    Logger.LogInfo($"Unloaded plugin {plugin.Name} {plugin.Version}.");
                } catch (Exception ex) {
                    Logger.LogError($"Failed to load plugin {plugin.Name} {plugin.Version}.", ex);
                }
            }
        }

        public async Task UnloadPlugin(IPlugin plugin) {
            if (!_pluginsDictionary.TryGetValue(plugin, out var baseModule)) return;
            await plugin.Disable();
            baseModule.Unload();
            _plugins.Remove(plugin);
            _pluginsDictionary.Remove(plugin);
        }

        private static string[] GetFiles(string path, string searchPattern = "*.*", SearchOption searchOption = SearchOption.TopDirectoryOnly) {
            List<string> files = Directory.GetFiles(path, searchPattern, SearchOption.TopDirectoryOnly).ToList();
            if (searchOption == SearchOption.TopDirectoryOnly) return files.ToArray();
            foreach (string subDir in Directory.GetDirectories(path)) {
                try {
                    files.AddRange(GetFiles(subDir, searchPattern, searchOption));
                } catch {
                    // ignored
                }
            }
            return files.ToArray();
        }

    }
}