using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using System.IO;
using BallGame;
using RC_Framework;

namespace StateLevelExampleV3
{
    /// <summary>
    /// helper class for global constants such as directory 
    /// </summary>
    class Dir
    {
        public static string dir = @"C:\uc\Game Programming (7191)\MT2\BallGame\BallGame\BallGame\BallGameContent\";        
    }
    

    // -------------------------------------------------------- Game level 0 ----------------------------------------------------------------------------------
    class GameLevel_0 : RC_GameStateParent
    {
        ImageBackground back1 = null;
        ImageBackground back2 = null;
        Texture2D texBack;
        Texture2D texBack1;
        Texture2D texBall = null;
        Scrolling scrolling1;
        Scrolling scrolling2;
        Sprite3 anmiation1;
        SpriteList anmiationList;
        int b;
        string dir = @"C:\uc\Game Programming (7191)\MT2\BallGame\BallGame\BallGame\BallGameContent\";


        public override void LoadContent()
        {
            scrolling1 = new Scrolling(Content.Load<Texture2D>("skyMenu"), new Rectangle(0, 0, 800, 600));
            scrolling2 = new Scrolling(Content.Load<Texture2D>("skyMenu"), new Rectangle(800, 0, 800, 600));
            texBack = texFromFile(graphicsDevice, dir + "menu1.png");
            texBall = texFromFile(graphicsDevice, dir + "ballMenu.png");
            back1 = new ImageBackground(texBack, Color.White, graphicsDevice);

            texBack1 = texFromFile(graphicsDevice, dir + "help.png");
            back2 = new ImageBackground(texBack1, Color.White, graphicsDevice);



            anmiationList = new SpriteList();
            for (int y = 0; y < 2; y++)
            {
   
                    anmiation1 = new Sprite3(true, texBall, 190, 265+b);
                    anmiation1.setXframes(3);
                    anmiation1.setYframes(1);
                    anmiation1.setWidthHeight(32, 32);
                     Vector2[] seq = new Vector2[3];
                    seq[0].X = 1; seq[0].Y = 0;
                    seq[1].X = 2; seq[1].Y = 0;
                    seq[2].X = 0; seq[2].Y = 0;

                    anmiation1.setAnimationSequence(seq, 0, 2, 15);
                    anmiation1.animationStart();
                    anmiationList.addSpriteReuse(anmiation1);

                    b = b + 56;
            }
        }

        public override void Update(GameTime gameTime)
        {
            anmiationList.animationTick();
            prevKeyState = keyState;
            keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Space) && !prevKeyState.IsKeyDown(Keys.Space))
            {
                gameStateManager.setLevel(1);
            }
            if (keyState.IsKeyDown(Keys.H) && !prevKeyState.IsKeyDown(Keys.H))
            {
                back1 = new ImageBackground(texBack1, Color.White, graphicsDevice);
                for (int i = 0; i < anmiationList.count(); i++)
                {

                    Sprite3 s = anmiationList.getSprite(i);
                    if (s == null) continue;
                    if (!s.active) continue;
                    if (!s.visible) continue;
                    s.setActive(false);
                }

            }
            if (keyState.IsKeyDown(Keys.M) && !prevKeyState.IsKeyDown(Keys.M))
            {
                back1 = new ImageBackground(texBack, Color.White, graphicsDevice);
                for (int i = 0; i < anmiationList.count(); i++)
                {

                    Sprite3 s = anmiationList.getSprite(i);
                    if (s == null) continue;
                   // if (!s.active) continue;
                   // if (!s.visible) continue;
                    s.setActive(true);
                }

            }
            if (scrolling1.rectangle.X + scrolling1.texture.Width <= 0)
                scrolling1.rectangle.X = scrolling2.rectangle.X + scrolling2.texture.Width;
            if (scrolling2.rectangle.X + scrolling2.texture.Width <= 0)
                scrolling2.rectangle.X = scrolling1.rectangle.X + scrolling1.texture.Width;

            scrolling1.Update();
            scrolling2.Update();
        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.Aqua);
            spriteBatch.Begin(SpriteSortMode.Deferred,BlendState.NonPremultiplied);
            
            scrolling1.Draw(spriteBatch);
            scrolling2.Draw(spriteBatch);
            back1.Draw(spriteBatch);
            //anmiation1.Draw(spriteBatch);
            anmiationList.drawActive(spriteBatch);

            //if (keyState.IsKeyDown(Keys.H) && !prevKeyState.IsKeyDown(Keys.H))
            //{
            //    back2.Draw(spriteBatch);
            //}
            spriteBatch.End();
        }
        public static Texture2D texFromFile(GraphicsDevice gd, string fName)
        {
            Stream fs = new FileStream(fName, FileMode.Open);
            Texture2D re = Texture2D.FromStream(gd, fs);
            fs.Close();
            return re;
        }
    }


    // -------------------------------------------------------- Game level 1 ----------------------------------------------------------------------------------
    class GameLevel_1 : RC_GameStateParent
    {
        GraphicsDeviceManager graphics;
        //SpriteBatch spriteBatch;
 //       List<Sprite3> Spritewall = new List<Sprite3>();
        List<Sprite3> dragons = new List<Sprite3>();
        Texture2D texBack = null;
        Texture2D texpaddle = null;
        Texture2D texBall = null;
        Texture2D spider = null;
        Texture2D bee = null;
        Texture2D texDragon = null;

        Texture2D texAnmiation1 = null;
        Sprite3 anmiation1 = null;
        SpriteList dragonList;
        Sprite3 dragon = null;

        //scoure 
        int scoure;
        SoundEffect music;
        LimitSound limSound;
        SoundEffect bazooka;
        SoundEffectInstance baz;

        Texture2D Particle1;
        Texture2D Particle2;
        Texture2D Particle3;
        Texture2D Particle4;
        Texture2D Particle5;
        Texture2D Particle6;

        Texture2D tex;

        Random random;
        int ticks = 0;
        int a = 0;
        int b = 0;

        Scrolling scrolling1;
        Scrolling scrolling2;


        ParticleSystem p;
        int tix = 0;
        Color screenColour = Color.Tan;

        Vector2 pTarget = new Vector2(0, 0);
        Vector2 ballPos;
        WayPointList wl = null;
        Rectangle rectangle = new Rectangle(0, 0, 800, 600);


        float xx = 350;
        float yy = 500;
        KeyboardState k;

        int paddleSpeed = 4;
        int lhs = 236;
        int rhs = 564;
        int bot = 543;
        int top = 56; // *** 
                      //      Bounds the top of the play area

        Sprite3 ball = null; //ball
        bool ballStuck = true;
        Vector2 ballOffset = new Vector2(32, -10);

        string dir = @"C:\uc\Game Programming (7191)\MT2\BallGame\BallGame\BallGame\BallGameContent\"; //***

        //this is just for the futre so we have a singe directory so we need to change less code when we move the program to new machines or directories
        Sprite3 paddle = null;
        //This will become the paddle sprite

        ImageBackground back1 = null;
        //        This will be used to display the background

        Rectangle playArea;
        //      This will define the inside rectangle of the play area

        bool showbb = false;
        //    Just a falag to help us see bounding information and hotspots

        KeyboardState prevK;
        //Somewhere to store previous keyboard information so we can detect a single key click(as opposed to the 30 a second we get on a key depress)

        


        public override void LoadContent()
        {
            spriteBatch = new SpriteBatch(graphicsDevice);

            //
            random = new Random();

          //  font1 = Content.Load<SpriteFont>("Fontey");

            Particle1 = Util.texFromFile(graphicsDevice, Dir.dir + "Particle1.png");
            Particle2 = Util.texFromFile(graphicsDevice, Dir.dir + "Particle2.png");
            Particle3 = Util.texFromFile(graphicsDevice, Dir.dir + "Particle3.png");
            Particle4 = Util.texFromFile(graphicsDevice, Dir.dir + "ParticleArrow.png");
            Particle5 = Util.texFromFile(graphicsDevice, Dir.dir + "Cloud8.png");
            Particle6 = Util.texFromFile(graphicsDevice, Dir.dir + "Bug2.png");

            tex = Particle2;

          
            //

            font1 = Content.Load<SpriteFont>("SpriteFont1");
            texBack = texFromFile(graphicsDevice, dir + "levelOneBack1.png");
            spider = texFromFile(graphicsDevice, dir + "Bug.png");
            texpaddle = texFromFile(graphicsDevice, dir + "paddle1.png");
            bee = texFromFile(graphicsDevice, dir + "Bee2.png");
            texAnmiation1 = Util.texFromFile(graphicsDevice, dir + "coin3.png");
            texDragon = Util.texFromFile(graphicsDevice, dir + "dragon.png");
            scrolling1 = new Scrolling(Content.Load<Texture2D>("skyMenu"), new Rectangle(0, 0, 800, 600));
            scrolling2 = new Scrolling(Content.Load<Texture2D>("skyMenu"), new Rectangle(800, 0, 800, 600));
            music = Content.Load<SoundEffect>("MUSIC1");
            limSound = new LimitSound(music, 3);
            bazooka = Content.Load<SoundEffect>("bazooka_fire");
            baz = bazooka.CreateInstance();

            LineBatch.init(graphicsDevice);


            dragonList = new SpriteList();
            for (int y = 0; y < 2; y++)
            {
                for (int i = 0; i < 5; i++)
                {
                    dragon = new Sprite3(true, texDragon, 250 + a, 80 + b);
                    dragon.setXframes(8);
                    dragon.setYframes(1);
                    dragon.setWidthHeight(32, 32);
                    //anmiation1.setBBToTexture();
                    dragon.setBB(7, 7, 130, 124);
                    Vector2[] seq1 = new Vector2[8];
                    seq1[0].X = 0; seq1[0].Y = 0;
                    seq1[1].X = 1; seq1[1].Y = 0;
                    seq1[2].X = 2; seq1[2].Y = 0;
                    seq1[3].X = 3; seq1[3].Y = 0;
                    seq1[4].X = 4; seq1[4].Y = 0;
                    seq1[5].X = 5; seq1[5].Y = 0;
                    seq1[6].X = 6; seq1[6].Y = 0;
                    seq1[7].X = 7; seq1[7].Y = 0;

                    dragon.setAnimationSequence(seq1, 0, 7, 10);
                    dragon.animationStart();
                    // if (dragonList != null)
                    dragonList.addSpriteReuse(dragon);
                    a = a + 64;
                }
                a = 0;
                b = b + 80;
            }

            ballPos = new Vector2(xx, yy);
            paddle = new Sprite3(true, texpaddle, xx, bot - texpaddle.Height);
            paddle.setBBToTexture();
            back1 = new ImageBackground(texBack, Color.White, graphicsDevice);
            playArea = new Rectangle(lhs, top, rhs - lhs, bot - top); // width and height

            texBall = Util.texFromFile(graphicsDevice, dir + "ball2.png"); //***

            ball = new Sprite3(true, texBall, xx, yy);
            ball.setBBandHSFractionOfTexCentered(0.7f);
        }

        public override void Update(GameTime gameTime)
        {
            ticks++;

            //load animation

            dragonList.animationTick();
            if (scrolling1.rectangle.X + scrolling1.texture.Width <= 0)
                scrolling1.rectangle.X = scrolling2.rectangle.X + scrolling2.texture.Width;
            if (scrolling2.rectangle.X + scrolling2.texture.Width <= 0)
                scrolling2.rectangle.X = scrolling1.rectangle.X + scrolling1.texture.Width;

            scrolling1.Update();
            scrolling2.Update();

            for (int i = 0; i < dragonList.count(); i++)
            {
                
                Sprite3 s = dragonList.getSprite(i);
                if (s == null) continue;
                if (!s.active) continue;
                if (!s.visible) continue;
                if(ball.getBoundingBoxAA().Intersects(s.getBoundingBoxAA()))
                {
                    baz.Play();
                    s.setActive(false);
                    scoure++;
                    ball.setDeltaSpeed(ball.getDeltaSpeed() * new Vector2(1, -1));
                    setSys5();
                    p.Update(gameTime);
                }
            }

            if (k.IsKeyDown(Keys.B) && prevK.IsKeyUp(Keys.B)) // ***
            {
                showbb = !showbb;
            }

                     
                      
            // TODO: Add your update logic here
            k = Keyboard.GetState();

            if (k.IsKeyDown(Keys.Right))
            {
                if (paddle.getPosX() < rhs - texpaddle.Width) paddle.setPosX(paddle.getPosX() + paddleSpeed);
            }

            if (k.IsKeyDown(Keys.Left))
            {
                if (paddle.getPosX() > lhs) paddle.setPosX(paddle.getPosX() - paddleSpeed);
            }

            if (ballStuck)
            {
                ball.setPos(paddle.getPos() + ballOffset);
            }

            if (ballStuck)
            {
                ball.setPos(paddle.getPos() + ballOffset);
                if (k.IsKeyDown(Keys.Space) && prevK.IsKeyUp(Keys.Space))
                {
                    ballStuck = false;
                    ball.setDeltaSpeed(new Vector2(1, -2));
                }
            }
            else
            {
                // move ball
                ball.savePosition();
                ball.moveByDeltaXY();
            }

            if (ballStuck)
            {
                ball.setPos(paddle.getPos() + ballOffset);
                if (k.IsKeyDown(Keys.Space) && prevK.IsKeyUp(Keys.Space))
                {
                    ballStuck = false;
                    ball.setDeltaSpeed(new Vector2(2, -3));
                }
            }
            else
            {
                // move ball
                ball.savePosition();
                ball.moveByDeltaXY();
                Rectangle ballbb = ball.getBoundingBoxAA();

                if (ballbb.X + ballbb.Width > rhs)
                {
                    ball.setDeltaSpeed(ball.getDeltaSpeed() * new Vector2(-1, 1));
                }

                if (ballbb.X < lhs)
                {
                    ball.setDeltaSpeed(ball.getDeltaSpeed() * new Vector2(-1, 1));
                }

                if (ballbb.Y < top)
                {
                    ball.setDeltaSpeed(ball.getDeltaSpeed() * new Vector2(1, -1));
                }

                //pedal intersects

                if (ballbb.Intersects(paddle.getBoundingBoxAA()))

                {
                    ball.setDeltaSpeed(ball.getDeltaSpeed() * new Vector2(1, -1));
                }

            }

            if (k.IsKeyDown(Keys.P))
            {
                gameStateManager.setLevel(0);
            }
            if (k.IsKeyDown(Keys.M))
            {
                gameStateManager.setLevel(0);
                ball.setPos(xx, yy);
                paddle.setPos(xx, bot - texpaddle.Height);
                scoure = 0;
                //ball.setDeltaSpeed(new Vector2(0,0));
                for (int i = 0; i < dragonList.count(); i++)
                {
                    Sprite3 s = dragonList.getSprite(i);
                    s.setActive(true);
                    // p.Update(gameTime);
                }
            }


            if (ball.getPosY() > bot)
            {
                gameStateManager.setLevel(2);
                ball.setPos(xx, yy);
                paddle.setPos(xx, bot - texpaddle.Height);
                scoure = 0;
                //ball.setDeltaSpeed(new Vector2(0,0));
                for (int i = 0; i < dragonList.count(); i++)
                {
                    Sprite3 s = dragonList.getSprite(i);
                    s.setActive(true);
                   // p.Update(gameTime);
                }
            }
            // next leve condition
            if (scoure >= 10)
            {
                gameStateManager.setLevel(4);
                limSound.playSound();
            }
            ballPos = new Vector2(ball.getPosX(), ball.getPosY());
            if(p != null)
            p.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.BlanchedAlmond);
            spriteBatch.Begin(SpriteSortMode.Deferred,BlendState.NonPremultiplied);
            scrolling1.Draw(spriteBatch);
            scrolling2.Draw(spriteBatch);


            back1.Draw(spriteBatch); //***
            paddle.Draw(spriteBatch); //***
            ball.draw(spriteBatch);
            if(p != null)
            p.Draw(spriteBatch);
            if (showbb)
            {
                paddle.drawBB(spriteBatch, Color.Black);
                paddle.drawHS(spriteBatch, Color.Green);
                dragonList.drawInfo(spriteBatch, Color.Red, Color.Blue);
                LineBatch.drawLineRectangle(spriteBatch, playArea, Color.Blue);
                ball.drawInfo(spriteBatch, Color.Gray, Color.Green);


            }

            if (wl != null) { wl.Draw(spriteBatch, Color.Teal, Color.Red); }
            LineBatch.drawLineRectangle(spriteBatch, rectangle, Color.Gray);

            //draw animation
            //string output1 = "Socre:  ";
            Vector2 FontOrigin1 = font1.MeasureString("a") / 2;
            spriteBatch.DrawString(font1, "Score " + scoure.ToString(), new Vector2(100, 100), Color.Green);
            //anmiation1.Draw(spriteBatch);
           // dragon.Draw(spriteBatch);
           //if(dragonList != null)
            dragonList.drawActive(spriteBatch);
            spriteBatch.End();
            
        }
        public static Texture2D texFromFile(GraphicsDevice gd, string fName)
        {
            Stream fs = new FileStream(fName, FileMode.Open);
            Texture2D re = Texture2D.FromStream(gd, fs);
            fs.Close();
            return re;
        }
        void setSys5()
        {
            p = new ParticleSystem(ballPos, 60, 999, 105);
            p.setMandatory1(tex, new Vector2(6, 6), new Vector2(12, 12), Color.OrangeRed, new Color(50, 50, 50, 50));
            p.setMandatory2(60, 40, 20, 2, 0);
           // rectangle = new Rectangle(500 - 40, 390 - 30, 80, 50);
            p.setMandatory3(40, rectangle);
            p.setMandatory4(new Vector2(0, 0.08f), new Vector2(0, -1.5f), new Vector2(2, 1));
            p.randomDelta = new Vector2(0.01f, 0.01f);
            p.Origin = 0;
            //p.originWayList = new WayPointList();
            //p.originWayList.makePathZigZag(new Vector2(100, 500), new Vector2(700, 500), new Vector2(0, 0), 3, 3);
            p.activate();
            wl = null;
        }
    }


    // -------------------------------------------------------- Game level 2 ----------------------------------------------------------------------------------

    class GameLevel_2 : RC_GameStateParent
    {
        ImageBackground back1 = null;
        Texture2D texBack;
        string dir = @"C:\uc\Game Programming (7191)\MT2\BallGame\BallGame\BallGame\BallGameContent\";

        public override void LoadContent()
        {
            font1 = Content.Load<SpriteFont>("SpriteFont1");
            texBack = texFromFile(graphicsDevice, dir + "gameOver.png");
            back1 = new ImageBackground(texBack, Color.White, graphicsDevice);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState k = Keyboard.GetState();

            if (k.IsKeyDown(Keys.R))
                gameStateManager.setLevel(1);
            if (k.IsKeyDown(Keys.M))
                gameStateManager.setLevel(0);
        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.Chartreuse);
            spriteBatch.Begin(SpriteSortMode.Deferred,BlendState.NonPremultiplied);
            back1.Draw(spriteBatch);
            spriteBatch.DrawString(font1, "Press r to restart", new Vector2(100, 80), Color.White);
            spriteBatch.End();
        }
        public static Texture2D texFromFile(GraphicsDevice gd, string fName)
        {
            Stream fs = new FileStream(fName, FileMode.Open);
            Texture2D re = Texture2D.FromStream(gd, fs);
            fs.Close();
            return re;
        }
    }


    // -------------------------------------------------------- Game level 3 ----------------------------------------------------------------------------------

    class GameLevel_3_Pause : RC_GameStateParent
     {

        GraphicsDeviceManager graphics;
        //SpriteBatch spriteBatch;
        List<Sprite3> Spritewall = new List<Sprite3>();
        List<Sprite3> dragons = new List<Sprite3>();
        Texture2D texBack = null;
        Texture2D texpaddle = null;
        Texture2D texBall = null;
        Texture2D spider = null;
        Texture2D bee = null;
        Texture2D texDragon = null;

        Texture2D texAnmiation1 = null;
        Sprite3 anmiation1 = null;
        SpriteList dragonList;
        Sprite3 dragon = null;

        //scoure 
        int scoure;
        SoundEffect music;
        LimitSound limSound;
        SoundEffect bazooka;
        SoundEffectInstance baz;

        Texture2D Particle1;
        Texture2D Particle2;
        Texture2D Particle3;
        Texture2D Particle4;
        Texture2D Particle5;
        Texture2D Particle6;

        Texture2D tex;

        Random random;
        int ticks = 0;
        int a = 0;
        int b = 0;

        Scrolling scrolling1;
        Scrolling scrolling2;


        ParticleSystem p;
        int tix = 0;
        Color screenColour = Color.Tan;

        Vector2 pTarget = new Vector2(0, 0);
        Vector2 ballPos;
        WayPointList wl = null;
        Rectangle rectangle = new Rectangle(0, 0, 800, 600);


        float xx = 350;
        float yy = 500;
        KeyboardState k;

        int paddleSpeed = 5;
        int lhs = 236;
        int rhs = 564;
        int bot = 543;
        int top = 56; // *** 
                      //      Bounds the top of the play area

        Sprite3 ball = null; //ball
        bool ballStuck = true;
        Vector2 ballOffset = new Vector2(32, -10);

        string dir = @"C:\uc\Game Programming (7191)\MT2\BallGame\BallGame\BallGame\BallGameContent\"; //***

        //this is just for the futre so we have a singe directory so we need to change less code when we move the program to new machines or directories
        Sprite3 paddle = null;
        //This will become the paddle sprite

        ImageBackground back1 = null;
        //        This will be used to display the background

        Rectangle playArea;
        //      This will define the inside rectangle of the play area

        bool showbb = false;
        //    Just a falag to help us see bounding information and hotspots

        KeyboardState prevK;
        //Somewhere to store previous keyboard information so we can detect a single key click(as opposed to the 30 a second we get on a key depress)




        public override void LoadContent()
        {
            spriteBatch = new SpriteBatch(graphicsDevice);

            //
            random = new Random();

            //  font1 = Content.Load<SpriteFont>("Fontey");

            Particle1 = Util.texFromFile(graphicsDevice, Dir.dir + "Particle1.png");
            Particle2 = Util.texFromFile(graphicsDevice, Dir.dir + "Particle2.png");
            Particle3 = Util.texFromFile(graphicsDevice, Dir.dir + "Particle3.png");
            Particle4 = Util.texFromFile(graphicsDevice, Dir.dir + "ParticleArrow.png");
            Particle5 = Util.texFromFile(graphicsDevice, Dir.dir + "Cloud8.png");
            Particle6 = Util.texFromFile(graphicsDevice, Dir.dir + "Bug2.png");

            tex = Particle2;


            //

            font1 = Content.Load<SpriteFont>("SpriteFont1");
            texBack = texFromFile(graphicsDevice, dir + "levelOneBack1.png");
            spider = texFromFile(graphicsDevice, dir + "Bug.png");
            texpaddle = texFromFile(graphicsDevice, dir + "paddle2.png");
            bee = texFromFile(graphicsDevice, dir + "Bee2.png");
            texAnmiation1 = Util.texFromFile(graphicsDevice, dir + "coin3.png");
            texDragon = Util.texFromFile(graphicsDevice, dir + "dragon.png");
            scrolling1 = new Scrolling(Content.Load<Texture2D>("skyMenu"), new Rectangle(0, 0, 800, 600));
            scrolling2 = new Scrolling(Content.Load<Texture2D>("skyMenu"), new Rectangle(800, 0, 800, 600));
            music = Content.Load<SoundEffect>("MUSIC1");
            limSound = new LimitSound(music, 3);
            bazooka = Content.Load<SoundEffect>("bazooka_fire");
            baz = bazooka.CreateInstance();

            LineBatch.init(graphicsDevice);


            dragonList = new SpriteList();
            for (int y = 0; y < 3; y++)
            {
                for (int i = 0; i < 5; i++)
                {
                    dragon = new Sprite3(true, texDragon, 250 + a, 80 + b);
                    dragon.setXframes(8);
                    dragon.setYframes(1);
                    dragon.setWidthHeight(32, 32);
                    //anmiation1.setBBToTexture();
                    dragon.setBB(7, 7, 130, 124);
                    Vector2[] seq1 = new Vector2[8];
                    seq1[0].X = 0; seq1[0].Y = 0;
                    seq1[1].X = 1; seq1[1].Y = 0;
                    seq1[2].X = 2; seq1[2].Y = 0;
                    seq1[3].X = 3; seq1[3].Y = 0;
                    seq1[4].X = 4; seq1[4].Y = 0;
                    seq1[5].X = 5; seq1[5].Y = 0;
                    seq1[6].X = 6; seq1[6].Y = 0;
                    seq1[7].X = 7; seq1[7].Y = 0;

                    dragon.setAnimationSequence(seq1, 0, 7, 10);
                    dragon.animationStart();
                    dragonList.addSpriteReuse(dragon);
                    a = a + 64;
                }
                a = 0;
                b = b + 80;
            }

            ballPos = new Vector2(xx, yy);
            paddle = new Sprite3(true, texpaddle, xx, bot - texpaddle.Height);
            paddle.setBBToTexture();
            back1 = new ImageBackground(texBack, Color.White, graphicsDevice);
            playArea = new Rectangle(lhs, top, rhs - lhs, bot - top); // width and height

            texBall = Util.texFromFile(graphicsDevice, dir + "ball2.png"); //***

            ball = new Sprite3(true, texBall, xx, yy);
            ball.setBBandHSFractionOfTexCentered(0.7f);
        }

        public override void Update(GameTime gameTime)
        {
            ticks++;

            //load animation
            //anmiation1.animationTick();
            dragonList.animationTick();
            if (scrolling1.rectangle.X + scrolling1.texture.Width <= 0)
                scrolling1.rectangle.X = scrolling2.rectangle.X + scrolling2.texture.Width;
            if (scrolling2.rectangle.X + scrolling2.texture.Width <= 0)
                scrolling2.rectangle.X = scrolling1.rectangle.X + scrolling1.texture.Width;

            scrolling1.Update();
            scrolling2.Update();

            for (int i = 0; i < dragonList.count(); i++)
            {

                Sprite3 s = dragonList.getSprite(i);
                if (s == null) continue;
                if (!s.active) continue;
                if (!s.visible) continue;
                if (ball.getBoundingBoxAA().Intersects(s.getBoundingBoxAA()))
                {
                    baz.Play();
                    s.setActive(false);
                    scoure++;
                    ball.setDeltaSpeed(ball.getDeltaSpeed() * new Vector2(1, -1));
                    setSys5();
                    p.Update(gameTime);
                }
            }

            if (k.IsKeyDown(Keys.B) && prevK.IsKeyUp(Keys.B)) // ***
            {
                showbb = !showbb;
            }
            k = Keyboard.GetState();

            if (k.IsKeyDown(Keys.Right))
            {
                if (paddle.getPosX() < rhs - texpaddle.Width) paddle.setPosX(paddle.getPosX() + paddleSpeed);
            }

            if (k.IsKeyDown(Keys.Left))
            {
                if (paddle.getPosX() > lhs) paddle.setPosX(paddle.getPosX() - paddleSpeed);
            }

            if (ballStuck)
            {
                ball.setPos(paddle.getPos() + ballOffset);
            }

            //if (ballStuck)
            //{
            //    ball.setPos(paddle.getPos() + ballOffset);
            //    if (k.IsKeyDown(Keys.Space) && prevK.IsKeyUp(Keys.Space))
            //    {
            //        ballStuck = false;
            //        ball.setDeltaSpeed(new Vector2(1, -3));
            //    }
            //}
            else
            {
                // move ball
                ball.savePosition();
                ball.moveByDeltaXY();
            }

            if (ballStuck)
            {
                ball.setPos(paddle.getPos() + ballOffset);
                if (k.IsKeyDown(Keys.Space) && prevK.IsKeyUp(Keys.Space))
                {
                    ballStuck = false;
                    ball.setDeltaSpeed(new Vector2(2, -3));
                }
            }
            else
            {
                // move ball
                ball.savePosition();
                ball.moveByDeltaXY();
                Rectangle ballbb = ball.getBoundingBoxAA();

                if (ballbb.X + ballbb.Width > rhs)
                {
                    ball.setDeltaSpeed(ball.getDeltaSpeed() * new Vector2(-1, 1));
                }

                if (ballbb.X < lhs)
                {
                    ball.setDeltaSpeed(ball.getDeltaSpeed() * new Vector2(-1, 1));
                }

                if (ballbb.Y < top)
                {
                    ball.setDeltaSpeed(ball.getDeltaSpeed() * new Vector2(1, -1));
                }

                //pedal intersects

                if (ballbb.Intersects(paddle.getBoundingBoxAA()))

                {
                    ball.setDeltaSpeed(ball.getDeltaSpeed() * new Vector2(1, -1));
                }

            }
            if (k.IsKeyDown(Keys.P))
            {
                gameStateManager.setLevel(0);
            }
            if (k.IsKeyDown(Keys.M))
            {
                gameStateManager.setLevel(0);
                ball.setPos(xx, yy);
                paddle.setPos(xx, bot - texpaddle.Height);
                scoure = 0;
                //ball.setDeltaSpeed(new Vector2(0,0));
                for (int i = 0; i < dragonList.count(); i++)
                {
                    Sprite3 s = dragonList.getSprite(i);
                    s.setActive(true);
                }
            }


            if (ball.getPosY() > bot)
            {
                gameStateManager.setLevel(5);
                ball.setPos(xx, yy);
                paddle.setPos(xx, bot - texpaddle.Height);
                scoure = 0;
                //ball.setDeltaSpeed(new Vector2(0,0));
                for (int i = 0; i < dragonList.count(); i++)
                {
                    Sprite3 s = dragonList.getSprite(i);
                    s.setActive(true);
                    // p.Update(gameTime);
                }
            }
            // next leve condition
            if (scoure >= 15)
            {
                gameStateManager.setLevel(0);
                limSound.playSound();
            }
            ballPos = new Vector2(ball.getPosX(), ball.getPosY());
            if (p != null)
                p.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.BlanchedAlmond);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            scrolling1.Draw(spriteBatch);
            scrolling2.Draw(spriteBatch);


            back1.Draw(spriteBatch); //***
            paddle.Draw(spriteBatch); //***
            ball.draw(spriteBatch);
            if (p != null)
                p.Draw(spriteBatch);
            if (showbb)
            {
                paddle.drawBB(spriteBatch, Color.Black);
                paddle.drawHS(spriteBatch, Color.Green);
                dragonList.drawInfo(spriteBatch, Color.Red, Color.Blue);
                LineBatch.drawLineRectangle(spriteBatch, playArea, Color.Blue);
                ball.drawInfo(spriteBatch, Color.Gray, Color.Green);


            }
            if (ball.getPosY() > bot)
            {
                //ball.setPosY(60);
                spriteBatch.DrawString(font1, "Game Over!", new Vector2(100, 100), Color.Green);
                //dragonList.drawActive(spriteBatch);
            }

            //foreach (Sprite3 p in Spritewall)
            //    p.Draw(spriteBatch);
            if (wl != null) { wl.Draw(spriteBatch, Color.Teal, Color.Red); }
            LineBatch.drawLineRectangle(spriteBatch, rectangle, Color.Gray);

            //draw animation
            //string output1 = "Socre:  ";
            Vector2 FontOrigin1 = font1.MeasureString("a") / 2;
            spriteBatch.DrawString(font1, "Score " + scoure.ToString(), new Vector2(100, 100), Color.Green);
            //anmiation1.Draw(spriteBatch);
            // dragon.Draw(spriteBatch);
            //if(dragonList != null)
            dragonList.drawActive(spriteBatch);
            spriteBatch.End();

        }
        public static Texture2D texFromFile(GraphicsDevice gd, string fName)
        {
            Stream fs = new FileStream(fName, FileMode.Open);
            Texture2D re = Texture2D.FromStream(gd, fs);
            fs.Close();
            return re;
        }
        void setSys5()
        {
            p = new ParticleSystem(ballPos, 60, 999, 105);
            p.setMandatory1(tex, new Vector2(6, 6), new Vector2(12, 12), Color.OrangeRed, new Color(50, 50, 50, 50));
            p.setMandatory2(60, 40, 20, 2, 0);
            // rectangle = new Rectangle(500 - 40, 390 - 30, 80, 50);
            p.setMandatory3(40, rectangle);
            p.setMandatory4(new Vector2(0, 0.08f), new Vector2(0, -1.5f), new Vector2(2, 1));
            p.randomDelta = new Vector2(0.01f, 0.01f);
            p.Origin = 0;
            //p.originWayList = new WayPointList();
            //p.originWayList.makePathZigZag(new Vector2(100, 500), new Vector2(700, 500), new Vector2(0, 0), 3, 3);
            p.activate();
            wl = null;
        }

    }


    // -------------------------------------------------------- Game level 4 ----------------------------------------------------------------------------------

    class GameLevel_4 : RC_GameStateParent
    {

        Texture2D texBack = null;
        Texture2D texpaddle = null;
        Texture2D texBall = null;
        Texture2D spider = null;
        Texture2D bee = null;
        Texture2D texDragon = null;

        Texture2D texAnmiation1 = null;
        Sprite3 anmiation1 = null;
        SpriteList dragonList;
        Sprite3 dragon = null;

        //scoure 
        int scoure;
        SoundEffect music;
        LimitSound limSound;
        SoundEffect bazooka;
        SoundEffectInstance baz;

        Texture2D Particle1;
        Texture2D Particle2;
        Texture2D Particle3;
        Texture2D Particle4;
        Texture2D Particle5;
        Texture2D Particle6;

        Texture2D tex;

        Random random;
        int ticks = 0;
        int a = 0;
        int b = 0;

        Scrolling scrolling1;
        Scrolling scrolling2;


        ParticleSystem p;
        int tix = 0;
        Color screenColour = Color.Tan;

        Vector2 pTarget = new Vector2(0, 0);
        Vector2 ballPos;
        WayPointList wl = null;
        Rectangle rectangle = new Rectangle(0, 0, 800, 600);

        ImageBackground back1 = null;
 
        string dir = @"C:\uc\Game Programming (7191)\MT2\BallGame\BallGame\BallGame\BallGameContent\";

        public override void LoadContent()
        {
            random = new Random();
            font1 = Content.Load<SpriteFont>("SpriteFont1");
            texBack = texFromFile(graphicsDevice, dir + "nextLevel.png");
            Particle2 = Util.texFromFile(graphicsDevice, Dir.dir + "Particle2.png");
            back1 = new ImageBackground(texBack, Color.White, graphicsDevice);
            tex = Particle2;
            setSys2();
        }

        public override void Update(GameTime gameTime)
        {
            ticks++;
            p.Update(gameTime);
            KeyboardState k = Keyboard.GetState();
 
                if (k.IsKeyDown(Keys.N))
                gameStateManager.setLevel(3);
        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.Chartreuse);
            spriteBatch.Begin();
           
            back1.Draw(spriteBatch);
            if (p != null)
                p.Draw(spriteBatch);
            if (wl != null) { wl.Draw(spriteBatch, Color.Teal, Color.Red); }
            spriteBatch.End();
        }
        public static Texture2D texFromFile(GraphicsDevice gd, string fName)
        {
            Stream fs = new FileStream(fName, FileMode.Open);
            Texture2D re = Texture2D.FromStream(gd, fs);
            fs.Close();
            return re;
        }
        void setSys2()
        {
            p = new ParticleSystem(new Vector2(300, 100), 40, 900, 107);
            p.setMandatory1(tex, new Vector2(6, 6), new Vector2(24, 24), Color.White, new Color(255, 255, 255, 100));
            p.setMandatory2(-1, 1, 1, 3, 0);
           // rectangle = new Rectangle(0, 0, 800, 600);
            p.setMandatory3(120, rectangle);
            p.setMandatory4(new Vector2(0, 0.1f), new Vector2(0, 0), new Vector2(1, 0));
            p.randomDelta = new Vector2(0.1f, 0.1f);
            p.Origin = 1;
            p.originRectangle = new Rectangle(0, 0, 800, 10);
            p.moveTowards = 3;
            p.moveTowardsPos = new Vector2(mouse_x, mouse_y);
            p.moveToDrift = 0.1f;
            p.activate();
            wl = null;
        }
    }

    // -------------------------------------------------------- Game level 5 ----------------------------------------------------------------------------------

    class GameLevel_5 : RC_GameStateParent
    {
        ImageBackground back1 = null;
        Texture2D texBack;
        string dir = @"C:\uc\Game Programming (7191)\MT2\BallGame\BallGame\BallGame\BallGameContent\";

        public override void LoadContent()
        {
            font1 = Content.Load<SpriteFont>("SpriteFont1");
            texBack = texFromFile(graphicsDevice, dir + "gameOver.png");
            back1 = new ImageBackground(texBack, Color.White, graphicsDevice);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState k = Keyboard.GetState();

            if (k.IsKeyDown(Keys.R))
                gameStateManager.setLevel(3);
        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.Chartreuse);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            back1.Draw(spriteBatch);
            spriteBatch.DrawString(font1, "Press r to restart", new Vector2(100, 80), Color.White);
            spriteBatch.End();
        }
        public static Texture2D texFromFile(GraphicsDevice gd, string fName)
        {
            Stream fs = new FileStream(fName, FileMode.Open);
            Texture2D re = Texture2D.FromStream(gd, fs);
            fs.Close();
            return re;
        }
    }

    // -------------------------------------------------------- Game level 6 (help) ----------------------------------------------------------------------------------

    class GameLevel_6 : RC_GameStateParent
    {
        ImageBackground back1 = null;
        Texture2D texBack;
        Scrolling scrolling1;
        Scrolling scrolling2;

        string dir = @"C:\uc\Game Programming (7191)\MT2\BallGame\BallGame\BallGame\BallGameContent\";


        public override void LoadContent()
        {
            scrolling1 = new Scrolling(Content.Load<Texture2D>("skyMenu"), new Rectangle(0, 0, 800, 600));
            scrolling2 = new Scrolling(Content.Load<Texture2D>("skyMenu"), new Rectangle(800, 0, 800, 600));
            texBack = texFromFile(graphicsDevice, dir + "help.png");
            back1 = new ImageBackground(texBack, Color.White, graphicsDevice);

        }

        public override void Update(GameTime gameTime)
        {
            prevKeyState = keyState;
            keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Space) && !prevKeyState.IsKeyDown(Keys.Space))
            {
                gameStateManager.setLevel(1);
            }
            if (scrolling1.rectangle.X + scrolling1.texture.Width <= 0)
                scrolling1.rectangle.X = scrolling2.rectangle.X + scrolling2.texture.Width;
            if (scrolling2.rectangle.X + scrolling2.texture.Width <= 0)
                scrolling2.rectangle.X = scrolling1.rectangle.X + scrolling1.texture.Width;

            scrolling1.Update();
            scrolling2.Update();
        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.Aqua);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            scrolling1.Draw(spriteBatch);
            scrolling2.Draw(spriteBatch);
            back1.Draw(spriteBatch);
            spriteBatch.End();
        }
        public static Texture2D texFromFile(GraphicsDevice gd, string fName)
        {
            Stream fs = new FileStream(fName, FileMode.Open);
            Texture2D re = Texture2D.FromStream(gd, fs);
            fs.Close();
            return re;
        }
    }
}
