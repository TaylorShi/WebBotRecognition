using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UAParser;

namespace WebBotRecognition.Controllers
{
    /// <summary>
    /// 识别
    /// </summary>
    [Route("api/[controller]")]
    public class RecognizeController : ControllerBase
    {
        /// <summary>
        /// 获取并解析代理字符串
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserAgent")]
        public async Task<JsonResult> GetUserAgent()
        {
            // 定义解析结果信息对象
            ClientInfo clientInfo = null;

            // 尝试从头部里面获取User-Agent字符串
            if(Request.Headers.TryGetValue("User-Agent", out var requestUserAgent) && !string.IsNullOrEmpty(requestUserAgent))
            {
                // 获取UaParser实例
                var uaParser = Parser.GetDefault();

                // 解析User-Agent字符串
                clientInfo = uaParser.Parse(requestUserAgent);
            }

            return new JsonResult(clientInfo);
        }

        /// <summary>
        /// 获取真实客户端来源Ip
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetRealClientIpAddress")]
        public async Task<JsonResult> GetRealClientIpAddress()
        {
            // 通过Connection的RemoteIpAddress获取IPV4的IP地址
            var requestIp = Request.HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
            return new JsonResult(requestIp);
        }

        /// <summary>
        /// 获取rDNS反向域名
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetrDnsHostName")]
        public async Task<JsonResult> GetrDnsHostName(string requestIp)
        {
            if(string.IsNullOrEmpty(requestIp))
            {
                // 通过Connection的RemoteIpAddress获取IPV4的IP地址
                requestIp = Request.HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
            }

            IPHostEntry iPHostEntry;
            try
            {
                // 根据真实请求Ip反向解析请求Ip的域名
                iPHostEntry = await Dns.GetHostEntryAsync(requestIp);
            }
            catch
            {
                iPHostEntry = null;
            }

            // 获取请求的域名地址
            var requestHostName = iPHostEntry?.HostName;
            
            return new JsonResult(requestHostName);
        }

        /// <summary>
        /// 是否来自真实的爬虫机器人
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("IsRealSpiderBot")]
        public async Task<bool> IsRealSpiderBot(string requestIp)
        {
            // 定义解析结果信息对象
            ClientInfo clientInfo = null;

            // 尝试从头部里面获取User-Agent字符串
            if(Request.Headers.TryGetValue("User-Agent", out var requestUserAgent) && !string.IsNullOrEmpty(requestUserAgent))
            {
                // 获取UaParser实例
                var uaParser = Parser.GetDefault();

                // 解析User-Agent字符串
                clientInfo = uaParser.Parse(requestUserAgent);
            }

            if(string.IsNullOrEmpty(requestIp))
            {
                // 通过Connection的RemoteIpAddress获取IPV4的IP地址
                requestIp = Request.HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
            }

            IPHostEntry iPHostEntry;
            try
            {
                // 根据真实请求Ip反向解析请求Ip的域名
                iPHostEntry = await Dns.GetHostEntryAsync(requestIp);
            }
            catch
            {
                iPHostEntry = null;
            }

            // 获取请求的域名地址
            var requestHostName = iPHostEntry?.HostName;

            // 当反向解析域名不为空且UserAgent解析结果为爬虫时，才真正为爬虫机器人
            var isRealSpiderBot = !string.IsNullOrEmpty(requestHostName) && (clientInfo?.Device?.IsSpider ?? false);
            return isRealSpiderBot;
        }
    }
}