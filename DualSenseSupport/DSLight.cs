namespace DualSenseSupport
{
    
    public class DSLight
    {
        public enum LedOptions 
        {
            None=0x0,
            PlayerLedBrightnes=0x1,
            UninterrumpableLed=0x2,
            Both=0x01 | 0x02
        }

        public enum PulseOptions
        {
            None=0,
            FadeBlue=1,
            FadeOut=2
        }
        public Colors Colors=new Colors();
        public int Brightness=0;
        public int PlayerNumber=0;
        public int LedOption = 0;
        public int PulseOption = 0;
    }
}