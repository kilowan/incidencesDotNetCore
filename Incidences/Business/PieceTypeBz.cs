using Incidences.Data;
using Incidences.Data.Models;
using Incidences.Models.Incidence;
using System;
using System.Collections.Generic;
using System.Data;

namespace Incidences.Business
{
    public class PieceTypeBz : IPieceTypeBz
    {
        #region constants
        //tables
        private const string piece_type = "piece_type";

        //columns
        private const string ALL = "*";

        #endregion

        private readonly ISqlBase sql;
        private readonly IBusinessBase bisba;
        public PieceTypeBz(ISqlBase sql, IBusinessBase bisba)
        {
            this.sql = sql;
            this.bisba = bisba;
        }
        public PieceType SelectPieceTypeById(int pieceTypeId)
        {
            try
            {
                return SelectPieceType(
                    this.bisba.WhereId(new CDictionary<string, string>(), pieceTypeId)
                )[0];
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private IList<PieceType> SelectPieceType(CDictionary<string, string> conditions = null)
        {
            try
            {
                bool result = this.sql.Select(new Select(piece_type, new List<string> { ALL }, conditions));
                IList<PieceType> pieceTypes = new List<PieceType>();
                using IDataReader reader = this.sql.GetReader();

                while (reader.Read())
                {
                    pieceTypes.Add(
                        new PieceType(
                            (int)reader.GetValue(0),
                            (string)reader.GetValue(1),
                            (string)reader.GetValue(2)
                        )
                    );
                }

                this.sql.Close();
                return pieceTypes;
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
                return SelectPieceType();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
