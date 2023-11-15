using System.Collections.Generic;

public struct RecSession
{
    public int sF; // sampling frequency
    public int sT; // sampling time
    public int cT; // contraction time
    public int rT; // rest time
    public int nM; // number of movements
    public int nR; // number of repetitions
    public int nCh; // number of channels (7)
    public List<GameLookup.graspNamesEnum> mov; // movements
    public string date; 
    public double[,,] tdata; // (sF * sT) x nCh x nM
}