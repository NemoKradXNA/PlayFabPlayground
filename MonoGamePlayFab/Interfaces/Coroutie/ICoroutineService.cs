using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MonoGamePlayFab.Interfaces
{
    public interface ICoroutineService
    {
        IList<ICoroutine> Coroutines { get; }

        void Update(GameTime gameTime);

        void UpdateEndFrame(GameTime gameTime);

        ICoroutine StartCoroutine(IEnumerator routine);

        void StopCoroutine(IEnumerator coroutine);

        ICoroutine StartCoroutine(ICoroutine coroutine);
        void StopCoroutine(ICoroutine coroutine);
    }
}
