using System;
using Monocle;

namespace Celeste.Mod.OverworldAA;

public class OverworldAAModule : EverestModule {
    public static OverworldAAModule Instance { get; private set; }

    public override Type SettingsType => typeof(OverworldAAModuleSettings);
    public static OverworldAAModuleSettings Settings => (OverworldAAModuleSettings) Instance._Settings;

    public OverworldAAModule() {
        Instance = this;
#if DEBUG
        // debug builds use verbose logging
        Logger.SetLogLevel(nameof(OverworldAAModule), LogLevel.Verbose);
#else
        // release builds use info logging to reduce spam in log files
        Logger.SetLogLevel(nameof(OverworldAAModule), LogLevel.Info);
#endif
    }

    private static AALevel? _lastAaLevel;

    public override void Load() {
        On.Celeste.MountainModel.ResetRenderTargets += ModMountainModelResetRenderTargets;
    }

    public override void Unload() {
        On.Celeste.MountainModel.ResetRenderTargets -= ModMountainModelResetRenderTargets;
    }

    // this would probably be better as an IL hook
    private static void ModMountainModelResetRenderTargets(On.Celeste.MountainModel.orig_ResetRenderTargets orig, MountainModel self) {
        var AALevelValue = Settings.AntialiasingLevel switch {
            AALevel.Disabled => 0,
            AALevel._2x => 2,
            AALevel._4x => 4,
            AALevel._8x => 8,
            _ => throw new ArgumentOutOfRangeException()
        };

        // orig (mostly)
        var width = Math.Min(1920, Engine.ViewWidth);
        var height = Math.Min(1080, Engine.ViewHeight);
        if (self.buffer is { IsDisposed: false } && (self.buffer.Width == width || self.LockBufferResizing) && Settings.AntialiasingLevel == _lastAaLevel)
            return;
        self.DisposeTargets();
        self.buffer = VirtualContent.CreateRenderTarget("mountain-a", width, height, true, false, AALevelValue);
        self.blurA = VirtualContent.CreateRenderTarget("mountain-blur-a", width / 2, height / 2);
        self.blurB = VirtualContent.CreateRenderTarget("mountain-blur-b", width / 2, height / 2);
        // end orig

        Logger.Log(LogLevel.Info, "OverworldAA", $"Set overworld AA level to {AALevelValue}");
        _lastAaLevel = Settings.AntialiasingLevel;
    }
}