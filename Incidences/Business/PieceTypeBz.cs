using Incidences.Data;
using Incidences.Models.Incidence;
using System;
using System.Collections.Generic;

namespace Incidences.Business
{
    public class PieceTypeBz : IPieceTypeBz
    {
        private readonly IPieceTypeData pieceTypeData;
        public PieceTypeBz(IPieceTypeData pieceTypeData)
        {
            this.pieceTypeData = pieceTypeData;
        }
        public PieceType SelectPieceTypeById(int pieceTypeId)
        {
            try
            {
                return pieceTypeData.SelectPieceTypeById(pieceTypeId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public IList<PieceType> SelectPieceTypeList()
        {
            try
            {
                return pieceTypeData.SelectPieceType();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
