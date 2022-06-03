using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KyowonToy.lib.DataBase
{
    public class MysqlDapperHelper : IDisposable
    {
        MySqlConnection conn;
        MySqlTransaction trans;
        private bool disposedValue;

        public MysqlDapperHelper()
        {
            conn = new MySqlConnection("Server = 127.0.0.1; Port = 3306; Database = kyowontoy; Uid = root; Pwd = root;");
            // Allow User Variables=true;
        }

        public void BeguinTransaction()
        {
            trans = conn.BeginTransaction();
            trans = null;
        }

        public void Commit()
        {
            trans.Commit();
            trans = null;
        }

        public void RollBack()
        {
            trans.Rollback();

        }

        public List<T> Query<T>(string sql, object param)
        {

            return Dapper.SqlMapper.Query<T>(conn, sql, param, trans).ToList();

        }

  
        public T QuerySingle<T>(string sql, object param)
        {
            return Dapper.SqlMapper.QuerySingleOrDefault<T>(conn, sql, param, trans);
        }

        public T QuerySingle<T>(string sql, object param, object param2)
        {
            return Dapper.SqlMapper.QuerySingleOrDefault<T>(conn, sql, param, trans);
        }

        public int Execute(string sql, object param)
        {

            return Dapper.SqlMapper.Execute(conn, sql, param);

        }



        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리형 상태(관리형 개체)를 삭제합니다.
                    conn.Dispose(); // 메모리 해제

                    if(trans != null)
                    {
                        trans.Rollback();
                        trans.Dispose();
                    }
                }

                // TODO: 비관리형 리소스(비관리형 개체)를 해제하고 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                disposedValue = true;
            }
        }

   

  

        // // TODO: 비관리형 리소스를 해제하는 코드가 'Dispose(bool disposing)'에 포함된 경우에만 종료자를 재정의합니다.
        // ~MysqlDapperHelper()
       
        // {
        //     // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
        //     Dispose(disposing: false);
      
        // }

        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

}
