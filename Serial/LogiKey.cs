using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LedCSharp;
using System.Drawing;

namespace Serial
{
    public struct PercentageColor
    {
        public int red, green, blue;

        public PercentageColor(int red, int green, int blue)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
        }   
    }
       
    public static class ColorExtentions
    {
        public static PercentageColor toPercentageColor(this Color color)
        {
            return new PercentageColor((int)(100 * color.R / 255.0), (int)(100 * color.G / 255.0), (int)( 100 * color.B / 255.0));
        }
    }

    class LogiKey : Key
    {

        public LedCSharp.keyboardNames name { get; private set; }

        public LogiKey(LedCSharp.keyboardNames name)
        {
            this.name = name;
        }

        public override void CancelAll()
        {
            LogitechGSDK.LogiLedSetLightingForKeyWithKeyName(name, 0, 0, 0);
        }

        public bool SetLighting(Color color)
        {
            return SetLighting(color.toPercentageColor());
        }

        public bool SetLighting(PercentageColor color)
        {
            Console.WriteLine("setting color");
            LogitechGSDK.LogiLedSetLightingForKeyWithKeyName(name, color.red, color.green, color.blue);
            Console.WriteLine("done setting color");
            return true;
        }

        public bool SaveLighting()
        {
            return LogitechGSDK.LogiLedSaveLightingForKey(name);
        }

        public bool RestoreLighting()
        {
            return LogitechGSDK.LogiLedRestoreLightingForKey(name);
        }

        public bool Flash(PercentageColor color, int msDuration, int interval)
        {
            return LogitechGSDK.LogiLedFlashSingleKey(name, color.red, color.green, color.blue, msDuration, interval);
        }

        public bool Flash(Color color, int msDuration, int interval)
        {
            return Flash(color.toPercentageColor(), msDuration, interval);
        }

        public bool Pulse(PercentageColor startColor, PercentageColor endColor,int msDuration, bool isInfinite)
        {
            Console.WriteLine("pulsing");
            LogitechGSDK.LogiLedPulseSingleKey(name, startColor.red, startColor.green, startColor.blue,
                                                      endColor.red, endColor.green, endColor.blue, msDuration, isInfinite);
            Console.WriteLine("done pulsing");
            return true;
        }

        public bool Pulse(Color startColor, Color endColor, int msDuration, bool isInfinite)
        {
            return Pulse(startColor.toPercentageColor(), endColor.toPercentageColor(), msDuration, isInfinite);
        }

        public bool StopEffects()
        {
            return LogitechGSDK.LogiLedStopEffectsOnKey(name);
        }

    }

}
