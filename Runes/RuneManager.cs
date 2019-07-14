﻿using Terraria.Graphics;
using TLoZ.Managers;

namespace TLoZ.Runes
{
    public class RuneManager : SingletonManager<RuneManager, Rune>
    {
        internal override void DefaultInitialize()
        {
            base.DefaultInitialize();

            Stasis = Add(new StasisRune()) as StasisRune;

            BombRound = Add(new BombRoundRune()) as BombRoundRune;
            BombSquare = Add(new BombSquareRune()) as BombSquareRune;
        }

        public Rune GetPrevious(Rune current)
        {
            int index = GetIndex(current);

            return index == 0 ? this[Count - 1] : this[index - 1];
        }

        public Rune GetNext(Rune current)
        {
            int index = GetIndex(current);

            return index == Count - 1 ? this[0] : this[index + 1];
        }

        public StasisRune Stasis { get; private set; }

        public Rune BombRound { get; private set; }
        public Rune BombSquare { get; private set; }

        public Rune Magnesis { get; private set; }

        public Rune Cryonis { get; private set; }
    }
}