using LiteDB;
using RESI.Sorter;
using Season;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LH.Sorter.Util.LiteDB
{
    public class LiteDBHelper
    {
        private static object obj=new object();
        
        public static LiteDBHelper _instance = null;

        public static string DBNAME = "DEFAULT";
        public string PATH;
        public string fileName;
        public static LiteDBHelper Ins
        {
            get
            {
                lock (obj)
                {
                    if (_instance == null)
                    {
                        _instance = new LiteDBHelper();

                    }

                    return _instance;

                }
            }

        }
        public LiteDBHelper() {


        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbName">litedb数据库名称</param>
        /// <param name="path">数据库储存的位置</param>
        public void InitDB(string dbName,string path) {

            DBNAME = dbName;
            PATH = path;
            fileName = PATH + "\\LiteDB" + DBNAME+".db";

        }

        public LiteCollection<T> GetCollection<T>() {

            using (var db = new LiteDatabase(@fileName)) {
                Type type = typeof(T);
                
                string tableName = type.Name.ToString();
                var customers = db.GetCollection<T>(tableName);
              
                return customers;
            }

        }

        public bool Insert<T>(T t)
        {
            int res = 0;
            try
            {
                
                using (var db = new LiteDatabase(@fileName))
                {
                    Type type = typeof(T);
                    string tableName = type.Name.ToString();
                    var customers = db.GetCollection<T>(tableName);
                    
                     res=customers.Insert(t);
                    if (res > 0)
                    {
                        return true;
                    }
                    else {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {

                Log.Error("LiteDB-Insert-Error:{0}",ex);
                return false;
            }

        }

        public int Insert<T>(List<T> t)
        {
            int res = 0;
            try
            {

                using (var db = new LiteDatabase(@fileName))
                {
                    Type type = typeof(T);
                    string tableName = type.Name.ToString();
                    var customers = db.GetCollection<T>(tableName);

                     res = customers.Insert(t);
                    return res;
                }
            }
            catch (Exception ex)
            {

                Log.Error("LiteDB-InsertList-Error:{0}", ex);
                return res;
            }

        }


        public bool Update<T>(T t)
        {
            bool res = false;
            try
            {

                using (var db = new LiteDatabase(@fileName))
                {
                    Type type = typeof(T);
                    string tableName = type.Name.ToString();
                    var customers = db.GetCollection<T>(tableName);
                    res = customers.Update(t);
                    return res;
                }
            }
            catch (Exception ex)
            {

                Log.Error("LiteDB-Update-Error:{0}", ex);
                return res;
            }
           

        }

        public int Update<T>(List<T> t)
        {
            int res = 0;
            try
            {

                using (var db = new LiteDatabase(@fileName))
                {
                    Type type = typeof(T);
                    string tableName = type.Name.ToString();
                    var customers = db.GetCollection<T>(tableName);
                    res = customers.Update(t);
                    return res;
                    
                }
            }
            catch (Exception ex)
            {

                Log.Error("LiteDB-UpdateList-Error:{0}", ex);
                return res;
            }


        }
        public bool Delete<T>(BsonValue Id)
        {

            try
            {

                using (var db = new LiteDatabase(@fileName))
                {
                    Type type = typeof(T);
                    string tableName = type.Name.ToString();
                    var customers = db.GetCollection<T>(tableName);
                    bool res=customers.Delete(Id);
                    return res;
                }
            }
            catch (Exception ex)
            {

                Log.Error("LiteDB-Delete-Error:{0}", ex);
                return false;
            }

        }

        public int Delete<T>(Query query)
        {
            int res = 0;
            try
            {

                using (var db = new LiteDatabase(@fileName))
                {
                    Type type = typeof(T);
                    string tableName = type.Name.ToString();
                    var customers = db.GetCollection<T>(tableName);
                    res = customers.Delete(query);
                    return res;
                }
            }
            catch (Exception ex)
            {

                Log.Error("LiteDB-DeleteList-Error:{0}", ex);
                return res;
            }

        }

    }
}
