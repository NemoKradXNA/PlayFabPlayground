using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGamePlayFab.Interfaces
{
    public interface IUIBase
    {
        Point Position { get; set; }
        Point Size { get; set; }
        Rectangle Rectangle { get; }
        Color Tint { get; set; }

        Texture2D CreateBoxTexture(int width, int height, Rectangle thickenss, Color bgColor, Color edgeColor);

    }
}
