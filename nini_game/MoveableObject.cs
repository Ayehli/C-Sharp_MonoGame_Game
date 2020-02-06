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
    //The class that represents Gravity fot the Objects.
    class PhysicsEngine
    {
        static float GravityConstant = 5f;
        public float JumpConstant = 50f;
        Vector2 Gravity;

        public PhysicsEngine()
        {
            this.Gravity = Vector2.Zero;
        }

        public Vector2 IncGrav()
        {
            //Adding more gravity force until a certin point. 
            this.Gravity += new Vector2(0, GravityConstant);
            if (Gravity.Y > 30f)
            {
                Gravity.Y = 30f;
            }
            return Gravity;
        }

        public Vector2 Jump(Vector2 pos)
        {
            JumpConstant -= 0.9f;
            if (JumpConstant < 0)
            {
                JumpConstant = 0;
            }

            pos -= new Vector2(0, JumpConstant);
            return pos;
        }

        public void SetGrav(Vector2 newG)
        {
            Gravity = newG;
        }
        
        public Vector2 GetGrav()
        {
            return Gravity;
        }

    }

    //The class represents an object that has an animation and can move.
    class MoveableObject : CanBeAnimated
    {
        ControlsBasic keys;
        PhysicsEngine E;
        bool IsJumping = false;
        bool isFalling = false;
        

        /// <summary>
        /// The MovableObject Constructor. only takes a set of Keys
        /// And gives the rest to the base constructor.
        /// </summary>
        /// <param name="keys">Four KeayBoard Keys.</param>
        /// <param name="c">Chracter</param>
        /// <param name="s">State</param>
        /// <param name="position"></param>
        /// <param name="color"></param>
        /// <param name="rotation"></param>
        /// <param name="scale"></param>
        /// <param name="effects"></param>
        /// <param name="layerDepth"></param>
        public MoveableObject(ControlsBasic keys, Character c, State s, Vector2 position, Color color,
            float rotation, float scale, SpriteEffects effects, float layerDepth)

            : base(c, s, position, color, rotation, scale, effects, layerDepth)
        {
            this.keys = keys;
            this.E = new PhysicsEngine  ();
            
        }

       
        

        public override void UpdateThis()
        {
            



            if (CirCollisionD() != "down")
            {
                E.IncGrav();
                Position += E.GetGrav();
                base.s = State.Jump;
                base.texture = G.MegaDictionary[c][s].Tex;
                isFalling = true;

            }

            else
            {
                E.SetGrav(Vector2.Zero);
                IsJumping = false;
                isFalling = false;
                E.JumpConstant = 50f;
            }



            if (base.s != State.Damaged)
            {

                if (keys.IsAttackPressed())
                {
                    base.s = State.Attack;
                    base.texture = G.MegaDictionary[c][s].Tex;

                }


                if (IsJumping == true)
                {
                    base.s = State.Jump;
                    base.texture = G.MegaDictionary[c][s].Tex;
                }

                if (CirCollisionU() != "up")
                {
                    if (keys.IsUpPressed() && IsJumping == false)
                    {
                        IsJumping = true;
                    }

                    if (IsJumping == true)
                    {
                        Position = E.Jump(Position);
                    }

                }



                //The following if Statments check wether there was collision from each side 
                //Respectively, and if so does not enter the movment is statment.
                //If there is no collision, Cheks if the Movment Key has been pressed,
                //And if so moves the player. 
                if (CirCollisionL() != "left")
                {
                    if (keys.IsLeftPressed())
                    {

                        Position += new Vector2(-11, 0);
                        if (IsJumping == false && isFalling == false)
                        {
                            base.s = State.Walk;
                            this.texture = G.MegaDictionary[c][s].Tex;

                        }
                        Effects = SpriteEffects.FlipHorizontally;
                    }


                }

                if (CirCollisionR() != "right")
                {
                    if (keys.IsRightPressed())
                    {
                        Position += new Vector2(11, 0);
                        if (IsJumping == false && isFalling == false)
                        {
                            base.s = State.Walk;
                            base.texture = G.MegaDictionary[c][s].Tex;

                        }
                        Effects = SpriteEffects.None;
                    }
                }

            }

            CirCollisionLW();

            base.UpdateThis();
            
        }

        //The Following Method Cheks if the player has won or Lost,
        //And if So Changes the G.GameState so that different pars of the Update and 
        //Draw Method will activate.
        public void CirCollisionLW()
        {
            if (base.stateIndex >= G.MegaDictionary[c][s].Origs.Count())
            {
                base.stateIndex = 0;
            }
            int NumOfCirc = G.MegaDictionary[c][s].Circuli[base.stateIndex].Count();


            for (int i = 0; i < NumOfCirc; i++)
            {
                float CurrentRad = G.MegaDictionary[c][s].Circuli[base.stateIndex][i].Radius;
                Vector2 CurrentCircleCenter = Position - G.MegaDictionary[c][s].Circuli[base.stateIndex][i].CircleCenter;

                Vector2 RCol = new Vector2((CurrentRad) + 80, 0);
                Vector2 DCol = new Vector2(0, (CurrentRad) + 10);

               

                if (G.map.Ground[
                     (int)((CurrentCircleCenter + DCol).X / G.Scale),
                     (int)((CurrentCircleCenter + DCol).Y / G.Scale)] == BgType.death ||
                     this.LifePoints <= 0)
                {
                    G.GameState = "GameEndedL";
                    
                }

                if (G.map.Ground[
                    (int)((CurrentCircleCenter + RCol).X / G.Scale),
                    (int)((CurrentCircleCenter + RCol).Y / G.Scale)] == BgType.victory ||

                    G.map.Ground[
                     (int)((CurrentCircleCenter + DCol).X / G.Scale),
                     (int)((CurrentCircleCenter + DCol).Y / G.Scale)] == BgType.victory
                    )
                {
                    G.GameState = "GameEndedW";
                    
                }
 
            }
        }


        //The Following three Methods eachCheck if the player has collided with a wall
        //From a Different Direction by using the circules it has as refrences for collision areas. 
        public String CirCollisionU()
        {
            if (base.stateIndex >= G.MegaDictionary[c][s].Origs.Count())
            {
                base.stateIndex = 0;
            }
            int NumOfCirc = G.MegaDictionary[c][s].Circuli[base.stateIndex].Count();
            
            //for each circle in the frame, the method checks if the circle has collided with a wall
            //Type from the BgType enum.
            for (int i = 0; i < NumOfCirc; i++)
            {
                float CurrentRad = G.MegaDictionary[c][s].Circuli[base.stateIndex][i].Radius;

                //Calculting the Current Circle Center with the Position of the player to get the
                //"real" place of the circle.
                Vector2 CurrentCircleCenter = Position - G.MegaDictionary[c][s].Circuli[base.stateIndex][i].CircleCenter;
                Vector2 UCol = new Vector2(0, -(CurrentRad) - 180);

                //if true, there is collision and the finction returns a string.
                if (G.map.Ground[
                   (int)((CurrentCircleCenter + UCol).X / G.Scale),
                   (int)((CurrentCircleCenter + UCol).Y / G.Scale)] == BgType.wall)
                {
                    return "up";
                }

            }

            //if not, return null (No Collision).
            return "null";
        }

        public String CirCollisionD()
        {
            if (base.stateIndex >= G.MegaDictionary[c][s].Origs.Count())
            {
                base.stateIndex = 0;
            }
            int NumOfCirc = G.MegaDictionary[c][s].Circuli[base.stateIndex].Count();

            for (int i = 0; i < NumOfCirc; i++)
            {
                float CurrentRad = G.MegaDictionary[c][s].Circuli[base.stateIndex][i].Radius;
                Vector2 CurrentCircleCenter = Position -
                    G.MegaDictionary[c][s].Circuli[base.stateIndex][i].CircleCenter;
                Vector2 DCol = new Vector2(0, (CurrentRad) + 15);

                if (G.map.Ground[
                   (int)((CurrentCircleCenter + DCol).X / G.Scale),
                   (int)((CurrentCircleCenter + DCol).Y / G.Scale)] == BgType.wall)
                {
                    return "down";
                }
            }
            return "null";
        }

        public String CirCollisionL()
        {
            if (base.stateIndex >= G.MegaDictionary[c][s].Origs.Count())
            {
                base.stateIndex = 0;
            }
            int NumOfCirc = G.MegaDictionary[c][s].Circuli[base.stateIndex].Count();

            for (int i = 0; i < NumOfCirc; i++)
            {
                float CurrentRad = G.MegaDictionary[c][s].Circuli[base.stateIndex][i].Radius;
                Vector2 CurrentCircleCenter = Position - G.MegaDictionary[c][s].Circuli[base.stateIndex][i].CircleCenter;
                Vector2 LCol = new Vector2(-(CurrentRad) - 50, 0);

                if (G.map.Ground[
                  (int)((CurrentCircleCenter + LCol).X / G.Scale),
                  (int)((CurrentCircleCenter + LCol).Y / G.Scale)] == BgType.wall ||

                  G.map.Ground[
                      (int)((Position.X - 50) / G.Scale),
                      (int)((Position.Y - 20) / G.Scale)] == BgType.wall)
                {
                    return "left";
                }
            }

            return "null";
        }

        public String CirCollisionR()
        {
            if (base.stateIndex >= G.MegaDictionary[c][s].Origs.Count())
            {
                base.stateIndex = 0;
            }
            int NumOfCirc = G.MegaDictionary[c][s].Circuli[base.stateIndex].Count();

            for (int i = 0; i < NumOfCirc; i++)
            {
                float CurrentRad = G.MegaDictionary[c][s].Circuli[base.stateIndex][i].Radius;
                Vector2 CurrentCircleCenter = Position - G.MegaDictionary[c][s].Circuli[base.stateIndex][i].CircleCenter;
                Vector2 RCol = new Vector2((CurrentRad) + 80, 0);

                if (G.map.Ground[
                    (int)((CurrentCircleCenter + RCol).X / G.Scale),
                    (int)((CurrentCircleCenter + RCol).Y / G.Scale)] == BgType.wall ||

                    G.map.Ground[
                        (int)((Position.X + 50) / G.Scale),
                        (int)((Position.Y - 20) / G.Scale)] == BgType.wall)
                {
                    return "right";
                }

            }

            return "null";
        }
    }
}
