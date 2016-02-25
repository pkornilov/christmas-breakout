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
    public class AboutScene : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;
        private Vector2 bounds;
        private string header;
        private string about;
        private string item;

        public AboutScene(Game game, SpriteBatch spriteBatch,
            SpriteFont spriteFont,
            Vector2 bounds)
            : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.spriteFont = spriteFont;
            this.bounds = bounds;
            header = "ABOUT";
            about = "Christmas Breakout - v1.0\r\n" +
                "Built by Philippe Kornilov\r\n" +
                "(c) Copyright, 2015";
            item = "Main Menu";
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            int tempY = (int)bounds.Y / 2 - (spriteFont.LineSpacing * 7) / 2;

            spriteBatch.Begin();
            spriteBatch.DrawString(spriteFont, header, new Vector2(bounds.X / 2 - spriteFont.MeasureString(header).X / 2, tempY), Color.White);
            tempY += spriteFont.LineSpacing * 2;
            spriteBatch.DrawString(spriteFont, about, new Vector2(bounds.X / 2 - spriteFont.MeasureString(about).X / 2, tempY), Color.White);
            tempY += spriteFont.LineSpacing * 4;
            spriteBatch.DrawString(spriteFont, item, new Vector2(bounds.X / 2 - spriteFont.MeasureString(item).X / 2, tempY), new Color(249, 45, 46));
            spriteBatch.End();

            base.Draw(gameTime);
        }

        // Show/hide the how to play scene
        public void Show(bool act)
        {
            this.Enabled = act;
            this.Visible = act;
        }
    }
}
