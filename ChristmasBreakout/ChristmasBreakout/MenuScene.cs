using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ChristmasBreakout
{
    public class MenuScene : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;
        private Vector2 bounds;

        private const int LINESPACE = 2; // additional spacing between lines

        // List of menu items
        List<string> items = new List<string>();
        int index = 0; //selected index (0 - Play, 1 - Help, etc.)
        public int Index { get { return index; } set { index = value; } }

        // Old keyboard state
        KeyboardState oks = Keyboard.GetState();

        public MenuScene(Game game, SpriteBatch spriteBatch,
            SpriteFont spriteFont,
            Vector2 bounds)
            : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.spriteFont = spriteFont;
            this.bounds = bounds;
            items.Add("Start Game");
            items.Add("How to Play");
            items.Add("Help");
            items.Add("About");
            items.Add("Exit");
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Down) && oks.IsKeyUp(Keys.Down))
            {
                index++;
                if (index == items.Count)
                    index = 0;
            }
            if (ks.IsKeyDown(Keys.Up) && oks.IsKeyUp(Keys.Up))
            {
                index--;
                if (index == -1)
                    index = items.Count - 1;
            }
            oks = ks;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 itemPosition = bounds; //position of each item; to be iterated
            Color itemColor = Color.White; //colour of each item; selected to be blue

            int tempY = (int)bounds.Y / 2 - (spriteFont.LineSpacing * 5) / 2;

            spriteBatch.Begin();
            for (int i = 0; i < items.Count; i++)
            {
                if (index == i)
                    itemColor = new Color(249, 45, 46); //selected item
                else
                    itemColor = Color.White; //unselected items

                itemPosition = new Vector2(bounds.X / 2 - spriteFont.MeasureString(items[i]).X / 2,
                    tempY);

                tempY += spriteFont.LineSpacing + LINESPACE;

                spriteBatch.DrawString(spriteFont, items[i], itemPosition, itemColor);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        // Show/hide the menu scene
        public void Show(bool act)
        {
            this.Enabled = act;
            this.Visible = act;
        }

    }
}
