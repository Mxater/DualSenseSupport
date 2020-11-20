namespace DSProject
{
    public class DsTrigger
    {
        public enum Modes
        {
            Off =0x0 ,
            Rigid =0x1,
            Pulse =0x2,
            Rigid_A=0x1 | 0x20,
            Rigid_B=0x1 | 0x04,
            Rigid_AB=0x1 | 0x20 | 0x04,
            Pulse_A = 0x2 | 0x20,
            Pulse_B = 0x2 | 0x04,
            Pulse_AB = 0x2 | 0x20 | 0x04,
            Calibration= 0xFC
        }

        public Modes Mode = Modes.Off;
        public int Force1 = 0;
        public int Force2 = 0;
        public int Force3 = 0;
        public int Force4 = 0;
        public int Force5 = 0;
        public int Force6 = 0;
        public int Force7 = 0;

        public DsTrigger(Modes mode,int force1 =0, int force2=0, int force3=0, int force4=0, int force5=0, int force6=0 ,int force7=0)
        {
            Mode = mode;
            Force1 = force1;
            Force2 = force2;
            Force3 = force3;
            Force4 = force4;
            Force5 = force5;
            Force6 = force6;
            Force7 = force7;
        }

        
        public DsTrigger()
        {
        }
    }
}