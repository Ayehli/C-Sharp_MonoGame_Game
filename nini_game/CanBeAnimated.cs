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
    class CanBeAnimated : CanBeDrawn
    {
        protected Character c;
        protected State s;
        protected int stateIndex;
        int delay;
        public int LifePoints = 100;
        public float MovingSpeed = 0.99f;


        /// <summary>
        /// the Constructor for an object that has an animation.
        /// </summary>
        /// <param name="c">a Chracter form the enum list in G class</param>
        /// <param name="s">a state form the enum list in G class</param>
        /// <param name="position"></param>
        /// <param name="color"></param>
        /// <param name="rotation"></param>
        /// <param name="scale"></param>
        /// <param name="effects"></param>
        /// <param name="layerDepth"></param>
        public CanBeAnimated(Character c, State s, Vector2 position, Color color,
            float rotation, float scale, SpriteEffects effects, float layerDepth) 
            //All the other information travels back to the base constructor (CanBeDrawn).
            :base(position, color,
             rotation, scale, effects, layerDepth)
        {
            //The constructor uses the MegaDictionary built in the G class to attribute each 
            //texture to its corresponding one.
            this.texture = G.MegaDictionary[c][s].Tex;
            this.usedRectangle = G.MegaDictionary[c][s].Recs[stateIndex];
            this.origin = G.MegaDictionary[c][s].Origs[stateIndex];
            this.c = c;
            this.s = s;
            
            //signs itself to the update Evnet in order to be Updated.
            Game1.UpdateThis += UpdateThis;

            G.ColBetweenObj.Enqueue(this);
        }

        public virtual void UpdateThis()
        {
            /*The function That Detirmins if the player and an Object have collided.
            //It does so by calculating for each object in The G.ColBetweenObj Queue if the distance
            //Between the center of two of their circuls is less then the sum of their Rediuses.
            //If so, it means their is a collision and that something needs to happen.
            //At the End The function returns all of the Objects to the Queue and puts the player on top
            So It will be removed first.
            */
            if (G.ColBetweenObj.Count > 0 && G.ColBetweenObj.Peek().s != State.Damaged)
            {
               

                Queue<CanBeAnimated> tmpObjColQue = new Queue<CanBeAnimated>();
                CanBeAnimated player = G.ColBetweenObj.Dequeue();
               

                int NumOfCircP = G.MegaDictionary[player.c][player.s]
                    .Circuli[player.stateIndex].Count;
                int numOfSuspectObj = G.ColBetweenObj.Count;

                for (int i = 0; i < numOfSuspectObj ; i++)
                {
                    CanBeAnimated SuspectColObj = G.ColBetweenObj.Peek();
                    tmpObjColQue.Enqueue(G.ColBetweenObj.Dequeue());

                    //Making Sure the StateIndex does not go Out of Range for the Current State.
                    if (SuspectColObj.stateIndex >= G.MegaDictionary[SuspectColObj.c][SuspectColObj.s]
                        .Origs.Count())
                    {
                        SuspectColObj.stateIndex = 0;
                    }


                    int NumOfCircObj = G.MegaDictionary[SuspectColObj.c][SuspectColObj.s]
                        .Circuli[SuspectColObj.stateIndex].Count();

                    for (int z = 0; z < NumOfCircP; z++)
                    {
                        //If the Player is already Damaged, the function ends.
                        if (player.s != State.Damaged)
                        {
                            Vector2 CurrentCircCenterP = player.Position -
                            G.MegaDictionary[player.c][player.s]
                            .Circuli[player.stateIndex][z].CircleCenter;

                            float RadP = G.MegaDictionary[player.c][player.s]
                                .Circuli[player.stateIndex][z].Radius;

                            for (int q = 0; q < NumOfCircObj; q++)
                            {
                                
                                Vector2 CurrentCircCenterObj = SuspectColObj.Position -
                                    G.MegaDictionary[SuspectColObj.c][SuspectColObj.s]
                                    .Circuli[SuspectColObj.stateIndex][q].CircleCenter;

                                float RadObj = G.MegaDictionary[SuspectColObj.c][SuspectColObj.s]
                                .Circuli[SuspectColObj.stateIndex][q].Radius;

                                if ((CurrentCircCenterP - CurrentCircCenterObj).Length() - 100 <
                                    (RadObj + RadP))
                                {
                                    if (SuspectColObj.GetType() == typeof(AiEnemy))
                                    {
                                        if (player.s == State.Attack)
                                        {
                                           
                                            SuspectColObj.s = State.Damaged;
                                            SuspectColObj.MovingSpeed -= 0.001f;
                                            if (SuspectColObj.MovingSpeed < 0)
                                            {
                                                SuspectColObj.MovingSpeed = 0;
                                            }
                                            
                                        }

                                        if (SuspectColObj.s == State.Attack)
                                        {
                                            player.LifePoints--;
                                            player.s = State.Damaged;
                                        }
                                    }
                                    
                                    
                                    

                                    
                                    //the Action that will Happen if their is a Collision.
                                   
                                    
                                }
                            }
                        }

                        else
                        {
                            break;
                        }
                       


                    }

                }


                //putting back all the Objects in the G Queue, 
                //Starting with the Player.
                G.ColBetweenObj.Enqueue(player);

                numOfSuspectObj = tmpObjColQue.Count;

                for (int i = 0; i < numOfSuspectObj; i++)
                {
                    G.ColBetweenObj.Enqueue(tmpObjColQue.Dequeue());
                }


            }

            //This Update makes sure that thw stateIndex does not exceed the rectanglr capacity
            //The delay in the frame rotations come from the value assigned to every State in G class.

            if ((++delay) % (int)s == 0)
            {
                stateIndex++;
                stateIndex %= G.MegaDictionary[c][s].Origs.Count;
            }

            if (stateIndex >= G.MegaDictionary[c][s].Origs.Count)
            {
                stateIndex = 0;
            }

            this.texture = G.MegaDictionary[c][s].Tex;
            base.usedRectangle = G.MegaDictionary[c][s].Recs[stateIndex];
            base.origin = G.MegaDictionary[c][s].Origs[stateIndex];
            
            //makes sure that the object always retuen to the standing animation if not moving.
            this.s = State.Stand;


        }
    }
}
