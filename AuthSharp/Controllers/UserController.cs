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
        // GET: Login
        /// <summary>
        /// 当新用户通过网关希望上网时，网关会将用户重定向到此页面，
        /// 用户可以在此页面查看当前的状态，登录，单击按钮进行确认等。
        /// 当用户确认上网时，应当生成一个 token，并且被重定向至
        /// http://{网关 IP}:{网关端口}/wifidog/auth?token={随机生成的 token}，
        /// 然后网关将访问 /Gateway/Auth，进行进一步认证。
        /// </summary>
        /// <param name="gw_address">网关的 IP （对于用户）。</param>
        /// <param name="gw_port">网关 HTTP 服务器端口。</param>
        /// <param name="gw_id">网关 ID （数据库中为 GatewayName）。</param>
        /// <param name="url">用户正在尝试访问的 URL。</param>
        /// <returns>当用户成功登陆时重定向至网关（正式实现中有可能是一个确认上网页面）。</returns>
        [Authorize, HttpGet]
        public ActionResult Login(string gw_address, string gw_port, string gw_id, string url)
        {
            ViewBag.UserName = User.Identity.Name;
            return View();
        }

        [Authorize, HttpPost, ActionName("Login")]
        public ActionResult LoginConfirmed(string gw_address, string gw_port, string gw_id)
        {
            return Redirect(string.Format("http://{0}:{1}/wifidog/auth?token={2}", gw_address, gw_port, "Token_here"));
        }

        /// <summary>
        /// 当用户成功登陆(Auth: 1)时网关将用户重定向至此页面。可以显示一个按钮，将用户跳转到刚才正在尝试访问的 URL。
        /// </summary>
        /// <param name="gw_id">网关 ID。</param>
        /// <returns>向用户显示的视图。</returns>
        public string Portal(string gw_id)
        {
            return "Auth Succeedeed. Gateway ID is " + gw_id;
        }

        /// <summary>
        /// 当成功登陆以外的状态时网关将用户重定向至此页面，用于显示状态消息及进行下一步操作（如充值等）。
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