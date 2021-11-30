using Incidences.Data.Models;
using Incidences.Models.Incidence;
using System;
using System.Collections.Generic;
using System.Data;

namespace Incidences.Data
{
    public class PieceTypeData : IPieceTypeData
    {
        #region constants
        //tables
        private const string piece_type = "piece_type";

        //columns
        private const string ALL = "*";

        #endregion

        private readonly ISqlBase sql;

        public PieceTypeData(ISqlBase sql)
        {
            this.sql = sql;
        }

        public IList<PieceType> SelectPieceType(CDictionary<string, string> conditions = null)
        {
            try
            {
                bool result = sql.Select(new Select(piece_type, new List<string> { ALL }, conditions));
                IList<PieceType> pieceTypes = new List<PieceType>();
                using IDataReader reader = sql.GetReader();

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

                sql.Close();
                return pieceTypes;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public PieceType SelectPieceTypeById(int pieceTypeId)
        {
            try
            {
                return SelectPieceType(sql.WhereId(pieceTypeId))[0];
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
