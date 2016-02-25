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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        SpriteFont spriteFont2;

        GameState gs;
        EnterReleased er;

        int score;
        int lifes;
        const int SCOREPOINTS = 10;
        const int LIFEBONUS = 15;
        const int WINDOWWIDTH = 565;
        const int WINDOWHEIGHT = 400;
        const int MAXLIFES = 10;

        // Textures
        Texture2D snowTex;
        Texture2D snowDriftTex;
        Texture2D blockTex;
        Texture2D ballTex;
        Texture2D paddleTex;
        Texture2D heartTex;

        // Snowfall
        Vector2 snowPosition1;
        Vector2 snowPosition2;
        Vector2 snowSpeed;

        // Blocks (candy)
        List<Rectangle> blocks;
        const int BLOCKCOLS = 7;
        const int BLOCKROWS = 5;
        const int BLOCKWIDTH = 75;
        const int BLOCKHEIGHT = 25;
        const int BLOCKMARGIN = 20;

        // Ball
        Rectangle ball;
        Vector2 ballSpeed;
        const int BALLSPEED = 7;
        const int BALLDIM = 20;

        // Paddle (bat)
        Rectangle paddle;
        const int PADDLEPSPEED = 7;
        const int PADDLEWIDTH = 95;
        const int PADDLEHEIGHT = 20;

        EndScene endScene;
        MenuScene menuScene;
        HowToScene howToScene;
        AboutScene aboutScene;
        HelpScene helpScene;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = WINDOWWIDTH;
            graphics.PreferredBackBufferHeight = WINDOWHEIGHT;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
            MenuScene();
            gs = GameState.Menu;
            er = EnterReleased.Yes;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            spriteFont = Content.Load<SpriteFont>("fonts/spriteFont");
            spriteFont2 = Content.Load<SpriteFont>("fonts/spriteFont2");

            snowTex = Content.Load<Texture2D>("images/snow");
            snowDriftTex = Content.Load<Texture2D>("images/snowdrift");
            blockTex = Content.Load<Texture2D>("images/block");
            ballTex = Content.Load<Texture2D>("images/ball");
            paddleTex = Content.Load<Texture2D>("images/paddle");
            heartTex = Content.Load<Texture2D>("images/heart");

            snowSpeed = new Vector2(0, 1);
            snowPosition1 = Vector2.Zero;
            snowPosition1 = new Vector2(0, -snowTex.Height);

            endScene = new EndScene(this, spriteBatch, spriteFont, spriteFont2, new Vector2(WINDOWWIDTH,
                WINDOWHEIGHT), score, lifes);
            Components.Add(endScene);

            menuScene = new MenuScene(this, spriteBatch, spriteFont, new Vector2(WINDOWWIDTH,
                WINDOWHEIGHT));
            Components.Add(menuScene);

            howToScene = new HowToScene(this, spriteBatch, spriteFont, new Vector2(WINDOWWIDTH,
                WINDOWHEIGHT));
            Components.Add(howToScene);

            aboutScene = new AboutScene(this, spriteBatch, spriteFont, new Vector2(WINDOWWIDTH,
               WINDOWHEIGHT));
            Components.Add(aboutScene);

            helpScene = new HelpScene(this, spriteBatch, spriteFont, new Vector2(WINDOWWIDTH,
               WINDOWHEIGHT));
            Components.Add(helpScene);
        }

        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();

            // Always
            snowPosition1 += snowSpeed;
            snowPosition2 += snowSpeed;

            if (snowPosition1.Y > WINDOWHEIGHT)
                snowPosition1.Y = snowPosition2.Y - snowTex.Height;
            if (snowPosition2.Y > WINDOWHEIGHT)
                snowPosition2.Y = snowPosition1.Y - snowTex.Height;

            // Running / Pause
            if (gs == GameState.Running || gs == GameState.Pause)
            {
                if (ks.IsKeyDown(Keys.Left))
                {
                    paddle.X -= PADDLEPSPEED;
                    if (paddle.X <= 0)
                        paddle.X = 0;
                }
                if (ks.IsKeyDown(Keys.Right))
                {
                    paddle.X += PADDLEPSPEED;
                    if (paddle.X > WINDOWWIDTH - paddle.Width)
                        paddle.X = WINDOWWIDTH - paddle.Width;
                }

                if (blocks.Count() == 0)
                {
                    EndScene();
                    gs = GameState.Over;
                    endScene.Score = score;
                    endScene.LifeBonus = lifes * LIFEBONUS;
                    return;
                }

                if (lifes == 0)
                {
                    EndScene();
                    gs = GameState.Over;
                    endScene.Score = score;
                    endScene.LifeBonus = lifes;
                    lifes = MAXLIFES;
                    return;
                }
            }

            // Pause
            if (gs == GameState.Pause)
            {
                if (ks.IsKeyUp(Keys.Enter))
                {
                    er = EnterReleased.Yes;
                }

                ball.X = paddle.Center.X - ballTex.Width / 2;

                if (ks.IsKeyDown(Keys.Space) || (ks.IsKeyDown(Keys.Enter) && er == EnterReleased.Yes))
                {
                    gs = GameState.Running;
                    ballSpeed.Y = -BALLSPEED;
                }
            }

            // Menu
            if (gs == GameState.Menu)
            {
                if (ks.IsKeyUp(Keys.Enter))
                {
                    er = EnterReleased.Yes;
                }

                if (menuScene.Index == 0 && er == EnterReleased.Yes && ks.IsKeyDown(Keys.Enter))
                {
                    StartGame();
                    er = EnterReleased.No;
                }
                if (menuScene.Index == 1 && er == EnterReleased.Yes && ks.IsKeyDown(Keys.Enter))
                {
                    HowToScene();
                    er = EnterReleased.No;
                }
                if (menuScene.Index == 2 && er == EnterReleased.Yes && ks.IsKeyDown(Keys.Enter))
                {
                    HelpScene();
                    er = EnterReleased.No;
                }
                if (menuScene.Index == 3 && er == EnterReleased.Yes && ks.IsKeyDown(Keys.Enter))
                {
                    AboutScene();
                    er = EnterReleased.No;
                }
                if (menuScene.Index == 4 && ks.IsKeyDown(Keys.Enter))
                {
                    this.Exit();
                }
            }

            if (gs == GameState.Help || gs == GameState.HowTo || gs == GameState.About)
            {
                if (ks.IsKeyUp(Keys.Enter))
                {
                    er = EnterReleased.Yes;
                }
                if (er == EnterReleased.Yes && ks.IsKeyDown(Keys.Enter))
                {
                    MenuScene();
                    er = EnterReleased.No;
                }
            }

            //Game Over
            if (gs == GameState.Over)
            {
                if (endScene.Index == 0 && ks.IsKeyDown(Keys.Enter))
                {
                    StartGame();
                    er = EnterReleased.No;
                }
                if (endScene.Index == 1 && ks.IsKeyDown(Keys.Enter))
                {
                    MenuScene();
                    er = EnterReleased.No;
                }
            }

            // Running
            if (gs == GameState.Running)
            {
                // Ball Speed
                ball.X += (int)ballSpeed.X;
                ball.Y += (int)ballSpeed.Y;

                // Ball - Paddle
                if (ball.Intersects(paddle))
                {
                    ballSpeed.Y *= -1;
                    ballSpeed.X = (ball.Center.X - paddle.Center.X) / 6; // To slow down
                }

                // Ball - Block
                foreach (var block in blocks)
                {
                    if (ball.Intersects(block))
                    {
                        blocks.Remove(block);
                        score += SCOREPOINTS;
                        ballSpeed.Y *= -1;
                        break;
                    }
                }

                // Ball - Walls
                if (ball.Y < 0)
                {
                    ballSpeed.Y *= -1;
                }

                if (ball.X < 0 || ball.X > WINDOWWIDTH - ball.Width)
                {
                    ballSpeed.X *= -1;
                }

                if (ball.Y > WINDOWHEIGHT)
                {
                    ResetPaddle();
                    lifes--;
                    return;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(4, 31, 122));

            spriteBatch.Begin();

            spriteBatch.Draw(snowTex, snowPosition1, Color.LightBlue);
            spriteBatch.Draw(snowTex, snowPosition2, Color.LightBlue);

            foreach (var block in blocks)
                spriteBatch.Draw(blockTex, block, Color.White);

            spriteBatch.Draw(paddleTex, paddle, Color.White);
            spriteBatch.Draw(ballTex, ball, Color.White);

            spriteBatch.Draw(snowDriftTex, new Vector2(0, WINDOWHEIGHT - snowDriftTex.Height), Color.White);

            if (gs == GameState.Running || gs == GameState.Pause)
            {
                spriteBatch.DrawString(spriteFont, score.ToString(), new Vector2(WINDOWWIDTH -
                    spriteFont.MeasureString(score.ToString("0 ")).X, WINDOWHEIGHT - spriteFont.LineSpacing + 2), Color.White);
                spriteBatch.Draw(heartTex, new Vector2(5, WINDOWHEIGHT - heartTex.Height - 3),
                    new Color(249, 45, 46));
                spriteBatch.DrawString(spriteFont, lifes.ToString(), new Vector2(heartTex.Width + 8,
                    WINDOWHEIGHT - spriteFont.LineSpacing + 2), new Color(249, 45, 46));
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void ResetLevel()
        {
            int blockCols = BLOCKCOLS;
            int blockRows = BLOCKROWS;
            score = 0;
            lifes = MAXLIFES;
            Rectangle blockSize = new Rectangle(0, 0, BLOCKWIDTH, BLOCKHEIGHT);
            blocks = new List<Rectangle>();

            for (int cols = 0; cols < blockCols; cols++)
            {
                for (int rows = 0; rows < blockRows; rows++)
                {
                    blocks.Add(new Rectangle(BLOCKMARGIN + cols * blockSize.Width, BLOCKMARGIN +
                        rows * blockSize.Height, blockSize.Width, blockSize.Height));
                }
            }

            ResetPaddle();
        }

        protected void ResetPaddle()
        {
            paddle = new Rectangle(0, 0, PADDLEWIDTH, PADDLEHEIGHT);
            paddle.X = WINDOWWIDTH / 2 - paddle.Width / 2;
            paddle.Y = WINDOWHEIGHT - 47;

            ball = new Rectangle(paddle.Center.X, paddle.Center.Y - paddle.Height - BALLDIM / 2,
                BALLDIM, BALLDIM);
            ballSpeed = Vector2.Zero;

            gs = GameState.Pause;
        }

        protected void HideGame()
        {
            paddle = new Rectangle();
            ball = new Rectangle();
            blocks = new List<Rectangle>();
        }

        protected void EndScene()
        {
            gs = GameState.Over;

            HideGame();
            endScene.Show(true);
            menuScene.Show(false);
            howToScene.Show(false);
            aboutScene.Show(false);
            helpScene.Show(false);

            endScene.Index = 0;
        }

        protected void StartGame()
        {
            gs = GameState.Pause;

            ResetLevel();
            endScene.Show(false);
            menuScene.Show(false);
            howToScene.Show(false);
            aboutScene.Show(false);
            helpScene.Show(false);
        }

        protected void MenuScene()
        {
            gs = GameState.Menu;

            HideGame();
            endScene.Show(false);
            menuScene.Show(true);
            howToScene.Show(false);
            aboutScene.Show(false);
            helpScene.Show(false);
        }

        protected void HowToScene()
        {
            gs = GameState.HowTo;

            HideGame();
            endScene.Show(false);
            menuScene.Show(false);
            howToScene.Show(true);
            aboutScene.Show(false);
            helpScene.Show(false);
        }

        protected void AboutScene()
        {
            gs = GameState.About;

            HideGame();
            endScene.Show(false);
            menuScene.Show(false);
            howToScene.Show(false);
            aboutScene.Show(true);
            helpScene.Show(false);
        }

        protected void HelpScene()
        {
            gs = GameState.Help;

            HideGame();
            endScene.Show(false);
            menuScene.Show(false);
            howToScene.Show(false);
            aboutScene.Show(false);
            helpScene.Show(true);
        }

        enum GameState { Menu, Running, Pause, Over, HowTo, About, Help };
        enum EnterReleased { Yes, No };
    }
}