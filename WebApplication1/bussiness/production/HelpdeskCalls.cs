using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Net.Mail;
using System.Net;

namespace WebApplication1.bussiness.production
{
    public class HelpdeskCalls
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();

        public bool InsertHelpDeskTicket(string ticketId, string createdByWorkman, string createdByName, string creatorRegion, string creatorComp,
        string root1Value, string root2Value, string root3Value, string priorityLevel, string description, string status)
        {
            bool isInsertSuccessful = false;
            try
            {
                dbcl.Sqlconnection();
                using (SqlConnection connection = dbcl.Conn)
                {
                    using (SqlCommand command = new SqlCommand("usp_InsertHelpDeskTicket", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters to the stored procedure
                        command.Parameters.AddWithValue("@ticket_id", ticketId);
                        //command.Parameters.AddWithValue("@CreatedOn", createdOn);
                        command.Parameters.AddWithValue("@CreatedByWorkman", createdByWorkman);
                        command.Parameters.AddWithValue("@CreatedByName", createdByName);
                        command.Parameters.AddWithValue("@CreatorRegion", creatorRegion);
                        command.Parameters.AddWithValue("@CreatorComp", creatorComp);
                        command.Parameters.AddWithValue("@root1_value", root1Value);
                        command.Parameters.AddWithValue("@root2_value", root2Value);
                        command.Parameters.AddWithValue("@root3_value", root3Value);
                        command.Parameters.AddWithValue("@priority_level", priorityLevel);
                        command.Parameters.AddWithValue("@description", description);
                        command.Parameters.AddWithValue("@status", status);
                        //command.Parameters.AddWithValue("@Date_Open", dateOpen);
                        //command.Parameters.AddWithValue("@Date_Closed", dateClosed);
                        //command.Parameters.AddWithValue("@AssignedTo", assignedTo);
                        //command.Parameters.AddWithValue("@AssignedOn", assignedOn);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        isInsertSuccessful = rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return isInsertSuccessful;
        }


        public void SendEmail(string hlpdskid, string createdByWorkman, string createdByName, string creatorRegion, string creatorComp, string root1Value, string root2Value, string root3Value, string priorityLevel, string description)
        {
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587;
            string smtpUsername = "it_helpdesk@atswork.in";
            string smtpPassword = "wpdcbssoxcfovwmj";

            try
            {
                using (SmtpClient client = new SmtpClient(smtpServer, smtpPort))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    client.EnableSsl = true;

                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress(smtpUsername);
                        // Add recipient email addresses
                        mail.To.Add("kaushik@atswork.in");
                        mail.To.Add("anupam.sharma@atswork.in");
                        // Set email subject and body
                        mail.Subject = "Grivance Ticket Created # "+ hlpdskid + " with Priority Level : " + priorityLevel + "!";
                        //mail.Body = $"Ticket ID: {hlpdskid}\n\nCreated By: {createdByName}[{createdByWorkman}]\nWorking At:{creatorRegion}[{creatorComp}]\n\nGrievances Category: {root1Value}\nSupport Topic: {root2Value}\n\nDetailed Description:{description}\n\n\n\nThank You\nATS Portal";

                        // Use HTML formatting in the email body
                        mail.Body = $@"<html>
                                <body>
                                    <p><strong>Ticket ID:</strong> {hlpdskid}</p>
                                    <p><strong>Created By:</strong> {createdByName}</p>
                                    <p><strong>Working At:</strong> {creatorComp}</p>
                                    <p><strong>Grievance Category:</strong> {root1Value}</p>
                                    <p><strong>Support Topic:</strong> {root2Value}</p>
                                    <p></p>
                                    <p><strong>Detailed Description:</strong> {description}</p>
                                    <p></p>
                                    <p></p>
                                    <p>Thank You</p>
                                    <!-- Add other formatted content here -->
                                </body>
                            </html>";
                        mail.IsBodyHtml = true;

                        client.Send(mail);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}