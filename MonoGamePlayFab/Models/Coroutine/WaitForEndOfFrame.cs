using Microsoft.Xna.Framework;
using MonoGamePlayFab.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MonoGamePlayFab.Models.Coroutine
{
    public class WaitForEndOfFrame : Coroutine, IWaitCoroutine
    {
        public WaitForEndOfFrame(Game game) : base(game)
        {
            Routine = routine();
            CoroutineManager.StartCoroutine(this);
        }

        protected virtual IEnumerator routine()
        {
            yield break;
        }
    }
}
