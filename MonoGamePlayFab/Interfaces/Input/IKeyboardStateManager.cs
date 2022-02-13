using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGamePlayFab.Interfaces
{
    public interface IKeyboardStateManager : IInputStateManager
    {
        KeyboardState State { get; set; }
        KeyboardState LastState { get; set; }

        bool NumLock { get; }
        bool CapsLock { get; }
        bool ShiftIsDown { get; }
        bool CtrlIsDown { get; }

        Keys[] KeysPressed();

        List<Keys> KeysPressedThisFrame();
        bool KeyDown(Keys key);
        bool KeyPress(Keys key);
    }
}
