using System.ComponentModel;

namespace Celeste.Mod.OverworldAA;

public class OverworldAAModuleSettings : EverestModuleSettings {
    [SettingInGame(false)]
    [DefaultValue(AALevel._8x)]
    public AALevel AntialiasingLevel { get; set; }
}

public enum AALevel {
    Disabled,
    _2x,
    _4x,
    _8x
}