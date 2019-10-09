using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Raylib;
using rl = Raylib.Raylib;

namespace ConsoleApp1
{
    class Game
    {
        Stopwatch stopwatch = new Stopwatch();

        SceneObject tankObject = new SceneObject();
        SceneObject turrentObject = new SceneObject();

        SpriteObject tankSprite = new SpriteObject();
        SpriteObject turrentSprite = new SpriteObject();

        private long currentTime = 0;
        private long lastTime = 0;
        private float timer = 0;
        private int fps = 1;
        private int frames;

        private float deltaTime = 0.005f;

        public void Init()
        {
            stopwatch.Start();
            lastTime = stopwatch.ElapsedMilliseconds;

            tankSprite.Load("tankBlue_outline.png");
            tankSprite.SetRotate(-90 * (float)(Math.PI / 180.0f));
            tankSprite.SetPosition(-tankSprite.Width / 2.0f, tankSprite.Height / 2.0f);


            turrentSprite.Load("barrelBlue.png");
            turrentSprite.SetRotate(-90 * (float)(Math.PI / 180.0f));
            turrentSprite.SetPosition(0, turrentSprite.Width/ 2.0f);

            turrentObject.AddChild(turrentSprite);
            tankObject.AddChild(tankSprite);
            tankObject.AddChild(turrentObject);

            tankObject.SetPosition(rl.GetScreenWidth()/2.0f, rl.GetScreenHeight()/2.0f);
        }

        public void Shutdown()
        {

        }

        public void Update()
        {
            currentTime = stopwatch.ElapsedMilliseconds;
            deltaTime = (currentTime - lastTime) / 1000.0f;

            timer += deltaTime;
            if(timer>= 1)
            {
                fps = frames;
                frames = 0;
                timer -= 1;
            }
            frames++;

            if(rl.IsKeyDown(KeyboardKey.KEY_A))
            {
                tankObject.Rotate(-deltaTime);
            }
            if(rl.IsKeyDown(KeyboardKey.KEY_D))
            {
                tankObject.Rotate(deltaTime);
            }
            if(rl.IsKeyDown(KeyboardKey.KEY_W))
            {
                Vector3 facing = new Vector3(tankObject.LocalTransform.m1, 
                                            tankObject.LocalTransform.m2, 1) *
                                            deltaTime * 100;

                tankObject.Translate(facing.x, facing.y);
            }
            if(rl.IsKeyDown(KeyboardKey.KEY_S))
            {
                Vector3 facing = new Vector3(tankObject.LocalTransform.m1, tankObject.LocalTransform.m2, 1) *
                                            deltaTime * -100;

                tankObject.Translate(facing.x, facing.y);
            }
            if(rl.IsKeyDown(KeyboardKey.KEY_Q))
            {
                turrentObject.Rotate(-deltaTime);
            }
            if(rl.IsKeyDown(KeyboardKey.KEY_E))
            {
                turrentObject.Rotate(deltaTime);
            }
            tankObject.Update(deltaTime);

            lastTime = currentTime;
        }

        public void Draw()
        {
            rl.BeginDrawing();

            rl.ClearBackground(Color.WHITE);
            rl.DrawText(fps.ToString(), 10, 10, 12, Color.RED);

            tankObject.Draw();

            rl.EndDrawing();
        }
    }
}
