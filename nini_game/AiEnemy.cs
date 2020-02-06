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
    
    //This Class represents the AI enemy.
    class AiEnemy : CanBeAnimated
    {
        ControlsBasic Keys;
        MoveableObject Player;
        String PLeftOrRight;
        

        Vector2 Ray2Wall;
        Vector2 lookLeft = new Vector2(-1, 0);
        Vector2 lookRight = new Vector2(1, 0);

        /// <summary>
        /// The constructor for the Enemy. in addition to all the nececetys for a frame,
        /// It Tekes the Playe, So It Can Track Its Movment.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="keys">A st of Keys so that the Ai can control the Enemy. </param>
        /// <param name="c"></param>
        /// <param name="s"></param>
        /// <param name="position"></param>
        /// <param name="color"></param>
        /// <param name="rotation"></param>
        /// <param name="scale"></param>
        /// <param name="effects"></param>
        /// <param name="layerDepth"></param>
        public AiEnemy(MoveableObject player ,ControlsAi keys, Character c, State s, Vector2 position, Color color,
            float rotation, float scale, SpriteEffects effects, float layerDepth) 

          :  base(c, s, position, color, rotation, scale, effects, layerDepth)
        {
            this.Keys = keys;
            this.Player = player;
        }

        public override void UpdateThis()
        {
            PLeftOrRight = "null";



            if (base.s != State.Damaged)
            {
                // If the bot is moving left
                if (Keys.IsLeftPressed())
                {
                    Keys.SetAttack(false);
                    base.s = State.Walk;
                    base.texture = G.MegaDictionary[c][s].Tex;

                    if (base.MovingSpeed == 0)
                    {
                        base.s = State.Stand;
                        base.texture = G.MegaDictionary[c][s].Tex;
                    }

                    //Cheks the distance to the wall.
                    Ray2Wall = G.map.Ray_to_wall(Position, lookLeft);


                    //if its too close, turns around.
                    if (G.map.IsWallClose(Position, Ray2Wall))
                    {
                        Keys.SetLeft(false);
                        Keys.SetRight(true);

                        Effects = SpriteEffects.None;
                    }

                    else
                    {
                        Position += new Vector2(-10 * base.MovingSpeed, 0);
                    }

                }

                //If the bot is moving right
                if (Keys.IsRightPressed())
                {
                    Keys.SetAttack(false);
                    base.s = State.Walk;
                    base.texture = G.MegaDictionary[c][s].Tex;
                    if (base.MovingSpeed == 0)
                    {
                        base.s = State.Stand;
                        base.texture = G.MegaDictionary[c][s].Tex;
                    }

                    //Cheks the distance to the wall.
                    Ray2Wall = G.map.Ray_to_wall(Position, lookRight);

                    //if its too close, turns around.
                    if (G.map.IsWallClose(Position, Ray2Wall))
                    {
                        Keys.SetLeft(true);
                        Keys.SetRight(false);

                        Effects = SpriteEffects.FlipHorizontally;
                    }

                    else
                    {
                        Position += new Vector2(10 * base.MovingSpeed, 0);
                    }

                }



                //Checks if the player is Close and if so from whitch direction.
                //will later be used to detirmne if the Enemy needs to attack.
                if ((G.map.IsPlayerClose(Position, Player.Position, out PLeftOrRight) &&
                  Keys.IsRightPressed() && PLeftOrRight == "right") ||

                  (G.map.IsPlayerClose(Position, Player.Position, out PLeftOrRight) &&
                  Keys.IsLeftPressed() && PLeftOrRight == "left"))
                {
                    Keys.SetAttack(true);
                    Keys.SetLeft(false);
                    Keys.SetRight(false);
                }

                else
                {
                    if (Effects == SpriteEffects.None)
                    {
                        Keys.SetRight(true);

                    }

                    if (Effects == SpriteEffects.FlipHorizontally)
                    {
                        Keys.SetLeft(true);

                    }


                }


                if (Keys.IsAttackPressed())
                {
                    base.s = State.Attack;
                    base.texture = G.MegaDictionary[c][s].Tex;

                }
            }
            



            base.UpdateThis();
        }

        

    }
}
