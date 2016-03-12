using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LedCSharp;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Threading;

namespace Serial
{

    class Program
    {
        List<LogiKey> keys;
        // Create the serial port with basic settings 
        private SerialPort port = new SerialPort("COM3",
          9600, Parity.None, 8, StopBits.One);

        static void Main(string[] args)
        {   
            LogitechGSDK.LogiLedInit();
            LogitechGSDK.LogiLedSetLighting(0, 0, 0);
            LogitechGSDK.LogiLedSetLightingForKeyWithScanCode((int)LedCSharp.keyboardNames.ESC, 100, 0, 0);
            new Program();
            while (true) ;       
            LogitechGSDK.LogiLedShutdown();
        }


        private Program()
        {
            keys = new List<LogiKey>();
            var f1 = new LogiKey(LedCSharp.keyboardNames.F1);
            f1.onTouchBegan += SetRedLight;
            f1.onTouchEnded += PulseRed;
            keys.Add(f1);

            var f2 = new LogiKey(LedCSharp.keyboardNames.F2);
            f2.onTouchBegan += SetRedLight;
            f2.onTouchEnded += PulseRed;
            keys.Add(f2);

            var f3 = new LogiKey(LedCSharp.keyboardNames.F3);
            f3.onTouchBegan += SetRedLight;
            f3.onTouchEnded += PulseRed;
            keys.Add(f3);

            var f4 = new LogiKey(LedCSharp.keyboardNames.F4);
            f4.onTouchBegan += SetRedLight;
            f4.onTouchEnded += PulseRed;
            keys.Add(f4);

            // Attach a method to be called when there
            // is data waiting in the port's buffer 

            var listeningThread = new Thread(() =>
            {
            port = new SerialPort("COM3",
                9600, Parity.None, 8, StopBits.One);

                port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            // Begin communications 
                port.Open();
                while (true)
                {
                
                }
            });

            listeningThread.IsBackground = true;
            listeningThread.Start();

        
        }

        private void SetRedLight(object sender, KeyEventArgs e)
        {
            ((LogiKey)sender).SetLighting(Color.FromArgb(0, 120, 120, 0));
        }

        private void PulseRed(object sender, KeyEventArgs e)
        {
            //((LogiKey)sender).SetLighting(Color.FromArgb(0, 0, 0, 0));
            LogitechGSDK.LogiLedSetLighting(0, 0, 0);
            //((LogiKey)sender).SaveLighting();
            //Thread.Sleep(20);
            //var b = ((LogiKey)sender).Flash(Color.FromArgb(0, 255, 0, 0), 500, 50);
            //Thread.Sleep(600);
            
            //while (true) ;
            //LogitechGSDK.LogiLedSetLightingForKeyWithScanCode((int)((LogiKey)sender).name, 0, 0, 0);
            //((LogiKey)sender).StopEffects();
            
            //((LogiKey)sender).RestoreLighting();

        }

        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Show all the incoming data in the port's buffer
            // String.
            // Regex.Split()
            var data = port.ReadLine();
            var lines = data.Split(new[] { '\n' });
            foreach(var line in lines)
            {
                try
                {
                    var num = Int32.Parse(line);
                    Console.WriteLine(num);
                    //var keyName = LedCSharp.keyboardNames.ESC;
                    switch (num)
                    {
                        case 1:
                            keys[0].Touch(Key.KeyState.Down);
                            //keyName = LedCSharp.keyboardNames.F1;
                            break;
                        case 2:
                            keys[1].Touch(Key.KeyState.Down);
                            //keyName = LedCSharp.keyboardNames.F2;
                        break;
                        case 3:
                            keys[2].Touch(Key.KeyState.Down);
                            //keyName = LedCSharp.keyboardNames.F3;
                            break;
                        case 4:
                            keys[3].Touch(Key.KeyState.Down);
                            //keyName = LedCSharp.keyboardNames.F4;
                            break;
                        case -1:
                            keys[0].Touch(Key.KeyState.Up);
                            //keyName = LedCSharp.keyboardNames.F1;
                            break;
                        case -2:
                            keys[1].Touch(Key.KeyState.Up);
                            //keyName = LedCSharp.keyboardNames.F2;
                            break;
                        case -3:
                            keys[2].Touch(Key.KeyState.Up);
                            //keyName = LedCSharp.keyboardNames.F3;
                            break;
                        case -4:
                            keys[3].Touch(Key.KeyState.Up);
                            //var listeningThread = new Thread(() =>
                            //{
                            //    keys[3].Touch(Key.KeyState.Up);

                            //});
                            //listeningThread.IsBackground = true;
                            //listeningThread.Start();
                            //keyName = LedCSharp.keyboardNames.F4;
                            break;
                    }
                    //LogitechGSDK.LogiLedPulseSingleKey(keyName, 100, 0, 0, 
                    //    0, 0, 0, 500, false);
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
