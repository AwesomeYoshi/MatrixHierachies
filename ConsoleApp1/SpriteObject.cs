using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Raylib;
using rl = Raylib.Raylib;

namespace ConsoleApp1
{
    public class SpriteObject : SceneObject
    {
        Texture2D texture = new Texture2D();
        Image image = new Image();
        public float imgScale = 1;

        public float Width
        {
            get { return texture.width; }
        }

        public float Height
        {
            get { return texture.height; }
        }

        public SpriteObject()
        {

        }

        //Load in a image sprite
        public void Load(string filename)
        {
            Image img = rl.LoadImage(filename);
            texture = rl.LoadTextureFromImage(img);
        }

        public override void OnDraw()
        {
            float rotation = (float)Math.Atan2(globalTransform.m2, globalTransform.m1);
            
            //Changes the position, rotation, imagescale, and color of the sprite
            Raylib.Raylib.DrawTextureEx(texture, new Vector2(globalTransform.m7, globalTransform.m8), rotation * (float)(180.0f / Math.PI), imgScale, Color.BLUE);

            
        }
    }
}
