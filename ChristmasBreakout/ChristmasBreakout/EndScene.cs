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
    public class EndScene : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont; //regular text font (18pt)
        private SpriteFont spriteFont2; //result screen - score font (48pt)
        private Vector2 bounds; //position of whole block
        private int score;
        private int lifeBonus;
        private int total;

        public int Score { get { return score; } set { score = value; } }
        public int LifeBonus { get { return lifeBonus; } set { lifeBonus = value; } }

        //navigation items (Play Again, Go to Menu)
        List<string> items = new List<string>();
        int index = 0; //selected index (0 - Play Again)
        public int Index { get { return index; } set { index = value; } }
        private const int ITEMINDENT = 25; //indention between navigation items

        public EndScene(Game game, SpriteBatch spriteBatch,
            SpriteFont spriteFont,
            SpriteFont spriteFont2,
            Vector2 bounds,
            int score,
            int lifes)
            : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.spriteFont = spriteFont;
            this.spriteFont2 = spriteFont2;
            this.bounds = bounds;
            this.score = score;
            this.lifeBonus = lifes;
            items.Add("Replay");
            items.Add("Main Menu");
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Left))
            {
                index--;
                if (index == -1)
                    index = 0;
            }
            if (ks.IsKeyDown(Keys.Right))
            {
                index++;
                if (index == items.Count)
                    index = 1;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            int tempX = (int)bounds.X / 2 - (int)spriteFont.MeasureString(items[0].ToString()
                + items[1].ToString()).X / 2 - ITEMINDENT / 2;
            int tempY = (int)bounds.Y / 2 - (spriteFont.LineSpacing * 8) / 2;

            total = score + lifeBonus;

            spriteBatch.Begin();

            spriteBatch.DrawString(spriteFont, "Total Score:", new Vector2(bounds.X / 2 - 
                spriteFont.MeasureString("Total Score:").X / 2, tempY), Color.White);
            tempY += spriteFont.LineSpacing; //next line, regular spacing
            spriteBatch.DrawString(spriteFont2, total.ToString(), new Vector2(bounds.X / 2 -
                spriteFont2.MeasureString(total.ToString()).X / 2, tempY), Color.White);
            tempY += spriteFont.LineSpacing * 3; //next line, score (big) spacing
            spriteBatch.DrawString(spriteFont, "Points: " + score.ToString(), new Vector2(bounds.X / 2 -
                spriteFont.MeasureString("Points: " + score.ToString()).X / 2, tempY), Color.White);
            tempY += spriteFont.LineSpacing; //next line, regular spacing
            spriteBatch.DrawString(spriteFont, "Life Bonus: " + lifeBonus.ToString(), new Vector2(bounds.X / 2 -
                spriteFont.MeasureString("Life Bonus: " + lifeBonus.ToString()).X / 2, tempY), Color.White);
            tempY += spriteFont.LineSpacing * 2; //skip one line, regular spacing

            //navigation selection logic; selected - blue, unselected - white
            for (int i = 0; i < items.Count; i++)
                if (index == i)
                {
                    spriteBatch.DrawString(spriteFont, items[i], new Vector2(tempX, tempY),
                        new Color(249, 45, 46));
                    tempX += (int)spriteFont.MeasureString(items[i].ToString()).X + ITEMINDENT;
                }
                else
                {
                    spriteBatch.DrawString(spriteFont, items[i], new Vector2(tempX, tempY), Color.White);
                    tempX += (int)spriteFont.MeasureString(items[i].ToString()).X + ITEMINDENT;
                }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        // Show/hide game over scene
        public void Show(bool act)
        {
            this.Enabled = act;
            this.Visible = act;
        }
    }
}
