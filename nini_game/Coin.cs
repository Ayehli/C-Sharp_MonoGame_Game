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
    class Coin : CanBeAnimated
    {
        MoveableObject Player;
        
       
        public Coin(MoveableObject player, Character c, State s, Vector2 position, Color color,
            float rotation, float scale, SpriteEffects effects, float layerDepth)

         :   base (c, s, position, color, rotation, scale, effects, layerDepth)
        {
            this.Player = player;
            G.CoinQueue.Enqueue(this);
        }

        public override void UpdateThis()
        {

            if (G.map.IsPlayerClose(base.Position, Player.Position))
            {
                Game1.DrawThis -= DrawThis;
            }

            base.UpdateThis();
        }
    }
}
