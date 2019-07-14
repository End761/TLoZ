using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TLoZ.Runes;

namespace TLoZ.Players
{
    public sealed partial class TLoZPlayer : ModPlayer
    {
        private void ProcessRuneSelectionTriggers(TriggersSet triggersSet)
        {
            if (IsSelectingRune)
            {
                int newInputLag = 8;

                if (triggersSet.Left && inputLag <= 0)
                {
                    inputLag = newInputLag;
                    SelectedRune = RuneManager.Instance.GetPrevious(SelectedRune);
                }

                if (triggersSet.Right && inputLag <= 0)
                {
                    inputLag = newInputLag;
                    SelectedRune = RuneManager.Instance.GetNext(SelectedRune);
                }

                if (PlayerInput.ScrollWheelDelta > 0 && this.inputLag <= 0)
                    SelectedRune = RuneManager.Instance.GetNext(SelectedRune);

                if (PlayerInput.ScrollWheelDelta < 0 && this.inputLag <= 0)
                    SelectedRune = RuneManager.Instance.GetPrevious(SelectedRune);

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

            UnlockedRunes.Clear();
            List<string> runeNames = tag.GetList<string>(nameof(UnlockedRunes)) as List<string>;
            
            for (int i = 0; i < runeNames.Count; i++)
                UnlockedRunes.Add(RuneManager.Instance[runeNames[i]]);
        }


        public Rune SelectedRune { get; private set; }

        internal List<Rune> UnlockedRunes { get; private set; } = new List<Rune>();
    }
}
