using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Test1.Models
{
    public class UserActionAuditFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (!context.Canceled)
            {
                IRepository _repository = (IRepository) context.HttpContext.RequestServices.GetService(typeof(IRepository));

                string ipAddress = context.HttpContext.Request.Host.Value;
                DateTime currentTime = DateTime.Now;

                LogEntry entry = new LogEntry();
                entry.Id = Guid.NewGuid();
                entry.TimeCreated = currentTime;
                entry.Message = "A Request from ip address " + ipAddress + " to delete player ended at " + currentTime.ToString();
            
                _repository.WriteLog(entry);

            }
        }

         public override void OnActionExecuting(ActionExecutingContext context)
         {
                IRepository _repository = (IRepository) context.HttpContext.RequestServices.GetService(typeof(IRepository));

                string ipAddress = context.HttpContext.Request.Host.Value;
                DateTime currentTime = DateTime.Now;

                LogEntry entry = new LogEntry();
                entry.Id = Guid.NewGuid();
                entry.TimeCreated = currentTime;
                entry.Message = "A Request from ip address " + ipAddress + " to delete player started at " + currentTime.ToString();
            
                _repository.WriteLog(entry);

         }

    }

}