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
    //The Abstract Class that dictats how the movment classes are bult.
    abstract class ControlsBasic
    {
        public abstract bool IsLeftPressed();
        public abstract bool IsRightPressed();
        public abstract bool IsUpPressed();
        public abstract bool IsAttackPressed();
        public abstract void SetLeft(bool b);
        public abstract void SetRight(bool b);
        public abstract void SetUp(bool b);
        public abstract void SetAttack(bool b);
    }

    class ControlsPlayer : ControlsBasic
    {
        Keys left, right, up, attack;
        //A human player has four KeyBoard Keys.
        public ControlsPlayer(Keys left, Keys right, Keys up, Keys attack)
        {
            this.left = left;
            this.right = right;
            this.up = up;
            this.attack = attack;
        }
        public override bool IsLeftPressed()
        {
            return (Keyboard.GetState().IsKeyDown(left));
        }
        public override bool IsRightPressed()
        {
            return (Keyboard.GetState().IsKeyDown(right));
        }
        public override bool IsUpPressed()
        {
            return (Keyboard.GetState().IsKeyDown(up));
        }
        public override bool IsAttackPressed()
        {
            return (Keyboard.GetState().IsKeyDown(attack));
        }
        public override void SetLeft(bool b)
        {
           
        }
        public override void SetRight(bool b)
        {

        }
        public override void SetUp(bool b)
        {

        }
        public override void SetAttack(bool b)
        {

        }
    }

    class ControlsAi : ControlsBasic
    {
        bool left {  get; set; }
        bool right { get; set; }
        bool up { get; set; }
        bool attack { get; set; }
        //A computer Player has four bool Keys to represent pressing and un- pressing.
        public ControlsAi(bool left, bool right, bool up, bool attack)
        {
            this.left = left;
            this.right = right;
            this.up = up;
            this.attack = attack;
        }

        public override bool IsLeftPressed()
        {
            return left;
        }
        public override bool IsRightPressed()
        {
            return right;
        }
        public override bool IsUpPressed()
        {
            return up;
        }
        public override bool IsAttackPressed()
        {
            return attack;
        }

        public override void SetLeft(bool B)
        {
            left = B;
        }
        public override void SetRight(bool B)
        {
            right = B;
        }
        public override void SetUp(bool B)
        {
            up = B;
        }
        public override void SetAttack(bool B)
        {
            attack = B;
        }
    }
}

