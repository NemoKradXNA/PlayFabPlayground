using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGamePlayFab.Interfaces
{
    public interface IEncryptionService
    {
        byte[] Encrypt(string data);
        string Decrypt(byte[] data);
    }
}
