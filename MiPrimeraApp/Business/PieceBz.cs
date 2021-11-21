using MiPrimeraApp.Data;
using MiPrimeraApp.Data.Models;
using MiPrimeraApp.Models.Incidence;
using System;
using System.Collections.Generic;
using System.Data;

namespace MiPrimeraApp.Business
{
    public class PieceBz : BusinessBase
    {
        public bool InsertPiecesSql(IList<int> pieces, int incidenceId)
        {
            try
            {
                IList<string> values = new List<string>();
                foreach (int piece in pieces)
                {
                    values.Add($"({piece}, {incidenceId})");
                }
                string stringPieces = string.Join(", ", values);
                string text = $"INSERT INTO incidence_piece_log (piece, incidence) VALUES ({ stringPieces });";
                return Call(text);
            }
            catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
        public bool DeletePiecesSql(IList<int> pieces, int incidenceId)
        {
            try
            {

                CDictionary<string, string> conditions;
                if (pieces.Count > 1) conditions = WherePieceId(new CDictionary<string, string>(), pieces);
                else conditions = WherePieceId(new CDictionary<string, string>(), pieces[0]);

                return Update("incidence_piece_log", new CDictionary<string, string> { { "status", null, "1" } }, conditions); ;
            }
            catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
        public bool DeletePiece(int id)
        {
            try
            {
                return Update("piece_class", new CDictionary<string, string> { { "deleted", null, "1" } }, new CDictionary<string, string> { { "id", null, id.ToString() } });
            }
            catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
        public CDictionary<string, string> GetPieceColumns(int type, string name, bool deleted)
        {
            CDictionary<string, string> columns = new()
            {
                { "type", null, type.ToString() },
                { "name", null, name },
                { "deleted", null, deleted.ToString() }
            };

            return columns;
        }
        public bool UpdatePiece(int id, int type, string name, bool deleted = false)
        {
            try
            {
                return Update(
                    "piece_class",
                    new()
                    {
                        { "type", null, type.ToString() },
                        { "name", null, name },
                        { "deleted", null, deleted.ToString() }
                    },
                    WherePieceId(new CDictionary<string, string>(),  id ));
            }
            catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
        public bool UpdatePiece(int id, bool deleted)
        {
            try
            {
                return Update("piece_class", 
                    new(){
                        { "deleted", null, deleted.ToString() }
                    }, 
                    WherePieceId(
                        new CDictionary<string, string>(), 
                        id
                    )
                );
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private Piece SelectPieceById(int pieceId)
        {
            try
            {
                IList<Piece> pieces = SelectPieces(WherePieceId(new CDictionary<string, string>(), pieceId));
                return pieces[0];
            }
            catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
        private IList<Piece> SelectPieces(CDictionary<string, string> conditions = null)
        {
            try
            {
                bool result = Select(new Select("FullPiece", new List<string> { "*" }, conditions));
                if (result)
                {
                    IList<Piece> pieces = new List<Piece>();
                    using IDataReader reader = this.get_sql.ExecuteReader();
                    while (reader.Read())
                    {
                        pieces.Add(new(
                            (int)reader.GetValue(0),
                            (string)reader.GetValue(1),
                            new PieceType(
                                (int)reader.GetValue(2),
                                (string)reader.GetValue(3),
                                (string)reader.GetValue(4)
                            ),
                            Convert.ToBoolean(reader.GetValue(5))
                        ));
                    }

                    return pieces;
                } else throw new Exception("Ningún registro");
            }
            catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
        public bool DeletePieceFn(int id)
        {
            try
            {
                return UpdatePiece(id, true);
            } catch (Exception e) {
                throw new Exception(e.Message);
            }
}
        public IList<Piece> GetPieces(int? piece = null)
        {
            try
            {
                CDictionary<string, string> conditions;
                if (piece != null)
                {
                    conditions = WherePieceId(WhereNotDeleted(new CDictionary<string, string>()), piece);
                }
                else
                {
                    conditions = WhereNotDeleted(new CDictionary<string, string>());
                }

                return SelectPieces(conditions);
            }
            catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
    }
}
