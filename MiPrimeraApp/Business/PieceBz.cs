using MiPrimeraApp.Data;
using MiPrimeraApp.Data.Models;
using MiPrimeraApp.Models.Incidence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimeraApp.Business
{
    public class PieceBz : BusinessBase
    {
        public bool InsertPiecesSql(IList<int> pieces, int incidenceId, IDbCommand conexion = null)
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
                return Call(text, conexion);
            }
            catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
        public bool DeletePiecesSql(IList<int> pieces, int incidenceId, IDbCommand conexion = null)
        {
            try
            {
                IList<string> values = new List<string>();
                string text = string.Empty;
                foreach (int piece in pieces)
                {
                    values.Add($"({piece}, {incidenceId})");
                }
                if (pieces.Count > 1)
                {
                    string stringPieces = string.Join(", ", values);
                    text = $"UPDATE incidence_piece_log  SET status = 1 WHERE piece IN ({ stringPieces })";
                }
                else
                {
                    int piece = pieces[0];
                    text = $"UPDATE incidence_piece_log  SET status = 1 WHERE piece = { piece }";
                }

                return Call(text, conexion);
            }
            catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
        public bool DeletePiece(int id, IDbCommand conexion = null)
        {
            try
            {
                return Update("piece_class", new CDictionary<string, string> { { "deleted", null, "1" } }, new CDictionary<string, string> { { "id", null, id.ToString() } }, conexion);
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
                    WherePieceId(new CDictionary<string, string>(), new List<int> { id }));
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
                        new List<int> { id }
                    )
                );
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public Piece SelectPieceById(int pieceId)
        {
            try
            {
                IList<Piece> pieces = SelectPieces(WherePieceId(new CDictionary<string, string>(), new List<int> { pieceId}));
                return pieces[0];
            }
            catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
        public IList<Piece> SelectPieces(CDictionary<string, string> conditions = null)
        {
            try
            {
                object con = Select(new Select("FullPiece", new List<string> { "*" }, conditions));
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
                        (int)reader.GetValue(5)
                    ));
                }

                return pieces;
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
    }
}
