using MiPrimeraApp.Data;
using MiPrimeraApp.Models.Incidence;
using System;
using System.Collections.Generic;

namespace MiPrimeraApp.Business
{
    public class Incidence
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
        public Incidences SelectIncidences(IList<string> fields, IDictionary<string, ColumnKeyValue> conditions = null)
        {
            try
            {
                object con = select(new select(['incidence'], $fields, $conditions));
                $incidences = [];
                if ($con) {
                    while ($fila = $con->fetch_array(MYSQLI_ASSOC)) {
                        array_push($incidences, fillIncidence($fields, $fila));
                    }
                }

                return $incidences;
            }
            catch (Exception $e) {
                return 'Something fails: '.$e->getMessage();
            }
            }
        }
