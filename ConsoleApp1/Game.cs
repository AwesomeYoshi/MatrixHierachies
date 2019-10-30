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

            //Load and position the tank
            tankSprite.Load("tankBlue_outline.png");
            tankSprite.SetRotate(-90 * (float)(Math.PI/180.0f));
            tankSprite.SetPosition(-tankSprite.Width / 2.0f, tankSprite.Height / 2.0f);

            //Load and position the green bullet to make the sprite go straight of the turrent
            bulletSprite.Load("bulletGreenSilver_outline.png");
            bulletSprite.SetRotate(90 * (float)(Math.PI / 180.0f));
            bulletSprite.imgScale = 0.5f;

            //Load and position the turret
            turrentSprite.Load("barrelBlue.png");
            turrentSprite.SetRotate(-90 * (float)(Math.PI / 180.0f));
            turrentSprite.SetPosition(0, turrentSprite.Width/ 2.0f);

            //Add child to the parent
            bulletObject.AddChild(bulletSprite);
            turrentObject.AddChild(turrentSprite);
            tankObject.AddChild(tankSprite);
            tankObject.AddChild(turrentObject);
           
            //Set the position of the tank in the middle
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

            //The controls for the tank
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
            //The controls for the turret
            if(rl.IsKeyDown(KeyboardKey.KEY_Q))
            {
                turrentObject.Rotate(-deltaTime);
            }
            if(rl.IsKeyDown(KeyboardKey.KEY_E))
            {
                turrentObject.Rotate(deltaTime);
            }        
            //The control for the bullet
            if(rl.IsKeyPressed(KeyboardKey.KEY_SPACE))
            {
                //Make the bullet appear, set the position, and update per frame when it becomes active
                turrentObject.AddChild(bulletObject);
                bulletObject.SetPosition(65,-5.5f);
                bulletObject.Update(deltaTime);
                bulletObject.active = true;
              
                float xPos = bulletObject.GlobalTransform.m7;
                float yPos = bulletObject.GlobalTransform.m8;

                float xR = turrentSprite.GlobalTransform.m1;
                float yR = turrentSprite.GlobalTransform.m4;
                float rot = (float)Math.Atan2(xR, yR);

                //Make the bullet remove from the turrent
                turrentObject.RemoveChild(bulletObject);

                //Set the rotation of the turrent from the globalTransform
                bulletObject.SetRotate(rot);
                //Set the position of the turrent from the globalTransform
                bulletObject.SetPosition(xPos, yPos);

                bulletTime = 2;    
            }

             lastTime = currentTime;

            //The collision boundries to make the bullet disappear
            if(bulletObject.GlobalTransform.m7 > 795 || bulletObject.GlobalTransform.m7 < 5 || bulletObject.GlobalTransform.m8 > 445|| bulletObject.GlobalTransform.m8 < 5)
            {
                bulletObject.active = false;
            }

            //The method for when the bullet is active so the bullet appears
            if(/*bulletTime > 0 && */bulletObject.active)
            {
                //bulletTime -= deltaTime;
                bulletSprite.Draw();
                //To make the bullet go foward
                Vector3 facing = new Vector3(bulletObject.LocalTransform.m1,
                            bulletObject.LocalTransform.m2, 1) *
                            deltaTime * 100;
                
                bulletObject.Translate(facing.x, facing.y);
            }
        }

        //Draw the framerate text and display the tank and turrent
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
