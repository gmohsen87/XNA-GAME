using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BallGame
{
    class Backgrounds
    {
        public Texture2D texture;
        public Rectangle rectangle;
    

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, rectangle, Color.White);
    }
}
class Scrolling : Backgrounds
    {
        public Scrolling(Texture2D newTexture, Rectangle newRectangle)
        {
            texture = newTexture;
            rectangle = newRectangle;
        }
        public void Update()
        {
            rectangle.X -= 3;
        }
            
    }
}
