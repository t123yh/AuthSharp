using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;

namespace AuthSharp.Controllers
{
    public class GatewayController : Controller
    {

        // GET: /Gateway/Ping
        /// <summary>
        /// 网关的心跳操作。
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
        /// 网关请求验证用户的操作。
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
        /// 1：允许用户访问。网关将允许用户访问网络（按照配置文件 known-users 一节来设定针对该用户的防火墙规则）并定期（配置文件中设定间隔）执行 Auth 操作。
        /// 5：用户正在执行认证操作（可能的情况为用户正在使用外部授权提供程序登陆）。网关将按照配置文件 validating-users 一节来设定针对该用户的防火墙规则。
        /// 6：用户认证超时。网关将删除用户规则。
        /// -1：服务器错误。网关将无动作。
        /// </returns>
        public string Auth(string stage, string ip, string mac, string token, long incoming, long outgoing, long incomingdelta, long outgoingdelta)
        {
            // 此处验证操作大致如下：
            // 1. 使用 token 找到当前用户并扣除流量；
            // 2. 判断用户流量是否足够来决定用户是否能够继续上网。
            return "Auth: 1";
        }
    }
}