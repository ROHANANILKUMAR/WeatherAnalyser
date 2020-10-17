using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WeatherAnalyser
{
    public partial class Result : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (resultclass.success)
            {
                Response.Text = "You have been registered successfully";
                Response.ForeColor = System.Drawing.Color.Green;

            }
            else
            {
                Response.Text = "You opps there was an error";
                Error.Text = resultclass.ErrorString;
                Response.ForeColor = System.Drawing.Color.Green;
            }
        }
    }
}