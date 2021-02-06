using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WeatherAnalyser
{
    public partial class Home : System.Web.UI.Page
    {
        SQL sql = new SQL();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Sign_Up_Click(object sender, EventArgs e)
        {
            try
            {
                sql.InsertData("Users", new string[] { "Name", "Address", "Location", "PhNo", "SerialNo" }, new string[] { Name.Text, Address.Text, Location.Text, PhNo.Text, Ser.Text });
                resultclass.success = true;
            }
            catch(Exception ex)
            {
                resultclass.ErrorString = ex.Message;
            }
            Response.Redirect("Result.aspx");
        }
    }
}