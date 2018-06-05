using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
//using Aliyun.Acs.Sms.Model.V20160927;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Holidays.Web.Code
{
    public static class AliyunSMS
    {
        // Access Key ID
        private const string AccessKeyID = "LTAITDomJgeG5riT";

        // Access Key Secret
        private const string AccessKeySecret = "iYwPd6qRAixedAv8AGzTF4UsxQPtht";

        /// <summary>
        /// 用户注册验证码
        /// </summary>
        private const string SMS_33715408 = "SMS_33715408";

        /// <summary>
        /// 登录确认验证码
        /// </summary>
        private const string SMS_33715410 = "SMS_33715410";

        /// <summary>
        /// 订单通知
        /// </summary>
        private const string SMS_50660003 = "SMS_50660003";

        /// <summary>
        /// 顾客订单通知
        /// </summary>
        private const string SMS_132996357 = "SMS_132996357";

        /// <summary>
        /// 短信API产品名称
        /// </summary>
        private const string product = "Dysmsapi";

        /// <summary>
        /// 短信API产品域名
        /// </summary>
        private const string domain = "dysmsapi.aliyuncs.com";

        private const string regionIdForPop = "cn-hangzhou";

        /// <summary>
        /// 发送短信
        /// add by fruitchan
        /// update liuyu 2018/03/19
        /// 2016-12-18 00:12:30
        /// </summary>
        /// <param name="recNum">目标手机号，多个手机号可以逗号分隔</param>
        /// <param name="templateCode">管理控制台中配置的审核通过的短信模板的模板CODE（状态必须是验证通过）</param>
        /// <param name="paramString">短信模板中的变量；数字需要转换为字符串；个人用户每个变量长度必须小于15个字符。 例如:短信模板为：“接受短信验证码${no}”,此参数传递{“no”:”123456”}，用户将接收到[短信签名]接受短信验证码123456</param>
        /// <returns>发送结果</returns>
        private static string SendSMS(string recNum, string templateCode, string paramString)
        {
            IClientProfile profile = DefaultProfile.GetProfile(regionIdForPop, AccessKeyID, AccessKeySecret);
            DefaultProfile.AddEndpoint(regionIdForPop, regionIdForPop, product, domain);
            IAcsClient client = new DefaultAcsClient(profile);
            // SingleSendSmsRequest request = new SingleSendSmsRequest();
            SendSmsRequest request = new SendSmsRequest();
            try
            {
                request.SignName = "我要去度假";
                request.TemplateCode = templateCode;
                //request.RecNum = recNum;
                //request.ParamString = paramString;
                //SingleSendSmsResponse httpResponse = client.GetAcsResponse(request);
                request.PhoneNumbers = recNum;
                request.TemplateParam = paramString;
                SendSmsResponse httpResponse = client.GetAcsResponse(request);
                if (httpResponse.HttpResponse.Status == 200)
                {
                    return "ok";
                }
            }
            catch (ServerException se)
            {
                return "发送短信频繁，请1小时后再试！";
            }
            catch (ClientException ce)
            {
                return "发送短信频繁，请1小时后再试！";
            }

            return "发送短信失败！";
        }

        /// <summary>
        /// 发送注册短信
        /// add by fruitchan
        /// 2016-12-18 14:04:10
        /// </summary>
        /// <param name="recNum">目标手机号，多个手机号可以逗号分隔</param>
        /// <param name="code">验证码</param>
        /// <returns>发送短信结果（ok：成功）</returns>
        public static string SendRegisterSMS(string recNum, string code)
        {
            return SendSMS(recNum, SMS_33715408, JsonConvert.SerializeObject(new { code = code, product = "我要去度假" }));
        }

        /// <summary>
        /// 发送登录授权短信
        /// add by fruitchan
        /// 2016-12-18 14:07:16
        /// </summary>
        /// <param name="recNum">目标手机号，多个手机号可以逗号分隔</param>
        /// <param name="code">验证码</param>
        /// <returns>发送短信结果（ok：成功）</returns>
        public static string SendLoginAuthSMS(string recNum, string code)
        {
            return SendSMS(recNum, SMS_33715410, JsonConvert.SerializeObject(new { code = code, product = "我要去度假" }));
        }

        /// <summary>
        /// 发送订单短信
        /// add by fruitchan
        /// 2017-3-2 08:40:48
        /// </summary>
        /// <param name="recNum">目标手机号，多个手机号可以逗号分隔</param>
        /// <param name="houseTitle">房源标题</param>
        /// <param name="startTime">入住时间</param>
        /// <param name="endTime">退房时间</param>
        /// <param name="numOfPeople">入住人数</param>
        /// <param name="username">联系人姓名</param>
        /// <param name="phoneNo">联系人电话</param>
        /// <returns>发送短信结果（ok：成功）</returns>
        public static string SendOrderSMS(string recNum, string houseTitle, string startTime, string endTime, string numOfPeople, string username, string phoneNo)
        {
            return SendSMS(recNum, SMS_50660003, JsonConvert.SerializeObject(new { houseTitle = houseTitle, startTime = startTime, endTime = endTime, numOfPeople = numOfPeople, username = username, phoneNo = phoneNo }));
        }

        /// <summary>
        /// 发送顾客订单短信
        /// </summary>
        /// <param name="recNum">目标手机号，多个手机号可以逗号分隔</param>
        /// <param name="username">顾客名称</param>
        /// <param name="starttime">入住时间</param>
        /// <param name="endtime">退房时间</param>
        /// <param name="shopname">店铺名称</param>
        /// <param name="numOfPeople">入住人数</param>
        /// <param name="housetitle">房源标题</param>
        /// <param name="housecode">预定码</param>
        /// <param name="phoneno">商家电话</param>
        /// <returns></returns>
        public static string SendOrderSMSByUser(string recNum, string username, string starttime, string endtime, string shopname, string numOfPeople, string housetitle, string housecode, string phoneno)
        {
            return SendSMS(recNum, SMS_132996357, JsonConvert.SerializeObject(new { username = username, startTime = starttime, endTime = endtime, shopname = shopname, numOfPeople = numOfPeople, houseTitle = housetitle, housecode = housecode, phoneNo = phoneno }));
        }
    }
}