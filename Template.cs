using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace FlexAmerica.Class {

    public class Template : DBObject {

        #region Private Properties
        //Private Properties
        #endregion

        #region Public Accessors
        //Public Accessors
        #endregion

        #region Constructors
        public static Template GetById(int Id) {
            Template o = new Template();
            o = ((Template)DBObject.GetById(Id, o.GetType()));
            return o;
        }
        #endregion 

        #region Methods
   
        #endregion 

    }


}
