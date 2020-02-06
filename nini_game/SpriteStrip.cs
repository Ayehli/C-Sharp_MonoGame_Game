using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;


namespace nini_game
{
    //This class creates the collision circles assigned to each
    //SpriteStrip.
    class CollisionCircle
    {
        public Vector2 CircleCenter { get; set; }
        public float Radius { get; set; }

        public CollisionCircle(Vector2 Ccenter, float r)
        {
            this.CircleCenter = Ccenter;
            this.Radius = r;
        }
    }



    //This class creates a SpriteStrip that has all the information needed to 
    //Draw a frame from a texture.
    class SpriteStrip
    {
        //The Texture that will be drawn in the end.
        public Texture2D Tex { get; private set; }

        //The list of rectangels that represent a Frame each on the texture.
        public List<Rectangle> Recs { get; private set; } = new List<Rectangle>();

        //A list of origins For the Draw method.
        public List<Vector2> Origs { get; private set; } = new List<Vector2>();

        //A texture That has the Information about creating the Collision circels,
        //And will not be drawn.
        public Texture2D CollisionMask { get; private set; }

        //The first list represents every Frame in a StateStrip,
        //The second one represents the amount of Collision circels each frame has.
        public List<List<CollisionCircle>> Circuli { get; private set; } = new List<List<CollisionCircle>>();


        /// <summary>
        /// This Method takes all the information to drawing a frame from 
        /// Dots on the Texture itself. black Dots On Tex To represent The
        /// Origin And Rectangle places, and the green spots on the
        /// CollisionMask represent the center of the circels and their radiuses.
        /// </summary>
        /// <param name="hero">A chracter from enum Character</param>
        /// <param name="state">A state from enum State</param>
        public SpriteStrip(Character hero, State state)
        {
            //Loading the Texture and Collision Texture respectivly.
            string path = Directory.GetCurrentDirectory();

            String TexPath = path + "/content/" + hero.ToString() + "/" + state.ToString() + ".xnb";
            if (File.Exists(TexPath))
            {
                Tex = G.cm.Load<Texture2D>(hero.ToString() + "/" + state.ToString());
            }

            String ColMaskPath = path + "/content/" + hero.ToString() + "Mask" + "/" +
                state.ToString() + "Mask" + ".xnb";
            if (File.Exists(ColMaskPath))
            {
                CollisionMask = G.cm.Load<Texture2D>(hero.ToString() + "Mask" + "/" +
                    state.ToString() + "Mask");
            }
            

            //Creats an array of color, with width of the tex.width

            Color[] Cstrip = new Color[Tex.Width]; 
            Tex.GetData<Color>(0, new Rectangle(0, Tex.Height - 1 , Tex.Width, 1), Cstrip, 0, Cstrip.Length);
            
            // Takes a rectangle from a texture and puts it into a color array (Cstrip) 
            List<int> Bpnts = new List<int>(); // a list of int points that contain the X position of the balck dots
            

           //Puts every black point in the arrray.
            for (int i = 0; i < Cstrip.Length; i++) 
            {
                if (Cstrip[i] != Cstrip[1])
                {
                    Bpnts.Add(i);
                }
            }


            // Creates the diffrent "frames" of the animation strip by skipping
            // Two at a time on the array of black point.
            for (int i = 0; i < Bpnts.Count - 2; i +=2 ) 
            {
                Rectangle r = new Rectangle(Bpnts[i], 0, Bpnts[i + 2] - Bpnts[i], Tex.Height - 1);
                Recs.Add(r);
            }
           
            //The method uses radCount to count the radius of the Current circle.
            //And tmpCulmn to skip over the next green Dot and not mistake it for another circle.
            int radCount = 0;
            int tmpCulmn;

            for (int i = 0; i < Recs.Count; i++)
            {
                // Creats an array of color, with width and height of the current rectangel
                Color[] tmpCS = new Color[Recs[i].Width * Recs[i].Height]; 
                CollisionMask.GetData<Color>(0, Recs[i], tmpCS, 0, tmpCS.Length);
                List<CollisionCircle> SemiCollisionCircles = new List<CollisionCircle>();


                for (int row = 0; row < Recs[i].Height; row++)
                {
                    radCount = 0;
                    tmpCulmn = 0;
                    for (int culmn = 1; culmn < Recs[i].Width; culmn++)
                    {
                        
                        if (tmpCS[(row * Recs[i].Width) + culmn + tmpCulmn] == tmpCS[0])
                        {
                            radCount = 0;
                            tmpCulmn = culmn + 1;
                            while (tmpCS[(row * Recs[i].Width) + tmpCulmn] != tmpCS[0])
                            {
                                tmpCulmn++;
                                radCount++;
                            }
                            //Creating the Circle.
                            CollisionCircle c = new CollisionCircle(new Vector2(culmn, row), (float)radCount);
                            SemiCollisionCircles.Add(c);
                            
                        }
                    }

                }

                Circuli.Add(SemiCollisionCircles);
            }



            // Creates the diffrent "origins" of each frame.
            for (int i = 1; i < Bpnts.Count - 1; i += 2) 
            {
                Vector2 v = new Vector2(Bpnts[i] - Bpnts[i - 1], Tex.Height - 2);
                Origs.Add(v);
            }
           

            MakeBgTrans();
        }

        void MakeBgTrans()
        {
            //This Method Tekes the whole Texture and turns the background color into clear pixels.
            Color[] c = new Color[Tex.Width * Tex.Height];
            Tex.GetData<Color>(c);
            Color trans = c[0];
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == trans)
                {
                    c[i] = Color.Transparent;
                }
            }
            
            //Putting back the clear version of the texture into the Texture.
            Tex.SetData<Color>(c);
        }
    }
}
