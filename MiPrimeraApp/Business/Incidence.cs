using MiPrimeraApp.Data;
using MiPrimeraApp.Data.Models;
using MiPrimeraApp.Models.Incidence;
using System;
using System.Collections.Generic;
using System.Data;

namespace MiPrimeraApp.Business
{
    public class Incidence : SqlBase
    {
        public Incidences GetIncidencesByStateTypeFn(string state, string userId, string type)
        {
            try
            {
                $own = SelectIncidences(['*'], 
                whereEmployeeId(
                    whereIncidenceState(new dictionary(), $state),
                    $userId)
                    );
                $incidences = new incidences($own);
                if (in_array($type, ['Technician', 'Admin']) && $state !== '4') {
                    $incidences->other = selectIncidences(['*'], 
                    whereTechnicianId(
                        whereIncidenceState(new dictionary(), $state),
                        $userId)
                    );
                }

                return $incidences;
            }
            catch (Exception $e) {
                return 'Something fails: '.$e->getMessage();
            }
            }
        }
        public Incidences SelectIncidences(IList<string> fields, CDictionary<string, string> conditions = null)
        {
            try
            {
                object con = Select(new Select("FullIncidence", fields, conditions));
                Incidences incidences = new Incidences();
                if (con != null) {
                    using IDataReader reader = this.get_sql.ExecuteReader();
                    while (reader.Read())
                    {
                        Models.Incidence.Incidence inc = new Models.Incidence.Incidence(
                            (int)reader.GetValue(0),
                            (string)reader.GetValue(2),
                            (int)reader.GetValue(1),
                            (DateTime)reader.GetValue(6),
                            (string)reader.GetValue(3)
                        );
                        incidences.own.Add(inc);
                    }
                }
                foreach (Models.Incidence.Incidence incidence in incidences.own)
                {
                    IList<string> list = new List<string>();
                    list.Add("*");

                    con = Select(new Select("piece_class", list, conditions));
                }
                return $incidences;
            }
            catch (Exception $e) {
                return 'Something fails: '.$e->getMessage();
            }
            }
        }
