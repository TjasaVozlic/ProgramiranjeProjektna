using HanoiTowers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanoiTowers
{

    public interface IMoveStrategy
    {
        public void MoveDisks(byte[] state, HashSet<uint> setPrev, HashSet<uint> setNew);
    }

    public class K13_01MoveStrategy : MoveStrategyBase, IMoveStrategy
    {
        public K13_01MoveStrategy(bool[] canMoveArray, short numDiscs, short numPegs, HanoiType hanoi, HashSet<uint> setPrev, byte[] newState, int currentState, HashSet<uint> setNew, HashSet<uint> setCurrent) : base(canMoveArray, numDiscs, hanoi, numPegs, setPrev, newState, currentState, setNew, setCurrent)
        { }

        public void MoveDisks(byte[] state, HashSet<uint> setPrev, HashSet<uint> setNew)
        {

            bool[] canMoveArray = Enumerable.Repeat(true, numPegs).ToArray();
            
                for (int i = 0; i < numDiscs - 2; i++)
                {
                            byte var0 = state[i];
                            if (canMoveArray[var0])
                            {
                                if (var0 == 0)
                                {
                                    for (byte j = 1; j < numPegs; j++)
                                    {
                                        if (canMoveArray[j])
                                        {
                                            AddNewState(state, i, j, setPrev, setNew);
                                        }
                                    }
                                }
                                else // From other vertices we can only move to center
                                {
                                    if (canMoveArray[0])
                                    {
                                        AddNewState(state, i, 0, setPrev, setNew);
                                    }
                                }
                            }

                            canMoveArray[var0] = false;
                }

                 byte var1 = state[numDiscs - 1];
                 byte var2 = state[numDiscs - 2];

                        // The second biggest:
                  if (var2 == 0 && var1 == 0)
                  {
                  if (canMoveArray[0] && canMoveArray[2])
                            {
                                   AddNewState(state, numDiscs - 2, 2, setPrev, setNew);
                            }
                            if (canMoveArray[0] && canMoveArray[3])
                            {
                                AddNewState(state, numDiscs - 2, 3, setPrev, setNew);
                            }
                            canMoveArray[0] = false;
                  }
                  else if (var2 == 0 && var1 == 1)
                  {
                            if (canMoveArray[0] && canMoveArray[1])
                            {
                    AddNewState(state, numDiscs - 2, 1, setPrev, setNew);
                            }
                            canMoveArray[0] = false;
                  }
                  else if (var2 > 1 && var1 == 1)
                  {
                            if (canMoveArray[var2] && canMoveArray[0])
                            {
                    AddNewState(state, numDiscs - 2, 0, setPrev, setNew);
                            }
                            canMoveArray[var2] = false;
                  }
                     // Biggest disk is moved only once
                  if (var1 == 0)
                  {
                            if (canMoveArray[0] && canMoveArray[1])
                            {
                                 AddNewState(state, numDiscs - 1, 1, setPrev, setNew);
                                //Console.WriteLine("The biggest is moved!\n");
                            }
                  }
            //Console.WriteLine("canMoveArray " + string.Join(", ", canMoveArray));


        }
    }
        public class K13_12MoveStrategy : MoveStrategyBase, IMoveStrategy
    {
        public K13_12MoveStrategy(bool[] canMoveArray, short numDiscs, HanoiType hanoi, short numPegs, HashSet<uint> setPrev, byte[] newState, int currentState, HashSet<uint> setNew, HashSet<uint> setCurrent) : base(canMoveArray, numDiscs, hanoi, numPegs, setPrev, newState, currentState, setNew, setCurrent)
        { }
        public void MoveDisks(byte[] state, HashSet<uint> setPrev, HashSet<uint> setNew)
        {

            bool[] canMoveArray = Enumerable.Repeat(true, numPegs).ToArray();

            for (int i = 0; i < numDiscs; i++)
            {
                if (canMoveArray[state[i]])
                {
                    if (state[i] == 0)
                    {
                        for (byte j = 1; j < numPegs; j++)
                        {
                            if (canMoveArray[j])
                            {
                                AddNewState(state, i, j, setPrev, setNew);
                            }
                        }
                    }
                    else // From other vertices we can only move to center
                    {
                        if (canMoveArray[0])
                        {
                            AddNewState(state, i, 0, setPrev, setNew);
                        }
                    }
                }
                canMoveArray[state[i]] = false;
            }

        }

    }
    public class K13eMoveStrategy : MoveStrategyBase, IMoveStrategy
    {
        public K13eMoveStrategy(bool[] canMoveArray, short numDiscs, HanoiType hanoi, short numPegs, HashSet<uint> setPrev, byte[] newState, int currentState, HashSet<uint> setNew, HashSet<uint> setCurrent) : base(canMoveArray, numDiscs, hanoi, numPegs, setPrev, newState, currentState, setNew, setCurrent)
        { }

        public void MoveDisks(byte[] state, HashSet<uint> setPrev, HashSet<uint> setNew)
        {
            bool[] innerCanMoveArray = Enumerable.Repeat(true, numPegs).ToArray();
            byte[] innerNewState;

            for (int i = 0; i < numDiscs; i++)
            {
                if (innerCanMoveArray[state[i]])
                {
                    byte stateValue = state[i];
                    byte[] possibleMoves;

                    switch (stateValue)
                    {
                        case 0:
                            possibleMoves = Enumerable.Range(1, numPegs - 1).Select(j => (byte)j).ToArray();
                            break;
                        case 1:
                            possibleMoves = new byte[] { 0 };
                            break;
                        case 2:
                            possibleMoves = new byte[] { 0, 3 };
                            break;
                        case 3:
                            possibleMoves = new byte[] { 0, 2 };
                            break;
                        default:
                            possibleMoves = Array.Empty<byte>();
                            break;
                    }

                    AddNewPossibleStates(state, possibleMoves, innerCanMoveArray, i, setPrev, setNew);

                }
                innerCanMoveArray[state[i]] = false;
            }
        }


    }
    public class K4eMoveStrategy : MoveStrategyBase, IMoveStrategy
    {
        public K4eMoveStrategy(bool[] canMoveArray, short numDiscs, HanoiType hanoi, short numPegs, HashSet<uint> setPrev, byte[] newState, int currentState, HashSet<uint> setNew, HashSet<uint> setCurrent) : base(canMoveArray, numDiscs, hanoi, numPegs, setPrev, newState, currentState, setNew, setCurrent)
        { }

        public void MoveDisks(byte[] state, HashSet<uint> setPrev, HashSet<uint> setNew)
        {
            bool[] canMoveArray = Enumerable.Repeat(true, numPegs).ToArray();

            for (int i = 0; i < numDiscs; i++)
            {
                if (canMoveArray[state[i]])
                {
                    byte stateValue = state[i];
                    byte[] possibleMoves;

                    switch (stateValue)
                    {
                        case 0:
                            possibleMoves = new byte[] { 1, 2, 3 };
                            break;
                        case 1:
                            possibleMoves = new byte[] { 0, 2, 3 };
                            break;
                        case 2:
                            possibleMoves = new byte[] { 0, 1 };
                            break;
                        case 3:
                            possibleMoves = new byte[] { 0, 1 };
                            break;
                        default:
                            possibleMoves = Array.Empty<byte>();
                            break;
                    }

                    AddNewPossibleStates(state, possibleMoves, canMoveArray, i, setPrev, setNew);

                }
                canMoveArray[state[i]] = false;
            }
        
        }


    }
    public class C4MoveStrategy : MoveStrategyBase, IMoveStrategy
    {
        public C4MoveStrategy(bool[] canMoveArray, short numDiscs, HanoiType hanoi, short numPegs, HashSet<uint> setPrev, byte[] newState, int currentState, HashSet<uint> setNew, HashSet<uint> setCurrent) : base(canMoveArray, numDiscs, hanoi, numPegs, setPrev, newState, currentState, setNew, setCurrent)
        { }

        public void MoveDisks(byte[] state, HashSet<uint> setPrev, HashSet<uint> setNew)
        {
            bool[] canMoveArray = Enumerable.Repeat(true, numPegs).ToArray();
            for (int i = 0; i < numDiscs; i++)
            {
                if (canMoveArray[state[i]])
                {
                    byte stateValue = state[i];
                    byte[] possibleMoves;

                    switch (stateValue)
                    {
                        case 0:
                        case 1:
                            possibleMoves = new byte[] { 2, 3 };
                            break;
                        case 2:
                        case 3:
                            possibleMoves = new byte[] { 0, 1 };
                            break;
                        default:
                            possibleMoves = Array.Empty<byte>();
                            break;
                    }

                    AddNewPossibleStates(state, possibleMoves, canMoveArray, i, setPrev, setNew);

                }
                canMoveArray[state[i]] = false;
            }
        }


    }
    public class P4MoveStrategy : MoveStrategyBase, IMoveStrategy
    {
        public P4MoveStrategy(bool[] canMoveArray, short numDiscs, HanoiType hanoi, short numPegs, HashSet<uint> setPrev, byte[] newState, int currentState, HashSet<uint> setNew, HashSet<uint> setCurrent) : base(canMoveArray, numDiscs, hanoi, numPegs, setPrev, newState, currentState, setNew, setCurrent)
        { }
        public void MoveDisks(byte[] state,HashSet<uint> setPrev, HashSet<uint> setNew)
        {

            bool[] canMoveArray = Enumerable.Repeat(true, numPegs).ToArray();

            for (int i = 0; i < numDiscs; i++)
            {
                if (canMoveArray[state[i]])
                {
                    byte stateValue = state[i];
                    byte[] possibleMoves;

                    switch (stateValue)
                    {
                        case 0:
                            possibleMoves = new byte[] { 3 };
                            break;
                        case 1:
                            possibleMoves = new byte[] { 2 };
                            break;
                        case 2:
                            possibleMoves = new byte[] { 1, 3 };
                            break;
                        case 3:
                            possibleMoves = new byte[] { 0, 2 };
                            break;
                        default:
                            possibleMoves = Array.Empty<byte>();
                            break;
                    }

                    AddNewPossibleStates(state, possibleMoves, canMoveArray, i, setPrev, setNew);
                }
                canMoveArray[state[i]] = false;
            }
        }
    }


}
