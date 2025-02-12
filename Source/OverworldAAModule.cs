﻿using System;
using Monocle;

namespace Celeste.Mod.OverworldAA;

public class OverworldAAModule : EverestModule {
    public static OverworldAAModule Instance { get; private set; }

    public override Type SettingsType => typeof(OverworldAAModuleSettings);
    public static OverworldAAModuleSettings Settings => (OverworldAAModuleSettings) Instance._Settings;

    public override Type SessionType => typeof(OverworldAAModuleSession);
    public static OverworldAAModuleSession Session => (OverworldAAModuleSession) Instance._Session;

    public override Type SaveDataType => typeof(OverworldAAModuleSaveData);
    public static OverworldAAModuleSaveData SaveData => (OverworldAAModuleSaveData) Instance._SaveData;

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

    public override void Load() {
        On.Celeste.MountainModel.ResetRenderTargets += modMountainModelResetRenderTargets;
    }

    public override void Unload() {
        On.Celeste.MountainModel.ResetRenderTargets -= modMountainModelResetRenderTargets;
    }

    // this would be better as an IL hook
    // I could've also done orig() and then replace the buffer, but that requires creating a reduntant render target (which is fast tbf)
    private void modMountainModelResetRenderTargets(On.Celeste.MountainModel.orig_ResetRenderTargets orig, MountainModel self) {
        int width = Math.Min(1920, Engine.ViewWidth);
        int height = Math.Min(1080, Engine.ViewHeight);
        if (self.buffer != null && !self.buffer.IsDisposed && (self.buffer.Width == width || self.LockBufferResizing))
            return;
        self.DisposeTargets();
        self.buffer = VirtualContent.CreateRenderTarget("mountain-a", width, height, true, false, 8);
        self.blurA = VirtualContent.CreateRenderTarget("mountain-blur-a", width / 2, height / 2);
        self.blurB = VirtualContent.CreateRenderTarget("mountain-blur-b", width / 2, height / 2);
    }
}