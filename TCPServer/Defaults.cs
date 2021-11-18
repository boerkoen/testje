using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remote
{
    class Defaults
    {
        //public enum ButtonCode // zal ze toch niet gebruiken, heb een tag gebruikt bij de buttons zodat ik maar 1 functie nodig heb. Zie de buttondefinition in de XAML om de code per toets te zien
        //{
        //    OnOff,
        //    One,
        //    Two,
        //    Three,
        //    Four,
        //    Five,
        //    Six,
        //    Seven,
        //    Eight,
        //    Nine,
        //    Source,
        //    Zero,
        //    Settings,
        //    Volumeup,
        //    Volumedown,
        //    Channelup,
        //    Channeldown
        //}
        public enum Sources
        {
            Cable_TV,
            VGA,
            HDMI_1,
            HDMI_2
        }
        public enum Channels
        {
            vtmHD,
            één,
            VIER,
            Canvas,
            Q2,
            VIJF,
            Vitaya,
            Regionaal,
            CAZ,
            KanaalZ,
            PlayTime,
            Ketnet,
            Fox,
            ZES,
            Discovery,
            NatGeo,
            ComedyCentral,
            MTV,
            Njam,
            Viceland,
            PlattelandsTV,
            BBC,
            Cadet
        }



        public static int MinVolume = 1;
        public static int MaxVolume = 10;
        public static int DefaultVolume = 3;

        public static int DefaultChannel = 1;

        public static int DefaultSource = 0;


    }
}
