using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MonoGamePlayFab.Interfaces
{
    public interface ICoroutine
    {
        ICoroutineService CoroutineManager { get; }
        IEnumerator Routine { get; set; }
        ICoroutine WaitForCoroutine { get; set; }
        bool Finished { get; set; }
    }
}
