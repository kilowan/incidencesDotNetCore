namespace Incidences.Models.Incidence
{
    public class ReportedPiece
    {
        private string pieceName;
        private int pieceNumber;

        public string PieceName
        {
            get
            {
                return pieceName;
            }
            set
            {
                pieceName = value;
            }
        }
        public int PieceNumber
        {
            get
            {
                return pieceNumber;
            }
            set
            {
                pieceNumber = value;
            }
        }
        public ReportedPiece(string pieceName, int pieceNumber)
        {
            this.pieceName = pieceName;
            this.pieceNumber = pieceNumber;
        }
    }
}
