﻿using Incidences.Data;
using Incidences.Data.Models;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Incidences.Business
{
    public class BusinessBase : IBusinessBase
    {
        public IList<ColumnKeyValue<string, string>> Pushcolumns(string field, string value, IList<ColumnKeyValue<string, string>> columns, string key = null)
        {
            if (!string.IsNullOrEmpty(value)) columns.Add(new ColumnKeyValue<string, string>(field, key, value));
            return columns;
        }
        public CDictionary<string, string> WhereIncidenceId(CDictionary<string, string> conditions, int incidenceId)
        {
            return WhereCommon(conditions, "incidenceId", null, $"{ incidenceId }");
        }
        public CDictionary<string, string> WhereNoteType(CDictionary<string, string> conditions, string noteType)
        {
            return WhereCommon(conditions, "noteType", null, $"{ noteType }");
        }
        public CDictionary<string, string> WhereOwnerId(CDictionary<string, string> conditions, int ownerId)
        {
            return WhereCommon(conditions, "ownerId", null, $"{ ownerId }");
        }
        public CDictionary<string, string> WhereSolverId(CDictionary<string, string> conditions, int solverId)
        {
            return WhereCommon(conditions, "solverId", null, $"{ solverId }");
        }
        public CDictionary<string, string> WhereEmployeeTypeName(CDictionary<string, string> conditions, string typeName)
        {
            return WhereCommon(conditions, "name", null, $"{ typeName }");
        }
        public CDictionary<string, string> WhereIncidence(CDictionary<string, string> conditions, int incidenceId)
        {
            return WhereCommon(conditions, "id", null, $"{ incidenceId }");
        }
        public CDictionary<string, string> WhereIncidenceState(CDictionary<string, string> conditions, int state)
        {
            return WhereCommon(conditions, "state", null, $"{ state }");
        }
        public CDictionary<string, string> WhereNotDeleted(CDictionary<string, string> conditions)
        {
            return WhereCommon(conditions, "deleted", "<>", $"{ 1 }");
        }
        public CDictionary<string, string> WherePieceId(CDictionary<string, string> conditions, int? id)
        {
            return WhereCommon(conditions, "id", "=", id.ToString());
        }
        public CDictionary<string, string> WherePieceId(CDictionary<string, string> conditions, IList<int> ids)
        {
            return WhereCommon(conditions, "id", "IN", string.Join(", ", ids));
        }
        public CDictionary<string, string> WhereUsername(CDictionary<string, string> conditions, string username)
        {
            return WhereCommon(conditions, "username", null, $"{ username }");
        }
        public CDictionary<string, string> WhereEmployee(CDictionary<string, string> conditions, int employee)
        {
            return WhereCommon(conditions, "employee", null, $"{ employee }");
        }
        public CDictionary<string, string> WhereEmployeeId(CDictionary<string, string> conditions, int? employeeId)
        {
            return WhereCommon(conditions, "employeeId", null, $"{ employeeId }");
        }
        public CDictionary<string, string> WhereTechnicianId(CDictionary<string, string> conditions, int technicianId)
        {
            return WhereCommon(conditions, "technicianId", null, $"{ technicianId }");
        }
        public CDictionary<string, string> WherePassword(CDictionary<string, string> conditions, string password)
        {
            return WhereCommon(conditions, "password", null, GetMD5(password));
        }
        public CDictionary<string, string> WhereId(CDictionary<string, string> conditions, int? id)
        {
            return WhereCommon(conditions, "id", null, id.ToString());
        }
        public static CDictionary<string, string> WhereCommon(CDictionary<string, string> conditions, string column, string key, string value)
        {
            conditions.Add(column, key, value);
            return conditions;
        }
        public string GetMD5(string str)
        {
            MD5 md5 = MD5.Create();
            ASCIIEncoding encoding = new();
            byte[] stream = null;
            StringBuilder sb = new();
            stream = md5.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }
    }
}