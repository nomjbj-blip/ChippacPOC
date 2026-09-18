using System;

namespace DACrux.MESIF.DSL
{
    public class MES_MRCPRCPARS : Miracom.Middleware.QueryComponent
    {
        #region Class Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public MES_MRCPRCPARS()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["MES_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "MES_MRCPRCPARS.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
               
        // ----------------------------------------------------------------------------------------------------------------------

        public System.Data.DataTable SELECT_MES_RECIPE(
            string factory, 
            string mat_id,
            string oper,
            string resModel
            )
        {
            try
            {
                return this.GetDataTable("SELECT_MES_RECIPE", null, new string[] { factory, mat_id, oper, resModel });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // ----------------------------------------------------------------------------------------------------------------------

        public System.Data.DataTable SELECT_MES_RECIPE_LIST(
            string factory,
            string mat_id,
            string oper,
            string resModel
            )
        {
            try
            {
                return this.GetDataTable("SELECT_MES_RECIPE_LIST", null, new string[] { factory, mat_id, oper, resModel });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // ----------------------------------------------------------------------------------------------------------------------

        public System.Data.DataTable SELECT_MES_RECIPE_LIST(
            string factory,
            string mat_id,
            string oper,
            string resModel,
            string searchRecipe
            )
        {
            try
            {
                if (searchRecipe == string.Empty)
                {
                    return this.GetDataTable("SELECT_MES_RECIPE_LIST", null, new string[] { factory, mat_id, oper, resModel });
                }
                return this.GetDataTable("SELECT_MES_RECIPE_LIST_02", null, new string[] { factory, mat_id, oper, resModel, searchRecipe });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // ----------------------------------------------------------------------------------------------------------------------

        public System.Data.DataTable SELECT_MES_RECIPEID_LIST(
            string factory,
            string mat_id,
            string oper,
            string resModel
            )
        {
            try
            {
                return this.GetDataTable("SELECT_MES_RECIPEID_LIST", null, new string[] { factory, mat_id, oper, resModel });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // ----------------------------------------------------------------------------------------------------------------------

        public System.Data.DataTable SELECT_MES_RECIPE_VERSION(
            string factory,
            string mat_id,
            string oper,
            string resModel,
            string recipe
            )
        {
            try
            {
                return this.GetDataTable("SELECT_MES_RECIPE_VERSION", null, new string[] { factory, mat_id, oper, resModel, recipe });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // ----------------------------------------------------------------------------------------------------------------------
    }
}
