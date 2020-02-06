using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace nini_game
{
    class CanBeDrawn
    {
        //The diffrent requirements in order to draw a texture.
        protected Texture2D texture;
        public Vector2 Position { get; set; }
        protected Rectangle? usedRectangle;
        Color color;
        float rotation;
        protected Vector2 origin;
        public float scale;
        public SpriteEffects Effects { get; set; }
        float layerDepth;
        /// <summary>
        /// The constructor for an object that can be drawn.
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="position">o</param>
        /// <param name="usedRectangle">A spacific rectangle to draw</param>
        /// <param name="color"></param>
        /// <param name="rotation"></param>
        /// <param name="origin">were the computer will treat the object</param>
        /// <param name="scale"></param>
        /// <param name="effects"></param>
        /// <param name="layerDepth">on what layer will the textur be drawn</param>
        public CanBeDrawn(Texture2D texture, Vector2 position, Rectangle? usedRectangle, Color color,
            float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            this.texture = texture;
            this.Position = position;
            this.usedRectangle = usedRectangle;
            this.color = color;
            this.rotation = rotation;
            this.origin = origin;
            this.scale = scale;
            this.Effects = effects;
            this.layerDepth = layerDepth;
            //Signs itself to the Draw Event in order to be Drawn.
            Game1.DrawThis += DrawThis;

        }
        //Another constructor with lesser requirements. used for the background.
        public CanBeDrawn(Vector2 position, Color color,
            float rotation, float scale, SpriteEffects effects, float layerDepth)
        {
            this.Position = position;
            this.color = color;
            this.rotation = rotation;
            this.scale = scale;
            this.Effects = effects;
            this.layerDepth = layerDepth;
            //Signs itself to the Draw Event in order to be Drawn.
            Game1.DrawThis += DrawThis;
        }
        //The method that draws the object.
        public void DrawThis()
        {
            G.sb.Draw(texture, Position, usedRectangle, color, rotation, origin, scale, Effects, layerDepth);
            
        }
    }
}