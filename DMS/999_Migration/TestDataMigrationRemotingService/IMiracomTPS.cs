using System.Data;
using System.Collections.Generic;

namespace TestDataMigrationRemotingService
{
    public interface IMiracomTPS
    {
        void InsertBulk(DataTable dt);
        void ExecuteTable(DataTable schemaTable, List<object[]> dataList);
        string Sysdate();
    }
}
