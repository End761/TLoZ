using System.Collections.Generic;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TLoZ.Extensions;
using TLoZ.Runes;

namespace TLoZ.Players
{
    public sealed partial class TLoZPlayer : ModPlayer
    {
        private void InitializeRunes()
        {
            UnlockedRunes = new List<Rune>();
        }

        private void ProcessRuneSelectionTriggers(TriggersSet triggersSet)
        {
            if (IsSelectingRune)
            {
                if (UnlockedRunes.Count > 0)
                {
                    int newInputLag = 10;

                    if (triggersSet.Left && InputDelay <= 0)
                    {
                        InputDelay = newInputLag;
                        SelectedRune = UnlockedRunes.Previous(SelectedRune);
                    }

                    if (triggersSet.Right && InputDelay <= 0)
                    {
                        InputDelay = newInputLag;
                        SelectedRune = UnlockedRunes.Next(SelectedRune);
                    }

                    if (PlayerInput.ScrollWheelDelta > 0 && this.InputDelay <= 0)
                        SelectedRune = UnlockedRunes.Next(SelectedRune);

                    if (PlayerInput.ScrollWheelDelta < 0 && this.InputDelay <= 0)
                        SelectedRune = UnlockedRunes.Previous(SelectedRune);
                }
                PlayerInput.ScrollWheelDelta = 0;
            }
        }

        public void UnlockRune(Rune rune)
        {
            if (SelectedRune == null)
                SelectedRune = rune;

            UnlockedRunes.Add(rune);
        }

        private void SaveRunes(TagCompound tag)
        {
            if (SelectedRune != null)
                tag.Add(nameof(SelectedRune), SelectedRune.UnlocalizedName);

            List<string> runeNames = new List<string>(UnlockedRunes.Count);

            for (int i = 0; i < UnlockedRunes.Count; i++)
                runeNames.Add(UnlockedRunes[i].UnlocalizedName);

            tag.Add(nameof(UnlockedRunes), runeNames);
        }

        private void LoadRunes(TagCompound tag)
        {
            if (tag.ContainsKey(nameof(SelectedRune)))
                SelectedRune = RuneManager.Instance[tag.GetString(nameof(SelectedRune))];

            List<string> runeNames = tag.GetList<string>(nameof(UnlockedRunes)) as List<string>;
            
            for (int i = 0; i < runeNames.Count; i++)
                UnlockedRunes.Add(RuneManager.Instance[runeNames[i]]);
        }


        public Rune SelectedRune { get; private set; }

        internal List<Rune> UnlockedRunes { get; private set; }
    }
}
