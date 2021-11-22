﻿namespace MiPrimeraApp.Models.Incidence
{
    public class ReportedPiece
    {
        public string pieceName;
        public int pieceNumber;
        public ReportedPiece(string pieceName, int pieceNumber)
        {
            this.pieceName = pieceName;
            this.pieceNumber = pieceNumber;
        }
    }
}
