namespace Celeste.Mod.OverworldAA;

public class OverworldAAModuleSettings : EverestModuleSettings {
    [SettingInGame(false)]
    public AALevels AntialiasingLevel { get; set; } = AALevels._8x;
}

public enum AALevels {
    Disabled,
    _2x,
    _4x,
    _8x
}