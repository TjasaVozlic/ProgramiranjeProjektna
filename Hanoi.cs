using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HanoiTowers;

namespace HanoiTowers
{
    public enum HanoiType
    {
        K13_01,
        K13_12,
        K13e_01,
        K13e_12,
        K13e_23,
        K13e_30,
        P4_01,
        P4_12,
        P4_23,
        P4_31,
        C4_01,
        C4_12,
        K4e_01,
        K4e_12,
        K4e_23,
    }

    public abstract class Hanoi
    {
        public readonly short numDiscs;
        public readonly short numPegs;
        public HanoiType type;
        public HashSet<uint> setPrev;
        public HashSet<uint> setCurrent;
        public HashSet<uint> setNew;
        public byte[] stateArray;
        public bool[] canMoveArray;

        public byte[] newState;
        public int currentState;
        public uint currentDistance;
        int finalState = 0;
        public IMoveStrategy MoveStrategy { get; set; } // Make sure MoveStrategy is set before calling MakeAMove


        public Hanoi(short numDiscs, short numPegs)
        {
            this.numDiscs = numDiscs;
            this.numPegs = numPegs;
        }

        public static HanoiType SelectHanoiType()
        {
            Console.WriteLine(">> Select coloring type:");
            WriteHanoiTypes();
            return (HanoiType)Enum.Parse(typeof(HanoiType), Console.ReadLine());
        }

        private static void WriteHanoiTypes()
        {
            foreach (string s in Enum.GetNames(typeof(HanoiType)))
            {
                Console.WriteLine("\t" + (int)Enum.Parse(typeof(HanoiType), s) + " - " + s);
            }
        }
        public int MakeMoveForSmallDimension(out string path)
        {
            MoveStrategyBase moveStrategy = new MoveStrategyBase(this.numDiscs, this.numPegs, this.type, this.MoveStrategy);
            // Uporabite izbrano strategijo
            return moveStrategy.ShortestPathForSmallDimension(out path);
        }

    }

    public class K13_01 : Hanoi
    {
        public K13_01(short numDiscs, short numPegs) : base(numDiscs, numPegs)
        {

            type = HanoiType.K13_01;
            MoveStrategy = new K13_01MoveStrategy(this.canMoveArray, this.numDiscs, this.numPegs, this.type,  this.setPrev, this.newState, this.currentState, this.setNew, this.setCurrent);
        }
    }

    public class K13_12 : Hanoi
    {
        public K13_12(short numDiscs, short numPegs) : base(numDiscs, numPegs)
        {
            type = HanoiType.K13_12;
            MoveStrategy = new K13_12MoveStrategy(this.canMoveArray, this.numDiscs, this.type, this.numPegs, this.setPrev, this.newState, this.currentState, this.setNew, this.setCurrent);
        }
    }

    public class K13e : Hanoi
    {
        public K13e(short numDiscs, short numPegs, HanoiType hanoiType) : base(numDiscs, numPegs)
        {
            type = hanoiType;
            MoveStrategy = new K13eMoveStrategy(this.canMoveArray, this.numDiscs, this.type, this.numPegs, this.setPrev, this.newState, this.currentState, this.setNew, this.setCurrent);
        }
    }

    public class K4e : Hanoi
    {
        public K4e(short numDiscs, short numPegs, HanoiType hanoiType) : base(numDiscs, numPegs)
        {
            type = hanoiType;
            MoveStrategy = new K4eMoveStrategy(this.canMoveArray, this.numDiscs, this.type, this.numPegs, this.setPrev, this.newState, this.currentState, this.setNew, this.setCurrent);
        }
    }
    public class C4 : Hanoi
    {
        public C4(short numDiscs, short numPegs, HanoiType hanoiType) : base(numDiscs, numPegs)
        {
            type = hanoiType;
            MoveStrategy = new C4MoveStrategy(this.canMoveArray, this.numDiscs, this.type, this.numPegs, this.setPrev, this.newState, this.currentState, this.setNew, this.setCurrent);
        }
    }
    public class P4 : Hanoi
    {
        public P4(short numDiscs, short numPegs, HanoiType hanoiType) : base(numDiscs, numPegs)
        {
            type = hanoiType;
            MoveStrategy = new P4MoveStrategy(this.canMoveArray, this.numDiscs, this.type, this.numPegs, this.setPrev, this.newState, this.currentState, this.setNew, this.setCurrent);
        }
    }


  


}
