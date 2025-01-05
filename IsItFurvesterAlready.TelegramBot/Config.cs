using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Dates
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

public class Config
{
    public Dates? Dates { get; set; }
    public string? TelegramBotToken { get; set; }
    public int ThrottleTimeOutSeconds { get; set; }
}