using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using Wx_Dev.AppCode;
using Wx_Dev.Model;

namespace ShowStartWeb
{
    /// <summary>
    /// WxHandler 的摘要说明
    /// </summary>
    public class WxHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            if (context.Request.HttpMethod.ToLower() == "post")
            {
                if (CheckSignature(context))//出于安全考虑，在接收与回复时需要校验签名
                {
                    //接收消息并进行相应处理
                    ReceiveMessage(context);
                }
                else
                {
                    context.Response.Write("消息并非来自微信");
                    context.Response.End();
                }
            }
            else
            {
                CheckWechat(context);
            }
        }

        /// <summary>
        /// 接收消息并做相应处理
        /// </summary>
        /// <param name="context"></param>
        private void ReceiveMessage(HttpContext context)
        {
            string postString = string.Empty;
            string returnMessage = string.Empty;
            bool isEncrypt = false;

            try
            {
                string encrypt_type = context.Request.QueryString["encrypt_type"] ?? "";//判断是否存此参数，若存在则在传递消息时进行加解密操作
                if (!string.IsNullOrEmpty(encrypt_type))
                {
                    isEncrypt = true;
                }

                using (Stream stream = context.Request.InputStream)
                {
                    Byte[] postBytes = new Byte[stream.Length];
                    stream.Read(postBytes, 0, (Int32)stream.Length);
                    postString = Encoding.UTF8.GetString(postBytes);
                }

                if (!string.IsNullOrEmpty(postString))
                {
                    if (isEncrypt)
                    {
                        postString = DecryptPostData(context, postString);
                    }
                    XmlDocument requestXml = new XmlDocument();
                    requestXml.LoadXml(postString);
                    XmlElement rootElement = requestXml.DocumentElement;
                    if (rootElement == null) return;

                    MessageInfo messageInfo = new MessageInfo();
                    messageInfo.ToUserName = rootElement.SelectSingleNode("ToUserName").InnerText;
                    messageInfo.FromUserName = rootElement.SelectSingleNode("FromUserName").InnerText;
                    messageInfo.CreateTime = rootElement.SelectSingleNode("CreateTime").InnerText;
                    messageInfo.MsgType = rootElement.SelectSingleNode("MsgType").InnerText;

                    switch (messageInfo.MsgType.ToLower())
                    {
                        case "text"://文本消息
                            messageInfo.Content = rootElement.SelectSingleNode("Content").InnerText;
                            messageInfo.MsgId = rootElement.SelectSingleNode("MsgId").InnerText;
                            if (messageInfo.Content == "#获取openId#")
                            {
                                returnMessage = ResponseMessage.ReplyText(messageInfo.ToUserName, messageInfo.FromUserName, messageInfo.FromUserName);
                            }
                            else
                            {
                                //可以增加判断并且自定义返回消息
                            }
                            break;
                        case "event"://事件
                            messageInfo.Event = rootElement.SelectSingleNode("Event").InnerText;
                            if (messageInfo.Event.ToLower() == "templatesendjobfinish")//模版消息推送类型
                            {
                                messageInfo.MsgId = rootElement.SelectSingleNode("MsgID").InnerText;
                                messageInfo.Status = rootElement.SelectSingleNode("Status").InnerText;
                                //success   成功
                                //failed:user block 用户拒收
                                //failed: system failed 发送失败
                                log4net.ILog logger = LogManager.GetLogger();
                                logger.Info(Newtonsoft.Json.JsonConvert.SerializeObject(messageInfo));
                            }
                            else//其他事件消息
                            {
                                if (messageInfo.Event.ToLower() == "subscribe")//关注
                                {
                                    string[] welcomeText = new string[] { "在火炉旁，找个地方随便坐。",
                                    "欢迎回来，好久不见了。",
                                    "小伙子们，看看谁来了。",
                                    "嚯嚯嚯，很高兴见到你。",
                                    "欢迎来到我的酒馆。",
                                        "哈，快找个位置坐下来。",
                                    "今晚累得够呛，但只要有客人来玩，我都欢迎。",
                                    "把你冰冷的靴子脱下来烘烘热！",
                                    "欢迎！小伙子们！在火炉旁挤挤！",
                                    "你看到外面的鬼天气了吗！？还是这里暖和！"};
                                    Random rd = new Random();
                                    int num = rd.Next(0, welcomeText.Length - 1);
                                    returnMessage = ResponseMessage.ReplyText(messageInfo.ToUserName, messageInfo.FromUserName, welcomeText[num]);
                                    log4net.ILog logger = LogManager.GetLogger();
                                    logger.Error(Newtonsoft.Json.JsonConvert.SerializeObject(messageInfo));
                                }
                                else if (messageInfo.Event.ToLower() == "unsubscribe")//取消关注
                                {
                                    //记录事件，并进行解绑以及其他操作
                                    log4net.ILog logger = LogManager.GetLogger();
                                    logger.Error(Newtonsoft.Json.JsonConvert.SerializeObject(messageInfo));
                                }
                                else//按钮事件
                                {
                                    messageInfo.EventKey = rootElement.SelectSingleNode("EventKey").InnerText;
                                    if (messageInfo.EventKey == "clt")
                                    {
                                        string[] welcomeText = new string[] { "我可没时间陪你玩游戏", "信仰圣光吧", "并非所有流浪者都迷失了自我", "暴风城的将士听我号令", "让火焰净化一切！" };
                                        Random rd = new Random();
                                        int num = rd.Next(0, welcomeText.Length - 1);
                                        returnMessage = ResponseMessage.ReplyText(messageInfo.ToUserName, messageInfo.FromUserName, welcomeText[num]);
                                    }
                                    else if (messageInfo.EventKey == "http://nothing.cn/")
                                    {

                                        string[] welcomeText = new string[] { "nothing is impossible", "nothing gonna change my love for you", "nothing in the world", "nothing to fear", "nothing but.." };
                                        Random rd = new Random();
                                        int num = rd.Next(0, welcomeText.Length - 1);
                                        returnMessage = ResponseMessage.ReplyText(messageInfo.ToUserName, messageInfo.FromUserName, welcomeText[num]);
                                    }
                                }
                            }
                            break;
                        case "image"://图片消息
                        case "voice"://语音消息
                        case "video"://视频消息
                        case "shortvideo"://小视频消息
                        case "location"://地理位置消息
                        case "link"://链接消息
                        default:
                            messageInfo.MsgId = rootElement.SelectSingleNode("MsgId").InnerText;
                            returnMessage = ResponseMessage.ReplyText(messageInfo.ToUserName, messageInfo.FromUserName, "对不起，本店没有这项业务。");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                log4net.ILog logger = LogManager.GetLogger();
                logger.Error(postString, ex);
                returnMessage = "";
            }
            ResponseData(context, returnMessage, isEncrypt);
        }
        /// <summary>
        /// 返回数据
        /// </summary>
        /// <param name="context"></param>
        /// <param name="responseData"></param>
        /// <param name="isEncrypt">是否加密</param>
        private void ResponseData(HttpContext context, string responseData, bool isEncrypt)
        {
            if (string.IsNullOrEmpty(responseData.Trim()))
            {
                responseData = "success";
            }
            if (isEncrypt)
            {
                responseData = EncryptMsg(context, responseData);
            }
            context.Response.Write(responseData);//不做处理时需要返回success或空字符串，不然微信默认服务出错
            context.Response.End();
        }

        #region 验证微信签名
        /// <summary>
        /// 返回随机数表示验证成功
        /// </summary>
        private void CheckWechat(HttpContext context)
        {
            if (string.IsNullOrEmpty(context.Request.QueryString["echoStr"]))
            {
                context.Response.Write("消息并非来自微信");
                context.Response.End();
            }
            string echoStr = context.Request.QueryString["echoStr"].ToString() ?? "";
            if (CheckSignature(context))
            {
                context.Response.Write(echoStr);
                context.Response.End();
            }
        }

        /// <summary>
        /// 验证微信签名
        /// </summary>
        /// <returns></returns>
        /// * 将token、timestamp、nonce三个参数进行字典序排序
        /// * 将三个参数字符串拼接成一个字符串进行sha1加密
        /// * 开发者获得加密后的字符串可与signature对比，标识该请求来源于微信。
        private bool CheckSignature(HttpContext context)
        {
            string token = ConfigurationManager.AppSettings["token"].ToString();
            string signature = context.Request.QueryString["signature"].ToString() ?? "";
            string timestamp = context.Request.QueryString["timestamp"].ToString() ?? "";
            string nonce = context.Request.QueryString["nonce"].ToString() ?? "";
            //string echostr = context.Request.QueryString["echostr"].ToString() ?? "";

            string[] ArrTmp = { token, timestamp, nonce };
            Array.Sort(ArrTmp);     //字典排序
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");

            if (tmpStr.ToLower() == signature)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region 加解密数据
        /// <summary>
        /// 数据解密
        /// </summary>
        /// <param name="context"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        private string DecryptPostData(HttpContext context, string postData)
        {
            string sMsg = "";  //解析之后的明文
            try
            {
                string sAppID = ConfigurationManager.AppSettings["appID"].ToString();
                string sToken = ConfigurationManager.AppSettings["token"].ToString();
                string sEncodingAESKey = ConfigurationManager.AppSettings["EncodingAESKey"].ToString();

                string sReqMsgSig = context.Request.QueryString["msg_signature"].ToString() ?? "";
                string sReqTimeStamp = context.Request.QueryString["timestamp"].ToString() ?? "";
                string sReqNonce = context.Request.QueryString["nonce"].ToString() ?? "";
                string sReqData = postData;

                Tencent.WXBizMsgCrypt wxcpt = new Tencent.WXBizMsgCrypt(sToken, sEncodingAESKey, sAppID);
                int ret = 0;
                ret = wxcpt.DecryptMsg(sReqMsgSig, sReqTimeStamp, sReqNonce, sReqData, ref sMsg);
                if (ret != 0)
                {
                    log4net.ILog logger = LogManager.GetLogger();
                    logger.Error("校验数据失败，错误代码：" + ret);
                }
            }
            catch (Exception ex)
            {
                log4net.ILog logger = LogManager.GetLogger();
                logger.Error("解密数据失败", ex);
            }
            return sMsg;
        }
        /// <summary>
        /// 加密消息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private string EncryptMsg(HttpContext context, string msg)
        {
            string sMsg = "";  //解析之后的明文
            try
            {
                string sAppID = ConfigurationManager.AppSettings["appID"].ToString();
                string sToken = ConfigurationManager.AppSettings["token"].ToString();
                string sEncodingAESKey = ConfigurationManager.AppSettings["EncodingAESKey"].ToString();

                string sReqMsgSig = context.Request.QueryString["msg_signature"].ToString() ?? "";
                string sReqTimeStamp = context.Request.QueryString["timestamp"].ToString() ?? "";
                string sReqNonce = context.Request.QueryString["nonce"].ToString() ?? "";
                string sRespData = msg;

                Tencent.WXBizMsgCrypt wxcpt = new Tencent.WXBizMsgCrypt(sToken, sEncodingAESKey, sAppID);
                int ret = 0;
                ret = wxcpt.EncryptMsg(sRespData, sReqTimeStamp, sReqNonce, ref sMsg);
            }
            catch (Exception ex)
            {
                log4net.ILog logger = LogManager.GetLogger();
                logger.Error("加密数据失败", ex);
            }
            return sMsg;
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}