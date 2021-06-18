using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ObjectBuilder {
    public partial class ObjectBuilder : Form {

        public ObjectBuilder() {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e) {

        }

        private void btnBuild_Click(object sender, EventArgs e) {

            string tableName = this.txtTableName.Text.Trim();

            System.Collections.Generic.List<Column> columns;
            columns = Column.GetColumns(tableName);

            txtClass.Text = string.Empty;
            txtClass.Text = ClassBuilder.Build(tableName, columns);

            txtProcedure.Text = Procedure.GetProcedure(tableName, columns);

        }
    }
}