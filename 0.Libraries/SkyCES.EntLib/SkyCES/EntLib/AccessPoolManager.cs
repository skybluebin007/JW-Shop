namespace SkyCES.EntLib
{
    using System;
    using System.Collections.Generic;
    using System.Data.OleDb;

    public class AccessPoolManager
    {
        private static string connectionString = string.Empty;
        private static int maxPools = 200;
        private static Dictionary<int, AccessPool> pools = new Dictionary<int, AccessPool>();
        private static int timeOut = 300;

        public static AccessPool Instance(string connString)
        {
            lock (typeof(AccessPool))
            {
                foreach (KeyValuePair<int, AccessPool> pair in pools)
                {
                    if (!pair.Value.IsUsing)
                    {
                        pair.Value.IsUsing = true;
                        pair.Value.StartTime = DateTime.Now;
                        return pair.Value;
                    }
                }
                OleDbConnection connection = new OleDbConnection(connString);
                int id = pools.Count + 1;
                AccessPool pool = new AccessPool(id, true, DateTime.Now, connection);
                if (pools.Count < maxPools)
                {
                    pools.Add(pool.ID, pool);
                }
                pool.Connection.Open();
                return pool;
            }
        }

        public class AccessPool : IDisposable
        {
            private OleDbConnection connection;
            private int id = 0;
            private bool isUsing = false;
            private DateTime startTime;

            public AccessPool(int id, bool isUsing, DateTime startTime, OleDbConnection connection)
            {
                this.id = id;
                this.isUsing = isUsing;
                this.startTime = startTime;
                this.connection = connection;
            }

            public void Dispose()
            {
                if (AccessPoolManager.pools.ContainsKey(this.id) && (AccessPoolManager.pools[this.id] != null))
                {
                    AccessPoolManager.AccessPool pool = AccessPoolManager.pools[this.id];
                    pool.IsUsing = false;
                    TimeSpan span = (TimeSpan) (DateTime.Now - pool.StartTime);
                    if (span.TotalSeconds > AccessPoolManager.timeOut)
                    {
                        pool.Connection.Close();
                        AccessPoolManager.pools.Remove(this.id);
                    }
                }
            }

            public OleDbConnection Connection
            {
                get
                {
                    return this.connection;
                }
                set
                {
                    this.connection = value;
                }
            }

            public int ID
            {
                get
                {
                    return this.id;
                }
                set
                {
                    this.id = value;
                }
            }

            public bool IsUsing
            {
                get
                {
                    return this.isUsing;
                }
                set
                {
                    this.isUsing = value;
                }
            }

            public DateTime StartTime
            {
                get
                {
                    return this.startTime;
                }
                set
                {
                    this.startTime = value;
                }
            }
        }
    }
}

