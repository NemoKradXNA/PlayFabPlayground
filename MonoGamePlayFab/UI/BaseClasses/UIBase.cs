using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGamePlayFab.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGamePlayFab.UI.BaseClasses
{
    public abstract class UIBase : DrawableGameComponent, IUIBase
    {
        protected IAudioService audioManager { get { return Game.Services.GetService<IAudioService>(); } }
        protected IInputStateService inputManager { get { return Game.Services.GetService<IInputStateService>(); } }

        public Point Position { get; set; }
        public Point Size { get; set; }

        protected SpriteBatch _spriteBatch { get; set; }

        private Rectangle _rectangle;
        public Rectangle Rectangle
        {
            get
            {
                if (_rectangle == null || (_rectangle.X != Position.X || _rectangle.Y != Position.Y || _rectangle.Width != Size.X || _rectangle.Height != Size.Y))
                    _rectangle = new Rectangle(Position.X, Position.Y, Size.X, Size.Y);

                return _rectangle;
            }
        }
        public Color Tint { get; set; }

        public UIBase(Game game, Point position, Point size) : base(game)
        {
            Position = position;
            Size = size;
            Tint = Color.White;
        }

        public override void Initialize()
        {
            base.Initialize();
            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }

        public Texture2D CreateBoxTexture(int width, int height, Rectangle thickenss, Color bgColor, Color edgeColor)
        {
            Texture2D boxTexture = new Texture2D(Game.GraphicsDevice, width, height);

            Color[] c = new Color[width * height];

            Color color = new Color(0, 0, 0, 0);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x < thickenss.X || x >= width - thickenss.Width || y < thickenss.Height || y >= height - thickenss.Y)
                        color = edgeColor;
                    else
                        color = bgColor;

                    c[x + y * width] = color;
                }
            }

            boxTexture.SetData(c);

            return boxTexture;
        }
    }
}
