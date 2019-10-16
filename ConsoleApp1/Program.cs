using Raylib;
using rl = Raylib.Raylib;

namespace ConsoleApp1
{
    static class Program
    {
        public static int Main()
        {
            Game game = new Game();
            SceneObject tankObject = new SceneObject();
            SceneObject turrentObject = new SceneObject();

            // Initialization
            //--------------------------------------------------------------------------------------
            int screenWidth = 800;
            int screenHeight = 450;

            rl.InitWindow(screenWidth, screenHeight, "Tanks for Everything!");

            rl.SetTargetFPS(60);
            //--------------------------------------------------------------------------------------
            game.Init();
            // Main game loop
            while (!rl.WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                game.Update();
                // TODO: Update your variables here
                //----------------------------------------------------------------------------------
              
                // Draw
                //----------------------------------------------------------------------------------
                game.Draw();
              
                //----------------------------------------------------------------------------------
            }
            game.Shutdown();
            // De-Initialization
            //--------------------------------------------------------------------------------------
            rl.CloseWindow();
            // Close window and OpenGL context
                                     //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}
