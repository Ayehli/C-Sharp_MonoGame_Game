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
    // The class that gives us a camera to focus on a spacific object.
    class FrameOfView
    {


        public Matrix Mat { get; private set; }
        MoveableObject hero;
        public float Zoom { get; private set; }
        Viewport vp;
        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="hero">the object ypu want to follow</param>
        /// <param name="vp">the size of the window you want to look through</param>
        public FrameOfView(MoveableObject hero, Viewport vp)
        {
            this.hero = hero;
            this.vp = vp;
            Zoom = 0.8f;
            Game1.UpdateThis += UpdateThis;
        }

       

        void UpdateThis()
        {
            //Via different Matrix multiplication, you create a matrix that
            // Follows the object wherever he goes.
            Mat = Matrix.CreateTranslation(-hero.Position.X, -hero.Position.Y, 0) *
                  Matrix.CreateRotationZ(0) *
                  Matrix.CreateScale(Zoom) *
                  Matrix.CreateTranslation(vp.Width / 2, vp.Height / 2, 0);
        }

       

        

    }
}
