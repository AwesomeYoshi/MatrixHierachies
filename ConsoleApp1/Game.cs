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
        SceneObject bulletObject = new SceneObject();
        SceneObject RootObject = new SceneObject();

        SpriteObject tankSprite = new SpriteObject();
        SpriteObject turrentSprite = new SpriteObject();
        SpriteObject bulletSprite = new SpriteObject();


        private long currentTime = 0;
        private long lastTime = 0;
        private float timer = 0;
        private int fps = 1;
        private int frames;
        private float bulletTime = 0;
     
        private float deltaTime = 0.005f;

        public void Init()
        {
            stopwatch.Start();
            lastTime = stopwatch.ElapsedMilliseconds;

            tankSprite.Load("tankBlue_outline.png");
            tankSprite.SetRotate(-90 * (float)(Math.PI/180.0f));
            tankSprite.SetPosition(-tankSprite.Width / 2.0f, tankSprite.Height / 2.0f);

            bulletSprite.Load("bulletGreenSilver_outline.png");
            bulletSprite.SetRotate(90 * (float)(Math.PI / 180.0f));
            bulletSprite.imgScale = 0.5f;

            turrentSprite.Load("barrelBlue.png");
            turrentSprite.SetRotate(-90 * (float)(Math.PI / 180.0f));
            turrentSprite.SetPosition(0, turrentSprite.Width/ 2.0f);

            bulletObject.AddChild(bulletSprite);
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
            deltaTime = (currentTime - lastTime) / 700.0f;
            
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
                Vector3 facing = new Vector3(tankObject.LocalTransform.m1, 
                                            tankObject.LocalTransform.m2, 1) *
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
            if(rl.IsKeyDown(KeyboardKey.KEY_SPACE) && bulletTime <= 0)
            {
                turrentObject.AddChild(bulletObject);
                bulletObject.SetPosition(65,-5.5f);
                bulletObject.Update(deltaTime);
                turrentObject.RemoveChild(bulletObject);

                bulletObject.Update(deltaTime);
                RootObject.AddChild(bulletObject);



                bulletTime = 2;    
            }

             lastTime = currentTime;

            if(bulletTime > 0)
            {
                bulletTime -= deltaTime;
                bulletSprite.Draw();
                Vector3 facing = new Vector3(bulletObject.LocalTransform.m1,
                            bulletObject.LocalTransform.m2, 1) *
                            deltaTime * 100;

                bulletObject.Translate(facing.x, facing.y);
            }
        }

        public void Draw()
        {
            rl.BeginDrawing();

            rl.ClearBackground(Color.WHITE);
            rl.DrawText(fps.ToString(), 10, 10, 12, Color.DARKBLUE);

            tankObject.Draw();

            rl.EndDrawing();
        }
    }
}
