#r "System.Data"

using System.Net;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Linq;
using System.Data.Entity;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info("C# HTTP trigger function processed a request.");
    
    dynamic body = await req.Content.ReadAsStringAsync();
    var e = JsonConvert.DeserializeObject<Person>(body as string);

    try
    {        
        using (var context = new DbContext("XXX"))
        {
            context.Database.Connection.Open();
            context.Database.ExecuteSqlCommand(string.Format("INSERT INTO [dbo].[SubscriberDemo] ([FirstName_VC],[LastName_VC], [Email_VC],[Created_DT])" +  
                    "VALUES ('{0}', '{1}', '{2}',GETDATE()) ", e.firstname,e.lastname, e.email));
            context.Database.Connection.Close();
        }
    }
    catch(System.Data.Entity.Infrastructure.DbUpdateException ex)
    {
        log.Info(string.Format("Failure with database update {0}.", ex.Message));        
    }  
    

    return req.CreateResponse(HttpStatusCode.OK, "Ok");
}

public class Person{
    public string firstname {get;set;}
    public string lastname {get;set;}
    public string email {get;set;}
}