# PlayerUnknown.NET

``PlayerUnknown.NET`` is a library used to work with PUBG, a game by BlueHole.

## Projects

This library is made (for the moment) of two projects :
 - ``PlayerUnknown`` (the main library)
 - ``PlayerUnknown.Sniffer`` (capturing network packets)
 
You can have an example of how to use the 'PlayerUnknown.Sniffer' library with the demo project, 'PlayerUnknown.Radar', still WIP.

## Usage

```csharp
namespace PlayerUnknown.Example
{
    internal static class Program
    {
        /// <summary>
        /// Defines the entry point of this application.
        /// <summary>
        internal static void Main()
        {
            PUBG.Initialize();    // Mandatory.
            PUBG.Attach();        // Mandatory. (PUBG must be running tho)
            PUBG.EnableEvents();  // Optional, but preferred. // It permits to detect when PUBG is running, minimized, maximized, etc.. automaticaly.

            if (PUBG.IsAttached == false)
            {
                Logging.Info(typeof(Program), "Waiting for PUBG to start...");

                while (PUBG.IsAttached != true)
                {
                    Thread.Sleep(500);
                }

                Thread.Sleep(2500); // Waits for the PUBG window to open. (Will be improved in a future commit)
            }
            
            // PUBG is started !
            // Do what you have to do now..
        }
    }
}
```
