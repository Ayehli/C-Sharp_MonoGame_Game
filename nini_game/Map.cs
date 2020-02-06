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
    enum BgType { wall, other, death, victory }
    
    //This class helps build and mediate the Mask Map that helps players know where they are
    //And if They are near a wall.
    class Map
    {
        //A two Dimensional array that will store the "nature" of every pixel on the map.
        
        public BgType[,] Ground { get; set; }
        /// <summary>
        /// Building and filling the Array.
        /// </summary>
        /// <param name="mask">The Map Behind the map.</param>
        public Map(Texture2D mask)
        {
            //Creating a one dimensional Color array to store all the pixels  
            Color[] c = new Color[mask.Width * mask.Height];
            mask.GetData<Color>(c);

            Ground = new BgType[mask.Width, mask.Height];
            //This code segment helps convert a pexter from a one dimentional array
            //To a two Dimensional Array, and place it in the right place.
             
            for (int y = 0; y < mask.Height; y++)
            {
                for (int x = 0; x < mask.Width; x++)
                {
                    //By defult the pixel is not passable.
                    Ground[x, y] = BgType.wall;

                    //if this is true, the pixel is green and is passable.
                    if (c[(y * mask.Width) + x] == c[0])
                    {
                        Ground[x, y] = BgType.other;
                    }

                    //is this is true, the the pixel is black and is not passable.
                    if (c[(y * mask.Width) + x] == c[1])
                    {
                        Ground[x, y] = BgType.wall;
                    }

                    if (c[(y * mask.Width) + x] == c[2])
                    {
                        Ground[x, y] = BgType.death;
                    }

                    if (c[(y * mask.Width) + x] == c[3])
                    {
                        Ground[x, y] = BgType.victory;
                    }
                }

            }

        }

        /// <summary>
        /// Calculates the vector of the caller from an unpassable pixel.
        /// </summary>
        /// <param name="pos">caller position</param>
        /// <param name="lookDrc">The direction the method will check in</param>
        /// <returns></returns>
        public Vector2 Ray_to_wall(Vector2 pos, Vector2 lookDrc)
        {
            Vector2 tmpRay = Vector2.Zero;

            int tmpX = (int)pos.X;
            int tmpY = (int)pos.Y;

            while (G.map.Ground[(int)(tmpX / G.Scale),
                (int)(tmpY / G.Scale)] != BgType.wall)
            {
                tmpRay += lookDrc;
                tmpX = (int)(pos.X + tmpRay.X);
                tmpY = (int)(pos.Y + tmpRay.Y);
            }

            return tmpRay + pos;
        }
        /// <summary>
        /// return true if a vector to a wall is less then a certin number.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="rayToWall">the vector to the wall.</param>
        /// <returns></returns>
        public bool IsWallClose(Vector2 pos, Vector2 rayToWall)
        {
            if ((rayToWall - pos).Length() < 30)
            {
                return true;
            }

            return false;
        }

        
        
        /// <summary>
        /// return true if the vector to the player is less then a certin number.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="rayToPlayer">Vector to Player.</param>
        /// <returns></returns>
        public bool IsPlayerClose(Vector2 ObjPos, Vector2 PlayerPos, out String PLeftOrRight)
        {
            if (PlayerPos.Length() > ObjPos.Length())
            {
                PLeftOrRight = "right";
            }

            else
            {
                PLeftOrRight = "left";
            }

            if ((ObjPos - PlayerPos).Length() < 500)
            {
                return true;
            }

            return false;
        }

        public bool IsPlayerClose(Vector2 ObjPos, Vector2 PlayerPos)
        {
           
            if ((ObjPos - PlayerPos).Length() < 140)
            {
                return true;
            }

            return false;
        }

    }
}
