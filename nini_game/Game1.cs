
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace nini_game
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MoveableObject a;
        AiEnemy Enemy;
        AiEnemy Enemy2;
        AiEnemy Enemy3;
        AiEnemy Enemy4;
        AiEnemy Enemy5;

        Coin C1;
        Coin C2;
        Coin C3;
        Coin C4;
        Coin C5;

        CanBeDrawn BackG;
        Texture2D StartScreen;
        Texture2D GameOver;
        Texture2D VictoryScreen;

        FrameOfView cam;
       


        //The two Events in charge of calling Draw and Update methods.
        public static event DlgUpdate UpdateThis;
        public static event DlgDraw DrawThis;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
       
        protected override void Initialize()
        {
            //Sets the size of the Screen
            graphics.PreferredBackBufferHeight = 1600;
            graphics.PreferredBackBufferWidth = 3000;
            
            graphics.ApplyChanges();
            Window.Title = "NINI'S AWESOME GAME!";

            base.Initialize();
        }
       
        protected override void LoadContent()
        {
         
            //Loading all the different Textures into The Objects.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //The "activation" of the G class.
            G.Init(Content, spriteBatch, graphics);
            //Loading the G class with the mask Collision background
            G.map = new Map(G.cm.Load<Texture2D>("LvlMask"));


            StartScreen = G.cm.Load<Texture2D>("StartScreen");
            GameOver = G.cm.Load<Texture2D>("GameOver");
            VictoryScreen = G.cm.Load<Texture2D>("VictoryScreen");

            BackG = new CanBeDrawn(G.cm.Load<Texture2D>("GameLvl"), Vector2.Zero, null, Color.White,
                0f, Vector2.Zero, G.Scale, SpriteEffects.None, 0f);



            a = new MoveableObject(new ControlsPlayer(Keys.A, Keys.D, Keys.W, Keys.S)
                ,Character.Hero, State.Stand, new Vector2(220 * G.Scale, 200 * G.Scale)
                 , Color.White, 0f,
                  G.Scale - 2.5f, SpriteEffects.None, 1f);

            //Enemy 1 location.X = 1500


            Enemy = new AiEnemy(a ,new ControlsAi(false, true, false, false), Character.Enemy, State.Walk,
                new Vector2(300 * G.Scale, 286 * G.Scale)
                 , Color.White, 0f,
                  G.Scale - 3.4f, SpriteEffects.None, 1f);

            Enemy2 = new AiEnemy(a, new ControlsAi(false, true, false, false), Character.Enemy, State.Walk,
                new Vector2(1600 * G.Scale, 286 * G.Scale),
                Color.White, 0f, G.Scale - 3.4f, SpriteEffects.None, 1f);

            Enemy3 = new AiEnemy(a, new ControlsAi(false, true, false, false), Character.Enemy, State.Walk,
                new Vector2(1860 * G.Scale, 286 * G.Scale),
                Color.White, 0f, G.Scale - 3.4f, SpriteEffects.None, 1f);

            Enemy4 = new AiEnemy(a, new ControlsAi(false, true, false, false), Character.Enemy, State.Walk,
                new Vector2(2100 * G.Scale, 286 * G.Scale),
                Color.White, 0f, G.Scale - 3.4f, SpriteEffects.None, 1f);

            Enemy5 = new AiEnemy(a, new ControlsAi(false, true, false, false), Character.Enemy, State.Walk,
                new Vector2(2650 * G.Scale, 286 * G.Scale),
                Color.White, 0f, G.Scale - 3.4f, SpriteEffects.None, 1f);

            C1 = new Coin(a, Character.Coin, State.Stand,
                new Vector2(685 * G.Scale, 200 * G.Scale),
                Color.White, 0f, G.Scale, SpriteEffects.None, 1f);

            C2 = new Coin(a, Character.Coin, State.Stand,
                new Vector2(1438 * G.Scale, 216 * G.Scale),
                Color.White, 0f, G.Scale, SpriteEffects.None, 1f);

            C3 = new Coin(a, Character.Coin, State.Stand,
                new Vector2(1884 * G.Scale, 264 * G.Scale),
                Color.White, 0f, G.Scale, SpriteEffects.None, 1f);

            C4 = new Coin(a, Character.Coin, State.Stand,
                new Vector2(2518 * G.Scale, 142 * G.Scale),
                Color.White, 0f, G.Scale, SpriteEffects.None, 1f);

            C5 = new Coin(a, Character.Coin, State.Stand,
                new Vector2(3300 * G.Scale, 224 * G.Scale),
                Color.White, 0f, G.Scale, SpriteEffects.None, 1f);


            //Crating the Camera that will follow an Object.
            cam = new FrameOfView(a, new Viewport(0, 0, 2000, 1600));

            
            

        } 
     
        protected override void UnloadContent()
        {
            
        }
       
        protected override void Update(GameTime gameTime)
        {
            //Depending on G.GameState Value, different parts of the code will be activated.
            //This happes both on the Update Function and on the Draw function in Game1
            if (G.GameState == "GameBegin")
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    G.GameState = "GameOngoing";
                }
            }

            if (G.GameState == "GameOngoing")
            {
                //Checking to see if any methods are signed up to the Update Event.
                if (UpdateThis != null)
                {
                    //Invoking the Event. 
                    UpdateThis();
                }
            }

            if (G.GameState == "GameEndedL" ||
                G.GameState == "GameEndedW")
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    G.GameState = "GameOngoing";
                    Restart();
                    
                }
            }
           

            base.Update(gameTime);
        }
  
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (G.GameState == "GameBegin")
            {
                spriteBatch.Begin();

                G.sb.Draw(StartScreen,
                    new Rectangle(0, 0, StartScreen.Width, StartScreen.Height)
                    ,Color.White);

                spriteBatch.End();
            }

            if (G.GameState == "GameOngoing")
            {
                //Begining to Draw a thing, telling the screen to apply the
                //cam.Mat Matrix in order to follow the Hero.
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, cam.Mat);

                //Checking to see if any methods are signed up to the Update Event.
                if (DrawThis != null)
                {
                    //Invoking the Event. 
                    DrawThis();
                }

                spriteBatch.End();
            }

            if (G.GameState == "GameEndedL" ||
                G.GameState == "GameEndedW")
            {
                spriteBatch.Begin();
                if (G.GameState == "GameEndedL")
                {
                    G.sb.Draw(GameOver,
                   new Rectangle(0, 0, GameOver.Width, GameOver.Height),
                   Color.White);
                }

                if (G.GameState == "GameEndedW")
                {
                    G.sb.Draw(VictoryScreen,
                        new Rectangle(0, 0, VictoryScreen.Width, VictoryScreen.Height),
                        Color.White);
                }
               

                spriteBatch.End();
            }
            
           

            base.Draw(gameTime);
        }
        /// <summary>
        /// This Function Restarts the game if the player has won or lost the game
        /// without needing to exit and run the whole code again.
        /// </summary>
        public void Restart()
        {
            a.Position = new Vector2(220 * G.Scale, 200 * G.Scale);
            a.LifePoints = 50;

            Enemy.MovingSpeed = 0.99f;
            Enemy2.MovingSpeed = 0.99f;
            Enemy3.MovingSpeed = 0.99f;
            Enemy4.MovingSpeed = 0.99f;
            Enemy5.MovingSpeed = 0.99f;


            for (int i = 0; i < G.CoinQueue.Count; i++)
            {
                Game1.DrawThis += G.CoinQueue.Peek().DrawThis;
                G.CoinQueue.Enqueue(G.CoinQueue.Dequeue());

            }
            
        }
    }
}
