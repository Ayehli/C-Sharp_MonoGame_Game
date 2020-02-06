using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace nini_game
{
    //The two delegates used in the Event on Game1.
    public delegate void DlgUpdate();
    public delegate void DlgDraw();
    
    
    //List of Characters.
    enum Character { Hero, Enemy, Coin };
    //List of States. each valuce affects the time the animatin takes to complete.   
    enum State { Stand = 5, Walk = 4, Jump = 3, Attack = 6, Damaged = 20, Death = 12 };
    static class G
    {

        //Static class meant as a tool box for things all the other classes need.
        public static ContentManager cm;
        public static SpriteBatch sb;
        public static GraphicsDeviceManager gdm;
        public static Map map;
        public static float Scale = 7f;
        public static String GameState = "";
        public static Queue<CanBeAnimated> ColBetweenObj = new Queue<CanBeAnimated>();
        public static Queue<Coin> CoinQueue = new Queue<Coin>();

        // the dictionary that allows us to acces every different animation that a Character has.
        public static Dictionary<Character, Dictionary<State, SpriteStrip>> MegaDictionary = 
            new Dictionary<Character, Dictionary<State, SpriteStrip>>();
        /// <summary>
        /// This finction assignes the toolbox paraments into G, and builds the dictionary.
        /// </summary>
        /// <param name="cm">Used in order to load textures into the project</param>
        /// <param name="sb">Used to draw a Texture</param>
        /// <param name="gdm">allows acces to the screen.</param>
        public static void Init(ContentManager cm, SpriteBatch sb, GraphicsDeviceManager gdm)
        {
            G.cm = cm;
            G.sb = sb;
            G.gdm = gdm;
            G.GameState = "GameBegin";
          
            buildDic();
        }
        //The Method that builds the Dictionary.
        static void buildDic()
        {
            //Goes through every Chracter in enun Character.
            foreach(Character c in Enum.GetValues (typeof(Character)))
            {
                //Makes the little Dictionary that is put into the Mega Dictionary.
                Dictionary<State, SpriteStrip> stateDic = new Dictionary<State, SpriteStrip>();
                //Goes through every State in enun State.
                foreach (State s in Enum.GetValues(typeof(State)))
                {
                    
                    string path = Directory.GetCurrentDirectory();
                    path += "/content/" + c.ToString() + "/" + s.ToString() + ".xnb";
                    //Cheks if there is a path in the Content File.
                    if (File.Exists(path))
                    {
                        stateDic.Add(s, new SpriteStrip(c, s));
                    }
                    
                }
                //Adding the finished miniDictionary into the MegaDictionary.
                MegaDictionary.Add(c, stateDic);
            }
        }

        

    }
}
