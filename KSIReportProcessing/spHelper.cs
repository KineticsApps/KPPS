﻿using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;


namespace SPHelpers
{
    public static class Extensions
    {
        public static bool IsNegative(this int n)
        { return Math.Abs(n) > n; }
    }


    public static class QueryAssistants
    {
        internal static string getInternalFieldName(string strList, string field, ClientContext ctx)
        {
            field = field.Trim();
            var cList = ctx.Web.Lists.GetByTitle(strList);
            var fc = cList.Fields.GetByInternalNameOrTitle(field);
            ctx.Load(cList);
            ctx.Load(fc);
            ctx.ExecuteQuery();
            var internalName = fc.InternalName;
            return internalName;
        } // EndMethod: getInternalFieldName


        internal static int getListItemID(List cList, string keyVal, string keyID, ClientContext ctx)
        {
            //var cList = ctx.Web.Lists.GetByTitle(strlist);
            int rtnID = 0;
            var query = new CamlQuery
            {
                ViewXml = "<View><Query><Where><Eq><FieldRef Name='" + keyID + "'/><Value Type='Text'>" + keyVal + "</Value></Eq></Where></Query><RowLimit>1</RowLimit><ViewFields><FieldRef Name='ID' /></ViewFields><QueryOptions /></View>"
            };
            var rtn = cList.GetItems(query);
            ctx.Load(rtn);
            ctx.ExecuteQuery();

            foreach (ListItem oListItem in rtn)
            {
                rtnID = oListItem.Id;
            }
            return rtnID;
        } // EndMethod: getListItemID

        
    } // EndClass: Query Assistants




    public class GeneralLogging
    {
        public static List<string> Output = new List<string>();
        /// Error Log Tracking
        public static void WriteExceptionToLog(Exception exception)
        {
            try
            {
                using (StreamWriter sr = System.IO.File.AppendText("Log.txt")) //new StreamWriter("result.txt", Encoding. ))
                {
                    sr.WriteLine("=>" + DateTime.Now + " " + " An Error occurred: " + exception.StackTrace + " Message: " + exception.Message + "\n\n");
                    sr.Flush();
                }
            }
            catch (Exception)
            {
                throw;
            }
        } // EndMethod: WriteExceptionToLog


        /// Status Log Tracking            
        public static void WriteStatusHistoryToLog()
        {
            try
            {
                using (StreamWriter sr = System.IO.File.AppendText("Log.txt"))
                {
                    sr.WriteLine("=================================================================================================");
                    foreach (string item in Output)
                    { sr.WriteLine("=>" + DateTime.Now + " " + item + "\n\n"); }
                    sr.WriteLine("=================================================================================================");
                    sr.Flush();
                }
            }
            catch (Exception)
            {
                throw;
            }
        } // EndMethod: WriteStatusToLog


    } // EndClass: General Logging


} // EndNamespace: SP Helpers






