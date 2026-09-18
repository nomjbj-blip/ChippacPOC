using System.Data;

namespace TestDataMigration_RemotingService
{
    public interface IMiracomTPS
    {
        void InsertBulk(DataTable dt);
        string Sysdate();
    }
}
