using HanoiTowers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanoiTowers
{
    public class MoveStrategyBase
    {
        public readonly short numDiscs;
        public readonly short numPegs;
        public readonly HanoiType type;

        public Stack<int> setPrev;
        public Stack<int> setCurrent;
        public Stack<int> setNew;
        public byte[] stateArray;
        public bool[] canMoveArray;

        public byte[] newState;
        public int currentState;
        public int currentDistance;
        int finalState = 0;

        public MoveStrategyBase(short numDisc, short numPegs, HanoiType typeHanoi)
        {
            this.numDiscs = numDisc;
            this.numPegs = numPegs;
            this.type = typeHanoi;
            stateArray = new byte[numDiscs];
            canMoveArray = new bool[numPegs];

            setPrev = new();
            setCurrent = new();
            setNew = new();
        }


        public MoveStrategyBase(bool[] canMoveArray, short numDiscs, HanoiType typeHanoi, short numPegs, Stack<int> setPrev, byte[] newState, int currentState, Stack<int> setNew, Stack<int> setCurrent)
        {
            this.canMoveArray = canMoveArray;
            this.numDiscs = numDiscs;
            this.numPegs = numPegs;
            this.setPrev = setPrev;
            this.setNew = setNew;
            this.currentState = currentState;
            this.setCurrent = setCurrent;


        }
        public int ShortestPathForSmallDimension(out string path)
        {
            if (!Enum.IsDefined(typeof(HanoiType), type))
                throw new NotImplementedException("The search for this type is not implemented yet.");
            Console.WriteLine("number of discs" + numDiscs);
            // For each disc we have its peg
            stateArray = new byte[numDiscs];
            canMoveArray = new bool[numPegs];

            setPrev = new();
            setCurrent = new();
            setNew = new();


            // Set initial and final states for each case
            switch (type)
            {
                case HanoiType.K13_01:
                    stateArray = ArrayAllEqual(0);
                    finalState = FinalState();
                    break;
                case HanoiType.K13_12:
                    stateArray = ArrayAllEqual(2);
                    finalState = FinalState();
                    break;
                case HanoiType.K13e_01:
                    stateArray = ArrayAllEqual(0);
                    finalState = StateAllEqual(1);
                    break;
                case HanoiType.K13e_12:
                    stateArray = ArrayAllEqual(1);
                    finalState = StateAllEqual(2);
                    break;
                case HanoiType.K13e_23:
                    stateArray = ArrayAllEqual(2);
                    finalState = StateAllEqual(3);
                    break;
                case HanoiType.K13e_30:
                    stateArray = ArrayAllEqual(3);
                    finalState = StateAllEqual(0);
                    break;
                case HanoiType.K4e_01:
                    stateArray = ArrayAllEqual(0);
                    finalState = StateAllEqual(1);
                    break;
                case HanoiType.K4e_12:
                    stateArray = ArrayAllEqual(1);
                    finalState = StateAllEqual(2);
                    break;
                case HanoiType.K4e_23:
                    stateArray = ArrayAllEqual(2);
                    finalState = StateAllEqual(3);
                    break;
                case HanoiType.C4_01:
                    stateArray = ArrayAllEqual(0);
                    finalState = StateAllEqual(1);
                    break;
                case HanoiType.C4_12:
                    stateArray = ArrayAllEqual(1);
                    finalState = StateAllEqual(2);
                    break;
                case HanoiType.P4_01:
                    stateArray = ArrayAllEqual(0);
                    finalState = StateAllEqual(1);
                    break;
                case HanoiType.P4_12:
                    stateArray = ArrayAllEqual(1);
                    finalState = StateAllEqual(2);
                    break;
                case HanoiType.P4_23:
                    stateArray = ArrayAllEqual(2);
                    finalState = StateAllEqual(3);
                    break;
                case HanoiType.P4_31:
                    stateArray = ArrayAllEqual(3);
                    finalState = StateAllEqual(1);
                    break;
                default:
                    throw new Exception("Hanoi type state is not defined here!");
            }


            currentDistance = 0;
            int initialState = StateToLong(stateArray);
            setCurrent.Push(initialState);

            path = "";

            int maxCardinality = 0;
            long maxMemory = 0;
            InitIgnoredStates(type);

            while (true) // Analiza posameznega koraka (i-tega premika)
            {
                if (maxCardinality < setCurrent.Count)
                    maxCardinality = (short)setCurrent.Count;


                foreach (int num in setCurrent) // Znotraj i-tega premika preveri vsa možna stanja in se premakne v vse možne pozicije
                {
                    if (num == finalState)
                    {
                        return currentDistance;
                    }

                    byte[] tmpState = LongToState(num);
                    switch (type)
                    {
                        case HanoiType.K13_01:

                            K13_01MoveStrategy K13_01Move = new K13_01MoveStrategy(this.canMoveArray, this.numDiscs, this.numPegs, this.type, this.setPrev, this.newState, this.currentState, this.setNew, this.setCurrent);
                            K13_01Move.MoveDisks(tmpState);
                            break;
                        case HanoiType.K13_12:
                            K13_12MoveStrategy K13_12Move = new K13_12MoveStrategy(this.canMoveArray, this.numDiscs, this.type, this.numPegs, this.setPrev, this.newState, this.currentState, this.setNew, this.setCurrent);
                            K13_12Move.MoveDisks(tmpState);
                            break;
                        case HanoiType.K13e_01:
                        case HanoiType.K13e_12:
                        case HanoiType.K13e_23:
                        case HanoiType.K13e_30:
                            K13eMoveStrategy K13eMove = new K13eMoveStrategy(this.canMoveArray, this.numDiscs, this.type, this.numPegs, this.setPrev, this.newState, this.currentState, this.setNew, this.setCurrent);
                            K13eMove.MoveDisks(tmpState);
                            break;
                        case HanoiType.K4e_01:
                        case HanoiType.K4e_12:
                        case HanoiType.K4e_23:
                            K4eMoveStrategy K4eMove = new K4eMoveStrategy(this.canMoveArray, this.numDiscs, this.type, this.numPegs, this.setPrev, this.newState, this.currentState, this.setNew, this.setCurrent);
                            K4eMove.MoveDisks(tmpState);
                            break;
                        case HanoiType.C4_01:
                        case HanoiType.C4_12:
                            C4MoveStrategy c4Move = new C4MoveStrategy(this.canMoveArray, this.numDiscs, this.type, this.numPegs, this.setPrev, this.newState, this.currentState, this.setNew, this.setCurrent);
                            c4Move.MoveDisks(tmpState);
                            break;
                        case HanoiType.P4_01:
                        case HanoiType.P4_12:
                        case HanoiType.P4_23:
                        case HanoiType.P4_31:
                            P4MoveStrategy p4Move = new P4MoveStrategy(this.canMoveArray, this.numDiscs, this.type, this.numPegs, this.setPrev, this.newState, this.currentState, this.setNew, this.setCurrent);
                            p4Move.MoveDisks(tmpState);
                            break;
                    }
                }

                long mem = GC.GetTotalMemory(false);
                if (maxMemory < mem)
                {
                    maxMemory = mem;
                }

                // Ko se premaknemo iz vseh trenutnih stanj,
                // pregledamo nova trenutna stanja
                setPrev = setCurrent;
                setCurrent = new(setNew);
                setNew = new();

                currentDistance++;

                Console.WriteLine("Current distance: " + currentDistance + "     Maximum cardinality: " + maxCardinality);
                Console.WriteLine("Memory allocation: " + mem / 1000000 + "MB  \t\t Maximum memory: " + maxMemory / 1000000 + "MB");
                Console.CursorTop -= 2;
            }
        }

        public void InitIgnoredStates(HanoiType type)
        {
            switch (type)
            {
                case HanoiType.K13_01:
                    AddStateLeading3();
                    AddStateLeading1Then3();
                    break;
            }
        }

        public void AddStateLeading1Then3()
        {
            int[] newState;
            for (int i = 1; i < numDiscs; i++)
            {
                newState = new int[numDiscs];
                newState[0] = 1;
                for (int j = 1; j <= i; j++)
                    newState[j] = 3;
            }
        }

        public void AddStateLeading3()
        {
            int[] newState;
            for (int i = 0; i < numDiscs; i++)
            {
                newState = new int[numDiscs];
                for (int j = 0; j <= i; j++)
                    newState[j] = 3;
            }
        }

        public void AddNewState(byte[] state, int disc, byte toPeg)
        {
            byte[] newState = new byte[state.Length];
            Array.Copy(state, newState, state.Length);
            newState[disc] = toPeg;

            currentState = StateToLong(newState);

            if (!setPrev.Contains(currentState))
            {
                setNew.Push(currentState);
            }
        }

        public int StateToLong(byte[] state)
        {
            int num = 0;
            int factor = 1;
            for (int i = state.Length - 1; i >= 0; i--)
            {
                num += state[i] * factor;
                factor *= this.numPegs;
            }
            return num;
        }

        public int FinalState()
        {
            int num = 0;
            int factor = 1;
            for (int i = (int)numDiscs - 1; i >= 0; i--)
            {
                num += factor;
                factor *= numPegs;
            }
            Console.WriteLine("Final state: " + num);
            return num;
        }

        public byte[] LongToState(int num)
        {
            byte[] tmpState = new byte[numDiscs];
            for (int i = numDiscs - 1; i >= 0; i--)
            {
                tmpState[i] = (byte)(num % (int)numPegs);
                num /= numPegs;
            }
            return tmpState;
        }

        public int StateAllEqual(short pegNumber)
        {
            int num = 0;
            int factor = 1;
            for (int i = numDiscs - 1; i >= 0; i--)
            {
                num += ((int)pegNumber * factor);
                factor *= (int)numPegs;
            }
            return num;
        }

        public byte[] ArrayAllEqual(byte pegNumber)
        {
            byte[] array = Enumerable.Repeat(pegNumber, numDiscs).ToArray();
            return array;
        }

        public void ResetArray(bool[] array)
        {
            for(int i = array.Length - 1; i >= 0; i--)
{
                array[i] = true;
            }

        }

    }
}
