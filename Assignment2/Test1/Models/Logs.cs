using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Test1.Models
{
    public class LogEntry
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public DateTime TimeCreated { get; set; }
    }

    public class LogProcessor
    {
        IRepository _repository;
        public LogProcessor(IRepository repository)
        {
            _repository = repository;
        }

        public Task<LogEntry[]> GetLogs()
        {
            return _repository.GetLogs();
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class LogController
    {
        LogProcessor _processor;
        public LogController(LogProcessor processor)
        {
            _processor = processor;
        }

        [HttpGet]
        public Task<LogEntry[]> GetLogs()
        {
            return _processor.GetLogs();
        }

    }
}