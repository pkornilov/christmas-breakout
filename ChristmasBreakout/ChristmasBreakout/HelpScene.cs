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
    public class HelpScene : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;
        private Vector2 bounds;
        private string header;
        private string help;
        private string item;

        public HelpScene(Game game, SpriteBatch spriteBatch,
            SpriteFont spriteFont,
            Vector2 bounds)
            : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.spriteFont = spriteFont;
            this.bounds = bounds;
            header = "HELP";
            help = "Christmas Breakout is an independent\r\n" +
                "edition of the famous Atari Breakout.\r\n" +
                "In the game, there is a layer of blocks\r\n" +
                "(candy) that the player needs to destroy\r\n" +
                "using the ball and the paddle (bat).";
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
            int tempY = (int)bounds.Y / 2 - (spriteFont.LineSpacing * 9) / 2;

            spriteBatch.Begin();
            spriteBatch.DrawString(spriteFont, header, new Vector2(bounds.X / 2 - spriteFont.MeasureString(header).X / 2, tempY), Color.White);
            tempY += spriteFont.LineSpacing * 2;
            spriteBatch.DrawString(spriteFont, help, new Vector2(bounds.X / 2 - spriteFont.MeasureString(help).X / 2, tempY), Color.White);
            tempY += spriteFont.LineSpacing * 6;
            spriteBatch.DrawString(spriteFont, item, new Vector2(bounds.X / 2 - spriteFont.MeasureString(item).X / 2, tempY), new Color(249, 45, 46));
            spriteBatch.End();

            base.Draw(gameTime);
        }

        // Show/hide the help scene
        public void Show(bool act)
        {
            this.Enabled = act;
            this.Visible = act;
        }
    }
}
