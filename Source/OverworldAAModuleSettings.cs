using System;

namespace Celeste.Mod.OverworldAA;

public class OverworldAAModuleSettings : EverestModuleSettings {
    [SettingInGame(false)]
    public int AntialiasingLevel { get; set; } = 3;

    public void CreateAntialiasingLevelEntry(TextMenu menu, bool inGame) {
        TextMenu.Slider antialiasingLevelEntry;

        menu.Add(antialiasingLevelEntry = new TextMenu.Slider(
            label: "Antialiasing Level",
            values: i => {
                return i switch {
                    0 => "Disabled",
                    1 => "2x",
                    2 => "4x",
                    3 => "8x",
                    _ => throw new ArgumentOutOfRangeException()
                };
            },
            min: 0,
            max: 3,
            value: AntialiasingLevel));

        antialiasingLevelEntry.Change(newValue => AntialiasingLevel = newValue);
    }
}