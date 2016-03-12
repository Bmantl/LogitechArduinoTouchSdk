using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serial
{
    public delegate void KeyEventHandler(Object sender, KeyEventArgs e);

    public class KeyEventArgs : EventArgs
    {
        public Key.KeyState keyState { get; set; }
    }

    public class Key
    {
        public bool isChanging;
        public enum KeyState
        {
            Idle,
            Down,
            Up
        }

        public KeyState keyState { get; private set; }

        public virtual void CancelAll()
        {

        }

        public void Touch(KeyState keyState)
        {
            if (isChanging)
            {
                CancelAll();        
            }
            isChanging = true;
            if(this.keyState == KeyState.Idle && keyState == KeyState.Down)
            {            
                this.keyState = keyState;
                if (onTouchBegan != null)
                {
                    onTouchBegan(this, new KeyEventArgs() { keyState = keyState });

                }         
            }
            else if (this.keyState == KeyState.Down && keyState == KeyState.Up)
            {
                if (onTouchEnded != null)
                    onTouchEnded(this, new KeyEventArgs() { keyState = keyState });
                else if(onTouchBegan == null && onTouchClick != null)
                    onTouchClick(this, new KeyEventArgs() { keyState = keyState });
                this.keyState = KeyState.Idle;
            }
            isChanging = false;
        }

        public KeyEventHandler onTouchBegan { get; set; }
        public KeyEventHandler onTouchEnded { get; set; }
        public KeyEventHandler onTouchClick { get; set; }
    }
}
