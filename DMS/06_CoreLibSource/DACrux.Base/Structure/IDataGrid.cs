using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;

namespace DACrux.Base
{
    /// <summary>
    /// 데이터를 DataGridView로 표현합니다.
    /// </summary>
    public interface IDataGrid
    {
        /// <summary>
        /// 데이터를 DataTable 형태로 가져옵니다.
        /// </summary>
        /// <returns></returns>
        DataTable ToDataTable();

        /// <summary>
        /// 데이터를 DataGridView 형태로 가져옵니다.
        /// </summary>
        DataGridView ToDataGridView();
    }
}
