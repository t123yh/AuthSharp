using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;

namespace AuthSharp.Controllers
{
    public class UserController : Controller
    {
        // GET: UserAuth
        /// <summary>
        /// 新用户用户将被网关重定向到此页面。
        /// </summary>
        /// <param name="gw_address">网关的 IP （对于用户）。</param>
        /// <param name="gw_port">网关 HTTP 服务器端口。</param>
        /// <param name="gw_id">网关 ID （数据库中为 GatewayName）。</param>
        /// <returns>当用户成功登陆时重定向至网关（正式实现中有可能是一个确认上网页面）。</returns>
        [Authorize]
        public ActionResult Login(string gw_address, string gw_port, string gw_id)
        {
            return Redirect(string.Format("http://{0}:{1}/wifidog/auth?token={2}", gw_address, gw_port, "Token_here"));
        }

        /// <summary>
        /// 当用户成功登陆时网关将用户重定向至此页面(Auth: 1)。
        /// </summary>
        /// <param name="gw_id">网关 ID。</param>
        /// <returns>向用户显示的视图。</returns>
        public string Portal(string gw_id)
        {
            return "Auth Succeedeed. Gateway ID is " + gw_id;
        }

        /// <summary>
        /// 当成功登陆以外的状态时网关将用户重定向至此页面。
        /// </summary>
        /// <param name="message">
        /// 未成功登陆的原因：
        /// denied: 用户被禁止访问（Auth: 0）。
        /// activate: 用户正在执行认证操作（Auth: 5）。
        /// failed_validation: 用户认证超时（Auth: 6）。
        /// </param>
        /// <returns></returns>
        public string Message(string message)
        {
            return "Message: " + message;
        }
    }
}