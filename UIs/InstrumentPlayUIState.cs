using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using Terraria.UI;
using TLoZ.Notes;
using TLoZ.Players;

namespace TLoZ.UIs
{
    public class InstrumentPlayUIState : UIState
    {   
        private const float
            PANEL_WIDTH = 420,
            PANEL_HEIGHT = 200;

        public override void OnInitialize()
        {
            MainPanel = new UIPanel();

            MainPanel.Width.Set(PANEL_WIDTH, 0);
            MainPanel.Height.Set(PANEL_HEIGHT, 0);

            MainPanel.VAlign = 0.8f;
            MainPanel.HAlign = 0.5f;

            MainPanel.BackgroundColor = Color.Black * 0.75f;
            MainPanel.BorderColor = Color.Black * 0.75f;

            UIText playText = new UIText("Use Arrow Keys & 'A' Key to play");
            playText.VAlign = 0.02f;
            playText.HAlign = 0.5f;
            playText.TextColor = Color.Gray;

            UIText resetText = new UIText("Press 'X' to clear notes");
            resetText.VAlign = 0.16f;
            resetText.HAlign = 0.5f;
            resetText.TextColor = Color.Gray;

            MainPanel.Append(playText);
            MainPanel.Append(resetText);

            base.Append(MainPanel);
        }

        public override void Update(GameTime gameTime)
        {
            MainPanel.VAlign = 0.8f;
            MainPanel.HAlign = 0.5f;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (!Visible)
                return;

            base.DrawSelf(spriteBatch);
        }

        protected override void DrawChildren(SpriteBatch spriteBatch)
        {
            // aren't playing instrument, don't draw
            if (!Visible)
                return;


            base.DrawChildren(spriteBatch);

            Vector2 position = MainPanel.GetDimensions().Position() + new Vector2(16, 44);


            // draw note stripes
            for (int i = 0; i < 4; i++)
                spriteBatch.Draw(Main.magicPixel, position + new Vector2(0, 32 + 22 * i), new Rectangle(0, 0, (int)PANEL_WIDTH - 32, 4), Color.Red, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

            // draw note thing™
            spriteBatch.Draw(TLoZTextures.MiscNoteThing, position + new Vector2(-10, 20), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);


            // no notes played, don't need the rest of code to be run
            if(TLoZPlayer.CurrentNotes.Count <= 0)
                return;

            foreach(Note note in TLoZPlayer.CurrentNotes)
            {
                Texture2D texture = ModContent.GetTexture(note.TexturePath);

                int noteIndex = TLoZPlayer.CurrentNotes.FindIndex(x => x == note);

                spriteBatch.Draw(texture, position + new Vector2(46 + 42 * noteIndex, note.HeightOffset), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            }
        }

        public UIPanel MainPanel { get; private set; }

        public bool Visible => TLoZPlayer.IsPlayingInstrument;

        public TLoZPlayer TLoZPlayer => TLoZPlayer.Get(Main.LocalPlayer);
    }
}
