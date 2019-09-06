using Microsoft.Xna.Framework.Input;
using Terraria.ModLoader;

namespace TLoZ
{
    public static class TLoZInput
    {
        public static void Load(Mod mod)
        {
            EquipParaglider = mod.RegisterHotKey("Use paraglider", "F");
            ChangeRune = mod.RegisterHotKey("Rune select", "Q");
            ZTarget = mod.RegisterHotKey("Z/L-Targeting", "Z");
        }

        public static void Unload()
        {
            EquipParaglider = null;
            ChangeRune = null;
            ZTarget = null;
        }

        public static void Update()
        {
            LastState = CurrentState;
        }

        public static bool HasTriggeredKey(Keys key) => CurrentState.IsKeyDown(key) && !LastState.IsKeyDown(key);

        public static bool IsHoldingKey(Keys key) => CurrentState.IsKeyDown(key);

        public static KeyboardState CurrentState => Keyboard.GetState();

        public static KeyboardState LastState { get; private set; }

        public static ModHotKey ChangeRune { get; private set; }
        public static ModHotKey EquipParaglider { get; private set; }
        public static ModHotKey ZTarget { get; private set; }
    }
}
