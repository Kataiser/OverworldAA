using System;

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
        // TODO: apply any hooks that should always be active
    }

    public override void Unload() {
        // TODO: unapply any hooks applied in Load()
    }
}