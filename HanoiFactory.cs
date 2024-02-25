using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HanoiTowers
{
    public static class HanoiFactory
    {

        public static Hanoi GetHanoi(short numDiscs, short numPegs, HanoiType type)
        {
            // Pripravimo si novo spremenljivko
            Hanoi hanoi = null;

            // Ustvarimo instanco glede na izbrani tip
            switch (type)
            {
                case HanoiType.K13_01:
                    hanoi = new K13_01(numDiscs, numPegs);
                    break;
                case HanoiType.K13_12:
                    hanoi = new K13_12(numDiscs, numPegs);
                    break;
                case HanoiType.K13e_01:
                case HanoiType.K13e_12:
                case HanoiType.K13e_23:
                case HanoiType.K13e_30:
                    hanoi = new K13e(numDiscs, numPegs, type);
                    break;
                case HanoiType.K4e_01:
                case HanoiType.K4e_12:
                case HanoiType.K4e_23:
                    hanoi = new K4e(numDiscs, numPegs, type);
                    break;
                case HanoiType.C4_01:
                case HanoiType.C4_12:
                    hanoi = new C4(numDiscs, numPegs, type);
                    break;
                case HanoiType.P4_01:
                case HanoiType.P4_12:
                case HanoiType.P4_23:
                case HanoiType.P4_31:
                    hanoi = new P4(numDiscs, numPegs, type);
                    break;
            }

            return hanoi;
        }
    }

}
