using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebBotRecognition.Controllers
{
    /// <summary>
    /// 识别
    /// </summary>
    [Route("api/[controller]")]
    public class RecognizeController : ControllerBase
    {
        /// <summary>
        /// 获取UserAgent
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserAgent")]
        public async Task<JsonResult> GetUserAgent()
        {
            return new JsonResult(null);
        }

        
    }
}