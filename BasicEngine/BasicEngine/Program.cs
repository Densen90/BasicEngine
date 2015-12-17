// C# example program to demonstrate OpenTK
//
// Steps:
// 1. Create an empty C# console application project in Visual Studio
// 2. Place OpenTK.dll in the directory of the C# source file
// 3. Add System.Drawing and OpenTK as References to the project
// 4. Paste this source code into the C# source file
// 5. Run. You should see a colored triangle. Press ESC to quit.
//
// Copyright (c) 2013 Ashwin Nanjappa
// Released under the MIT License

using System;
using BasicEngine;
using BasicEngine.Managers;

namespace StarterKit
{
    class Application
    { 
        [STAThread]
        static void Main()
        {
            // The 'using' idiom guarantees proper resource cleanup.
            // We request 30 UpdateFrame events per second, and unlimited
            // RenderFrame events (as fast as the computer can handle).
            using (BasicEngine.Core.Game game = new BasicEngine.Core.Game())
            {
                game.SetListener(new SceneManager());
                game.Run();
            }
        }
    }
}
