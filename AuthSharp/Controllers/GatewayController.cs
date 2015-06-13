using AuthSharp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AuthSharp.Controllers
{
    public class GatewayController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: /Gateway/Ping
        /// <summary>
        /// 网关的心跳操作。
        /// 当网关上 WiFiDog 启动时，将访问此操作，确认当前服务器是否为验证服务器。
        /// 如果服务器没有返回“Pong”，则视为服务器无效， WiFiDog 不会设置任何防火墙规则。
        /// 如果服务器返回“Pong”，则视为服务器有效，WiFiDog 将会正常运行。
        /// WiFiDog 在运行时会间隔一定的时间发送心跳包（Ping），如果服务器正常回复则继续运行，如果服务器没有正常回复，
        /// 则会尝试服务器列表的下一个服务器。如果所有服务器都无法正常工作，WiFiDog 则会清除所有防火墙规则，
        /// 一直循环 Ping 服务器列表的所有服务器，直到有一个服务器正常回复才会继续运行。
        /// </summary>
        /// <param name="gw_id">网关 ID （数据库中为 GatewayName）。</param>
        /// <param name="sys_uptime">系统启动的时间。</param>
        /// <param name="sys_memfree">系统剩余内存。</param>
        /// <param name="sys_load">系统负荷。</param>
        /// <param name="wifidog_uptime"> WiFiDog 启动时间。</param>
        /// <returns>
        /// 返回值为 Pong。
        /// 如果返回值不为 Pong，则网关将不设置防火墙规则并定期重新发送心跳包。
        /// </returns>
        public string Ping(string gw_id, long sys_uptime, int sys_memfree, float sys_load, long wifidog_uptime)
        {
            // 此处心跳操作的实现，大致如下：
            // 1. 核对数据库中是否有相应的 GatewayName (gw_id)，无则返回错误，不允许网关上线；
            // 2. 将各种信息（启动时间，系统负载，内存空闲，最后一次心跳的时间）写入数据库。
            return "Pong";
        }
        
        /// <summary>
        /// 网关请求验证用户的操作。网关在收到请求时，即会访问此方法，
        /// 通过token判断用户是否能够上网。具体返回值及下一步操作见 returns 一节。
        /// </summary>
        /// <param name="stage">当前操作的类型：login 为用户首次登陆的验证；counter 为正常上网时的验证。</param>
        /// <param name="ip">用户相对于网关的 IP。</param>
        /// <param name="mac">用户的 MAC 地址。</param>
        /// <param name="token">用户在 /User/Login 操作时重定向传递给网关的令牌。</param>
        /// <param name="incoming">用户的总入站（下载）流量。</param>
        /// <param name="outgoing">用户的总出站（上传）流量。</param>
        /// <param name="incomingdelta">用户自上次网关执行 Auth 操作以来的入站流量。</param>
        /// <param name="outgoingdelta">用户自上次网关执行 Auth 操作以来的出站流量。</param>
        /// <returns>
        /// 返回值为 Auth: n
        /// 可能的 n 的值及对应网关动作的有：
        /// 0：用户已被禁止访问。网关将删除规则，用户将被视为新用户。用户将被定向到配置文件中 MsgScriptPathFragment 所指定的地址（在 AuthSharp 中为 /User/Message）。
        /// 1：允许用户访问。网关将允许用户访问网络（按照配置文件 known-users 一节来设定针对该用户的防火墙规则）并定期（配置文件中设定间隔）执行 Auth 操作。用户将被重定向至配置文件中 PortalScriptPathFragment 所指向的地址（在 WiFiDog 中为 /User/Portal）。
        /// 5：用户正在执行认证操作（可能的情况为用户正在使用外部授权提供程序登陆）。网关将按照配置文件 validating-users 一节来设定针对该用户的防火墙规则。
        /// 6：用户认证超时。网关将删除用户规则。
        /// -1：服务器错误。网关将无动作。
        /// </returns>
        public string Auth(string stage, string ip, string mac, string token, long incoming, long outgoing, long incomingdelta, long outgoingdelta)
        {
            Guid tokenGuid;
            if (!Guid.TryParse(token, out tokenGuid))
            {
                return "Auth: 0";
            }
            if (db.Tokens.Any(item => item.Token == tokenGuid))
            {
                UserToken tokenObject = db.Tokens.Single(item => item.Token == tokenGuid);
                tokenObject.UpdateTime = DateTime.Now;
                tokenObject.User.TrafficRemaining -= incomingdelta + outgoingdelta;
                db.SaveChanges();
                if (tokenObject.User.TrafficRemaining > 0)
                {
                    return "Auth: 1";
                }
            }
            return "Auth: 0";
        }
    }
}