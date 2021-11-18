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
    }
}
