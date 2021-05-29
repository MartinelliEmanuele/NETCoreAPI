using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;
using static Utils.Enumeratori;

namespace Utils
{
    public static class Tools
    {

        public static ILog _log = ResourceFactory.FileLogger(typeof(Tools));
        #region Gestione delle date
        public static DateTime OnlyData(DateTime Data)
        {
            DateTime OnlyData = new DateTime(Data.Year, Data.Month, Data.Day);
            return OnlyData;
        }

        public static string GetDateStr(DateTime dataConv, string format)
        {
            //Ottiene una stringa da un DateTime con la data formattata in base alla nostra scelta (in questo caso Italiana "dd/MM/yyyy")
            string retString = dataConv.ToString(format);
            return retString;
        }

        public static string GetDateTimeStr(DateTime dataConv)
        {
            //Ottiene una stringa da un DateTime con la data formattata in base alla nostra scelta (in questo caso Italiana "dd/MM/yyyy")
            string retString = dataConv.ToString();
            return retString;
        }

        public static string GetDateStr(Object dataConv)
        {
            //Ottiene DateTime da un Object che viene poi passato alla funzione "GetDateStr" per ottenere una stringa
            DateTime retData = Convert.ToDateTime(dataConv);
            string retString = GetDateStr(retData);
            return retString;
        }

        public static DateTime GetDateDT(string dataConv)
        {
            //Ottiene un DateTime da una stringa con la data formattata in base alla nostra scelta (in questo caso Italiana "dd/MM/yyyy")
            DateTime DTdata = new DateTime(Convert.ToInt32(dataConv.Substring(6, 4)), Convert.ToInt32(dataConv.Substring(3, 2)), Convert.ToInt32(dataConv.Substring(0, 2)));
            return DTdata;
        }

        public static string GetClearSring(string testo)
        {
            //Sostituisce eventuali caratteri html con l'opportuna codifica.
            return HttpUtility.HtmlDecode(testo);
        }
        #endregion

        #region Max e Min su liste di dati
        public static decimal GetMinOfList(List<decimal> listValue)
        {
            decimal min = decimal.MaxValue;
            foreach (decimal value in listValue)
            {
                min = (value > 0 && value < min ? value : min);
            }
            return (min == decimal.MaxValue ? 1 : min);
        }

        public static double GetMinOfList(List<double> listValue)
        {
            double min = double.MaxValue;
            foreach (double value in listValue)
            {
                min = (value != double.NaN && value != -1 && value < min ? value : min);
            }
            return min;
        }

        public static decimal GetMinOfList(List<int> listValue)
        {
            int min = int.MaxValue;
            foreach (int value in listValue)
            {
                min = (value != -1 && value < min ? value : min);
            }
            return min;
        }

        public static decimal GetMaxOfList(List<decimal> listValue)
        {
            decimal max = decimal.MinValue;
            foreach (decimal value in listValue)
            {
                max = (value != -1 && value > max ? value : max);
            }
            return max;
        }

        public static double GetMaxOfList(List<double> listValue)
        {
            double max = 0;
            foreach (double value in listValue)
            {
                max = (value != -1 && value > max ? value : max);
            }
            return max;
        }

        public static decimal GetMaxOfList(List<int> listValue)
        {
            int max = int.MinValue;
            foreach (int value in listValue)
            {
                max = (value != -1 && value > max ? value : max);
            }
            return max;
        }
        #endregion

        #region Conversione liste in stringhe concatenate
        public static string GetStringFromListForQuery(List<int> listId)
        {
            // Converte una lista di int in una stringa formattata per fare una query
            string ret = "";
            foreach (int id in listId)
            {
                ret += (ret == "" ? id.ToString() : ", " + id.ToString());
            }
            return ret;
        }

        public static string GetStringFromListForQuery(List<long> listId)
        {
            // Converte una lista di int in una stringa formattata per fare una query
            string ret = "";
            foreach (long id in listId)
            {
                ret += (ret == "" ? id.ToString() : ", " + id.ToString());
            }
            return ret;
        }

        public static string GetStringFromListForQuery(List<string> objects)
        {
            // Converte una lista di int in una stringa formattata per fare una query
            string ret = "";
            foreach (string obj in objects)
            {
                ret += (ret == "" ? obj : ", " + obj);
            }
            return ret;
        }

        public static string GetStringFromListForQuery(Dictionary<int, string> listId)
        {
            // Converte una lista di int in una stringa formattata per fare una query
            string ret = "";
            foreach (KeyValuePair<int, string> id in listId)
            {
                ret += (ret == "" ? id.Key.ToString() : ", " + id.Key.ToString());
            }
            return ret;
        }

        public static string GetStringFromListForQuery(Dictionary<long, string> listId)
        {
            // Converte una lista di int in una stringa formattata per fare una query
            string ret = "";
            foreach (KeyValuePair<long, string> id in listId)
            {
                ret += (ret == "" ? id.Key.ToString() : ", " + id.Key.ToString());
            }
            return ret;
        }
        #endregion

        #region Gestione File
        /*public static void DownloadFile(System.Web.HttpResponse Response, string filePath)
        {
            Byte[] file = File.ReadAllBytes(filePath);
            File.Delete(filePath);
            Response.ContentType = "documento/" + Path.GetExtension(filePath).ToUpper();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
            Response.BinaryWrite(file);
            Response.End();
        }

        public static void DownloadFile(System.Web.HttpContext context, string filePath)
        {
            Byte[] file = File.ReadAllBytes(filePath);
            File.Delete(filePath);
            context.Response.ContentType = "text/" + Path.GetExtension(filePath).ToUpper();
            context.Response.AddHeader("content-disposition", "attachment; filename=" + Path.GetFileName(filePath));
            context.Response.BinaryWrite(file);
            context.Response.End();
        }*/
        #endregion

        #region Sicurezza delle stringhe

        /*public static string GetProtectedString(string text)
        {
            string purpose = "ohnZcaO#EwzQ2vcx@";
            byte[] stream = Encoding.UTF8.GetBytes(text);
            byte[] encodedValue = MachineKey.Protect(stream, purpose);
            return HttpServerUtility.UrlTokenEncode(encodedValue);
        }

        public static string GetUnprotectedString(string text)
        {
            string purpose = "ohnZcaO#EwzQ2vcx@";
            byte[] stream = HttpServerUtility.UrlTokenDecode(text);
            byte[] decodedValue = MachineKey.Unprotect(stream, purpose);
            return Encoding.UTF8.GetString(decodedValue);
        }
        */
        public static string GetStringHash(string text)
        {
            var passwordSalt = "CzJ8OEOyZ6@P3DXt!AjLJc#";
            text = text + passwordSalt;
            StringBuilder Sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(text));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }
            return Sb.ToString();
        }

        public static string GetRandomString()
        {
            int length = 10;
            const string allowedChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            Random rng = new Random();
            char[] chars = new char[length];
            int setLength = allowedChars.Length;
            for (int i = 0; i < length; ++i)
            {
                chars[i] = allowedChars[rng.Next(setLength)];
            }
            return new string(chars, 0, length);
        }
        #endregion

        #region Sql Server Query builder

        /// <summary>
        /// Method use to get query dinamically.
        /// The primary table has "T" acronym
        /// If you want create where select or more colmplex where condition you can custom string in filter "SqlValue" param
        /// </summary>
        /// <param name="pagedRequest"> Represent an object that contains all filter fields, order by fields, filter logic operator, and the number of rows to select</param>
        /// <param name="columns"> Columns to select</param>
        /// <param name="join">String that contain the join string</param>
        /// <param name="groupBy">String that contain the list of columns to add in the group by</param>
        /// <returns>Return the sql query string</returns>
        public static string GetSqlStandardFilter(PagedRequest pagedRequest, string columns = "", string join = "", string groupBy = "")
        {
            int fromRow = ((pagedRequest.CurrentPage - 1) < 0 ? 0 : pagedRequest.CurrentPage - 1) * pagedRequest.RowsPerPage + 1;
            int toRow = (pagedRequest.CurrentPage == 0 ? 1 : pagedRequest.CurrentPage) * pagedRequest.RowsPerPage;
            StringBuilder sqlString = new StringBuilder();
            sqlString.AppendLine(" SELECT a.* FROM ");
            sqlString.AppendLine($" ( SELECT ROW_NUMBER() OVER( ");
            
            if (pagedRequest.OrderByFields.Count() > 0)
            {
                List<string> orderBy = new List<string>();
                foreach (string col in pagedRequest.OrderByFields)
                {
                    string sort = "";
                    string column = "";
                    if (col.ToUpper().Contains("DESC"))
                    {
                        sort = " DESC ";
                        column = col.Replace("DESC", "");
                    }
                    else if (col.ToUpper().Contains("ASC"))
                    {
                        sort = " ASC ";
                        column = col.Replace("ASC", "");
                    }
                    else
                    {
                        column = col;
                    }
                    bool isOtherTable = column.ToLower().Split('.').Count() > 1;
                    string[] words = Regex.Matches(isOtherTable ? column.Split('.')[1] : column, "(^[a-z]+|[A-Z]+(?![a-z])|[A-Z][a-z]+)").OfType<Match>().Select(m => m.Value).ToArray();
                    //orderBy.Add((isOtherTable ? column.Split('.')[0] + "." : "T.") + string.Join("_", words) + sort);
                    orderBy.Add(string.Join("_", words) + sort);
                }
                sqlString.AppendLine($" ORDER BY {string.Join(",", orderBy)}");
            } else
            {
                sqlString.AppendLine($" ORDER BY (SELECT TOP(1) COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{pagedRequest.Entity}')");
            }

            sqlString.AppendLine(") as RN, b.*  FROM");
            sqlString.AppendLine($" ( SELECT {(pagedRequest.Distinct? " DISTINCT " : "")}");
            if (columns.Length > 0)
            {
                sqlString.AppendLine(columns);
            }
            else
            {
                sqlString.AppendLine("T.*");
            }
            sqlString.AppendLine(" FROM " + pagedRequest.Entity + " T ");

            if (join.Length > 0)
            {
                sqlString.AppendLine(join);
            }
            int countFilter = pagedRequest.Filters.Count();
            if (countFilter > 0)
            {
                GetSQLWhereCondition(pagedRequest, ref sqlString);
            }
            if (groupBy.Length > 0)
            {
                sqlString.AppendLine(groupBy);
            }
            
            sqlString.AppendLine(" ) b ");
            sqlString.AppendLine(" ) a ");
            if (toRow != 0)
            {
                sqlString.Append(" WHERE a.RN BETWEEN " + fromRow + " AND " + toRow);
            }

            return sqlString.ToString();
        }

        public static string CountRows(PagedRequest pagedRequest, string join = "", string groupBy = "")
        {
            StringBuilder sqlString = new StringBuilder();
            sqlString.AppendLine($" SELECT {(pagedRequest.Distinct? " DISTINCT " : "")} COUNT(*) ");
            sqlString.AppendLine(" FROM " + pagedRequest.Entity + " T ");

            if (join.Length > 0)
            {
                sqlString.AppendLine(join);
            }
            int countFilter = pagedRequest.Filters.Count();
            if (countFilter > 0)
            {
                GetSQLWhereCondition(pagedRequest, ref sqlString);
            }
            if (groupBy.Length > 0)
            {
                sqlString.AppendLine(groupBy);
            }
            return sqlString.ToString();
        }

        private static void GetSQLWhereCondition(PagedRequest pagedRequest, ref StringBuilder sqlString)
        {
            int countFilter = pagedRequest.Filters.Count();
            sqlString.AppendLine(" WHERE ");
            int i = 0;
            foreach (Filter filter in pagedRequest.Filters)
            {
                i++;
                bool isOtherTable = filter.Key.ToLower().Split('.').Count() > 1;
                string[] words = Regex.Matches(isOtherTable ? filter.Key.Split('.')[1] : filter.Key, "(^[a-z]+|[A-Z]+(?![a-z])|[A-Z][a-z]+)").OfType<Match>().Select(m => m.Value).ToArray();
                filter.Key = (isOtherTable ? filter.Key.Split('.')[0] + "." : "") + string.Join("_", words);
                if (filter.Type == FilterDataType.DateYear || filter.Type == FilterDataType.Date || filter.Type == FilterDataType.DateMonth)
                {
                    string formatData = filter.Type == FilterDataType.DateYear ? "DATEPART( YYYY, " : (filter.Type == FilterDataType.DateMonth ? "DATEPART( MM, " : "(");
                    sqlString.Append($" {formatData} CONVERT(date," + (isOtherTable ? filter.Key : "T." + filter.Key) + $"))  {filter.Operator.ToString()}  ");
                }
                else
                {
                    sqlString.Append(" " + (isOtherTable ? filter.Key : "T." + filter.Key) + " " + filter.Operator.ToString() + " ");
                }

                switch (filter.Operator.ToUpper())
                {
                    case "IN":
                        if (filter.SqlValue != "")
                        {
                            sqlString.Append("(" + filter.SqlValue + ")");
                        }
                        else
                        {
                            string[] values = filter.Value.Split(',');
                            List<string> In = new List<string>();
                            foreach (string value in values)
                            {
                                In.Add("'" + value + "'");
                            }
                            sqlString.AppendLine("(" + string.Join(",", In) + ")");
                        }
                        break;
                    case "LIKE":
                        sqlString.AppendLine("'%" + filter.Value.ToUpper() + "%'");
                        break;
                    case "IS NULL":
                        break;
                    case "IS NOT NULL":
                        break;
                    case "BETWEEN":
                        break;
                    default:
                        if (filter.SqlValue != "")
                        {
                            sqlString.AppendLine(filter.SqlValue);
                        }
                        else
                        {
                            sqlString.AppendLine("'" + filter.Value.ToUpper() + "'");
                        }
                        break;
                }
                if (i < countFilter)
                {
                    if (pagedRequest.FilterLogicOperator != null && pagedRequest.FilterLogicOperator == "OR")
                    {
                        sqlString.AppendLine(" OR ");
                    }
                    else
                    {
                        sqlString.AppendLine(" AND ");
                    }
                }
            }
        }

        public static string GetSqlStringForMerge(string tableName, Dictionary<string, string> fields, bool columnUsingUderscore = true)
        {
            StringBuilder sql = new StringBuilder();
            List<string> fKeys = new List<string>();
            sql.Append($"MERGE {tableName} AS dest ");
            sql.Append(" USING( ");
            sql.Append(" SELECT ");
            fKeys = (from field in fields
                     select $" @{field.Key} ").ToList();
            sql.Append(string.Join(",", fKeys));
            sql.Append(") AS source ( ");
            fKeys = (from field in fields
                     select $" {(columnUsingUderscore ? string.Join("_", Regex.Matches(field.Key, "(^[a-z]+|[A-Z]+(?![a-z])|[A-Z][a-z]+)").OfType<Match>().Select(m => m.Value)) : field.Key)} ").ToList();
            sql.Append(string.Join(",", fKeys));
            sql.Append(" ) ON ( ");
            fKeys = (from field in fields
                     where field.Value == "pk"
                     select $" dest.{ (columnUsingUderscore ? string.Join("_", Regex.Matches(field.Key, "(^[a-z]+|[A-Z]+(?![a-z])|[A-Z][a-z]+)").OfType<Match>().Select(m => m.Value)) : field.Key) }" +
                     $" = source.{ (columnUsingUderscore ? string.Join("_", Regex.Matches(field.Key, "(^[a-z]+|[A-Z]+(?![a-z])|[A-Z][a-z]+)").OfType<Match>().Select(m => m.Value)) : field.Key)} ").ToList();
            sql.Append(string.Join("AND", fKeys));
            sql.Append(" )");

            sql.Append(" WHEN MATCHED THEN UPDATE SET ");
            fKeys = (from field in fields
                     where field.Value != "pk" && field.Value != "DOI" && field.Value != "CreatedBy"
                     select $" dest.{(columnUsingUderscore ? string.Join("_", Regex.Matches(field.Key, "(^[a-z]+|[A-Z]+(?![a-z])|[A-Z][a-z]+)").OfType<Match>().Select(m => m.Value)) : field.Key)} " +
                     $"= source.{(columnUsingUderscore ? string.Join("_", Regex.Matches(field.Key, "(^[a-z]+|[A-Z]+(?![a-z])|[A-Z][a-z]+)").OfType<Match>().Select(m => m.Value)) : field.Key)} ").ToList();
            sql.Append(string.Join(",", fKeys));

            sql.Append(" WHEN NOT MATCHED THEN INSERT (");
            fKeys = (from field in fields
                     where field.Value != "pk" // && field.Value == ""
                     select $" {(columnUsingUderscore ? string.Join("_", Regex.Matches(field.Key, "(^[a-z]+|[A-Z]+(?![a-z])|[A-Z][a-z]+)").OfType<Match>().Select(m => m.Value)) : field.Key)} ").ToList();
            sql.Append(string.Join(",", fKeys));
            sql.Append(") VALUES ( ");
            fKeys = (from field in fields
                     where field.Value != "pk"  // && field.Value == ""
                     select $" source.{(columnUsingUderscore ? string.Join("_", Regex.Matches(field.Key, "(^[a-z]+|[A-Z]+(?![a-z])|[A-Z][a-z]+)").OfType<Match>().Select(m => m.Value)) : field.Key)} ").ToList();
            sql.Append(string.Join(",", fKeys));
            sql.Append(")");

            fKeys = (from field in fields
                     where field.Value == "pk"
                     select $" inserted.{(columnUsingUderscore ? string.Join("_", Regex.Matches(field.Key, "(^[a-z]+|[A-Z]+(?![a-z])|[A-Z][a-z]+)").OfType<Match>().Select(m => m.Value)) : field.Key)}").ToList();
            sql.Append($"OUTPUT {string.Join(",", fKeys)} ;");

            return sql.ToString();
        }


        public static string GetSqlStringForDelete(string tableName, string pk, string paramName)
        {
            return $"DELETE FROM {tableName} WHERE {pk} = @{paramName}";

        }
        #endregion

        #region Mailing List
        public static List<string> CreaListaMailFornitori(string mails)
        {
            List<string> destinatari = new List<string>();
            char separator = ' ';

            if (mails.Contains(','))
            {
                separator = ',';
            }
            if (mails.Contains(';'))
            {
                separator = ';';
            }

            if (separator != ' ')
            {
                string[] fArr = mails.Split(separator);

                foreach (string singleMail in fArr)
                {
                    destinatari.Add(singleMail.Trim());
                }
            }
            else
            {
                destinatari.Add(mails);
            }

            return destinatari;
        }
        #endregion

        #region File - Folder handling
        public static bool FolderExist(DirectoryInfo folder, bool createFolder = true)
        {
            bool res = true;
            if (!folder.Exists && createFolder)
            {
                folder.Create();
                res = true;
            }
            else if (!folder.Exists && !createFolder)
            {
                res = false;
            }
            return res;
        }

        public static bool FileExist(string filePath, bool createFile = true, bool deleteFile = false)
        {
            bool res = true;
            if (!File.Exists(filePath) && createFile)
            {
                File.Create(filePath).Close();
                res = true;
            }
            else if (!File.Exists(filePath) && !createFile && !deleteFile)
            {
                res = false;
            }
            else if (File.Exists(filePath) && deleteFile)
            {
                File.Delete(filePath);
                res = true;
            }
            return res;
        }

        public static string GetFileExtension(string fileName)
        {
            string[] split = fileName.Split('.');
            if (split.Length > 1)
                return split[split.Length - 1];
            else
                return null;
        }

        public static void DeleteFolderRecoursive(DirectoryInfo folder)
        {
            string filePath = "";
            try
            {
                foreach (FileInfo file in folder.GetFiles())
                {
                    filePath = file.DirectoryName;
                    file.Delete();
                }
                filePath = "";
                folder.Delete();
            }
            catch (Exception ex)
            {
                ExceptionExtras eh = new ExceptionExtras("Errore durante l'eliminazione di una folder");
                //logger.Error("Errore durante l'eliminazione della cartella " + folder.FullName);
                //logger.Error("Errore durante l'eliminazione del file " + filePath);
                throw new CustomException(eh, ex);
            }
        }

        public static bool IsExcel(string type)
        {
            return new string[] { "application/vnd.ms-excel","application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" }.ToList().Contains(type);
        }
        public static bool IsCsv(string type)
        {
            return new string[] { "application/vnd.ms-excel", "text/csv" }.ToList().Contains(type);
        }
        public static bool IsXml(string type)
        {
            return new string[] { "application/xml", "text/xml" }.ToList().Contains(type);
        }        

#endregion

        #region Object toString
        /// <summary>
        /// Trasforma un oggetto in formato JSON.
        /// Questo metodo sostituisce Fody per la scrittura degli oggetti nei file di log
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x">Oggetto da deserializzare</param>
        /// <returns>Stringa otenente l'oggetto in JSON</returns>
        public static string Dump<T>(this T x)
        {
            return JsonConvert.SerializeObject(x, Formatting.Indented);
        }
#endregion
    }
}
