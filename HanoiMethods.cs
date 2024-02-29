using HanoiTowers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public HashSet<uint> setPrev;
        public HashSet<uint> setCurrent;
        public HashSet<uint> setNew;
        public byte[] stateArray;
        public bool[] canMoveArray;

        public byte[] newState;
        public int currentState;
        public int currentDistance;
        int finalState = 0;

        public IMoveStrategy MoveStrategy { get; set; } // Make sure MoveStrategy is set before calling MakeAMove

        private readonly object distanceLock = new object();

        public MoveStrategyBase(short numDisc, short numPegs, HanoiType typeHanoi, IMoveStrategy MoveStrategy)
        {
            this.numDiscs = numDisc;
            this.numPegs = numPegs;
            this.type = typeHanoi;
            stateArray = new byte[numDiscs];
            canMoveArray = new bool[numPegs];
            this.MoveStrategy = MoveStrategy;
            setPrev = new();
            setCurrent = new();
            setNew = new();
        }


        public MoveStrategyBase(bool[] canMoveArray, short numDiscs, HanoiType typeHanoi, short numPegs, HashSet<uint> setPrev, byte[] newState, int currentState, HashSet<uint> setNew, HashSet<uint> setCurrent)
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
                    Console.WriteLine("Final state" + finalState);
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
            setCurrent.Add((uint)initialState);

            path = "";

            int maxCardinality = 0;
            long maxMemory = 0;
            InitIgnoredStates(type);
            Console.WriteLine("final state" + finalState);
            while (true) // Analiza posameznega koraka (i-tega premika)
            {
                if (maxCardinality < setCurrent.Count)
                    maxCardinality = setCurrent.Count;

                bool solutionFound = false;

                var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
                Parallel.ForEach(setCurrent, parallelOptions, (num, loopState) =>
                {
                    //Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} processing number {num}");
                 
                    int localCurrentState = 0;
                    if (num == finalState)
                    {
                        //Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} found final state: {num}");
                        solutionFound = true;
                        loopState.Stop();
                    }

                    byte[] tmpState = LongToState((int)num);
                    MoveStrategy.MoveDisks(tmpState, setPrev, setNew);
                    //Console.WriteLine("setPrev " + string.Join(", ", setPrev));
                    //Console.WriteLine("setNew " + string.Join(", ", setNew));

                 


                    //Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} finished processing number {num}");
                });

                if(solutionFound)
                {
                    return currentDistance;
                }

                //Console.WriteLine("\n");
                //Console.WriteLine("\n");

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

        public void AddNewState(byte[] state, int disc, byte toPeg, HashSet<uint> setPrev, HashSet<uint> setNew)
        {
            byte[] newState = new byte[state.Length];
            Array.Copy(state, newState, state.Length);        
            newState[disc] = toPeg;

            int localCurrentState = StateToLong(newState);

            if (!setPrev.Contains((uint)localCurrentState))
            {
                lock (setNew)
                {
                    setNew.Add((uint)localCurrentState);
                }

            }
         
        }
        public void AddNewPossibleStates(byte[] state, byte[] posibleMoves, bool[] canMoveArray, int i, HashSet<uint> setPrev, HashSet<uint> setNew)
        {
            byte[] newState = new byte[state.Length];
            int currentState;
            foreach (byte j in posibleMoves)
            {
                if (canMoveArray[j])
                {
                    Array.Copy(state, newState, state.Length);
                    newState[i] = j;
                    currentState = StateToLong(newState);

                    if (!setPrev.Contains((uint)currentState))
                    {
                        lock (setNew)
                        {
                            setNew.Add((uint)currentState);
                        }
                    }

                }
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
            Array.Fill(array, true);

        }

    }
}
